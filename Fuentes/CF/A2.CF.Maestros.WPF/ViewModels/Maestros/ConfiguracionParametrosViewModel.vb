Imports Telerik.Windows.Controls

Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFMaestros
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations

''' <summary>
''' Métodos creados para la comunicación con el RIA (MaestrosDomainService.vb y OyD_Maestros.dbml)
''' Pantalla Configuración de Parámetros (Maestros)
''' </summary>
''' <remarks>Jorge Peña y Javier Pardo (Alcuadrado S.A.) - 10 de Julio 2015</remarks>
''' <history>
'''JEPM20150715 Se añaden propiedades Display y StringLength a algunas propiedades y se cambia la lógica al confirmar búsqueda
'''</history>
Public Class ConfiguracionParametrosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As Parametro
    Private mdcProxy As MaestrosCFDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As Parametro
    Dim strTipoFiltroBusqueda As String = String.Empty

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' ID caso de prueba: Id_1
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Septiembre 2/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Septiembre 2/2014 - Resultado Ok 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                consultarEncabezadoPorDefecto()

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty)

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' Lista de Parametro que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of Parametro)
    Public Property ListaEncabezado() As EntitySet(Of Parametro)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of Parametro))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Parametro para navegar sobre el grid con paginación
    ''' </summary>
    Public ReadOnly Property ListaEncabezadoPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEncabezado) Then
                If IsNothing(_ListaEncabezadoPaginada) Then
                    Dim view = New PagedCollectionView(_ListaEncabezado)
                    _ListaEncabezadoPaginada = view
                    Return view
                Else
                    Return (_ListaEncabezadoPaginada)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Elemento de la lista de Parametro que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionado As Parametro
    Public Property EncabezadoSeleccionado() As Parametro
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As Parametro)
            _EncabezadoSeleccionado = value

            If Not IsNothing(value) Then
                Select Case value.Tipo.ToLower
                    Case "numerico"
                        clasevalor.ValorNumeric = value.Valor
                        Visiblenumeric = Visibility.Visible
                        VisibleSino = Visibility.Collapsed
                        VisibleTexto = Visibility.Collapsed
                    Case "si/no"
                        clasevalor.ValorSino = value.Valor.ToUpper
                        VisibleSino = Visibility.Visible
                        Visiblenumeric = Visibility.Collapsed
                        VisibleTexto = Visibility.Collapsed
                    Case Else
                        clasevalor.ValorTexto = value.Valor
                        VisibleTexto = Visibility.Visible
                        VisibleSino = Visibility.Collapsed
                        Visiblenumeric = Visibility.Collapsed

                End Select
            End If

            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaConfiguracionParametros
    Public Property cb() As CamposBusquedaConfiguracionParametros
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaConfiguracionParametros)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    Private _HabilitarEncabezado As Boolean = False
    Public Property HabilitarEncabezado() As Boolean
        Get
            Return _HabilitarEncabezado
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEncabezado = value
            MyBase.CambioItem("HabilitarEncabezado")
        End Set
    End Property

    Private _VisibleSino As Visibility = Visibility.Collapsed
    Public Property VisibleSino As Visibility
        Get
            Return _VisibleSino
        End Get
        Set(value As Visibility)
            _VisibleSino = value
            MyBase.CambioItem("VisibleSino")
        End Set
    End Property
    Private _Visiblenumeric As Visibility = Visibility.Collapsed
    Public Property Visiblenumeric As Visibility
        Get
            Return _Visiblenumeric
        End Get
        Set(value As Visibility)
            _Visiblenumeric = value
            MyBase.CambioItem("Visiblenumeric")
        End Set
    End Property
    Private _VisibleTexto As Visibility = Visibility.Collapsed
    Public Property VisibleTexto As Visibility
        Get
            Return _VisibleTexto
        End Get
        Set(value As Visibility)
            _VisibleTexto = value
            MyBase.CambioItem("VisibleTexto")
        End Set
    End Property

    Private _combosino As New List(Of CamposComboParametros)
    Public Property combosino As List(Of CamposComboParametros)
        Get
            Return _combosino
        End Get
        Set(value As List(Of CamposComboParametros))
            _combosino = value
            MyBase.CambioItem("combosino")
        End Set
    End Property

    Private _clasevalor As New CamposValorParametros
    Public Property clasevalor As CamposValorParametros
        Get
            Return _clasevalor
        End Get
        Set(value As CamposValorParametros)
            _clasevalor = value
            MyBase.CambioItem("clasevalor")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP008
    ''' Creado por       : Javier Pardo (Alcuadrado S.A.)
    ''' Descripción      : No permitir añadir ni eliminar parámetro
    ''' Fecha            : Julio 10/2015
    ''' Pruebas CB       : Javier Pardo (Alcuadrado S.A.) - Julio 10/2015 - Resultado Ok  
    ''' </history>
    Public Overrides Sub NuevoRegistro()
        Try
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción Buscar de la barra de herramientas.
    ''' Ejecuta una búsqueda sobre los datos que contengan en los campos definidos internamente en el procedimiento de búsqueda (filtrado) el texto ingresado en el campo Filtro de la barra de herramientas
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: Id_3
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Septiembre 2/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Septiembre 2/2014 - Resultado Ok 
    ''' </history>
    Public Overrides Async Sub Filtrar()
        Try
            Await ConsultarEncabezado(True, FiltroVM)
            strTipoFiltroBusqueda = "FILTRAR"
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicializar la ejecución del filtro", Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub QuitarFiltro()
        MyBase.QuitarFiltro()
        strTipoFiltroBusqueda = String.Empty
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción "Búsqueda avanzada" de la barra de herramientas.
    ''' Presenta la forma de búsqueda para que el usuario seleccione los valores por los cuales desea buscar dentro de los campos definidos en la forma de búsqueda
    ''' </summary>
    Public Overrides Sub Buscar()
        PrepararNuevaBusqueda()
        MyBase.Buscar()
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Buscar de la forma de búsqueda
    ''' Ejecuta una búsqueda por los campos contenidos en la forma de búsqueda y cuyos valores correspondan con los seleccionados por el usuario
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: Id_4
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Septiembre 2/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Septiembre 2/2014 - Resultado Ok  
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: CP003, CP004
    ''' Creado por       : Javier Pardo (Alcuadrado S.A.)
    ''' Descripción      : Se añade el campo Descripción
    ''' Fecha            : Julio 10/2015
    ''' Pruebas CB       : Javier Pardo (Alcuadrado S.A.) - Julio 10/2015 - Resultado Ok  
    ''' </history>
    Public Overrides Async Sub ConfirmarBuscar()
        Try
            'JEPM20150715 se cambia la lógica de comparar con IsNothing y se cambia a String.Empty
            'If Not cb.IDparametro <> 0 Or Not IsNothing(cb.Parametro) Or
            '    Not IsNothing(cb.Valor) Or Not IsNothing(cb.Descripcion) Then 'Validar que ingresó algo en los campos de búsqueda
            If cb.IDparametro <> 0 Or cb.Parametro <> String.Empty Or cb.Valor <> String.Empty Or cb.Descripcion <> String.Empty Then
                Await ConsultarEncabezado(False, String.Empty, cb.IDparametro, cb.Parametro, cb.Valor, cb.Descripcion)
                strTipoFiltroBusqueda = "BUSQUEDA"
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: Id_5, Id_6, Id_7, Id_8
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Septiembre 2/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Septiembre 2/2014 - Resultado Ok 
    ''' </history>
    Public Overrides Sub ActualizarRegistro()

        Dim intNroOcurrencias As Integer
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            ErrorForma = String.Empty
            IsBusy = True

            '' ''Asignar el valor del campo 'Valor' a la propiedad de la entidad según el tipo 
            ' ''If Not IsNothing(EncabezadoSeleccionado) Then
            ' ''    Select Case EncabezadoSeleccionado.Tipo.ToLower
            ' ''        Case "numerico"
            ' ''            EncabezadoSeleccionado.Valor = clasevalor.ValorNumeric
            ' ''        Case "fecha"
            ' ''            If Not IsDate(EncabezadoSeleccionado.Valor) Then
            ' ''                A2Utilidades.Mensajes.mostrarMensaje("El valor no es de tipo fecha (yyyy-mm-dd)", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            ' ''                Exit Sub
            ' ''            End If
            ' ''            EncabezadoSeleccionado.Valor = clasevalor.ValorTexto
            ' ''        Case "si/no"
            ' ''            EncabezadoSeleccionado.Valor = clasevalor.ValorSino
            ' ''        Case Else
            ' ''            EncabezadoSeleccionado.Valor = clasevalor.ValorTexto
            ' ''    End Select
            ' ''End If


            If ValidarRegistro() Then
                ' Incializar los mensajes de validación
                '_EncabezadoSeleccionado.strMsgValidacion = String.Empty

                ' Validar si el registro ya existe en la lista
                intNroOcurrencias = (From e In ListaEncabezado Where e.IDparametro = _EncabezadoSeleccionado.IDparametro Select e).Count

                If intNroOcurrencias = 0 Then
                    strAccion = ValoresUserState.Ingresar.ToString()
                    ListaEncabezado.Add(_EncabezadoSeleccionado)
                End If

                'Asignar el encabezado anterior como el actual
                mobjEncabezadoAnterior = EncabezadoSeleccionado

                ' Enviar cambios al servidor
                Program.VerificarCambiosProxyServidor(mdcProxy)
                mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, strAccion)
            Else
                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar el proceso de actualización.", Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
    ''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
    ''' </summary>
    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_EncabezadoSeleccionado) Then

            If mdcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            IsBusy = True

            _EncabezadoSeleccionado.Usuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            HabilitarEncabezado = True

            IsBusy = False
        End If
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Cancelar de la barra de herramientas durante el ingreso o modificación del encabezado activo
    ''' </summary>
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = String.Empty

            If Not _EncabezadoSeleccionado Is Nothing Then
                mdcProxy.RejectChanges()

                Editando = False
                MyBase.CambioItem("Editando")

                EncabezadoSeleccionado = mobjEncabezadoAnterior
                HabilitarEncabezado = False

                'Select Case EncabezadoSeleccionado.Tipo.ToLower
                '    Case "numerico"
                '        clasevalor.ValorNumeric = EncabezadoSeleccionado.Valor
                '    Case "si/no"
                '        clasevalor.ValorSino = EncabezadoSeleccionado.Valor.ToUpper
                '    Case Else
                '        clasevalor.ValorTexto = EncabezadoSeleccionado.Valor
                'End Select
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: Id_9
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Septiembre 2/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Septiembre 2/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: CP008
    ''' Creado por       : Javier Pardo (Alcuadrado S.A.)
    ''' Descripción      : No permitir añadir ni eliminar parámetro
    ''' Fecha            : Julio 10/2015
    ''' Pruebas CB       : Javier Pardo (Alcuadrado S.A.) - Julio 10/2015 - Resultado Ok  
    ''' </history>
    Public Overrides Sub BorrarRegistro()
        Try
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaConfiguracionParametros
            objCB.IDparametro = Nothing
            objCB.Parametro = String.Empty
            objCB.Valor = String.Empty
            objCB.Descripcion = String.Empty
            cb = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar los datos de la forma de búsqueda", Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Retorna una copia del encabezado activo. 
    ''' Se hace un "clon" del encabezado activo para poder devolver los cambios y dejar el encabezado activo en su estado original si es necesario.
    ''' </summary>

    ''' <history>
    ''' ID caso de prueba: CP007
    ''' Creado por       : Javier Pardo (Alcuadrado S.A.)
    ''' Descripción      : Dejar los datos en su estado original al dar Cancelar en modo edición
    ''' Fecha            : Julio 10/2015
    ''' Pruebas CB       : Javier Pardo (Alcuadrado S.A.) - Julio 10/2015 - Resultado Ok  
    ''' </history>
    Private Function ObtenerRegistroAnterior() As Parametro
        Dim objEncabezado As Parametro = New Parametro

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of Parametro)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.IDparametro = _EncabezadoSeleccionado.IDparametro
                objEncabezado.Parametro = _EncabezadoSeleccionado.Parametro
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para guardar los datos del registro activo antes de su modificación.", Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objEncabezado)
    End Function

    ''' <history>
    ''' ID caso de prueba: CP005, CP006
    ''' Creado por       : Javier Pardo (Alcuadrado S.A.)
    ''' Descripción      : Permitir que el campo valor se pueda modificar 
    ''' Fecha            : Julio 10/2015
    ''' Pruebas CB       : Javier Pardo (Alcuadrado S.A.) - Julio 10/2015 - Resultado Ok  
    ''' </history>
    Private Function ValidarRegistro() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then

                'Valida el nombre del parametro
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.Parametro) Then
                    strMsg = String.Format("{0}{1} + El nombre del parámetro es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la descripción
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.Descripcion) Then
                    strMsg = String.Format("{0}{1} + La descripción del parámetro es un campo requerido.", strMsg, vbCrLf)
                End If

            Else
                strMsg = String.Format("{0}{1} Debe de seleccionar un registro", strMsg, vbCrLf)
            End If

            If strMsg.Equals(String.Empty) Then
                '------------------------------------------------------------------------------------------------------------------------
                '-- VALIDAR DATOS DEL DETALLE
                '-------------------------------------------------------------------------------------------------------------------------
                logResultado = True
            Else
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias antes de guardar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Resultados Asincrónicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Procedimiento que se ejecuta cuando finaliza la ejecución de una actualización a la base de datos
    ''' </summary>
    Public Overrides Async Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Dim strMsg As String = String.Empty

        Try
            IsBusy = False
            If So.HasError Then
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    If Not So.Error Is Nothing Then
                        strMsg = So.Error.Message
                    End If
                End If

                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.obtenerMensajeValidacion(strMsg, So.UserState.ToString, True), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

                ' Marcar los cambios como rechazados
                mdcProxy.RejectChanges()

                'If So.UserState.Equals(ValoresUserState.Borrar.ToString) Then
                If So.UserState.Equals("") Then
                    ' Cuando se elimina un registro, el método RejectChanges lo vuelve a adicionar a la lista pero al final no en la posición original
                    _ListaEncabezadoPaginada.MoveToLastPage()
                    _ListaEncabezadoPaginada.MoveCurrentToLast()
                    MyBase.CambioItem("ListaEncabezadoPaginada")
                End If

                So.MarkErrorAsHandled()
            Else
                HabilitarEncabezado = False

                MyBase.TerminoSubmitChanges(So)

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                If strTipoFiltroBusqueda = "FILTRAR" Then
                    Await ConsultarEncabezado(True, FiltroVM, 0, String.Empty, String.Empty, String.Empty, _EncabezadoSeleccionado.IDparametro)
                ElseIf strTipoFiltroBusqueda = "BUSQUEDA" And Not IsNothing(cb) Then
                    Await ConsultarEncabezado(False, String.Empty, cb.IDparametro, cb.Parametro, cb.Valor, cb.Descripcion, _EncabezadoSeleccionado.IDparametro)
                Else
                    Await ConsultarEncabezado(True, String.Empty, 0, String.Empty, String.Empty, String.Empty, _EncabezadoSeleccionado.IDparametro)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del resultado de la actualización de los datos", Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
            If Not So Is Nothing Then
                If So.HasError Then
                    So.MarkErrorAsHandled()
                End If
            End If
        End Try
    End Sub

    Private Sub TerminoTraerParametrosPorDefecto_Completed(ByVal lo As LoadOperation(Of Parametro))
        If Not lo.HasError Then
            mobjEncabezadoPorDefecto = lo.Entities.FirstOrDefault
        Else
            mobjEncabezadoPorDefecto = Nothing

            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Parametro por defecto", _
                                             Me.ToString(), "TerminoTraerParametroPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Sub consultarEncabezadoPorDefecto()
        Dim dcProxy As MaestrosCFDomainContext

        Try
            dcProxy = inicializarProxyMaestros()

            dcProxy.Load(dcProxy.TraerParametroPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerParametrosPorDefecto_Completed, "Default")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "consultarEncabezadoPorDefecto", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de Parametro
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal pIDparametro As Integer = 0,
                                               Optional ByVal pParametro As String = "",
                                               Optional ByVal pValor As String = "",
                                               Optional ByVal pDescripcion As String = "",
                                               Optional ByVal pIDParametroPosicionar As Integer = 0) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of Parametro)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyMaestros()
            End If

            Dim objComboSiNo As New List(Of CamposComboParametros)

            If objComboSiNo.Count = 0 Then
                objComboSiNo.Add(New CamposComboParametros With {.ID = "SI", .Descripcion = "SI"})
                objComboSiNo.Add(New CamposComboParametros With {.ID = "NO", .Descripcion = "NO"})
            End If

            combosino = objComboSiNo

            mdcProxy.Parametros.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxy.Load(mdcProxy.FiltrarConfiguracionParametrosSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxy.Load(mdcProxy.ConsultarConfiguracionParametrosSyncQuery(pIDparametro:=pIDparametro,
                                                                                                pParametro:=pParametro,
                                                                                                pValor:=pValor,
                                                                                                pDescripcion:=pDescripcion, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            End If

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarEncabezado", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaEncabezado = mdcProxy.Parametros

                    If pIDParametroPosicionar > 0 Then
                        If ListaEncabezado.Where(Function(i) i.IDparametro = pIDParametroPosicionar).Count > 0 Then
                            EncabezadoSeleccionado = ListaEncabezado.Where(Function(i) i.IDparametro = pIDParametroPosicionar).First
                        End If
                    End If

                    If objRet.Entities.Count > 0 Then
                        If Not plogFiltrar Then
                            MyBase.ConfirmarBuscar()
                        End If
                    Else
                        If Not plogFiltrar Or (plogFiltrar And Not pstrFiltro.Equals(String.Empty)) Then
                            ' Solamente se presenta el mensaje cuando se ejecuta la opción de filtrar con un filtro específico o la opción de buscar (consultar) 
                            A2Utilidades.Mensajes.mostrarMensaje("No se encontraron datos que concuerden con los criterios de búsqueda.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                End If
            Else
                ListaEncabezado.Clear()
            End If

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de Parametro ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

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
''' JEPM20150715 Se añaden propiedades Display y StringLength a algunas propiedades
''' </history>
Public Class CamposBusquedaConfiguracionParametros
    Implements INotifyPropertyChanged

    Private _IDparametro As Integer
    <Display(Name:="Código")> _
    Public Property IDparametro() As Integer
        Get
            Return _IDparametro
        End Get
        Set(ByVal value As Integer)
            _IDparametro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDparametro"))
        End Set
    End Property

    Private _Parametro As String
    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
    <Display(Name:="Parámetro")> _
    Public Property Parametro() As String
        Get
            Return _Parametro
        End Get
        Set(ByVal value As String)
            _Parametro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Parametro"))
        End Set
    End Property

    Private _Valor As String
    <StringLength(5000, ErrorMessage:="La longitud máxima es de 5000")> _
    <Display(Name:="Valor")> _
    Public Property Valor() As String
        Get
            Return _Valor
        End Get
        Set(ByVal value As String)
            _Valor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Valor"))
        End Set
    End Property

    Private _Descripcion As String
    <StringLength(5000, ErrorMessage:="La longitud máxima es de 5000")> _
    <Display(Name:="Descripción")> _
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class

Public Class CamposComboParametros

    <Display(Name:="ID")> _
    Public Property ID As String

    <Display(Name:="Descripcion")> _
    Public Property Descripcion As String

End Class

Public Class CamposValorParametros
    Implements INotifyPropertyChanged

    Private _ValorNumeric As String
    <Display(Name:="Valor")> _
    Public Property ValorNumeric As String
        Get
            Return _ValorNumeric
        End Get
        Set(value As String)
            _ValorNumeric = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ValorNumeric"))
        End Set
    End Property
    Private _ValorSino As String
    <Display(Name:="Valor")> _
    Public Property ValorSino As String
        Get
            Return _ValorSino
        End Get
        Set(value As String)
            _ValorSino = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ValorSino"))
        End Set
    End Property

    Private _ValorTexto As String
    <Display(Name:="Valor")> _
    Public Property ValorTexto As String
        Get
            Return _ValorTexto
        End Get
        Set(value As String)
            _ValorTexto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ValorTexto"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class