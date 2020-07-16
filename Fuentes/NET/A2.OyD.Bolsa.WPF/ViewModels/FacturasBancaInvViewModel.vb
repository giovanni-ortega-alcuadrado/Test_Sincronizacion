Imports Telerik.Windows.Controls
'Codigo generado : Sebastian Londoño Benitez
'Plantilla: ViewModelTemplate2010
'Archivo: FacturasBancaInvViewModel.vb
'Generado el : 02/24/2012 07:45:33
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
Imports A2.OyD.OYDServer.RIA.Web.OyDBolsa
Imports Microsoft.VisualBasic.CompilerServices
Imports A2Utilidades.Mensajes

Public Class FacturasBancaInvViewModel

    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaFacturasBancaIn
    Private FacturasBancaInPorDefecto As FacturasBancaIn
    Private FacturasBancaInAnterior As FacturasBancaIn
    Private DetalleFacturasBancaInPorDefecto As DetalleFacturasBancaIn
    Private DetalleFacturasBancaInAnterior As DetalleFacturasBancaIn
    Public Property TotalFactu As New TotalFacturaBancaInversion
    Dim dcProxy As BolsaDomainContext
    Dim dcProxy1 As BolsaDomainContext
    Dim dcProxy2 As BolsaDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim FechaCierre As DateTime
    Dim dblCampoTablaIVA As Decimal = 0
    Dim dblCampoTablaRetencion As Decimal = 0
    Dim EstadoPropiedad As Boolean = False

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New BolsaDomainContext()
            dcProxy1 = New BolsaDomainContext()
            dcProxy2 = New BolsaDomainContext()
            objProxy = New UtilidadesDomainContext()
        Else
            dcProxy = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy2 = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
        End If
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.FacturasBancaInvFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFacturasBancaInv, "")
                dcProxy1.Load(dcProxy1.TraerFacturasBancaInPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFacturasBancaInvPorDefecto_Completed, "Default")
                dcProxy2.Load(dcProxy2.TraerDetalleFacturasBancaInPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDetalleFacturasBancaInvPorDefecto_Completed, "Default")
                dcProxy1.ConsultarCamposTablaFacBancaInv(Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarCamposTabla, "")
                objProxy.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "FacturasBancaInvViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerFacturasBancaInvPorDefecto_Completed(ByVal lo As LoadOperation(Of FacturasBancaIn))
        If Not lo.HasError Then
            FacturasBancaInPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la FacturasBancaIn por defecto", _
                                             Me.ToString(), "TerminoTraerFacturasBancaInPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerFacturasBancaInv(ByVal lo As LoadOperation(Of FacturasBancaIn))
        If Not lo.HasError Then
            ListaFacturasBancaInv = dcProxy.FacturasBancaIns
            If dcProxy.FacturasBancaIns.Count > 0 Then
                'If lo.UserState = "insert" Then
                '    FacturasBancaInSelected = ListaFacturasBancaInv.Last
                'End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de FacturasBancaInv", _
                                             Me.ToString(), "TerminoTraerFacturasBancaIn", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoTraerDetalleFacturasBancaInvPorDefecto_Completed(ByVal lo As LoadOperation(Of DetalleFacturasBancaIn))
        If Not lo.HasError Then
            DetalleFacturasBancaInPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Detalle Facturas Banca Inv por defecto", _
                                             Me.ToString(), "TerminoTraerDetalleFacturasBancaInvPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerDetalleFacturasBancaInv(ByVal lo As LoadOperation(Of DetalleFacturasBancaIn))
        If Not lo.HasError Then
            ListaDetalleFacturasBancaInv = dcProxy1.DetalleFacturasBancaIns.ToList
            TotalFactu.TotalFactura = 0
            If ListaDetalleFacturasBancaInv.Count > 0 Then
                TotalDetalleFactura(False)
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de DetalleFacturasBancaInv", _
                                             Me.ToString(), "TerminoTraerDetalleFacturasBancaInv", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    'Tablas padres

    Private Sub consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            FechaCierre = obj.Value
        End If
    End Sub

    ''' <summary>
    ''' Se anula correctamente la factura, se cambia el estado de la factura, esto para no recargar la lista. 
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20121005</remarks>
    Private Sub TerminoAnularFactura(ByVal lo As InvokeOperation(Of Boolean))
        If Not lo.HasError Then
            FacturasBancaInSelected.Estado = "A"
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al anular la Factura", _
                                             Me.ToString(), "TerminoAnularFactura", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    ''' <summary>
    ''' Se obtiene el valor de los campos tabla IVA y Retención
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20121022</remarks>
    Private Sub TerminoConsultarCamposTabla(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            Dim Resultado() = Split(lo.Value, ",")
            dblCampoTablaIVA = Resultado(0)
            dblCampoTablaRetencion = Resultado(1)
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al consultar los Campos Tabla", _
                                             Me.ToString(), "TerminoConsultarCamposTabla", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    ''' <summary>
    ''' Se obtiene el resultado de buscar el cliente cuando se digita desde el control
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20121210</remarks>
    Private Sub buscarComitenteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If lo.Entities.ToList.Count > 0 Then
                If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                    Me.ComitenteSeleccionado(Nothing)
                    A2Utilidades.Mensajes.mostrarMensaje("El comitente ingresado se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    Me.ComitenteSeleccionado(lo.Entities.ToList.Item(0))
                End If

            Else
                Me.ComitenteSeleccionado(Nothing)
                A2Utilidades.Mensajes.mostrarMensaje("El comitente ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaFacturasBancaInv As EntitySet(Of FacturasBancaIn)
    Public Property ListaFacturasBancaInv() As EntitySet(Of FacturasBancaIn)
        Get
            Return _ListaFacturasBancaInv
        End Get
        Set(ByVal value As EntitySet(Of FacturasBancaIn))
            _ListaFacturasBancaInv = value
            'If Not IsNothing(value) Then
            '    FacturasBancaInSelected = _ListaFacturasBancaInv.FirstOrDefault
            'If IsNothing(FacturasBancaInAnterior) Then
            '    FacturasBancaInSelected = _ListaFacturasBancaInv.FirstOrDefault
            'Else
            '    FacturasBancaInSelected = FacturasBancaInAnterior
            'End If
            'End If
            MyBase.CambioItem("ListaFacturasBancaInv")
            MyBase.CambioItem("ListaFacturasBancaInvPaged")
        End Set
    End Property

    Public ReadOnly Property ListaFacturasBancaInvPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaFacturasBancaInv) Then
                Dim view = New PagedCollectionView(_ListaFacturasBancaInv)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _FacturasBancaInSelected As FacturasBancaIn
    'Private _FacturasBancaInSelected As FacturasBancaIn
    Public Property FacturasBancaInSelected() As FacturasBancaIn
        Get
            Return _FacturasBancaInSelected
        End Get
        Set(ByVal value As FacturasBancaIn)
            'If Not IsNothing(_FacturasBancaInSelected) AndAlso _FacturasBancaInSelected.Equals(value) Then
            '    Exit Property
            'End If
            _FacturasBancaInSelected = value
            'dcProxy1.RejectChanges()
            If Not IsNothing(_FacturasBancaInSelected) Then
                If Not IsNothing(dcProxy1.DetalleFacturasBancaIns) Then
                    dcProxy1.DetalleFacturasBancaIns.Clear()
                End If
                dcProxy1.Load(dcProxy1.DetalleFacturasBancaInvConsultarQuery(_FacturasBancaInSelected.AnoDocumento, _FacturasBancaInSelected.Prefijo,
                                                                             _FacturasBancaInSelected.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDetalleFacturasBancaInv, Nothing)
            Else
                Dim objListaDetalleBancaInv As New List(Of DetalleFacturasBancaIn)
                ListaDetalleFacturasBancaInv = objListaDetalleBancaInv
            End If
            MyBase.CambioItem("FacturasBancaInSelected")
        End Set
    End Property

    Private _EditarRetencion As Boolean = False
    Public Property EditarRetencion As Boolean
        Get
            Return _EditarRetencion
        End Get
        Set(ByVal value As Boolean)
            _EditarRetencion = value
            MyBase.CambioItem("EditarRetencion")
        End Set
    End Property

    Private _EditarIva As Boolean = False
    Public Property EditarIva As Boolean
        Get
            Return _EditarIva
        End Get
        Set(ByVal value As Boolean)
            _EditarIva = value
            MyBase.CambioItem("EditarIva")
        End Set
    End Property


    Private Sub _FacturasBancaInSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _FacturasBancaInSelected.PropertyChanged
        Select Case e.PropertyName
            Case "EsExenta"
                If Not IsNothing(FacturasBancaInSelected) And Editando Then
                    Select Case FacturasBancaInSelected.EsExenta
                        Case "A"
                            EditarRetencion = True
                            EditarIva = True
                        Case "N"
                            EditarRetencion = False
                            EditarIva = False
                            FacturasBancaInSelected.Retencion = 0
                            FacturasBancaInSelected.IVA = 0
                        Case "eta"
                            EditarRetencion = True
                            EditarIva = False
                            FacturasBancaInSelected.IVA = 0
                        Case "eda"
                            EditarRetencion = False
                            EditarIva = True
                            FacturasBancaInSelected.Retencion = 0
                    End Select
                End If
                TotalDetalleFactura(True)
        End Select
    End Sub

    ''' <summary>
    ''' Pone en modo de solo lectura los detalles de la orden.
    ''' </summary>
    ''' <remarks></remarks>
    Private _EditandoDetalle As Boolean = True
    Public Property EditandoDetalle() As Boolean
        Get
            Return _EditandoDetalle
        End Get
        Set(ByVal value As Boolean)
            _EditandoDetalle = value
            MyBase.CambioItem("EditandoDetalle")
        End Set
    End Property

    Public ReadOnly Property FechaActual As DateTime
        Get
            Return Now.Date
        End Get
    End Property

#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Dim NewFacturasBancaIn As New FacturasBancaIn
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewFacturasBancaIn.IDComisionista = FacturasBancaInPorDefecto.IDComisionista
            NewFacturasBancaIn.IDSucComisionista = FacturasBancaInPorDefecto.IDSucComisionista
            NewFacturasBancaIn.AnoDocumento = FacturasBancaInPorDefecto.AnoDocumento
            NewFacturasBancaIn.Prefijo = FacturasBancaInPorDefecto.Prefijo
            NewFacturasBancaIn.ID = FacturasBancaInPorDefecto.ID
            NewFacturasBancaIn.Documento = FacturasBancaInPorDefecto.Documento
            NewFacturasBancaIn.IDComitente = FacturasBancaInPorDefecto.IDComitente
            NewFacturasBancaIn.TipoIdentificacion = FacturasBancaInPorDefecto.TipoIdentificacion
            NewFacturasBancaIn.NroIdentificacion = FacturasBancaInPorDefecto.NroIdentificacion
            NewFacturasBancaIn.NroIdentificacion = Nothing
            NewFacturasBancaIn.Nombre = FacturasBancaInPorDefecto.Nombre
            NewFacturasBancaIn.Direccion = FacturasBancaInPorDefecto.Direccion
            NewFacturasBancaIn.Telefono = FacturasBancaInPorDefecto.Telefono
            NewFacturasBancaIn.Estado = FacturasBancaInPorDefecto.Estado
            NewFacturasBancaIn.Impresiones = FacturasBancaInPorDefecto.Impresiones
            NewFacturasBancaIn.IVA = FacturasBancaInPorDefecto.IVA
            NewFacturasBancaIn.Retencion = FacturasBancaInPorDefecto.Retencion
            'NewFacturasBancaIn.Estado = FacturasBancaInPorDefecto.Estado
            NewFacturasBancaIn.Actualizacion = FacturasBancaInPorDefecto.Actualizacion
            NewFacturasBancaIn.Usuario = Program.Usuario
            NewFacturasBancaIn.IDFacturasBancaInv = FacturasBancaInPorDefecto.IDFacturasBancaInv
            NewFacturasBancaIn.Maquina = FacturasBancaInPorDefecto.Maquina
            NewFacturasBancaIn.EsExenta = "A"
            NewFacturasBancaIn.Fecha_Estado = Now()

            FacturasBancaInAnterior = FacturasBancaInSelected
            FacturasBancaInSelected = NewFacturasBancaIn

            MyBase.CambioItem("FacturasBancaInv")
            Editando = True
            EditandoDetalle = False
            EditarRetencion = True
            EditarIva = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.FacturasBancaIns.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.FacturasBancaInvFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFacturasBancaInv, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.FacturasBancaInvFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFacturasBancaInv, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Prefijo <> String.Empty Or cb.ID <> 0 Or cb.Comitente <> String.Empty Or (Not IsNothing(cb.Documento)) Or cb.NombreComitente <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.FacturasBancaIns.Clear()
                FacturasBancaInAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Prefijo = " &  cb.Prefijo.ToString() & " ID = " &  cb.ID.ToString() & " Documento = " &  cb.Documento.ToString() & " IDComitente = " &  cb.IDComitente.ToString() & " NroIdentificacion = " &  cb.NroIdentificacion.ToString() & " Nombre = " &  cb.Nombre.ToString()    'Dic202011 quitar
                dcProxy.Load(dcProxy.FacturasBancaInvConsultarQuery(cb.Prefijo, cb.ID, cb.Documento, cb.Comitente, System.Web.HttpUtility.UrlEncode(cb.NombreComitente), Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFacturasBancaInv, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaFacturasBancaIn
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            If validaFechaCierre("Actualizar") Then
                Dim strDetalleFacturas As String = String.Empty

                'TotalDetalleFactura(False)

                If FacturasBancaInSelected.Prefijo = String.Empty Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el Tipo de Documento", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                'If FacturasBancaInSelected.IDComitente = String.Empty Then
                '    A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el Cliente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '    Exit Sub
                'End If
                'If FacturasBancaInSelected.NroIdentificacion = 0 Then
                If IsNothing(_FacturasBancaInSelected.NroIdentificacion) Then
                    A2Utilidades.Mensajes.mostrarMensaje("El número de identificación es un dato requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If IsNothing(_FacturasBancaInSelected.TipoIdentificacion) Then
                    A2Utilidades.Mensajes.mostrarMensaje("El tipo de identificación es un dato requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If IsNothing(_FacturasBancaInSelected.Nombre) Then
                    A2Utilidades.Mensajes.mostrarMensaje("El nombre es un dato requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If FacturasBancaInSelected.EsExenta = String.Empty Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el Tipo de Factura", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                'If FacturasBancaInSelected.IVA <> 0 And FacturasBancaInSelected.EsExenta <> "N" Then
                '    A2Utilidades.Mensajes.mostrarMensaje("Cuando el IVA es mayor que 0, el tipo de factura debe ser 'No aplica'", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '    Exit Sub
                'End If

                Dim lstDetalleFacturas As New List(Of Object)

                strDetalleFacturas = "<?xml version=" & """1.0" & """ encoding=" & """iso-8859-1" & """ ?> <detfactura>"
                If ListaDetalleFacturasBancaInv.Count > 0 Then
                    For Each obj In ListaDetalleFacturasBancaInv

                        If obj.Descripcion = String.Empty Then
                            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la descripción del detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                        If obj.Valor = 0 Then
                            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el valor del detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If

                        'SLB20130220 Codificación para que no falle el XML con los simbolos 
                        '&amp; = &
                        '&lt; = <
                        '&gt; = >
                        '&apos; = '
                        '&quot; = "
                        'strDetalleFacturas &= "<factura strDescripcion=""" & CStr(System.Web.HttpUtility.HtmlEncode(obj.Descripcion)) & """ curValor=""" & Replace(obj.Valor, ",", ".") & """ lngAnoDocumento=""" & CStr(obj.AnoDocumento) & """ />"
                        strDetalleFacturas &= "<factura strDescripcion=""" & CStr(System.Web.HttpUtility.HtmlEncode(obj.Descripcion)) & """ curValor=""" & obj.Valor & """ lngAnoDocumento=""" & CStr(obj.AnoDocumento) & """ />"
                    Next
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar los Detalles de la Factura", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                strDetalleFacturas &= "</detfactura>"

                _FacturasBancaInSelected.DetalleFacturaXML = strDetalleFacturas

                TotalDetalleFactura(False)

                Dim origen = "update"
                ErrorForma = ""
                FacturasBancaInAnterior = FacturasBancaInSelected
                If Not ListaFacturasBancaInv.Contains(FacturasBancaInSelected) Then
                    origen = "insert"
                    ListaFacturasBancaInv.Add(FacturasBancaInSelected)
                End If
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
            End If
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
                So.MarkErrorAsHandled()

                If So.UserState = "insert" Then
                    ListaFacturasBancaInv.Remove(FacturasBancaInSelected)
                End If

                Exit Try
            End If

            If So.UserState = "insert" Then
                MyBase.QuitarFiltroDespuesGuardar()
                Filtrar()
            End If

            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        Try
            If dcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_FacturasBancaInSelected) Then
                If FacturasBancaInSelected.Estado = "P" Then
                    Editando = True
                    EditandoDetalle = False
                Else
                    MyBase.RetornarValorEdicionNavegacion()
                    Dim strMsg As String = "Anulada"
                    If FacturasBancaInSelected.Estado = "I" Then
                        strMsg = "Impresa"
                    End If
                    A2Utilidades.Mensajes.mostrarMensaje("La Factura no se puede modificar ya que se encuentra " & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar el registro.", _
                                             Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_FacturasBancaInSelected) Then
                dcProxy.RejectChanges()
                'dcProxy1.RejectChanges()
                Editando = False
                EditandoDetalle = True
                If _FacturasBancaInSelected.EntityState = EntityState.Detached Then
                    FacturasBancaInSelected = FacturasBancaInAnterior
                Else
                    If Not IsNothing(dcProxy1.DetalleFacturasBancaIns) Then
                        dcProxy1.DetalleFacturasBancaIns.Clear()
                    End If
                    dcProxy1.Load(dcProxy1.DetalleFacturasBancaInvConsultarQuery(_FacturasBancaInSelected.AnoDocumento, _FacturasBancaInSelected.Prefijo, _
                                                                                 _FacturasBancaInSelected.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDetalleFacturasBancaInv, Nothing)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_FacturasBancaInSelected) Then
                If Not IsNothing(FacturasBancaInSelected.Estado) Then
                    If FacturasBancaInSelected.Estado = "P" Then
                        A2Utilidades.Mensajes.mostrarMensaje("El documento se encuentra pendiente y no se puede anular", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    Else
                        If FacturasBancaInSelected.Estado = "A" Then
                            A2Utilidades.Mensajes.mostrarMensaje("El documento se encuentra anulado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                    End If
                End If
                'C1.Silverlight.C1MessageBox.Show("¿Esta seguro que desea anular la factura?", "Preacución", C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminoPreguntaAnular)
                mostrarMensajePregunta("¿Está seguro que desea anular la factura?", _
                                       Program.TituloSistema, _
                                       "BORRARREGISTRO", _
                                       AddressOf TerminoPreguntaAnular, False)
                'dcProxy.FacturasBancaIns.Remove(_FacturasBancaInSelected)
                'FacturasBancaInSelected = _ListaFacturasBancaInv.LastOrDefault  'Dic202011  nueva
                'IsBusy = True
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")   'Dic202011 Nothing -> "BorrarRegistro"
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoPreguntaAnular(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            If validaFechaCierre("Anular") Then
                IsBusy = True
                If Not IsNothing(_FacturasBancaInSelected) Then
                    dcProxy.AnularFacturasBancaInv(FacturasBancaInSelected.IDFacturasBancaInv, FacturasBancaInSelected.ID, FacturasBancaInSelected.Prefijo, Program.Usuario, Program.HashConexion, AddressOf TerminoAnularFactura, "anular")
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Valida la Fecha de Cierre del Sistema
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>SLB20121005</remarks>
    Private Function validaFechaCierre(ByVal pstrAccion As String) As Boolean
        validaFechaCierre = True
        'If Format(FacturasBancaInSelected.Documento.Date, "MM/dd/yyyy") <= Format(FechaCierre, "MM/dd/yyyy") Then 'Intentan registrar un documento con fecha inferior a la fecha de cierre registrada en tblInstalacion
        If Format(FacturasBancaInSelected.Documento.Date, "yyyy/MM/dd") <= Format(FechaCierre, "yyyy/MM/dd") Then 'Intentan registrar un documento con fecha inferior a la fecha de cierre registrada en tblInstalacion
            If Format(FechaCierre, "yyyy/MM/dd") <> "1900/01/01" Then
                Select Case pstrAccion
                    Case "Anular"
                        A2Utilidades.Mensajes.mostrarMensaje("El documento con fecha (" & FacturasBancaInSelected.Documento.Date.ToLongDateString & ") no puede ser anulado porque su fecha es inferior a la fecha de cierre (" & FechaCierre.Date.ToLongDateString & ")", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Case "Actualizar"
                        A2Utilidades.Mensajes.mostrarMensaje("La operación con fecha (" & FacturasBancaInSelected.Documento.Date.ToLongDateString & ") no puede ser ingresada o modificada porque su fecha es inferior a la fecha de cierre (" & FechaCierre.Date.ToLongDateString & ")", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End Select
                validaFechaCierre = False

                '    If Format(FacturasBancaInSelected.Documento.Date, "MM/dd/yyyy") <> Format(FechaCierre, "MM/dd/yyyy") Then
                '        Select Case pstrAccion
                '            Case "Anular"
                '                A2Utilidades.Mensajes.mostrarMensaje("El documento con fecha (" & FacturasBancaInSelected.Documento.Date.ToLongDateString & ") no puede ser anulado porque su fecha no es igual a la fecha abierta para el usuario (" & FechaCierre.Date.ToLongDateString & ")", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '            Case "Actualizar"
                '                A2Utilidades.Mensajes.mostrarMensaje("La operación con fecha (" & FacturasBancaInSelected.Documento.Date.ToLongDateString & ") no puede ser ingresada o modificada porque su fecha no es igual a la fecha abierta para el usuario (" & FechaCierre.Date.ToLongDateString & ")", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '        End Select
                '        validaFechaCierre = False
                '    End If
                'Else
                '    Select Case pstrAccion
                '        Case "Anular"
                '            A2Utilidades.Mensajes.mostrarMensaje("El documento con fecha (" & FacturasBancaInSelected.Documento.Date.ToLongDateString & ") no puede ser anulado porque su fecha es inferior a la fecha de cierre (" & FechaCierre.Date.ToLongDateString & ")", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '        Case "Actualizar"
                '            A2Utilidades.Mensajes.mostrarMensaje("La operación con fecha (" & FacturasBancaInSelected.Documento.Date.ToLongDateString & ") no puede ser ingresada o modificada porque su fecha es inferior a la fecha de cierre (" & FechaCierre.Date.ToLongDateString & ")", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '    End Select
                '    validaFechaCierre = False
            End If
        End If
        Return validaFechaCierre
    End Function

    Public Overrides Sub Buscar()
        'cb.Prefijo = Nothing
        'cb.Documento = Nothing
        'cb.Comitente = Nothing
        'cb.NombreComitente = Nothing
        cb = New CamposBusquedaFacturasBancaIn()
        MyBase.CambioItem("cb")
        MyBase.Buscar()
    End Sub

    ''' <summary>
    ''' Calcula el total de la factura sumando los valores de cada detalle y adicionando a este resultado el valor del IVA y el valor de la retención. 
    ''' </summary>
    ''' <remarks>SLB20121023</remarks>
    Public Sub TotalDetalleFactura(ByVal BolOrigenDetalle As Boolean)

        Dim curIva As Decimal = 0
        Dim curRetencion As Decimal = 0
        Dim curSuma As Decimal = 0

        For Each obj In ListaDetalleFacturasBancaInv
            curSuma = curSuma + obj.Valor
        Next

        If (IsNothing(FacturasBancaInSelected.IVA) Or BolOrigenDetalle) And EditarIva Then
            curIva = curSuma * dblCampoTablaIVA / 100
            FacturasBancaInSelected.IVA = curIva
        End If

        If (IsNothing(FacturasBancaInSelected.Retencion) Or BolOrigenDetalle) And EditarRetencion Then
            curRetencion = curSuma * dblCampoTablaRetencion / 100
            FacturasBancaInSelected.Retencion = curRetencion
        End If

        TotalFactu.TotalFactura = curSuma + FacturasBancaInSelected.IVA - FacturasBancaInSelected.Retencion

    End Sub

    ''' <summary>
    ''' Buscar los datos del comitente que tiene asignada la orden.
    ''' </summary>
    ''' <param name="pstrIdComitente">Comitente que se debe buscar. Es opcional y normalmente se toma de la orden activa</param>
    ''' <remarks></remarks>
    Friend Sub buscarComitente(Optional ByVal pstrIdComitente As String = "", Optional ByVal pstrBusqueda As String = "")

        Dim strIdComitente As String = String.Empty

        Try
            If Not Me.FacturasBancaInSelected Is Nothing Then

                If Not strIdComitente.Equals(Me.FacturasBancaInSelected.IDComitente) Then
                    If pstrIdComitente.Trim.Equals(String.Empty) Then
                        strIdComitente = Me.FacturasBancaInSelected.IDComitente
                    Else
                        strIdComitente = pstrIdComitente
                    End If

                    If Not strIdComitente Is Nothing AndAlso Not strIdComitente.Trim.Equals(String.Empty) Then
                        objProxy.BuscadorClientes.Clear()
                        objProxy.Load(objProxy.buscarClienteEspecificoQuery(strIdComitente, Program.Usuario, "IdComitente", Program.HashConexion), AddressOf buscarComitenteCompleted, pstrBusqueda)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Friend Sub ComitenteSeleccionado(ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            FacturasBancaInSelected.IDComitente = pobjComitente.CodigoOYD
            FacturasBancaInSelected.Nombre = pobjComitente.Nombre
            FacturasBancaInSelected.TipoIdentificacion = pobjComitente.CodTipoIdentificacion
            FacturasBancaInSelected.Direccion = pobjComitente.DireccionEnvio
            FacturasBancaInSelected.Telefono = pobjComitente.Telefono
            FacturasBancaInSelected.NroIdentificacion = pobjComitente.NroDocumento
            'If Versioned.IsNumeric(pobjComitente.NroDocumento) Then
            '    FacturasBancaInSelected.NroIdentificacion = pobjComitente.NroDocumento
            'Else
            '    FacturasBancaInSelected.NroIdentificacion = 0
            'End If
        Else
            FacturasBancaInSelected.IDComitente = String.Empty
            FacturasBancaInSelected.Nombre = String.Empty
            FacturasBancaInSelected.TipoIdentificacion = String.Empty
            FacturasBancaInSelected.Direccion = String.Empty
            FacturasBancaInSelected.Telefono = String.Empty
            FacturasBancaInSelected.NroIdentificacion = Nothing
        End If
    End Sub

#End Region

#Region "Tablas Hijas"

    Private _ListaDetalleFacturasBancaInv As List(Of DetalleFacturasBancaIn)
    Public Property ListaDetalleFacturasBancaInv() As List(Of DetalleFacturasBancaIn)
        Get
            Return _ListaDetalleFacturasBancaInv
        End Get
        Set(ByVal value As List(Of DetalleFacturasBancaIn))
            _ListaDetalleFacturasBancaInv = value
            MyBase.CambioItem("ListaDetalleFacturasBancaInv")
            MyBase.CambioItem("ListaDetalleFacturasBancaInvPaged")
        End Set
    End Property

    Public ReadOnly Property ListaDetalleFacturasBancaInvPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalleFacturasBancaInv) Then
                Dim view = New PagedCollectionView(_ListaDetalleFacturasBancaInv)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _DetalleFacturasBancaInSelected As DetalleFacturasBancaIn
    Public Property DetalleFacturasBancaInSelected() As DetalleFacturasBancaIn
        Get
            Return _DetalleFacturasBancaInSelected
        End Get
        Set(ByVal value As DetalleFacturasBancaIn)
            _DetalleFacturasBancaInSelected = value
            MyBase.CambioItem("DetalleFacturasBancaInSelected")
        End Set
    End Property

    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Dim NewDetalleFacturasBancaIn As New DetalleFacturasBancaIn
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewDetalleFacturasBancaIn.IDComisionista = Me._FacturasBancaInSelected.IDComisionista
            NewDetalleFacturasBancaIn.IDSucComisionista = Me._FacturasBancaInSelected.IDSucComisionista
            'NewDetalleFacturasBancaIn.AnoDocumento = FacturasBancaInSelected.AnoDocumento
            NewDetalleFacturasBancaIn.AnoDocumento = Now.Date.Year
            NewDetalleFacturasBancaIn.Prefijo = FacturasBancaInSelected.Prefijo
            NewDetalleFacturasBancaIn.ID = DetalleFacturasBancaInPorDefecto.ID
            NewDetalleFacturasBancaIn.Secuencia = DetalleFacturasBancaInPorDefecto.Secuencia
            NewDetalleFacturasBancaIn.Descripcion = DetalleFacturasBancaInPorDefecto.Descripcion
            NewDetalleFacturasBancaIn.Valor = DetalleFacturasBancaInPorDefecto.Valor
            NewDetalleFacturasBancaIn.Actualizacion = DetalleFacturasBancaInPorDefecto.Actualizacion
            NewDetalleFacturasBancaIn.Usuario = Program.Usuario
            NewDetalleFacturasBancaIn.IDDetalleFacturasBancaInv = DetalleFacturasBancaInPorDefecto.IDDetalleFacturasBancaInv
            DetalleFacturasBancaInAnterior = DetalleFacturasBancaInSelected

            Dim objListaDetalleFacturas As New List(Of DetalleFacturasBancaIn)

            If Not IsNothing(ListaDetalleFacturasBancaInv) Then
                For Each I In ListaDetalleFacturasBancaInv
                    objListaDetalleFacturas.Add(I)

                Next
            End If

            objListaDetalleFacturas.Add(NewDetalleFacturasBancaIn)

            ListaDetalleFacturasBancaInv = objListaDetalleFacturas


            DetalleFacturasBancaInSelected = NewDetalleFacturasBancaIn

            MyBase.CambioItem("DetalleFacturasBancaInv")
            MyBase.CambioItem("ListaDetalleFacturasBancaInv")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Try
            If Not IsNothing(_DetalleFacturasBancaInSelected) Then
                If Not _DetalleFacturasBancaInSelected Is Nothing Then
                    Dim objListaDetalleFacturas As New List(Of DetalleFacturasBancaIn)
                    Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(DetalleFacturasBancaInSelected, ListaDetalleFacturasBancaInv)

                    If Not IsNothing(ListaDetalleFacturasBancaInv) Then
                        For Each I In ListaDetalleFacturasBancaInv
                            objListaDetalleFacturas.Add(I)
                        Next
                    End If


                    If objListaDetalleFacturas.Contains(_DetalleFacturasBancaInSelected) Then
                        objListaDetalleFacturas.Remove(_DetalleFacturasBancaInSelected)
                    End If

                    ListaDetalleFacturasBancaInv = objListaDetalleFacturas

                    If ListaDetalleFacturasBancaInv.Count > 0 Then
                        Program.PosicionarItemLista(DetalleFacturasBancaInSelected, ListaDetalleFacturasBancaInv, intRegistroPosicionar)
                    End If
                    MyBase.CambioItem("ListaDetalleFacturasBancaInv")
                    TotalDetalleFactura(True)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaFacturasBancaIn

    Implements INotifyPropertyChanged

    <StringLength(5, ErrorMessage:="La longitud máxima es de 5")> _
     <Display(Name:="Prefijo")> _
    Public Property Prefijo As String

    <Display(Name:="Número")> _
    Public Property ID As Integer

    Private _Documento As DateTime?
    <Display(Name:="Elaboración")> _
    Public Property Documento As DateTime?
        Get
            Return _Documento
        End Get
        Set(value As DateTime?)
            _Documento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Documento"))
        End Set
    End Property

    Private _Comitente As String
    <StringLength(17, ErrorMessage:="La longitud máxima es de 17")> _
     <Display(Name:="Comitente")> _
    Public Property Comitente() As String
        Get
            Return _Comitente
        End Get
        Set(ByVal value As String)
            _Comitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Comitente"))
        End Set
    End Property

    Private _NombreComitente As String
    <Display(Name:="")> _
    Public Property NombreComitente As String
        Get
            Return _NombreComitente
        End Get
        Set(value As String)
            _NombreComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreComitente"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class

''' <summary>
''' Clase para mostrar el Total de la Factura
''' </summary>
''' <remarks>SLB20121022</remarks>
Public Class TotalFacturaBancaInversion
    Implements INotifyPropertyChanged

    Private _TotalFactura As Double = 0
    Public Property TotalFactura() As Double
        Get
            Return _TotalFactura
        End Get
        Set(ByVal value As Double)
            _TotalFactura = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TotalFactura"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




