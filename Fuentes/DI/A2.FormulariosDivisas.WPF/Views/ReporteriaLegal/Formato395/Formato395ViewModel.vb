'RABP20181109: Desarrollo de pantalla de generación de archivos formato 395

Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web
Imports System.IO
Imports System.Text

Public Class Formato395ViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As FormulariosDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------

    Public objViewPrincipal As Formato395View

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
    Private _ListaFormato395 As List(Of CPX_FORMATO_395)
    Public Property ListaFormato395() As List(Of CPX_FORMATO_395)
        Get
            Return _ListaFormato395
        End Get
        Set(ByVal value As List(Of CPX_FORMATO_395))
            _ListaFormato395 = value
            MyBase.CambioItem("ListaFormato395")
        End Set
    End Property

    ''' <summary>
    ''' Lista de datos del fomato 395 validaciones de días no habíl
    ''' </summary>
    Private _ListaFormato395Validar As List(Of CPX_FORMATO395_Validar)
    Public Property ListaFormato395Validar() As List(Of CPX_FORMATO395_Validar)
        Get
            Return _ListaFormato395Validar
        End Get
        Set(ByVal value As List(Of CPX_FORMATO395_Validar))
            _ListaFormato395Validar = value
            MyBase.CambioItem("ListaFormato395Validar")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaFormato395
    Public Property cb() As CamposBusquedaFormato395
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaFormato395)
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
            Dim objCB As New CamposBusquedaFormato395
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
    Public Async Sub ConsultarFormato395()
        Try
            If IsNothing(cb.dtmFecha) Then
                MessageBox.Show((DiccionarioEtiquetasPantalla("FALTANFILTROS").Titulo))
            Else

                Dim objRespuestaValidar = Await mdcProxy.Formato395_ValidarAsync(cb.dtmFecha, Program.Usuario)
                If Not IsNothing(objRespuestaValidar) Then
                    ListaFormato395Validar = objRespuestaValidar.Value
                    If Not IsNothing(ListaFormato395Validar) AndAlso ListaFormato395Validar.Count > 0 Then

                        mstrPalabraClave = ListaFormato395Validar.FirstOrDefault.strPalabraClave
                        mstrTipoEntidad = ListaFormato395Validar.FirstOrDefault.strTipoEntidad
                        mstrCodigoEntidad = ListaFormato395Validar.FirstOrDefault.strCodigoEntidad
                        mstrNitComisionista = ListaFormato395Validar.FirstOrDefault.strNitComisionista
                        mstrNombreCompleto = ListaFormato395Validar.FirstOrDefault.strNombreCompleto
                        mdtmNoHabil = ListaFormato395Validar.FirstOrDefault.dtmNoHabil

                    End If
                End If

                If mdtmNoHabil = 1 Then
                    MessageBox.Show((DiccionarioEtiquetasPantalla("DIANOHABIL").Titulo))
                Else
                    If String.IsNullOrEmpty(mstrPalabraClave) Then
                        MessageBox.Show("No existe la palabra clave para poder generar el archivo", "Palabra clave")
                        Exit Sub
                    Else
                        Dim objRespuesta = Await mdcProxy.Formato395_GenerarAsync(cb.dtmFecha, Program.Usuario)
                        If Not IsNothing(objRespuesta) Then
                            ListaFormato395 = objRespuesta.Value
                            If Not IsNothing(ListaFormato395) AndAlso ListaFormato395.Count > 0 Then
                                mstrDetalle = ListaFormato395.FirstOrDefault.strDetalle
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
            If ListaFormato395.Count = 0 Then
                MessageBox.Show("No existen datos para este dia. Por favor verifique")
                Exit Sub
            End If

            Dim STR_ARCHIVO_DEFECTO As String
            Dim lngTotalRegistros As Long
            Dim TotalRegistros As Long
            Dim Registros As String
            Dim strTIPO_REGISTRO_DETALLE As String = "2"
            Dim strCadenaActualizar As String = ""

            'variable que se llena copn las líneas del archivo
            Dim lstLineasArchivo As New List(Of String) ' Arreglo con el string para escribir en el archivo

            STR_ARCHIVO_DEFECTO = "TCRM" + CStr(Format$(Year(cb.dtmFecha), "0000")) + CStr(Format$(Month(cb.dtmFecha), "00")) + CStr(Format$(Day(cb.dtmFecha), "00")) + ".txt"

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


                        For Each objRegistro In ListaFormato395

                            mstrDetalle = objRegistro.strDetalle
                            'mstrDetalle = mstrDetalle & " "

                            lstLineasArchivo.Add(mstrDetalle)

                            TotalRegistros = TotalRegistros + 1

                        Next

                        mstrDetalle = ""

                        ' se coloca el total de registros
                        TotalRegistros = TotalRegistros + 1
                        Registros = Format$(TotalRegistros, "00000000")

                        mstrDetalle = mstrDetalle & Registros
                        mstrDetalle = mstrDetalle & 6

                        lstLineasArchivo.Add(mstrDetalle)

                        mstrDetalle = ""

                        'If TotalRegistros > 0 Then
                        Dim reg As Integer
                            reg = 1

                            If reg = 1 Then
                                Registros = Format$(TotalRegistros, "00000000")
                                ' NUMERO DE REGISTRO
                                mstrDetalle = "00000001"
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
                                mstrDetalle = mstrDetalle & "09"
                                'Tipo de informe
                                mstrDetalle = mstrDetalle & "00"
                            End If


                        'End If

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
                _ConsultarCmd = New RelayCommand(AddressOf ConsultarFormato395)
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
Public Class CamposBusquedaFormato395
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
