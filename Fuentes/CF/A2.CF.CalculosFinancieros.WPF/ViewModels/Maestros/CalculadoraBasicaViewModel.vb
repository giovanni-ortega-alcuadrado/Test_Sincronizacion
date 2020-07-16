Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

''' <summary>
''' ViewModel para la pantalla Calculadora Financiera Básica perteneciente al proyecto de Cálculos Financieros.
''' </summary>
''' <history>
''' Creado por       : Germán González  - Alcuadrado S.A.
''' Descripción      : Creacion.
''' Fecha            : Junio 17/2014
''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
''' </history>

Public Class CalculadoraBasicaViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables"

    ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mdcProxy As CalculosFinancierosDomainContext
    ' Este View se requiere como un popup para visualizar el resultado de la calculadora financiera básica
    Private ViewDetalle As DetalleModalCalculadoraView
    ' ViewModel correspondiente al detalle de la calculadora para el resultado
    Private mobjVM As DetalleModalCalculadoraViewModal

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Esta propiedad contiene una clase con las características digitadas por el usuario para realizar el calculo
    ''' </summary>
    ''' <history>
    ''' Creado por       : Germán González  - Alcuadrado S.A.
    ''' Descripción      : Creacion.
    ''' Fecha            : Junio 17/2014
    ''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
    ''' </history>
    Private WithEvents _CB As CamposBusquedaCalculadoraBasica
    Public Property CB() As CamposBusquedaCalculadoraBasica
        Get
            Return _CB
        End Get
        Set(ByVal value As CamposBusquedaCalculadoraBasica)
            _CB = value
            Me.CambioItem("CB")
        End Set
    End Property

    ''' <summary>
    ''' Se utiliza para limpiar los datos del buscador de especies
    ''' </summary>
    ''' <history>
    ''' Creado por       : Germán González  - Alcuadrado S.A.
    ''' Descripción      : Creacion.
    ''' Fecha            : Junio 17/2014
    ''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
    ''' </history>
    Private _BorrarEspecie As Boolean = False
    Public Property BorrarEspecie() As Boolean
        Get
            Return _BorrarEspecie
        End Get
        Set(ByVal value As Boolean)
            _BorrarEspecie = value
            If Not IsNothing(CB) Then
                CB.strISIN = String.Empty
            End If
            Me.CambioItem("BorrarEspecie")
        End Set
    End Property

    Private _HabilitarCamposRentaFija As Boolean = True
    Public Property HabilitarCamposRentaFija() As Boolean
        Get
            Return _HabilitarCamposRentaFija
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCamposRentaFija = value
            MyBase.CambioItem("HabilitarCamposRentaFija")
        End Set
    End Property

#End Region

#Region "Inicialización"

    ''' <summary>
    ''' Inicializa el objeto y activa el control que bloquea la pantalla mientras se está procesando
    ''' </summary>
    ''' <history>
    ''' Creado por       : Germán González  - Alcuadrado S.A.
    ''' Descripción      : Creacion.
    ''' Fecha            : Junio 17/2014
    ''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
    ''' </history>
    Public Sub New()
        IsBusy = True
    End Sub

    ''' <summary>
    ''' Carga inicial de datos
    ''' </summary>
    ''' <returns>Retorna un objeto de tipo boolean</returns>
    ''' <history>
    ''' Creado por       : Germán González  - Alcuadrado S.A.
    ''' Descripción      : Creacion.
    ''' Fecha            : Junio 17/2014
    ''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
    ''' </history>
    Public Function inicializar() As Boolean

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                PrepararNuevaBusqueda()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Métodos privados- REQUERIDOS"

    ''' <summary>
    ''' Establece los valores por defecto en la clase que tiene los campos de busqueda
    ''' </summary>
    ''' <history>
    ''' Creado por       : Germán González  - Alcuadrado S.A.
    ''' Descripción      : Creacion.
    ''' Fecha            : Junio 17/2014
    ''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
    ''' </history>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaCalculadoraBasica
            objCB.strIDEspecie = String.Empty
            objCB.strISIN = String.Empty
            objCB.strModalidad = String.Empty
            objCB.strBase = String.Empty
            objCB.dtmEmision = Date.Now
            objCB.logEsAccion = False
            objCB.dtmVencimiento = Date.Now
            objCB.dtmCompra = Date.Now
            objCB.strIndicador = String.Empty
            objCB.strTasaRef = String.Empty
            objCB.strMoneda = String.Empty
            objCB.strMetodoValoracion = String.Empty
            objCB.dblValorNominal = 0
            objCB.dblValorGiro = 0
            objCB.dblTasaTIR = 0
            objCB.dblTasaTitulo = 0
            objCB.dtmProceso = Date.Now
            BorrarEspecie = True
            BorrarEspecie = False
            CB = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar los datos de la forma de búsqueda", Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos publicos - REQUERIDOS"

    ''' <summary>
    ''' Metodo para capturar el evento del boton Limpiar
    ''' </summary>
    ''' <history>
    ''' Creado por       : Germán González  - Alcuadrado S.A.
    ''' Descripción      : Creacion.
    ''' Fecha            : Junio 17/2014
    ''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
    ''' </history>
    Public Sub LimpiarBusqueda()
        PrepararNuevaBusqueda()
    End Sub

    ''' <summary>
    ''' Metodo para capturar el evento del boton Calcular, inicializa la comunicación con los datos y levanta el popup de resultado
    ''' </summary>
    ''' <history>
    ''' Creado por       : Germán González  - Alcuadrado S.A.
    ''' Descripción      : Creacion.
    ''' Fecha            : Junio 17/2014
    ''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
    ''' </history>
    Public Sub Calcular()

        Try
            IsBusy = True

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCalculosFinancieros()
            End If

            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            mobjVM = New DetalleModalCalculadoraViewModal("Resultado Calculadora Básica", CB)
            ViewDetalle = New DetalleModalCalculadoraView(mobjVM)
            Program.Modal_OwnerMainWindowsPrincipal(ViewDetalle)
            ViewDetalle.ShowDialog()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al procesar la valoración", Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

    End Sub

#End Region

#Region "Métodos para controlar cambio de campos asociados a buscadores"

    ''' <summary>
    ''' Establece los datos retornados por el buscador de especies
    ''' </summary>
    ''' <param name="pstrIDEspecie"></param>
    ''' <history>
    ''' Creado por       : Germán González  - Alcuadrado S.A.
    ''' Descripción      : Creacion.
    ''' Fecha            : Junio 17/2014
    ''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
    ''' </history>
    Public Sub TraerNemotecnico(ByVal pstrIDEspecie As String)
        If Not IsNothing(CB) Then
            CB.strIDEspecie = pstrIDEspecie
            CB.strISIN = String.Empty
        End If
    End Sub

    ''' <summary>
    ''' Establece las caracteristicas del nemotécnico seleccionado por el usuario
    ''' </summary>
    ''' <param name="pobjNemotecnico"></param>
    ''' <history>
    ''' Creado por       : Germán González  - Alcuadrado S.A.
    ''' Descripción      : Creacion.
    ''' Fecha            : Junio 17/2014
    ''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
    ''' Modificado por   : Jhonatan Acevedo  - Alcuadrado S.A.
    ''' Descripción      : Modificado para que en caso de ser acción no genere error.
    ''' Fecha            : Abril 1/2015
    ''' Pruebas CB       : Jhonatan Acevedo - Abril 1/2015 - Resultado Ok 
    ''' </history>
    Public Sub TraerCaracteristicasNemotecnico(ByVal pobjNemotecnico As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(CB) Then
            CB.strISIN = pobjNemotecnico.ISIN
            CB.strIDEspecie = pobjNemotecnico.Especie
            CB.strModalidad = pobjNemotecnico.CodModalidad
            CB.dtmEmision = pobjNemotecnico.Emision
            CB.logEsAccion = pobjNemotecnico.EsAccion
            CB.dtmVencimiento = pobjNemotecnico.Vencimiento
            If Not IsNothing(pobjNemotecnico.IdIndicador) Then
                CB.strIndicador = CStr(pobjNemotecnico.IdIndicador)
            End If
            If Not CB.logEsAccion Then
                If pobjNemotecnico.CodTipoTasaFija.Equals("F") Then
                    CB.dblTasaTitulo = CDbl(pobjNemotecnico.TasaFacial)
                Else
                    CB.dblTasaTitulo = CDbl(pobjNemotecnico.PuntosIndicador)
                End If
            End If

            HabilitarInhabilitarCampos(CB.logEsAccion)
        End If
    End Sub

    Public Sub LimpiarObjeto()
        Dim objCB As New CamposBusquedaCalculadoraBasica
        objCB.strIDEspecie = String.Empty
        objCB.strISIN = String.Empty
        objCB.strModalidad = String.Empty
        objCB.strBase = String.Empty
        objCB.dtmEmision = Nothing
        objCB.dtmVencimiento = Nothing
        objCB.logEsAccion = False
        objCB.dtmCompra = Date.Now
        objCB.strIndicador = String.Empty
        objCB.strTasaRef = String.Empty
        objCB.strMoneda = String.Empty
        objCB.strMetodoValoracion = String.Empty
        objCB.dblValorNominal = 0
        objCB.dblValorGiro = 0
        objCB.dblTasaTIR = 0
        objCB.dblTasaTitulo = 0
        objCB.dtmProceso = Date.Now

        CB = objCB
    End Sub

    Public Sub HabilitarInhabilitarCampos(ByVal EsAccion As Boolean)
        If EsAccion Then
            HabilitarCamposRentaFija = False
        Else
            HabilitarCamposRentaFija = True
        End If
    End Sub

#End Region

#Region "Clases adicionales para captura de datos"

    ''' <summary>
    ''' Clase de administrar los campos de busqueda de la calculadora financiera básica
    ''' </summary>
    ''' <history>
    ''' Creado por       : Germán González  - Alcuadrado S.A.
    ''' Descripción      : Creacion.
    ''' Fecha            : Junio 17/2014
    ''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
    ''' </history>
    Public Class CamposBusquedaCalculadoraBasica
        Implements INotifyPropertyChanged

        Private _strIDEspecie As String
        Public Property strIDEspecie() As String
            Get
                Return _strIDEspecie
            End Get
            Set(ByVal value As String)
                _strIDEspecie = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strIDEspecie"))
            End Set
        End Property

        Private WithEvents _strISIN As String
        Public Property strISIN() As String
            Get
                Return _strISIN
            End Get
            Set(ByVal value As String)
                _strISIN = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strISIN"))
            End Set
        End Property

        Private _strModalidad As String
        Public Property strModalidad() As String
            Get
                Return _strModalidad
            End Get
            Set(ByVal value As String)
                _strModalidad = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strModalidad"))
            End Set
        End Property

        Private _strBase As String
        Public Property strBase() As String
            Get
                Return _strBase
            End Get
            Set(ByVal value As String)
                _strBase = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strBase"))
            End Set
        End Property

        Private _dtmEmision As System.Nullable(Of System.DateTime)
        Public Property dtmEmision() As System.Nullable(Of System.DateTime)
            Get
                Return _dtmEmision
            End Get
            Set(ByVal value As System.Nullable(Of System.DateTime))
                _dtmEmision = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmEmision"))
            End Set
        End Property

        Private _logEsAccion As Boolean
        Public Property logEsAccion As Boolean
            Get
                Return _logEsAccion
            End Get
            Set(value As Boolean)
                _logEsAccion = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logEsAccion"))
            End Set
        End Property

        Private _dtmVencimiento As System.Nullable(Of System.DateTime)
        Public Property dtmVencimiento() As System.Nullable(Of System.DateTime)
            Get
                Return _dtmVencimiento
            End Get
            Set(ByVal value As System.Nullable(Of System.DateTime))
                _dtmVencimiento = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmVencimiento"))
            End Set
        End Property

        Private _dtmCompra As System.Nullable(Of System.DateTime)
        Public Property dtmCompra() As System.Nullable(Of System.DateTime)
            Get
                Return _dtmCompra
            End Get
            Set(ByVal value As System.Nullable(Of System.DateTime))
                _dtmCompra = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmCompra"))
            End Set
        End Property

        Private _strIndicador As String
        Public Property strIndicador() As String
            Get
                Return _strIndicador
            End Get
            Set(ByVal value As String)
                _strIndicador = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strIndicador"))
            End Set
        End Property

        Private _strTasaRef As String
        Public Property strTasaRef() As String
            Get
                Return _strTasaRef
            End Get
            Set(ByVal value As String)
                _strTasaRef = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTasaRef"))
            End Set
        End Property

        Private _strMoneda As String
        Public Property strMoneda() As String
            Get
                Return _strMoneda
            End Get
            Set(ByVal value As String)
                _strMoneda = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strMoneda"))
            End Set
        End Property

        Private _strMetodoValoracion As String
        Public Property strMetodoValoracion() As String
            Get
                Return _strMetodoValoracion
            End Get
            Set(ByVal value As String)
                _strMetodoValoracion = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strMetodoValoracion"))
            End Set
        End Property

        Private _dblValorNominal As Double
        Public Property dblValorNominal() As Double
            Get
                Return _dblValorNominal
            End Get
            Set(ByVal value As Double)
                _dblValorNominal = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblValorNominal"))
            End Set
        End Property

        Private _dblValorGiro As Double
        Public Property dblValorGiro() As Double
            Get
                Return _dblValorGiro
            End Get
            Set(ByVal value As Double)
                _dblValorGiro = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblValorGiro"))
            End Set
        End Property

        Private _dblTasaTIR As Double
        Public Property dblTasaTIR() As Double
            Get
                Return _dblTasaTIR
            End Get
            Set(ByVal value As Double)
                _dblTasaTIR = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblTasaTIR"))
            End Set
        End Property

        Private _dblTasaTitulo As Double
        Public Property dblTasaTitulo() As Double
            Get
                Return _dblTasaTitulo
            End Get
            Set(ByVal value As Double)
                _dblTasaTitulo = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblTasaTitulo"))
            End Set
        End Property

        Private _dtmProceso As System.Nullable(Of System.DateTime)
        Public Property dtmProceso() As System.Nullable(Of System.DateTime)
            Get
                Return _dtmProceso
            End Get
            Set(ByVal value As System.Nullable(Of System.DateTime))
                _dtmProceso = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmProceso"))
            End Set
        End Property

        Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    End Class

#End Region

End Class
