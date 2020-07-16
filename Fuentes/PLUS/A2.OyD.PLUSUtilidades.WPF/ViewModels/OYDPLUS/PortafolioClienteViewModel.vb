Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Imports System.Windows.Data
Imports System.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports A2.OyD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client

Public Class PortafolioClienteViewModel
    Implements INotifyPropertyChanged
    Private dcProxy As OyDPLUSutilidadesDomainContext
    Dim logCambiarValor As Boolean = True

#Region "Inicialización"

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OyDPLUSutilidadesDomainContext()
            Else
                dcProxy = New OyDPLUSutilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of OyDPLUSutilidadesDomainContext.IOYDPLUSUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 500)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "PortafolioClienteViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Metodos"

    Public Sub ConsultarPortafolioCliente(Optional ByVal plogLanzarConsultaSinEspecie As Boolean = False)
        Try
            If plogLanzarConsultaSinEspecie Then
                If Not String.IsNullOrEmpty(TipoNegocio) And Not String.IsNullOrEmpty(CodigoOYD) Then
                    If Not IsNothing(dcProxy.tblPortafolioClientes) Then
                        dcProxy.tblPortafolioClientes.Clear()
                    End If

                    LimpiarPortafolio()
                    MostrarConsultandoPortafolio = Visibility.Visible
                    dcProxy.Load(dcProxy.OYDPLUS_ConsultarPortafolioClienteQuery(TipoNegocio, CodigoOYD, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarPortafolioCliente, String.Empty)
                End If
            Else
                If ConsultarPortafolio Then
                    If Not IsNothing(TipoNegocio) And (Not IsNothing(CodigoOYD) Or Not IsNothing(Especie)) Then
                        If Not String.IsNullOrEmpty(TipoNegocio) And ((Not String.IsNullOrEmpty(CodigoOYD) And CodigoOYD <> "-9999999999") Or (Not String.IsNullOrEmpty(Especie) And Especie <> "(No Seleccionado)")) Then
                            If Not IsNothing(dcProxy.tblPortafolioClientes) Then
                                dcProxy.tblPortafolioClientes.Clear()
                            End If

                            LimpiarPortafolio()
                            MostrarConsultandoPortafolio = Visibility.Visible
                            dcProxy.Load(dcProxy.OYDPLUS_ConsultarPortafolioClienteQuery(TipoNegocio, CodigoOYD, Especie, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarPortafolioCliente, String.Empty)
                        Else
                            LimpiarPortafolio()
                            MostrarConsultandoPortafolio = Visibility.Collapsed
                        End If
                    Else
                        LimpiarPortafolio()
                        MostrarConsultandoPortafolio = Visibility.Collapsed
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el portafolio del cliente.",
                                 Me.ToString(), "ConsultarPortafolioCliente", Application.Current.ToString(), Program.Maquina, ex)
            MostrarConsultandoPortafolio = Visibility.Collapsed
        End Try
    End Sub

    Private Sub TotalizarXEspecie()
        Try
            If Not IsNothing(_ListaPortafolioCliente) Then
                ValorTotalPortafolio = 0
                Dim ValorPrecio As Double = 0
                Dim Cantidad As Double = 0
                Dim Contador As Integer = 0
                Dim objListaPortafolio As New List(Of OYDPLUSUtilidades.tblPortafolioCliente)
                Dim logIgual As Boolean = False
                Dim intCantidad As Integer = 0

                If TotalizarPortafolio And Not String.IsNullOrEmpty(Especie) And Especie <> "(No Seleccionado)" Then
                    For Each liPortafolio In _ListaPortafolioCliente
                        If liPortafolio.Especie = Especie Then
                            Cantidad = Cantidad + CDbl(IIf(IsNothing(liPortafolio.Cantidad), 0.0, liPortafolio.Cantidad))
                        End If
                    Next

                    For Each liPortafolio In _ListaPortafolioCliente
                        If liPortafolio.Especie = Especie Then
                            ValorPrecio = CDbl(ValorPrecio + ((liPortafolio.Cantidad * CDbl(IIf(IsNothing(liPortafolio.PrecioCompra), 0.0, liPortafolio.PrecioCompra))) / Cantidad))
                        End If
                    Next

                    For Each liPortafolio In _ListaPortafolioCliente
                        If liPortafolio.Especie = Especie Then
                            objListaPortafolio.Add(New OYDPLUSUtilidades.tblPortafolioCliente With {.ID = liPortafolio.ID,
                                                                                                    .Cantidad = Cantidad,
                                                                                                    .CentralDeposito = liPortafolio.CentralDeposito,
                                                                                                    .ClaseLiquidacion = liPortafolio.ClaseLiquidacion,
                                                                                                    .CodigoOYD = liPortafolio.CodigoOYD,
                                                                                                    .CuentaDeposito = liPortafolio.CuentaDeposito,
                                                                                                    .Custodia = Nothing,
                                                                                                    .CustodiaSecuencia = Nothing,
                                                                                                    .Especie = liPortafolio.Especie,
                                                                                                    .FechaEmision = liPortafolio.FechaEmision,
                                                                                                    .FechaLiquidacion = liPortafolio.FechaLiquidacion,
                                                                                                    .FechaVencimiento = liPortafolio.FechaVencimiento,
                                                                                                    .IDLiquidacion = liPortafolio.IDLiquidacion,
                                                                                                    .Indicador = liPortafolio.Indicador,
                                                                                                    .Modalidad = liPortafolio.Modalidad,
                                                                                                    .NombreCentralDeposito = liPortafolio.NombreCentralDeposito,
                                                                                                    .NombreTipoOperacion = liPortafolio.NombreTipoOperacion,
                                                                                                    .Parcial = liPortafolio.Parcial,
                                                                                                    .PrecioCompra = ValorPrecio,
                                                                                                    .PuntosIndicador = liPortafolio.PuntosIndicador,
                                                                                                    .DescripcionIndicador = liPortafolio.DescripcionIndicador,
                                                                                                    .Seleccionada = False})
                            Exit For
                        End If
                    Next

                    ListaPortafolioCliente = Nothing
                    ListaPortafolioCliente = objListaPortafolio
                Else
                    If TotalizarPortafolio Then
                        If _MostrarCamposRentaFija Then
                            Dim objListaPaginada = From con In _ListaPortafolioCliente
                                                   Group By _Especie = con.Especie,
                                                         _FechaEmision = con.FechaEmision,
                                                         _FechaVencimiento = con.FechaVencimiento,
                                                         _Modalidad = con.Modalidad,
                                                         _Tasa = con.TasaFacial,
                                                         _Indicador = con.Indicador,
                                                         _PuntosIndicador = con.PuntosIndicador
                                                    Into DatosEspecie = Group

                            If Not IsNothing(objListaPaginada) Then
                                If objListaPaginada.Count > 0 Then
                                    Contador = 1
                                    For Each liConsulta In objListaPaginada
                                        ValorPrecio = 0
                                        Cantidad = 0
                                        intCantidad = 0

                                        For Each liContador In _ListaPortafolioCliente
                                            logIgual = True

                                            If ValorDiferente(liConsulta._Especie, liContador.Especie) Then
                                                logIgual = False
                                            End If
                                            If ValorDiferente(liConsulta._FechaEmision, liContador.FechaEmision) Then
                                                logIgual = False
                                            End If
                                            If ValorDiferente(liConsulta._FechaVencimiento, liContador.FechaVencimiento) Then
                                                logIgual = False
                                            End If
                                            If ValorDiferente(liConsulta._Modalidad, liContador.Modalidad) Then
                                                logIgual = False
                                            End If
                                            If ValorDiferente(liConsulta._Tasa, liContador.TasaFacial) Then
                                                logIgual = False
                                            End If
                                            If ValorDiferente(liConsulta._Indicador, liContador.Indicador) Then
                                                logIgual = False
                                            End If
                                            If ValorDiferente(liConsulta._PuntosIndicador, liContador.PuntosIndicador) Then
                                                logIgual = False
                                            End If

                                            If logIgual Then
                                                intCantidad += 1
                                            End If
                                        Next

                                        If intCantidad = 1 Then
                                            For Each liPortafolio In _ListaPortafolioCliente
                                                logIgual = True

                                                If ValorDiferente(liConsulta._Especie, liPortafolio.Especie) Then
                                                    logIgual = False
                                                End If
                                                If ValorDiferente(liConsulta._FechaEmision, liPortafolio.FechaEmision) Then
                                                    logIgual = False
                                                End If
                                                If ValorDiferente(liConsulta._FechaVencimiento, liPortafolio.FechaVencimiento) Then
                                                    logIgual = False
                                                End If
                                                If ValorDiferente(liConsulta._Modalidad, liPortafolio.Modalidad) Then
                                                    logIgual = False
                                                End If
                                                If ValorDiferente(liConsulta._Tasa, liPortafolio.TasaFacial) Then
                                                    logIgual = False
                                                End If
                                                If ValorDiferente(liConsulta._Indicador, liPortafolio.Indicador) Then
                                                    logIgual = False
                                                End If
                                                If ValorDiferente(liConsulta._PuntosIndicador, liPortafolio.PuntosIndicador) Then
                                                    logIgual = False
                                                End If

                                                If logIgual Then
                                                    objListaPortafolio.Add(New OYDPLUSUtilidades.tblPortafolioCliente With {.ID = liPortafolio.ID,
                                                                                                                            .Cantidad = liPortafolio.Cantidad,
                                                                                                                            .CentralDeposito = liPortafolio.CentralDeposito,
                                                                                                                            .ClaseLiquidacion = liPortafolio.ClaseLiquidacion,
                                                                                                                            .CodigoOYD = liPortafolio.CodigoOYD,
                                                                                                                            .CuentaDeposito = liPortafolio.CuentaDeposito,
                                                                                                                            .Custodia = liPortafolio.Custodia,
                                                                                                                            .CustodiaSecuencia = liPortafolio.CustodiaSecuencia,
                                                                                                                            .Especie = liPortafolio.Especie,
                                                                                                                            .FechaEmision = liPortafolio.FechaEmision,
                                                                                                                            .FechaLiquidacion = liPortafolio.FechaLiquidacion,
                                                                                                                            .FechaVencimiento = liPortafolio.FechaVencimiento,
                                                                                                                            .IDLiquidacion = liPortafolio.IDLiquidacion,
                                                                                                                            .Indicador = liPortafolio.Indicador,
                                                                                                                            .Modalidad = liPortafolio.Modalidad,
                                                                                                                            .NombreCentralDeposito = liPortafolio.NombreCentralDeposito,
                                                                                                                            .NombreTipoOperacion = liPortafolio.NombreTipoOperacion,
                                                                                                                            .Parcial = liPortafolio.Parcial,
                                                                                                                            .PrecioCompra = liPortafolio.PrecioCompra,
                                                                                                                            .PuntosIndicador = liPortafolio.PuntosIndicador,
                                                                                                                            .DescripcionIndicador = liPortafolio.DescripcionIndicador,
                                                                                                                            .Seleccionada = False})
                                                    Exit For
                                                End If
                                            Next
                                        Else
                                            For Each liSuma In _ListaPortafolioCliente
                                                logIgual = True

                                                If ValorDiferente(liConsulta._Especie, liSuma.Especie) Then
                                                    logIgual = False
                                                End If
                                                If ValorDiferente(liConsulta._FechaEmision, liSuma.FechaEmision) Then
                                                    logIgual = False
                                                End If
                                                If ValorDiferente(liConsulta._FechaVencimiento, liSuma.FechaVencimiento) Then
                                                    logIgual = False
                                                End If
                                                If ValorDiferente(liConsulta._Modalidad, liSuma.Modalidad) Then
                                                    logIgual = False
                                                End If
                                                If ValorDiferente(liConsulta._Tasa, liSuma.TasaFacial) Then
                                                    logIgual = False
                                                End If
                                                If ValorDiferente(liConsulta._Indicador, liSuma.Indicador) Then
                                                    logIgual = False
                                                End If
                                                If ValorDiferente(liConsulta._PuntosIndicador, liSuma.PuntosIndicador) Then
                                                    logIgual = False
                                                End If

                                                If logIgual Then
                                                    ValorPrecio = CDbl(IIf(IsNothing(liSuma.PrecioCompra), 0.0, liSuma.PrecioCompra))
                                                    Cantidad = Cantidad + CDbl(IIf(IsNothing(liSuma.Cantidad), 0.0, liSuma.Cantidad))
                                                End If
                                            Next

                                            For Each liPortafolio In _ListaPortafolioCliente
                                                logIgual = True

                                                If liConsulta._Especie <> liPortafolio.Especie Then
                                                    logIgual = False
                                                End If
                                                If liConsulta._FechaEmision <> liPortafolio.FechaEmision Then
                                                    logIgual = False
                                                End If
                                                If liConsulta._FechaVencimiento <> liPortafolio.FechaVencimiento Then
                                                    logIgual = False
                                                End If
                                                If liConsulta._Modalidad <> liPortafolio.Modalidad Then
                                                    logIgual = False
                                                End If
                                                If liConsulta._Tasa <> liPortafolio.TasaFacial Then
                                                    logIgual = False
                                                End If
                                                If liConsulta._Indicador <> liPortafolio.Indicador Then
                                                    logIgual = False
                                                End If
                                                If liConsulta._PuntosIndicador <> liPortafolio.PuntosIndicador Then
                                                    logIgual = False
                                                End If

                                                If logIgual Then
                                                    objListaPortafolio.Add(New OYDPLUSUtilidades.tblPortafolioCliente With {.ID = liPortafolio.ID,
                                                                                                                            .Cantidad = Cantidad,
                                                                                                                            .CentralDeposito = liPortafolio.CentralDeposito,
                                                                                                                            .ClaseLiquidacion = liPortafolio.ClaseLiquidacion,
                                                                                                                            .CodigoOYD = liPortafolio.CodigoOYD,
                                                                                                                            .CuentaDeposito = liPortafolio.CuentaDeposito,
                                                                                                                            .Custodia = Nothing,
                                                                                                                            .CustodiaSecuencia = Nothing,
                                                                                                                            .Especie = liPortafolio.Especie,
                                                                                                                            .FechaEmision = liPortafolio.FechaEmision,
                                                                                                                            .FechaLiquidacion = liPortafolio.FechaLiquidacion,
                                                                                                                            .FechaVencimiento = liPortafolio.FechaVencimiento,
                                                                                                                            .IDLiquidacion = liPortafolio.IDLiquidacion,
                                                                                                                            .Indicador = liPortafolio.Indicador,
                                                                                                                            .Modalidad = liPortafolio.Modalidad,
                                                                                                                            .NombreCentralDeposito = liPortafolio.NombreCentralDeposito,
                                                                                                                            .NombreTipoOperacion = liPortafolio.NombreTipoOperacion,
                                                                                                                            .Parcial = liPortafolio.Parcial,
                                                                                                                            .PrecioCompra = ValorPrecio,
                                                                                                                            .PuntosIndicador = liPortafolio.PuntosIndicador,
                                                                                                                            .DescripcionIndicador = liPortafolio.DescripcionIndicador,
                                                                                                                            .Seleccionada = False})
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                        Contador += 1
                                    Next
                                End If

                                ListaPortafolioCliente = Nothing
                                ListaPortafolioCliente = objListaPortafolio
                            End If
                        Else
                            Dim objListaPaginada = From con In _ListaPortafolioCliente
                                                   Group By _Especie = con.Especie
                                                    Into DatosEspecie = Group

                            If Not IsNothing(objListaPaginada) Then
                                If objListaPaginada.Count > 0 Then
                                    Contador = 1
                                    For Each liConsulta In objListaPaginada
                                        ValorPrecio = 0
                                        Cantidad = 0
                                        intCantidad = 0

                                        For Each liContador In _ListaPortafolioCliente
                                            If liConsulta._Especie = liContador.Especie Then
                                                intCantidad += 1
                                            End If
                                        Next

                                        If intCantidad = 1 Then
                                            For Each liPortafolio In _ListaPortafolioCliente
                                                If liPortafolio.Especie = liConsulta._Especie Then
                                                    objListaPortafolio.Add(New OYDPLUSUtilidades.tblPortafolioCliente With {.ID = liPortafolio.ID,
                                                                                                                            .Cantidad = liPortafolio.Cantidad,
                                                                                                                            .CentralDeposito = liPortafolio.CentralDeposito,
                                                                                                                            .ClaseLiquidacion = liPortafolio.ClaseLiquidacion,
                                                                                                                            .CodigoOYD = liPortafolio.CodigoOYD,
                                                                                                                            .CuentaDeposito = liPortafolio.CuentaDeposito,
                                                                                                                            .Custodia = liPortafolio.Custodia,
                                                                                                                            .CustodiaSecuencia = liPortafolio.CustodiaSecuencia,
                                                                                                                            .Especie = liPortafolio.Especie,
                                                                                                                            .FechaEmision = liPortafolio.FechaEmision,
                                                                                                                            .FechaLiquidacion = liPortafolio.FechaLiquidacion,
                                                                                                                            .FechaVencimiento = liPortafolio.FechaVencimiento,
                                                                                                                            .IDLiquidacion = liPortafolio.IDLiquidacion,
                                                                                                                            .Indicador = liPortafolio.Indicador,
                                                                                                                            .Modalidad = liPortafolio.Modalidad,
                                                                                                                            .NombreCentralDeposito = liPortafolio.NombreCentralDeposito,
                                                                                                                            .NombreTipoOperacion = liPortafolio.NombreTipoOperacion,
                                                                                                                            .Parcial = liPortafolio.Parcial,
                                                                                                                            .PrecioCompra = liPortafolio.PrecioCompra,
                                                                                                                            .PuntosIndicador = liPortafolio.PuntosIndicador,
                                                                                                                            .DescripcionIndicador = liPortafolio.DescripcionIndicador,
                                                                                                                            .Seleccionada = False})
                                                    Exit For
                                                End If
                                            Next
                                        Else
                                            For Each liSuma In _ListaPortafolioCliente
                                                If liConsulta._Especie = liSuma.Especie Then
                                                    Cantidad = Cantidad + CDbl(IIf(IsNothing(liSuma.Cantidad), 0.0, liSuma.Cantidad))
                                                End If
                                            Next

                                            For Each liSuma In _ListaPortafolioCliente
                                                If liConsulta._Especie = liSuma.Especie Then
                                                    ValorPrecio = CDbl(ValorPrecio + ((liSuma.Cantidad * CDbl(IIf(IsNothing(liSuma.PrecioCompra), 0.0, liSuma.PrecioCompra))) / Cantidad))
                                                End If
                                            Next

                                            For Each liPortafolio In _ListaPortafolioCliente
                                                If liPortafolio.Especie = liConsulta._Especie Then
                                                    objListaPortafolio.Add(New OYDPLUSUtilidades.tblPortafolioCliente With {.ID = liPortafolio.ID,
                                                                                                                            .Cantidad = Cantidad,
                                                                                                                            .CentralDeposito = liPortafolio.CentralDeposito,
                                                                                                                            .ClaseLiquidacion = liPortafolio.ClaseLiquidacion,
                                                                                                                            .CodigoOYD = liPortafolio.CodigoOYD,
                                                                                                                            .CuentaDeposito = liPortafolio.CuentaDeposito,
                                                                                                                            .Custodia = Nothing,
                                                                                                                            .CustodiaSecuencia = Nothing,
                                                                                                                            .Especie = liPortafolio.Especie,
                                                                                                                            .FechaEmision = liPortafolio.FechaEmision,
                                                                                                                            .FechaLiquidacion = liPortafolio.FechaLiquidacion,
                                                                                                                            .FechaVencimiento = liPortafolio.FechaVencimiento,
                                                                                                                            .IDLiquidacion = liPortafolio.IDLiquidacion,
                                                                                                                            .Indicador = liPortafolio.Indicador,
                                                                                                                            .Modalidad = liPortafolio.Modalidad,
                                                                                                                            .NombreCentralDeposito = liPortafolio.NombreCentralDeposito,
                                                                                                                            .NombreTipoOperacion = liPortafolio.NombreTipoOperacion,
                                                                                                                            .Parcial = liPortafolio.Parcial,
                                                                                                                            .PrecioCompra = ValorPrecio,
                                                                                                                            .PuntosIndicador = liPortafolio.PuntosIndicador,
                                                                                                                            .DescripcionIndicador = liPortafolio.DescripcionIndicador,
                                                                                                                            .Seleccionada = False})
                                                    Exit For
                                                End If
                                            Next
                                        End If

                                        Contador += 1
                                    Next

                                    ListaPortafolioCliente = Nothing
                                    ListaPortafolioCliente = objListaPortafolio
                                End If
                            End If
                        End If
                    End If
                End If

                ValorTotalCantidad = 0
                ValorTotalPrecio = 0
                ValorTotalPortafolio = 0
                For Each li In _ListaPortafolioCliente
                    ValorTotalPrecio = ValorTotalPrecio + CDbl(IIf(IsNothing(li.PrecioCompra), 0.0, li.PrecioCompra))
                    ValorTotalCantidad = ValorTotalCantidad + CDbl(IIf(IsNothing(li.Cantidad), 0.0, li.Cantidad))
                Next

                ValorTotalPortafolio = ValorTotalPrecio * ValorTotalCantidad
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al totalizar el portafolio", _
                                 Me.ToString(), "TotalizarXEspecie", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function ValorDiferente(ByVal objValor1 As Object, ByVal objValor2 As Object) As Boolean
        Dim objRetorno As Boolean = False

        If IsNothing(objValor1) And IsNothing(objValor2) Then
            objRetorno = False
        ElseIf IsNothing(objValor1) And Not IsNothing(objValor2) Then
            objRetorno = True
        ElseIf Not IsNothing(objValor1) And IsNothing(objValor2) Then
            objRetorno = True
        Else
            If TypeOf objValor1 Is String Then
                If CStr(objValor1) <> CStr(objValor2) Then
                    objRetorno = True
                End If
            ElseIf TypeOf objValor1 Is Double Then
                If CDbl(objValor1) <> CDbl(objValor2) Then
                    objRetorno = True
                End If
            ElseIf TypeOf objValor1 Is Integer Then
                If CInt(objValor1) <> CInt(objValor2) Then
                    objRetorno = True
                End If
            ElseIf TypeOf objValor1 Is DateTime Then
                If CDate(objValor1) <> CDate(objValor2) Then
                    objRetorno = True
                End If
            End If
        End If

        Return objRetorno
    End Function

    Public Sub LimpiarPortafolio()
        Try
            logCambiarValor = False
            ListaPortafolioCliente = Nothing
            PortafolioClienteSelected = Nothing
            TotalizarPortafolio = False
            ValorTotalPortafolio = CDbl(0)
            logCambiarValor = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los controles.", _
                                 Me.ToString(), "LimpiarPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaPortafolioCliente As List(Of OYDPLUSUtilidades.tblPortafolioCliente)
    Public Property ListaPortafolioCliente() As List(Of OYDPLUSUtilidades.tblPortafolioCliente)
        Get
            Return _ListaPortafolioCliente
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblPortafolioCliente))
            _ListaPortafolioCliente = value
            If Not IsNothing(_ListaPortafolioCliente) Then
                PortafolioClienteSelected = _ListaPortafolioCliente.FirstOrDefault
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaPortafolioCliente"))
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaPortafolioClientePaged"))
        End Set
    End Property

    Public ReadOnly Property ListaPortafolioClientePaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaPortafolioCliente) Then
                Dim view = New PagedCollectionView(_ListaPortafolioCliente)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _PortafolioClienteSelected As OYDPLUSUtilidades.tblPortafolioCliente
    Public Property PortafolioClienteSelected() As OYDPLUSUtilidades.tblPortafolioCliente
        Get
            Return _PortafolioClienteSelected
        End Get
        Set(ByVal value As OYDPLUSUtilidades.tblPortafolioCliente)
            _PortafolioClienteSelected = value
            ViewPortafolio.PortafolioCliente = _PortafolioClienteSelected

            If Not IsNothing(_PortafolioClienteSelected) Then
                If _PortafolioClienteSelected.Seleccionada Then
                    ViewPortafolio.PortafolioSeleccionada = True
                Else
                    ViewPortafolio.PortafolioSeleccionada = False
                End If
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("PortafolioClienteSelected"))
        End Set
    End Property

    Private _MostrarConsultandoPortafolio As Visibility = Visibility.Collapsed
    Public Property MostrarConsultandoPortafolio() As Visibility
        Get
            Return _MostrarConsultandoPortafolio
        End Get
        Set(ByVal value As Visibility)
            _MostrarConsultandoPortafolio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarConsultandoPortafolio"))
        End Set
    End Property

    Private _ViewPortafolio As PortafolioClienteView
    Public Property ViewPortafolio() As PortafolioClienteView
        Get
            Return _ViewPortafolio
        End Get
        Set(ByVal value As PortafolioClienteView)
            _ViewPortafolio = value
        End Set
    End Property

    Private _CodigoOYD As String
    Public Property CodigoOYD() As String
        Get
            Return _CodigoOYD
        End Get
        Set(ByVal value As String)
            _CodigoOYD = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodigoOYD"))
        End Set
    End Property

    Private _Especie As String
    Public Property Especie() As String
        Get
            Return _Especie
        End Get
        Set(ByVal value As String)
            _Especie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Especie"))
        End Set
    End Property

    Private _TipoNegocio As String
    Public Property TipoNegocio() As String
        Get
            Return _TipoNegocio
        End Get
        Set(ByVal value As String)
            _TipoNegocio = value
            If Not IsNothing(TipoNegocio) Then
                If TipoNegocio.ToUpper = "C" Or TipoNegocio.ToUpper = "S" Then
                    MostrarCamposRentaFija = True
                Else
                    MostrarCamposRentaFija = False
                End If
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoNegocio"))
        End Set
    End Property

    Private _MostrarCamposRentaFija As Boolean
    Public Property MostrarCamposRentaFija() As Boolean
        Get
            Return _MostrarCamposRentaFija
        End Get
        Set(ByVal value As Boolean)
            _MostrarCamposRentaFija = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarCamposRentaFija"))
        End Set
    End Property


    Private _TotalizarPortafolio As Boolean = False
    Public Property TotalizarPortafolio() As Boolean
        Get
            Return _TotalizarPortafolio
        End Get
        Set(ByVal value As Boolean)
            _TotalizarPortafolio = value
            If Not IsNothing(TotalizarPortafolio) Then
                If logCambiarValor Then
                    If TotalizarPortafolio Then
                        TotalizarXEspecie()
                    Else
                        ConsultarPortafolioCliente(True)
                    End If
                End If
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TotalizarPortafolio"))
        End Set
    End Property

    Private _ValorTotalCantidad As Double
    Public Property ValorTotalCantidad() As Double
        Get
            Return _ValorTotalCantidad
        End Get
        Set(ByVal value As Double)
            _ValorTotalCantidad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ValorTotalCantidad"))
        End Set
    End Property

    Private _ValorTotalPrecio As Double
    Public Property ValorTotalPrecio() As Double
        Get
            Return _ValorTotalPrecio
        End Get
        Set(ByVal value As Double)
            _ValorTotalPrecio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ValorTotalPrecio"))
        End Set
    End Property

    Private _ValorTotalPortafolio As Double
    Public Property ValorTotalPortafolio() As Double
        Get
            Return _ValorTotalPortafolio
        End Get
        Set(ByVal value As Double)
            _ValorTotalPortafolio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ValorTotalPortafolio"))
        End Set
    End Property

    Private _ConsultarPortafolio As Boolean
    Public Property ConsultarPortafolio() As Boolean
        Get
            Return _ConsultarPortafolio
        End Get
        Set(ByVal value As Boolean)
            _ConsultarPortafolio = value
            ConsultarPortafolioCliente()
        End Set
    End Property


#End Region

#Region "Resultados Asincronicos"

    Private Sub TerminoConsultarPortafolioCliente(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblPortafolioCliente))
        Try
            If lo.HasError = False Then
                ListaPortafolioCliente = lo.Entities.ToList
                MostrarConsultandoPortafolio = Visibility.Collapsed
                TotalizarXEspecie()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener el portafolio del cliente.", Me.ToString(), "TerminoConsultarPortafolioCliente", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                MostrarConsultandoPortafolio = Visibility.Collapsed
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener el portafolio del cliente.", Me.ToString(), "TerminoConsultarPortafolioCliente", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            MostrarConsultandoPortafolio = Visibility.Collapsed
        End Try
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Este evento se dispara cuando alguna propiedad de la orden activa cambia
    ''' Desarrollado por Juan David Correa
    ''' Fecha 14 de Agosto del 2012
    ''' </summary>
    Private Sub _PortafolioClienteSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _PortafolioClienteSelected.PropertyChanged
        Try
            Select Case e.PropertyName.ToLower()
                Case "seleccionada"
                    If PortafolioClienteSelected.Seleccionada Then
                        For Each li In _ListaPortafolioCliente
                            If li.ID <> PortafolioClienteSelected.ID Then
                                li.Seleccionada = False
                            End If
                        Next
                        ViewPortafolio.PortafolioSeleccionada = True
                    Else
                        ViewPortafolio.PortafolioSeleccionada = False
                    End If
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_OrdenesSAESelected_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            MostrarConsultandoPortafolio = Visibility.Collapsed
        End Try

    End Sub

#End Region

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
