Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ActImportacionLiqViewModel.vb
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


Public Class ArregloUBICACIONTITULOViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property LiquidacionArreglo As New TituloArregloUbicacion
    Public Property ClaseSeleccionada As New A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo
    Public Property TipoSeleccionado As New A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo
    Public Property UbicacionSeleccionado As New A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo

    Dim dcProxy As BolsaDomainContext
    Private _ListaCuentasCliente As CuentasDepositoCliente
    Private _Validaciones As Boolean = True

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New BolsaDomainContext()
        Else
            dcProxy = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If
        'JFSB 20160207
        LiquidacionArreglo.FechaLiquidacion = Now.AddDays(-1)

    End Sub

    Public Sub NroCuentaSeleccionada(pintNroCuenta As Integer)
        LiquidacionArreglo.NroCuenta = pintNroCuenta
    End Sub

    Public Sub Ejecutar()
        Dim Ejecuta = Validaciones()
    End Sub

    Public Sub BuscarCuentasDeposito()
        If Validaciones(False) Then
            With LiquidacionArreglo
                dcProxy.CuentasDepositoClientes.Clear()
                dcProxy.Load(dcProxy.CuentasDepositoClienteQuery(.NroLiq, .NroParcial, .TipoOperacion, .ClasePapel, .FechaLiquidacion, .UBICACIONTITULO, Program.Usuario, Program.HashConexion), AddressOf TerminaCargarCuentasDepositoCliente, "")
            End With
        End If
    End Sub

    Private Sub TerminaCargarCuentasDepositoCliente(lo As LoadOperation(Of CuentasDepositoCliente))
        If Not lo.HasError Then
            ListaCuentasClientes = dcProxy.CuentasDepositoClientes
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de las cuentas del cliente", _
                                             Me.ToString(), "TerminaCargarCuentasDepositoCliente_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private _ListaCuentasClientes As EntitySet(Of CuentasDepositoCliente)
    Public Property ListaCuentasClientes() As EntitySet(Of CuentasDepositoCliente)
        Get
            Return _ListaCuentasClientes
        End Get
        Set(ByVal value As EntitySet(Of CuentasDepositoCliente))
            _ListaCuentasClientes = value
            MyBase.CambioItem("ListaCuentasClientes")
        End Set
    End Property

    Public ReadOnly Property FechaMostrar() As Date
        Get
            Return Now
        End Get
    End Property

    Private _ResultadoActualizacion As EntitySet(Of ValidacionCustodiaLiq)
    Public Property ResultadoActualizacion() As EntitySet(Of ValidacionCustodiaLiq)
        Get
            Return _ResultadoActualizacion
        End Get
        Set(ByVal value As EntitySet(Of ValidacionCustodiaLiq))
            _ResultadoActualizacion = value
            MyBase.CambioItem("ResultadoActualizacion")
        End Set
    End Property

    Private _ResultadoValidacion As EntitySet(Of ValidacionCustodiaLiq)
    Public Property ResultadoValidacion() As EntitySet(Of ValidacionCustodiaLiq)
        Get
            Return _ResultadoValidacion
        End Get
        Set(ByVal value As EntitySet(Of ValidacionCustodiaLiq))
            _ResultadoValidacion = value
            MyBase.CambioItem("ResultadoValidacion")
        End Set
    End Property

    Private Function Validaciones(Optional pbolValidarNroCta As Boolean = True) As Boolean
        '/******************************************************************************************
        '/* INICIO DOCUMENTO
        '/* Function Validaciones() As Boolean
        '/* Alcance     :   Private
        '/* Descripción :   Realiza validaciones de obligatoriedad y de correspondencia de tipos
        '/*                 en la entrada de los datos.
        '/* Parámetros  :
        '/* Valores de retorno:
        '/* Validaciones:   Devuelve true si todos los controles tienen información valida.
        '/*                 Devuelve false si los controles tienen información NO valida.
        '/* FIN DOCUMENTO
        '/******************************************************************************************
        Dim lngContar As Long = 0, strTabla As String = String.Empty
        Dim strMensaje = String.Empty

        Validaciones = True

        If IsNothing(LiquidacionArreglo.NroLiq) Or LiquidacionArreglo.NroLiq <= 0 Then
            strMensaje = strMensaje & vbCrLf & "** El número de la liquidación es requerido"
            Validaciones = False
        End If

        If IsNothing(LiquidacionArreglo.NroParcial) Or LiquidacionArreglo.NroParcial < 0 Then
            strMensaje = strMensaje & vbCrLf & "** El número del parcial es requerido"
            Validaciones = False
        End If

        If IsNothing(LiquidacionArreglo.TipoOperacion) Then
            strMensaje = strMensaje & vbCrLf & "** Seleccione de la Lista un tipo de Operación"
            Validaciones = False
        End If

        If IsNothing(LiquidacionArreglo.ClasePapel) Then
            strMensaje = strMensaje & vbCrLf & "** Seleccione la clase a la que pertenece la liquidación"
            Validaciones = False
        End If

        If Not IsDate(LiquidacionArreglo.FechaLiquidacion) Or LiquidacionArreglo.FechaLiquidacion > Now.Date Then
            strMensaje = strMensaje & vbCrLf & "** Digite una fecha valida"
            Validaciones = False
        End If

        If IsNothing(LiquidacionArreglo.UBICACIONTITULO) Or LiquidacionArreglo.UBICACIONTITULO Is Nothing Then
            strMensaje = strMensaje & vbCrLf & "** Seleccione Físico o Depósito"
            Validaciones = False
        End If

        If (IsNothing(LiquidacionArreglo.NroCuenta) Or LiquidacionArreglo.NroCuenta <= 0) And pbolValidarNroCta Then
            strMensaje = strMensaje & vbCrLf & "** Seleccione la cuenta del depósito"
            Validaciones = False
        End If

        _Validaciones = Validaciones

        If Not _Validaciones Then
            Dim strMensajeMostrar = "Se presentaron las siguientes inconsistencias: " & vbCrLf & vbCrLf & strMensaje
            A2Utilidades.Mensajes.mostrarMensaje(strMensajeMostrar, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
            Return Validaciones
            Exit Function
        ElseIf pbolValidarNroCta Then
            With LiquidacionArreglo
                IsBusy = True
                dcProxy.ValidacionCustodiaLiqs.Clear()
                dcProxy.Load(dcProxy.ValidarSiLiquidacionPuedeModificarseQuery(.NroLiq, .NroParcial, .TipoOperacion, .ClasePapel, .FechaLiquidacion, Program.Usuario, Program.HashConexion), AddressOf TerminaValidacion, "")
            End With
        End If

    End Function

    Private Sub TerminaValidacion(lo As LoadOperation(Of ValidacionCustodiaLiq))
        IsBusy = False

        If Not lo.HasError Then
            ResultadoValidacion = dcProxy.ValidacionCustodiaLiqs

            If ResultadoValidacion(0).intResultado > 0 Then
                _Validaciones = False
                A2Utilidades.Mensajes.mostrarMensaje("Esta liquidacion ya fue " & ResultadoValidacion(0).strTabla & " no se puede modificar la cuenta del deposito", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
            End If

            If _Validaciones Then
                SalvarDatos()
            End If

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de las cuentas del cliente", _
                                             Me.ToString(), "TerminaCargarCuentasDepositoCliente_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub SalvarDatos()
        '        '/******************************************************************************************
        '        '/* INICIO DOCUMENTO
        '        '/* Sub SalvarDatos
        '        '/* Alcance     :   Private
        '        '/* Descripción :   Asigna los valores al prepared statment para actualizar una ubicacion de
        '        '/*                 título en una operación y graba un log de dicha operación.
        '        '/* Parámetros  :
        '        '/* Valores de retorno:
        '        '/* FIN DOCUMENTO
        '        '/******************************************************************************************

        With LiquidacionArreglo
            IsBusy = True
            dcProxy.ValidacionCustodiaLiqs.Clear()
            dcProxy.Load(dcProxy.ArregloUBICACIONTITULOQuery(.NroLiq, .NroParcial, .TipoOperacion, .ClasePapel, .FechaLiquidacion, .UBICACIONTITULO, .NroCuenta, Program.Usuario, 0, Program.HashConexion), AddressOf TerminarActualizacion, "")
        End With


        'Dim intArchivo = FreeFile
        ''Abre el archivo para el llevar un log de este proceso
        'Open Application.Path & "\Ubicacion.txt" For Append Access Write As intArchivo
        'Write #intArchivo, strCadena
        'Close #intArchivo


    End Sub

    Private Sub TerminarActualizacion(lo As LoadOperation(Of ValidacionCustodiaLiq))

        IsBusy = False
        Dim strCadena As String = String.Empty
        If Not lo.HasError Then
            ResultadoActualizacion = dcProxy.ValidacionCustodiaLiqs
            Dim Resultado = (From res In ResultadoActualizacion Select res.intResultado).FirstOrDefault

            Dim objTipoMensaje As A2Utilidades.wppMensajes.TiposMensaje

            With LiquidacionArreglo
                strCadena = "La liquidación " & .NroLiq & "-" & .NroParcial & " de "
                strCadena = strCadena & TipoSeleccionado.Descripcion & " de " & ClaseSeleccionada.Descripcion & " con fecha "
                strCadena = strCadena & Format(.FechaLiquidacion.Date, "MMMM dd yyyy") & " "

                If ResultadoActualizacion.Count > 0 Or Not ResultadoActualizacion Is Nothing Then

                    Select Case Resultado
                        Case 0
                            strCadena = strCadena & " no fue actualizada con Ubicación Título " & UbicacionSeleccionado.Descripcion
                            objTipoMensaje = A2Utilidades.wppMensajes.TiposMensaje.Advertencia
                        Case 1
                            strCadena = strCadena & " ha sido actualizada con Ubicación Título " & UbicacionSeleccionado.Descripcion
                            objTipoMensaje = A2Utilidades.wppMensajes.TiposMensaje.Exito
                    End Select

                Else
                    objTipoMensaje = A2Utilidades.wppMensajes.TiposMensaje.Advertencia
                    strCadena = strCadena & " no fue actualizada con Ubicación Título " & UbicacionSeleccionado.Descripcion
                End If
               

            End With

            A2Utilidades.Mensajes.mostrarMensaje(strCadena, Program.TituloSistema, objTipoMensaje)

            'If Resultado > 0 Then
            '    _Validaciones = False
            '    A2Utilidades.Mensajes.mostrarMensaje("Esta liquidacion ya fue " & ResultadoValidacion(0).strTabla & " no se puede modificar la cuenta del deposito", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
            'End If

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización de la ubicación del título", _
                                             Me.ToString(), "TerminarActualizacion_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If

    End Sub

End Class

