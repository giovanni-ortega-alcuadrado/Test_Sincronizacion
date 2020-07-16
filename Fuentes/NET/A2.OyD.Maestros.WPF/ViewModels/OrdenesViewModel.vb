Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: OrdenesViewModel.vb
'Generado el : 05/20/2011 14:58:45
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

Public Class OrdenesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaOrdene
    Private OrdenePorDefecto As Ordene
    Private OrdeneAnterior As Ordene
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
        End If
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.OrdenesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerOrdenePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenesPorDefecto_Completed, "Default")

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "OrdenesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerOrdenesPorDefecto_Completed(ByVal lo As LoadOperation(Of Ordene))
        If Not lo.HasError Then
            OrdenePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Ordene por defecto",
                                             Me.ToString(), "TerminoTraerOrdenePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerOrdenes(ByVal lo As LoadOperation(Of Ordene))
        If Not lo.HasError Then
            ListaOrdenes = dcProxy.Ordenes
            If dcProxy.Ordenes.Count > 0 Then
                If lo.UserState = "insert" Then
                    OrdeneSelected = ListaOrdenes.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    'MessageBox.Show("No se encontró ningún registro")
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes",
                                             Me.ToString(), "TerminoTraerOrdene", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres



#End Region

#Region "Propiedades"

    Private _ListaOrdenes As EntitySet(Of Ordene)
    Public Property ListaOrdenes() As EntitySet(Of Ordene)
        Get
            Return _ListaOrdenes
        End Get
        Set(ByVal value As EntitySet(Of Ordene))
            _ListaOrdenes = value

            MyBase.CambioItem("ListaOrdenes")
            MyBase.CambioItem("ListaOrdenesPaged")
            If Not IsNothing(value) Then
                If IsNothing(OrdeneAnterior) Then
                    OrdeneSelected = _ListaOrdenes.FirstOrDefault
                Else
                    OrdeneSelected = OrdeneAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaOrdenesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaOrdenes) Then
                Dim view = New PagedCollectionView(_ListaOrdenes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _OrdeneSelected As Ordene
    Public Property OrdeneSelected() As Ordene
        Get
            Return _OrdeneSelected
        End Get
        Set(ByVal value As Ordene)
            _OrdeneSelected = value
            MyBase.CambioItem("OrdeneSelected")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewOrdene As New Ordene
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewOrdene.IDComisionista = OrdenePorDefecto.IDComisionista
            NewOrdene.IdSucComisionista = OrdenePorDefecto.IdSucComisionista
            NewOrdene.Tipo = OrdenePorDefecto.Tipo
            NewOrdene.Clase = OrdenePorDefecto.Clase
            NewOrdene.ID = OrdenePorDefecto.ID
            NewOrdene.Version = OrdenePorDefecto.Version
            NewOrdene.Ordinaria = OrdenePorDefecto.Ordinaria
            NewOrdene.Objeto = OrdenePorDefecto.Objeto
            NewOrdene.Repo = OrdenePorDefecto.Repo
            NewOrdene.Renovacion = OrdenePorDefecto.Renovacion
            NewOrdene.IDComitente = OrdenePorDefecto.IDComitente
            NewOrdene.IDOrdenante = OrdenePorDefecto.IDOrdenante
            NewOrdene.ComisionPactada = OrdenePorDefecto.ComisionPactada
            NewOrdene.CondicionesNegociacion = OrdenePorDefecto.CondicionesNegociacion
            NewOrdene.TipoLimite = OrdenePorDefecto.TipoLimite
            NewOrdene.FormaPago = OrdenePorDefecto.FormaPago
            NewOrdene.Orden = OrdenePorDefecto.Orden
            NewOrdene.VigenciaHasta = OrdenePorDefecto.VigenciaHasta
            NewOrdene.Instrucciones = OrdenePorDefecto.Instrucciones
            NewOrdene.Notas = OrdenePorDefecto.Notas
            NewOrdene.Estado = OrdenePorDefecto.Estado
            NewOrdene.Estado = OrdenePorDefecto.Estado
            NewOrdene.Sistema = OrdenePorDefecto.Sistema
            NewOrdene.UBICACIONTITULO = OrdenePorDefecto.UBICACIONTITULO
            NewOrdene.TipoInversion = OrdenePorDefecto.TipoInversion
            NewOrdene.Actualizacion = OrdenePorDefecto.Actualizacion
            NewOrdene.Usuario = Program.Usuario
            NewOrdene.IDPreliquidacion = OrdenePorDefecto.IDPreliquidacion
            NewOrdene.IDProducto = OrdenePorDefecto.IDProducto
            NewOrdene.CostoAdicionalesOrden = OrdenePorDefecto.CostoAdicionalesOrden
            NewOrdene.IdBolsa = OrdenePorDefecto.IdBolsa
            NewOrdene.UsuarioIngreso = OrdenePorDefecto.UsuarioIngreso
            NewOrdene.NegocioEspecial = OrdenePorDefecto.NegocioEspecial
            NewOrdene.Eca = OrdenePorDefecto.Eca
            NewOrdene.OrdenEscrito = OrdenePorDefecto.OrdenEscrito
            NewOrdene.ConsecutivoSwap = OrdenePorDefecto.ConsecutivoSwap
            NewOrdene.Ejecucion = OrdenePorDefecto.Ejecucion
            NewOrdene.Duracion = OrdenePorDefecto.Duracion
            NewOrdene.CantidadMinima = OrdenePorDefecto.CantidadMinima
            NewOrdene.PrecioStop = OrdenePorDefecto.PrecioStop
            NewOrdene.CantidadVisible = OrdenePorDefecto.CantidadVisible
            NewOrdene.HoraVigencia = OrdenePorDefecto.HoraVigencia
            NewOrdene.EstadoLEO = OrdenePorDefecto.EstadoLEO
            NewOrdene.UsuarioOperador = OrdenePorDefecto.UsuarioOperador
            NewOrdene.CanalRecepcion = OrdenePorDefecto.CanalRecepcion
            NewOrdene.MedioVerificable = OrdenePorDefecto.MedioVerificable
            NewOrdene.FechaHoraRecepcion = OrdenePorDefecto.FechaHoraRecepcion
            NewOrdene.SitioIngreso = OrdenePorDefecto.SitioIngreso
            NewOrdene.Seteada = OrdenePorDefecto.Seteada
            NewOrdene.Folio = OrdenePorDefecto.Folio
            NewOrdene.TipoOrdenPreOrdenes = OrdenePorDefecto.TipoOrdenPreOrdenes
            NewOrdene.NroOrdenPreOrdenes = OrdenePorDefecto.NroOrdenPreOrdenes
            NewOrdene.Impresion = OrdenePorDefecto.Impresion
            NewOrdene.Impresiones = OrdenePorDefecto.Impresiones
            NewOrdene.PreordenDetalle = OrdenePorDefecto.PreordenDetalle
            NewOrdene.EstadoOrdenBus = OrdenePorDefecto.EstadoOrdenBus
            NewOrdene.IDOrdenes = OrdenePorDefecto.IDOrdenes
            NewOrdene.IpOrigen = OrdenePorDefecto.IpOrigen
            OrdeneAnterior = OrdeneSelected
            OrdeneSelected = NewOrdene
            MyBase.CambioItem("Ordenes")
            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.Ordenes.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.OrdenesFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, Nothing)
            Else
                dcProxy.Load(dcProxy.OrdenesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.ID <> 0 Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Ordenes.Clear()
                OrdeneAnterior = Nothing
                IsBusy = True
                DescripcionFiltroVM = " ID = " & cb.ID.ToString()
                dcProxy.Load(dcProxy.OrdenesConsultarQuery(cb.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaOrdene
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
                Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            Dim origen = "update"
            ErrorForma = ""
            OrdeneAnterior = OrdeneSelected
            If Not ListaOrdenes.Contains(OrdeneSelected) Then
                origen = "insert"
                ListaOrdenes.Add(OrdeneSelected)
            End If
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As OpenRiaServices.DomainServices.Client.SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_OrdeneSelected) Then
            Editando = True
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_OrdeneSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _OrdeneSelected.EntityState = EntityState.Detached Then
                    OrdeneSelected = OrdeneAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_OrdeneSelected) Then
                dcProxy.Ordenes.Remove(_OrdeneSelected)
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaOrdene

    <Display(Name:="ID", Description:="ID")>
    Public Property ID As Integer
End Class




