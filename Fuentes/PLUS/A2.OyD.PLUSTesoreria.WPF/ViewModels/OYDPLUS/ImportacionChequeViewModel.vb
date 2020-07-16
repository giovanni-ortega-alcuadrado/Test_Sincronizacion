Imports Telerik.Windows.Controls
'Codigo Creado Por: Rafael Cordero
'Archivo: ImportacionDecevalViewModel.vb
'Generado el : 08/04/2011 
'Propiedad de Alcuadrado S.A. 2011

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports System.Text
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSTesoreria
Imports A2Utilidades.Mensajes
Imports OpenRiaServices.DomainServices.Client

Public Class ImportacionChequeViewModel
    Inherits A2ControlMenu.A2ViewModel

    Private _strNombreArchivoImportado As String = String.Empty
    Public Const GSTR_NOMBRE_PROCESO As String = "TesoreriaPlus"
    Dim dcProxy As ImportacionesDomainContext


    Public Sub New()

        If strFormapagoImportacion = GSTR_CHEQUE Then
            strFormaPago = "Cheques"

            'Plano Cheque|*.csv

            MostrarTipoCheque = Visibility.Visible
        ElseIf strFormapagoImportacion = GSTR_TRANSFERENCIA Then
            strFormaPago = "Transferencia"


            MostrarTipoCheque = Visibility.Collapsed
        End If


        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext
        Else
            dcProxy = New A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
        End If

        Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ImportacionDecevalViewModel.New", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub


    Public Sub MostrarCargadorArchivos()
        Try
            'Dim cwCar As New CargarArchivosView(GSTR_NOMBRE_PROCESO)
            'AddHandler cwCar.Closed, AddressOf VentanaCargaArchivoCerro
            'cwCar.Show()



        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
                                 Me.ToString(), "ImportacionDecevalViewModel.MostrarCargadorArchivos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarResultados()
        If Not Resultados Is Nothing Then Resultados.Clear()
    End Sub

    Public Sub CargarArchivo(pstrFormaPago As String, pArchivo As String)
        Try
            If Not IsNothing(dcProxy.clsChequesOyDPlus) Then
                dcProxy.clsChequesOyDPlus.Clear()
            End If
            If Not IsNothing(dcProxy.clsTransferenciaOyDPlus) Then
                dcProxy.clsTransferenciaOyDPlus.Clear()
            End If
            If String.IsNullOrEmpty(pArchivo) Then
                mostrarMensaje("No se ha Seleccionado ningun Archivo", "Validar Cargar Archivo", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If pstrFormaPago = GSTR_CHEQUE Then
                If String.IsNullOrEmpty(TipoChequeImportacion) Then
                    mostrarMensaje("No se ha Seleccionado ningun Tipo de Cheque", "Validar Cargar Archivo", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                Else

                End If
                IsBusy = True
                dcProxy.Load(dcProxy.CargarArchivoImportacionChequeOyDPlusQuery(pArchivo, False, Nothing, Nothing, GSTR_NOMBRE_PROCESO, Program.Usuario, Nothing, Program.HashConexion), AddressOf TerminoCargarArchivoCheque, "")
            ElseIf pstrFormaPago = GSTR_TRANSFERENCIA Then
                dcProxy.Load(dcProxy.CargarArchivoImportacionTransferenciaOyDPlusQuery(pArchivo, False, Nothing, Nothing, GSTR_NOMBRE_PROCESO, Program.Usuario, Nothing, Program.HashConexion), AddressOf TerminoCargarArchivoTransferencia, "")

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción", _
                                 Me.ToString(), "CargarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Asincronos"
    Sub TerminoCargarArchivoTransferencia(lo As LoadOperation(Of clsTransferenciaOyDPlus))
        Try
            IsBusy = False

            If Not lo.HasError Then

                If dcProxy.clsTransferenciaOyDPlus.Count > 0 Then
                    dcProxy.clsTransferenciaOyDPlus.Remove(dcProxy.clsTransferenciaOyDPlus.First)
                    For Each li In dcProxy.clsTransferenciaOyDPlus
                        If li.NroDocTitular <> objVM_TesoreriaOYDPLUS.TesoreriaOrdenesPlusCE_Selected.strNroDocumento Then
                            li.EsTercero = True
                        Else
                            li.EsTercero = False
                        End If
                    Next

                    objListaTransferencia = dcProxy.clsTransferenciaOyDPlus.ToList

                    If ValidarInformacionArchivoTransferencia(objListaTransferencia) Then 'JBT20140328
                        Dim objListaTesoreriaOrdenesPlusCE_Detalle_Transferencia As New List(Of TesoreriaOyDPlusTransferencia)
                        Dim objAnteriorTransferenciaDetalle As New List(Of TesoreriaOyDPlusTransferencia)
                        Dim intValorMaxDetalle As Integer

                        If Not IsNothing(objVM_TesoreriaOYDPLUS.ListaTesoreriaOrdenesPlusCE_Detalle_Transferencias) Then
                            objListaTesoreriaOrdenesPlusCE_Detalle_Transferencia = objVM_TesoreriaOYDPLUS.ListaTesoreriaOrdenesPlusCE_Detalle_Transferencias
                            If objVM_TesoreriaOYDPLUS.ListaTesoreriaOrdenesPlusCE_Detalle_Transferencias.Count > 0 Then
                                intValorMaxDetalle = objListaTesoreriaOrdenesPlusCE_Detalle_Transferencia.LastOrDefault.lngIDDetalle
                            Else
                                intValorMaxDetalle = 0
                            End If

                            For Each itemeLista In objVM_TesoreriaOYDPLUS.ListaTesoreriaOrdenesPlusCE_Detalle_Transferencias
                                objAnteriorTransferenciaDetalle.Add(itemeLista)
                            Next
                        Else
                            objAnteriorTransferenciaDetalle = Nothing
                            objListaTesoreriaOrdenesPlusCE_Detalle_Transferencia = New List(Of TesoreriaOyDPlusTransferencia)
                            intValorMaxDetalle = 0
                        End If

                        For Each item In objListaTransferencia
                            Dim objDetalleTransferenciaOyDPlus As New TesoreriaOyDPlusTransferencia
                            objDetalleTransferenciaOyDPlus.curValor = Decimal.Parse(item.ValorGiro)
                            objDetalleTransferenciaOyDPlus.strEstado = GSTR_PENDIENTE_Plus_Detalle
                            If item.EsTercero = True Then
                                objDetalleTransferenciaOyDPlus.strIDTipoCliente = GSTR_TERCERO
                                objDetalleTransferenciaOyDPlus.logEsTercero = True
                                objDetalleTransferenciaOyDPlus.strEsTercero = "SI"
                                objDetalleTransferenciaOyDPlus.strTipoCobroGMF = DescripcionGMF_Importacion
                                objDetalleTransferenciaOyDPlus.ValorTipoGMF = IDGMF_Importacion
                                objDetalleTransferenciaOyDPlus.strTipoCobroGMF = DescripcionGMF_Importacion

                                'DEMC20181122 se corrige calculo del campo curvalor en el egreso ya que cuando el GMF se hacia por encima el valor del egreso venia incluido con el GMF lo cual es incorrecto.
                                If IDGMF_Importacion = GSTR_GMF_ENCIMA Then
                                    objDetalleTransferenciaOyDPlus.curValorGMF = Decimal.Parse(item.ValorGiro) * dblGMF_D       'Columna GMF
                                    objDetalleTransferenciaOyDPlus.curValorNeto = Decimal.Parse(item.ValorGiro) + objDetalleTransferenciaOyDPlus.curValorGMF                'Columna valor Giro
                                    objDetalleTransferenciaOyDPlus.curValor = Decimal.Parse(item.ValorGiro)  'Columna valor
                                    objDetalleTransferenciaOyDPlus.ValorTotalNota = objDetalleTransferenciaOyDPlus.curValor + objDetalleTransferenciaOyDPlus.curValorGMF
                                Else
                                    objDetalleTransferenciaOyDPlus.curValorGMF = Decimal.Parse(item.ValorGiro) * dblGMF_D       'Columna GMF
                                    objDetalleTransferenciaOyDPlus.curValorNeto = Decimal.Parse(item.ValorGiro)                 'Columna valor Giro
                                    objDetalleTransferenciaOyDPlus.curValor = Decimal.Parse(item.ValorGiro) - objDetalleTransferenciaOyDPlus.curValorGMF 'Columna valor
                                    objDetalleTransferenciaOyDPlus.ValorTotalNota = objDetalleTransferenciaOyDPlus.curValor
                                End If

                            Else
                                objDetalleTransferenciaOyDPlus.strIDTipoCliente = GSTR_CLIENTE
                                objDetalleTransferenciaOyDPlus.strTipoCobroGMF = String.Empty
                                objDetalleTransferenciaOyDPlus.logEsTercero = False
                                objDetalleTransferenciaOyDPlus.ValorTipoGMF = String.Empty
                                objDetalleTransferenciaOyDPlus.strEsTercero = "NO"
                                objDetalleTransferenciaOyDPlus.curValorGMF = 0
                                objDetalleTransferenciaOyDPlus.curValorNeto = Decimal.Parse(item.ValorGiro)
                                objDetalleTransferenciaOyDPlus.ValorTotalNota = objDetalleTransferenciaOyDPlus.curValorNeto
                            End If

                            objDetalleTransferenciaOyDPlus.dtmFechaActualizacion = Date.Now
                            objDetalleTransferenciaOyDPlus.dtmFechaDocumento = Date.Now
                            objDetalleTransferenciaOyDPlus.lngIDConcepto = Nothing
                            objDetalleTransferenciaOyDPlus.strDetalleConcepto = objVM_TesoreriaOYDPLUS.ConcatenarDetalle(String.Empty, item.Concepto, 0)
                            objDetalleTransferenciaOyDPlus.lngIDTesoreriaEncabezado = objVM_TesoreriaOYDPLUS.TesoreriaOrdenesPlusCE_Selected.lngID
                            objDetalleTransferenciaOyDPlus.logEsTercero = item.EsTercero
                            objDetalleTransferenciaOyDPlus.strFormaPago = GSTR_TRANSFERENCIA
                            objDetalleTransferenciaOyDPlus.strNombreTitular = item.NombreTitular
                            objDetalleTransferenciaOyDPlus.strNroDocumentoTitular = item.NroDocTitular
                            objDetalleTransferenciaOyDPlus.strTipo = GSTR_ORDENGIRO

                            Dim tipoCuenta = Split(objVM_TesoreriaOYDPLUS.TIPOCUENTAOYDPLUS, "|")
                            For Each itemCuenta In tipoCuenta
                                If LTrim(RTrim(item.TipoCuenta.ToUpper)) = itemCuenta.ToUpper Then
                                    objDetalleTransferenciaOyDPlus.strValorTipoCuenta = item.TipoCuenta.Substring(0, 1).ToUpper
                                    objDetalleTransferenciaOyDPlus.strTipoCuenta = item.TipoCuenta.ToUpper
                                    Exit For
                                Else
                                    If objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus.ContainsKey("TIPOCUENTABANCARIA") Then
                                        For Each recorrerTipoCuenta In objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus("TIPOCUENTABANCARIA")
                                            If RTrim(LTrim(item.TipoCuenta.ToUpper)).Substring(0, 1) = recorrerTipoCuenta.Retorno.ToUpper Then
                                                objDetalleTransferenciaOyDPlus.strTipoCuenta = recorrerTipoCuenta.Descripcion.ToUpper
                                                objDetalleTransferenciaOyDPlus.strValorTipoCuenta = recorrerTipoCuenta.Retorno.ToUpper
                                                Exit For

                                            End If

                                        Next
                                    Else

                                    End If
                                End If
                            Next

                            Dim tiposID = Split(objVM_TesoreriaOYDPLUS.TIPODOCUMENTOSIMPORTACION, "|")
                            For Each IDs In tiposID
                                If LTrim(RTrim(item.TipoDocTitular.ToUpper)) = IDs.ToUpper Then
                                    objDetalleTransferenciaOyDPlus.strValorTipoDocumentoTitular = item.TipoDocTitular.Substring(0, 1).ToUpper
                                    objDetalleTransferenciaOyDPlus.strTipoDocumentoTitular = item.TipoDocTitular.ToUpper
                                Else
                                    If objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus.ContainsKey("TIPOID") Then
                                        For Each Recorrer_TIPOID_COMBO In objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus("TIPOID")
                                            If LTrim(RTrim(item.TipoDocTitular.ToUpper)).Substring(0, 1) = Recorrer_TIPOID_COMBO.Retorno.ToUpper Then
                                                objDetalleTransferenciaOyDPlus.strValorTipoDocumentoTitular = Recorrer_TIPOID_COMBO.Retorno.ToUpper
                                                objDetalleTransferenciaOyDPlus.strTipoDocumentoTitular = Recorrer_TIPOID_COMBO.Descripcion.ToUpper
                                            End If
                                        Next
                                    End If

                                End If
                            Next

                            objDetalleTransferenciaOyDPlus.strUsuario = Program.Usuario
                            objDetalleTransferenciaOyDPlus.strCuenta = item.NroCuenta


                            item.EsCuentaNoRegistrada = True 'DEMC20180606
                            objDetalleTransferenciaOyDPlus.strEsCuentaRegistrada = "NO" 'DEMC20180606
                            objDetalleTransferenciaOyDPlus.logEsCuentaRegistrada = False 'DEMC20180606

                            If Not IsNothing(objVM_TesoreriaOYDPLUS.ListaCuentasClientes) Then
                                For Each itemCuenta In objVM_TesoreriaOYDPLUS.ListaCuentasClientes
                                    If RTrim(LTrim(item.NroCuenta)) = itemCuenta.strCuenta And (item.CodigoBanco = itemCuenta.lngIDBanco) Then 'SM20180831
                                        item.EsCuentaNoRegistrada = False
                                        objDetalleTransferenciaOyDPlus.strEsCuentaRegistrada = "SI"
                                        objDetalleTransferenciaOyDPlus.logEsCuentaRegistrada = True
                                        Exit For 'DEMC20180606
                                    End If
                                Next
                            End If


                            If objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus.ContainsKey("TIPOCUENTABANCARIA") Then
                                For Each recorrerTipoCuenta In objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus("TIPOCUENTABANCARIA")
                                    If RTrim(LTrim(objDetalleTransferenciaOyDPlus.strTipoCuenta.ToUpper)).Substring(0, 1) = recorrerTipoCuenta.Retorno.ToUpper Then
                                        objDetalleTransferenciaOyDPlus.strTipoCuenta = recorrerTipoCuenta.Descripcion.ToUpper
                                        objDetalleTransferenciaOyDPlus.strValorTipoCuenta = recorrerTipoCuenta.Retorno.ToUpper
                                        Exit For

                                    End If

                                Next
                            End If

                            objDetalleTransferenciaOyDPlus.lngIdBanco = Integer.Parse(LTrim(RTrim(item.CodigoBanco)))
                            objDetalleTransferenciaOyDPlus.logEsProcesada = True
                            intValorMaxDetalle = intValorMaxDetalle + 1
                            objDetalleTransferenciaOyDPlus.lngIDDetalle = intValorMaxDetalle
                            objListaTesoreriaOrdenesPlusCE_Detalle_Transferencia.Add(objDetalleTransferenciaOyDPlus)

                        Next
                        If Not IsNothing(objListaTesoreriaOrdenesPlusCE_Detalle_Transferencia) Then
                            For Each ItemValidar In objListaTesoreriaOrdenesPlusCE_Detalle_Transferencia
                                If IsNothing(ItemValidar.strValorTipoDocumentoTitular) Then
                                    A2Utilidades.Mensajes.mostrarMensaje("No Fue Posible Cargar el Archivo, Uno de los Registros del Archivo Contiene un Tipo de Documento no Válido o no existe en Base de Datos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    objVM_TesoreriaOYDPLUS.ListaTesoreriaOrdenesPlusCE_Detalle_Transferencias = objAnteriorTransferenciaDetalle
                                    Exit Sub
                                End If

                            Next

                        End If

                        objVM_TesoreriaOYDPLUS.ListaTesoreriaOrdenesPlusCE_Detalle_Transferencias = Nothing
                        objVM_TesoreriaOYDPLUS.ListaTesoreriaOrdenesPlusCE_Detalle_Transferencias = objListaTesoreriaOrdenesPlusCE_Detalle_Transferencia
                        If BorrarArchivoFinalizar Then
                            dcProxy.BorrarArchivo(NombreArchivoSeleccionado, GSTR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion)
                        End If
                        If IDGMF_Importacion = GSTR_GMF_ENCIMA Then
                            If dblGMF_E = 0 Then
                                objVM_TesoreriaOYDPLUS.MostrarMensajeGMF(GSTR_GMF_ENCIMA)


                            End If
                        ElseIf IDGMF_Importacion = GSTR_GMF_DEBAJO Then
                            If dblGMF_D = 0 Then
                                objVM_TesoreriaOYDPLUS.MostrarMensajeGMF(GSTR_GMF_DEBAJO)
                            End If
                        End If
                        objVM_TesoreriaOYDPLUS.objWppImportacion.Close()
                        objVM_TesoreriaOYDPLUS.CalcularTotales(GSTR_TRANSFERENCIA)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No se pudo procesar, Verifique que el Archivo contenga el formato válido, o que no se encuentre vacio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga y procesamiento del Archivo de Cheques", _
                                                 Me.ToString(), "TerminoCargarArchivoTransferencia", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga y procesamiento del Archivo de Cheques", _
                                                 Me.ToString(), "TerminoCargarArchivoTransferencia", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Function ValidarInformacionArchivoTransferencia(ByVal pobjListaDatos As List(Of clsTransferenciaOyDPlus)) As Boolean 'JBT20140328
        Dim logRetornoValidacion As Boolean = True
        Try
            Dim strValorAValidar As String()
            Dim logEncontrado As Boolean = True

            If Not IsNothing(pobjListaDatos) Then
                If pobjListaDatos.Count > 0 Then
                    Dim objListaMensajes As New List(Of String)
                    Dim logPasoValidacion As Boolean = True
                    Dim intContadorFila As Integer = 1
                    objListaMensajes.Add("El archivo genero algunas inconsistencias al intentar subirlo:")

                    For Each li In pobjListaDatos
                        intContadorFila += 1
                        'Valida el  TipoDocTitular
                        If String.IsNullOrEmpty(li.TipoDocTitular) Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El campo es requerido.", intContadorFila, 1))
                            logPasoValidacion = False
                        ElseIf Len(li.TipoDocTitular) > 18 Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: La longitud del campo es de 18.", intContadorFila, 1))
                            logPasoValidacion = False
                        Else
                            strValorAValidar = Split(objVM_TesoreriaOYDPLUS.TIPODOCUMENTOSIMPORTACION, "|")
                            logEncontrado = False

                            For Each itemCuenta In strValorAValidar
                                If LTrim(RTrim(li.TipoDocTitular.ToUpper)) = itemCuenta.ToUpper Then
                                    logEncontrado = True
                                    Exit For
                                End If
                            Next

                            If logEncontrado = False Then
                                If ValidarCampoEnLista(li.TipoDocTitular, "TIPOID", True) Then
                                    logEncontrado = True
                                End If
                            End If

                            If logEncontrado = False Then
                                If ValidarCampoEnLista(li.TipoDocTitular, "TIPOID", False) Then
                                    logEncontrado = False
                                End If
                            End If

                            If logEncontrado = False Then
                                objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El valor no se encuentra dentro de los tipos permitidos.", intContadorFila, 1))
                                logPasoValidacion = False
                            End If
                        End If

                        'Valida el NroDocTitular
                        If String.IsNullOrEmpty(li.NroDocTitular) Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El campo es requerido.", intContadorFila, 2))
                            logPasoValidacion = False
                        ElseIf Len(li.NroDocTitular) > 18 Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: La longitud del campo es de 18.", intContadorFila, 2))
                            logPasoValidacion = False
                        ElseIf ValidarCampoArchivo(li.NroDocTitular, clsExpresiones.TipoExpresion.Caracteres) = False Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El valor contiene caracteres invalidos.", intContadorFila, 2))
                            logPasoValidacion = False
                        End If

                        'Valida el NombreTitular
                        If String.IsNullOrEmpty(li.NombreTitular) Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El campo es requerido.", intContadorFila, 3))
                            logPasoValidacion = False
                        ElseIf Len(li.NombreTitular) > 80 Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: La longitud del campo es de 80.", intContadorFila, 3))
                            logPasoValidacion = False
                        ElseIf ValidarCampoArchivo(li.NombreTitular, clsExpresiones.TipoExpresion.Caracteres) = False Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El valor contiene caracteres invalidos.", intContadorFila, 3))
                            logPasoValidacion = False
                        End If

                        'Valida el TipoCuenta
                        If String.IsNullOrEmpty(li.TipoCuenta) Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El campo es requerido.", intContadorFila, 4))
                            logPasoValidacion = False
                        ElseIf Len(li.TipoCuenta) > 18 Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: La longitud del campo es de 18.", intContadorFila, 4))
                            logPasoValidacion = False
                        Else
                            strValorAValidar = Split(objVM_TesoreriaOYDPLUS.TIPOCUENTAOYDPLUS, "|")
                            logEncontrado = False

                            For Each itemCuenta In strValorAValidar
                                If LTrim(RTrim(li.TipoCuenta.ToUpper)) = itemCuenta.ToUpper Then
                                    logEncontrado = True
                                    Exit For
                                End If
                            Next

                            If logEncontrado = False Then
                                If ValidarCampoEnLista(li.TipoCuenta, "TIPOID", True) Then
                                    logEncontrado = True
                                End If
                            End If

                            If logEncontrado = False Then
                                If ValidarCampoEnLista(li.TipoCuenta, "TIPOID", False) Then
                                    logEncontrado = False
                                End If
                            End If

                            If logEncontrado = False Then
                                objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El valor no se encuentra dentro de los tipos permitidos.", intContadorFila, 4))
                                logPasoValidacion = False
                            End If
                        End If

                        'Valida el CodigoBanco
                        If String.IsNullOrEmpty(li.CodigoBanco) Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El campo es requerido.", intContadorFila, 5))
                            logPasoValidacion = False
                        ElseIf Len(li.CodigoBanco) > 18 Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: La longitud del campo es de 18.", intContadorFila, 5))
                            logPasoValidacion = False
                        ElseIf ValidarCampoArchivo(li.CodigoBanco, clsExpresiones.TipoExpresion.Numeros) = False Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El valor contiene caracteres invalidos.", intContadorFila, 5))
                            logPasoValidacion = False
                        ElseIf ValidarCampoEnLista(li.CodigoBanco, "BANCOSNACIONESTRANSFERENCIAS", True) = False Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El valor no se encuentra dentro de los tipos permitidos.", intContadorFila, 5))
                            logPasoValidacion = False
                        End If

                        'Valida el NroCuenta
                        If String.IsNullOrEmpty(li.NroCuenta) Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El campo es requerido.", intContadorFila, 6))
                            logPasoValidacion = False
                        ElseIf Len(li.NroCuenta) > 18 Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: La longitud del campo es de 18.", intContadorFila, 6))
                            logPasoValidacion = False
                        ElseIf ValidarCampoArchivo(li.NroCuenta, clsExpresiones.TipoExpresion.Caracteres) = False Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El valor contiene caracteres invalidos.", intContadorFila, 6))
                            logPasoValidacion = False
                        End If

                        'Valida el ValorGiro
                        If String.IsNullOrEmpty(li.ValorGiro) Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El campo es requerido.", intContadorFila, 7))
                            logPasoValidacion = False
                        ElseIf Len(li.ValorGiro) > 18 Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: La longitud del campo es de 18.", intContadorFila, 7))
                            logPasoValidacion = False
                        ElseIf ValidarCampoArchivo(li.ValorGiro, clsExpresiones.TipoExpresion.Doble) = False Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El valor contiene caracteres invalidos.", intContadorFila, 7))
                            logPasoValidacion = False
                        End If

                        'Valida el Concepto
                        If String.IsNullOrEmpty(li.Concepto) Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El campo es requerido.", intContadorFila, 8))
                            logPasoValidacion = False
                        ElseIf Len(li.Concepto) > 80 Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: La longitud del campo es de 80.", intContadorFila, 8))
                            logPasoValidacion = False
                        ElseIf ValidarCampoArchivo(li.Concepto, clsExpresiones.TipoExpresion.Caracteres) = False Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El valor contiene caracteres invalidos.", intContadorFila, 8))
                            logPasoValidacion = False
                        End If
                    Next

                    If logPasoValidacion = False Then
                        Dim objValidaciones As New ImportarArchivoRecibos()
                        Program.Modal_OwnerMainWindowsPrincipal(objValidaciones)
                        objValidaciones.ListaMensajes = objListaMensajes
                        objValidaciones.ShowDialog()

                        logRetornoValidacion = False
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el archivo de cheque.", _
                                                 Me.ToString(), "ValidarInformacionArchivoCheque", Application.Current.ToString(), Program.Maquina, ex)
            logRetornoValidacion = False
        End Try
        Return logRetornoValidacion
    End Function

    Sub TerminoCargarArchivoCheque(lo As LoadOperation(Of clsChequesOyDPlus))
        Try


            If Not lo.HasError Then

                If dcProxy.clsChequesOyDPlus.Count > 0 Then
                    objVM_TesoreriaOYDPLUS.logEditarCheque = False
                    dcProxy.clsChequesOyDPlus.Remove(dcProxy.clsChequesOyDPlus.First)

                    For Each li In dcProxy.clsChequesOyDPlus
                        If li.NroDocBeneficiario <> objVM_TesoreriaOYDPLUS.TesoreriaOrdenesPlusCE_Selected.strNroDocumento Then
                            li.EsTercero = True
                        Else
                            li.EsTercero = False
                        End If
                    Next

                    objListaCheques = dcProxy.clsChequesOyDPlus.ToList

                    'Valida sí el archivo contiene la información valida y al primer
                    If ValidarInformacionArchivoCheque(objListaCheques) Then
                        Dim objListaTesoreriaOrdenesPlusCE_Detalle_Cheques As New List(Of TesoreriaOyDPlusCheques)
                        Dim objAnteriorChequeDetalle As New List(Of TesoreriaOyDPlusCheques)
                        Dim intValorMaxDetalle As Integer

                        If Not IsNothing(objVM_TesoreriaOYDPLUS.ListaTesoreriaOrdenesPlusCE_Detalle_Cheques) Then
                            objListaTesoreriaOrdenesPlusCE_Detalle_Cheques = objVM_TesoreriaOYDPLUS.ListaTesoreriaOrdenesPlusCE_Detalle_Cheques
                            If objVM_TesoreriaOYDPLUS.ListaTesoreriaOrdenesPlusCE_Detalle_Cheques.Count > 0 Then
                                intValorMaxDetalle = objListaTesoreriaOrdenesPlusCE_Detalle_Cheques.LastOrDefault.lngIDDetalle
                            Else
                                intValorMaxDetalle = 0
                            End If

                            For Each itemeLista In objVM_TesoreriaOYDPLUS.ListaTesoreriaOrdenesPlusCE_Detalle_Cheques
                                objAnteriorChequeDetalle.Add(itemeLista)
                            Next
                        Else
                            objAnteriorChequeDetalle = Nothing
                            objListaTesoreriaOrdenesPlusCE_Detalle_Cheques = New List(Of TesoreriaOyDPlusCheques)
                            intValorMaxDetalle = 0
                        End If

                        For Each item In objListaCheques
                            Dim objDetalleChequeOyDPlus As New TesoreriaOyDPlusCheques
                            objDetalleChequeOyDPlus.curValor = Decimal.Parse(item.ValorGiro)
                            objDetalleChequeOyDPlus.strEstado = GSTR_PENDIENTE_Plus_Detalle
                            If item.EsTercero = True Then
                                objDetalleChequeOyDPlus.strIDTipoCliente = GSTR_TERCERO
                                objDetalleChequeOyDPlus.logEsTercero = True
                                objDetalleChequeOyDPlus.strEsTercero = "SI"
                                objDetalleChequeOyDPlus.strTipoCobroGMF = DescripcionGMF_Importacion
                                objDetalleChequeOyDPlus.ValorTipoGMF = IDGMF_Importacion
                                objDetalleChequeOyDPlus.strTipoCobroGMF = DescripcionGMF_Importacion

                                'JABG20180613
                                If IDGMF_Importacion = GSTR_GMF_ENCIMA Then
                                    objDetalleChequeOyDPlus.curValorGMF = Decimal.Parse(item.ValorGiro) * dblGMF_E                          'Columna GMF
                                    objDetalleChequeOyDPlus.curValorNeto = Decimal.Parse(item.ValorGiro)                                    'Columna valor Giro
                                    objDetalleChequeOyDPlus.curValor = Decimal.Parse(item.ValorGiro) + objDetalleChequeOyDPlus.curValorGMF  'Columna valor
                                    objDetalleChequeOyDPlus.ValorTotalNota = objDetalleChequeOyDPlus.curValor
                                Else
                                    objDetalleChequeOyDPlus.curValorGMF = Decimal.Parse(item.ValorGiro) * dblGMF_E                              'Columna GMF
                                    objDetalleChequeOyDPlus.curValorNeto = Decimal.Parse(item.ValorGiro)                                        'Columna valor Giro
                                    objDetalleChequeOyDPlus.curValor = Decimal.Parse(item.ValorGiro) - objDetalleChequeOyDPlus.curValorGMF      'Columna valor
                                    objDetalleChequeOyDPlus.ValorTotalNota = objDetalleChequeOyDPlus.curValor
                                End If

                            Else
                                objDetalleChequeOyDPlus.strIDTipoCliente = GSTR_CLIENTE
                                objDetalleChequeOyDPlus.strTipoCobroGMF = String.Empty
                                objDetalleChequeOyDPlus.logEsTercero = False
                                objDetalleChequeOyDPlus.ValorTipoGMF = String.Empty
                                objDetalleChequeOyDPlus.strEsTercero = "NO"
                                objDetalleChequeOyDPlus.curValorGMF = 0
                                objDetalleChequeOyDPlus.curValorNeto = Decimal.Parse(item.ValorGiro)
                                objDetalleChequeOyDPlus.ValorTotalNota = objDetalleChequeOyDPlus.curValorNeto
                            End If

                            objDetalleChequeOyDPlus.dtmFechaActualizacion = Date.Now
                            objDetalleChequeOyDPlus.dtmFechaDocumento = Date.Now
                            objDetalleChequeOyDPlus.lngIDConcepto = Nothing
                            objDetalleChequeOyDPlus.strDetalleConcepto = objVM_TesoreriaOYDPLUS.ConcatenarDetalle(String.Empty, item.Concepto, 0)
                            objDetalleChequeOyDPlus.lngIDTesoreriaEncabezado = objVM_TesoreriaOYDPLUS.TesoreriaOrdenesPlusCE_Selected.lngID
                            objDetalleChequeOyDPlus.logEsTercero = item.EsTercero
                            objDetalleChequeOyDPlus.strFormaPago = GSTR_CHEQUE
                            objDetalleChequeOyDPlus.strNombre = item.NombreBeneficiario
                            objDetalleChequeOyDPlus.strNroDocumento = item.NroDocBeneficiario
                            objDetalleChequeOyDPlus.strTipo = GSTR_ORDENGIRO
                            objDetalleChequeOyDPlus.strTipoCheque = Descripcion_TipoChequeImportacion

                            'objDetalleChequeOyDPlus.strTipoCruce = item.TipoCruce


                            Dim tiposID = Split(objVM_TesoreriaOYDPLUS.TIPODOCUMENTOSIMPORTACION, "|")

                            For Each IDs In tiposID
                                If LTrim(RTrim(item.TipoDocBeneficiario.ToUpper)) = IDs.ToUpper Then
                                    objDetalleChequeOyDPlus.ValorTipoDocumento = item.TipoDocBeneficiario.Substring(0, 1).ToUpper
                                Else
                                    If objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus.ContainsKey("TIPOID") Then
                                        For Each Recorrer_TIPOID_COMBO In objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus("TIPOID")
                                            If LTrim(RTrim(item.TipoDocBeneficiario.ToUpper)).Substring(0, 1) = Recorrer_TIPOID_COMBO.Retorno.ToUpper Then
                                                objDetalleChequeOyDPlus.ValorTipoDocumento = Recorrer_TIPOID_COMBO.Retorno()
                                                objDetalleChequeOyDPlus.strTipoDocumento = Recorrer_TIPOID_COMBO.Descripcion
                                            End If

                                        Next
                                    End If

                                End If
                            Next



                            If TipoChequeImportacion = GSTR_CHEQUE Then



                                Dim tiposCruce = Split(objVM_TesoreriaOYDPLUS.TIPOSCRUCEIMPORTACION, "|")

                                If Not IsNothing(objVM_TesoreriaOYDPLUS.TIPOSCRUCEIMPORTACION) Then

                                    For Each TCs In tiposCruce
                                        If LTrim(RTrim(item.TipoCruce.ToUpper)) = TCs.ToUpper Then
                                            objDetalleChequeOyDPlus.ValorTipoCruce = item.TipoCruce.Substring(0, 1).ToUpper
                                        Else
                                            If objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus.ContainsKey("TIPOCRUCE") Then
                                                For Each Recorrer_TIPOSELLO_COMBO In objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus("TIPOCRUCE")
                                                    If LTrim(RTrim(item.TipoCruce.ToUpper)).Substring(0, 1) = Recorrer_TIPOSELLO_COMBO.Retorno.ToUpper Then
                                                        objDetalleChequeOyDPlus.ValorTipoCruce = Recorrer_TIPOSELLO_COMBO.Retorno()
                                                        objDetalleChequeOyDPlus.strTipoCruce = Recorrer_TIPOSELLO_COMBO.Descripcion
                                                        If TipoChequeImportacion = GSTR_CHEQUE_GERENCIA Then
                                                            objDetalleChequeOyDPlus.ValorTipoCruce = Nothing
                                                            objDetalleChequeOyDPlus.strTipoCruce = Nothing
                                                        End If
                                                    End If

                                                Next
                                            End If

                                        End If
                                    Next
                                End If
                            End If


                            objDetalleChequeOyDPlus.strUsuario = Program.Usuario

                            If Not String.IsNullOrEmpty(TipoChequeImportacion) Then
                                objDetalleChequeOyDPlus.ValorTipoCheque = TipoChequeImportacion
                            End If

                            If TipoChequeImportacion = GSTR_CHEQUE Then
                                If Not String.IsNullOrEmpty(TipoCruce) Then
                                    objDetalleChequeOyDPlus.ValorTipoCruce = TipoCruce
                                    objDetalleChequeOyDPlus.strTipoCruce = DescripcionCruce

                                End If
                            End If
                            objDetalleChequeOyDPlus.logEsProcesada = True
                            intValorMaxDetalle = intValorMaxDetalle + 1
                            objDetalleChequeOyDPlus.lngIDDetalle = intValorMaxDetalle
                            objListaTesoreriaOrdenesPlusCE_Detalle_Cheques.Add(objDetalleChequeOyDPlus)

                        Next
                        If Not IsNothing(objListaTesoreriaOrdenesPlusCE_Detalle_Cheques) Then
                            For Each ItemValidar In objListaTesoreriaOrdenesPlusCE_Detalle_Cheques
                                If IsNothing(ItemValidar.ValorTipoDocumento) Then
                                    A2Utilidades.Mensajes.mostrarMensaje("No Fue Posible Cargar el Archivo, Uno de los Registros del Archivo Contiene un Tipo de Documento no Válido o no existe en Base de Datos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    objVM_TesoreriaOYDPLUS.ListaTesoreriaOrdenesPlusCE_Detalle_Cheques = objAnteriorChequeDetalle
                                    Exit Sub
                                End If

                            Next

                        End If
                        If TipoChequeImportacion = GSTR_CHEQUE Then
                            If Not IsNothing(objListaTesoreriaOrdenesPlusCE_Detalle_Cheques) Then
                                For Each ItemValidar In objListaTesoreriaOrdenesPlusCE_Detalle_Cheques
                                    If IsNothing(ItemValidar.ValorTipoCruce) Then
                                        A2Utilidades.Mensajes.mostrarMensaje("No Fue Posible Cargar el Archivo, Uno de los Registros del Archivo Contiene un Tipo de Cruce no Válido o no existe en Base de Datos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        objVM_TesoreriaOYDPLUS.ListaTesoreriaOrdenesPlusCE_Detalle_Cheques = objAnteriorChequeDetalle
                                        Exit Sub
                                    End If

                                Next

                            End If
                        End If




                        objVM_TesoreriaOYDPLUS.ListaTesoreriaOrdenesPlusCE_Detalle_Cheques = Nothing
                        objVM_TesoreriaOYDPLUS.ListaTesoreriaOrdenesPlusCE_Detalle_Cheques = objListaTesoreriaOrdenesPlusCE_Detalle_Cheques

                        If BorrarArchivoFinalizar Then
                            dcProxy.BorrarArchivo(NombreArchivoSeleccionado, GSTR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion)

                        End If
                        IsBusy = False
                        objVM_TesoreriaOYDPLUS.objWppImportacion.Close()
                        objVM_TesoreriaOYDPLUS.CalcularTotales(GSTR_CHEQUE)
                        If IDGMF_Importacion = GSTR_GMF_ENCIMA Then
                            If dblGMF_E = 0 Then
                                objVM_TesoreriaOYDPLUS.MostrarMensajeGMF(GSTR_GMF_ENCIMA)


                            End If
                        ElseIf IDGMF_Importacion = GSTR_GMF_DEBAJO Then
                            If dblGMF_D = 0 Then
                                objVM_TesoreriaOYDPLUS.MostrarMensajeGMF(GSTR_GMF_DEBAJO)
                            End If
                        End If
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No se pudo procesar, Verifique que el Archivo contenga el formato válido, o que no se encuentre vacio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga y procesamiento del Archivo de Cheques", _
                                                 Me.ToString(), "TerminoCargarArchivoCheque", Application.Current.ToString(), Program.Maquina, lo.Error)

            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga y procesamiento del Archivo de Cheques", _
                                                 Me.ToString(), "TerminoCargarArchivoCheque", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function ValidarInformacionArchivoCheque(ByVal pobjListaDatos As List(Of clsChequesOyDPlus)) As Boolean
        Dim logRetornoValidacion As Boolean = True
        Try
            Dim strValorAValidar As String()
            Dim strValorAValidarTipoCruce As String()
            Dim logEncontrado As Boolean = True

            If Not IsNothing(pobjListaDatos) Then
                If pobjListaDatos.Count > 0 Then
                    Dim objListaMensajes As New List(Of String)
                    Dim logPasoValidacion As Boolean = True
                    Dim intContadorFila As Integer = 1
                    objListaMensajes.Add("El archivo genero algunas inconsistencias al intentar subirlo:")

                    For Each li In pobjListaDatos
                        intContadorFila += 1

                        'Valida el tipo beneficiario
                        If String.IsNullOrEmpty(li.TipoDocBeneficiario) Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El campo es requerido.", intContadorFila, 1))
                            logPasoValidacion = False
                        ElseIf Len(li.TipoDocBeneficiario) > 18 Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: La longitud del campo es de 18.", intContadorFila, 1))
                            logPasoValidacion = False
                        Else
                            strValorAValidar = Split(objVM_TesoreriaOYDPLUS.TIPODOCUMENTOSIMPORTACION, "|")
                            logEncontrado = False

                            For Each itemCuenta In strValorAValidar
                                If LTrim(RTrim(li.TipoDocBeneficiario.ToUpper)) = itemCuenta.ToUpper Then
                                    logEncontrado = True
                                    Exit For
                                End If
                            Next

                            If logEncontrado = False Then
                                If ValidarCampoEnLista(li.TipoDocBeneficiario, "TIPOID", True) Then
                                    logEncontrado = True
                                End If
                            End If

                            If logEncontrado = False Then
                                If ValidarCampoEnLista(li.TipoDocBeneficiario, "TIPOID", False) Then
                                    logEncontrado = False
                                End If
                            End If

                            If logEncontrado = False Then
                                objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El valor no se encuentra dentro de los tipos permitidos.", intContadorFila, 1))
                                logPasoValidacion = False
                            End If
                        End If

                        'Valida el nro documento benerificiario
                        If String.IsNullOrEmpty(li.NroDocBeneficiario) Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El campo es requerido.", intContadorFila, 2))
                            logPasoValidacion = False
                        ElseIf Len(li.NroDocBeneficiario) > 18 Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: La longitud del campo es de 18.", intContadorFila, 2))
                            logPasoValidacion = False
                        ElseIf ValidarCampoArchivo(li.NroDocBeneficiario, clsExpresiones.TipoExpresion.Caracteres) = False Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El valor contiene caracteres invalidos.", intContadorFila, 2))
                            logPasoValidacion = False
                        End If

                        'Valida el nombre
                        If String.IsNullOrEmpty(li.NombreBeneficiario) Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El campo es requerido.", intContadorFila, 3))
                            logPasoValidacion = False
                        ElseIf Len(li.NombreBeneficiario) > 80 Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: La longitud del campo es de 80.", intContadorFila, 3))
                            logPasoValidacion = False
                        ElseIf ValidarCampoArchivo(li.NombreBeneficiario, clsExpresiones.TipoExpresion.Caracteres) = False Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El valor contiene caracteres invalidos.", intContadorFila, 3))
                            logPasoValidacion = False
                        End If

                        'Valida el valor de giro
                        If String.IsNullOrEmpty(li.ValorGiro) Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El campo es requerido.", intContadorFila, 4))
                            logPasoValidacion = False
                        ElseIf Len(li.ValorGiro) > 18 Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: La longitud del campo es de 18.", intContadorFila, 4))
                            logPasoValidacion = False
                        ElseIf ValidarCampoArchivo(li.ValorGiro, clsExpresiones.TipoExpresion.Doble) = False Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El valor contiene caracteres invalidos.", intContadorFila, 4))
                            logPasoValidacion = False
                        End If

                        'Valida el valor de giro
                        If String.IsNullOrEmpty(li.Concepto) Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El campo es requerido.", intContadorFila, 5))
                            logPasoValidacion = False
                        ElseIf Len(li.Concepto) > 80 Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: La longitud del campo es de 80.", intContadorFila, 5))
                            logPasoValidacion = False
                        ElseIf ValidarCampoArchivo(li.Concepto, clsExpresiones.TipoExpresion.Caracteres) = False Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El valor contiene caracteres invalidos.", intContadorFila, 5))
                            logPasoValidacion = False
                        End If

                        'Valida tipo cruce
                        If TipoChequeImportacion = GSTR_CHEQUE Then
                            If String.IsNullOrEmpty(li.TipoCruce) Then
                                objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El campo es requerido.", intContadorFila, 6))
                                logPasoValidacion = False
                            ElseIf Len(li.TipoCruce) > 80 Then
                                objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: La longitud del campo es de 80.", intContadorFila, 6))
                                logPasoValidacion = False
                            ElseIf ValidarCampoArchivo(li.TipoCruce, clsExpresiones.TipoExpresion.Caracteres) = False Then
                                objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El valor contiene caracteres invalidos.", intContadorFila, 6))
                                logPasoValidacion = False

                            Else
                                strValorAValidarTipoCruce = Split(objVM_TesoreriaOYDPLUS.TIPOSCRUCEIMPORTACION, "|")
                                logEncontrado = False

                                For Each itemCruce In strValorAValidarTipoCruce
                                    If LTrim(RTrim(li.TipoCruce.ToUpper)) = itemCruce.ToUpper Then
                                        logEncontrado = True
                                        Exit For

                                    End If
                                Next
                                Exit For

                                If logEncontrado = False Then
                                    If ValidarCampoEnLista(li.TipoCruce, "TIPOCRUCE", True) Then
                                        logEncontrado = True
                                    End If
                                End If

                                If logEncontrado = False Then
                                    If ValidarCampoEnLista(li.TipoCruce, "TIPOCRUCE", False) Then
                                        logEncontrado = False
                                    End If
                                End If

                                If logEncontrado = False Then
                                    objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: El valor no se encuentra dentro de los tipos permitidos.", intContadorFila, 6))
                                    logPasoValidacion = False
                                End If
                            End If
                        End If
                    Next
                    If logPasoValidacion = False Then
                        Dim objValidaciones As New ImportarArchivoRecibos()
                        Program.Modal_OwnerMainWindowsPrincipal(objValidaciones)
                        objValidaciones.ListaMensajes = objListaMensajes
                        objValidaciones.ShowDialog()

                        logRetornoValidacion = False
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el archivo de cheque.", _
                                                 Me.ToString(), "ValidarInformacionArchivoCheque", Application.Current.ToString(), Program.Maquina, ex)
            logRetornoValidacion = False
        End Try
        Return logRetornoValidacion
    End Function

    Private Function ValidarCampoArchivo(ByVal pstrValor As String, ByVal pobjTipoExpresion As clsExpresiones.TipoExpresion) As Boolean
        Dim logPasoValidacion As Boolean = True

        If pobjTipoExpresion = clsExpresiones.TipoExpresion.Numeros Then
            logPasoValidacion = clsExpresiones.ValidarConversionNumero(pstrValor)
        ElseIf pobjTipoExpresion = clsExpresiones.TipoExpresion.Doble Then
            logPasoValidacion = clsExpresiones.ValidarConversionDoble(pstrValor)
        Else
            Dim objValidacionExpresion As TextoFormateado = Nothing
            objValidacionExpresion = clsExpresiones.ValidarCaracteresEnCadena(pstrValor, pobjTipoExpresion)
            If Not IsNothing(objValidacionExpresion) Then
                logPasoValidacion = objValidacionExpresion.TextoValido
            End If
        End If

        Return logPasoValidacion
    End Function

    Private Function ValidarCampoEnLista(ByVal pstrValor As String, ByVal pstrTopico As String, ByVal plogValidarContraID As Boolean) As Boolean
        Dim logPasoValidacion As Boolean = False

        If objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus.ContainsKey(pstrTopico) Then
            If plogValidarContraID Then
                If objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus(pstrTopico).Where(Function(i) i.Retorno.ToUpper = pstrValor.ToUpper).Count > 0 Then
                    logPasoValidacion = True
                End If
            Else
                If objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus(pstrTopico).Where(Function(i) i.Descripcion.ToUpper = pstrValor.ToUpper).Count > 0 Then
                    logPasoValidacion = True
                End If
            End If
        End If

        Return logPasoValidacion
    End Function

    Private _NombreArchivoSeleccionado As String
    Public Property NombreArchivoSeleccionado() As String
        Get
            Return _NombreArchivoSeleccionado
        End Get
        Set(ByVal value As String)
            _NombreArchivoSeleccionado = value
            CambioItem("NombreArchivoSeleccionado")
        End Set
    End Property



#End Region


#Region "Propiedades"
    Private _BorrarArchivoFinalizar As Boolean
    Public Property BorrarArchivoFinalizar() As Boolean
        Get
            Return _BorrarArchivoFinalizar
        End Get
        Set(ByVal value As Boolean)
            _BorrarArchivoFinalizar = value
            MyBase.CambioItem("BorrarArchivoFinalizar")
        End Set
    End Property



    Private _DescripcionCruce As String
    Public Property DescripcionCruce() As String
        Get
            Return _DescripcionCruce
        End Get
        Set(ByVal value As String)
            _DescripcionCruce = value
            MyBase.CambioItem("DescripcionCruce")
        End Set
    End Property



    Private _TipoCruce As String
    Public Property TipoCruce() As String
        Get
            Return _TipoCruce
        End Get
        Set(ByVal value As String)
            _TipoCruce = value
            If Not IsNothing(_TipoCruce) Then
                If Not IsNothing(objVM_TesoreriaOYDPLUS) Then
                    If Not IsNothing(objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus) Then
                        If objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus.ContainsKey("TIPOCRUCE") Then
                            If objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus("TIPOCRUCE").Where(Function(i) i.Retorno = _TipoCruce).Count > 0 Then
                                DescripcionCruce = objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus("TIPOCRUCE").Where(Function(i) i.Retorno = _TipoCruce).First.Descripcion
                            End If
                        End If
                    End If
                End If
            End If
            MyBase.CambioItem("TipoCruce")
        End Set
    End Property

    Private _MostrarTipoCheque As Visibility = Visibility.Collapsed
    Public Property MostrarTipoCheque() As Visibility
        Get
            Return _MostrarTipoCheque
        End Get
        Set(ByVal value As Visibility)
            _MostrarTipoCheque = value
            MyBase.CambioItem("MostrarTipoCheque")
        End Set
    End Property


    Private _HabilitarTipoCruce As Visibility = Visibility.Collapsed
    Public Property HabilitarTipoCruce() As Visibility
        Get
            Return _HabilitarTipoCruce
        End Get
        Set(ByVal value As Visibility)
            _HabilitarTipoCruce = value
            MyBase.CambioItem("HabilitarTipoCruce")
        End Set
    End Property


    Private _TipoChequeImportacion As String
    Public Property TipoChequeImportacion() As String
        Get
            Return _TipoChequeImportacion
        End Get
        Set(ByVal value As String)
            _TipoChequeImportacion = value
            If Not String.IsNullOrEmpty(_TipoChequeImportacion) Then
                If _TipoChequeImportacion = GSTR_CHEQUE Then
                    Descripcion_TipoChequeImportacion = GSTR_DESCRIPCION_CHEQUE
                    HabilitarTipoCruce = Visibility.Visible
                ElseIf _TipoChequeImportacion = GSTR_CHEQUE_GERENCIA Then
                    HabilitarTipoCruce = Visibility.Collapsed
                    Descripcion_TipoChequeImportacion = GSTR_DESCRIPCION_CHEQUE_GERENCIA
                End If
            End If
            MyBase.CambioItem("TipoChequeImportacion")
        End Set
    End Property

    Private _Descripcion_TipoChequeImportacion As String
    Public Property Descripcion_TipoChequeImportacion() As String
        Get
            Return _Descripcion_TipoChequeImportacion
        End Get
        Set(ByVal value As String)
            _Descripcion_TipoChequeImportacion = value
            MyBase.CambioItem("Descripcion_TipoChequeImportacion")
        End Set
    End Property

    Private _IDGMF_Importacion As String
    Public Property IDGMF_Importacion() As String
        Get
            Return _IDGMF_Importacion
        End Get
        Set(ByVal value As String)
            _IDGMF_Importacion = value
            If Not String.IsNullOrEmpty(_IDGMF_Importacion) Then
                If _objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus.ContainsKey("TIPOGMF") Then
                    For Each li In _objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus("TIPOGMF")
                        If li.Retorno = _IDGMF_Importacion Then
                            DescripcionGMF_Importacion = li.Descripcion
                        End If
                    Next
                End If
            End If
            MyBase.CambioItem("IDGMF_Importacion")
        End Set
    End Property

    Private _DescripcionGMF_Importacion As String
    Public Property DescripcionGMF_Importacion() As String
        Get
            Return _DescripcionGMF_Importacion
        End Get
        Set(ByVal value As String)
            _DescripcionGMF_Importacion = value
            MyBase.CambioItem("DescripcionGMF_Importacion")
        End Set
    End Property


    Private _objVM_TesoreriaOYDPLUS As TesoreriaViewModel_OYDPLUS
    Public Property objVM_TesoreriaOYDPLUS() As TesoreriaViewModel_OYDPLUS
        Get
            Return _objVM_TesoreriaOYDPLUS
        End Get
        Set(ByVal value As TesoreriaViewModel_OYDPLUS)
            _objVM_TesoreriaOYDPLUS = value
            If Not IsNothing(_objVM_TesoreriaOYDPLUS) Then
                If _objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus.ContainsKey("TIPOGMF") Then
                    If _objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus("TIPOGMF").Count = 1 Then
                        IDGMF_Importacion = _objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus("TIPOGMF").FirstOrDefault.Retorno
                        DescripcionGMF_Importacion = _objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus("TIPOGMF").FirstOrDefault.Descripcion
                    End If
                    If _objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus("TIPOCHEQUE").Count = 1 Then
                        TipoChequeImportacion = _objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus("TIPOCHEQUE").FirstOrDefault.Retorno
                        Descripcion_TipoChequeImportacion = _objVM_TesoreriaOYDPLUS.DiccionarioCombosOYDPlus("TIPOCHEQUE").FirstOrDefault.Descripcion
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("El Receptor Seleccionado, no posee configuraciones para el Tipo GMF", "Carga Archivo", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

            End If

            MyBase.CambioItem("objVM_TesoreriaOYDPLUS")
        End Set
    End Property


    Private _strFormaPago As String
    Public Property strFormaPago() As String
        Get
            Return _strFormaPago
        End Get
        Set(ByVal value As String)
            _strFormaPago = value
            MyBase.CambioItem("strFormaPago")
        End Set
    End Property
    Private _strResultados As New StringBuilder
    Public Property Resultados() As StringBuilder
        Get
            Return _strResultados
        End Get
        Set(ByVal value As StringBuilder)
            _strResultados = value
            MyBase.CambioItem("Resultados")
        End Set
    End Property

    Private _ListaArchivos As List(Of Archivo)
    Public Property ListaArchivos() As List(Of Archivo)
        Get
            Return _ListaArchivos
        End Get
        Set(ByVal value As List(Of Archivo))
            _ListaArchivos = value

            ArchivoSeleccionado = _ListaArchivos.FirstOrDefault
            MyBase.CambioItem("ListaArchivos")
        End Set
    End Property

    Private _ArchivoSeleccionado As Archivo
    Public Property ArchivoSeleccionado() As Archivo
        Get
            Return _ArchivoSeleccionado
        End Get
        Set(ByVal value As Archivo)
            _ArchivoSeleccionado = value
            MyBase.CambioItem("ArchivoSeleccionado")
        End Set
    End Property

    Private _objListaCheques As List(Of clsChequesOyDPlus)
    Public Property objListaCheques() As List(Of clsChequesOyDPlus)
        Get
            Return _objListaCheques
        End Get
        Set(ByVal value As List(Of clsChequesOyDPlus))
            If Not IsNothing(value) Then
                _objListaCheques = value
                MyBase.CambioItem("objListaCheques")
            End If
        End Set
    End Property
    Private _objListaTransferencia As List(Of clsTransferenciaOyDPlus)
    Public Property objListaTransferencia() As List(Of clsTransferenciaOyDPlus)
        Get
            Return _objListaTransferencia
        End Get
        Set(ByVal value As List(Of clsTransferenciaOyDPlus))
            If Not IsNothing(value) Then
                _objListaTransferencia = value
                MyBase.CambioItem("objListaTransferencia")
            End If
        End Set
    End Property
#End Region

End Class
