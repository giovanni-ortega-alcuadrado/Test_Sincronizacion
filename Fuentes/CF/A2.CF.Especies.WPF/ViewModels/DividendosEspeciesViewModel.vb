Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: DividendosEspeciesViewModel.vb
'Generado el : 06/30/2011 10:47:25
'Propiedad de Alcuadrado S.A. 2010

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

Public Class DividendosEspeciesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private EspeciePorDefecto As Especi
    Private EspeciesDividendoPorDefecto As EspeciesDividendos
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Dim dcProxy As EspeciesCFDomainContext
    Dim dcProxy1 As EspeciesCFDomainContext
    Dim logcancelar As Boolean
    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New EspeciesCFDomainContext()
            dcProxy1 = New EspeciesCFDomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext()
        Else
            dcProxy = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
            dcProxy1 = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy1.Load(dcProxy1.TraerEspeciesDividendoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesDividendoPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "EspeciesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#Region "Propiedades"
    Private WithEvents _EspecieSelected As New cbEspeciesdividendos
    Public Property EspecieSelected As cbEspeciesdividendos
        Get
            Return _EspecieSelected
        End Get
        Set(value As cbEspeciesdividendos)
            _EspecieSelected = value
            MyBase.CambioItem("EspecieSelected")
        End Set
    End Property
    Private _InhabilitarDetalles As Boolean = True
    Public Property InhabilitarDetalles() As Boolean
        Get
            Return _InhabilitarDetalles
        End Get
        Set(ByVal value As Boolean)
            _InhabilitarDetalles = value
            MyBase.CambioItem("InhabilitarDetalles")
        End Set
    End Property
    Private _Contenboton As String = "Editar"
    Public Property Contenboton() As String
        Get
            Return _Contenboton
        End Get
        Set(ByVal value As String)
            _Contenboton = value
            MyBase.CambioItem("Contenboton")
        End Set
    End Property
    Private _Inhabilitarboton As Boolean
    Public Property Inhabilitarboton() As Boolean
        Get
            Return _Inhabilitarboton
        End Get
        Set(ByVal value As Boolean)
            _Inhabilitarboton = value
            MyBase.CambioItem("Inhabilitarboton")
        End Set
    End Property

    Private _InhabilitarTexto As Boolean = True
    Public Property InhabilitarTexto() As Boolean
        Get
            Return _InhabilitarTexto
        End Get
        Set(ByVal value As Boolean)
            _InhabilitarTexto = value
            MyBase.CambioItem("InhabilitarTexto")
        End Set
    End Property
#End Region

#Region "Metodos"

    Private Sub _EspecieSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EspecieSelected.PropertyChanged
        Try
            If e.PropertyName.Equals("IdEspecie") Then
                If Not String.IsNullOrEmpty(_EspecieSelected.IdEspecie) Then
                    'SLB20130813 Se mueve esta consulta
                    'IsBusy = True
                    'dcProxy.EspeciesDividendos.Clear()
                    'dcProxy.Load(dcProxy.EspeciesDividendosConsultarQuery(EspecieSelected.IdEspecie), AddressOf TerminoTraerEspeciesDividendos, Nothing)
                    buscarItem("especies")
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Cambiar de Propiedad", _
             Me.ToString(), "_EspecieSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub cancelar()
        EspecieSelected.IdEspecie = ""
        EspecieSelected.NombreEspecie = ""
        dcProxy.EspeciesDividendos.Clear()
        Contenboton = "Editar"
        Editando = False
        InhabilitarDetalles = True
        Inhabilitarboton = False
        InhabilitarTexto = True
        logcancelar = True
    End Sub

    Public Sub editar()
        Try
            If Contenboton = "Grabar" Then
                If Not IsNothing(ListaEspeciesDividendos) Then
                    If ListaEspeciesDividendos.Count > 0 Then
                        For Each led In ListaEspeciesDividendos
                            If led.FinPago.Date < led.InicioPago.Date Then
                                A2Utilidades.Mensajes.mostrarMensaje("El Fin Pago no Debe ser Menor que el Inicio Pago.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                            If led.FinVigencia.Date < led.InicioVigencia.Date Then
                                A2Utilidades.Mensajes.mostrarMensaje("El Fin Vigencia no Debe ser Menor que el Inicio Vigencia.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If

                            ' Se valida que la fecha de inicio pago del registro especifico no este repetida en otro de los registros
                            Dim fechaInicioPago = led.InicioPago.Date
                            Dim fechaInicioRepetida = From ld In ListaEspeciesDividendos Where ld.InicioPago.Date = fechaInicioPago
                                                      Select ld

                            If fechaInicioRepetida.Count > 1 Then
                                A2Utilidades.Mensajes.mostrarMensaje("Fecha Inicio Pago Duplicada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                            If IsNothing(led.Causacion) Or led.Causacion = 0 Then
                                A2Utilidades.Mensajes.mostrarMensaje("La Causación no puede ser cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                            If IsNothing(led.IDCtrlDividendo) Or String.IsNullOrEmpty(led.IDCtrlDividendo) Then
                                A2Utilidades.Mensajes.mostrarMensaje("La Modalidad no puede estar vacia.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                        Next
                    End If
                    Program.VerificarCambiosProxyServidor(dcProxy)
                    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Nothing)
                    IsBusy = True
                    Contenboton = "Editar"
                    Editando = False
                End If
            Else
                Contenboton = "Grabar"
                Editando = True
                InhabilitarDetalles = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "editar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Friend Sub buscarItem(ByVal pstrTipoItem As String, Optional ByVal pstrIdItem As String = "")
        Dim strIdItem As String = String.Empty
        Dim logConsultar As Boolean = False

        Try
            If Not Me.EspecieSelected Is Nothing Then
                Select Case pstrTipoItem
                    Case "especies"
                        If Not IsNothing(EspecieSelected.IdEspecie) Then
                            pstrIdItem = pstrIdItem.Trim()
                            If pstrIdItem.Equals(String.Empty) Then
                                strIdItem = Me.EspecieSelected.IdEspecie
                            Else
                                strIdItem = pstrIdItem
                            End If
                            If Not strIdItem.Equals(String.Empty) Then
                                logConsultar = True
                            End If
                            If logConsultar Then
                                IsBusy = True
                                mdcProxyUtilidad01.BuscadorEspecies.Clear()
                                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarNemotecnicoEspecificoQuery("", EspecieSelected.IdEspecie, Program.Usuario, Program.HashConexion), AddressOf buscarEspecieCompleted, pstrTipoItem)
                                'SE ENVIA EL PARAMETRO PSTRMERCADO VACIO A LA FUNCION buscarNemotecnicoEspecifico EN EL DOMAINSERVICES DE UTILIDADES
                            End If
                        End If
                    Case Else
                        logConsultar = False
                End Select
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos de la ciudad", Me.ToString(), "Buscar ciudad", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub buscarEspecieCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorEspecies))
        Dim strTipoItem As String
        Try
            IsBusy = False
            If lo.UserState Is Nothing Then
                strTipoItem = ""
            Else
                strTipoItem = lo.UserState
            End If

            'SLB20130813 Se organiza el código para que funcione como VB.6
            If lo.Entities.ToList.Count > 0 Then
                Me.EspecieSelected.NombreEspecie = lo.Entities.ToList.Item(0).Especie
                Inhabilitarboton = True
                InhabilitarTexto = False
                IsBusy = True
                dcProxy.EspeciesDividendos.Clear()
                dcProxy.Load(dcProxy.EspeciesDividendosConsultarQuery(EspecieSelected.IdEspecie, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesDividendos, Nothing)
            Else
                A2Utilidades.Mensajes.mostrarMensaje("El nemotécnico ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                _EspecieSelected.IdEspecie = String.Empty
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarOrdenanteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Asincronicos"

    Private Sub TerminoTraerEspeciesDividendos(ByVal lo As LoadOperation(Of EspeciesDividendos))
        IsBusy = False
        If Not lo.HasError Then
            ListaEspeciesDividendos = dcProxy.EspeciesDividendos
            'If ListaEspeciesDividendos.Count > 0 Then
            '    Inhabilitarboton = True
            '    InhabilitarTexto = False
            'Else
            '    If Not logcancelar Then
            '        A2Utilidades.Mensajes.mostrarMensaje("No se encontraron dividendos para el nemotécnico seleccionado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    End If
            '    logcancelar = False
            'End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EspeciesDividendos", _
                                             Me.ToString(), "TerminoTraerEspeciesDividendos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerEspeciesDividendoPorDefecto_Completed(ByVal lo As LoadOperation(Of EspeciesDividendos))
        Try
            IsBusy = False
            If Not lo.HasError Then
                EspeciesDividendoPorDefecto = lo.Entities.FirstOrDefault
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la EspeciesDividendo por defecto", _
                                                 Me.ToString(), "TerminoTraerEspeciesDividendoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la EspeciesDividendo por defecto", _
                                                 Me.ToString(), "TerminoTraerEspeciesDividendoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,") '.Split(So.Error.Message, vbCr)
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                    Exit Try
                End If
                'TODO: Pendiente garantizar que Userstate no venga vacío
                Dim strMsg As String = String.Empty
                'TODO: Pendiente garantizar que Userstate no venga vacío
                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                '                       Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If

            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "EspeciesDividendos"

    '******************************************************** EspeciesDividendos 
    Private _ListaEspeciesDividendos As EntitySet(Of EspeciesDividendos)
    Public Property ListaEspeciesDividendos() As EntitySet(Of EspeciesDividendos)
        Get
            Return _ListaEspeciesDividendos
        End Get
        Set(ByVal value As EntitySet(Of EspeciesDividendos))
            _ListaEspeciesDividendos = value
            MyBase.CambioItem("ListaEspeciesDividendos")
            MyBase.CambioItem("ListaEspeciesDividendosPaged")
        End Set
    End Property

    Public ReadOnly Property ListaEspeciesDividendosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEspeciesDividendos) Then
                Dim view = New PagedCollectionView(_ListaEspeciesDividendos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ListaEspeciesDividendosAnterior As List(Of EspeciesDividendos)
    Public Property ListaEspeciesDividendosAnterior() As List(Of EspeciesDividendos)
        Get
            Return _ListaEspeciesDividendosAnterior
        End Get
        Set(ByVal value As List(Of EspeciesDividendos))
            _ListaEspeciesDividendosAnterior = value
            MyBase.CambioItem("ListaEspeciesDividendosAnterior")
        End Set
    End Property

    Private _EspeciesDividendosSelected As EspeciesDividendos
    Public Property EspeciesDividendosSelected() As EspeciesDividendos
        Get
            Return _EspeciesDividendosSelected
        End Get
        Set(ByVal value As EspeciesDividendos)

            If Not value Is Nothing Then
                _EspeciesDividendosSelected = value
                MyBase.CambioItem("EspeciesDividendosSelected")
            End If
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Este es el Metodo de Nuevo de todos los detalles de Especies 
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub NuevoRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmEspeciesDividendos"
                Dim NewEspeciesDividendos As New EspeciesDividendos
                NewEspeciesDividendos.IDEspecie = EspecieSelected.IdEspecie
                NewEspeciesDividendos.InicioVigencia = EspeciesDividendoPorDefecto.InicioVigencia
                NewEspeciesDividendos.FinVigencia = EspeciesDividendoPorDefecto.FinVigencia
                NewEspeciesDividendos.Causacion = EspeciesDividendoPorDefecto.Causacion
                NewEspeciesDividendos.InicioPago = EspeciesDividendoPorDefecto.InicioPago
                NewEspeciesDividendos.FinPago = EspeciesDividendoPorDefecto.FinPago
                NewEspeciesDividendos.CantidadAcciones = EspeciesDividendoPorDefecto.CantidadAcciones
                NewEspeciesDividendos.CantidadPesos = EspeciesDividendoPorDefecto.CantidadPesos
                NewEspeciesDividendos.IDCtrlDividendo = EspeciesDividendoPorDefecto.IDCtrlDividendo
                If Not IsNothing(EspeciesDividendosSelected) Then
                    If ListaEspeciesDividendos.Count < 1 Then
                        NewEspeciesDividendos.IDDividendos = EspeciesDividendoPorDefecto.IDDividendos
                    Else
                        NewEspeciesDividendos.IDDividendos = ListaEspeciesDividendos.Last.IDDividendos + 1
                    End If
                End If
                NewEspeciesDividendos.Usuario = Program.Usuario
                ListaEspeciesDividendos.Add(NewEspeciesDividendos)
                EspeciesDividendosSelected = NewEspeciesDividendos
                MyBase.CambioItem("EspeciesDividendosSelected")
                MyBase.CambioItem("ListaEspeciesDividendos")
                MyBase.CambioItem("Editando")
        End Select
    End Sub
    Public Overrides Sub BorrarRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmEspeciesDividendos"
                If Not IsNothing(ListaEspeciesDividendos) Then
                    If Not IsNothing(EspeciesDividendosSelected) Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(EspeciesDividendosSelected, ListaEspeciesDividendos.ToList)
                        ListaEspeciesDividendos.Remove(EspeciesDividendosSelected)
                        EspeciesDividendosSelected = ListaEspeciesDividendos.LastOrDefault
                        If ListaEspeciesDividendos.Count > 0 Then
                            Program.PosicionarItemLista(EspeciesDividendosSelected, ListaEspeciesDividendos.ToList, intRegistroPosicionar)
                        End If
                        MyBase.CambioItem("ListaEspeciesDividendosSelected")
                        MyBase.CambioItem("ListaEspeciesDividendos")
                    End If
                End If
        End Select
    End Sub

End Class

Public Class cbEspeciesdividendos
    Implements INotifyPropertyChanged

    Private _IdEspecie As String
    <Display(Name:=" ")> _
    Public Property IdEspecie As String
        Get
            Return _IdEspecie
        End Get
        Set(ByVal value As String)
            _IdEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdEspecie"))
        End Set
    End Property

    Private _NombreEspecie As String
    <Display(Name:=" ")> _
    Public Property NombreEspecie As String
        Get
            Return _NombreEspecie
        End Get
        Set(ByVal value As String)
            _NombreEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreEspecie"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class





