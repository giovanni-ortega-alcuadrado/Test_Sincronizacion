Imports Telerik.Windows.Controls
'Codigo Desarrollado por: Santiago Alexander Vergara Orrego
'Archivo: Public Class ChequesSinEntregarViewModel.vb
'Propiedad de Alcuadrado S.A. 2013

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports System.Threading

Public Class CamposObligatoriosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim objProxy As UtilidadesDomainContext
    Public Const NOMBRE_TABLA As String = "tblCheques"
    Public Const NOMBRE_CAMPO_CONDICIONANTE As String = "strFormaPagoRC"

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            objProxy = New UtilidadesDomainContext()
        Else
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
        End If
        objProxy.Load(objProxy.cargarCombosEspecificosQuery("Tesoreria_ComprobantesEgreso", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombos, "Tesoreria_ComprobantesEgreso")
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "CamposObligatoriosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#Region "Resultados Asincrónicos"


    ''' <summary>
    ''' Proceso donde se recibe el resultado de la consulta de las formas de pago disponibles en el aplicativo
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SV20130522</remarks>
    Private Sub TerminoCargarCombos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then

                Dim ListaFormaPago As List(Of OYDUtilidades.ItemCombo)
                ListaFormaPago = objProxy.ItemCombos.ToList()
                Dim objListaCamposObligatoriosSeleccionados = New List(Of CamposObligatoriosSeleccionados)

                If Not IsNothing(ListaFormaPago) Then
                    For Each objFormaPago In (From c In ListaFormaPago Where c.Categoria = "FormaPagoCE" Select c)

                        Dim objCampo As New CamposObligatoriosSeleccionados
                        objCampo.FormaPago = objFormaPago.ID
                        objCampo.BancoConsignacionObligatorio = False
                        objCampo.BancoGiradorObligatorio = False
                        objCampo.ChequeNroObligatorio = False
                        objCampo.FechaConsignacionObligatorio = False
                        objCampo.ObservacionesObligatorio = False
                        objCampo.TipoProductoObligatorio = False
                        objCampo.RegistroModificado = False
                        objListaCamposObligatoriosSeleccionados.Add(objCampo)
                    Next

                    ListaCamposObligatoriosSeleccionados = objListaCamposObligatoriosSeleccionados

                    CampoObligatoriosSeleccionado = ListaCamposObligatoriosSeleccionados.FirstOrDefault
                    intCantidadRegistros = ListaCamposObligatoriosSeleccionados.Count
                End If

                IsBusy = False
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de combos", _
                                                 Me.ToString(), "TerminoCargarCombos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de combos", _
                                             Me.ToString(), "TerminoCargarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo donde se recibe el resultado de la consulta de los campos que son obligatorios para una forma de pago especifica
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SV20130522</remarks>
    Private Sub TerminoTraerCamposObligatorios(ByVal lo As LoadOperation(Of OYDUtilidades.CamposObligatorios))
        If Not lo.HasError Then
            _ListaCamposObligatorio = objProxy.CamposObligatorios

            _CampoObligatoriosSeleccionado.BancoConsignacionObligatorio = False
            _CampoObligatoriosSeleccionado.BancoGiradorObligatorio = False
            _CampoObligatoriosSeleccionado.ChequeNroObligatorio = False
            _CampoObligatoriosSeleccionado.FechaConsignacionObligatorio = False
            _CampoObligatoriosSeleccionado.ObservacionesObligatorio = False
            _CampoObligatoriosSeleccionado.TipoProductoObligatorio = False

            For Each obj In ListaCamposObligatorio

                Select Case obj.NombreCampoObligado

                    Case "strBancoGirador"
                        _CampoObligatoriosSeleccionado.BancoGiradorObligatorio = True

                    Case "lngNumCheque"
                        _CampoObligatoriosSeleccionado.ChequeNroObligatorio = True

                    Case "lngBancoConsignacion"
                        _CampoObligatoriosSeleccionado.BancoConsignacionObligatorio = True

                    Case "dtmConsignacion"
                        _CampoObligatoriosSeleccionado.FechaConsignacionObligatorio = True

                    Case "strComentario"
                        _CampoObligatoriosSeleccionado.ObservacionesObligatorio = True

                    Case "lngidproducto"
                        _CampoObligatoriosSeleccionado.TipoProductoObligatorio = True

                End Select
            Next
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de campos obligatorios", _
                                             Me.ToString(), "TerminoTraerCamposObligatorios", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

#End Region


#Region "Propiedades"

    Private _CampoObligatoriosSeleccionado As CamposObligatoriosSeleccionados
    Public Property CampoObligatoriosSeleccionado As CamposObligatoriosSeleccionados
        Get
            Return _CampoObligatoriosSeleccionado
        End Get
        Set(ByVal value As CamposObligatoriosSeleccionados)

            _CampoObligatoriosSeleccionado = value

            If Not IsNothing(_CampoObligatoriosSeleccionado) Then
                ConsultarCamposObligatorios()
            End If

            MyBase.CambioItem("CampoObligatoriosSeleccionado")
        End Set
    End Property

    Private _intCantidadRegistros As Integer
    Public Property intCantidadRegistros() As Integer
        Get
            Return _intCantidadRegistros
        End Get
        Set(ByVal value As Integer)
            _intCantidadRegistros = value
            MyBase.CambioItem("intCantidadRegistros")
        End Set
    End Property

    Private _CampoObligatorioAnterior As New CamposObligatoriosSeleccionados

    Private _ListaCamposObligatoriosSeleccionados As List(Of CamposObligatoriosSeleccionados)
    Public Property ListaCamposObligatoriosSeleccionados() As List(Of CamposObligatoriosSeleccionados)
        Get
            Return _ListaCamposObligatoriosSeleccionados
        End Get
        Set(ByVal value As List(Of CamposObligatoriosSeleccionados))
            _ListaCamposObligatoriosSeleccionados = value

            MyBase.CambioItem("ListaCamposObligatoriosSeleccionados")
            MyBase.CambioItem("ListaCamposObligatoriosSeleccionadosPaged")
        End Set
    End Property

    Public ReadOnly Property ListaCamposObligatoriosSeleccionadosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCamposObligatoriosSeleccionados) Then
                Dim view = New PagedCollectionView(_ListaCamposObligatoriosSeleccionados)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ListaCamposObligatorio As EntitySet(Of OYDUtilidades.CamposObligatorios)
    Public Property ListaCamposObligatorio() As EntitySet(Of OYDUtilidades.CamposObligatorios)
        Get
            Return _ListaCamposObligatorio
        End Get
        Set(ByVal value As EntitySet(Of OYDUtilidades.CamposObligatorios))
            _ListaCamposObligatorio = value
        End Set
    End Property

    Private _Modificando As boolean
    Public Property Modificando() As Boolean
        Get
            Return _Modificando
        End Get
        Set(ByVal value As Boolean)
            _Modificando = value
            MyBase.CambioItem("Modificando")
        End Set
    End Property


#End Region

#Region "Métodos"

    ''' <summary>
    ''' Metodo que se lanza cuando el usuario hace clic en aceptar para guardar los cambios
    ''' </summary>
    ''' <remarks>SV20130522</remarks>
    Public Overrides Sub ActualizarRegistro()
        Try
            If Not IsNothing(_CampoObligatoriosSeleccionado) Then
                IsBusy = True
                EliminarRegistros()
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Elimina los registros asociados a un tipo de pago en especifico
    ''' </summary>
    ''' <remarks>SV20130522</remarks>
    Public Sub EliminarRegistros()

        If _ListaCamposObligatorio.Count >= 1 Then
            objProxy.CamposObligatorios.Remove(_ListaCamposObligatorio.FirstOrDefault)

            Program.VerificarCambiosProxyServidorUtilidades(objProxy)
            objProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
        Else
            InsertarNuevosRegistros()
        End If
    End Sub

    ''' <summary>
    ''' inserta los campos que son requeridos para un tipo de pago especifico
    ''' </summary>
    ''' <param name="strMarcador"></param>
    ''' <remarks>SV20130522</remarks>
    Public Sub InsertarNuevosRegistros(Optional strMarcador As String = "0")


        If strMarcador = "6" Then
            IsBusy = False
            A2Utilidades.Mensajes.mostrarMensaje("El registro se actualizó correctamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
            Exit Sub
        End If

        Dim objCampoInicial As New OYDUtilidades.CamposObligatorios
        With objCampoInicial
            .ID = -1
            .Usuario = Program.Usuario
            .NombreTabla = NOMBRE_TABLA
            .NombreCampoCondicionante = NOMBRE_CAMPO_CONDICIONANTE
            .ValorCampoCondicionante = _CampoObligatoriosSeleccionado.FormaPago
            .FormadePago = _CampoObligatoriosSeleccionado.FormaPago
            .Actualizacion = Date.Now
        End With

        If strMarcador = "0" Then
            If _CampoObligatoriosSeleccionado.BancoGiradorObligatorio = True Then
                Dim objCampo As New OYDUtilidades.CamposObligatorios
                objCampo = objCampoInicial
                objCampo.NombreCampoObligado = "strBancoGirador"
                _ListaCamposObligatorio.Add(objCampo)
                strMarcador = "1"
            Else
                InsertarNuevosRegistros("1")
                Exit Sub
            End If

        ElseIf strMarcador = "1" Then
            If _CampoObligatoriosSeleccionado.ChequeNroObligatorio = True Then
                Dim objCampo As New OYDUtilidades.CamposObligatorios
                objCampo = objCampoInicial
                objCampo.NombreCampoObligado = "lngNumCheque"
                _ListaCamposObligatorio.Add(objCampo)
                strMarcador = "2"
            Else
                InsertarNuevosRegistros("2")
                Exit Sub
            End If

        ElseIf strMarcador = "2" Then
            If _CampoObligatoriosSeleccionado.BancoConsignacionObligatorio = True Then
                Dim objCampo As New OYDUtilidades.CamposObligatorios
                objCampo = objCampoInicial
                objCampo.NombreCampoObligado = "lngBancoConsignacion"
                _ListaCamposObligatorio.Add(objCampo)
                strMarcador = "3"
            Else
                InsertarNuevosRegistros("3")
                Exit Sub
            End If

        ElseIf strMarcador = "3" Then
            If _CampoObligatoriosSeleccionado.FechaConsignacionObligatorio = True Then
                Dim objCampo As New OYDUtilidades.CamposObligatorios
                objCampo = objCampoInicial
                objCampo.NombreCampoObligado = "dtmConsignacion"
                _ListaCamposObligatorio.Add(objCampo)
                strMarcador = "4"
            Else
                InsertarNuevosRegistros("4")
                Exit Sub
            End If

        ElseIf strMarcador = "4" Then
            If _CampoObligatoriosSeleccionado.ObservacionesObligatorio = True Then
                Dim objCampo As New OYDUtilidades.CamposObligatorios
                objCampo = objCampoInicial
                objCampo.NombreCampoObligado = "strComentario"
                _ListaCamposObligatorio.Add(objCampo)
                strMarcador = "5"
            Else
                InsertarNuevosRegistros("5")
                Exit Sub
            End If

        ElseIf strMarcador = "5" Then
            If _CampoObligatoriosSeleccionado.TipoProductoObligatorio = True Then
                Dim objCampo As New OYDUtilidades.CamposObligatorios
                objCampo = objCampoInicial
                objCampo.NombreCampoObligado = "lngidproducto"
                _ListaCamposObligatorio.Add(objCampo)
                strMarcador = "6"
            Else
                InsertarNuevosRegistros("6")
                Exit Sub
            End If
        End If

        Program.VerificarCambiosProxyServidorUtilidades(objProxy)
        objProxy.SubmitChanges(AddressOf TerminoSubmitChanges, strMarcador)

    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If So.UserState = "BorrarRegistro" Then
                    objProxy.RejectChanges()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            Else
                If So.UserState = "BorrarRegistro" Then
                    EliminarRegistros()
                Else
                    InsertarNuevosRegistros(So.UserState)
                End If

            End If

            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_CampoObligatoriosSeleccionado) Then
            _CampoObligatorioAnterior = _CampoObligatoriosSeleccionado
            Modificando = True
            Editando = True
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_CampoObligatoriosSeleccionado) Then
                objProxy.RejectChanges()
                Modificando = False
                Editando = False
                ConsultarCamposObligatorios()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar los campos que son obligatorios para el tipo de pago seleccionado
    ''' </summary>
    ''' <remarks>SV20130522</remarks>
    Public Sub ConsultarCamposObligatorios()
        Dim strFormaPagoSelected As String = _CampoObligatoriosSeleccionado.FormaPago
        objProxy.CamposObligatorios.Clear()
        objProxy.Load(objProxy.ConsularCamposObligatoriosQuery("tblCheques", "(Todos)", "strFormaPagoRC", strFormaPagoSelected, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCamposObligatorios, "Consultar")
    End Sub

#End Region

End Class


''' <summary>
''' Clase para el manejo de los atributos en el formulario de edición de registros
''' </summary>
''' <remarks>SV20130520</remarks>
Public Class CamposObligatoriosSeleccionados
    Implements INotifyPropertyChanged
    Private _FormaPago As String
    <Display(Name:="Forma de Pago")> _
    Public Property FormaPago As String
        Get
            Return _FormaPago
        End Get
        Set(ByVal value As String)
            _FormaPago = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("_FormaPago"))
        End Set
    End Property

    Private _BancoGiradorObligatorio As Boolean
    <Display(Name:="Banco Girador")>
     Public Property BancoGiradorObligatorio() As Boolean
        Get
            Return _BancoGiradorObligatorio
        End Get
        Set(ByVal value As Boolean)
            _BancoGiradorObligatorio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("BancoGiradorObligatorio"))
        End Set
    End Property

    Private _ChequeNroObligatorio As Boolean
    <Display(Name:="Cheque Nro")>
    Public Property ChequeNroObligatorio() As Boolean
        Get
            Return _ChequeNroObligatorio
        End Get
        Set(ByVal value As Boolean)
            _ChequeNroObligatorio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ChequeNroObligatorio"))
        End Set
    End Property

    Private _BancoConsignacionObligatorio As Boolean
    <Display(Name:="Banco Consignación")>
    Public Property BancoConsignacionObligatorio() As Boolean
        Get
            Return _BancoConsignacionObligatorio
        End Get
        Set(ByVal value As Boolean)
            _BancoConsignacionObligatorio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("BancoConsignacionObligatorio"))
        End Set
    End Property

    Private _FechaConsignacionObligatorio As Boolean
    <Display(Name:="Fecha Consignación")>
    Public Property FechaConsignacionObligatorio() As Boolean
        Get
            Return _FechaConsignacionObligatorio
        End Get
        Set(ByVal value As Boolean)
            _FechaConsignacionObligatorio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaConsignacionObligatorio"))
        End Set
    End Property

    Private _ObservacionesObligatorio As Boolean
    <Display(Name:="Observaciones")>
    Public Property ObservacionesObligatorio() As Boolean
        Get
            Return _ObservacionesObligatorio
        End Get
        Set(ByVal value As Boolean)
            _ObservacionesObligatorio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ObservacionesObligatorio"))
        End Set
    End Property

    Private _TipoProductoObligatorio As Boolean
    <Display(Name:="Tipo Producto")>
    Public Property TipoProductoObligatorio() As Boolean
        Get
            Return _TipoProductoObligatorio
        End Get
        Set(ByVal value As Boolean)
            _TipoProductoObligatorio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoProductoObligatorio"))
        End Set
    End Property

    Private _RegistroModificado As Boolean
    Public Property RegistroModificado() As Boolean
        Get
            Return _RegistroModificado
        End Get
        Set(ByVal value As Boolean)
            _RegistroModificado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("RegistroModificado"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class
