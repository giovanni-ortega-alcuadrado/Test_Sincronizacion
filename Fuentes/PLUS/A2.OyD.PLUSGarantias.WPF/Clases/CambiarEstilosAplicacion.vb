Imports Telerik.Windows.Controls
Imports System.Reflection
Imports System.Reflection.Emit

Public Class CambiarEstilosAplicacion

    ''' <summary>
    ''' Clase para realizar el cambio de Estilos por pantalla.
    ''' Se debe de invocar una vez dentro del new de cada view antes de Iniciar los componentes.
    ''' Desarrollado por: Juan David Correa
    ''' Fecha 29 de Enero del 2012
    ''' </summary>
    ''' <param name="objVentana">View al cual se le quiere aplicar los estilos.</param>
    ''' <param name="objOpcion">Opción el cual dice que estilos seran aplicados para la vista.</param>
    ''' <remarks></remarks>
    Public Shared Sub ObtenerEstilos(ByVal objVentana As Object, ByVal objOpcion As OpcionEstilo)
        Try
            Dim objAssembly As Assembly = Assembly.GetExecutingAssembly()
            Dim objAssemblyName As New AssemblyName(objAssembly.FullName)
            Dim strNombreProyecto As String = objAssemblyName.Name()

            Select Case objOpcion

                Case OpcionEstilo.OYDPLUS

                    LimpiarRecursos(objVentana)

                    AdicionarRecurso(objVentana, "Assets/CoreStyles.xaml", strNombreProyecto)
                    AdicionarRecurso(objVentana, "Assets/SDKStyles.xaml", strNombreProyecto)
                    AdicionarRecurso(objVentana, "Assets/C1Styles.xaml", strNombreProyecto)
                    AdicionarRecurso(objVentana, "Assets/ToolkitStyles.xaml", strNombreProyecto)

                Case OpcionEstilo.OYDNET

                    LimpiarRecursos(objVentana)

                    AdicionarRecurso(objVentana, "Assets/Styles.xaml", strNombreProyecto)
                    AdicionarRecurso(objVentana, "Assets/TemaBlue.xaml", strNombreProyecto)
                    AdicionarRecurso(objVentana, "Assets/Formularios.xaml", strNombreProyecto)
                    AdicionarRecurso(objVentana, "Assets/A2DataForm.xaml", strNombreProyecto)
                    AdicionarRecurso(objVentana, "Assets/ControlesStyles.xaml", strNombreProyecto)

            End Select

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al adicionar los estilos de oydplus", "CambiarEstilosAplicacion", "ObtenerEstilosOYDPLUS", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Shared Sub AdicionarRecurso(ByVal objVentana As Object, ByVal strUrl As String, ByVal pstrNombreProyecto As String)
        Try
            Dim objRecursoEstilo As New System.Windows.ResourceDictionary
            Dim objUri As Uri = New Uri(String.Format("/{0};component/{1}", pstrNombreProyecto, strUrl), UriKind.RelativeOrAbsolute)

            objRecursoEstilo.Source = objUri

            If Not IsNothing(objVentana) Then


                If TypeOf objVentana Is UserControl Then
                    CType(objVentana, UserControl).Resources.MergedDictionaries.Add(objRecursoEstilo)
                ElseIf TypeOf objVentana Is Window Then
                    CType(objVentana, Window).Resources.MergedDictionaries.Add(objRecursoEstilo)
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al adicionar el recurso al objeto", "CambiarEstilosAplicacion", "AdicionarRecurso", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Shared Sub LimpiarRecursos(ByVal objVentana As Object)
        Try
            If Not IsNothing(objVentana) Then


                If TypeOf objVentana Is UserControl Then
                    CType(objVentana, UserControl).Resources.MergedDictionaries.Clear()
                ElseIf TypeOf objVentana Is Window Then
                    CType(objVentana, Window).Resources.MergedDictionaries.Clear()
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al adicionar el recurso al objeto", "CambiarEstilosAplicacion", "AdicionarRecurso", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Enum OpcionEstilo
        OYDPLUS
        OYDNET
    End Enum

End Class


