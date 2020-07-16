Imports A2Utilidades.Recursos.RecursosApp
Imports A2Utilidades.Aplicaciones
Imports A2Utilidades.CifrarSL
Imports System.Reflection
Imports A2Utilidades
Imports System.IO

Partial Public Class AcercaDe
    Inherits Window

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub AcercaDe_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        Dim intPos As Integer
        Dim intLong As Integer
        Dim intNroComponentes As Integer = 0
        Dim intComponenteActual As Integer = 0
        Dim strClave As String
        Dim strServicioDatos As String = String.Empty
        Dim strFullAssembly As String = String.Empty
        Dim strVersion As String = String.Empty
        Dim strBD As String
        Dim objApp As A2Utilidades.Aplicacion

        Try
            Dim intAlto As Double = 730
            Dim intAncho As Double = 1000

            If Application.Current.Resources.Contains("MainWindowsPrincipalAnchoTotal") Then
                Dim intAnchoTotal = CType(Application.Current.Resources("MainWindowsPrincipalAnchoTotal"), Double)
                If Not intAnchoTotal.Equals(Double.NaN) Then
                    intAncho = intAnchoTotal * 0.8
                End If
            End If

            If Application.Current.Resources.Contains("MainWindowsPrincipalAltoTotal") Then
                Dim intAltoTotal = CType(Application.Current.Resources("MainWindowsPrincipalAltoTotal"), Double)
                If Not intAltoTotal.Equals(Double.NaN) Then
                    intAlto = intAltoTotal - 200
                End If
            End If

            Me.dtgModulos.Height = intAlto - 300
            Me.scrollPrincipal.MaxHeight = intAlto
            Me.scrollPrincipal.Width = intAncho

            Me.txtAplicacion.Text = Program.Aplicacion

            If Application.Current.Resources.Contains(A2Consola_AppActiva.ToString) Then
                objApp = CType(Application.Current.Resources(A2Utilidades.Recursos.RecursosApp.A2Consola_Aplicaciones.ToString), A2Utilidades.Aplicaciones).obtenerAplicacion(Program.Aplicacion, Program.VersionAplicacion, Program.Division)

                If objApp.Parametros.ContainsKey(GSTR_CLAVE_NOMBRE_APP) Then
                    Me.txtAplicacion.Text = objApp.Parametros(GSTR_CLAVE_NOMBRE_APP).ToString
                End If

                Me.txtDAAplicacion.Text = Me.txtAplicacion.Text

                If objApp.Parametros.ContainsKey(GSTR_CLAVE_VERSION_APP) Then
                    Me.txtVersionAplicacion.Text = objApp.Parametros(GSTR_CLAVE_VERSION_APP).ToString
                    Me.txtDAVersion.Text = "Versión " & objApp.Parametros(GSTR_CLAVE_VERSION_APP).ToString
                    If objApp.Parametros.ContainsKey(GSTR_CLAVE_FECHA_APP) Then
                        Me.txtDAVersion.Text &= ", " & objApp.Parametros(GSTR_CLAVE_FECHA_APP).ToString
                    End If
                Else
                    Me.txtDAVersion.Text = "Versión " & Program.VersionAplicacion
                End If

                If objApp.Parametros.ContainsKey(GSTR_CLAVE_MSG_DERECHOSAUTOR) Then
                    Me.txtDATexto.Text = objApp.Parametros(GSTR_CLAVE_MSG_DERECHOSAUTOR).ToString
                End If

                If objApp.Parametros.ContainsKey(GSTR_NAMESPACE_APLICACION) Then
                    strClave = objApp.Parametros(GSTR_NAMESPACE_APLICACION).ToString.ToLower

                    Dim objListaAssemblies As New List(Of clsModulos_Assemblies)

                    If Not IsNothing(objApp.ListaDLLClienteValidacion) Then
                        For Each li In objApp.ListaDLLClienteValidacion
                            objListaAssemblies.Add(li)
                        Next
                    End If

                    If Not IsNothing(objApp.ListaDLLRIAValidacion) Then
                        For Each li In objApp.ListaDLLRIAValidacion
                            objListaAssemblies.Add(li)
                        Next
                    End If

                    Dim objListaAssembliesVisualizar As List(Of clsModulos_Assemblies) = Nothing

                    If objListaAssemblies.Where(Function(i) i.EsValida = False).Count > 0 Then
                        objListaAssembliesVisualizar = objListaAssemblies.Where(Function(i) i.EsValida = False).ToList
                        objListaAssembliesVisualizar.AddRange(objListaAssemblies.Where(Function(i) i.EsValida).ToList)
                    Else
                        objListaAssembliesVisualizar = objListaAssemblies
                    End If

                    If objListaAssembliesVisualizar.Count > 0 Then
                        dtgModulos.Visibility = Visibility.Visible
                        dtgModulos.ItemsSource = objListaAssembliesVisualizar
                    Else
                        dtgModulos.Visibility = Visibility.Collapsed
                    End If

                End If

                If objApp.Parametros.ContainsKey(GSTR_CLAVE_USUARIO_AUT_ACERCADE) Then ' Validar si el usuario está autorizado a ver esta información
                    If objApp.Parametros(GSTR_CLAVE_USUARIO_AUT_ACERCADE).ToString.ToUpper.Equals("SI") Or objApp.Parametros(GSTR_CLAVE_USUARIO_AUT_ACERCADE).ToString.ToUpper.Equals("1") Then ' Validar si el usuario está autorizado a ver esta información
                        Me.tabServicios.Visibility = Visibility.Visible
                        Me.tabParametros.Visibility = Visibility.Visible

                        Dim objListaServicios As New List(Of clsAcercaDe_ParametrosTecnicos)

                        If Application.Current.Resources.Contains(A2Consola_ServicioSeguridad.ToString()) Then
                            strClave = Application.Current.Resources(A2Consola_ServicioSeguridad.ToString()).ToString
                            objListaServicios.Add(New clsAcercaDe_ParametrosTecnicos With {.Nombre = "Servicio seguridad", .Valor = strClave.Substring(0, strClave.LastIndexOf("/"))})
                        End If

                        strClave = GSTR_CLAVE_PARAM_APP_BASESERVICIOWPF
                        If objApp.Parametros.ContainsKey(strClave) Then
                            strClave = objApp.Parametros(strClave).ToString
                            objListaServicios.Add(New clsAcercaDe_ParametrosTecnicos With {.Nombre = "Servicios de datos", .Valor = strClave})
                        End If

                        strClave = GSTR_CLAVE_SW_DOCUMENTOS
                        If objApp.Parametros.ContainsKey(strClave) Then
                            strClave = objApp.Parametros(strClave).ToString
                            objListaServicios.Add(New clsAcercaDe_ParametrosTecnicos With {.Nombre = "Servicios documentos", .Valor = strClave.Substring(0, strClave.LastIndexOf("/"))})
                        End If

                        strClave = GSTR_CLAVE_SW_REPORTING_SERVICES
                        If objApp.Parametros.ContainsKey(strClave) Then
                            strClave = objApp.Parametros(strClave).ToString
                            objListaServicios.Add(New clsAcercaDe_ParametrosTecnicos With {.Nombre = "Reporting Services", .Valor = strClave.Substring(0, strClave.LastIndexOf("/"))})
                        End If

                        strClave = GSTR_CLAVE_SW_VISOR_RPTS
                        If objApp.Parametros.ContainsKey(strClave) Then
                            strClave = objApp.Parametros(strClave).ToString
                            objListaServicios.Add(New clsAcercaDe_ParametrosTecnicos With {.Nombre = "Visor de reportes", .Valor = strClave.Substring(0, strClave.LastIndexOf("/"))})
                        End If

                        strClave = GSTR_CLAVE_SW_VISOR_SETEADOR
                        If objApp.Parametros.ContainsKey(strClave) Then
                            strClave = objApp.Parametros(strClave).ToString
                            objListaServicios.Add(New clsAcercaDe_ParametrosTecnicos With {.Nombre = "Visor del seteador", .Valor = strClave.Substring(0, strClave.LastIndexOf("\"))})
                        End If

                        dtgServicios.ItemsSource = objListaServicios

                        If objApp.Parametros.ContainsKey(GSTR_CLAVE_PARAM_CONEXION_DATOS) Then
                            strBD = A2Utilidades.CifrarSL.descifrar(objApp.Parametros(GSTR_CLAVE_PARAM_CONEXION_DATOS).ToString)

                            Dim objListaParametros As New List(Of clsAcercaDe_ParametrosTecnicos)

                            '--------------------------------------------------------------
                            '-- Servidor de base de datos del aplicativo actual
                            intPos = strBD.ToLower.IndexOf("data source")
                            intPos = strBD.IndexOf("=", intPos) + 1
                            intLong = strBD.IndexOf(";", intPos) - intPos
                            strClave = strBD.Substring(intPos, intLong)

                            objListaParametros.Add(New clsAcercaDe_ParametrosTecnicos With {.Nombre = "Servidor", .Valor = strClave})

                            '--------------------------------------------------------------
                            '-- Base de datos del aplicativo actual
                            intPos = strBD.ToLower.IndexOf("initial catalog")
                            intPos = strBD.IndexOf("=", intPos) + 1
                            intLong = strBD.IndexOf(";", intPos) - intPos
                            strClave = strBD.Substring(intPos, intLong)

                            objListaParametros.Add(New clsAcercaDe_ParametrosTecnicos With {.Nombre = "Base de datos", .Valor = strClave})

                            '--------------------------------------------------------------
                            '-- Base de datos de utilidades (la registrada en la base de datos del aplicativo)
                            strClave = GSTR_CLAVE_BASE_DATOS_UTILS
                            If objApp.Parametros.ContainsKey(strClave) Then
                                strClave = objApp.Parametros(strClave).ToString
                                objListaParametros.Add(New clsAcercaDe_ParametrosTecnicos With {.Nombre = "Utilidades", .Valor = strClave})
                            End If

                            '--------------------------------------------------------------
                            '-- Sistema framework
                            strClave = System.Reflection.Assembly.GetCallingAssembly().ImageRuntimeVersion
                            objListaParametros.Add(New clsAcercaDe_ParametrosTecnicos With {.Nombre = "Framework", .Valor = strClave})

                            ' Carpeta para uploads
                            objListaParametros.Add(New clsAcercaDe_ParametrosTecnicos With {.Nombre = "Carpeta uploads", .Valor = A2Utilidades.CifrarSL.descifrar(objApp.Parametros(GSTR_CLAVE_PARAM_CARPETA_UPLOAD).ToString)})

                            dtgOtrosParametros.ItemsSource = objListaParametros
                        End If
                    End If

                    If Not IsNothing(objApp.ListaProductosAplicacion) Then
                        Dim objListaProductosAplicacion As New List(Of clsAcercaDe_ParametrosTecnicos)

                        For Each li In objApp.ListaProductosAplicacion
                            objListaProductosAplicacion.Add(New clsAcercaDe_ParametrosTecnicos With {.Nombre = li.Key, .Valor = li.Value})
                        Next

                        dtgProductos.ItemsSource = objListaProductosAplicacion
                    End If
                End If
            Else
                Mensajes.mostrarMensaje(String.Format("No se pudo obtener la información necesaria de la versión de la aplicación {1}, versión {2} desde el servicio RIA.", Program.Aplicacion, Program.VersionAplicacion), Program.ConsolaTitulo, wppMensajes.TiposMensaje.Errores)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdAceptar_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub
End Class

Public Class clsAcercaDe_ParametrosTecnicos
    Public Property Nombre As String
    Public Property Valor As String
End Class