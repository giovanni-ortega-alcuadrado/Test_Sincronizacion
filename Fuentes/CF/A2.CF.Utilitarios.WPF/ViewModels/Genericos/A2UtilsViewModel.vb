Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu
Imports System.Threading
Imports System.Threading.Tasks
Imports RIAWeb = A2.OyD.OYDServer.RIA.Web
Imports GalaSoft.MvvmLight.Messaging
Imports GalaSoft.MvvmLight.Threading
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.Infraestructura

Public Class A2UtilsViewModel
    Inherits A2ControlMenu.A2ViewModel

    Private mdcProxy As RIAWeb.UtilidadesDomainContext
    Private mdcProxy1 As RIAWeb.UtilidadesDomainContext

    'se modifica el tipo de diccionario de List a ObservableCollection 
    'para que las modificaciones se reflejen en los controles de lista
    Public Property DiccionarioCombos() As New Dictionary(Of String, ObservableCollection(Of RIAWeb.OYDUtilidades.ItemCombo)) ' Propiedad que expone la colección de combos genéricos
    Public Property DiccionarioCombosEspecificos() As New Dictionary(Of String, ObservableCollection(Of RIAWeb.OYDUtilidades.ItemCombo)) ' Propiedad que expone la colección de combos específicos

    'JDCP20140116 - Se crea variable para obtener el nombre del compo especifico y actualizarlo en la cache.
    Private pstrNombreClaseCombos As String = String.Empty

    Private mstrDicCombosEspecificos As String = String.Empty ' Guarda el nombre de la colección de combos específicos que se carga/consulta
    Private mlogMostrarMensaje As Boolean = False ' Indica si se ha configurado que se muestre un mensaje de log para ver pasos de ejecución

#Region "Inicializar"

    Public Sub New()
        Me.New(String.Empty, String.Empty)
    End Sub

    Public Sub New(ByVal pstrClaseCombos As String, ByVal pstrNomControl As String)
#If DEBUG Then
        If DesignerProperties.GetIsInDesignMode(New DependencyObject()) Then Return
#End If
        'se inicia la suscripcion al control de refrescar combos en a2controlmenu
        'JDCP20140116 - Se crea variable para obtener el nombre del compo especifico y actualizarlo en la cache.
        pstrNombreClaseCombos = String.Format("{0}_{1}", pstrClaseCombos, pstrNomControl)
        InicializaSuscripcionControl()
        mlogMostrarMensaje = Program.mostrarMensaje()
    End Sub

#End Region

#Region "Métodos privados"

    'se adiciona el parametro logRecargaRecursos para determinar si debe recargar el diccionario
    Public Async Function inicializarCombos(ByVal pstrClaseCombos As String, ByVal pstrNombreControl As String, Optional ByVal logRecargaRecursos As Boolean = False) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            A2Utilidades.Mensajes.mostrarMensajeLog(Program.Usuario, "Inicializar combos", Me.ToString(), "inicializarCombos", Program.TituloSistema, mlogMostrarMensaje)

            If pstrClaseCombos.Equals(String.Empty) Then
                If Application.Current.Resources.Contains(Program.NombreListaCombos) AndAlso Not logRecargaRecursos Then
                    DiccionarioCombos = CType(Application.Current.Resources(Program.NombreListaCombos),
                        Dictionary(Of String, System.Collections.Generic.List(Of RIAWeb.OYDUtilidades.ItemCombo))).
                        ToDictionary(Function(k) k.Key, Function(k) New ObservableCollection(Of RIAWeb.OYDUtilidades.ItemCombo)(k.Value))
                    MyBase.CambioItem("DiccionarioCombos")
                Else

                    If Not logRecargaRecursos Then
                        'carga valores de la cache
                        CargaCacheDiccionario(DiccionarioCombos, Program.NombreListaCombos, Program.NombreListaCombos)
                        MyBase.CambioItem("DiccionarioCombos")
                    End If

                    If Not DiccionarioCombos.Any() Or logRecargaRecursos Then
                        Await consultarCombos(pstrClaseCombos:=String.Empty, pstrNombreControl:=String.Empty, pstrUsuario:=Program.Usuario, logRecargaRecursos:=logRecargaRecursos)
                    End If

                End If
            Else
                If Application.Current.Resources.Contains(pstrNombreControl) AndAlso Not logRecargaRecursos Then
                    DiccionarioCombosEspecificos = CType(Application.Current.Resources(pstrNombreControl),
                        Dictionary(Of String, System.Collections.Generic.List(Of RIAWeb.OYDUtilidades.ItemCombo))).
                        ToDictionary(Function(k) k.Key, Function(k) New ObservableCollection(Of RIAWeb.OYDUtilidades.ItemCombo)(k.Value))
                    MyBase.CambioItem("DiccionarioCombosEspecificos")
                Else

                    If Not logRecargaRecursos Then
                        'carga valores de la cache
                        'JDCP20140116 - Se modifica la obtención del recurso creado en la cache por el nombre creado en el constructor de la clase.
                        CargaCacheDiccionario(DiccionarioCombosEspecificos, pstrNombreClaseCombos, pstrNombreControl)
                        MyBase.CambioItem("DiccionarioCombosEspecificos")
                    End If

                    If Not DiccionarioCombosEspecificos.Any() Or logRecargaRecursos Then
                        Await consultarCombos(pstrClaseCombos:=pstrClaseCombos, pstrNombreControl:=pstrNombreControl, pstrUsuario:=Program.Usuario, logRecargaRecursos:=logRecargaRecursos)
                    End If

                End If
            End If

            logResultado = True

            A2Utilidades.Mensajes.mostrarMensajeLog(Program.Usuario, "Terminó inicialización de combos", Me.ToString(), "inicializarCombos", Program.TituloSistema, mlogMostrarMensaje)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización de las listas de selección", Me.ToString(), "inicializarCombos", Program.TituloSistema, Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Métodos acceso datos sincronicos"

    'se adiciona el parametro logRecargaRecursos para determinar si debe recargar el diccionario
    Public Async Function consultarCombos(ByVal pstrClaseCombos As String, ByVal pstrNombreControl As String, ByVal pstrUsuario As String, Optional ByVal logRecargaRecursos As Boolean = False) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim strNombreCategoria As String
        Dim dicListaCombos As Dictionary(Of String, ObservableCollection(Of RIAWeb.OYDUtilidades.ItemCombo))
        Dim objNodosCategoria As ObservableCollection(Of RIAWeb.OYDUtilidades.ItemCombo) = Nothing

        Try

            'adiciono la clase combos a un recurso global para luego ser usado por recargar combos especificos
            If Application.Current.Resources.Contains(Program.NOMBRE_DICCIONARIO_ACTUALIZARCOMBOSESPECIFICOS) Then
                If CType(Application.Current.Resources(Program.NOMBRE_DICCIONARIO_ACTUALIZARCOMBOSESPECIFICOS), List(Of String)).
                    Where(Function(k) k = pstrClaseCombos).FirstOrDefault() Is Nothing Then
                    CType(Application.Current.Resources(Program.NOMBRE_DICCIONARIO_ACTUALIZARCOMBOSESPECIFICOS), List(Of String)).Add(pstrClaseCombos)
                End If
            Else
                Application.Current.Resources.Add(Program.NOMBRE_DICCIONARIO_ACTUALIZARCOMBOSESPECIFICOS, New List(Of String) From {pstrClaseCombos})
            End If

            Dim objCombos = Await CargarCombosAsync(pstrClaseCombos, pstrUsuario)

            If Not objCombos Is Nothing Then
                If objCombos.Count > 0 Then

                    Dim lstCombos = From lc In objCombos Select lc.Categoria Distinct 'Lista de categorias incluidas en la consulta retornada

                    ' Guardar los datos recibidos en un diccionario
                    dicListaCombos = New Dictionary(Of String, ObservableCollection(Of RIAWeb.OYDUtilidades.ItemCombo))

                    For Each NombreCategoria As String In lstCombos
                        strNombreCategoria = NombreCategoria
                        objNodosCategoria = New ObservableCollection(Of RIAWeb.OYDUtilidades.ItemCombo)((From ln In objCombos Where ln.Categoria = strNombreCategoria))

                        dicListaCombos.Add(strNombreCategoria, objNodosCategoria)
                    Next

                    If pstrClaseCombos.Equals(String.Empty) Then
                        ' Guardar los datos almacenados en el nuevo diccionario en un recurso global
                        If Application.Current.Resources.Contains(Program.NombreListaCombos) Then
                            Application.Current.Resources.Remove(Program.NombreListaCombos)
                        End If
                        Application.Current.Resources.Add(Program.NombreListaCombos, dicListaCombos.ToDictionary(Function(k) k.Key, Function(k) k.Value.ToList()))

                        If logRecargaRecursos Then
                            RefrescaDiccionarioCombos(DiccionarioCombos, dicListaCombos)
                            'avisa a a2controlmenu que termino de refrescar
                            TerminaSuscripcionControl()
                        Else
                            DiccionarioCombos = dicListaCombos
                        End If
                        MyBase.CambioItem("DiccionarioCombos")
                    Else
                        ' Guardar los datos almacenados en el nuevo diccionario en un recurso global
                        If Application.Current.Resources.Contains(pstrNombreControl) Then
                            Application.Current.Resources.Remove(pstrNombreControl)
                        End If
                        Application.Current.Resources.Add(pstrNombreControl, dicListaCombos.ToDictionary(Function(i) i.Key, Function(i) i.Value.ToList()))

                        If logRecargaRecursos Then
                            RefrescaDiccionarioCombos(DiccionarioCombosEspecificos, dicListaCombos)
                            'avisa a a2controlmenu que termino de refrescar
                            TerminaSuscripcionControl()
                        Else
                            DiccionarioCombosEspecificos = dicListaCombos
                        End If
                        MyBase.CambioItem("DiccionarioCombosEspecificos")
                    End If
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de datos para las listas de selección.", Me.ToString(), "consultarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    Private Async Function CargarCombosAsync(ByVal pstrClaseCombos As String, ByVal pstrUsuario As String) As Task(Of ObservableCollection(Of RIAWeb.OYDUtilidades.ItemCombo))

        Dim objTask As LoadOperation(Of RIAWeb.OYDUtilidades.ItemCombo)
        Dim dcProxy As RIAWeb.UtilidadesDomainContext

        Try
            dcProxy = inicializarProxyUtilidadesOYD()

            If Not IsNothing(dcProxy.ItemCombos) Then
                dcProxy.ItemCombos.Clear()
            End If

            If pstrClaseCombos.Equals(String.Empty) Then
                objTask = Await dcProxy.Load(dcProxy.cargarCombosQuery(Program.Usuario, Program.HashConexion)).AsTask()
            Else
                objTask = Await dcProxy.Load(dcProxy.cargarCombosEspecificosQuery(pstrListasCombos:=pstrClaseCombos, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            End If

            Return New ObservableCollection(Of RIAWeb.OYDUtilidades.ItemCombo)(objTask.Entities)
        Catch ex As Exception
            objTask = Nothing
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de datos para las listas de selección", Me.ToString(), "CargarCombosAsync", Application.Current.ToString(), Program.Maquina, ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Finalizar componente"

    Protected Overrides Sub Finalize()
        'se elimina la suscripcion al control a2controlmenu
        'Messenger.Default.Unregister(Of String)(Me, Me.GetType().Assembly.FullName, AddressOf EjecutaActualizacionCombos)
        MyBase.Finalize()
    End Sub

#End Region

#Region "Refresca Combos"

    'se suscribe a los eventos del boton refrescar combos de a2controlmenu
    Private Sub InicializaSuscripcionControl()
        'Messenger.Default.Register(Of String)(Me, Me.GetType().Assembly.FullName, False, AddressOf EjecutaActualizacionCombos)
    End Sub

    Private Sub TerminaSuscripcionControl()
        'Envio el mensaje a a2controlmenu con un token (nombre de la vista)
        'Messenger.Default.Send(Of String)("", NombreView)
    End Sub

    'llama en hilos asincronos la consulta de los combos
    Public Async Sub EjecutaActualizacionCombos(ByVal strNombreVista As String)
        Try
            NombreView = strNombreVista
            'recargo los genericos
            Await consultarCombos(String.Empty, String.Empty, Program.Usuario, True)
            'recargo los especificos
            If Application.Current.Resources.Contains(Program.NOMBRE_DICCIONARIO_ACTUALIZARCOMBOSESPECIFICOS) Then
                For Each catego In CType(Application.Current.Resources(Program.NOMBRE_DICCIONARIO_ACTUALIZARCOMBOSESPECIFICOS), List(Of String))
                    Await consultarCombos(catego, String.Empty, Program.Usuario, True)
                Next
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recarga los combos",
                     Me.ToString(), "EjecutaActualizacionCombos", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    'refresca los diccionarios de combos adicionando los nuevos y actualizando la descripcion
    Private Sub RefrescaDiccionarioCombos(ByRef dicCombos As Dictionary(Of String, ObservableCollection(Of RIAWeb.OYDUtilidades.ItemCombo)), ByVal dicNuevo As Dictionary(Of String, ObservableCollection(Of RIAWeb.OYDUtilidades.ItemCombo)))
        Try
            For Each Catego In dicNuevo
                'verfico si existe la categoria
                If Not dicCombos.ContainsKey(Catego.Key) Then
                    dicCombos.Add(Catego.Key, Catego.Value)
                Else
                    'si existe actualizo el listado
                    For Each Item In Catego.Value
                        If Not dicCombos(Catego.Key).Contains(Item, New ItemComboComparer()) Then
                            dicCombos(Catego.Key).Add(Item)
                        Else
                            'actualizo la descripcion
                            dicCombos(Catego.Key).Where(
                                Function(i) i.Categoria = Catego.Key And i.ID = Item.ID) _
                                .FirstOrDefault().Descripcion = Item.Descripcion
                        End If
                    Next
                End If
            Next
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recarga los combos",
                     Me.ToString(), "RefrescaDiccionarioCombos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Cache"

    ''' <summary>
    ''' Carga valores desde la cache
    ''' </summary>
    ''' <param name="strItemCache">Key en la cache de los valores a buscar</param>
    ''' <param name="strItemRecurso">Key de como se registra el los recursos de la aplicacion (en algunos casos era diferente por eso este parametro)</param>
    ''' <remarks></remarks>
    Private Sub CargaCacheDiccionario(ByRef dicCombos As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo)), ByVal strItemCache As String, ByVal strItemRecurso As String)
        If Application.Current.Resources.Contains(strItemRecurso) Then
            dicCombos = DirectCast(Application.Current.Resources(strItemRecurso),
                Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).
                ToDictionary(Function(i) i.Key, Function(i) New ObservableCollection(Of OYDUtilidades.ItemCombo)(i.Value))
        End If
    End Sub

#End Region

End Class

'ItemComboComparer
'Creado el : 19/09/2014 
'Propiedad de Alcuadrado S.A. 2014
'
'Se crea para identificar cuando un item del combo es igual a otro
'el objetivo es determinar si la descripcion cambio
Public Class ItemComboComparer
    Implements IEqualityComparer(Of RIAWeb.OYDUtilidades.ItemCombo)

    Public Function Equals1(ByVal x As RIAWeb.OYDUtilidades.ItemCombo, ByVal y As RIAWeb.OYDUtilidades.ItemCombo) As Boolean Implements IEqualityComparer(Of RIAWeb.OYDUtilidades.ItemCombo).Equals
        'es igual
        If x Is y Then Return True
        'es null
        If x Is Nothing OrElse y Is Nothing Then Return False
        'propiedades iguales
        Return (x.Categoria = y.Categoria) AndAlso (x.ID = y.ID)
    End Function

    Public Function GetHashCode1(ByVal Item As RIAWeb.OYDUtilidades.ItemCombo) As Integer Implements IEqualityComparer(Of RIAWeb.OYDUtilidades.ItemCombo).GetHashCode
        'es null
        If Item Is Nothing Then Return 0
        'obtengo hash propiedad 1
        Dim hash1 = If(Item.Categoria Is Nothing, 0, Item.Categoria.GetHashCode())
        'obtengo hash propiedad 2
        Dim hash2 = If(Item.ID Is Nothing, 0, Item.ID.GetHashCode())
        'calculo hash de la entidad
        Return hash1 Xor hash2
    End Function

End Class
