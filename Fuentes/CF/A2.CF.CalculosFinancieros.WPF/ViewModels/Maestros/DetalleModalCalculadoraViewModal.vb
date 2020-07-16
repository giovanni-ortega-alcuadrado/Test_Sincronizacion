Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports System.Threading.Tasks

''' <summary>
''' ViewModel para la pantalla que retorna el resultado de la calculadora financiera basica, perteneciente al proyecto de Cálculos Financieros.
''' </summary>
''' <history>
''' Creado por       : Germán González  - Alcuadrado S.A.
''' Descripción      : Creacion.
''' Fecha            : Junio 17/2014
''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
''' </history>

Public Class DetalleModalCalculadoraViewModal
    Implements INotifyPropertyChanged

#Region "Variables"

    ''' <summary>
    ''' Propiedad para realizar el enlace con la capa de datos del DomainContext correspondiente
    ''' </summary>
    Private mdcProxy As CalculosFinancierosDomainContext

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Propiedad para establecer el titulo de la pantalla
    ''' </summary>
    ''' <history>
    ''' Creado por       : Germán González  - Alcuadrado S.A.
    ''' Descripción      : Creacion.
    ''' Fecha            : Junio 17/2014
    ''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
    ''' </history>
    Private _strTituloPantalla As String
    Public Property strTituloPantalla As String
        Get
            Return _strTituloPantalla
        End Get
        Set(value As String)
            _strTituloPantalla = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTituloPantalla"))
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que contiene los registros del resultado de la calculadora basica
    ''' </summary>
    ''' <history>
    ''' Creado por       : Germán González  - Alcuadrado S.A.
    ''' Descripción      : Creacion.
    ''' Fecha            : Junio 17/2014
    ''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
    ''' </history>
    Private WithEvents _ListaDetalles As List(Of ResultadoCalculadoraBasica)
    Public Property ListaDetalles() As List(Of ResultadoCalculadoraBasica)
        Get
            Return _ListaDetalles
        End Get
        Set(ByVal value As List(Of ResultadoCalculadoraBasica))
            _ListaDetalles = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaDetalles"))
        End Set
    End Property

    ''' <summary>
    ''' Esta propiedad contiene una clase con las características digitadas por el usuario para realizar el calculo
    ''' </summary>
    ''' <history>
    ''' Creado por       : Germán González  - Alcuadrado S.A.
    ''' Descripción      : Creacion.
    ''' Fecha            : Junio 17/2014
    ''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
    ''' </history>
    Private WithEvents _CB As CalculadoraBasicaViewModel.CamposBusquedaCalculadoraBasica
    Public Property CB As CalculadoraBasicaViewModel.CamposBusquedaCalculadoraBasica
        Get
            Return _CB
        End Get
        Set(ByVal value As CalculadoraBasicaViewModel.CamposBusquedaCalculadoraBasica)
            _CB = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CB"))
        End Set
    End Property

#End Region

#Region "Inicialización"

    ''' <summary>
    ''' Inicializa el proxy y establece las filtros para realizar el cálculo
    ''' </summary>
    ''' <param name="TituloVentanaModal">Nombre del popup</param>
    ''' <param name="CamposBusqueda">Clase con los campos de busqueda</param>
    ''' <history>
    ''' Creado por       : Germán González  - Alcuadrado S.A.
    ''' Descripción      : Creacion.
    ''' Fecha            : Junio 17/2014
    ''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
    ''' </history>
    Public Sub New(TituloVentanaModal As String, CamposBusqueda As CalculadoraBasicaViewModel.CamposBusquedaCalculadoraBasica)
        Try
            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCalculosFinancieros()
            End If
            strTituloPantalla = TituloVentanaModal
            CB = CamposBusqueda
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ProcesoCobroUtilidadesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicia la consulta de la calculadora financiera básica
    ''' </summary>
    ''' <returns>Retorna un booleano que indica si el proceso es exitoso</returns>
    ''' <history>
    ''' Creado por       : Germán González  - Alcuadrado S.A.
    ''' Descripción      : Creacion.
    ''' Fecha            : Junio 17/2014
    ''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea sincrónico
                logResultado = Await ConsultarResultadoCalculadoraBasica()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return logResultado

    End Function

#End Region

#Region "Métodos sincrónicos"

    ''' <summary>
    ''' Método sincrónico que realiza el llamado a la función del proxy que realiza el cálculo
    ''' </summary>
    ''' <returns>Retorna un booleano de tipo Task que indica si el proceso es exitoso</returns>
    ''' <history>
    ''' Creado por       : Germán González  - Alcuadrado S.A.
    ''' Descripción      : Creacion.
    ''' Fecha            : Junio 17/2014
    ''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
    ''' </history>
    Private Async Function ConsultarResultadoCalculadoraBasica() As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of ResultadoCalculadoraBasica)

        Try
            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCalculosFinancieros()
            End If

            mdcProxy.ResultadoCalculadoraBasicas.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarResultadoCalculadoraBasicaQuery(
                                         CB.strIDEspecie, CB.strISIN, CB.strModalidad,
                                         CB.strBase, CB.dtmEmision, CB.logEsAccion, CB.dtmVencimiento, CB.dtmCompra,
                                         CB.strIndicador, CB.strTasaRef, CB.strMoneda,
                                         CB.strMetodoValoracion, CB.dblValorNominal, CB.dblValorGiro,
                                         CB.dblTasaTIR, CB.dblTasaTitulo, CB.dtmProceso, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar el cálculo con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar el cálculo con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarResultadoCalculadoraBasica", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaDetalles = mdcProxy.ResultadoCalculadoraBasicas.ToList
                End If
            End If

            logResultado = True

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las Utilidades de Custodias.", Me.ToString(), "ConsultarUtilidadesCustodias", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return logResultado

    End Function

#End Region

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class
