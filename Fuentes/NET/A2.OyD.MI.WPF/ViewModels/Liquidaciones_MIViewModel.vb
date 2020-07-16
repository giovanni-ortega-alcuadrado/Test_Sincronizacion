Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: Liquidaciones_MIViewModel.vb
'Generado el : 06/05/2012 17:17:01
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
Imports A2.OyD.OYDServer.RIA.Web.OyDMILA

Public Class Liquidaciones_MIViewModel
    Inherits A2ControlMenu.A2ViewModel    
    Private Liquidaciones_MPorDefecto As Liquidacione_MI
    Private Liquidaciones_MAnterior As Liquidacione_MI
    Dim dcProxy As MILADomainContext
    Dim dcProxy1 As MILADomainContext

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MILADomainContext()
            dcProxy1 = New MILADomainContext()
        Else
            dcProxy = New MILADomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New MILADomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If
        Try
            If Not Program.IsDesignMode() Then

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "Liquidaciones_MIViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"



    Private Sub TerminoTraerLiquidaciones_MI(ByVal lo As LoadOperation(Of Liquidacione_MI))
        If Not lo.HasError Then
          
            ListaLiquidaciones_MI = dcProxy.Liquidacione_MIs
            For Each lid In ListaLiquidaciones_MI.Where(Function(r) IsNothing(r.Elegir))
                lid.Elegir = False
            Next

            If dcProxy.Liquidacione_MIs.Count > 0 Then
                ClaseReliquidacionesSelected.Habilitar = True
                ClaseReliquidaciones_IngresoSelected.Habilitar2 = True                
                    Liquidaciones_MSelected = ListaLiquidaciones_MI.Last                
            Else                
                A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                ClaseReliquidacionesSelected.Habilitar = False
                ClaseReliquidaciones_IngresoSelected.Habilitar2 = False            
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Liquidaciones_MI", _
                                             Me.ToString(), "TerminoTraerLiquidaciones_M", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub terminoGuardar(ByVal So As InvokeOperation(Of String))
        If So.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else                            
                dcProxy.Liquidacione_MIs.Clear()
            dcProxy.Load(dcProxy.ReLiquidacionesConsultarQuery(ClaseReliquidacionesSelected.lngIDComitente, ClaseReliquidacionesSelected.lngOpcionFecha, ClaseReliquidacionesSelected.dtmFecha, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones_MI, " ") ' Recarga la lista para que carguen los include
        End If
        IsBusy = False
    End Sub
    'Tablas padres



#End Region

#Region "Propiedades"

    Private _ListaLiquidaciones_MI As EntitySet(Of Liquidacione_MI)
    Public Property ListaLiquidaciones_MI() As EntitySet(Of Liquidacione_MI)
        Get
            Return _ListaLiquidaciones_MI
        End Get
        Set(ByVal value As EntitySet(Of Liquidacione_MI))
            _ListaLiquidaciones_MI = value

            MyBase.CambioItem("ListaLiquidaciones_MI")
            MyBase.CambioItem("ListaLiquidaciones_MIPaged")
            If Not IsNothing(value) Then
                If IsNothing(Liquidaciones_MAnterior) Then
                    Liquidaciones_MSelected = _ListaLiquidaciones_MI.FirstOrDefault
                Else
                    Liquidaciones_MSelected = Liquidaciones_MAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaLiquidaciones_MIPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaLiquidaciones_MI) Then
                Dim view = New PagedCollectionView(_ListaLiquidaciones_MI)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _Liquidaciones_MSelected As Liquidacione_MI
    Public Property Liquidaciones_MSelected() As Liquidacione_MI
        Get
            Return _Liquidaciones_MSelected
        End Get
        Set(ByVal value As Liquidacione_MI)
            _Liquidaciones_MSelected = value
            MyBase.CambioItem("Liquidaciones_MSelected")
        End Set
    End Property

    Private _ClaseReliquidacionesSelected As Reliquidaciones = New Reliquidaciones
    Public Property ClaseReliquidacionesSelected As Reliquidaciones
        Get
            Return _ClaseReliquidacionesSelected
        End Get
        Set(ByVal value As Reliquidaciones)
            _ClaseReliquidacionesSelected = value
            MyBase.CambioItem("ClaseReliquidacionesSelected")
        End Set
    End Property

    Private _ClaseReliquidaciones_IngresoSelected As Reliquidaciones_Ingreso = New Reliquidaciones_Ingreso
    Public Property ClaseReliquidaciones_IngresoSelected As Reliquidaciones_Ingreso
        Get
            Return _ClaseReliquidaciones_IngresoSelected
        End Get
        Set(ByVal value As Reliquidaciones_Ingreso)
            _ClaseReliquidaciones_IngresoSelected = value
            MyBase.CambioItem("ClaseReliquidaciones_IngresoSelected")
        End Set
    End Property





#End Region

#Region "Métodos"

    Public Overrides Sub Filtrar()
        Try
            If (Not IsDate(ClaseReliquidacionesSelected.dtmFecha)) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe elegir una Fecha para Consultar las Liquidaciones MI.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True
            dcProxy.Liquidacione_MIs.Clear()
            dcProxy.Load(dcProxy.ReLiquidacionesConsultarQuery(ClaseReliquidacionesSelected.lngIDComitente, ClaseReliquidacionesSelected.lngOpcionFecha, ClaseReliquidacionesSelected.dtmFecha, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones_MI, "")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub Chequear()
        If Not IsNothing(ClaseReliquidaciones_IngresoSelected.Chequear) Then
            If ClaseReliquidaciones_IngresoSelected.Chequear = False Then
                For Each lid In ListaLiquidaciones_MI
                    lid.Elegir = False
                Next
            Else
                For Each lid In ListaLiquidaciones_MI
                    lid.Elegir = True
                Next
            End If
        End If
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try

            If IsNothing(ClaseReliquidaciones_IngresoSelected.dblComisionPesos) Or ClaseReliquidaciones_IngresoSelected.dblComisionPesos = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el valor de la comisión en pesos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(ClaseReliquidaciones_IngresoSelected.dblComisionPorcentaje) Or ClaseReliquidaciones_IngresoSelected.dblComisionPorcentaje = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el porcentaje de la comisión en pesos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(ClaseReliquidaciones_IngresoSelected.dblTasaDolares) Or ClaseReliquidaciones_IngresoSelected.dblTasaDolares = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el valor de la tasa en dolares", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(ClaseReliquidaciones_IngresoSelected.dblTasaPesos) Or ClaseReliquidaciones_IngresoSelected.dblTasaPesos = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el valor de la tasa conversión en pesos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            Dim Lista = ListaLiquidaciones_MI.Where(Function(ic) ic.Elegir = True).ToList
            If Lista.Count < 1 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe haber al menos un registro seleccionado para reliquidar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            IsBusy = True
            For Each led In Lista
                dcProxy.ReLiquidacionesActualizar(ClaseReliquidaciones_IngresoSelected.dblComisionPesos,
                                                  ClaseReliquidaciones_IngresoSelected.dblComisionPorcentaje,
                                                  ClaseReliquidaciones_IngresoSelected.dblTasaDolares,
                                                  ClaseReliquidaciones_IngresoSelected.dblTasaPesos,
                                                  Liquidaciones_MSelected.ID,
                                                  Liquidaciones_MSelected.Parcial,
                                                  Liquidaciones_MSelected.Tipo,
                                                  Liquidaciones_MSelected.ClaseOrden,
                                                  Liquidaciones_MSelected.IDEspecie,
                                                  Liquidaciones_MSelected.IDOrden,
                                                  Liquidaciones_MSelected.IDComitente,
                                                  Liquidaciones_MSelected.Transaccion_cur,
                                                  Liquidaciones_MSelected.IDMoneda,
                                                  Liquidaciones_MSelected.ValorCostos,
                                                  ClaseReliquidacionesSelected.lngOpcionFecha,
                                                  Liquidaciones_MSelected.Liquidacion,
                                                  Program.Usuario, Program.HashConexion,
                                                  AddressOf terminoGuardar, "")
            Next
            ClaseReliquidaciones_IngresoSelected.dblComisionPesos = 0
            ClaseReliquidaciones_IngresoSelected.dblComisionPorcentaje = 0
            ClaseReliquidaciones_IngresoSelected.dblTasaDolares = 0
            ClaseReliquidaciones_IngresoSelected.dblTasaPesos = 0
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class

Public Class Reliquidaciones
    Implements INotifyPropertyChanged


    Private _lngIDComitente As String
    <Display(Name:="IDComitente")> _
    Public Property lngIDComitente As String
        Get
            Return _lngIDComitente
        End Get
        Set(ByVal value As String)
            _lngIDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDComitente"))
        End Set
    End Property

    Private _Nombre As String
    <Display(Name:="")> _
    Public Property Nombre As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombre"))
        End Set
    End Property


    Private _lngOpcionFecha As Integer
    <Display(Name:="Tipo Fecha")> _
    Public Property lngOpcionFecha As Integer
        Get
            Return _lngOpcionFecha
        End Get
        Set(ByVal value As Integer)
            _lngOpcionFecha = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngOpcionFecha"))
        End Set
    End Property

    Private _dtmFecha As DateTime = Date.Now
    <Display(Name:="Fecha")> _
    Public Property dtmFecha As DateTime
        Get
            Return _dtmFecha
        End Get
        Set(ByVal value As DateTime)
            _dtmFecha = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFecha"))
        End Set
    End Property

    Private _Habilitar As Boolean = False
    Public Property Habilitar() As Boolean
        Get
            Return _Habilitar
        End Get
        Set(ByVal value As Boolean)
            _Habilitar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Habilitar"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class


Public Class Reliquidaciones_Ingreso
    Implements INotifyPropertyChanged

    Private _dblComisionPesos As Double
    <Display(Name:="$")> _
    Public Property dblComisionPesos As Double
        Get
            Return _dblComisionPesos
        End Get
        Set(ByVal value As Double)
            _dblComisionPesos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblComisionPesos"))
        End Set
    End Property


    Private _dblComisionPorcentaje As Double
    <Display(Name:="%")> _
Public Property dblComisionPorcentaje() As Double
        Get
            Return _dblComisionPorcentaje
        End Get
        Set(ByVal value As Double)
            _dblComisionPorcentaje = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblComisionPorcentaje"))
        End Set
    End Property

    Private _dblTasaDolares As Double
    <Display(Name:="Dólares")> _
    Public Property dblTasaDolares() As Double
        Get
            Return _dblTasaDolares
        End Get
        Set(ByVal value As Double)
            _dblTasaDolares = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblTasaDolares"))
        End Set
    End Property
    Private _dblTasaPesos As Double
    <Display(Name:="Pesos")> _
    Public Property dblTasaPesos() As Double
        Get
            Return _dblTasaPesos
        End Get
        Set(ByVal value As Double)
            _dblTasaPesos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblTasaPesos"))
        End Set
    End Property


    Private _Chequear As Boolean = False
    <Display(Name:="")> _
    Public Property Chequear() As Boolean
        Get
            Return _Chequear
        End Get
        Set(ByVal value As Boolean)
            _Chequear = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Chequear"))
        End Set
    End Property


    Private _Habilitar2 As Boolean
    Public Property Habilitar2() As Boolean
        Get
            Return _Habilitar2
        End Get
        Set(ByVal value As Boolean)
            _Habilitar2 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Habilitar2"))
        End Set
    End Property





    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
