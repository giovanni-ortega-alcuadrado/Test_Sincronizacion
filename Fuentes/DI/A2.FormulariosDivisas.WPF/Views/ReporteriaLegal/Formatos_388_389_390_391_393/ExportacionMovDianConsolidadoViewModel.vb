Imports System.Threading.Tasks
Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb
Imports System.Data

Public Class ExportacionMovDianConsolidadoViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------    
    Private mdcProxy As FormulariosDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios

#End Region

#Region "Constantes"

    ''' <summary>
    ''' RABP20191001: constante para el nombre del archivo txt
    ''' </summary>
    Dim strHoy As String
    Dim strNombreFormato As String = "388A393M"
    Dim mstrDetalle As String


#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' RABP20191001: Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True 'RABP20191001: Activar el control que bloquea la pantalla mientras se está procesando
    End Sub

    ''' <summary>
    ''' RABP20191001: Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False
        IsBusy = True
        Try
            mdcProxy = inicializarProxy()

            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region


#Region "Propiedades del Encabezado - REQUERIDO"


    ''' <summary>
    ''' RABP20191001: codigo OyD seleccionado en el buscador
    ''' </summary>
    Private _strAno As String
    Public Property strAno() As String
        Get
            Return _strAno
        End Get
        Set(ByVal value As String)
            _strAno = value
            CambioItem("strAno")
        End Set
    End Property

    ''' <summary>
    ''' RABP20191001: formato legal DIAN seleccionado consolidado
    ''' </summary>
    Private _Mes As Integer
    Public Property Mes() As Integer
        Get
            Return _Mes
        End Get
        Set(ByVal value As Integer)
            If Not IsNothing(value) Then
                _Mes = value
                'ActivarVisibilidad(_Mes)
            End If
            CambioItem("Mes")
        End Set
    End Property


    ''' <summary>
    ''' RABP20191001: lista que almacenara la consulta de movimientos de formato consolidado
    ''' </summary>
    Private _lstArchivosDianConsolidado As List(Of CPX_TransaccionesConsolidado)
    Public Property lstArchivosDianConsolidado() As List(Of CPX_TransaccionesConsolidado)
        Get
            Return _lstArchivosDianConsolidado
        End Get
        Set(ByVal value As List(Of CPX_TransaccionesConsolidado))
            _lstArchivosDianConsolidado = value
            CambioItem("lstArchivosDianConsolidado")
        End Set
    End Property


    ''' <summary>
    ''' RABP20191001: lista que almacenara la consulta de movimientos de formato consolidado
    ''' </summary>
    Private _lstArchivosDianConsolidado_Excel As List(Of CPX_TransaccionesConsolidado_EXCEL)
    Public Property lstArchivosDianConsolidado_Excel() As List(Of CPX_TransaccionesConsolidado_EXCEL)
        Get
            Return _lstArchivosDianConsolidado_Excel
        End Get
        Set(ByVal value As List(Of CPX_TransaccionesConsolidado_EXCEL))
            _lstArchivosDianConsolidado_Excel = value
            CambioItem("lstArchivosDianConsolidado_Excel")
        End Set
    End Property

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
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaFormatoConsolidado
    Public Property cb() As CamposBusquedaFormatoConsolidado
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaFormatoConsolidado)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    ''' <summary>
    ''' RABP20191002: formato legal DIAN seleccionado
    ''' </summary>
    Private _strFormato As String
    Public Property strFormato() As String
        Get
            Return _strFormato
        End Get
        Set(ByVal value As String)
            If Not IsNothing(value) Then
                _strFormato = value
            End If
            CambioItem("strFormato")
        End Set
    End Property

#End Region


#Region "Métodos sincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' RABP20191001: metodo para consultar movimientos legales DIAN consolidado
    ''' </summary>
    Private Async Sub ConsultarMovimientosDianConsolidado()
        Try
            IsBusy = True
            Dim objRespuesta = Nothing
            Dim user = Program.Usuario

            If IsNothing(strFormato) Then
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_CONSOLIDADO_CONSULTAR"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(Mes) Then
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_CONSOLIDADO_ANO"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(strAno) Then
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_CONSOLIDADO_MES"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If


            If Not IsNothing(strFormato) Then
                'ActivarVisibilidad(strFormato)
                Select Case strFormato
                    Case "Excel"
                        objRespuesta = Await mdcProxy.TransaccionesConsolidado_ExcelAsync(Mes, False, strAno, user)
                    Case "Texto"
                        objRespuesta = Await mdcProxy.TransaccionesConsolidado_GenerarAsync(Mes, False, strAno, user)
                End Select

            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_FORMATODIANCONSOLIDADO"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
            End If

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    Select Case strFormato
                        Case "Excel"
                            lstArchivosDianConsolidado_Excel = objRespuesta.Value
                        Case "Texto"
                            lstArchivosDianConsolidado = objRespuesta.Value
                    End Select

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarMovimientosDianConsolidado", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub


    ''' <summary>
    ''' RABP20191003: mMetodo para exportar los datos a Excel
    ''' </summary>
    ''' <param name="strRuta"></param>
    Private Async Sub ExportarMovimientosDIANConsolidado_Excel(ByVal strRuta As String)
        Try
            IsBusy = True
            Dim STR_ARCHIVO_DEFECTO As String
            Dim strCadenaActualizar As String = ""


            If IsNothing(strFormato) Then
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_CONSOLIDADO_CONSULTAR"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(Mes) Then
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_CONSOLIDADO_ANO"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(strAno) Then
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_CONSOLIDADO_MES"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            'variable que se llena copn las líneas del archivo
            Dim lstLineasArchivo As New List(Of String) ' Arreglo con el string para escribir en el archivo

            STR_ARCHIVO_DEFECTO = strNombreFormato + CStr(Mes) + CStr(strAno) + ""

            Dim fileDialog = New System.Windows.Forms.SaveFileDialog()
            fileDialog.DefaultExt = ".xlsx"
            fileDialog.Filter = "Excel File(*.xlsx)|*.xlsx"
            fileDialog.FileName = STR_ARCHIVO_DEFECTO


            Dim objRespuesta As InvokeResult(Of List(Of CPX_TransaccionesConsolidado_EXCEL)) = Nothing
            Dim user = Program.Usuario

            If Not IsNothing(strFormato) Then
                Select Case strFormato
                    Case "Excel" 'Excel
                        objRespuesta = Await mdcProxy.TransaccionesConsolidado_ExcelAsync(Mes, False, strAno, user)

                        If Not IsNothing(objRespuesta) Then
                            lstArchivosDianConsolidado_Excel = objRespuesta.Value

                            'Validar que exista información de los formularios
                            If lstArchivosDianConsolidado_Excel.Count = 0 Then
                                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_NOHAYDATOS"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If

                            'RABP20191004_Se convierte  la lista a un datatable para llevar los datos a Excel
                            Dim objDataTable = ConvertirLista_DateTable(lstArchivosDianConsolidado_Excel)
                            strNombreArchivo = fileDialog.FileName
                            'Dialog para obtener la ruta de guardado del archivo
                            Dim result = fileDialog.ShowDialog()
                            Dim strRutaFile As String

                            strRutaFile = Replace(fileDialog.FileName, STR_ARCHIVO_DEFECTO + ".xlsx", "")

                            Dim strArchivo = GenerarExcel(objDataTable, strRutaFile, strNombreArchivo, "FormatosConsolidado")

                            A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_GENERACIONARCHIVOCONEXITO"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                            Exit Select
                        End If

                    Case "Texto"
                        A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_BOTONTEXTO"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        Exit Select
                End Select
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_FORMATODIANCONSOLIDADOTEXTO"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ExportarMovimientosDIANConsolidado_texto", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

    End Sub


    Shared Function GenerarExcel(pobjDatos As DataTable, pstrRuta As String, pstrNombreArchivo As String, Optional pstrNombreHoja As String = "") As String
        Try

            Dim strExtensionNombreArchivo As String = ".xlsx"
            Dim objWorkbook As SpreadsheetGear.IWorkbook = SpreadsheetGear.Factory.GetWorkbook
            Dim objWorkSheet As SpreadsheetGear.IWorksheet = objWorkbook.Worksheets(0)
            objWorkSheet.Name = "Sheet1"
            If Not String.IsNullOrEmpty(pstrNombreHoja) Then
                objWorkSheet.Name = pstrNombreHoja
            End If
            Dim objRange As SpreadsheetGear.IRange = objWorkSheet.Cells(0, 0)

            Dim strAgrupadorMiles As String = objWorkbook.WorkbookSet.Culture.NumberFormat.NumberGroupSeparator
            Dim strSeparadorDecimal As String = objWorkbook.WorkbookSet.Culture.NumberFormat.NumberDecimalSeparator
            Dim strFormatoNumerosExcel As String = "#" & strAgrupadorMiles & "##0" & strSeparadorDecimal & "00"
            Dim strFormatoFechasExcel As String = "dd/mm/yyyy"
            Dim strBackSlash As String = "\"

            Dim strNombreArchivo As String
            Dim strRutaCompletaArchivo As String

            If pstrRuta.Substring(pstrRuta.Length - 2, 1) <> strBackSlash Then
                pstrRuta = pstrRuta & strBackSlash
            End If

            strNombreArchivo = pstrNombreArchivo & strExtensionNombreArchivo
            strRutaCompletaArchivo = pstrRuta & strNombreArchivo


            objRange.CopyFromDataTable(pobjDatos, SpreadsheetGear.Data.SetDataFlags.None)


            For I As Integer = 0 To pobjDatos.Columns.Count - 1
                If pobjDatos.Columns(I).DataType Is GetType(Decimal) Then
                    objWorkSheet.Cells(0, I).EntireColumn.NumberFormat = strFormatoNumerosExcel
                ElseIf pobjDatos.Columns(I).DataType Is GetType(Double) Then
                    objWorkSheet.Cells(0, I).EntireColumn.NumberFormat = strFormatoNumerosExcel
                ElseIf pobjDatos.Columns(I).DataType Is GetType(Date) Then
                    objWorkSheet.Cells(0, I).EntireColumn.NumberFormat = strFormatoFechasExcel
                End If
            Next

            objWorkSheet.UsedRange.Columns.AutoFit()
            objWorkSheet.UsedRange.Columns.AutoFilter()
            objWorkSheet.Cells(0, 0).EntireRow.Interior.Color = SpreadsheetGear.Color.FromArgb(51, 51, 153)
            objWorkSheet.Cells(0, 0).EntireRow.Font.Bold = True
            objWorkSheet.Cells(0, 0).EntireRow.Font.Color = SpreadsheetGear.Color.FromArgb(255, 255, 255)
            objWorkbook.SaveAs(strRutaCompletaArchivo, SpreadsheetGear.FileFormat.OpenXMLWorkbook)

            Return strNombreArchivo
        Catch ex As Exception
            Throw
            Return String.Empty
        End Try

    End Function



    Public Shared Function ConvertirLista_DateTable(Of T)(ByVal list As IList(Of T)) As DataTable
        Dim td As New DataTable
        Dim entityType As Type = GetType(T)
        Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(entityType)

        For Each prop As PropertyDescriptor In properties
            td.Columns.Add(prop.Name)
        Next

        For Each item As T In list
            Dim row As DataRow = td.NewRow()

            For Each prop As PropertyDescriptor In properties
                row(prop.Name) = prop.GetValue(item)
            Next

            td.Rows.Add(row)
        Next

        Return td
    End Function

    ''' <summary>
    ''' Generación del archivo
    ''' </summary>
    Private Async Sub GrabarArchivo()
        Try
            IsBusy = True

            Dim objRespuesta As InvokeResult(Of List(Of CPX_TransaccionesConsolidado)) = Nothing
            Dim user = Program.Usuario
            Dim STR_ARCHIVO_DEFECTO As String
            Dim lngTotalRegistros As Long
            Dim TotalRegistros As Long

            'variable que se llena copn las líneas del archivo
            Dim lstLineasArchivo As New List(Of String) ' Arreglo con el string para escribir en el archivo

            If IsNothing(strFormato) Then
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_CONSOLIDADO_CONSULTAR"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(Mes) Then
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_CONSOLIDADO_ANO"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(strAno) Then
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_CONSOLIDADO_MES"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If


            If Not IsNothing(strFormato) Then
                Select Case strFormato
                    Case "Texto" 'Texto
                        objRespuesta = Await mdcProxy.TransaccionesConsolidado_GenerarAsync(Mes, False, strAno, user)
                        'Validar que exista información de los formularios

                        If Not IsNothing(objRespuesta) Then
                            lstArchivosDianConsolidado = objRespuesta.Value
                            If Not IsNothing(lstArchivosDianConsolidado) AndAlso lstArchivosDianConsolidado.Count > 0 Then
                                mstrDetalle = lstArchivosDianConsolidado.FirstOrDefault.strDetalle
                            End If

                            If lstArchivosDianConsolidado.Count = 0 Then
                                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_NOHAYDATOS"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                        End If

                        STR_ARCHIVO_DEFECTO = strNombreFormato + CStr(Mes) + CStr(strAno) + "" '".SDV"

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

                                    For Each objRegistro In lstArchivosDianConsolidado
                                        mstrDetalle = objRegistro.strDetalle
                                        lstLineasArchivo.Add(mstrDetalle)
                                        TotalRegistros = TotalRegistros + 1
                                    Next

                                    archivo.WriteLine(mstrDetalle)

                                    For Each strLinea In lstLineasArchivo
                                        archivo.WriteLine(strLinea)
                                    Next

                                End Using

                                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_GENERACIONARCHIVOCONEXITO"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                                Exit Select

                            Case System.Windows.Forms.DialogResult.Cancel
                                Exit Select
                            Case Else
                        End Select
                    Case "Excel"
                        A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_BOTONEXCEL"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        Exit Select
                End Select
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_FORMATODIANCONSOLIDADOTEXTO"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ExportarMovimientosDIANConsolidado_texto", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
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
                _ConsultarCmd = New RelayCommand(AddressOf ConsultarMovimientosDianConsolidado)
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

    ''' <summary>
    ''' Comando para la generación del archivo
    ''' </summary>
    Private WithEvents _ArchivoExcel As RelayCommand
    Public ReadOnly Property ArchivoExcel() As RelayCommand
        Get
            If _ArchivoExcel Is Nothing Then
                _ArchivoExcel = New RelayCommand(AddressOf ExportarMovimientosDIANConsolidado_Excel)
            End If
            Return _ArchivoExcel
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
Public Class CamposBusquedaFormatoConsolidado
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