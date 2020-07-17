﻿Imports Telerik.Windows.Controls
'A2UtilsViewModel
'Archivo: ProductsViewModel.vb
'Creado el : 11/01/2010 16:49:28
'Propiedad de Alcuadrado S.A. 2010
'
' Modificado por Cristian Ciceri para eliminar carga de combos del App y conversión en el new de esta clase cada que se carga 
' para hacer carga de las lista de combos solamente si no está inicializada.

Imports System.ComponentModel
Imports System.Linq
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Threading
Imports GalaSoft.MvvmLight.Messaging
Imports GalaSoft.MvvmLight.Threading
Imports A2.OyD.Infraestructura

Public Class A2UtilsViewModel
    Inherits A2ControlMenu.A2ViewModel

    Private mdcProxy As UtilidadesDomainContext

    'se modifica el tipo de diccionario de List a ObservableCollection 
    'para que las modificaciones se reflejen en los controles de lista
    Public Property DiccionarioCombosA2() As New Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))

    Public Sub New()
#If DEBUG Then
        If DesignerProperties.GetIsInDesignMode(New DependencyObject()) Then Return
#End If
        Dim logMensaje As Boolean = IIf(Program.MostrarMensajeLog.Trim.Equals("1"), True, False)
        'se inicia la suscripcion al control de refrescar combos en a2controlmenu
        InicializaSuscripcionControl()
        inicializar()
    End Sub

    'se adiciona el parametro logRecargaRecursos para determinar si debe recargar el diccionario
    Private Sub inicializar(Optional ByVal logRecargaRecursos As Boolean = False)
        Dim logInicializarCombos As Boolean = False
        Dim strListaCombosReq As String = ""

        Try
            If Not logRecargaRecursos Then
                If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                    If strListaCombosReq.Trim().Equals(String.Empty) Then
                        logInicializarCombos = False
                    Else
                        '//////////////// Pendiente carga parcial //////////////////////
                        logInicializarCombos = False ' Se define el valor según la lista de combos. PENDIENTE
                    End If
                Else
                    logInicializarCombos = True
                End If
            Else
                logInicializarCombos = logRecargaRecursos
            End If

            If logInicializarCombos Then

                If Not logRecargaRecursos Then
                    'carga valores de la cache
                    CargaCacheDiccionario(DiccionarioCombosA2, Program.NOMBRE_DICCIONARIO_COMBOS, Program.NOMBRE_LISTA_COMBOS)
                    MyBase.CambioItem(Program.NOMBRE_DICCIONARIO_COMBOS)
                End If

                If Not DiccionarioCombosA2.Any() Or logRecargaRecursos Then
                    '// Establecer la conexión para cargar combos
                    If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                        mdcProxy = New UtilidadesDomainContext()
                    Else
                        mdcProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                    End If

                    '// Ejecutar la carga de los combos
                    mdcProxy.Load(mdcProxy.cargarCombosQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombos, logRecargaRecursos)
                End If

            Else
                If Not logRecargaRecursos Then
                    'se convierte el recurso en el tipo adecuado
                    CargaCacheDiccionario(DiccionarioCombosA2, Program.NOMBRE_DICCIONARIO_COMBOS, Program.NOMBRE_LISTA_COMBOS)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de las listas de selección", _
                                 Me.ToString(), "New", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub


    Private Sub TerminoCargarCombos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        'se definen de tipo observable los diccionarios y los recursos List
        Dim objListaCombos As ObservableCollection(Of OYDUtilidades.ItemCombo) = Nothing
        Dim objListaNodosCategoria As ObservableCollection(Of OYDUtilidades.ItemCombo) = Nothing
        Dim dicListaCombos As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo)) = Nothing
        Dim strNombreCategoria As String = ""
        Try
            If Not lo.HasError Then
                'Convierte los datos recibidos en un diccionario donde el nombre de la categoría es la clave
                objListaCombos = New ObservableCollection(Of OYDUtilidades.ItemCombo)(mdcProxy.ItemCombos)
                If objListaCombos.Count > 0 Then
                    Dim listaCategorias = From lc In objListaCombos Select lc.Categoria Distinct 'Lista de categorias incluidas en la consulta retornada

                    ' Guardar los datos recibidos en un diccionario
                    dicListaCombos = New Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))

                    For Each NombreCategoria As String In listaCategorias
                        strNombreCategoria = NombreCategoria
                        objListaNodosCategoria = New ObservableCollection(Of OYDUtilidades.ItemCombo)((From ln In objListaCombos Where ln.Categoria = strNombreCategoria))
                        dicListaCombos.Add(strNombreCategoria, objListaNodosCategoria)
                    Next

                    '' Guardar los datos almacenados en el nuevo diccionario en un recurso global
                    If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                        Application.Current.Resources.Remove(Program.NOMBRE_LISTA_COMBOS)
                    End If
                    Application.Current.Resources.Add(Program.NOMBRE_LISTA_COMBOS,
                                                      dicListaCombos.ToDictionary(Function(k) k.Key, Function(k) k.Value.ToList()))

                    'verfico si es una recarga de combos
                    If Convert.ToBoolean(lo.UserState) Then
                        RefrescaDiccionarioCombos(DiccionarioCombosA2, dicListaCombos)
                        'avisa a a2controlmenu que termino de refrescar
                        TerminaSuscripcionControl()
                    Else
                        DiccionarioCombosA2 = dicListaCombos
                    End If
                    MyBase.CambioItem(Program.NOMBRE_DICCIONARIO_COMBOS)
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los combos", _
                                     Me.ToString(), "New", Program.TituloSistema, Program.Maquina, New Exception("No se pudieron cargar los items de los combos"))
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos", _
                     Me.ToString(), "TerminoCargarCombos", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos", _
                     Me.ToString(), "TerminoCargarCombos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        'se elimina la suscripcion al control a2controlmenu
        'Messenger.Default.Unregister(Of String)(Me, Me.GetType().Assembly.FullName, AddressOf EjecutaActualizacionCombos)
        MyBase.Finalize()
    End Sub

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
    Public Sub EjecutaActualizacionCombos(ByVal strNombreVista As String)
        Try
            NombreView = strNombreVista
            'recargo los genericos
            inicializar(True)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recarga los combos",
                     Me.ToString(), "EjecutaActualizacionCombos", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    'refresca los diccionarios de combos adicionando los nuevos y actualizando la descripcion
    Private Sub RefrescaDiccionarioCombos(ByRef dicCombos As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo)), ByVal dicNuevo As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo)))
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
    Implements IEqualityComparer(Of OYDUtilidades.ItemCombo)

    Public Function Equals1(ByVal x As OYDUtilidades.ItemCombo, ByVal y As OYDUtilidades.ItemCombo) As Boolean Implements IEqualityComparer(Of OYDUtilidades.ItemCombo).Equals
        'es igual
        If x Is y Then Return True
        'es null
        If x Is Nothing OrElse y Is Nothing Then Return False
        'propiedades iguales
        Return (x.Categoria = y.Categoria) AndAlso (x.ID = y.ID)
    End Function

    Public Function GetHashCode1(ByVal Item As OYDUtilidades.ItemCombo) As Integer Implements IEqualityComparer(Of OYDUtilidades.ItemCombo).GetHashCode
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
