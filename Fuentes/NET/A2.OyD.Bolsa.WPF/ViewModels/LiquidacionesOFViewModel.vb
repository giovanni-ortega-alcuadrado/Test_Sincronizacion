Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: LiquidacionesViewModel.vb
'Generado el : 05/30/2011 09:18:58
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
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports System.Threading.Tasks

Public Class LiquidacionesOFViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Declaración de Variables"

    Public Property cb As New CamposBusquedaLiquidacionesOF
    Private LiquidacionesOFPorDefecto As LiquidacionesOF
    Private LiquidacionesOFAnterior As LiquidacionesOF
    Private ReceptoresOrdenesOFAnterior As ReceptoresOrdenesOF
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private mdcProxyUtilidad02 As UtilidadesDomainContext
    Dim objProxy1 As UtilidadesDomainContext
    Private dcProxy As BolsaDomainContext
    Dim dcProxy1 As BolsaDomainContext
    Private objProxy As BolsaDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Dim strTipoAplazamiento As String = ""
    Dim Userstate As String = ""
    Dim aplazamientoserie As Aplazamiento_En_Serie
    Const strtitulo As String = "Titulo"
    Const strefectivo As String = "Efectivo"
    Dim strAplazamiento As String = ""
    Dim strerror As String
    Dim strnroaplazamiento As Integer
    Dim selected As Boolean
    Dim fechaCierre As DateTime
    Dim ListaLiquidacionesOFvalidar As New List(Of LiquidacionesOFConsultar)
    Dim mlogRepo As Boolean
    Dim consultarcantidadesOF As New consultarcantidadOF
    Dim mcursaldoOrden As Double
    Dim LiquidacionesOrdenOF As New LiquidacionesOrdenesOF
    Const BOLSACOLOMBIA = 4
    Dim numeroliq As Integer
    Dim duplica As Boolean
    Public estadoregistro, mlogCantidad As Boolean
    Public logNuevoRegistro As Boolean = False
    Public logEditarRegistro As Boolean = False
    Public IDLiquidacionEdicion As Integer = 0
    Public ParcialEdicion As Integer = 0
    Public IDBolsaEdicion As Integer = 0
    Public FechaLiquidacionEdicion As DateTime = Now
    Dim IDRegistroModificado As Integer = 0
    Dim logRecargarRegistro As Boolean = True

    Dim validafiltro,
        disparafocus,
        disparaguardar,
        esnuevo,
        ESTADO_EDICION As Boolean

    Dim mdblCantidad, CANTIDADLIQ As Double
    Private _mlogDuplicando As Boolean = False
#End Region

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New BolsaDomainContext()
            dcProxy1 = New BolsaDomainContext()
            objProxy = New BolsaDomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext()
            mdcProxyUtilidad02 = New UtilidadesDomainContext()
            objProxy1 = New UtilidadesDomainContext()
        Else
            dcProxy = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxy = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            mdcProxyUtilidad02 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            objProxy1 = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.LiquidacionesOFFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesOF, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerLiquidacionesOFPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesOFPorDefecto_Completed, "Default")
                objProxy1.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "LiquidacionesOFViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#Region "Resultados Asincrónicos"
#Region "Tablas Padres"
    Private Sub TerminoTraerLiquidacionesOFPorDefecto_Completed(ByVal lo As LoadOperation(Of LiquidacionesOF))
        Try
            If Not lo.HasError Then
                LiquidacionesOFPorDefecto = lo.Entities.FirstOrDefault
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la LiquidacionesOF por defecto", _
                                                 Me.ToString(), "TerminoTraerLiquidacionesOFPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "TerminoTraerLiquidacionesOFPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoTraerLiquidacionesOF(ByVal lo As LoadOperation(Of LiquidacionesOF))
        Try
            If Not lo.HasError Then
                If lo.UserState = "update" Or lo.UserState = "insert" Then
                    logRecargarRegistro = False
                End If

                ListaLiquidacionesOF = dcProxy.LiquidacionesOFs

                logRecargarRegistro = True

                If dcProxy.LiquidacionesOFs.Count > 0 Then
                    If lo.UserState = "insert" Then
                        LiquidacionesOFSelected = _ListaLiquidacionesOF.First
                    ElseIf lo.UserState = "update" Then
                        If _ListaLiquidacionesOF.Where(Function(i) i.ID = IDRegistroModificado).Count > 0 Then
                            LiquidacionesOFSelected = _ListaLiquidacionesOF.Where(Function(i) i.ID = IDRegistroModificado).Last
                        Else
                            LiquidacionesOFSelected = _ListaLiquidacionesOF.Last
                        End If
                    End If
                Else
                    If lo.UserState = "Busqueda" Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de LiquidacionesOF", _
                                                 Me.ToString(), "TerminoTraerLiquidacionesOF", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el Metodo TerminoTraerLiquidacionesOF", _
                                                 Me.ToString(), "TerminoTraerLiquidacionesOF", Application.Current.ToString(), Program.Maquina, lo.Error)
        End Try
    End Sub
    Private Sub TerminotraerliquidacinesOFValidar(ByVal lo As LoadOperation(Of LiquidacionesOFConsultar))
        Try
            If Not lo.HasError Then
                ListaLiquidacionesOFvalidar = dcProxy.LiquidacionesOFConsultars.ToList

                If ListaLiquidacionesOFvalidar.Count = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("No existe la orden con esas caracteristicas", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    LiquidacionesOFSelected.IDOrden = Nothing
                    Exit Sub
                End If

                If ListaLiquidacionesOFvalidar.First.Estado.Equals("A") Then
                    A2Utilidades.Mensajes.mostrarMensaje("La orden está cancelada. No es posible ingresar la liquidación  ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    LiquidacionesOFSelected.IDOrden = Nothing
                    Exit Sub
                ElseIf ListaLiquidacionesOFvalidar.First.Estado.Equals("C") Then
                    A2Utilidades.Mensajes.mostrarMensaje("La orden está cumplida. No es posible ingresar la liquidación  ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If Not duplica Then
                        LiquidacionesOFSelected.IDOrden = Nothing
                    Else
                        IsBusy = False
                        duplica = False
                    End If
                    Exit Sub
                End If
                habilitaradio = False
                If Not duplica Then
                    mlogRepo = ListaLiquidacionesOFvalidar.First.Repo
                    clientes.Comitente = ListaLiquidacionesOFvalidar.First.IDComitente.Trim
                    clientes.NombreCliente = ListaLiquidacionesOFvalidar.First.Comitente
                    clientesOrdenantes.Ordenante = ListaLiquidacionesOFvalidar.First.IDOrdenante.Trim
                    clientesOrdenantes.NombreClienteOrdenante = ListaLiquidacionesOFvalidar.First.Ordenante
                    Especie.IDEspecie = ListaLiquidacionesOFvalidar.First.IDEspecie
                    Especie.NombreEspecie = ListaLiquidacionesOFvalidar.First.Especie
                    LiquidacionesOFSelected.IDComitente = ListaLiquidacionesOFvalidar.First.IDComitente
                    LiquidacionesOFSelected.IDOrdenante = ListaLiquidacionesOFvalidar.First.IDOrdenante
                    LiquidacionesOFSelected.IDEspecie = ListaLiquidacionesOFvalidar.First.IDEspecie
                    LiquidacionesOFSelected.UBICACIONTITULO = ListaLiquidacionesOFvalidar.First.UBICACIONTITULO
                    LiquidacionesOFSelected.FactorComisionPactada = Format(ListaLiquidacionesOFvalidar.First.ComisionPactada / 100, "#0.0#####")
                    LiquidacionesOFSelected.Emision = ListaLiquidacionesOFvalidar.First.Emision
                    LiquidacionesOFSelected.Vencimiento = ListaLiquidacionesOFvalidar.First.Vencimiento
                    LiquidacionesOFSelected.Modalidad = ListaLiquidacionesOFvalidar.First.Modalidad
                    LiquidacionesOFSelected.TasaDescuento = ListaLiquidacionesOFvalidar.First.TasaInicial
                    LiquidacionesOFSelected.TasaEfectiva = IIf(Not IsNothing(ListaLiquidacionesOFvalidar.First.TasaNominal), ListaLiquidacionesOFvalidar.First.TasaNominal, 0.0)
                    If ListaLiquidacionesOFvalidar.First.Tipo.Equals("C") Or ListaLiquidacionesOFvalidar.First.Tipo.Equals("R") Then
                        LiquidacionesOFSelected.Precio = IIf(Not IsNothing(ListaLiquidacionesOFvalidar.First.Inferior), ListaLiquidacionesOFvalidar.First.Inferior, 0.0)
                    Else
                        LiquidacionesOFSelected.Precio = IIf(Not IsNothing(ListaLiquidacionesOFvalidar.First.Superior), ListaLiquidacionesOFvalidar.First.Superior, 0.0)

                    End If
                    LiquidacionesOFSelected.Swap = "N"
                    LiquidacionesOFSelected.Reinversion = "N"
                    LiquidacionesOFSelected.AutoRetenedor = "N"
                    LiquidacionesOFSelected.Certificacion = "N"
                    LiquidacionesOFSelected.ConstanciaEnajenacion = "N"
                    LiquidacionesOFSelected.RepoTitulo = "N"
                    LiquidacionesOFSelected.Sujeto = "N"
                    LiquidacionesOFSelected.IDBaseDias = "N"
                    LiquidacionesOFSelected.IDBolsa = BOLSACOLOMBIA
                End If
                SaldoOrden()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de LiquidacionesOF", _
                                                 Me.ToString(), "TerminotraerliquidacinesOFValidar", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el Metodo TerminotraerliquidacinesOFValidar", _
                                                 Me.ToString(), "TerminotraerliquidacinesOFValidar", Application.Current.ToString(), Program.Maquina, lo.Error)
        End Try
    End Sub

    Private Sub TerminartraerCantidadOF(ByVal lo As LoadOperation(Of consultarcantidadOF))
        If Not lo.HasError Then

            consultarcantidadesOF = dcProxy.consultarcantidadOFs.First
            If Not duplica Then
                'cuando es edicion es esta misma formula restandole liquidacionesselected.cantidad
                If ESTADO_EDICION Then
                    mcursaldoOrden = consultarcantidadesOF.CantidadOrden - (consultarcantidadesOF.CantidadLiq - LiquidacionesOFSelected.Cantidad)
                    ESTADO_EDICION = False
                    Exit Sub
                Else
                    LiquidacionesOFSelected.Cantidad = consultarcantidadesOF.CantidadOrden - consultarcantidadesOF.CantidadLiq
                    mcursaldoOrden = LiquidacionesOFSelected.Cantidad
                End If

                If LiquidacionesOFSelected.Precio = 0 Then
                    LiquidacionesOFSelected.Transaccion_cur = ListaLiquidacionesOFvalidar.First.Cantidad
                    LiquidacionesOFSelected.SubTotalLiq = LiquidacionesOFSelected.Transaccion_cur
                    LiquidacionesOFSelected.TotalLiq = LiquidacionesOFSelected.Transaccion_cur
                Else
                    If LiquidacionesOFSelected.ClaseOrden.Equals("A") Then
                        LiquidacionesOFSelected.Transaccion_cur = LiquidacionesOFSelected.Precio * LiquidacionesOFSelected.Cantidad
                    Else
                        LiquidacionesOFSelected.Transaccion_cur = LiquidacionesOFSelected.Precio * LiquidacionesOFSelected.Cantidad / 100
                    End If
                    LiquidacionesOFSelected.SubTotalLiq = LiquidacionesOFSelected.Transaccion_cur
                    LiquidacionesOFSelected.TotalLiq = LiquidacionesOFSelected.Transaccion_cur

                End If

                If Not IsNothing(ListaLiquidacionesOFvalidar.First.ComisionPactada) Then
                    If Not IsNothing(LiquidacionesOFSelected.Transaccion_cur) Then
                        LiquidacionesOFSelected.Comision = LiquidacionesOFSelected.Transaccion_cur * LiquidacionesOFSelected.FactorComisionPactada
                        If ListaLiquidacionesOFvalidar.First.Tipo.Equals("C") Or ListaLiquidacionesOFvalidar.First.Tipo.Equals("R") Then
                            LiquidacionesOFSelected.SubTotalLiq = LiquidacionesOFSelected.SubTotalLiq + LiquidacionesOFSelected.Comision
                            LiquidacionesOFSelected.TotalLiq = LiquidacionesOFSelected.TotalLiq + LiquidacionesOFSelected.Comision
                        Else
                            LiquidacionesOFSelected.SubTotalLiq = LiquidacionesOFSelected.SubTotalLiq - LiquidacionesOFSelected.Comision
                            LiquidacionesOFSelected.TotalLiq = LiquidacionesOFSelected.TotalLiq - LiquidacionesOFSelected.Comision
                        End If
                    End If
                Else
                    LiquidacionesOFSelected.Comision = 0
                End If
                If Not IsNothing(ListaLiquidacionesOFvalidar.First.Objeto) Then
                    If Not ListaLiquidacionesOFvalidar.First.Ordinaria And ListaLiquidacionesOFvalidar.First.Objeto.Equals("CRR") Then
                        'HABILITAR ESTAS DOS  PROPIEDADES
                        habilitaparciald = True
                    End If
                End If
            End If
            NuevoReceptorliq()
            habilitamanualsi = True
            If _mlogDuplicando Then
                If LiquidacionesOFSelected.Plaza = "LOC" Then
                    HabilitarComisionistaLocal = True
                    HabilitarComisionistaOtraPlaza = False
                Else
                    HabilitarComisionistaLocal = False
                    HabilitarComisionistaOtraPlaza = True
                End If
                _mlogDuplicando = False
            Else
                HabilitarComisionistaLocal = False
                HabilitarComisionistaOtraPlaza = False
            End If
            HabilitarFechaLiquidacion = True
            HabilitarFechaVendido = False
            HabilitarFechaTitulo = True
            HabilitarFechaEfectivo = True

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Liquidacione ", _
                                             Me.ToString(), "TerminartraerCantidad", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminotraerreceptoresOFliq(ByVal lo As LoadOperation(Of ReceptoresOrdenesOF))
        Try
            If Not lo.HasError Then
                ListaReceptoresOrdenesOF = dcProxy.ReceptoresOrdenesOFs
                BeneficiariosOrdenesOF()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la LiquidacionesOF ", _
                                                 Me.ToString(), "TerminotraerreceptoresOFliq", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el Metodo TerminotraerreceptoresOFliq", _
                                                 Me.ToString(), "TerminotraerreceptoresOFliq", Application.Current.ToString(), Program.Maquina, lo.Error)
        End Try

    End Sub
    Private Sub TerminoTraerBeneficiariosOFliq(ByVal lo As LoadOperation(Of BeneficiariosOrdenesOF))
        Try
            If Not lo.HasError Then
                ListaBeneficiariosOrdenesOF = dcProxy.BeneficiariosOrdenesOFs
                EspecieliqOF()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de BeneficiariosOrdenesOF", _
                                                 Me.ToString(), "TerminoTraerBeneficiariosOFliq", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el Metodo TerminoTraerBeneficiariosOFliq", _
                                                 Me.ToString(), "TerminoTraerBeneficiariosOFliq", Application.Current.ToString(), Program.Maquina, lo.Error)
        End Try
    End Sub
    Private Sub TerminoTraerEspeciesliqOF(ByVal lo As LoadOperation(Of EspeciesLiquidacionesOF))
        Try
            If Not lo.HasError Then
                ListaEspeciesLiquidacionesOF = dcProxy.EspeciesLiquidacionesOFs
                If duplica Then
                    NuevoDuplicado()
                    duplica = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EspeciesLiquidacionesOF", _
                                                 Me.ToString(), "TerminoTraerEspeciesliqOF", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el Metodo TerminoTraerEspeciesliqOF", _
                                                 Me.ToString(), "TerminoTraerEspeciesliqOF", Application.Current.ToString(), Program.Maquina, lo.Error)
        End Try

    End Sub
    Private Sub TerminotraerOrdenesliqOF(ByVal lo As LoadOperation(Of LiquidacionesOrdenesOF))
        If Not lo.HasError Then
            Try
                LiquidacionesOrdenOF = dcProxy.LiquidacionesOrdenesOFs.First
                If IsNothing(LiquidacionesOrdenOF.Ordinaria) Then
                    LiquidacionesOFSelected.Ordinaria = False
                Else
                    LiquidacionesOFSelected.Ordinaria = LiquidacionesOrdenOF.Ordinaria
                End If

                If IsNothing(LiquidacionesOrdenOF.Objeto) Then
                    LiquidacionesOFSelected.ObjetoOrdenExtraordinaria = Nothing
                Else
                    LiquidacionesOFSelected.ObjetoOrdenExtraordinaria = LiquidacionesOrdenOF.Objeto
                End If



                If IsNothing(LiquidacionesOrdenOF.Objeto) Then
                    LiquidacionesOFSelected.NumPadre = Nothing
                Else
                    If LiquidacionesOrdenOF.Objeto.Equals("CRR") Then
                        LiquidacionesOFSelected.NumPadre = LiquidacionesOFSelected.ID
                    Else
                        LiquidacionesOFSelected.NumPadre = Nothing
                    End If
                End If
                Guardarliq()
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarMensaje(ex.Message.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End Try

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EspeciesLiquidaciones", _
                                             Me.ToString(), "TerminoTraerEspeciesliq", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        Try
            If obj.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
            Else
                fechaCierre = obj.Value
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el Metodo consultarFechaCierreCompleted", _
                                             Me.ToString(), "consultarFechaCierreCompleted", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub Terminotraervalidar(ByVal lo As InvokeOperation(Of Integer))
        Try
            If Not lo.HasError Then
                numeroliq = lo.Value
                If numeroliq > 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("La liquidación " + LiquidacionesOFSelected.ID.ToString + " con el parcial " + LiquidacionesOFSelected.Parcial.ToString + " y fecha " + LiquidacionesOFSelected.Liquidacion.ToShortDateString + " ya existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    If LiquidacionesOFSelected.ClaseOrden.Equals("C") Then
                        Datostitulo = True
                        habilitaemision = True
                        habilitamanualdias = True
                        habilitavencimiento = True
                    End If

                    If lo.UserState = "ACTUALIZAR" Then
                        ActualizarRegistroDespuesValidadoDatos()
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidacionesOF", _
                                                 Me.ToString(), "Terminotraervalidar", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidacionesOF", _
                                                 Me.ToString(), "Terminotraervalidar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoTraerAplazamientosLiquidacionesvalor(ByVal lo As InvokeOperation(Of System.Nullable(Of Double)))
        Try
            If Not lo.HasError Then
                'LiquidacionesOFSelected.Precio = LiquidacionesOFSelected.Transaccion_cur / LiquidacionesOFSelected.Cantidad / lo.Value
                LiquidacionesOFSelected.Transaccion_cur = LiquidacionesOFSelected.Precio * LiquidacionesOFSelected.Cantidad * lo.Value
                LiquidacionesOFSelected.SubTotalLiq = LiquidacionesOFSelected.Transaccion_cur
                LiquidacionesOFSelected.TotalLiq = LiquidacionesOFSelected.Transaccion_cur

                Denominacionespecie()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones", _
                                                 Me.ToString(), "TerminoTraerAplazamientosLiquidacionesvalor", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer aplazamientos", Me.ToString(), "TerminoTraerAplazamientosLiquidacionesvalor", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    Private Sub TerminoTraernombre(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            If lo.Value.Equals("PESOS") Then
                LiquidacionesOFSelected.Comision = IIf(IsNothing(LiquidacionesOFSelected.Cantidad), 0, LiquidacionesOFSelected.Cantidad) * IIf(IsNothing(LiquidacionesOFSelected.FactorComisionPactada), 0, LiquidacionesOFSelected.FactorComisionPactada)
            Else
                LiquidacionesOFSelected.Comision = IIf(IsNothing(LiquidacionesOFSelected.Transaccion_cur), 0, LiquidacionesOFSelected.Transaccion_cur) * IIf(IsNothing(LiquidacionesOFSelected.FactorComisionPactada), 0, LiquidacionesOFSelected.FactorComisionPactada)
            End If
            dcProxy.verificadblIvacomision(Nothing, Program.Usuario, Program.HashConexion, AddressOf Terminotraeriva, Nothing)
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones", _
                                             Me.ToString(), "TerminoTraernombre", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub Terminotraeriva(ByVal lo As InvokeOperation(Of System.Nullable(Of Double)))
        If Not lo.HasError Then
            If Userstate <> String.Empty Then 'JBT 20130125  cambio realizado para evitar error cuando se daba doble clik en el boton grabar
                Exit Sub
            End If
            If LiquidacionesOFSelected.Comision > 0 And lo.Value > 0 Then
                LiquidacionesOFSelected.ValorIva = ((LiquidacionesOFSelected.Comision) * (lo.Value / 100) - 0.005)
            Else
                LiquidacionesOFSelected.ValorIva = 0
            End If
            If LiquidacionesOFSelected.Tipo.Equals("C") Or LiquidacionesOFSelected.Tipo.Equals("R") Then
                LiquidacionesOFSelected.SubTotalLiq = LiquidacionesOFSelected.SubTotalLiq + LiquidacionesOFSelected.Comision + LiquidacionesOFSelected.ValorIva
                LiquidacionesOFSelected.TotalLiq = LiquidacionesOFSelected.TotalLiq + LiquidacionesOFSelected.Comision + LiquidacionesOFSelected.ValorIva
            Else
                LiquidacionesOFSelected.SubTotalLiq = LiquidacionesOFSelected.SubTotalLiq - LiquidacionesOFSelected.Comision - LiquidacionesOFSelected.ValorIva
                LiquidacionesOFSelected.TotalLiq = LiquidacionesOFSelected.TotalLiq - LiquidacionesOFSelected.Comision - LiquidacionesOFSelected.ValorIva
            End If
            If Not IsNothing(LiquidacionesOFSelected.ServBolsaFijo) And trasladar.Traslado Then
                If LiquidacionesOFSelected.Tipo.Equals("C") Or LiquidacionesOFSelected.Tipo.Equals("R") Then
                    LiquidacionesOFSelected.TotalLiq = LiquidacionesOFSelected.TotalLiq + LiquidacionesOFSelected.ServBolsaFijo
                Else
                    LiquidacionesOFSelected.TotalLiq = LiquidacionesOFSelected.TotalLiq - LiquidacionesOFSelected.ServBolsaFijo
                End If
            End If

            If Not IsNothing(LiquidacionesOFSelected.Retencion) Then
                LiquidacionesOFSelected.TotalLiq = LiquidacionesOFSelected.TotalLiq + LiquidacionesOFSelected.Retencion

            End If

            If Not IsNothing(LiquidacionesOFSelected.Intereses) Then
                LiquidacionesOFSelected.TotalLiq = LiquidacionesOFSelected.TotalLiq + LiquidacionesOFSelected.Intereses

            End If


            If Not IsNothing(LiquidacionesOFSelected.ValorTraslado) Then
                LiquidacionesOFSelected.TotalLiq = LiquidacionesOFSelected.TotalLiq + LiquidacionesOFSelected.ValorTraslado
            End If

            If disparaguardar = True And disparafocus = True Then
                If logNuevoRegistro Then
                    Userstate = "insert"
                Else
                    Userstate = "update"
                End If
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Userstate)

            End If

            disparafocus = False
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones", _
                                             Me.ToString(), "TerminoTraerAplazamientosLiquidacionesvalor", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub TerminoactualizarordenestadoOF(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            If Not lo.Value = String.Empty Then
                Dim parametrosliq = Split(lo.UserState, ",")
                dcProxy.ActualizaordenestadocumplidaOF(parametrosliq(0), parametrosliq(1), CInt(parametrosliq(2)), 0, "P", Now, Program.Usuario, Program.HashConexion, AddressOf TerminoactualizarodencumplidaOF, Nothing)
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidacionesOF", _
                                             Me.ToString(), "TerminoactualizarordenestadoOF", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub TerminoactualizarodencumplidaOF(ByVal lo As InvokeOperation(Of Integer))
        If Not lo.HasError Then
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidacionesOF", _
                                             Me.ToString(), "TerminoactualizarodencumplidaOF", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub TerminoVerificarCumplimientoOrderliq(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then

            If Userstate <> String.Empty Then 'JBT 20130125  cambio realizado para evitar error cuando se daba doble clik en el boton grabar
                Exit Sub
            End If
            Dim pdblCantidadLiq = Split(lo.Value, ",")
            If Not ListaLiquidacionesOF.Contains(LiquidacionesOFSelected) Then

                If Not IsNothing(pdblCantidadLiq(0)) Then
                    CANTIDADLIQ = (pdblCantidadLiq(0) + LiquidacionesOFSelected.Cantidad)
                Else
                    CANTIDADLIQ = LiquidacionesOFSelected.Cantidad
                End If
                If Not ListaLiquidacionesOF.Contains(LiquidacionesOFSelected) Or mlogCantidad Then
                    If CANTIDADLIQ = pdblCantidadLiq(1) Then
                        dcProxy.ActualizaordenestadocumplidaOF(LiquidacionesOFSelected.Tipo, LiquidacionesOFSelected.ClaseOrden, LiquidacionesOFSelected.IDOrden, 0, "C", LiquidacionesOFSelected.Liquidacion, Program.Usuario, Program.HashConexion, AddressOf TerminoactualizarodencumplidaOF, Nothing)
                    End If
                End If
                'validaactualizarespecie = True
                ListaLiquidacionesOF.Add(LiquidacionesOFSelected)
            Else
                If mlogCantidad Then
                    If Not IsNothing(pdblCantidadLiq(0)) Then
                        CANTIDADLIQ = (pdblCantidadLiq(0) - mdblCantidad + LiquidacionesOFSelected.Cantidad)
                    Else
                        CANTIDADLIQ = LiquidacionesOFSelected.Cantidad
                    End If
                End If
            End If

            If mlogCantidad And ((CANTIDADLIQ < pdblCantidadLiq(1)) Or (CANTIDADLIQ > pdblCantidadLiq(1))) Then
                dcProxy.ActualizaordenestadocumplidaOF(LiquidacionesOFSelected.Tipo, LiquidacionesOFSelected.ClaseOrden, LiquidacionesOFSelected.IDOrden, 0, "P", LiquidacionesOFSelected.Liquidacion, Program.Usuario, Program.HashConexion, AddressOf TerminoactualizarodencumplidaOF, Nothing)
            End If

            If Not disparafocus Then
                If logNuevoRegistro Then
                    Userstate = "insert"
                Else
                    Userstate = "update"
                End If
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Userstate)
                'disparaguardar = False

            End If
            disparaguardar = True
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del Saldo de la Orden", _
                     Me.ToString(), "TerminoVerificarCumplimientoOrderliq", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
            IsBusy = False
        End If
    End Sub


#End Region
#Region "Tablas Hijas"
    Private Sub TerminoTraerReceptoresOrdenesOF(ByVal lo As LoadOperation(Of ReceptoresOrdenesOF))
        If Not lo.HasError Then
            ListaReceptoresOrdenesOF = dcProxy.ReceptoresOrdenesOFs
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ReceptoresOrdenesOF", _
                                             Me.ToString(), "TerminoTraerReceptoresOrdenesOF", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub TerminoTraerBeneficiariosOrdenesOF(ByVal lo As LoadOperation(Of BeneficiariosOrdenesOF))
        If Not lo.HasError Then
            ListaBeneficiariosOrdenesOF = dcProxy.BeneficiariosOrdenesOFs
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de BeneficiariosOrdenesOF", _
                                             Me.ToString(), "TerminoTraerBeneficiariosOrdenesOF", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub TerminoTraerEspeciesLiquidacionesOF(ByVal lo As LoadOperation(Of EspeciesLiquidacionesOF))
        If Not lo.HasError Then
            ListaEspeciesLiquidacionesOF = dcProxy.EspeciesLiquidacionesOFs
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EspeciesLiquidacionesOF", _
                                             Me.ToString(), "TerminoTraerEspeciesLiquidacionesOF", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub TerminoTraerAplazamientosLiquidaciones(ByVal lo As LoadOperation(Of AplazamientosLiquidacionesOF))
        If Not lo.HasError Then
            ListaAplazamientosLiquidacionesOF = dcProxy.AplazamientosLiquidacionesOFs
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de AplazamientosLiquidaciones", _
                                             Me.ToString(), "TerminoTraerAplazamientosLiquidaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

    'Private Sub TerminoTraerCustodiasLiquidaciones(ByVal lo As LoadOperation(Of CustodiasLiquidacione))
    '    If Not lo.HasError Then
    '        ListaCustodiasLiquidacionesOF = dcProxy.CustodiasLiquidacionesOFs
    '    Else
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de CustodiasLiquidaciones", _
    '                                         Me.ToString(), "TerminoTraerCustodiasLiquidaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
    '        lo.MarkErrorAsHandled()
    '    End If
    'End Sub

#End Region

#End Region
#Region "Propiedades"
    Private _ListaLiquidacionesOF As EntitySet(Of LiquidacionesOF)
    Public Property ListaLiquidacionesOF() As EntitySet(Of LiquidacionesOF)
        Get
            Return _ListaLiquidacionesOF
        End Get
        Set(ByVal value As EntitySet(Of LiquidacionesOF))
            _ListaLiquidacionesOF = value

            MyBase.CambioItem("ListaLiquidacionesOF")
            MyBase.CambioItem("ListaLiquidacionesOFPaged")
            If Not IsNothing(value) Then
                If logRecargarRegistro Then
                    If IsNothing(LiquidacionesOFAnterior) Then
                        LiquidacionesOFSelected = _ListaLiquidacionesOF.FirstOrDefault
                    Else
                        LiquidacionesOFSelected = LiquidacionesOFAnterior
                    End If
                End If
            End If
        End Set
    End Property
    Public ReadOnly Property ListaLiquidacionesOFPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaLiquidacionesOF) Then
                Dim view = New PagedCollectionView(_ListaLiquidacionesOF)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private WithEvents _LiquidacionesOFSelected As LiquidacionesOF
    Public Property LiquidacionesOFSelected() As LiquidacionesOF
        Get
            Return _LiquidacionesOFSelected
        End Get
        Set(ByVal value As LiquidacionesOF)
            Try
                _LiquidacionesOFSelected = value
                If selected = False Then
                    If Not IsNothing(LiquidacionesOFSelected) Then
                        If Not IsNothing(LiquidacionesOFSelected.Traslado) Then
                            If LiquidacionesOFSelected.Traslado.Equals("S") Then
                                trasladar.Traslado = 1
                            Else
                                trasladar.Traslado = 0
                            End If
                        End If
                    End If
                    If Not value Is Nothing Then
                        If Not IsNothing(LiquidacionesOFSelected.IDOrden) And LiquidacionesOFSelected.IDOrden <> 0 Then
                            VeriOrdenes.NumeroOrden = CInt(Mid$(CStr(LiquidacionesOFSelected.IDOrden), 1, 4))
                            VeriOrdenes.NumeroOrden1 = Mid$(CStr(LiquidacionesOFSelected.IDOrden), 5)
                        End If
                        numeroliq = 0

                        clientes.Comitente = value.IDComitente.Trim
                        clientesOrdenantes.Ordenante = value.IDOrdenante.Trim
                        Especie.IDEspecie = value.IDEspecie
                        buscarItem("clientes")
                        buscarItem("clientesOrdenantes")
                        buscarItem("especies")

                        dcProxy.ReceptoresOrdenesOFs.Clear()
                        dcProxy.BeneficiariosOrdenesOFs.Clear()
                        dcProxy.EspeciesLiquidacionesOFs.Clear()
                        dcProxy.AplazamientosLiquidacionesOFs.Clear()
                        dcProxy.CustodiasLiquidacionesOFs.Clear()

                        If Not esnuevo Then
                            dcProxy.Load(dcProxy.Traer_ReceptoresOrdenesOF_LiquidacionesOFQuery(LiquidacionesOFSelected.Tipo, LiquidacionesOFSelected.ClaseOrden, LiquidacionesOFSelected.IDOrden, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresOrdenesOF, "FiltroReceptoresOrdenesOF")
                            dcProxy.Load(dcProxy.Traer_BeneficiariosOrdenesOF_LiquidacionesOFQuery(LiquidacionesOFSelected.IDLiquidacionesOF, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBeneficiariosOrdenesOF, "FiltroBeneficiariosOrdenes")
                            dcProxy.Load(dcProxy.Traer_EspeciesLiquidacionesOFQuery(LiquidacionesOFSelected.ID, LiquidacionesOFSelected.Parcial, LiquidacionesOFSelected.Liquidacion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesLiquidacionesOF, "FiltroEspeciesLiquidaciones")
                            dcProxy.Load(dcProxy.Traer_AplazamientosLiquidacionesOFQuery(LiquidacionesOFSelected.Liquidacion, LiquidacionesOFSelected.ID, LiquidacionesOFSelected.Parcial, LiquidacionesOFSelected.Tipo, LiquidacionesOFSelected.ClaseOrden, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAplazamientosLiquidaciones, "FiltroAplazamientosLiquidaciones")
                            'dcProxy.Load(dcProxy.Traer_CustodiasLiquidacionesQuery(LiquidacionesOFSelected.IDComisionista,
                            '                                                       LiquidacionesOFSelected.IDSucComisionista,
                            '                                                       LiquidacionesOFSelected.IDComitente,
                            '                                                       LiquidacionesOFSelected.IDEspecie,
                            '                                                       LiquidacionesOFSelected.Tipo,
                            '                                                       LiquidacionesOFSelected.ClaseOrden,
                            '                                                       LiquidacionesOFSelected.ID,
                            '                                                       LiquidacionesOFSelected.Parcial,
                            '                                                       LiquidacionesOFSelected.Liquidacion,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodiasLiquidaciones, "FiltroCustodiasLiquidaciones")
                        End If
                        esnuevo = False

                    End If

                End If
                selected = False
                MyBase.CambioItem("LiquidacionesOFSelected")
            Catch ex As Exception

            End Try

        End Set
    End Property
    Private _TabSeleccionada As Integer = 0
    Public Property TabSeleccionada
        Get
            Return _TabSeleccionada
        End Get
        Set(ByVal value)
            _TabSeleccionada = value
            MyBase.CambioItem("TabSeleccionada")

        End Set
    End Property
    Private _clientes As Clientesclase = New Clientesclase
    Public Property clientes() As Clientesclase
        Get
            Return _clientes
        End Get
        Set(ByVal value As Clientesclase)
            _clientes = value
            MyBase.CambioItem("clientes")
        End Set
    End Property
    Private _clientesOrdenantes As ClientesOrdenantesclase = New ClientesOrdenantesclase
    Public Property clientesOrdenantes() As ClientesOrdenantesclase
        Get
            Return _clientesOrdenantes
        End Get
        Set(ByVal value As ClientesOrdenantesclase)
            _clientesOrdenantes = value
            MyBase.CambioItem("_clientesOrdenantes")
        End Set
    End Property
    Private _Especie As Especieclase = New Especieclase
    Public Property Especie() As Especieclase
        Get
            Return _Especie
        End Get
        Set(ByVal value As Especieclase)
            _Especie = value
            MyBase.CambioItem("Especie")
        End Set
    End Property
    Private _TabSeleccionadaFinanciero As Integer = 0
    Public Property TabSeleccionadaFinanciero
        Get
            Return _TabSeleccionadaFinanciero
        End Get
        Set(ByVal value)
            _TabSeleccionadaFinanciero = value
            MyBase.CambioItem("TabSeleccionadaFinanciero")
        End Set
    End Property
    Private _trasladar As traslado = New traslado
    Public Property trasladar As traslado
        Get
            Return _trasladar
        End Get
        Set(ByVal value As traslado)
            _trasladar = value
            MyBase.CambioItem("trasladar")
        End Set
    End Property

    Private _habilitaboton As Boolean = True
    Public Property habilitaboton As Boolean
        Get
            Return _habilitaboton
        End Get
        Set(ByVal value As Boolean)
            _habilitaboton = value
            MyBase.CambioItem("habilitaboton")
        End Set
    End Property

    Private _habilitamanualsi As Boolean
    Public Property habilitamanualsi As Boolean
        Get
            Return _habilitamanualsi
        End Get
        Set(ByVal value As Boolean)
            _habilitamanualsi = value
            MyBase.CambioItem("habilitamanualsi")
        End Set
    End Property

    Private _habilitavencimiento As Boolean
    Public Property habilitavencimiento As Boolean
        Get
            Return _habilitavencimiento
        End Get
        Set(ByVal value As Boolean)
            _habilitavencimiento = value
            MyBase.CambioItem("habilitavencimiento")
        End Set
    End Property
    Private _habilitaradio As Boolean
    Public Property habilitaradio As Boolean
        Get
            Return _habilitaradio
        End Get
        Set(ByVal value As Boolean)
            _habilitaradio = value
            MyBase.CambioItem("habilitaradio")
        End Set
    End Property

    Private _habilitaparciald As Boolean
    Public Property habilitaparciald As Boolean
        Get
            Return _habilitaparciald
        End Get
        Set(ByVal value As Boolean)
            _habilitaparciald = value
            MyBase.CambioItem("habilitaparciald")
        End Set
    End Property

    Private _habilitamanualdias As Boolean
    Public Property habilitamanualdias As Boolean
        Get
            Return _habilitamanualdias
        End Get
        Set(ByVal value As Boolean)
            _habilitamanualdias = value
            MyBase.CambioItem("habilitamanualdias")
        End Set
    End Property

    Private _HabilitarComisionistaLocal As Boolean = False
    Public Property HabilitarComisionistaLocal() As Boolean
        Get
            Return _HabilitarComisionistaLocal
        End Get
        Set(ByVal value As Boolean)
            _HabilitarComisionistaLocal = value
            MyBase.CambioItem("HabilitarComisionistaLocal")
        End Set
    End Property

    Private _HabilitarComisionistaOtraPlaza As Boolean = False
    Public Property HabilitarComisionistaOtraPlaza As Boolean
        Get
            Return _HabilitarComisionistaOtraPlaza
        End Get
        Set(ByVal value As Boolean)
            _HabilitarComisionistaOtraPlaza = value
            MyBase.CambioItem("HabilitarComisionistaOtraPlaza")
        End Set
    End Property
    Private _HabilitarFechaLiquidacion As Boolean = False
    Public Property HabilitarFechaLiquidacion As Boolean
        Get
            Return _HabilitarFechaLiquidacion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFechaLiquidacion = value
            MyBase.CambioItem("HabilitarFechaLiquidacion")
        End Set
    End Property

    Private _HabilitarFechaVendido As Boolean = False
    Public Property HabilitarFechaVendido As Boolean
        Get
            Return _HabilitarFechaVendido
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFechaVendido = value
            MyBase.CambioItem("HabilitarFechaVendido")
        End Set
    End Property

    Private _HabilitarFechaTitulo As Boolean = False
    Public Property HabilitarFechaTitulo As Boolean
        Get
            Return _HabilitarFechaTitulo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFechaTitulo = value
            MyBase.CambioItem("HabilitarFechaTitulo")
        End Set
    End Property

    Private _HabilitarFechaEfectivo As Boolean = False
    Public Property HabilitarFechaEfectivo As Boolean
        Get
            Return _HabilitarFechaEfectivo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFechaEfectivo = value
            MyBase.CambioItem("HabilitarFechaEfectivo")
        End Set
    End Property


    Private WithEvents _VeriOrdenes As VeriOrden = New VeriOrden
    Public Property VeriOrdenes As VeriOrden
        Get
            Return _VeriOrdenes
        End Get
        Set(ByVal value As VeriOrden)
            _VeriOrdenes = value
            MyBase.CambioItem("VeriOrdenes")
        End Set
    End Property

    Private _VmLiquidaciones As LiquidacionesOFView
    Public Property VmLiquidaciones As LiquidacionesOFView
        Get
            Return _VmLiquidaciones
        End Get
        Set(ByVal value As LiquidacionesOFView)
            _VmLiquidaciones = value
            MyBase.CambioItem("VmLiquidaciones")
        End Set
    End Property
    Private _Datostitulo As Boolean
    Public Property Datostitulo As Boolean
        Get
            Return _Datostitulo
        End Get
        Set(ByVal value As Boolean)
            _Datostitulo = value
            MyBase.CambioItem("Datostitulo")
        End Set
    End Property
    Private _habilitaemision As Boolean
    Public Property habilitaemision As Boolean
        Get
            Return _habilitaemision
        End Get
        Set(ByVal value As Boolean)
            _habilitaemision = value
            MyBase.CambioItem("habilitaemision")
        End Set
    End Property

#End Region
#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            estadoregistro = True
            'MessageBox.Show("Esta funcionalidad no está disponible.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
            'A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Dim NewLiquidacionesOF As New LiquidacionesOF
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewLiquidacionesOF.IDComisionista = LiquidacionesOFPorDefecto.IDComisionista
            NewLiquidacionesOF.IDSucComisionista = LiquidacionesOFPorDefecto.IDSucComisionista
            NewLiquidacionesOF.ID = LiquidacionesOFPorDefecto.ID
            NewLiquidacionesOF.Parcial = -1
            NewLiquidacionesOF.Tipo = "C"
            NewLiquidacionesOF.ClaseOrden = "A"
            NewLiquidacionesOF.IDEspecie = LiquidacionesOFPorDefecto.IDEspecie
            NewLiquidacionesOF.IDOrden = LiquidacionesOFPorDefecto.IDOrden
            NewLiquidacionesOF.Prefijo = LiquidacionesOFPorDefecto.Prefijo
            NewLiquidacionesOF.IDFactura = LiquidacionesOFPorDefecto.IDFactura
            NewLiquidacionesOF.Facturada = LiquidacionesOFPorDefecto.Facturada
            NewLiquidacionesOF.IDComitente = String.Empty
            NewLiquidacionesOF.IDOrdenante = String.Empty
            NewLiquidacionesOF.IDBolsa = LiquidacionesOFPorDefecto.IDBolsa
            NewLiquidacionesOF.ValBolsa = LiquidacionesOFPorDefecto.ValBolsa
            NewLiquidacionesOF.IDRueda = LiquidacionesOFPorDefecto.IDRueda
            NewLiquidacionesOF.TasaDescuento = LiquidacionesOFPorDefecto.TasaDescuento
            NewLiquidacionesOF.TasaCompraVende = LiquidacionesOFPorDefecto.TasaCompraVende
            NewLiquidacionesOF.Modalidad = LiquidacionesOFPorDefecto.Modalidad
            NewLiquidacionesOF.IndicadorEconomico = LiquidacionesOFPorDefecto.IndicadorEconomico
            NewLiquidacionesOF.PuntosIndicador = LiquidacionesOFPorDefecto.PuntosIndicador
            NewLiquidacionesOF.Plazo = LiquidacionesOFPorDefecto.Plazo
            NewLiquidacionesOF.Liquidacion = Now.Date
            NewLiquidacionesOF.Cumplimiento = Now.Date
            NewLiquidacionesOF.Emision = LiquidacionesOFPorDefecto.Emision
            NewLiquidacionesOF.Vencimiento = LiquidacionesOFPorDefecto.Vencimiento
            NewLiquidacionesOF.OtraPlaza = LiquidacionesOFPorDefecto.OtraPlaza
            NewLiquidacionesOF.Plaza = LiquidacionesOFPorDefecto.Plaza
            NewLiquidacionesOF.IDComisionistaLocal = LiquidacionesOFPorDefecto.IDComisionistaLocal
            NewLiquidacionesOF.IDComisionistaOtraPlaza = LiquidacionesOFPorDefecto.IDComisionistaOtraPlaza
            NewLiquidacionesOF.IDCiudadOtraPlaza = LiquidacionesOFPorDefecto.IDCiudadOtraPlaza
            NewLiquidacionesOF.TasaEfectiva = LiquidacionesOFPorDefecto.TasaEfectiva
            NewLiquidacionesOF.Cantidad = LiquidacionesOFPorDefecto.Cantidad
            NewLiquidacionesOF.Precio = LiquidacionesOFPorDefecto.Precio
            NewLiquidacionesOF.Transaccion = LiquidacionesOFPorDefecto.Transaccion
            NewLiquidacionesOF.SubTotalLiq = LiquidacionesOFPorDefecto.SubTotalLiq
            NewLiquidacionesOF.TotalLiq = LiquidacionesOFPorDefecto.TotalLiq
            NewLiquidacionesOF.Comision = LiquidacionesOFPorDefecto.Comision
            NewLiquidacionesOF.Retencion = LiquidacionesOFPorDefecto.Retencion
            NewLiquidacionesOF.Intereses = LiquidacionesOFPorDefecto.Intereses
            NewLiquidacionesOF.ValorIva = LiquidacionesOFPorDefecto.ValorIva
            NewLiquidacionesOF.DiasIntereses = LiquidacionesOFPorDefecto.DiasIntereses
            NewLiquidacionesOF.FactorComisionPactada = LiquidacionesOFPorDefecto.FactorComisionPactada
            NewLiquidacionesOF.Mercado = LiquidacionesOFPorDefecto.Mercado
            NewLiquidacionesOF.NroTitulo = LiquidacionesOFPorDefecto.NroTitulo
            NewLiquidacionesOF.IDCiudadExpTitulo = LiquidacionesOFPorDefecto.IDCiudadExpTitulo
            NewLiquidacionesOF.PlazoOriginal = LiquidacionesOFPorDefecto.PlazoOriginal
            NewLiquidacionesOF.Aplazamiento = LiquidacionesOFPorDefecto.Aplazamiento
            NewLiquidacionesOF.VersionPapeleta = LiquidacionesOFPorDefecto.VersionPapeleta
            NewLiquidacionesOF.EmisionOriginal = LiquidacionesOFPorDefecto.EmisionOriginal
            NewLiquidacionesOF.VencimientoOriginal = LiquidacionesOFPorDefecto.VencimientoOriginal
            NewLiquidacionesOF.Impresiones = LiquidacionesOFPorDefecto.Impresiones
            NewLiquidacionesOF.FormaPago = LiquidacionesOFPorDefecto.FormaPago
            NewLiquidacionesOF.CtrlImpPapeleta = LiquidacionesOFPorDefecto.CtrlImpPapeleta
            NewLiquidacionesOF.DiasVencimiento = LiquidacionesOFPorDefecto.DiasVencimiento
            NewLiquidacionesOF.PosicionPropia = LiquidacionesOFPorDefecto.PosicionPropia
            NewLiquidacionesOF.Transaccion = LiquidacionesOFPorDefecto.Transaccion
            NewLiquidacionesOF.TipoOperacion = LiquidacionesOFPorDefecto.TipoOperacion
            NewLiquidacionesOF.DiasContado = LiquidacionesOFPorDefecto.DiasContado
            NewLiquidacionesOF.Ordinaria = LiquidacionesOFPorDefecto.Ordinaria
            NewLiquidacionesOF.ObjetoOrdenExtraordinaria = LiquidacionesOFPorDefecto.ObjetoOrdenExtraordinaria
            NewLiquidacionesOF.NumPadre = LiquidacionesOFPorDefecto.NumPadre
            NewLiquidacionesOF.ParcialPadre = LiquidacionesOFPorDefecto.ParcialPadre
            NewLiquidacionesOF.OperacionPadre = LiquidacionesOFPorDefecto.OperacionPadre
            NewLiquidacionesOF.DiasTramo = LiquidacionesOFPorDefecto.DiasTramo
            NewLiquidacionesOF.Vendido = LiquidacionesOFPorDefecto.Vendido
            NewLiquidacionesOF.Vendido = LiquidacionesOFPorDefecto.Vendido
            NewLiquidacionesOF.Manual = True
            NewLiquidacionesOF.ValorTraslado = LiquidacionesOFPorDefecto.ValorTraslado
            NewLiquidacionesOF.ValorBrutoCompraVencida = LiquidacionesOFPorDefecto.ValorBrutoCompraVencida
            NewLiquidacionesOF.AutoRetenedor = LiquidacionesOFPorDefecto.AutoRetenedor
            NewLiquidacionesOF.Sujeto = LiquidacionesOFPorDefecto.Sujeto
            NewLiquidacionesOF.PcRenEfecCompraRet = LiquidacionesOFPorDefecto.PcRenEfecCompraRet
            NewLiquidacionesOF.PcRenEfecVendeRet = LiquidacionesOFPorDefecto.PcRenEfecVendeRet
            NewLiquidacionesOF.Reinversion = LiquidacionesOFPorDefecto.Reinversion
            NewLiquidacionesOF.Swap = LiquidacionesOFPorDefecto.Swap
            NewLiquidacionesOF.NroSwap = LiquidacionesOFPorDefecto.NroSwap
            NewLiquidacionesOF.Certificacion = LiquidacionesOFPorDefecto.Certificacion
            NewLiquidacionesOF.DescuentoAcumula = LiquidacionesOFPorDefecto.DescuentoAcumula
            NewLiquidacionesOF.PctRendimiento = LiquidacionesOFPorDefecto.PctRendimiento
            NewLiquidacionesOF.FechaCompraVencido = LiquidacionesOFPorDefecto.FechaCompraVencido
            NewLiquidacionesOF.PrecioCompraVencido = LiquidacionesOFPorDefecto.PrecioCompraVencido
            NewLiquidacionesOF.ConstanciaEnajenacion = LiquidacionesOFPorDefecto.ConstanciaEnajenacion
            NewLiquidacionesOF.RepoTitulo = LiquidacionesOFPorDefecto.RepoTitulo
            NewLiquidacionesOF.ServBolsaVble = LiquidacionesOFPorDefecto.ServBolsaVble
            NewLiquidacionesOF.ServBolsaFijo = LiquidacionesOFPorDefecto.ServBolsaFijo
            NewLiquidacionesOF.Traslado = LiquidacionesOFPorDefecto.Traslado
            NewLiquidacionesOF.UBICACIONTITULO = LiquidacionesOFPorDefecto.UBICACIONTITULO
            NewLiquidacionesOF.HoraGrabacion = LiquidacionesOFPorDefecto.HoraGrabacion
            NewLiquidacionesOF.OrigenOperacion = LiquidacionesOFPorDefecto.OrigenOperacion
            NewLiquidacionesOF.CodigoOperadorCompra = LiquidacionesOFPorDefecto.CodigoOperadorCompra
            NewLiquidacionesOF.CodigoOperadorVende = LiquidacionesOFPorDefecto.CodigoOperadorVende
            NewLiquidacionesOF.IdentificacionRemate = LiquidacionesOFPorDefecto.IdentificacionRemate
            NewLiquidacionesOF.ModalidaOperacion = LiquidacionesOFPorDefecto.ModalidaOperacion
            NewLiquidacionesOF.IndicadorPrecio = LiquidacionesOFPorDefecto.IndicadorPrecio
            NewLiquidacionesOF.PeriodoExdividendo = LiquidacionesOFPorDefecto.PeriodoExdividendo
            NewLiquidacionesOF.PlazoOperacionRepo = LiquidacionesOFPorDefecto.PlazoOperacionRepo
            NewLiquidacionesOF.ValorCaptacionRepo = LiquidacionesOFPorDefecto.ValorCaptacionRepo
            NewLiquidacionesOF.VolumenCompraRepo = LiquidacionesOFPorDefecto.VolumenCompraRepo
            NewLiquidacionesOF.PrecioNetoFraccion = LiquidacionesOFPorDefecto.PrecioNetoFraccion
            NewLiquidacionesOF.VolumenNetoFraccion = LiquidacionesOFPorDefecto.VolumenNetoFraccion
            NewLiquidacionesOF.CodigoContactoComercial = LiquidacionesOFPorDefecto.CodigoContactoComercial
            NewLiquidacionesOF.NroFraccionOperacion = LiquidacionesOFPorDefecto.NroFraccionOperacion
            NewLiquidacionesOF.IdentificacionPatrimonio1 = LiquidacionesOFPorDefecto.IdentificacionPatrimonio1
            NewLiquidacionesOF.TipoidentificacionCliente2 = LiquidacionesOFPorDefecto.TipoidentificacionCliente2
            NewLiquidacionesOF.NitCliente2 = LiquidacionesOFPorDefecto.NitCliente2
            NewLiquidacionesOF.IdentificacionPatrimonio2 = LiquidacionesOFPorDefecto.IdentificacionPatrimonio2
            NewLiquidacionesOF.TipoIdentificacionCliente3 = LiquidacionesOFPorDefecto.TipoIdentificacionCliente3
            NewLiquidacionesOF.NitCliente3 = LiquidacionesOFPorDefecto.NitCliente3
            NewLiquidacionesOF.IdentificacionPatrimonio3 = LiquidacionesOFPorDefecto.IdentificacionPatrimonio3
            NewLiquidacionesOF.IndicadorOperacion = LiquidacionesOFPorDefecto.IndicadorOperacion
            NewLiquidacionesOF.BaseRetencion = LiquidacionesOFPorDefecto.BaseRetencion
            NewLiquidacionesOF.PorcRetencion = LiquidacionesOFPorDefecto.PorcRetencion
            NewLiquidacionesOF.BaseRetencionTranslado = LiquidacionesOFPorDefecto.BaseRetencionTranslado
            NewLiquidacionesOF.PorcRetencionTranslado = LiquidacionesOFPorDefecto.PorcRetencionTranslado
            NewLiquidacionesOF.PorcIvaComision = LiquidacionesOFPorDefecto.PorcIvaComision
            NewLiquidacionesOF.IndicadorAcciones = LiquidacionesOFPorDefecto.IndicadorAcciones
            NewLiquidacionesOF.OperacionNegociada = LiquidacionesOFPorDefecto.OperacionNegociada
            NewLiquidacionesOF.FechaConstancia = LiquidacionesOFPorDefecto.FechaConstancia
            NewLiquidacionesOF.ValorConstancia = LiquidacionesOFPorDefecto.ValorConstancia
            NewLiquidacionesOF.GeneraConstancia = LiquidacionesOFPorDefecto.GeneraConstancia
            NewLiquidacionesOF.Cargado = LiquidacionesOFPorDefecto.Cargado
            NewLiquidacionesOF.Actualizacion = LiquidacionesOFPorDefecto.Actualizacion
            NewLiquidacionesOF.Usuario = Program.Usuario
            NewLiquidacionesOF.CumplimientoTitulo = Now.Date
            NewLiquidacionesOF.NroLote = LiquidacionesOFPorDefecto.NroLote
            NewLiquidacionesOF.ValorEntregaContraPago = LiquidacionesOFPorDefecto.ValorEntregaContraPago
            NewLiquidacionesOF.AquienSeEnviaRetencion = LiquidacionesOFPorDefecto.AquienSeEnviaRetencion
            NewLiquidacionesOF.IDBaseDias = LiquidacionesOFPorDefecto.IDBaseDias
            NewLiquidacionesOF.TipoDeOferta = LiquidacionesOFPorDefecto.TipoDeOferta
            NewLiquidacionesOF.NroLoteENC = LiquidacionesOFPorDefecto.NroLoteENC
            NewLiquidacionesOF.ContabilidadENC = LiquidacionesOFPorDefecto.ContabilidadENC
            NewLiquidacionesOF.IDLiquidacionesOF = -1
            NewLiquidacionesOF.CodigoIntermediario = LiquidacionesOFPorDefecto.CodigoIntermediario
            LiquidacionesOFAnterior = LiquidacionesOFSelected
            clientes.Comitente = ""
            clientes.NombreCliente = ""
            clientesOrdenantes.Ordenante = ""
            clientesOrdenantes.NombreClienteOrdenante = ""
            Especie.IDEspecie = ""
            Especie.NombreEspecie = ""
            habilitaboton = False
            habilitaradio = True
            esnuevo = True
            LiquidacionesOFSelected = NewLiquidacionesOF
            VeriOrdenes.NumeroOrden = Now.Date.Year
            VeriOrdenes.NumeroOrden1 = String.Empty

            logNuevoRegistro = True
            logEditarRegistro = False

            'CType(VmLiquidaciones.df.FindName("Stporden"), StackPanel).Cursor(True)
            'CType(VmLiquidaciones.df.FindName("txtNumeroOrden1"), TextBox).Focus()
            'CType(VmLiquidaciones.df.FindName("txtParcial"), A2Utilidades.A2NumericBox).ClearValue(Telerik.Windows.Controls.RadNumericUpDown.ValueProperty)
            MyBase.CambioItem("Liquidaciones")
            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub Filtrar()
        Try
            If Not validafiltro Then
                selected = True
                dcProxy.LiquidacionesOFs.Clear()
                IsBusy = True
                If FiltroVM.Length > 0 Then
                    Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                    dcProxy.Load(dcProxy.LiquidacionesOFFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesOF, "FiltroVM")
                Else
                    dcProxy.Load(dcProxy.LiquidacionesOFFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesOF, "Filtrar")
                End If
            End If
            validafiltro = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub ConfirmarBuscar()
        Try
            selected = True
            If cb.ID <> 0 Or cb.IDComitente <> String.Empty Or cb.Tipo <> String.Empty Or cb.ClaseOrden <> String.Empty Or cb.NumeroOrden1 <> String.Empty Or Not IsNothing(cb.Liquidacion) Or Not IsNothing(cb.CumplimientoTitulo) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.LiquidacionesOFs.Clear()
                LiquidacionesOFAnterior = Nothing

                Dim orden As String
                If Not IsNumeric(cb.NumeroOrden1) And Not IsNothing(cb.NumeroOrden1) Then
                    A2Utilidades.Mensajes.mostrarMensaje("La orden debe ser un valor númerico", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    cb.NumeroOrden1 = Nothing
                    Exit Sub
                End If
                IsBusy = True
                If cb.NumeroOrden1 Is Nothing Then
                    orden = "0"
                Else
                    orden = CStr(cb.NumeroOrden) + Format(CInt(cb.NumeroOrden1), "000000")
                End If
                'DescripcionFiltroVM = " ID = " & cb.ID.ToString() & " IDComitente = " & cb.IDComitente.ToString()
                'dcProxy.Load(dcProxy.TesoreriaConsultarQuery(Filtro, CamposBusquedaTesoreria.Tipo, CamposBusquedaTesoreria.NombreConsecutivo, CamposBusquedaTesoreria.IDDocumento, IIf(CamposBusquedaTesoreria.Documento.Year < 1900, Nothing, CamposBusquedaTesoreria.Documento), estadoMC,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreria, "Busqueda")
                'dcProxy.Load(dcProxy.LiquidacionesConsultarQuery(cb.ID, cb.IDComitente, cb.Tipo, cb.ClaseOrden, IIf(cb.Liquidacion.Date.Year < 1800, "1799/01/01", cb.Liquidacion), IIf(cb.CumplimientoTitulo.Date.Year < 1800, "1799/01/01", cb.CumplimientoTitulo), CInt(orden), 0,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, "Busqueda")
                dcProxy.Load(dcProxy.LiquidacionesOFConsultarQuery(cb.ID, cb.IDComitente, cb.Tipo, cb.ClaseOrden, IIf(cb.Liquidacion Is Nothing, "1799/01/01", cb.Liquidacion), IIf(cb.CumplimientoTitulo Is Nothing, "1799/01/01", cb.CumplimientoTitulo), CInt(orden), 0, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesOF, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaLiquidacionesOF
                CambioItem("cb")
            Else
                ErrorForma = ""
                dcProxy.LiquidacionesOFs.Clear()
                LiquidacionesOFAnterior = Nothing
                IsBusy = True
                'dcProxy.Load(dcProxy.LiquidacionesConsultarQuery(cb.ID, cb.IDComitente, cb.Tipo, cb.ClaseOrden, IIf(cb.Liquidacion.Date.Year < 1800, "1799/01/01", cb.Liquidacion), IIf(cb.CumplimientoTitulo.Date.Year < 1800, "1799/01/01", cb.CumplimientoTitulo), cb.NumeroOrden, cb.NumeroOrden,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, "Busqueda")
                dcProxy.Load(dcProxy.LiquidacionesOFConsultarQuery(cb.ID, cb.IDComitente, cb.Tipo, cb.ClaseOrden, IIf(cb.Liquidacion Is Nothing, "1799/01/01", cb.Liquidacion), IIf(cb.CumplimientoTitulo Is Nothing, "1799/01/01", cb.CumplimientoTitulo), cb.NumeroOrden, cb.NumeroOrden, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesOF, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaLiquidacionesOF
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Function ValidarReceptoresOrdenesOF() As Boolean
        Dim retorno As Boolean = False
        Try
            Dim existeLider As Integer
            Dim sumaPorcentaje As Integer

            If ListaReceptoresOrdenesOF.Count <> 0 Then
                For Each detalleReceptor In ListaReceptoresOrdenesOF
                    If IsNothing(detalleReceptor.Porcentaje) Or detalleReceptor.Porcentaje = 0 Then
                        Throw New ValidationException("El porcentaje no puede ser vacío.")
                    End If
                    If detalleReceptor.Lider Then
                        existeLider = existeLider + 1
                    End If
                    If IsNothing(detalleReceptor.IDReceptor) Then
                        Throw New ValidationException("El código de receptor no puede ser vacío.")
                    End If
                    If IsNothing(detalleReceptor.Nombre) Then
                        Throw New ValidationException("El nombre del receptor no puede ser vacío, este se establece seleccionando el código del receptor.")
                    End If
                    sumaPorcentaje = sumaPorcentaje + detalleReceptor.Porcentaje
                Next

                If existeLider = 0 Then
                    Throw New ValidationException("Debe haber un receptor líder.")
                End If
                If existeLider > 1 Then
                    Throw New ValidationException("Sólo debe existir un líder.")
                End If
                If sumaPorcentaje <> 100 Then
                    Throw New ValidationException("El porcentaje es: " & sumaPorcentaje & ", el total debe ser 100.")
                End If

                Dim numeroErrores = (From lr In ListaReceptoresOrdenesOF Where lr.HasValidationErrors = True).Count
                If numeroErrores <> 0 Then
                    Throw New ValidationException("Por favor revise que todos los campos requeridos en los registros de detalle hayan sido correctamente diligenciados.")
                End If
            Else
                Throw New ValidationException("Debe existir al menos un receptor.")
            End If
            retorno = True
        Catch ex As ValidationException
            IsBusy = False
            A2Utilidades.Mensajes.mostrarMensaje(ex.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ValidarReceptoresOrdenesOF", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return retorno
    End Function
    Public Overrides Sub ActualizarRegistro()
        Try
            LiquidacionesOFSelected.IDNegocio = 0
            If validafechacierre("ingresada o modificada") Then
                If ValidarReceptoresOrdenesOF() Then
                    If validaciones() Then
                        validaliq("ACTUALIZAR")
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            disparaguardar = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ActualizarRegistroDespuesValidadoDatos()
        Try
            If LiquidacionesOFSelected.Manual Then
                If LiquidacionesOFSelected.Cantidad > mcursaldoOrden Then
                    disparaguardar = False
                    '                    C1.Silverlight.C1MessageBox.Show("La cantidad de la operación supera el saldo de la Orden " _
                    '                                                     & "Desea continuar?", _
                    'Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminaPreguntaSobregiro)
                    mostrarMensajePregunta("La cantidad de la operación supera el saldo de la Orden ",
                                           Program.TituloSistema, _
                                           "ACTUALIZARREGISTRO", _
                                           AddressOf TerminaPreguntaSobregiro, True, "¿Desea continuar?")
                    Exit Sub
                End If
            End If

            Ordenesliq()
        Catch ex As Exception
            IsBusy = False
            disparaguardar = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistroDespuesValidadoDatos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminaPreguntaSobregiro(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            Ordenesliq()
        End If
    End Sub
    Private Sub Guardarliq()
        Try
            IsBusy = True
            Dim origen = "update"
            ErrorForma = ""
            LiquidacionesOFAnterior = LiquidacionesOFSelected

            dcProxy.CumplimientoOrdenOF_liq(LiquidacionesOFSelected.Tipo,
                                            LiquidacionesOFSelected.ClaseOrden,
                                            LiquidacionesOFSelected.IDOrden, _
                                            LiquidacionesOFSelected.IDEspecie,
                                            Nothing,
                                            Nothing,
                                            Nothing, Program.Usuario, Program.HashConexion,
                                            AddressOf TerminoVerificarCumplimientoOrderliq, Nothing)



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
                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                'Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)


                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If
                Dim strMsg As String = String.Empty
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If

                So.MarkErrorAsHandled()
                Exit Try
            End If
            'If So.UserState = "insert" or So.UserState = "BorrarRegistro"  then
            If So.UserState = "insert" Or So.UserState = "update" Or So.UserState = "BorrarRegistro" Then
                If So.UserState = "update" Then
                    If Not IsNothing(_LiquidacionesOFSelected) Then
                        IDRegistroModificado = _LiquidacionesOFSelected.ID
                    End If
                End If

                MyBase.QuitarFiltroDespuesGuardar()
                dcProxy.LiquidacionesOFs.Clear()
                dcProxy.Load(dcProxy.LiquidacionesOFFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesOF, So.UserState) ' Recarga la lista para que carguen los include
            End If
            'If So.UserState = "BorrarRegistro" Then
            '    ListaLiquidacionesOF = _ListaLiquidacionesOF
            'End If
            habilitaboton = True
            habilitaradio = False
            mlogCantidad = False
            disparafocus = False
            disparaguardar = False
            Userstate = ""

            logNuevoRegistro = False
            logEditarRegistro = False
            IDLiquidacionEdicion = 0
            ParcialEdicion = 0
            IDBolsaEdicion = 0
            FechaLiquidacionEdicion = Now
            HabilitarComisionistaLocal = False
            HabilitarComisionistaOtraPlaza = False

            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Metodo encargado de lanzar la edicion del formulario.
    ''' </summary>
    ''' <remarks>        
    ''' Modificado por:	Jeison Ramírez Pino.
    ''' Descripción:    Se inicializan algunas propiedades de la liquidacion seleccionada de manera provisional para poder realizar los metodos CRUD de la pestaña
    '''                 Receptores. Estos se deben quitar una vez se realicen los metodos CRUD para la liquidacion.
    ''' Fecha:			Junio 15/2013
    ''' </remarks>
    Public Overrides Sub EditarRegistro()
        If dcProxy.IsLoading Then
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If

        estadoregistro = False
        If Not IsNothing(_LiquidacionesOFSelected) Then

            'Retirar este codigo segun condiciones del Summary
            LiquidacionesOFSelected.AutoRetenedor = "N"
            LiquidacionesOFSelected.Certificacion = "N"
            LiquidacionesOFSelected.ConstanciaEnajenacion = "N"
            LiquidacionesOFSelected.RepoTitulo = "N"
            LiquidacionesOFSelected.Sujeto = "N"
            LiquidacionesOFSelected.Traslado = "1"
            LiquidacionesOFSelected.Swap = "N"
            LiquidacionesOFSelected.Reinversion = "N"
            LiquidacionesOFSelected.Usuario = Program.Usuario
            LiquidacionesOFSelected.Actualizacion = Now
            Editando = True
            habilitaboton = False
            habilitaradio = False
            ESTADO_EDICION = True
            SaldoOrden()
            'mcursaldoOrden = LiquidacionesOFSelected.Cantidad
            If LiquidacionesOFSelected.ClaseOrden.Equals("C") Then
                habilitavencimiento = True
                habilitamanualdias = True
            End If
            If LiquidacionesOFSelected.Manual Then
                If LiquidacionesOFSelected.IDFactura Is Nothing Or LiquidacionesOFSelected.IDFactura = 0 Then
                    habilitamanualsi = True
                    If Not IsNothing(LiquidacionesOFSelected.ParcialPadre) And LiquidacionesOFSelected.ParcialPadre <> 0 Then
                        habilitaparciald = True
                    End If
                    HabilitarFechaLiquidacion = True
                    If LiquidacionesOFSelected.ClaseOrden.Equals("C") Then
                        Datostitulo = True
                        habilitaemision = True
                    End If

                    If LiquidacionesOFSelected.Plaza = "LOC" Then
                        HabilitarComisionistaLocal = True
                        HabilitarComisionistaOtraPlaza = False
                    Else
                        HabilitarComisionistaLocal = False
                        HabilitarComisionistaOtraPlaza = True
                    End If
                    'HabilitarFechaTitulo = False
                    'HabilitarFechaEfectivo = False
                    'Else

                    '    HabilitarFechaTitulo = False
                    '    HabilitarFechaEfectivo = False
                End If
            End If
            If LiquidacionesOFSelected.Tipo.Equals("C") Or LiquidacionesOFSelected.Tipo.Equals("R") Then
                HabilitarFechaVendido = True
            End If

            logNuevoRegistro = False
            logEditarRegistro = True

            IDLiquidacionEdicion = LiquidacionesOFSelected.ID
            ParcialEdicion = LiquidacionesOFSelected.Parcial
            IDBolsaEdicion = LiquidacionesOFSelected.IDBolsa
            FechaLiquidacionEdicion = LiquidacionesOFSelected.Liquidacion

        End If
    End Sub
    Public Overrides Sub CancelarEditarRegistro()
        Try
            habilitaboton = True
            habilitamanualsi = False
            HabilitarComisionistaLocal = False
            HabilitarComisionistaOtraPlaza = False
            HabilitarFechaLiquidacion = False
            HabilitarFechaVendido = False
            HabilitarFechaTitulo = False
            HabilitarFechaEfectivo = False
            habilitaparciald = False
            habilitavencimiento = False
            Datostitulo = False
            habilitaemision = False
            HabilitarComisionistaLocal = False
            HabilitarComisionistaOtraPlaza = False
            ErrorForma = ""
            If Not IsNothing(_LiquidacionesOFSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _LiquidacionesOFSelected.EntityState = EntityState.Detached Then
                    LiquidacionesOFSelected = LiquidacionesOFAnterior
                End If
            End If

            logNuevoRegistro = False
            logEditarRegistro = False
            IDLiquidacionEdicion = 0
            ParcialEdicion = 0
            IDBolsaEdicion = 0
            FechaLiquidacionEdicion = Now

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub BorrarRegistro()
        'MessageBox.Show("Esta funcionalidad no está disponible.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
        'A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If Not IsNothing(LiquidacionesOFSelected) Then
                If LiquidacionesOFSelected.IDFactura > 0 And Not IsNothing(LiquidacionesOFSelected.IDFactura) Then
                    A2Utilidades.Mensajes.mostrarMensaje("La liquidación está facturada, no se puede borrar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                If validafechacierre("borrada") Then
                    selected = True
                    If Not IsNothing(LiquidacionesOFSelected) Then
                        dcProxy.ActualizaordenestadoOF(LiquidacionesOFSelected.Tipo, LiquidacionesOFSelected.ClaseOrden, LiquidacionesOFSelected.IDOrden, Program.Usuario, Program.HashConexion, AddressOf TerminoactualizarordenestadoOF, _
                                                     LiquidacionesOFSelected.Tipo + "," + LiquidacionesOFSelected.ClaseOrden + "," + LiquidacionesOFSelected.IDOrden.ToString)
                        dcProxy.LiquidacionesOFs.Remove(LiquidacionesOFSelected)
                        'ListaLiquidacionesOF.Remove(LiquidacionesOFSelected)
                        selected = True
                        LiquidacionesOFSelected = _ListaLiquidacionesOF.LastOrDefault
                        IsBusy = True
                        Program.VerificarCambiosProxyServidor(dcProxy)
                        dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                    End If
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub Buscar()
        cb.Liquidacion = Nothing
        cb.CumplimientoTitulo = Nothing
        cb.DisplayDate = Date.Now
        cb.DisplayDate2 = Date.Now
        cb.IDComitente = Nothing
        cb.ClaseOrden = Nothing
        cb.Tipo = Nothing
        cb.ID = Nothing
        cb.NumeroOrden1 = Nothing
        'CType(VmLiquidaciones.dfBuscar.FindName("txtnOrden"), TextBox).Focus()
        'datePicker1.ClearValue(DatePicker.SelectedDateProperty)

        MyBase.Buscar()

        'CType(VmLiquidaciones.dfBuscar.FindName("Cumplimiento"), DatePicker).ClearValue(DatePicker.SelectedDateProperty)
        'CType(VmLiquidaciones.dfBuscar.FindName("FLiquidacion"), DatePicker).ClearValue(DatePicker.SelectedDateProperty)
        'MessageBox.Show("Esta funcionalidad no está disponible.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
        'A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    End Sub
    Public Overrides Sub CancelarBuscar()
        validafiltro = True
        MyBase.CancelarBuscar()
    End Sub
    Friend Sub buscarItem(ByVal pstrTipoItem As String, Optional ByVal pstrIdItem As String = "")
        Dim strIdItem As String = String.Empty
        Dim logConsultar As Boolean = False

        Try
            If Not Me.LiquidacionesOFSelected Is Nothing Then
                Select Case pstrTipoItem
                    Case "clientes"
                        If Not IsNothing(LiquidacionesOFSelected.IDComitente) Then
                            pstrIdItem = pstrIdItem.Trim()
                            If pstrIdItem.Equals(String.Empty) Then
                                strIdItem = Me.LiquidacionesOFSelected.IDComitente
                            Else
                                strIdItem = pstrIdItem
                            End If
                            If Not strIdItem.Equals(String.Empty) Then
                                logConsultar = True
                            End If
                            If logConsultar Then
                                mdcProxyUtilidad01.BuscadorClientes.Clear()
                                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarClienteEspecificoQuery(LiquidacionesOFSelected.IDComitente, Program.Usuario, "IdComitenteLectura", Program.HashConexion), AddressOf buscarClienteCompleted, pstrTipoItem)
                            End If
                        End If
                    Case "clientesOrdenantes"
                        If Not IsNothing(LiquidacionesOFSelected.IDOrdenante) Then
                            pstrIdItem = pstrIdItem.Trim()
                            If pstrIdItem.Equals(String.Empty) Then
                                strIdItem = Me.LiquidacionesOFSelected.IDOrdenante
                            Else
                                strIdItem = pstrIdItem
                            End If
                            If Not strIdItem.Equals(String.Empty) Then
                                logConsultar = True
                            End If
                            If logConsultar Then
                                mdcProxyUtilidad02.BuscadorOrdenantes.Clear()
                                mdcProxyUtilidad02.Load(mdcProxyUtilidad02.buscarOrdenantesComitenteQuery(LiquidacionesOFSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf buscarOrdenanteCompleted, pstrTipoItem)
                            End If
                        End If
                    Case "especies"
                        If Not IsNothing(LiquidacionesOFSelected.IDEspecie) Then
                            pstrIdItem = pstrIdItem.Trim()
                            If pstrIdItem.Equals(String.Empty) Then
                                strIdItem = Me.LiquidacionesOFSelected.IDEspecie
                            Else
                                strIdItem = pstrIdItem
                            End If
                            If Not strIdItem.Equals(String.Empty) Then
                                logConsultar = True
                            End If
                            If logConsultar Then
                                mdcProxyUtilidad02.BuscadorEspecies.Clear()
                                mdcProxyUtilidad02.Load(mdcProxyUtilidad02.buscarNemotecnicoEspecificoQuery("", LiquidacionesOFSelected.IDEspecie, Program.Usuario, Program.HashConexion), AddressOf buscarEspecieCompleted, pstrTipoItem)
                                'SE ENVIA EL PARAMETRO PSTRMERCADO VACIO A LA FUNCION buscarNemotecnicoEspecifico EN EL DOMAINSERVICES DE UTILIDADES
                            End If
                        End If
                    Case Else
                        logConsultar = False
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos de la ciudad", Me.ToString(), "Buscar ciudad", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Private Sub buscarClienteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Dim strTipoItem As String
        Try
            If lo.UserState Is Nothing Then
                strTipoItem = ""
            Else
                strTipoItem = lo.UserState
            End If

            If lo.Entities.ToList.Count > 0 Then
                Me.clientes.NombreCliente = lo.Entities.ToList.Item(0).Nombre
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarClienteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Private Sub buscarOrdenanteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorOrdenantes))
        Dim strTipoItem As String
        Try
            If lo.UserState Is Nothing Then
                strTipoItem = ""
            Else
                strTipoItem = lo.UserState
            End If

            If lo.Entities.ToList.Count > 0 Then
                'Dim a = mdcProxyUtilidad02.BuscadorOrdenantes.Where(Function(l) l.Lider = True).First.Nombre
                Me.clientesOrdenantes.NombreClienteOrdenante = lo.Entities.ToList.Where(Function(l) l.Lider = True).First.Nombre
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarOrdenanteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Private Sub buscarEspecieCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorEspecies))
        Dim strTipoItem As String
        Try
            If lo.UserState Is Nothing Then
                strTipoItem = ""
            Else
                strTipoItem = lo.UserState
            End If

            If lo.Entities.ToList.Count > 0 Then
                Me.Especie.NombreEspecie = lo.Entities.ToList.Item(0).Especie
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarOrdenanteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub
    Public Sub Aplazamientos()
        If IsNothing(LiquidacionesOFSelected) Then
            Exit Sub
        End If
        If LiquidacionesOFSelected.Tipo.Equals("R") Or LiquidacionesOFSelected.Tipo.Equals("S") Then
            strTipoAplazamiento = "I"
        Else
            strTipoAplazamiento = "S"
        End If
        aplazamientoserie = New Aplazamiento_En_Serie(strTipoAplazamiento, LiquidacionesOFSelected.Liquidacion, LiquidacionesOFSelected.CumplimientoTitulo)
        AddHandler aplazamientoserie.Closed, AddressOf CerroVentana
        Program.Modal_OwnerMainWindowsPrincipal(aplazamientoserie)
        aplazamientoserie.ShowDialog()

    End Sub
    Public Sub Duplicar()
        If IsNothing(LiquidacionesOFSelected) Then
            Exit Sub
        End If
        IsBusy = True
        estadoregistro = True
        _mlogDuplicando = True
        logNuevoRegistro = True
        logEditarRegistro = False
        ListaLiquidacionesOFvalidar.Clear()
        dcProxy.LiquidacionesOFConsultars.Clear()
        dcProxy.Load(dcProxy.LiquidacionesOFConsultarvalidarQuery(LiquidacionesOFSelected.Tipo, LiquidacionesOFSelected.ClaseOrden, LiquidacionesOFSelected.IDOrden, Program.Usuario, Program.HashConexion), AddressOf TerminotraerliquidacinesOFValidar, Nothing)
        MyBase.CambioItem("Editando")
        duplica = True
    End Sub
    Private Sub NuevoDuplicado()
        Try
            'MessageBox.Show("Esta funcionalidad no está disponible.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
            'A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Dim NewLiquidacionesOF As New LiquidacionesOF
            'TODO: Verificar cuales son los campos que deben inicializarse 
            NewLiquidacionesOF.IDComisionista = LiquidacionesOFSelected.IDComisionista
            NewLiquidacionesOF.IDSucComisionista = LiquidacionesOFSelected.IDSucComisionista
            NewLiquidacionesOF.ID = LiquidacionesOFSelected.ID
            NewLiquidacionesOF.Parcial = -1
            NewLiquidacionesOF.Tipo = LiquidacionesOFSelected.Tipo
            NewLiquidacionesOF.ClaseOrden = LiquidacionesOFSelected.ClaseOrden
            NewLiquidacionesOF.IDEspecie = LiquidacionesOFSelected.IDEspecie
            NewLiquidacionesOF.IDOrden = LiquidacionesOFSelected.IDOrden
            NewLiquidacionesOF.Prefijo = LiquidacionesOFSelected.Prefijo
            NewLiquidacionesOF.IDFactura = LiquidacionesOFSelected.IDFactura
            NewLiquidacionesOF.Facturada = LiquidacionesOFSelected.Facturada
            NewLiquidacionesOF.IDComitente = LiquidacionesOFSelected.IDComitente
            NewLiquidacionesOF.IDOrdenante = LiquidacionesOFSelected.IDOrdenante
            NewLiquidacionesOF.IDBolsa = LiquidacionesOFSelected.IDBolsa
            NewLiquidacionesOF.ValBolsa = LiquidacionesOFSelected.ValBolsa
            NewLiquidacionesOF.IDRueda = LiquidacionesOFSelected.IDRueda
            NewLiquidacionesOF.TasaDescuento = LiquidacionesOFSelected.TasaDescuento
            NewLiquidacionesOF.TasaCompraVende = LiquidacionesOFSelected.TasaCompraVende
            NewLiquidacionesOF.Modalidad = LiquidacionesOFSelected.Modalidad
            NewLiquidacionesOF.IndicadorEconomico = LiquidacionesOFSelected.IndicadorEconomico
            NewLiquidacionesOF.PuntosIndicador = LiquidacionesOFSelected.PuntosIndicador
            NewLiquidacionesOF.Plazo = LiquidacionesOFSelected.Plazo
            NewLiquidacionesOF.Liquidacion = LiquidacionesOFSelected.Liquidacion
            NewLiquidacionesOF.Cumplimiento = LiquidacionesOFSelected.Cumplimiento
            NewLiquidacionesOF.Emision = LiquidacionesOFSelected.Emision
            NewLiquidacionesOF.Vencimiento = LiquidacionesOFSelected.Vencimiento
            NewLiquidacionesOF.OtraPlaza = LiquidacionesOFSelected.OtraPlaza
            NewLiquidacionesOF.Plaza = LiquidacionesOFSelected.Plaza
            NewLiquidacionesOF.IDComisionistaLocal = LiquidacionesOFSelected.IDComisionistaLocal
            NewLiquidacionesOF.IDComisionistaOtraPlaza = LiquidacionesOFSelected.IDComisionistaOtraPlaza
            NewLiquidacionesOF.IDCiudadOtraPlaza = LiquidacionesOFSelected.IDCiudadOtraPlaza
            NewLiquidacionesOF.TasaEfectiva = LiquidacionesOFSelected.TasaEfectiva
            'NewLiquidacione.Cantidad = LiquidacionesOFSelected.Cantidad
            NewLiquidacionesOF.Cantidad = consultarcantidadesOF.CantidadOrden - consultarcantidadesOF.CantidadLiq
            mcursaldoOrden = NewLiquidacionesOF.Cantidad
            NewLiquidacionesOF.Precio = 0
            NewLiquidacionesOF.FactorComisionPactada = 0
            If NewLiquidacionesOF.Precio = 0 Then
                NewLiquidacionesOF.Transaccion_cur = ListaLiquidacionesOFvalidar.First.Cantidad
                NewLiquidacionesOF.SubTotalLiq = NewLiquidacionesOF.Transaccion_cur
                NewLiquidacionesOF.TotalLiq = NewLiquidacionesOF.Transaccion_cur
            Else
                If NewLiquidacionesOF.ClaseOrden.Equals("A") Then
                    NewLiquidacionesOF.Transaccion_cur = NewLiquidacionesOF.Precio * NewLiquidacionesOF.Cantidad
                Else
                    NewLiquidacionesOF.Transaccion_cur = NewLiquidacionesOF.Precio * NewLiquidacionesOF.Cantidad / 100
                End If
                NewLiquidacionesOF.SubTotalLiq = NewLiquidacionesOF.Transaccion_cur
                NewLiquidacionesOF.TotalLiq = NewLiquidacionesOF.Transaccion_cur

            End If

            If Not IsNothing(ListaLiquidacionesOFvalidar.First.ComisionPactada) Then
                If Not IsNothing(NewLiquidacionesOF.Transaccion_cur) Then
                    NewLiquidacionesOF.Comision = NewLiquidacionesOF.Transaccion_cur * NewLiquidacionesOF.FactorComisionPactada
                    If ListaLiquidacionesOFvalidar.First.Tipo.Equals("C") Or ListaLiquidacionesOFvalidar.First.Tipo.Equals("R") Then
                        NewLiquidacionesOF.SubTotalLiq = NewLiquidacionesOF.SubTotalLiq + NewLiquidacionesOF.Comision
                        NewLiquidacionesOF.TotalLiq = NewLiquidacionesOF.TotalLiq + NewLiquidacionesOF.Comision
                    Else
                        NewLiquidacionesOF.SubTotalLiq = NewLiquidacionesOF.SubTotalLiq - NewLiquidacionesOF.Comision
                        NewLiquidacionesOF.TotalLiq = NewLiquidacionesOF.TotalLiq - NewLiquidacionesOF.Comision
                    End If
                End If
            Else
                NewLiquidacionesOF.Comision = 0
            End If
            If Not IsNothing(ListaLiquidacionesOFvalidar.First.Objeto) Then
                If Not ListaLiquidacionesOFvalidar.First.Ordinaria And ListaLiquidacionesOFvalidar.First.Objeto.Equals("CRR") Then
                    'HABILITAR ESTAS DOS  PROPIEDADES
                    habilitaparciald = True
                End If
            End If
            NewLiquidacionesOF.Transaccion = LiquidacionesOFSelected.Transaccion
            NewLiquidacionesOF.Retencion = LiquidacionesOFSelected.Retencion
            NewLiquidacionesOF.Intereses = LiquidacionesOFSelected.Intereses
            NewLiquidacionesOF.ValorIva = LiquidacionesOFSelected.ValorIva
            NewLiquidacionesOF.DiasIntereses = LiquidacionesOFSelected.DiasIntereses
            NewLiquidacionesOF.Mercado = LiquidacionesOFSelected.Mercado
            NewLiquidacionesOF.NroTitulo = LiquidacionesOFSelected.NroTitulo
            NewLiquidacionesOF.IDCiudadExpTitulo = LiquidacionesOFSelected.IDCiudadExpTitulo
            NewLiquidacionesOF.PlazoOriginal = LiquidacionesOFSelected.PlazoOriginal
            NewLiquidacionesOF.Aplazamiento = LiquidacionesOFPorDefecto.Aplazamiento
            NewLiquidacionesOF.VersionPapeleta = LiquidacionesOFSelected.VersionPapeleta
            NewLiquidacionesOF.EmisionOriginal = LiquidacionesOFSelected.EmisionOriginal
            NewLiquidacionesOF.VencimientoOriginal = LiquidacionesOFSelected.VencimientoOriginal
            NewLiquidacionesOF.Impresiones = LiquidacionesOFSelected.Impresiones
            NewLiquidacionesOF.FormaPago = LiquidacionesOFSelected.FormaPago
            NewLiquidacionesOF.CtrlImpPapeleta = LiquidacionesOFSelected.CtrlImpPapeleta
            NewLiquidacionesOF.DiasVencimiento = LiquidacionesOFSelected.DiasVencimiento
            NewLiquidacionesOF.PosicionPropia = LiquidacionesOFSelected.PosicionPropia
            NewLiquidacionesOF.Transaccion = LiquidacionesOFSelected.Transaccion
            NewLiquidacionesOF.TipoOperacion = LiquidacionesOFSelected.TipoOperacion
            NewLiquidacionesOF.DiasContado = LiquidacionesOFSelected.DiasContado
            NewLiquidacionesOF.Ordinaria = LiquidacionesOFSelected.Ordinaria
            NewLiquidacionesOF.ObjetoOrdenExtraordinaria = LiquidacionesOFSelected.ObjetoOrdenExtraordinaria
            NewLiquidacionesOF.NumPadre = LiquidacionesOFSelected.NumPadre
            NewLiquidacionesOF.ParcialPadre = LiquidacionesOFSelected.ParcialPadre
            NewLiquidacionesOF.OperacionPadre = LiquidacionesOFSelected.OperacionPadre
            NewLiquidacionesOF.DiasTramo = LiquidacionesOFSelected.DiasTramo
            NewLiquidacionesOF.Vendido = LiquidacionesOFSelected.Vendido
            NewLiquidacionesOF.Vendido = LiquidacionesOFSelected.Vendido
            NewLiquidacionesOF.Manual = LiquidacionesOFSelected.Manual
            NewLiquidacionesOF.ValorTraslado = LiquidacionesOFSelected.ValorTraslado
            NewLiquidacionesOF.ValorBrutoCompraVencida = LiquidacionesOFSelected.ValorBrutoCompraVencida
            NewLiquidacionesOF.AutoRetenedor = LiquidacionesOFSelected.AutoRetenedor
            NewLiquidacionesOF.Sujeto = LiquidacionesOFSelected.Sujeto
            NewLiquidacionesOF.PcRenEfecCompraRet = LiquidacionesOFSelected.PcRenEfecCompraRet
            NewLiquidacionesOF.PcRenEfecVendeRet = LiquidacionesOFSelected.PcRenEfecVendeRet
            NewLiquidacionesOF.Reinversion = LiquidacionesOFSelected.Reinversion
            NewLiquidacionesOF.Swap = LiquidacionesOFSelected.Swap
            NewLiquidacionesOF.NroSwap = LiquidacionesOFSelected.NroSwap
            NewLiquidacionesOF.Certificacion = LiquidacionesOFSelected.Certificacion
            NewLiquidacionesOF.DescuentoAcumula = LiquidacionesOFSelected.DescuentoAcumula
            NewLiquidacionesOF.PctRendimiento = LiquidacionesOFSelected.PctRendimiento
            NewLiquidacionesOF.FechaCompraVencido = LiquidacionesOFSelected.FechaCompraVencido
            NewLiquidacionesOF.PrecioCompraVencido = LiquidacionesOFSelected.PrecioCompraVencido
            NewLiquidacionesOF.ConstanciaEnajenacion = LiquidacionesOFSelected.ConstanciaEnajenacion
            NewLiquidacionesOF.RepoTitulo = LiquidacionesOFSelected.RepoTitulo
            NewLiquidacionesOF.ServBolsaVble = LiquidacionesOFSelected.ServBolsaVble
            NewLiquidacionesOF.ServBolsaFijo = LiquidacionesOFSelected.ServBolsaFijo
            NewLiquidacionesOF.Traslado = LiquidacionesOFSelected.Traslado
            NewLiquidacionesOF.UBICACIONTITULO = LiquidacionesOFSelected.UBICACIONTITULO
            NewLiquidacionesOF.HoraGrabacion = LiquidacionesOFSelected.HoraGrabacion
            NewLiquidacionesOF.OrigenOperacion = LiquidacionesOFSelected.OrigenOperacion
            NewLiquidacionesOF.CodigoOperadorCompra = LiquidacionesOFSelected.CodigoOperadorCompra
            NewLiquidacionesOF.CodigoOperadorVende = LiquidacionesOFSelected.CodigoOperadorVende
            NewLiquidacionesOF.IdentificacionRemate = LiquidacionesOFSelected.IdentificacionRemate
            NewLiquidacionesOF.ModalidaOperacion = LiquidacionesOFSelected.ModalidaOperacion
            NewLiquidacionesOF.IndicadorPrecio = LiquidacionesOFSelected.IndicadorPrecio
            NewLiquidacionesOF.PeriodoExdividendo = LiquidacionesOFSelected.PeriodoExdividendo
            NewLiquidacionesOF.PlazoOperacionRepo = LiquidacionesOFSelected.PlazoOperacionRepo
            NewLiquidacionesOF.ValorCaptacionRepo = LiquidacionesOFSelected.ValorCaptacionRepo
            NewLiquidacionesOF.VolumenCompraRepo = LiquidacionesOFSelected.VolumenCompraRepo
            NewLiquidacionesOF.PrecioNetoFraccion = LiquidacionesOFSelected.PrecioNetoFraccion
            NewLiquidacionesOF.VolumenNetoFraccion = LiquidacionesOFSelected.VolumenNetoFraccion
            NewLiquidacionesOF.CodigoContactoComercial = LiquidacionesOFSelected.CodigoContactoComercial
            NewLiquidacionesOF.NroFraccionOperacion = LiquidacionesOFSelected.NroFraccionOperacion
            NewLiquidacionesOF.IdentificacionPatrimonio1 = LiquidacionesOFSelected.IdentificacionPatrimonio1
            NewLiquidacionesOF.TipoidentificacionCliente2 = LiquidacionesOFSelected.TipoidentificacionCliente2
            NewLiquidacionesOF.NitCliente2 = LiquidacionesOFSelected.NitCliente2
            NewLiquidacionesOF.IdentificacionPatrimonio2 = LiquidacionesOFSelected.IdentificacionPatrimonio2
            NewLiquidacionesOF.TipoIdentificacionCliente3 = LiquidacionesOFSelected.TipoIdentificacionCliente3
            NewLiquidacionesOF.NitCliente3 = LiquidacionesOFSelected.NitCliente3
            NewLiquidacionesOF.IdentificacionPatrimonio3 = LiquidacionesOFSelected.IdentificacionPatrimonio3
            NewLiquidacionesOF.IndicadorOperacion = LiquidacionesOFSelected.IndicadorOperacion
            NewLiquidacionesOF.BaseRetencion = LiquidacionesOFSelected.BaseRetencion
            NewLiquidacionesOF.PorcRetencion = LiquidacionesOFSelected.PorcRetencion
            NewLiquidacionesOF.BaseRetencionTranslado = LiquidacionesOFSelected.BaseRetencionTranslado
            NewLiquidacionesOF.PorcRetencionTranslado = LiquidacionesOFSelected.PorcRetencionTranslado
            NewLiquidacionesOF.PorcIvaComision = LiquidacionesOFSelected.PorcIvaComision
            NewLiquidacionesOF.IndicadorAcciones = LiquidacionesOFSelected.IndicadorAcciones
            NewLiquidacionesOF.OperacionNegociada = LiquidacionesOFSelected.OperacionNegociada
            NewLiquidacionesOF.FechaConstancia = LiquidacionesOFSelected.FechaConstancia
            NewLiquidacionesOF.ValorConstancia = LiquidacionesOFSelected.ValorConstancia
            NewLiquidacionesOF.GeneraConstancia = LiquidacionesOFSelected.GeneraConstancia
            NewLiquidacionesOF.Cargado = LiquidacionesOFSelected.Cargado
            NewLiquidacionesOF.Actualizacion = Now
            NewLiquidacionesOF.Usuario = Program.Usuario
            NewLiquidacionesOF.CumplimientoTitulo = LiquidacionesOFSelected.CumplimientoTitulo
            NewLiquidacionesOF.NroLote = LiquidacionesOFSelected.NroLote
            NewLiquidacionesOF.ValorEntregaContraPago = LiquidacionesOFSelected.ValorEntregaContraPago
            NewLiquidacionesOF.AquienSeEnviaRetencion = LiquidacionesOFSelected.AquienSeEnviaRetencion
            NewLiquidacionesOF.IDBaseDias = LiquidacionesOFSelected.IDBaseDias
            NewLiquidacionesOF.TipoDeOferta = LiquidacionesOFSelected.TipoDeOferta
            NewLiquidacionesOF.NroLoteENC = LiquidacionesOFSelected.NroLoteENC
            NewLiquidacionesOF.ContabilidadENC = LiquidacionesOFSelected.ContabilidadENC
            NewLiquidacionesOF.IDLiquidacionesOF = -1
            NewLiquidacionesOF.CodigoIntermediario = LiquidacionesOFSelected.CodigoIntermediario
            LiquidacionesOFAnterior = LiquidacionesOFSelected
            habilitaboton = False
            habilitaradio = False
            LiquidacionesOFSelected = NewLiquidacionesOF
            MyBase.CambioItem("LiquidacionesOFSelected")
            Editando = True
            MyBase.CambioItem("Editando")
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
    Private Sub CerroVentana()

        If aplazamientoserie.DialogResult = True Then
            Select Case aplazamientoserie.TipoSelected.Descripcion
                Case strtitulo
                    strAplazamiento = "T"
                Case strefectivo
                    strAplazamiento = "A"
            End Select

            If strTipoAplazamiento.Equals("S") Then
                'C1.Silverlight.C1MessageBox.Show("Se aplazarán todos los parciales de compra o venta de la operación: " & LiquidacionesOFSelected.ID _
                '                                 & vbCr & " con la siguiente Fecha: " & aplazamientoserie.TipoSelected.FechaAplazamiento & " de " & aplazamientoserie.TipoSelected.Descripcion, Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminaPregunta)
                mostrarMensajePregunta("Se aplazarán todos los parciales de compra o venta de la operación: " & LiquidacionesOFSelected.ID _
                                                 & vbCr & " con la siguiente Fecha: " & aplazamientoserie.TipoSelected.FechaAplazamiento & " de " & aplazamientoserie.TipoSelected.Descripcion,
                                        Program.TituloSistema,
                                        "CERROVENTANA",
                                        AddressOf TerminaPregunta, True, "¿Desea continuar?")
            Else
                'C1.Silverlight.C1MessageBox.Show("Se aplazará la operación: " & LiquidacionesOFSelected.ID _
                '                                              & vbCr & " con la siguiente Fecha: " & aplazamientoserie.TipoSelected.FechaAplazamiento & " de " & aplazamientoserie.TipoSelected.Descripcion, Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminaPregunta)
                mostrarMensajePregunta("Se aplazará la operación: " & LiquidacionesOFSelected.ID _
                                                              & vbCr & " con la siguiente Fecha: " & aplazamientoserie.TipoSelected.FechaAplazamiento & " de " & aplazamientoserie.TipoSelected.Descripcion,
                                        Program.TituloSistema,
                                        "CERROVENTANA",
                                        AddressOf TerminaPregunta, True, "¿Desea continuar?")
            End If
        End If
    End Sub
    Private Sub TerminaPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            IsBusy = True
            If Not IsNothing(LiquidacionesOFSelected) Then
                objProxy.AplazamientoOF(strTipoAplazamiento, strAplazamiento, LiquidacionesOFSelected.ClaseOrden, LiquidacionesOFSelected.Tipo, LiquidacionesOFSelected.ID _
                , LiquidacionesOFSelected.Parcial, LiquidacionesOFSelected.Liquidacion, aplazamientoserie.TipoSelected.FechaAplazamiento, Program.Usuario, strerror, strnroaplazamiento, Program.HashConexion, AddressOf TerminoactualizarAplazamiento, "")
            End If
        Else
            Exit Sub

        End If
    End Sub
    Private Sub TerminoactualizarAplazamiento(ByVal obj As InvokeOperation(Of String))
        IsBusy = False
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else

            If Not IsNothing(obj.Value) Then
                If (IsNumeric(obj.Value)) Then
                    If CInt(obj.Value) = -5 Then
                        A2Utilidades.Mensajes.mostrarMensaje("La operación tiene titulos descargados" & vbNewLine & " No puede ser aplazada", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Se aplazaron " & obj.Value & " Operaciones", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        LiquidacionesOFSelected = _LiquidacionesOFSelected
                        dcProxy.LiquidacionesOFs.Clear()
                        dcProxy.Load(dcProxy.LiquidacionesOFFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesOF, "insert") ' Recarga la lista para que carguen los include
                    End If

                Else
                    A2Utilidades.Mensajes.mostrarMensaje(obj.Value, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

            Else

            End If
        End If
    End Sub
    Public Function [IsNumeric](ByVal str As String) As Boolean
        Dim result As Decimal = 0
        Return Decimal.TryParse(str, result)
    End Function
    Private Sub _LiquidacionesOFSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _LiquidacionesOFSelected.PropertyChanged
        Select Case e.PropertyName
            Case "DiasVencimiento"
                If Not IsNothing(LiquidacionesOFSelected.Plazo) And Not IsNothing(LiquidacionesOFSelected.DiasVencimiento) _
                    And LiquidacionesOFSelected.DiasVencimiento > LiquidacionesOFSelected.Plazo Then
                    A2Utilidades.Mensajes.mostrarMensaje("Los días al vencimiento deben ser menores o iguales que el plazo del título ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                If Not IsNothing(LiquidacionesOFSelected.DiasVencimiento) Then
                    If IsNothing(LiquidacionesOFSelected.Vencimiento) Then
                        LiquidacionesOFSelected.Vencimiento = DateAdd("d", LiquidacionesOFSelected.DiasVencimiento, LiquidacionesOFSelected.Liquidacion)
                    End If
                    If Not IsNothing(LiquidacionesOFSelected.Plazo) Then
                        If IsNothing(LiquidacionesOFSelected.Emision) Then
                            LiquidacionesOFSelected.Emision = DateAdd("d", LiquidacionesOFSelected.Plazo * (1 - 2), LiquidacionesOFSelected.Vencimiento)
                        End If
                    End If
                End If
            Case "Vencimiento"
                If LiquidacionesOFSelected.Vencimiento < LiquidacionesOFSelected.Emision Then
                    A2Utilidades.Mensajes.mostrarMensaje("La Fecha de vencimiento debe ser mayor que la de emisión ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    LiquidacionesOFSelected.Vencimiento = Now
                    Exit Sub
                End If
            Case "Cantidad"
                If ListaLiquidacionesOF.Contains(LiquidacionesOFSelected) Then
                    mlogCantidad = True
                End If
            Case "Plaza" 'SLB 20130109 Se adiciona para el manejo del Comisionista Local o Otra Plaza como VB.6
                If _LiquidacionesOFSelected.Plaza = "LOC" Then
                    HabilitarComisionistaLocal = True
                    HabilitarComisionistaOtraPlaza = False
                    _LiquidacionesOFSelected.IDComisionistaOtraPlaza = 0
                    _LiquidacionesOFSelected.IDCiudadOtraPlaza = 0
                Else
                    HabilitarComisionistaLocal = False
                    HabilitarComisionistaOtraPlaza = True
                    _LiquidacionesOFSelected.IDComisionistaLocal = 0
                End If
            Case "Tipo"
                If Me._LiquidacionesOFSelected.IDOrden <> 0 And Not IsNothing(Me._LiquidacionesOFSelected.IDOrden) Then
                    validaorden()
                End If
            Case "Clase"
                If Me._LiquidacionesOFSelected.IDOrden <> 0 And Not IsNothing(Me._LiquidacionesOFSelected.IDOrden) Then
                    validaorden()
                End If
        End Select

    End Sub
    Public Sub validaorden()
        If VeriOrdenes.NumeroOrden1 = Nothing Then
            Exit Sub
        End If
        If Not IsNumeric(VeriOrdenes.NumeroOrden1) Then
            A2Utilidades.Mensajes.mostrarMensaje("El numero de la orden debe ser un valor numerico", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            VeriOrdenes.NumeroOrden1 = Nothing
            Exit Sub
        End If
        Dim a = CStr(VeriOrdenes.NumeroOrden) + Format(CInt(VeriOrdenes.NumeroOrden1), "000000")
        LiquidacionesOFSelected.IDOrden = CInt(a)
        ListaLiquidacionesOFvalidar.Clear()
        dcProxy.LiquidacionesOFConsultars.Clear()
        dcProxy.Load(dcProxy.LiquidacionesOFConsultarvalidarQuery(LiquidacionesOFSelected.Tipo, LiquidacionesOFSelected.ClaseOrden, LiquidacionesOFSelected.IDOrden, Program.Usuario, Program.HashConexion), AddressOf TerminotraerliquidacinesOFValidar, Nothing)

    End Sub
    Private Sub _NumeroOrden1_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _VeriOrdenes.PropertyChanged
        If LiquidacionesOFSelected.IDOrden <> 0 And Not IsNothing(LiquidacionesOFSelected.IDOrden) Then
            Exit Sub
        End If
        If e.PropertyName.Equals("NumeroOrden1") Then
            validaorden()
        End If
    End Sub
    Public Sub validaliq(Optional ByVal pstrUserState As String = "")
        If LiquidacionesOFSelected.ID <> 0 Then
            If logNuevoRegistro Then
                dcProxy.verilifaliqOF(LiquidacionesOFSelected.ID, LiquidacionesOFSelected.Parcial, LiquidacionesOFSelected.Tipo, LiquidacionesOFSelected.ClaseOrden, LiquidacionesOFSelected.IDBolsa, LiquidacionesOFSelected.Liquidacion, Program.Usuario, Program.HashConexion, AddressOf Terminotraervalidar, pstrUserState)
            ElseIf logEditarRegistro And (LiquidacionesOFSelected.ID <> IDLiquidacionEdicion Or LiquidacionesOFSelected.Parcial <> ParcialEdicion Or LiquidacionesOFSelected.Liquidacion <> FechaLiquidacionEdicion Or LiquidacionesOFSelected.IDBolsa <> IDBolsaEdicion) Then
                dcProxy.verilifaliqOF(LiquidacionesOFSelected.ID, LiquidacionesOFSelected.Parcial, LiquidacionesOFSelected.Tipo, LiquidacionesOFSelected.ClaseOrden, LiquidacionesOFSelected.IDBolsa, LiquidacionesOFSelected.Liquidacion, Program.Usuario, Program.HashConexion, AddressOf Terminotraervalidar, pstrUserState)
            ElseIf pstrUserState.Equals("ACTUALIZAR") Then
                Guardarliq()
            End If
        End If
    End Sub

    Public Sub llevarvalores()
        Try
            If Editando Then
                disparafocus = True

                'LiquidacionesOFSelected.Transaccion_cur = IIf(Not IsNothing(LiquidacionesOFSelected.Precio) And Not IsNothing(LiquidacionesOFSelected.Cantidad), ((LiquidacionesOFSelected.Precio) * (LiquidacionesOFSelected.Cantidad)), LiquidacionesOFSelected.Transaccion_cur)
                LiquidacionesOFSelected.Comision = LiquidacionesOFSelected.Transaccion_cur * CType(IIf(IsNothing(LiquidacionesOFSelected.FactorComisionPactada), 0, LiquidacionesOFSelected.FactorComisionPactada), Double)
                LiquidacionesOFSelected.TotalLiq = CType(IIf(IsNothing(LiquidacionesOFSelected.TotalLiq), 0, LiquidacionesOFSelected.TotalLiq), Double)
                LiquidacionesOFSelected.TotalLiq = CType(IIf(IsNothing(LiquidacionesOFSelected.SubTotalLiq), 0, LiquidacionesOFSelected.SubTotalLiq), Double)

                'If LiquidacionesOFSelected.Cantidad = 0 Then
                '    LiquidacionesOFSelected.Precio = 0
                If LiquidacionesOFSelected.Precio = 0 Then
                    LiquidacionesOFSelected.Transaccion_cur = LiquidacionesOFSelected.Cantidad
                    LiquidacionesOFSelected.SubTotalLiq = LiquidacionesOFSelected.Transaccion_cur
                    LiquidacionesOFSelected.TotalLiq = LiquidacionesOFSelected.Transaccion_cur
                Else
                    dcProxy.verilifavalor(LiquidacionesOFSelected.IDEspecie, LiquidacionesOFSelected.Liquidacion, Nothing, Program.Usuario, Program.HashConexion, AddressOf TerminoTraerAplazamientosLiquidacionesvalor, Nothing)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular valores", Me.ToString(), "llevarvalores", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Private Sub SaldoOrden()
        dcProxy.consultarcantidadOFs.Clear()
        dcProxy.Load(dcProxy.LiquidacionesOFConsultarcantidadQuery(LiquidacionesOFSelected.Tipo, LiquidacionesOFSelected.ClaseOrden, LiquidacionesOFSelected.IDOrden, LiquidacionesOFSelected.IDEspecie, Program.Usuario, Program.HashConexion), AddressOf TerminartraerCantidadOF, Nothing)
    End Sub
    Private Sub NuevoReceptorliq()
        dcProxy.ReceptoresOrdenesOFs.Clear()
        If Not IsNothing(ListaReceptoresOrdenesOF) Then
            ListaReceptoresOrdenesOF.Clear()
        End If
        dcProxy.Load(dcProxy.ReceptoresOrdenesOFliqQuery(LiquidacionesOFSelected.Tipo, LiquidacionesOFSelected.ClaseOrden, LiquidacionesOFSelected.IDOrden, Program.Usuario, Program.HashConexion), AddressOf TerminotraerreceptoresOFliq, Nothing)
    End Sub
    Private Sub BeneficiariosOrdenesOF()
        dcProxy.BeneficiariosOrdenesOFs.Clear()
        dcProxy.Load(dcProxy.BeneficiariosOrdenesOFliqQuery(LiquidacionesOFSelected.Tipo, LiquidacionesOFSelected.ClaseOrden, LiquidacionesOFSelected.IDOrden, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBeneficiariosOFliq, Nothing)
    End Sub
    Private Sub EspecieliqOF()
        dcProxy.EspeciesLiquidacionesOFs.Clear()
        dcProxy.Load(dcProxy.EspeciesOrdenesliqOFQuery(LiquidacionesOFSelected.Tipo, LiquidacionesOFSelected.ClaseOrden, LiquidacionesOFSelected.IDOrden, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesliqOF, Nothing)
    End Sub
    Private Function validafechacierre(ByVal opcion As String) As Boolean
        validafechacierre = True
        If LiquidacionesOFSelected.Liquidacion.ToShortDateString <= fechaCierre Then
            If fechaCierre <> "1900/01/01" Then
                If LiquidacionesOFSelected.Liquidacion <> fechaCierre Then
                    A2Utilidades.Mensajes.mostrarMensaje("La operación con fecha (" & LiquidacionesOFSelected.Liquidacion.ToShortDateString() & "). no puede ser " & opcion & " porque su fecha no es igual a la fecha abierta para el usuario ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    validafechacierre = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("La operación con fecha (" & LiquidacionesOFSelected.Liquidacion.ToShortDateString() & "). no puede ser " & opcion & " porque su fecha es inferior a la fecha de cierre", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                validafechacierre = False
            End If
        End If
        Return validafechacierre
    End Function
    Private Sub Ordenesliq()
        dcProxy.Load(dcProxy.OrdenesliqOFQuery(LiquidacionesOFSelected.Tipo, LiquidacionesOFSelected.ClaseOrden, LiquidacionesOFSelected.IDOrden, Program.Usuario, Program.HashConexion), AddressOf TerminotraerOrdenesliqOF, Nothing)
    End Sub
    Private Function validaciones() As Boolean
        Dim a As Boolean
        a = True
        If LiquidacionesOFSelected.ID = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe Ingresar un número de  liquidación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If IsNothing(LiquidacionesOFSelected.Parcial) Or LiquidacionesOFSelected.Parcial = -1 Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe Ingresar un número de  parcial", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If (LiquidacionesOFSelected.CumplimientoTitulo < LiquidacionesOFSelected.Liquidacion) Or (LiquidacionesOFSelected.Cumplimiento < LiquidacionesOFSelected.Liquidacion) Then
            A2Utilidades.Mensajes.mostrarMensaje("La fecha de cumplimiento debe ser mayor o igual que la de liquidación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If LiquidacionesOFSelected.Precio = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("El precio no debe ser cero", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            _LiquidacionesOFSelected.Precio = 1
            a = False
            Return a
        End If
        If LiquidacionesOFSelected.Cantidad = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("La cantidad no debe ser cero", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If numeroliq > 0 And logNuevoRegistro Then
            A2Utilidades.Mensajes.mostrarMensaje("La liquidación " + LiquidacionesOFSelected.ID.ToString + " con el parcial " + LiquidacionesOFSelected.Parcial.ToString + " y fecha " + LiquidacionesOFSelected.Liquidacion.ToShortDateString + " ya existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If LiquidacionesOFSelected.IDOrden = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("La Orden no debe ser cero", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If LiquidacionesOFSelected.Comision > LiquidacionesOFSelected.Transaccion_cur Then
            A2Utilidades.Mensajes.mostrarMensaje("La Comisión no debe ser mayor que el valor", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If LiquidacionesOFSelected.Plaza <> "LOC" And LiquidacionesOFSelected.Plaza <> String.Empty Then
            If IsNothing(LiquidacionesOFSelected.IDComisionistaOtraPlaza) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el comisionista de la otra plaza", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                a = False
                Return a
            End If

            If IsNothing(LiquidacionesOFSelected.IDCiudadOtraPlaza) Or LiquidacionesOFSelected.IDCiudadOtraPlaza = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la ciudad de la otra plaza", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                a = False
                Return a
            End If
        ElseIf LiquidacionesOFSelected.Plaza = "LOC" Then
            If IsNothing(LiquidacionesOFSelected.IDComisionistaLocal) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el comisionista local", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                a = False
                Return a
            End If

        End If

        If LiquidacionesOFSelected.ClaseOrden = "C" Then
            If IsNothing(LiquidacionesOFSelected.Plazo) Or LiquidacionesOFSelected.Plazo = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el plazo del título", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                a = False
                Return a
            End If
            If IsNothing(LiquidacionesOFSelected.Modalidad) Or LiquidacionesOFSelected.Modalidad = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la modalidad del título", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                a = False
                Return a
            End If
        End If

        Return a
    End Function
    Private Sub Denominacionespecie()
        dcProxy.verificanombretarifa(LiquidacionesOFSelected.IDEspecie, "", Program.Usuario, Program.HashConexion, AddressOf TerminoTraernombre, Nothing)
    End Sub



#End Region
#Region "Tablas hijas"
#Region "ReceptoresOrdenesOF"
    Private _ListaReceptoresOrdenesOF As EntitySet(Of ReceptoresOrdenesOF)
    Public Property ListaReceptoresOrdenesOF() As EntitySet(Of ReceptoresOrdenesOF)
        Get
            Return _ListaReceptoresOrdenesOF
        End Get
        Set(ByVal value As EntitySet(Of ReceptoresOrdenesOF))
            _ListaReceptoresOrdenesOF = value
            MyBase.CambioItem("ListaReceptoresOrdenesOF")
        End Set
    End Property

    Private _ReceptoresOrdenesOFSelected As ReceptoresOrdenesOF
    Public Property ReceptoresOrdenesOFSelected() As ReceptoresOrdenesOF
        Get
            Return _ReceptoresOrdenesOFSelected
        End Get
        Set(ByVal value As ReceptoresOrdenesOF)
            If Not value Is Nothing Then
                _ReceptoresOrdenesOFSelected = value
                MyBase.CambioItem("ReceptoresOrdenesOFSelected")
            End If
        End Set
    End Property

    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmReceptoresOrdenes"
                    Dim NewReceptoresOrdenes As New ReceptoresOrdenesOF
                    NewReceptoresOrdenes.TipoOrden = LiquidacionesOFSelected.Tipo
                    NewReceptoresOrdenes.ClaseOrden = LiquidacionesOFSelected.ClaseOrden
                    NewReceptoresOrdenes.IDOrden = LiquidacionesOFSelected.IDOrden
                    NewReceptoresOrdenes.Version = 0
                    NewReceptoresOrdenes.Usuario = Program.Usuario
                    ListaReceptoresOrdenesOF.Add(NewReceptoresOrdenes)
                    ReceptoresOrdenesOFSelected = NewReceptoresOrdenes
                    MyBase.CambioItem("ReceptoresOrdenesOFSelected")
                    MyBase.CambioItem("ListaReceptoresOrdenesOF")
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmReceptoresOrdenes"
                    If Not IsNothing(ListaReceptoresOrdenesOF) Then
                        If Not IsNothing(ListaReceptoresOrdenesOF) Then
                            Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ReceptoresOrdenesOFSelected, ListaReceptoresOrdenesOF.ToList)

                            ListaReceptoresOrdenesOF.Remove(_ReceptoresOrdenesOFSelected)
                            If ListaReceptoresOrdenesOF.Count > 0 Then
                                Program.PosicionarItemLista(ReceptoresOrdenesOFSelected, ListaReceptoresOrdenesOF.ToList, intRegistroPosicionar)
                            End If
                            MyBase.CambioItem("ReceptoresOrdenesOFSelected")
                            MyBase.CambioItem("ListaReceptoresOrdenesOF")
                        End If
                    End If

            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el borrado del registro", _
                                                         Me.ToString(), "BorrarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region
#Region "BeneficiariosOrdenesOF"
    Private _ListaBeneficiariosOrdenesOF As EntitySet(Of BeneficiariosOrdenesOF)
    Public Property ListaBeneficiariosOrdenesOF() As EntitySet(Of BeneficiariosOrdenesOF)
        Get
            Return _ListaBeneficiariosOrdenesOF
        End Get
        Set(ByVal value As EntitySet(Of BeneficiariosOrdenesOF))
            _ListaBeneficiariosOrdenesOF = value
            MyBase.CambioItem("ListaBeneficiariosOrdenesOF")
        End Set
    End Property

    Private _BeneficiariosOrdenesOFSelected As BeneficiariosOrdenesOF
    Public Property BeneficiariosOrdenesOFSelected() As BeneficiariosOrdenesOF
        Get
            Return _BeneficiariosOrdenesOFSelected
        End Get
        Set(ByVal value As BeneficiariosOrdenesOF)
            _BeneficiariosOrdenesOFSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("BeneficiariosOrdenesOFSelected")
            End If
        End Set
    End Property
#End Region
#Region "EspeciesLiquidacionesOF"
    Private _ListaEspeciesLiquidacionesOF As EntitySet(Of EspeciesLiquidacionesOF)
    Public Property ListaEspeciesLiquidacionesOF() As EntitySet(Of EspeciesLiquidacionesOF)
        Get
            Return _ListaEspeciesLiquidacionesOF
        End Get
        Set(ByVal value As EntitySet(Of EspeciesLiquidacionesOF))
            _ListaEspeciesLiquidacionesOF = value
            MyBase.CambioItem("ListaEspeciesLiquidacionesOF")
        End Set
    End Property

    Private _EspeciesLiquidacionesOFSelected As EspeciesLiquidacionesOF
    Public Property EspeciesLiquidacionesOFSelected() As EspeciesLiquidacionesOF
        Get
            Return _EspeciesLiquidacionesOFSelected
        End Get
        Set(ByVal value As EspeciesLiquidacionesOF)
            _EspeciesLiquidacionesOFSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("EspeciesLiquidacionesOFSelected")
            End If
        End Set
    End Property
#End Region
#Region "AplazamientosLiquidacionesOF"
    Private _ListaAplazamientosLiquidacionesOF As EntitySet(Of AplazamientosLiquidacionesOF)
    Public Property ListaAplazamientosLiquidacionesOF() As EntitySet(Of AplazamientosLiquidacionesOF)
        Get
            Return _ListaAplazamientosLiquidacionesOF
        End Get
        Set(ByVal value As EntitySet(Of AplazamientosLiquidacionesOF))
            _ListaAplazamientosLiquidacionesOF = value
            MyBase.CambioItem("ListaAplazamientosLiquidacionesOF")
        End Set
    End Property

    Private _AplazamientosLiquidacionesOFSelected As AplazamientosLiquidacionesOF
    Public Property AplazamientosLiquidacionesOFSelected() As AplazamientosLiquidacionesOF
        Get
            Return _AplazamientosLiquidacionesOFSelected
        End Get
        Set(ByVal value As AplazamientosLiquidacionesOF)
            _AplazamientosLiquidacionesOFSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("AplazamientosLiquidacionesOFSelected")
            End If
        End Set
    End Property
#End Region

    '#Region "CustodiasLiquidacionesOF"
    '    Private _ListaCustodiasLiquidacionesOF As EntitySet(Of CustodiasLiquidacionesOF)
    '    Public Property ListaCustodiasLiquidacionesOF() As EntitySet(Of CustodiasLiquidacionesOF)
    '        Get
    '            Return _ListaCustodiasLiquidacionesOF
    '        End Get
    '        Set(ByVal value As EntitySet(Of CustodiasLiquidacionesOF))
    '            _ListaCustodiasLiquidacionesOF = value
    '            MyBase.CambioItem("ListaCustodiasLiquidaciones")
    '        End Set
    '    End Property

    '    Private _CustodiasLiquidacionesOFSelected As CustodiasLiquidacionesOF
    '    Public Property CustodiasLiquidacionesOFSelected() As CustodiasLiquidacionesOF
    '        Get
    '            Return _CustodiasLiquidacionesOFSelected
    '        End Get
    '        Set(ByVal value As CustodiasLiquidacionesOF)
    '            _CustodiasLiquidacionesOFSelected = value
    '            If Not value Is Nothing Then
    '                MyBase.CambioItem("CustodiasLiquidacionesOFSelected")
    '            End If
    '        End Set
    '    End Property
    '#End Region

#End Region
End Class
'Clase base para forma de búsquedas
Public Class CamposBusquedaLiquidacionesOF

    Implements INotifyPropertyChanged
    Private _ID As Integer
    <Display(Name:="Liquidación")> _
    Public Property ID As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property

    Private _IDComitente As String
    <Display(Name:="Comitente")> _
    Public Property IDComitente As String
        Get
            Return _IDComitente
        End Get
        Set(ByVal value As String)
            _IDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDComitente"))
        End Set
    End Property
    Private _Tipo As String
    <Display(Name:="Tipo")> _
    Public Property Tipo As String
        Get
            Return _Tipo
        End Get
        Set(ByVal value As String)
            _Tipo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Tipo"))
        End Set
    End Property
    Private _ClaseOrden As String
    <Display(Name:="Clase")> _
    Public Property ClaseOrden As String
        Get
            Return _ClaseOrden
        End Get
        Set(ByVal value As String)
            _ClaseOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ClaseOrden"))
        End Set
    End Property



    Private _Liquidacion As System.Nullable(Of DateTime)
    <Display(Name:="Fecha Liquidación")> _
    Public Property Liquidacion As System.Nullable(Of DateTime)
        Get
            Return _Liquidacion
        End Get
        Set(value As System.Nullable(Of DateTime))
            _Liquidacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Liquidacion"))
        End Set
    End Property

    Private _CumplimientoTitulo As System.Nullable(Of DateTime)
    <Display(Name:="Cumplimiento Título")> _
    Public Property CumplimientoTitulo As System.Nullable(Of DateTime)
        Get
            Return _CumplimientoTitulo
        End Get
        Set(value As System.Nullable(Of DateTime))
            _CumplimientoTitulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CumplimientoTitulo"))
        End Set
    End Property

    <Display(Name:="Cumplimiento Título")> _
    Public Property DisplayDate As DateTime

    <Display(Name:="Fecha Liquidación")> _
    Public Property DisplayDate2 As DateTime

    Private _NumeroOrden As Integer = Now.Year
    <Display(Name:="Orden")> _
    Public Property NumeroOrden As Integer
        Get
            Return _NumeroOrden
        End Get
        Set(ByVal value As Integer)
            _NumeroOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NumeroOrden"))
        End Set
    End Property
    Private _NumeroOrden1 As String
    <Display(Name:="")> _
    Public Property NumeroOrden1 As String
        Get
            Return _NumeroOrden1
        End Get
        Set(ByVal value As String)
            _NumeroOrden1 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NumeroOrden1"))
        End Set
    End Property

    Private _ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
    Public Property ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
        Get
            Return (_ComitenteSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorClientes)
            _ComitenteSeleccionado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ComitenteSeleccionado"))
        End Set
    End Property


    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

Public Class ClientesclasesOF
    Implements INotifyPropertyChanged

    Private _Comitente As String
    <Display(Name:=" ")> _
    Public Property Comitente As String
        Get
            Return _Comitente
        End Get
        Set(ByVal value As String)
            _Comitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Comitente"))
        End Set
    End Property

    Private _NombreCliente As String
    <Display(Name:=" ")> _
    Public Property NombreCliente As String
        Get
            Return _NombreCliente
        End Get
        Set(ByVal value As String)
            _NombreCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreCliente"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
Public Class ClientesOrdenantesclasesOF
    Implements INotifyPropertyChanged

    Private _Ordenante As String
    <Display(Name:=" ")> _
    Public Property Ordenante As String
        Get
            Return _Ordenante
        End Get
        Set(ByVal value As String)
            _Ordenante = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Ordenante"))
        End Set
    End Property

    Private _NombreClienteOrdenante As String
    <Display(Name:=" ")> _
    Public Property NombreClienteOrdenante As String
        Get
            Return _NombreClienteOrdenante
        End Get
        Set(ByVal value As String)
            _NombreClienteOrdenante = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreClienteOrdenante"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
Public Class EspecieclasesOF
    Implements INotifyPropertyChanged

    Private _IDEspecie As String
    <Display(Name:=" ")> _
    Public Property IDEspecie As String
        Get
            Return _IDEspecie
        End Get
        Set(ByVal value As String)
            _IDEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDEspecie"))
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
Public Class trasladoOF
    Implements INotifyPropertyChanged

    Private _Traslado As Boolean
    <Display(Name:="Traslado")> _
    Public Property Traslado As Boolean
        Get
            Return _Traslado
        End Get
        Set(ByVal value As Boolean)
            _Traslado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Traslado"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
Public Class VeriOrdenOF

    Implements INotifyPropertyChanged
    Private _NumeroOrden As Integer
    Public Property NumeroOrden As Integer
        Get
            Return _NumeroOrden
        End Get
        Set(ByVal value As Integer)
            _NumeroOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NumeroOrden"))
        End Set
    End Property
    Private _NumeroOrden1 As String
    Public Property NumeroOrden1 As String
        Get
            Return _NumeroOrden1
        End Get
        Set(ByVal value As String)
            _NumeroOrden1 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NumeroOrden1"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class
