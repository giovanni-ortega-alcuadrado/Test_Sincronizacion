Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Imports System.Windows.Data
Imports System.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client

Public Class ObtenerOrdenSAEViewModel
    Implements INotifyPropertyChanged
    Private dcProxy As OyDPLUSutilidadesDomainContext
    Dim logCambiarPropiedad As Boolean = True

#Region "Inicialización"

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OyDPLUSutilidadesDomainContext()
            Else
                dcProxy = New OyDPLUSutilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of OyDPLUSutilidadesDomainContext.IOYDPLUSUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 500)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ObtenerOrdenSAEViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Metodos"
    Private Async Sub RecorrerVarios()
        Dim CantidadSeleccionada As Integer = 0
        Dim objListaTotalizar As New List(Of OYDPLUSUtilidades.tblOrdenesSAERentaFija)

        Try
            For Each li In ListaOrdenesSAERentaFija.Where(Function(i) CBool(i.Seleccionada) = True)
                CantidadSeleccionada = CantidadSeleccionada + 1
            Next
            OrdenesSAERentaFijaSelected.Mensajes = String.Empty
            If CantidadSeleccionada = 1 Then
                ViewOrdenSAE.OrdenSAERentaFijaSeleccionada = ListaOrdenesSAERentaFija.Where(Function(i) CBool(i.Seleccionada = True)).FirstOrDefault

                ViewOrdenSAE.OrdenSeleccionada = True
            ElseIf CantidadSeleccionada > 1 Then
                Dim X = ListaOrdenesSAERentaFija.Where(Function(i) CBool(i.Seleccionada) = True).FirstOrDefault
                For Each Y In ListaOrdenesSAERentaFija.Where(Function(i) CBool(i.Seleccionada) = True)

                    If (X.Especie = Y.Especie) And
                        (X.FechaCumplimiento = Y.FechaCumplimiento And (X.FechaEmision = Y.FechaEmision And X.FechaVencimiento = Y.FechaVencimiento And X.Modalidad = Y.Modalidad)) Then
                        objListaTotalizar.Add(Y)
                    End If
                Next

                If Not IsNothing(objListaTotalizar) Then
                    If objListaTotalizar.Count = CantidadSeleccionada Then
                        OrdenesSAERentaFijaSelected = ObtenerDatosSAERentaFijaVarios(objListaTotalizar)
                        ViewOrdenSAE.OrdenSAERentaFijaSeleccionada = OrdenesSAERentaFijaSelected
                        ViewOrdenSAE.OrdenSeleccionada = True
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("¡No se pudo seleccionar varios registros!" + vbCrLf + "→Verifique que las faciales de la Especie sean iguales y contengan todos valores correspondientes." _
                             + vbCrLf + "→Valide que las fechas de cumplimiento sean iguales.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        'ViewOrdenSAE.OrdenSAERentaFijaSeleccionada.Mensajes = "¡No se pudo seleccionar varios registros!" + vbCrLf + "→Verifique que las faciales de la Especie sean iguales y contengan todos valores correspondientes." _
                        '     + vbCrLf + "→Valide que las fechas de cumplimiento sean iguales."
                        ConsultarOrdenes = True
                        Await ConsultarOrdenesSAE()
                        ConsultarOrdenes = True
                    End If
                End If
            Else
                ViewOrdenSAE.OrdenSeleccionada = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recorrer registros SAE.",
                                 Me.ToString(), "RecorrerVarios", Application.Current.ToString(), Program.Maquina, ex)
            MostrarConsultandoOrden = Visibility.Collapsed
        End Try
    End Sub
    Private Async Sub RecorrerVariosAcciones()
        Dim CantidadSeleccionada As Integer = 0
        Dim objListaTotalizar As New List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones)

        Try
            For Each li In ListaOrdenesSAEAcciones.Where(Function(i) CBool(i.Seleccionada) = True)
                CantidadSeleccionada = CantidadSeleccionada + 1
            Next
            ' OrdenesSAEAccionesSelected.Mensajes = String.Empty
            If CantidadSeleccionada = 1 Then
                ViewOrdenSAE.OrdenSAEAccionesSeleccionada = Nothing
                ViewOrdenSAE.OrdenSAEAccionesSeleccionada = ListaOrdenesSAEAcciones.Where(Function(i) CBool(i.Seleccionada = True)).FirstOrDefault
                ViewOrdenSAE.OrdenSeleccionada = True
            ElseIf CantidadSeleccionada > 1 Then
                Dim X = ListaOrdenesSAEAcciones.Where(Function(i) CBool(i.Seleccionada) = True).FirstOrDefault
                For Each Y In ListaOrdenesSAEAcciones.Where(Function(i) CBool(i.Seleccionada) = True)

                    If (X.Especie = Y.Especie) Then
                        objListaTotalizar.Add(Y)
                    End If
                Next

                If Not IsNothing(objListaTotalizar) Then
                    If objListaTotalizar.Count = CantidadSeleccionada Then
                        OrdenesSAEAccionesSelected = ObtenerDatosSAERentaAccionesVarios(objListaTotalizar)
                        ViewOrdenSAE.OrdenSAEAccionesSeleccionada = OrdenesSAEAccionesSelected
                        If ViewOrdenSAE.OrdenSeleccionada Then
                            ViewOrdenSAE.OrdenSeleccionada = False
                        End If
                        ViewOrdenSAE.OrdenSeleccionada = True
                    Else
                        ViewOrdenSAE.OrdenSAEAccionesSeleccionada.Mensajes = "¡No se pudo seleccionar varios registros!" + vbCrLf + "→Verifique que los Nemotecnicos sean iguales."
                        A2Utilidades.Mensajes.mostrarMensaje(ViewOrdenSAE.OrdenSAEAccionesSeleccionada.Mensajes, "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        ConsultarOrdenes = True
                        Await ConsultarOrdenesSAE("NOTIFICARSELECCIONADA")
                        ConsultarOrdenes = True
                    End If
                End If
            Else
                ViewOrdenSAE.OrdenSeleccionada = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recorrer registros SAE.",
                                 Me.ToString(), "RecorrerVariosAcciones", Application.Current.ToString(), Program.Maquina, ex)
            MostrarConsultandoOrden = Visibility.Collapsed
        End Try
    End Sub
    Public Async Function ConsultarOrdenesSAE_CargaMasivaComplementacionPrecioPromedio(Optional ByVal pstrUserState As String = "") As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones)

        Try
            LimpiarOrdenesSAE()
            MostrarConsultandoOrden = Visibility.Visible
            If ConsultarOrdenes Then

                If Not IsNothing(dcProxy.tblOrdenesSAEAcciones) Then
                    dcProxy.tblOrdenesSAEAcciones.Clear()
                End If
                If Not IsNothing(dcProxy.tblOrdenesSAERentaFijas) Then
                    dcProxy.tblOrdenesSAERentaFijas.Clear()
                End If

                objRet = Await dcProxy.Load(dcProxy.OYDPLUS_ConsultarOrdenesBolsaAccionesPrecioPromedioQuery(TipoOperacion, strEspecieComplementacionPrecioPromedio, Program.Usuario, Program.HashConexion)).AsTask()
                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al cargar operaciones de bolsa..", Me.ToString(), "ConsultarOrdenesSAE_CargaMasivaComplementacionPrecioPromedio", Program.TituloSistema, Program.Maquina, objRet.Error, Program.RutaServicioLog)
                        MostrarConsultandoOrden = Visibility.Collapsed
                    Else
                        If objRet.Entities.ToList.Count > 0 Then
                            ListaOrdenesSAEAcciones = objRet.Entities.ToList
                            MostrarConsultandoOrden = Visibility.Collapsed
                        End If
                    End If
                End If
            End If

            Return Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al cargar operaciones de bolsa..", Me.ToString(), "ConsultarOrdenesSAE_CargaMasivaComplementacionPrecioPromedio", Application.Current.ToString(), Program.Maquina, ex)
            Return Nothing
        Finally
            MostrarConsultandoOrden = Visibility.Collapsed
        End Try
    End Function
    Public Async Function ConsultarOrdenesSAE(Optional ByVal pstrUserState As String = "") As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones)
        Dim objRetRF As LoadOperation(Of OYDPLUSUtilidades.tblOrdenesSAERentaFija)

        Try
            LimpiarOrdenesSAE()
            MostrarConsultandoOrden = Visibility.Visible
            If ConsultarOrdenes Then
                If Not IsNothing(CodigoReceptor) Then
                    If Not IsNothing(dcProxy.tblOrdenesSAEAcciones) Then
                        dcProxy.tblOrdenesSAEAcciones.Clear()
                    End If
                    If Not IsNothing(dcProxy.tblOrdenesSAERentaFijas) Then
                        dcProxy.tblOrdenesSAERentaFijas.Clear()
                    End If
                    If ClaseOrden = "A" Then
                        objRet = Await dcProxy.Load(dcProxy.OYDPLUS_ConsultarOrdenesBolsaAccionesQuery(CodigoReceptor, ClaseOrden, TipoOperacion, Program.Usuario, TipoNegocio, LiquidacionesHabilitar, Program.HashConexion)).AsTask()
                        If Not objRet Is Nothing Then
                            If objRet.HasError Then
                                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al cargar operaciones de bolsa..", Me.ToString(), "ConsultarOrdenesSAE", Program.TituloSistema, Program.Maquina, objRet.Error, Program.RutaServicioLog)
                                MostrarConsultandoOrden = Visibility.Collapsed
                            Else
                                If objRet.Entities.ToList.Count > 0 Then
                                    ListaOrdenesSAEAcciones = objRet.Entities.ToList
                                    ListaOrdenesSAEAccionesTotales = objRet.Entities.ToList
                                    MostrarConsultandoOrden = Visibility.Collapsed
                                End If
                            End If
                        End If
                    Else
                        objRetRF = Await dcProxy.Load(dcProxy.OYDPLUS_ConsultarOrdenesBolsaRentaFijaQuery(CodigoReceptor, ClaseOrden, TipoOperacion, Program.Usuario, TipoNegocio, LiquidacionesHabilitar, Program.HashConexion)).AsTask()
                        If Not objRetRF Is Nothing Then
                            If objRetRF.HasError Then
                                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al cargar operaciones de bolsa.", Me.ToString(), "ConsultarOrdenesSAE", Program.TituloSistema, Program.Maquina, objRetRF.Error, Program.RutaServicioLog)
                                MostrarConsultandoOrden = Visibility.Collapsed
                            Else
                                If objRetRF.Entities.ToList.Count > 0 Then
                                    ListaOrdenesSAERentaFija = objRetRF.Entities.ToList
                                    ListaOrdenesSAERentaFijaTotales = objRetRF.Entities.ToList
                                    MostrarConsultandoOrden = Visibility.Collapsed
                                    If pstrUserState = "NOTIFICARSELECCIONADA" Then
                                        If ViewOrdenSAE.OrdenSeleccionada = True Then
                                            ViewOrdenSAE.OrdenSeleccionada = False
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
            Return Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al cargar operaciones de bolsa..", Me.ToString(), "ConsultarOrdenesSAE", Application.Current.ToString(), Program.Maquina, ex)
            Return Nothing
        Finally
            MostrarConsultandoOrden = Visibility.Collapsed
        End Try
    End Function
    'Public Sub ConsultarOrdenesSAE(Optional ByVal pstrUserState As String = "")
    '    Try
    '        If ConsultarOrdenes Then
    '            If Not IsNothing(CodigoReceptor) Then
    '                If Not String.IsNullOrEmpty(CodigoReceptor) Then

    '                    LimpiarOrdenesSAE()
    '                    MostrarConsultandoOrden = Visibility.Visible
    '                    If Not IsNothing(dcProxy.tblOrdenesSAEAcciones) Then
    '                        dcProxy.tblOrdenesSAEAcciones.Clear()
    '                    End If
    '                    If Not IsNothing(dcProxy.tblOrdenesSAERentaFijas) Then
    '                        dcProxy.tblOrdenesSAERentaFijas.Clear()
    '                    End If
    '                    If ClaseOrden = "A" Then
    '                        dcProxy.Load(dcProxy.OYDPLUS_ConsultarOrdenesBolsaAccionesQuery(CodigoReceptor, ClaseOrden, TipoOperacion, Program.Usuario, TipoNegocio), AddressOf TerminoConsultarOrdenesSAEAcciones, String.Empty)
    '                    Else
    '                        dcProxy.Load(dcProxy.OYDPLUS_ConsultarOrdenesBolsaRentaFijaQuery(CodigoReceptor, ClaseOrden, TipoOperacion, Program.Usuario, TipoNegocio), AddressOf TerminoConsultarOrdenesSAERentaFija, pstrUserState)
    '                    End If


    '                Else
    '                    LimpiarOrdenesSAE()
    '                    MostrarConsultandoOrden = Visibility.Collapsed
    '                End If
    '            Else
    '                LimpiarOrdenesSAE()
    '                MostrarConsultandoOrden = Visibility.Collapsed
    '            End If
    '        Else
    '            LimpiarOrdenesSAE()
    '            MostrarConsultandoOrden = Visibility.Collapsed
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las ordenes en bolsa del receptor.", _
    '                             Me.ToString(), "ConsultarOrdenesSAE", Application.Current.ToString(), Program.Maquina, ex)
    '        MostrarConsultandoOrden = Visibility.Collapsed
    '    End Try
    'End Sub
    Public Sub LimpiarOrdenesSAE()
        Try
            ListaOrdenesSAEAcciones = Nothing
            OrdenesSAEAccionesSelected = Nothing
            ListaOrdenesSAERentaFija = Nothing
            OrdenesSAERentaFijaSelected = Nothing
            'JDR20180302
            SeleccionarTodosFolios = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los controles.", _
                                 Me.ToString(), "LimpiarOrdenesSAE", Application.Current.ToString(), Program.Maquina, ex)
            MostrarConsultandoOrden = Visibility.Collapsed
        End Try
    End Sub
    Public Function ObtenerDatosSAERentaFijaVarios(pobj As List(Of OYDPLUSUtilidades.tblOrdenesSAERentaFija)) As OYDPLUSUtilidades.tblOrdenesSAERentaFija
        Try
            Dim obj As New OYDPLUSUtilidades.tblOrdenesSAERentaFija
            Dim cantidad As Double = 0
            Dim captacion As Double = 0
            Dim Futuro As Double = 0
            Dim precio As Double = 0
            Dim precioLimpio As Double = 0
            Dim precioSucio As Double = 0

            For Each li In pobj
                cantidad = CDbl(cantidad + IIf(IsNothing(li.Cantidad), 0, li.Cantidad))
                precio = CDbl(precio + (IIf(IsNothing(li.Cantidad), 0, li.Cantidad) * IIf(IsNothing(li.Precio), 0, li.Precio)))
                precioLimpio = CDbl(precioLimpio + (IIf(IsNothing(li.Cantidad), 0, li.Cantidad) * IIf(IsNothing(li.PrecioLimpio), 0, li.PrecioLimpio)))
                precioSucio = CDbl(precioSucio + (IIf(IsNothing(li.Cantidad), 0, li.Cantidad) * IIf(IsNothing(li.PrecioSucio), 0, li.PrecioSucio)))
                captacion = CDbl(captacion + IIf(IsNothing(li.CantidadCaptacion), 0, li.CantidadCaptacion))
                Futuro = CDbl(Futuro + IIf(IsNothing(li.CantidadFutura), 0, li.CantidadFutura))
            Next
            precio = precio / cantidad
            precioLimpio = precioLimpio / cantidad
            precioSucio = precioSucio / cantidad

            obj.Precio = precio
            obj.PrecioLimpio = precioLimpio
            obj.PrecioSucio = precioSucio
            obj.Cantidad = cantidad
            obj.ID = pobj.FirstOrDefault.ID
            obj.ISIN = pobj.FirstOrDefault.ISIN
            obj.Mercado = pobj.FirstOrDefault.Mercado
            obj.Modalidad = pobj.FirstOrDefault.Modalidad
            obj.NombreCondicionesNegociacion = pobj.FirstOrDefault.NombreModalidad
            obj.NombreIndicador = pobj.FirstOrDefault.NombreIndicador
            obj.NombreModalidad = pobj.FirstOrDefault.NombreModalidad
            obj.NroLiquidacion = pobj.FirstOrDefault.NroLiquidacion
            obj.PuntosIndicador = pobj.FirstOrDefault.PuntosIndicador
            obj.RuedaNegocio = pobj.FirstOrDefault.RuedaNegocio
            obj.Seleccionada = pobj.FirstOrDefault.Seleccionada
            obj.TasaInicial = pobj.FirstOrDefault.TasaInicial
            obj.TasaNominal = pobj.FirstOrDefault.TasaNominal
            obj.TasaRepoSimultanea = pobj.FirstOrDefault.TasaRepoSimultanea
            obj.TipoSimultanea = pobj.FirstOrDefault.TipoSimultanea
            obj.Trader = pobj.FirstOrDefault.Trader
            obj.UBICACIONTITULO = pobj.FirstOrDefault.UBICACIONTITULO
            obj.FechaEmision = pobj.FirstOrDefault.FechaEmision
            obj.FechaVencimiento = pobj.FirstOrDefault.FechaVencimiento
            obj.Especie = pobj.FirstOrDefault.Especie
            obj.TasaInicial = pobj.FirstOrDefault.TasaInicial
            obj.Indicador = pobj.FirstOrDefault.Indicador
            obj.FechaCumplimiento = pobj.FirstOrDefault.FechaCumplimiento
            obj.DiasCumplimiento = pobj.FirstOrDefault.DiasCumplimiento
            obj.CondicionesNegociacion = pobj.FirstOrDefault.CondicionesNegociacion
            obj.Clasificacion = pobj.FirstOrDefault.Clasificacion
            obj.FechaReferencia = pobj.FirstOrDefault.FechaReferencia
            obj.EstadoTitulo = pobj.FirstOrDefault.EstadoTitulo
            obj.CantidadRepoSimultanea = pobj.FirstOrDefault.CantidadRepoSimultanea
            obj.dtmFechaCumplimientoRepo = pobj.FirstOrDefault.dtmFechaCumplimientoRepo
            obj.CantidadCaptacion = captacion
            obj.CantidadFutura = Futuro 

            Return obj

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al sumar las cantidades y precio de varios registros.", _
                                 Me.ToString(), "ObtenerDatosSAERentaFijaVarios", Application.Current.ToString(), Program.Maquina, ex)
            MostrarConsultandoOrden = Visibility.Collapsed
            Return Nothing
        End Try
    End Function
    Public Function ObtenerDatosSAERentaAccionesVarios(pobj As List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones)) As OYDPLUSUtilidades.tblOrdenesSAEAcciones
        Try
            Dim obj As New OYDPLUSUtilidades.tblOrdenesSAEAcciones
            Dim cantidad As Double = 0
            Dim precio As Double = 0
            Dim precioLimpio As Double = 0
            ' Dim precioSucio As Double = 0

            For Each li In pobj
                cantidad = CDbl(cantidad + li.Cantidad)
                precio = CDbl(precio + (li.Cantidad * li.Precio))
                precioLimpio = CDbl(precioLimpio + (li.Cantidad * li.Precio))
                'precioSucio = CDbl(precioSucio + (li.Cantidad * li.PrecioSucio))
            Next
            precio = precio / cantidad
            precioLimpio = precioLimpio / cantidad
            'precioSucio = precioSucio / cantidad

            obj.Precio = precio
            obj.Precio = precioLimpio
            ' obj.PrecioSucio = precioSucio
            obj.Cantidad = cantidad
            obj.ID = pobj.FirstOrDefault.ID
            obj.NroLiquidacion = pobj.FirstOrDefault.NroLiquidacion
            obj.Seleccionada = pobj.FirstOrDefault.Seleccionada
            obj.Especie = pobj.FirstOrDefault.Especie
            obj.FechaReferencia = pobj.FirstOrDefault.FechaReferencia



            Return obj

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al sumar las cantidades y precio de varios registros.", _
                                 Me.ToString(), "ObtenerDatosSAERentaFijaVarios", Application.Current.ToString(), Program.Maquina, ex)
            MostrarConsultandoOrden = Visibility.Collapsed
            Return Nothing
        End Try
    End Function
    Public Sub FiltrarOperacionesEspecieCumplimiento()
        Try
            If (Not (String.IsNullOrEmpty(Especie)) And Not (Especie = "(No Seleccionado)")) Then
                If ClaseOrden = "A" Then
                    Dim objListatemp As New List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones)
                    If Not IsNothing(ListaOrdenesSAEAccionesTotales) Then
                        If ListaOrdenesSAEAccionesTotales.Count > 0 Then

                            For Each li In ListaOrdenesSAEAccionesTotales.Where(Function(x) (x.Especie.ToUpper = Especie.ToUpper))
                                objListatemp.Add(li)
                            Next
                            ListaOrdenesSAEAcciones = Nothing
                            ListaOrdenesSAEAcciones = objListatemp
                        End If
                    End If
                Else
                    Dim objListatemp As New List(Of OYDPLUSUtilidades.tblOrdenesSAERentaFija)
                    If Not IsNothing(ListaOrdenesSAERentaFijaTotales) Then
                        If ListaOrdenesSAERentaFijaTotales.Count > 0 Then
                            Dim logEsValida As Boolean = True

                            For Each li In ListaOrdenesSAERentaFijaTotales
                                logEsValida = True
                                If Not String.IsNullOrEmpty(Especie) Then
                                    If li.Especie.ToUpper = Especie.ToUpper Then
                                        logEsValida = True
                                    Else
                                        logEsValida = False
                                    End If
                                End If
                                If Not IsNothing(FechaEmision) And logEsValida Then
                                    If li.FechaEmision = FechaEmision Then
                                        logEsValida = True
                                    Else
                                        logEsValida = False
                                    End If
                                End If
                                If Not IsNothing(FechaVencimiento) And logEsValida Then
                                    If li.FechaVencimiento = FechaVencimiento Then
                                        logEsValida = True
                                    Else
                                        logEsValida = False
                                    End If
                                End If
                                If Not String.IsNullOrEmpty(Modalidad) And logEsValida Then
                                    If li.Modalidad.ToUpper = Modalidad.ToUpper Then
                                        logEsValida = True
                                    Else
                                        logEsValida = False
                                    End If
                                End If

                                If logEsValida Then
                                    objListatemp.Add(li)
                                End If
                            Next
                            ListaOrdenesSAERentaFija = Nothing
                            ListaOrdenesSAERentaFija = objListatemp
                        End If
                    End If
                End If
            Else
                If ClaseOrden = "A" Then
                    If Not IsNothing(ListaOrdenesSAEAccionesTotales) Then
                        'JDRH - Se limpia la totalidad de la lista como NO seleccionada
                        Dim ListaAccionesTemp As New List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones)
                        If Not IsNothing(ListaOrdenesSAEAccionesTotales) Then
                            If ListaOrdenesSAEAccionesTotales.Count > 0 Then
                                For Each li In ListaOrdenesSAEAccionesTotales
                                    ListaAccionesTemp.Add(li)
                                Next

                                For Each li In ListaAccionesTemp
                                    li.Seleccionada = False
                                Next

                                ListaOrdenesSAEAccionesTotales = Nothing
                                ListaOrdenesSAEAccionesTotales = ListaAccionesTemp

                            End If
                        End If

                        ListaOrdenesSAEAcciones = ListaOrdenesSAEAccionesTotales
                        SeleccionarTodosFolios = False
                    End If
                Else
                    If Not IsNothing(ListaOrdenesSAERentaFijaTotales) Then
                        'JDRH - Se limpia la totalidad de la lista como NO seleccionada
                        Dim ListaRentaFijaTemp As New List(Of OYDPLUSUtilidades.tblOrdenesSAERentaFija)
                        If Not IsNothing(ListaOrdenesSAERentaFijaTotales) Then
                            If ListaOrdenesSAERentaFijaTotales.Count > 0 Then
                                For Each li In ListaOrdenesSAERentaFijaTotales
                                    ListaRentaFijaTemp.Add(li)
                                Next

                                For Each li In ListaRentaFijaTemp
                                    li.Seleccionada = False
                                Next

                                ListaOrdenesSAERentaFijaTotales = Nothing
                                ListaOrdenesSAERentaFijaTotales = ListaRentaFijaTemp

                            End If
                        End If



                        ListaOrdenesSAERentaFija = ListaOrdenesSAERentaFijaTotales
                        SeleccionarTodosFolios = False

                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al filtrar las Operaciones de Bolsa.", Me.ToString(), "FiltrarOperacionesEspecieCumplimiento", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Sub
#End Region

#Region "Propiedades"
    'Propiedad para validacion de Carga Masiva por Precio Promedio : JDOL20170309
    Private _strEspecieComplementacionPrecioPromedio As String = String.Empty
    Public Property strEspecieComplementacionPrecioPromedio() As String
        Get
            Return _strEspecieComplementacionPrecioPromedio
        End Get
        Set(ByVal value As String)
            _strEspecieComplementacionPrecioPromedio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strEspecieComplementacionPrecioPromedio"))
        End Set
    End Property
    Private _ComplementacionPrecioPromedio As Boolean = False
    Public Property ComplementacionPrecioPromedio() As Boolean
        Get
            Return _ComplementacionPrecioPromedio
        End Get
        Set(ByVal value As Boolean)
            _ComplementacionPrecioPromedio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ComplementacionPrecioPromedio"))
        End Set
    End Property

    '************************************************************************************************
    Private _ListaOrdenesSAEAcciones As List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones)
    Public Property ListaOrdenesSAEAcciones() As List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones)
        Get
            Return _ListaOrdenesSAEAcciones
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones))
            _ListaOrdenesSAEAcciones = value
            ViewOrdenSAE.ListaOrdenSAEAcciones = _ListaOrdenesSAEAcciones

            If Not IsNothing(ViewOrdenSAE.ListaOrdenSAEAcciones) Then
                If ViewOrdenSAE.ListaOrdenSAEAcciones.Count > 0 Then

                    Dim strEspecietemp As String = _ListaOrdenesSAEAcciones.FirstOrDefault.Especie.ToString()
                    Dim intCantidadOperaciones As Integer = ViewOrdenSAE.ListaOrdenSAEAcciones.Count
                    Dim ContadorOpIguales As Integer = 0

                    For Each li In ViewOrdenSAE.ListaOrdenSAEAcciones
                        If (li.Especie.ToUpper = strEspecietemp.ToUpper) Then
                            ContadorOpIguales = ContadorOpIguales + 1
                        End If

                    Next

                    If ContadorOpIguales = intCantidadOperaciones Then
                        HabilitarSeleccionarTodosFolios = True
                    Else
                        HabilitarSeleccionarTodosFolios = False
                        'SeleccionarTodosFolios = False
                    End If

                End If
            End If

            If Not IsNothing(_ListaOrdenesSAEAcciones) Then
                OrdenesSAEAccionesSelected = _ListaOrdenesSAEAcciones.FirstOrDefault
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaOrdenesSAEAcciones"))
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaOrdenesSAEAccionesPaged"))
        End Set
    End Property

    Public ReadOnly Property ListaOrdenesSAEAccionesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaOrdenesSAEAcciones) Then
                Dim view = New PagedCollectionView(_ListaOrdenesSAEAcciones)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ListaOrdenesSAEAccionesTotales As List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones)
    Public Property ListaOrdenesSAEAccionesTotales() As List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones)
        Get
            Return _ListaOrdenesSAEAccionesTotales
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones))
            _ListaOrdenesSAEAccionesTotales = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaOrdenesSAEAccionesTotales"))
        End Set
    End Property

    Private WithEvents _OrdenesSAEAccionesSelected As OYDPLUSUtilidades.tblOrdenesSAEAcciones
    Public Property OrdenesSAEAccionesSelected() As OYDPLUSUtilidades.tblOrdenesSAEAcciones
        Get
            Return _OrdenesSAEAccionesSelected
        End Get
        Set(ByVal value As OYDPLUSUtilidades.tblOrdenesSAEAcciones)
            _OrdenesSAEAccionesSelected = value
            ViewOrdenSAE.OrdenSAEAccionesSeleccionada = OrdenesSAEAccionesSelected

            If Not IsNothing(OrdenesSAEAccionesSelected) Then
                If OrdenesSAEAccionesSelected.Seleccionada Then
                    ViewOrdenSAE.OrdenSeleccionada = True
                Else
                    ViewOrdenSAE.OrdenSeleccionada = False
                End If
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("OrdenesSAEAccionesSelected"))
        End Set
    End Property

    Private _ListaOrdenesSAERentaFija As List(Of OYDPLUSUtilidades.tblOrdenesSAERentaFija)
    Public Property ListaOrdenesSAERentaFija() As List(Of OYDPLUSUtilidades.tblOrdenesSAERentaFija)
        Get
            Return _ListaOrdenesSAERentaFija
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblOrdenesSAERentaFija))
            _ListaOrdenesSAERentaFija = value
            ViewOrdenSAE.ListaOrdenSAERentaFija = _ListaOrdenesSAERentaFija

            If Not IsNothing(ViewOrdenSAE.ListaOrdenSAERentaFija) Then
                If ViewOrdenSAE.ListaOrdenSAERentaFija.Count > 0 Then

                    Dim intCantidadOperaciones As Integer = ViewOrdenSAE.ListaOrdenSAERentaFija.Count
                    Dim ContadorOpIguales As Integer = 0

                    For Each li In ViewOrdenSAE.ListaOrdenSAERentaFija
                        If (li.Especie.ToUpper = _ListaOrdenesSAERentaFija.FirstOrDefault.Especie.ToUpper And
                            li.FechaEmision = _ListaOrdenesSAERentaFija.FirstOrDefault.FechaEmision And
                            li.FechaVencimiento = _ListaOrdenesSAERentaFija.FirstOrDefault.FechaVencimiento And
                            li.Modalidad = _ListaOrdenesSAERentaFija.FirstOrDefault.Modalidad) Then
                            ContadorOpIguales = ContadorOpIguales + 1
                        End If

                    Next

                    If ContadorOpIguales = intCantidadOperaciones Then
                        HabilitarSeleccionarTodosFolios = True
                    Else
                        HabilitarSeleccionarTodosFolios = False
                        'SeleccionarTodosFolios = False
                    End If

                End If
            End If

            If Not IsNothing(_ListaOrdenesSAERentaFija) Then
                OrdenesSAERentaFijaSelected = _ListaOrdenesSAERentaFija.FirstOrDefault
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaOrdenesSAERentaFija"))
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaOrdenesSAERentaFijaPaged"))
        End Set
    End Property

    Public ReadOnly Property ListaOrdenesSAERentaFijaPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaOrdenesSAERentaFija) Then
                Dim view = New PagedCollectionView(_ListaOrdenesSAERentaFija)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ListaOrdenesSAERentaFijaTotales As List(Of OYDPLUSUtilidades.tblOrdenesSAERentaFija)
    Public Property ListaOrdenesSAERentaFijaTotales() As List(Of OYDPLUSUtilidades.tblOrdenesSAERentaFija)
        Get
            Return _ListaOrdenesSAERentaFijaTotales
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblOrdenesSAERentaFija))
            _ListaOrdenesSAERentaFijaTotales = value
        End Set
    End Property

    Private WithEvents _OrdenesSAERentaFijaSelected As OYDPLUSUtilidades.tblOrdenesSAERentaFija
    Public Property OrdenesSAERentaFijaSelected() As OYDPLUSUtilidades.tblOrdenesSAERentaFija
        Get
            Return _OrdenesSAERentaFijaSelected
        End Get
        Set(ByVal value As OYDPLUSUtilidades.tblOrdenesSAERentaFija)
            _OrdenesSAERentaFijaSelected = value
            'ViewOrdenSAE.OrdenSAERentaFijaSeleccionada = OrdenesSAERentaFijaSelected

            If Not IsNothing(OrdenesSAERentaFijaSelected) Then
                'If OrdenesSAERentaFijaSelected.Seleccionada Then
                '    ViewOrdenSAE.OrdenSeleccionada = True
                'Else
                '    ViewOrdenSAE.OrdenSeleccionada = False
                'End If
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("OrdenesSAERentaFijaSelected"))
        End Set
    End Property

    Private _ViewOrdenSAE As ObtenerOrdenSAEView
    Public Property ViewOrdenSAE() As ObtenerOrdenSAEView
        Get
            Return _ViewOrdenSAE
        End Get
        Set(ByVal value As ObtenerOrdenSAEView)
            _ViewOrdenSAE = value
        End Set
    End Property

    Private _MostrarConsultandoOrden As Visibility = Visibility.Collapsed
    Public Property MostrarConsultandoOrden() As Visibility
        Get
            Return _MostrarConsultandoOrden
        End Get
        Set(ByVal value As Visibility)
            _MostrarConsultandoOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarConsultandoOrden"))
        End Set
    End Property

    Private _CodigoReceptor As String
    Public Property CodigoReceptor() As String
        Get
            Return _CodigoReceptor
        End Get
        Set(ByVal value As String)
            _CodigoReceptor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodigoReceptor"))
        End Set
    End Property

    Private _ClaseOrden As String
    Public Property ClaseOrden() As String
        Get
            Return _ClaseOrden
        End Get
        Set(ByVal value As String)
            _ClaseOrden = value
            If Not IsNothing(ClaseOrden) Then
                If ClaseOrden.ToUpper = "C" Then
                    MostrarCamposRentafija = True
                Else
                    MostrarCamposRentafija = False
                End If
            End If
            If ConsultarOrdenes And Not String.IsNullOrEmpty(ClaseOrden) And
                Not String.IsNullOrEmpty(TipoNegocio) And
                Not String.IsNullOrEmpty(TipoOperacion) Then
                ConsultarOrdenesSAE()
            End If

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ClaseOrden"))
        End Set
    End Property

    Private _TipoOperacion As String
    Public Property TipoOperacion() As String
        Get
            Return _TipoOperacion
        End Get
        Set(ByVal value As String)
            _TipoOperacion = value
            If ConsultarOrdenes And Not String.IsNullOrEmpty(ClaseOrden) And
              Not String.IsNullOrEmpty(TipoNegocio) And
              Not String.IsNullOrEmpty(TipoOperacion) Then
                ConsultarOrdenesSAE()
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoOperacion"))
        End Set
    End Property

    Private _MostrarCamposRentafija As Boolean
    Public Property MostrarCamposRentafija() As Boolean
        Get
            Return _MostrarCamposRentafija
        End Get
        Set(ByVal value As Boolean)
            _MostrarCamposRentafija = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarCamposRentafija"))
        End Set
    End Property

    Private _ConsultarOrdenes As Boolean
    Public Property ConsultarOrdenes() As Boolean
        Get
            Return _ConsultarOrdenes
        End Get
        Set(ByVal value As Boolean)
            _ConsultarOrdenes = value
            If _ComplementacionPrecioPromedio Then
                ConsultarOrdenesSAE_CargaMasivaComplementacionPrecioPromedio()
            Else
                ConsultarOrdenesSAE()

            End If
        End Set
    End Property

    Private _TipoNegocio As String
    Public Property TipoNegocio() As String
        Get
            Return _TipoNegocio
        End Get
        Set(ByVal value As String)
            _TipoNegocio = value
            If ConsultarOrdenes And Not String.IsNullOrEmpty(ClaseOrden) And
                Not String.IsNullOrEmpty(TipoNegocio) And
                Not String.IsNullOrEmpty(TipoOperacion) Then
                ConsultarOrdenesSAE()
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoNegocio"))
        End Set
    End Property

    Private _LiquidacionesHabilitar As String
    Public Property LiquidacionesHabilitar() As String
        Get
            Return _LiquidacionesHabilitar
        End Get
        Set(ByVal value As String)
            _LiquidacionesHabilitar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("LiquidacionesHabilitar"))
        End Set
    End Property

    Private _Especie As String
    Public Property Especie() As String
        Get
            Return _Especie
        End Get
        Set(ByVal value As String)
            _Especie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Especie"))
        End Set
    End Property

    Private _SeleccionarTodosFolios As Boolean
    Public Property SeleccionarTodosFolios() As Boolean
        Get
            Return _SeleccionarTodosFolios
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarTodosFolios = value

            Try
                If _ComplementacionPrecioPromedio Then

                    Dim objListatemp As New List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones)
                    If Not IsNothing(ListaOrdenesSAEAcciones) Then
                        If ListaOrdenesSAEAcciones.Count > 0 Then
                            For Each li In ListaOrdenesSAEAcciones
                                objListatemp.Add(li)
                            Next

                            For Each li In objListatemp
                                li.Seleccionada = _SeleccionarTodosFolios
                            Next

                            ListaOrdenesSAEAcciones = Nothing
                            ListaOrdenesSAEAcciones = objListatemp

                            RecorrerVariosAcciones()

                        End If
                    End If
                Else
                    If ClaseOrden = "A" Then
                        Dim objListatemp As New List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones)
                        If Not IsNothing(ListaOrdenesSAEAcciones) Then
                            If ListaOrdenesSAEAcciones.Count > 0 Then
                                For Each li In ListaOrdenesSAEAcciones
                                    objListatemp.Add(li)
                                Next

                                For Each li In objListatemp
                                    li.Seleccionada = _SeleccionarTodosFolios
                                Next

                                ListaOrdenesSAEAcciones = Nothing
                                ListaOrdenesSAEAcciones = objListatemp

                                RecorrerVariosAcciones()
                            End If
                        End If
                    Else
                        Dim objListatemp As New List(Of OYDPLUSUtilidades.tblOrdenesSAERentaFija)
                        If Not IsNothing(ListaOrdenesSAERentaFija) Then
                            If ListaOrdenesSAERentaFija.Count > 0 Then
                                For Each li In ListaOrdenesSAERentaFija
                                    objListatemp.Add(li)
                                Next

                                For Each li In objListatemp
                                    li.Seleccionada = _SeleccionarTodosFolios
                                Next

                                ListaOrdenesSAERentaFija = Nothing
                                ListaOrdenesSAERentaFija = objListatemp
                                RecorrerVarios()

                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar todas las Operaciones.",
                                                             Me.ToString(), "SeleccionarTodosFolios", Application.Current.ToString(), Program.Maquina, ex)
            End Try
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SeleccionarTodosFolios"))
        End Set
    End Property

    Private _HabilitarSeleccionarTodosFolios As Boolean = False
    Public Property HabilitarSeleccionarTodosFolios() As Boolean
        Get
            Return _HabilitarSeleccionarTodosFolios
        End Get
        Set(ByVal value As Boolean)
            _HabilitarSeleccionarTodosFolios = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarSeleccionarTodosFolios"))
        End Set
    End Property

    Private _FechaEmision As Nullable(Of DateTime)
    Public Property FechaEmision() As Nullable(Of DateTime)
        Get
            Return _FechaEmision
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaEmision = value
            'A2Utilidades.Mensajes.mostrarMensaje(value.ToString, "FechaEmision")
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaEmision"))
        End Set
    End Property

    Private _FechaVencimiento As Nullable(Of DateTime)
    Public Property FechaVencimiento() As Nullable(Of DateTime)
        Get
            Return _FechaVencimiento
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaVencimiento = value
            'A2Utilidades.Mensajes.mostrarMensaje(value.ToString, "FechaVencimiento")
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaVencimiento"))
        End Set
    End Property

    Private _Modalidad As String
    Public Property Modalidad() As String
        Get
            Return _Modalidad
        End Get
        Set(ByVal value As String)
            _Modalidad = value
            'A2Utilidades.Mensajes.mostrarMensaje(value.ToString, "Modalidad")
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Modalidad"))
        End Set
    End Property
#End Region

#Region "Resultados Asincronicos"

    Private Sub TerminoConsultarOrdenesSAEAcciones(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones))
        Try
            If lo.HasError = False Then
                ListaOrdenesSAEAcciones = lo.Entities.ToList
                MostrarConsultandoOrden = Visibility.Collapsed
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener las ordenes de SAE.", Me.ToString(), "TerminoConsultarOrdenesSAEAcciones", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                MostrarConsultandoOrden = Visibility.Collapsed
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener las ordenes de SAE.", Me.ToString(), "TerminoConsultarOrdenesSAEAcciones", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            MostrarConsultandoOrden = Visibility.Collapsed
        End Try
    End Sub

    Private Sub TerminoConsultarOrdenesSAERentaFija(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblOrdenesSAERentaFija))
        Try
            If lo.HasError = False Then
                ListaOrdenesSAERentaFija = lo.Entities.ToList

                If lo.UserState.ToString = "NOTIFICARSELECCIONADA" Then
                    If ViewOrdenSAE.OrdenSeleccionada = True Then
                        ViewOrdenSAE.OrdenSeleccionada = False
                    End If
                End If

                MostrarConsultandoOrden = Visibility.Collapsed
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener las ordenes de SAE.", Me.ToString(), "TerminoConsultarOrdenesSAERentaFija", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                MostrarConsultandoOrden = Visibility.Collapsed
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener las ordenes de SAE.", Me.ToString(), "TerminoConsultarOrdenesSAERentaFija", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            MostrarConsultandoOrden = Visibility.Collapsed
        End Try
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Este evento se dispara cuando alguna propiedad de la orden activa cambia
    ''' Desarrollado por Juan David Correa
    ''' Fecha 14 de Agosto del 2012
    ''' </summary>
    Private Sub _OrdenesSAEAccionesSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _OrdenesSAEAccionesSelected.PropertyChanged
        Try
            ViewOrdenSAE.ListaOrdenSAEAcciones = _ListaOrdenesSAEAcciones
            Select Case e.PropertyName.ToLower()
                Case "seleccionada"
                    If logCambiarPropiedad Then
                        If OrdenesSAEAccionesSelected.Seleccionada Then
                            logCambiarPropiedad = False
                            RecorrerVariosAcciones()
                            logCambiarPropiedad = True
                        Else
                            RecorrerVariosAcciones()
                        End If
                    End If
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_OrdenesSAEAccionesSelected_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            MostrarConsultandoOrden = Visibility.Collapsed
        End Try

    End Sub

    '' <summary>
    ''' Este evento se dispara cuando alguna propiedad de la orden activa cambia
    ''' Desarrollado por Juan David Correa
    ''' Fecha 14 de Agosto del 2012
    ''' </summary>
    Private Sub _OrdenesSAERentaFijaSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _OrdenesSAERentaFijaSelected.PropertyChanged
        Try
            ViewOrdenSAE.ListaOrdenSAERentaFija = _ListaOrdenesSAERentaFija
            Select Case e.PropertyName.ToLower()
                Case "seleccionada"
                    If logCambiarPropiedad Then
                        If OrdenesSAERentaFijaSelected.Seleccionada Then
                            logCambiarPropiedad = False
                            RecorrerVarios()
                            logCambiarPropiedad = True
                        Else
                            RecorrerVarios()
                        End If
                    End If
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_OrdenesSAERentaFijaSelected_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            MostrarConsultandoOrden = Visibility.Collapsed
        End Try

    End Sub

#End Region

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
