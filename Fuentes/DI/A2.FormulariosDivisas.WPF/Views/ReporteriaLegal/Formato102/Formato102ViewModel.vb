'RABP20181109: Desarrollo de pantalla de generación de archivos formato 395

Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web
Imports System.IO
Imports System.Text

Public Class Formato102ViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As FormulariosDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------

    Public objViewPrincipal As Formato102View

    Dim mstrPalabraClave As String
    Dim mstrDetalle As String
    Dim mstrTipoEntidad As String
    Dim mstrCodigoEntidad As String
    Dim mstrNitComisionista As String
    Dim mstrNombreCompleto As String
    Dim mdtmNoHabil As Integer

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
    End Sub


    Public Sub inicializar()

        Dim logResultado As Boolean = False

        Try
            mdcProxy = inicializarProxy()
            PrepararNuevaBusqueda()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        Finally
            IsBusy = False
        End Try

    End Sub
#End Region
#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' Nombre del archivo generado
    ''' </summary>
    Private _strNombreArchivo As String
    Public Property strNombreArchivo() As String
        Get
            Return _strNombreArchivo
        End Get
        Set(ByVal value As String)
            _strNombreArchivo = value
            MyBase.CambioItem("strNombreArchivo")
        End Set
    End Property

    ''' <summary>
    ''' Lista de datos del fomato 395
    ''' </summary>
    Private _ListaFormato102 As List(Of CPX_FORMATO_102)
    Public Property ListaFormato102() As List(Of CPX_FORMATO_102)
        Get
            Return _ListaFormato102
        End Get
        Set(ByVal value As List(Of CPX_FORMATO_102))
            _ListaFormato102 = value
            MyBase.CambioItem("ListaFormato102")
        End Set
    End Property

    ''' <summary>
    ''' Lista de datos del fomato 102 validaciones de días no habíl
    ''' </summary>
    Private _ListaFormato102Validar As List(Of CPX_FORMATO395_Validar)
    Public Property ListaFormato102Validar() As List(Of CPX_FORMATO395_Validar)
        Get
            Return _ListaFormato102Validar
        End Get
        Set(ByVal value As List(Of CPX_FORMATO395_Validar))
            _ListaFormato102Validar = value
            MyBase.CambioItem("ListaFormato102Validar")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaFormato102
    Public Property cb() As CamposBusquedaFormato102
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaFormato102)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    ''' <summary>
    ''' Preparar nueva busqueda de registros
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaFormato102
            objCB.dtmFecha = Date.Now

            cb = objCB

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos públicos del encabezado"

    ''' <summary>
    ''' Consultar todos los datos de los formularios
    ''' </summary>
    Public Async Sub ConsultarFormato102()
        Try
            If IsNothing(cb.dtmFecha) Then
                MessageBox.Show((DiccionarioEtiquetasPantalla("FALTANFILTROS").Titulo))
            Else

                Dim objRespuestaValidar = Await mdcProxy.Formato395_ValidarAsync(cb.dtmFecha, Program.Usuario)
                If Not IsNothing(objRespuestaValidar) Then
                    ListaFormato102Validar = objRespuestaValidar.Value
                    If Not IsNothing(ListaFormato102Validar) AndAlso ListaFormato102Validar.Count > 0 Then

                        mstrPalabraClave = ListaFormato102Validar.FirstOrDefault.strPalabraClave
                        mstrTipoEntidad = ListaFormato102Validar.FirstOrDefault.strTipoEntidad
                        mstrCodigoEntidad = ListaFormato102Validar.FirstOrDefault.strCodigoEntidad
                        mstrNitComisionista = ListaFormato102Validar.FirstOrDefault.strNitComisionista
                        mstrNombreCompleto = ListaFormato102Validar.FirstOrDefault.strNombreCompleto
                        mdtmNoHabil = ListaFormato102Validar.FirstOrDefault.dtmNoHabil

                    End If
                End If

                If mdtmNoHabil = 1 Then
                    MessageBox.Show((DiccionarioEtiquetasPantalla("DIANOHABIL").Titulo))
                Else
                    If String.IsNullOrEmpty(mstrPalabraClave) Then
                        MessageBox.Show("No existe la palabra clave para poder generar el archivo", "Palabra clave")
                        Exit Sub
                    Else
                        Dim objRespuesta = Await mdcProxy.Formato102_GenerarAsync(cb.dtmFecha, Program.Usuario)
                        If Not IsNothing(objRespuesta) Then
                            ListaFormato102 = objRespuesta.Value
                            If Not IsNothing(ListaFormato102) AndAlso ListaFormato102.Count > 0 Then
                                mstrDetalle = ListaFormato102.FirstOrDefault.CODIGOENTIDAD
                            End If
                        End If
                    End If
                End If

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "AbrirDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Generación del archivo
    ''' </summary>
    Private Async Sub GrabarArchivo()
        Try

            'Validar que exista información de los formularios
            If ListaFormato102.Count = 0 Then
                MessageBox.Show("No existen datos para este dia. Por favor verifique")
                Exit Sub
            End If

            Dim STR_ARCHIVO_DEFECTO As String
            Dim lngTotalRegistros As Long
            Dim TotalRegistros As Long
            Dim Registros As String
            Dim strCadenaActualizar As String = ""
            Dim strCadena As String
            Dim intPosicion As Integer

            'variable que se llena copn las líneas del archivo
            Dim lstLineasArchivo As New List(Of String) ' Arreglo con el string para escribir en el archivo

            STR_ARCHIVO_DEFECTO = CStr(Format$(Year(cb.dtmFecha), "0000")) + CStr(Format$(Month(cb.dtmFecha), "00")) + CStr(Format$(Day(cb.dtmFecha), "00")) + ".txt"

            Dim fileDialog = New System.Windows.Forms.SaveFileDialog()
            fileDialog.DefaultExt = ".txt"
            fileDialog.Filter = "Archivos *.txt (*.txt)|*.txt"
            fileDialog.FileName = STR_ARCHIVO_DEFECTO

            'Dialog para obtener la ruta de guardado del archivo
            Dim result = fileDialog.ShowDialog()
            Select Case result
                Case System.Windows.Forms.DialogResult.OK

                    strNombreArchivo = fileDialog.FileName

                    Dim objStream = fileDialog.OpenFile()

                    ' Se hace la construcción de las líneas del archivo
                    Using archivo As StreamWriter = New StreamWriter(objStream)

                        mstrDetalle = ""
                        lngTotalRegistros = 0

                        'registro TIPO DOS
                        TotalRegistros = 1
                        TotalRegistros = TotalRegistros + 1
                        mstrDetalle = Format$(TotalRegistros, "00000")     ' NUMERO DE REGISTRO
                        mstrDetalle = mstrDetalle & 2 ' TIPO DE REGISTRO
                        mstrDetalle = mstrDetalle & "0000"    'Caso direccion general
                        mstrDetalle = mstrDetalle & "0"       ' 0 constante fijo
                        mstrDetalle = mstrDetalle & "2"       ' Tipo moneda 1 nal, 2 extranjera , 0 total
                        mstrDetalle = mstrDetalle & "1"       ' indicador de tipo de informacion 0 puc 1 formatos
                        mstrDetalle = mstrDetalle & "0"       ' Tipo de fideicomiso o fondo
                        mstrDetalle = mstrDetalle & "0000"    ' codigo de fideicomiso o fondo 0000 cuando no aplica

                        If Len(mstrDetalle) <> 18 Then
                            MessageBox.Show("Error al generar regsitro dos de identificacion de la informacion o enganche, longitud debe ser 18, actual: " & Len(mstrDetalle), vbCritical)
                        End If

                        lstLineasArchivo.Add(mstrDetalle)

                        TotalRegistros = TotalRegistros + 1

                        mstrDetalle = ""

                        For Each objRegistro In ListaFormato102

                            mstrDetalle = Format(TotalRegistros, "00000") ' NUMERO DE REGISTRO
                            mstrDetalle = mstrDetalle & "4"       ' tipo de  REGISTRO
                            mstrDetalle = mstrDetalle & "102"   ' codigo del formato
                            mstrDetalle = mstrDetalle & objRegistro.COLUMNA        ' codigo dela columna
                            mstrDetalle = mstrDetalle & objRegistro.UNIDADCAPTURA  ' UNIDADCAPTURA
                            mstrDetalle = mstrDetalle & objRegistro.SUBCUENTA      ' SUBCUENTA
                            mstrDetalle = mstrDetalle & "+"      ' SIGNO

                            strCadena = CStr(Format(objRegistro.VALOR, "###.00"))
                            mstrDetalle = mstrDetalle & Format(Int(strCadena), "00000000000000000")
                            intPosicion = InStr(1, strCadena, ".")

                            If intPosicion > 0 Then
                                mstrDetalle = mstrDetalle & "." & Mid(strCadena, InStr(1, strCadena, ".") + 1, Len(objRegistro.VALOR))
                            Else
                                mstrDetalle = mstrDetalle & "00"   ' promedio tasa cambio
                            End If

                            lstLineasArchivo.Add(mstrDetalle)

                            TotalRegistros = TotalRegistros + 1
                        Next

                        ' REGISTRO CINCO FIN DE ARCHIVO
                        mstrDetalle = ""
                        TotalRegistros = TotalRegistros + 1
                        mstrDetalle = Format$(TotalRegistros, "00000")         ' NUMERO DE REGISTRO
                        mstrDetalle = mstrDetalle & 5 ' TIPO DE REGISTRO

                        lstLineasArchivo.Add(mstrDetalle)

                        mstrDetalle = ""

                        Dim reg As Integer
                        reg = 1

                        If reg = 1 Then
                            Registros = Format$(TotalRegistros, "00000")
                            ' NUMERO DE REGISTRO
                            mstrDetalle = "00001"
                            ' TIPO DE REGISTRO
                            mstrDetalle = mstrDetalle & 1
                            ' tipo de entidad
                            mstrDetalle = mstrDetalle & (mstrCodigoEntidad)
                            ' codigo de la entidad
                            mstrDetalle = mstrDetalle & (mstrTipoEntidad)
                            'fecha de corte
                            mstrDetalle = mstrDetalle & CStr(Format$(Day(cb.dtmFecha), "00")) + CStr(Format$(Month(cb.dtmFecha), "00")) + CStr(Format$(Year(cb.dtmFecha), "0000"))
                            'total de registros
                            mstrDetalle = mstrDetalle & Registros
                            'palabra clave
                            mstrDetalle = mstrDetalle & Mid(mstrPalabraClave, 1, 11)
                            'area de informacion
                            mstrDetalle = mstrDetalle & "01"
                            'Tipo de informe
                            mstrDetalle = mstrDetalle & "10"
                        End If

                        archivo.WriteLine(mstrDetalle)

                        For Each strLinea In lstLineasArchivo
                            archivo.WriteLine(strLinea)
                        Next

                    End Using

                    MessageBox.Show("El archivo se generó con éxito.")

                    Exit Select
                Case System.Windows.Forms.DialogResult.Cancel
                    Exit Select
                Case Else
            End Select
        Catch ex As Exception
            MessageBox.Show("Se presentó un error al grabar el archivo. " & ex.Message, "GrabarArchivo")
        End Try
    End Sub

#End Region

#Region "Comandos"

    ''' <summary>
    ''' Comando del botón para consultar la información
    ''' </summary>
    Private WithEvents _ConsultarCmd As RelayCommand
    Public ReadOnly Property ConsultarCmd() As RelayCommand
        Get
            If _ConsultarCmd Is Nothing Then
                _ConsultarCmd = New RelayCommand(AddressOf ConsultarFormato102)
            End If
            Return _ConsultarCmd
        End Get
    End Property

    ''' <summary>
    ''' Comando para la generación del archivo
    ''' </summary>
    Private WithEvents _ArchivoCmd As RelayCommand
    Public ReadOnly Property ArchivoCmd() As RelayCommand
        Get
            If _ArchivoCmd Is Nothing Then
                _ArchivoCmd = New RelayCommand(AddressOf GrabarArchivo)
            End If
            Return _ArchivoCmd
        End Get
    End Property

#End Region

End Class
''' <summary>
''' REQUERIDO
''' 
''' Clase base para forma de búsquedas 
''' Esta clase se instancia para crear un objeto que guarda los valores seleccionados por el usuario en la forma de búsqueda
''' Sus atributos dependen de los datos del encabezado relevantes en una búsqueda
''' </summary>
''' <history>
''' 
''' </history>
Public Class CamposBusquedaFormato102
    Implements INotifyPropertyChanged

    Private _dtmFecha As DateTime
    Public Property dtmFecha() As DateTime
        Get
            Return _dtmFecha
        End Get
        Set(ByVal value As DateTime)
            _dtmFecha = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFecha"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

