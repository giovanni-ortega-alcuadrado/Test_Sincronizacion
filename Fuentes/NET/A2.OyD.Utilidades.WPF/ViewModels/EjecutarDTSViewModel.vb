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
Imports System.Threading
Imports A2ComunesControl

Public Class EjecutarDTSViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxy As UtilidadesDomainContext
    Dim dcProxy1 As UtilidadesDomainContext
    Private DTAnterior As OYDUtilidades.DT

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New UtilidadesDomainContext()
            dcProxy1 = New UtilidadesDomainContext()
        Else
            dcProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            dcProxy1 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If

        'SLB20140528 Manejo del TimeOut a traves de un parametro de la aplicación.
        Dim TimeOutMinutos As Byte = 15
        If Not String.IsNullOrEmpty(Program.Consultar_TIME_OUT_DTS_EN_MINUTOS) Then
            TimeOutMinutos = CInt(Program.Consultar_TIME_OUT_DTS_EN_MINUTOS)
        End If

        DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, TimeOutMinutos, 0)

        'RABP20200123: Se crear función para poder consultar correctamente el listado de las DTS, ya que se perdia la información para mostrarla en el grid
        ' Y en el View se cambio el  ItemsSource="{Binding ListaDTSPage} por  ItemsSource="{Binding ListaDTS}""
        ConsultarDTS()

        'Try
        '    If Not Program.IsDesignMode() Then
        '        IsBusy = True
        '        dcProxy.Load(dcProxy.DTSFiltrarQuery(Program.Usuario, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDTS, "FiltroInicial")

        '    End If
        'Catch ex As Exception
        '    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
        '                         Me.ToString(), "CuentasDecevalPorAgrupadorViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        'End Try
    End Sub

#Region "Resultados Asincrónicos"

    'RABP20200123: Se crear función para poder consultar correctamente el listado de las DTS, ya que se perdia la información para mostrarla en el grid
    ' Y en el View se cambio el  ItemsSource="{Binding ListaDTSPage} por  ItemsSource="{Binding ListaDTS}""
    Public Async Function ConsultarDTS() As Task
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                Dim obj As LoadOperation(Of A2.OyD.OYDServer.RIA.Web.OYDUtilidades.DT)
                'Await dcProxy.Load(dcProxy.DTSFiltrarQuery(Program.Usuario, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDTS, "FiltroInicial")
                obj = Await dcProxy.Load(dcProxy.DTSFiltrarQuery(Program.Usuario, Program.Usuario, A2Utilidades.Program.HashConexion), AddressOf TerminoTraerDTS, "FiltroInicial").AsTask

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "CuentasDecevalPorAgrupadorViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Function

    Private Sub TerminoTraerDTS(ByVal lo As LoadOperation(Of A2.OyD.OYDServer.RIA.Web.OYDUtilidades.DT))
        If Not lo.HasError Then
            ListaDTS = dcProxy.DTs
            If dcProxy.DTs.Count > 0 Then
                If lo.UserState = "insert" Then
                    DTSelected = ListaDTS.Last
                End If
            Else
                If lo.UserState = "Busqueda" Then
                    MessageBox.Show("No se encontro ningún registro")
                    MyBase.Buscar()
                    MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de DTS", _
                                             Me.ToString(), "TerminoTraerDT", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres



#End Region
#Region "Propiedades"

    Private _ListaDTS As EntitySet(Of A2.OyD.OYDServer.RIA.Web.OYDUtilidades.DT)
    Public Property ListaDTS() As EntitySet(Of A2.OyD.OYDServer.RIA.Web.OYDUtilidades.DT)
        Get
            Return _ListaDTS
        End Get
        Set(ByVal value As EntitySet(Of A2.OyD.OYDServer.RIA.Web.OYDUtilidades.DT))
            _ListaDTS = value

            MyBase.CambioItem("ListaDTS")
            MyBase.CambioItem("ListaDTSPaged")
            If Not IsNothing(value) Then
                If IsNothing(DTAnterior) Then
                    DTSelected = _ListaDTS.FirstOrDefault
                Else
                    DTSelected = DTAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaDTSPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDTS) Then
                Dim view = New PagedCollectionView(_ListaDTS)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _DTSelected As OYDUtilidades.DT
    Public Property DTSelected() As OYDUtilidades.DT
        Get
            Return _DTSelected
        End Get
        Set(ByVal value As OYDUtilidades.DT)
            _DTSelected = value
            MyBase.CambioItem("DTSelected")
        End Set
    End Property

#End Region

#Region "Métodos"
    'Public Overrides Sub NuevoRegistro()
    '    Try
    '        Dim NewDT As New DT
    '        'TODO: Verificar cuales son los campos que deben inicializarse
    '        NewDT.IDComisionista = DTPorDefecto.IDComisionista
    '        NewDT.IDSucComisionista = DTPorDefecto.IDSucComisionista
    '        NewDT.IDDTS = DTPorDefecto.IDDTS
    '        NewDT.Descripcion = DTPorDefecto.Descripcion
    '        NewDT.NomSP = DTPorDefecto.NomSP
    '        NewDT.Actualizacion = DTPorDefecto.Actualizacion
    '        NewDT.Usuario = Program.Usuario
    '        NewDT.IDDTS = DTPorDefecto.IDDTS
    '        DTAnterior = DTSelected
    '        DTSelected = NewDT
    '        MyBase.CambioItem("DTS")
    '        Editando = True
    '        MyBase.CambioItem("Editando")
    '    Catch ex As Exception
    '        IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
    '                                                     Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

    'Public Overrides Sub Filtrar()
    '    Try
    '        dcProxy.DTS.Clear()
    '        IsBusy = True
    '        If FiltroVM.Length > 0 Then
    '            dcProxy.Load(dcProxy.DTSFiltrarQuery(FiltroVM,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDTS, Nothing)
    '        Else
    '            dcProxy.Load(dcProxy.DTSFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDTS, Nothing)
    '        End If
    '    Catch ex As Exception
    '        IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
    '                                         Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub


    'Public Overrides Sub ConfirmarBuscar()
    '    Try
    '        If cb.IDDTS <> 0 Then 'Validar que ingresó algo en los campos de búsqueda
    '            ErrorForma = ""
    '            dcProxy.DTS.Clear()
    '            DTAnterior = Nothing
    '            IsBusy = True
    '            DescripcionFiltroVM = " IDDTS = " & cb.IDDTS.ToString()
    '            dcProxy.Load(dcProxy.DTSConsultarQuery(cb.IDDTS,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDTS, "Busqueda")
    '            MyBase.ConfirmarBuscar()
    '            cb = New CamposBusquedaDT
    '            CambioItem("cb")
    '        End If
    '    Catch ex As Exception
    '        IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
    '         Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

    Public Sub EjecutarDTS()
        Try
            If IsNothing(DTSelected) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe escoger una DTS.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            Dim origen = "update"
            ErrorForma = ""
            DTAnterior = DTSelected
            DTSelected.Usuario = Program.Usuario
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                dcProxy.RejectChanges()
                So.MarkErrorAsHandled()
                Exit Try
            Else
                If So.UserState = "update" Then
                    MyBase.QuitarFiltroDespuesGuardar()
                    IsBusy = True
                    DTAnterior = Nothing
                    dcProxy.DTs.Clear()
                    dcProxy.Load(dcProxy.DTSFiltrarQuery(Program.Usuario, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDTS, Nothing) ' Recarga la lista para que carguen los include
                End If
                Thread.Sleep(3000)
                A2Utilidades.Mensajes.mostrarMensaje("La DTS fue satisfactoriamente ejecutada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub



#End Region
End Class
