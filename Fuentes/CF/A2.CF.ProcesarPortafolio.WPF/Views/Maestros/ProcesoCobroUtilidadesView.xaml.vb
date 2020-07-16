Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes

Partial Public Class ProcesoCobroUtilidadesView
    Inherits Window

#Region "Variables"

    Public CerrarSinProcesar As Boolean = True
    Private mobjVM As ProcesoCobroUtilidadesViewModel

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Propiedad de tipo Nullable(Of DateTime) para la fecha de valoracion del Cobro de Utilidades
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Marzo 31/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Private _dtmFechaValoracion As System.Nullable(Of System.DateTime)
    Public Property dtmFechaValoracion() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaValoracion
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaValoracion = value
        End Set
    End Property

    ''' <summary>
    ''' Propiedad de tipo String para strIdEspecie del Cobro de Utilidades
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Marzo 31/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Private _strIdEspecie As String = String.Empty
    Public Property strIdEspecie() As String
        Get
            Return _strIdEspecie
        End Get
        Set(ByVal value As String)
            _strIdEspecie = value
        End Set
    End Property

    ''' <summary>
    ''' Propiedad de tipo String para lngIDComitente del Cobro de Utilidades
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Marzo 31/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Private _lngIDComitente As String = String.Empty
    Public Property lngIDComitente() As String
        Get
            Return _lngIDComitente
        End Get
        Set(ByVal value As String)
            _lngIDComitente = value
        End Set
    End Property

    ''' <summary>
    ''' Propiedad de tipo String para strTipoPortafolio del Cobro de Utilidades
    ''' </summary>
    ''' <history>
    ''' Creado por   : Jhon Alexis Echavarria 
    ''' Fecha        : Marzo 09/2016
    ''' Pruebas CB   : Jhon Alexis Echavarria - 09/2016 - Resultado OK
    ''' </history>
    Private _strTipoCompania As String = String.Empty
    Public Property strTipoCompania() As String
        Get
            Return _strTipoCompania
        End Get
        Set(ByVal value As String)
            _strTipoCompania = value
        End Set
    End Property

    ''' <summary>
    ''' Propiedad de tipo String para strTipoProceso del Cobro de Utilidades
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Marzo 31/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Private _strTipoProceso As String = String.Empty
    Public Property strTipoProceso() As String
        Get
            Return _strTipoProceso
        End Get
        Set(ByVal value As String)
            _strTipoProceso = value
        End Set
    End Property

    ''' <summary>
    ''' Propiedad de tipo String para strUsuario del Cobro de Utilidades
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Marzo 31/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Private _strUsuario As String = String.Empty
    Public Property strUsuario() As String
        Get
            Return _strUsuario
        End Get
        Set(ByVal value As String)
            _strUsuario = value
        End Set
    End Property

#End Region

#Region "Inicializacion"

    ''' <summary>
    ''' Inicializa la ventana para cobro de utilidades (estilos y consulta de los registros)
    ''' </summary>
    ''' <param name="pmobjVM">Objeto de tipo ProcesoCobroUtilidadesViewModel</param>
    ''' <param name="pdtmFechaValoracion">Objeto de tipo Nullable(Of DateTime)</param>
    ''' <param name="pstrIdEspecie">Objeto de tipo String</param>
    ''' <param name="plngIDComitente">Objeto de tipo String</param>
    ''' <param name="pstrTipoPortafolio">Objeto de tipo String</param>
    ''' <param name="pstrTipoProceso">Objeto de tipo String</param>
    ''' <param name="pstrUsuario">Objeto de tipo String</param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Marzo 31/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Public Sub New(ByVal pmobjVM As ProcesoCobroUtilidadesViewModel, ByVal pdtmFechaValoracion As Nullable(Of DateTime), ByVal pstrIdEspecie As String, ByVal plngIDComitente As String, ByVal pstrTipoCompania As String, ByVal pstrTipoProceso As String, ByVal pstrUsuario As String)

        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.EncuentaContabilidad)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        mobjVM = pmobjVM
        Me.DataContext = mobjVM

        dtmFechaValoracion = pdtmFechaValoracion
        strIdEspecie = pstrIdEspecie
        lngIDComitente = plngIDComitente
        strTipoCompania = pstrTipoCompania   'JAEZ 20160930
        strTipoProceso = pstrTipoProceso
        strUsuario = pstrUsuario

        Inicializar()

        InitializeComponent()

    End Sub

    ''' <summary>
    ''' Realiza un llamado sincrono al proceso inicializar del ViewModel para obtener los Cobros Pendientes
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Marzo 31/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    ''' <history>
    ''' Descripción  : Se añade nuevo parámetro al método inicializar: strTipoCompania con valor String.Empty por defecto, 
    '''                ya que se modificó el SP para recibir este parámetro desde la pantalla CobroUtilidadesView.
    ''' Modificado   : Javier Eduardo Pardo Moreno
    ''' Fecha        : Septiembre 07/2015
    ''' Pruebas CB   : Javier Eduardo Pardo Moreno - Septiembre 07/2015 - Resultado OK
    ''' </history>
    ''' <history>
    ''' Descripción  : Se añade nuevo parámetro al método inicializar: strEstado con valor String.Empty por defecto, 
    '''                ya que se modificó el SP para recibir este parámetro desde la pantalla CobroUtilidadesView.
    ''' Modificado   : Javier Eduardo Pardo Moreno
    ''' Fecha        : Septiembre 09/2015
    ''' Pruebas CB   : Javier Eduardo Pardo Moreno - Septiembre 09/2015 - Resultado OK
    ''' </history>
    ''' <history>
    ''' Descripción  : Se añade nuevo parámetro al método inicializar: strTipoPortafolio con valor String.Empty por defecto, 
    '''                ya que se modificó el SP para recibir este parámetro desde la pantalla CobroUtilidadesView.
    ''' Modificado   : Jhon Alexis Echavarria
    ''' Fecha        : Septiembre 09/2016
    ''' Pruebas CB   : Jhon Alexis Echavarria - Septiembre 09/2016 - Resultado OK
    ''' </history>
    Private Async Sub Inicializar()
        Await mobjVM.inicializar(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoCompania, String.Empty)
    End Sub

#End Region

#Region "Métodos para control de eventos"

    ''' <summary>
    ''' Controlador de eventos para seleccionar todos los check en la columna Cobrar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Marzo 31/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Private Sub chkCobrar_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim chk As CheckBox = CType(sender, CheckBox)
            Dim check As Boolean = chk.IsChecked.Value

            mobjVM.CobrarTodasUtilidades(check)

        Catch ex As Exception
            mostrarMensaje("Error chkCobrar_Click.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try

    End Sub

    ''' <summary>
    ''' Controlador de eventos para seleccionar todos los check en la columna Anular
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Marzo 31/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Private Sub chkAnular_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim chk As CheckBox = CType(sender, CheckBox)
            Dim check As Boolean = chk.IsChecked.Value

            mobjVM.AnularTodasUtilidades(check)

        Catch ex As Exception
            mostrarMensaje("Error chkCobrar_Click.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try

    End Sub

    ''' <summary>
    ''' Controlador de eventos para el botón Aceptar, actualizando los registros y ejecutando la valoración
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Marzo 31/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Private Sub AceptarCobro(sender As Object, e As RoutedEventArgs)
        Try
            Dim CerrarVentana As Boolean = True

            mobjVM.btnAceptar_Click(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoCompania, strTipoProceso, CerrarVentana)

            If CerrarVentana Then
                CerrarSinProcesar = False
                Me.Close()
            End If

        Catch ex As Exception
            mostrarMensaje("Error chkAll_Click.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.Close()
        Catch ex As Exception
            mostrarMensaje("Error en el evento Cerrar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try
    End Sub

#End Region

End Class
