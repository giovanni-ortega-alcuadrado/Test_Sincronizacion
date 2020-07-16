Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations

Public Class ConfiguracionContableConceptoViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As CFCodificacionContable.ConfiguracionContableConcepto
    Private mdcProxy As CodificacionContableDomainContext  ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As CFCodificacionContable.ConfiguracionContableConcepto
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
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Marzo 31/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Marzo 31/2014 - Resultado Ok 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                ConsultarEncabezadoPorDefectoSync()

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty)

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' Lista de ConfiguracionContableConcepto que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of CFCodificacionContable.ConfiguracionContableConcepto)
    Public Property ListaEncabezado() As EntitySet(Of CFCodificacionContable.ConfiguracionContableConcepto)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of CFCodificacionContable.ConfiguracionContableConcepto))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de ConfiguracionContableConcepto para navegar sobre el grid con paginación
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
    ''' Elemento de la lista de ConfiguracionContableConcepto que se encuentra seleccionado
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 3/2014
    ''' Id caso de prueba: Id_2
    ''' Pruebas CB       : Jorge Peña - Abril 3/2014 - Resultado Ok 
    ''' </history>
    Private WithEvents _EncabezadoSeleccionado As CFCodificacionContable.ConfiguracionContableConcepto
    Public Property EncabezadoSeleccionado() As CFCodificacionContable.ConfiguracionContableConcepto
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CFCodificacionContable.ConfiguracionContableConcepto)
            _EncabezadoSeleccionado = value
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaConfiguracionContableConcepto
    Public Property cb() As CamposBusquedaConfiguracionContableConcepto
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaConfiguracionContableConcepto)
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

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    '''<history>
    ''' ID caso de prueba:  Id_7
    ''' Descripción:        Se agrega el campo intTipoProducto. CC 827
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              24 de Abril/2014
    ''' Pruebas CB:         Jorge Peña - 24 de Abril/2014 - Resultado OK
    '''</history>
    '''<history>
    ''' ID caso de prueba:  Id_14
    ''' Descripción:        Se agregan los campos strCuentaContableDBPositiva, strCuentaContableDBNegativa, strCuentaContableCRPositiva, strCuentaContableCRNegativa.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              22 de Octubre/2014
    ''' Pruebas CB:         Jorge Peña - 22 de Octubre/2014 - Resultado OK
    '''</history>
    Public Overrides Sub NuevoRegistro()

        Dim objNvoEncabezado As New CFCodificacionContable.ConfiguracionContableConcepto

        Try
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of CFCodificacionContable.ConfiguracionContableConcepto)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.intID = -1
                objNvoEncabezado.strConcepto = String.Empty
                objNvoEncabezado.strCuentaContableDBPositiva = String.Empty
                objNvoEncabezado.strCuentaContableDBNegativa = String.Empty
                objNvoEncabezado.strCuentaContableCRPositiva = String.Empty
                objNvoEncabezado.strCuentaContableCRNegativa = String.Empty
                objNvoEncabezado.strTipoTitulos = String.Empty
                objNvoEncabezado.intTipoProducto = 0
                objNvoEncabezado.strTipoInversion = String.Empty
                objNvoEncabezado.strNormaContable = String.Empty
            End If

            objNvoEncabezado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            EncabezadoSeleccionado = objNvoEncabezado
            HabilitarEncabezado = True

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
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 3/2014
    ''' Id caso de prueba: Id_3
    ''' Pruebas CB       : Jorge Peña - Abril 3/2014 - Resultado Ok 
    ''' </history>
    Public Overrides Async Sub Filtrar()
        Try
            Await ConsultarEncabezado(True, FiltroVM)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicializar la ejecución del filtro", Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción "Búsqueda avanzada" de la barra de herramientas.
    ''' Presenta la forma de búsqueda para que el usuario seleccione los valores por los cuales desea buscar dentro de los campos definidos en la forma de búsqueda
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 3/2014
    ''' Id caso de prueba: Id_4
    ''' Pruebas CB       : Jorge Peña - Abril 3/2014 - Resultado Ok 
    ''' </history>
    Public Overrides Sub Buscar()
        PrepararNuevaBusqueda()
        MyBase.Buscar()
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Buscar de la forma de búsqueda
    ''' Ejecuta una búsqueda por los campos contenidos en la forma de búsqueda y cuyos valores correspondan con los seleccionados por el usuario
    ''' </summary>
    '''<history>
    ''' ID caso de prueba:  Id_3, Id_4
    ''' Descripción:        Se agrega el campo intTipoProducto. CC 827
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              24 de Abril/2014
    ''' Pruebas CB:         Jorge Peña - 24 de Abril/2014 - Resultado OK
    '''</history>
    '''<history>
    ''' ID caso de prueba:  Id_13
    ''' Descripción:        Se agregan los campos strCuentaContableDBPositiva, strCuentaContableDBNegativa, strCuentaContableCRPositiva, strCuentaContableCRNegativa.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              22 de Octubre/2014
    ''' Pruebas CB:         Jorge Peña - 22 de Octubre/2014 - Resultado OK
    '''</history>
    Public Overrides Async Sub ConfirmarBuscar()
        Try
            If Not IsNothing(cb.strConcepto) Or Not IsNothing(cb.strNormaContable) Or Not IsNothing(cb.strCuentaContableDBPositiva) Or
                Not IsNothing(cb.strCuentaContableDBNegativa) Or Not IsNothing(cb.strCuentaContableCRPositiva) Or Not IsNothing(cb.strCuentaContableCRNegativa) Or Not IsNothing(cb.strTipoTitulos) Or Not IsNothing(cb.intTipoProducto) Or
                Not IsNothing(cb.strTipoInversion) Then 'Validar que ingresó algo en los campos de búsqueda
                Await ConsultarEncabezado(False, String.Empty, cb.strConcepto, cb.strNormaContable, cb.strCuentaContableDBPositiva, cb.strCuentaContableDBNegativa, cb.strCuentaContableCRPositiva, cb.strCuentaContableCRNegativa, cb.strTipoTitulos, CInt(cb.intTipoProducto), cb.strTipoInversion)
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
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 3/2014
    ''' Id caso de prueba: Id_6
    ''' Pruebas CB       : Jorge Peña - Abril 3/2014 - Resultado Ok 
    ''' </history>
    '''<history>
    ''' ID caso de prueba:  Id_6
    ''' Descripción:        Se agrega el campo intTipoProducto. CC 827
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              24 de Abril/2014
    ''' Pruebas CB:         Jorge Peña - 24 de Abril/2014 - Resultado OK
    '''</history>
    Public Overrides Sub ActualizarRegistro()

        Dim intNroOcurrencias As Integer
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            ErrorForma = String.Empty
            IsBusy = True

            If ValidarRegistro() Then
                ' Incializar los mensajes de validación
                _EncabezadoSeleccionado.strMsgValidacion = String.Empty
                ' Validar si el registro ya existe en la lista
                intNroOcurrencias = (From e In ListaEncabezado Where e.intID = _EncabezadoSeleccionado.intID Select e).Count

                If intNroOcurrencias = 0 Then
                    strAccion = ValoresUserState.Ingresar.ToString()
                    ListaEncabezado.Add(_EncabezadoSeleccionado)
                End If

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
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then

                If mdcProxy.IsLoading Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                IsBusy = True

                _EncabezadoSeleccionado.strUsuario = Program.Usuario

                mobjEncabezadoAnterior = ObtenerRegistroAnterior()

                Editando = True
                MyBase.CambioItem("Editando")

                HabilitarEncabezado = True

                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar el proceso de edición.", Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Cancelar de la barra de herramientas durante el ingreso o modificación del encabezado activo
    ''' </summary>
    ''' 
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = String.Empty

            If Not _EncabezadoSeleccionado Is Nothing Then
                mdcProxy.RejectChanges()

                Editando = False
                MyBase.CambioItem("Editando")

                EncabezadoSeleccionado = mobjEncabezadoAnterior
                HabilitarEncabezado = False
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
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 3/2014
    ''' Id caso de prueba: Id_8
    ''' Pruebas CB       : Jorge Peña - Abril 3/2014 - Resultado Ok 
    ''' </history>
    '''<history>
    ''' ID caso de prueba:  Id_9
    ''' Descripción:        Se agrega el campo intTipoProducto. CC 827
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              24 de Abril/2014
    ''' Pruebas CB:         Jorge Peña - 24 de Abril/2014 - Resultado OK
    '''</history>
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción borra el concepto seleccionado. ¿Confirma el borrado de este concepto?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcutá cuando el usuario confirma la eliminación del encabezado activo.
    ''' Ejecuta el proceso de eliminación en la base de datos
    ''' </summary>
    Private Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    IsBusy = True

                    mobjEncabezadoAnterior = ObtenerRegistroAnterior()

                    If mdcProxy.ConfiguracionContableConceptos.Where(Function(i) i.intID = EncabezadoSeleccionado.intID).Count > 0 Then
                        mdcProxy.ConfiguracionContableConceptos.Remove(mdcProxy.ConfiguracionContableConceptos.Where(Function(i) i.intID = EncabezadoSeleccionado.intID).First)
                    End If


                    If _ListaEncabezado.Count > 0 Then
                        EncabezadoSeleccionado = _ListaEncabezado.LastOrDefault
                    Else
                        EncabezadoSeleccionado = Nothing
                    End If

                    Program.VerificarCambiosProxyServidor(mdcProxy)
                    mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, ValoresUserState.Borrar.ToString)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    '''<history>
    ''' ID caso de prueba:  Id_3, Id_4
    ''' Descripción:        Se agrega el campo intTipoProducto. CC 827
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              24 de Abril/2014
    ''' Pruebas CB:         Jorge Peña - 24 de Abril/2014 - Resultado OK
    '''</history>
    '''<history>
    ''' ID caso de prueba:  Id_12, Id_13
    ''' Descripción:        Se agregan los campos strCuentaContableDBPositiva, strCuentaContableDBNegativa, strCuentaContableCRPositiva, strCuentaContableCRNegativa.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              22 de Octubre/2014
    ''' Pruebas CB:         Jorge Peña - 22 de Octubre/2014 - Resultado OK
    '''</history>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaConfiguracionContableConcepto
            objCB.strConcepto = String.Empty
            objCB.strNormaContable = String.Empty
            objCB.strCuentaContableDBPositiva = String.Empty
            objCB.strCuentaContableDBNegativa = String.Empty
            objCB.strCuentaContableCRPositiva = String.Empty
            objCB.strCuentaContableCRNegativa = String.Empty
            objCB.strTipoTitulos = String.Empty
            objCB.intTipoProducto = 0
            objCB.strTipoInversion = String.Empty
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
    ''' 
    Private Function ObtenerRegistroAnterior() As CFCodificacionContable.ConfiguracionContableConcepto
        Dim objEncabezado As CFCodificacionContable.ConfiguracionContableConcepto = New CFCodificacionContable.ConfiguracionContableConcepto

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of CFCodificacionContable.ConfiguracionContableConcepto)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.intID = _EncabezadoSeleccionado.intID
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para guardar los datos del registro activo antes de su modificación.", Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objEncabezado)
    End Function

    ''' <summary>
    ''' Función para validar todos los campos obligatorios de la pantalla, si la pantalla tiene detalle 
    ''' esta función realizará el llamado a otra función para validar el detalle.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: Id_5
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 3/2014
    ''' Pruebas CB       : Jorge Peña - Abril 3/2014 - Resultado Ok 
    ''' </history>
    '''<history>
    ''' ID caso de prueba:  Id_5
    ''' Descripción:        Se agrega el campo intTipoProducto. CC 827
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              24 de Abril/2014
    ''' Pruebas CB:         Jorge Peña - 24 de Abril/2014 - Resultado OK
    '''</history>
    '''<history>
    ''' ID caso de prueba:  Id_14
    ''' Descripción:        Se agregan los campos strCuentaContableDBPositiva, strCuentaContableCRPositiva.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              22 de Octubre/2014
    ''' Pruebas CB:         Jorge Peña - 22 de Octubre/2014 - Resultado OK
    '''</history>
    Private Function ValidarRegistro() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then
                'Valida la norma contable
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strNormaContable) Then
                    strMsg = String.Format("{0}{1} + La norma contable es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el concepto
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strConcepto) Then
                    strMsg = String.Format("{0}{1} + El concepto es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el tipo título
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strTipoTitulos) Then
                    strMsg = String.Format("{0}{1} + El tipo título es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el tipo producto
                If (_EncabezadoSeleccionado.intTipoProducto) = 0 Then
                    strMsg = String.Format("{0}{1} + El tipo producto es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el tipo inversión
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strTipoInversion) Then
                    strMsg = String.Format("{0}{1} + El tipo inversión es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la cuenta contable débito
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strCuentaContableDBPositiva) Then
                    strMsg = String.Format("{0}{1} + La cuenta contable débito es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la cuenta contable crédito
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strCuentaContableCRPositiva) Then
                    strMsg = String.Format("{0}{1} + La cuenta contable crédito es un campo requerido.", strMsg, vbCrLf)
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
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 3/2014
    ''' Id caso de prueba: Id_7
    ''' Pruebas CB       : Jorge Peña - Abril 3/2014 - Resultado Ok 
    ''' </history>
    '''<history>
    ''' ID caso de prueba:  Id_8
    ''' Descripción:        Se agrega el campo intTipoProducto. CC 827
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              24 de Abril/2014
    ''' Pruebas CB:         Jorge Peña - 24 de Abril/2014 - Resultado OK
    '''</history>
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

                If So.UserState.Equals(ValoresUserState.Borrar.ToString) Then
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
                Await ConsultarEncabezado(True, String.Empty)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del resultado de la actualización de los datos", Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
            If Not So Is Nothing Then
                If So.HasError Then
                    So.MarkErrorAsHandled()
                End If
            End If
        End Try
    End Sub

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Async Sub ConsultarEncabezadoPorDefectoSync()
        Dim objRet As LoadOperation(Of CFCodificacionContable.ConfiguracionContableConcepto)
        Dim dcProxy As CodificacionContableDomainContext

        Try
            dcProxy = inicializarProxyCodificacionContable()

            objRet = Await dcProxy.Load(dcProxy.ConsultarConfiguracionContableConceptoPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "ConsultarEncabezadoPorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    mobjEncabezadoPorDefecto = objRet.Entities.FirstOrDefault
                End If
            Else
                mobjEncabezadoPorDefecto = Nothing
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "ConsultarEncabezadoPorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de ConfiguracionContableConcepto
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>   
    ''' <history>
    ''' Id caso de prueba: Id_1 
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 3/2014
    ''' Pruebas CB       : Jorge Peña - Abril 3/2014 - Resultado Ok 
    ''' </history>
    '''<history>
    ''' ID caso de prueba:  Id_1, Id_2
    ''' Descripción:        Se agrega el campo intTipoProducto. CC 827
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              24 de Abril/2014
    ''' Pruebas CB:         Jorge Peña - 24 de Abril/2014 - Resultado OK
    '''</history>
    '''<history>
    ''' ID caso de prueba:  Id_13
    ''' Descripción:        Se agregan los campos strCuentaContableDBPositiva, strCuentaContableDBNegativa, strCuentaContableCRPositiva, strCuentaContableCRNegativa.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              22 de Octubre/2014
    ''' Pruebas CB:         Jorge Peña - 22 de Octubre/2014 - Resultado OK
    '''</history>
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal pstrConcepto As String = "",
                                               Optional ByVal pstrNormaContable As String = "",
                                               Optional ByVal pstrCuentaContableDBPositiva As String = "",
                                               Optional ByVal pstrCuentaContableDBNegativa As String = "",
                                               Optional ByVal pstrCuentaContableCRPositiva As String = "",
                                               Optional ByVal pstrCuentaContableCRNegativa As String = "",
                                               Optional ByVal pstrDetalleTipoTitulos As String = "",
                                               Optional ByVal pintTipoProducto As Integer = 0,
                                               Optional ByVal pstrTipoInversion As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCodificacionContable.ConfiguracionContableConcepto)

        Try
            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCodificacionContable()
            End If

            mdcProxy.ConfiguracionContableConceptos.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxy.Load(mdcProxy.FiltrarConfiguracionContableConceptoSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxy.Load(mdcProxy.ConsultarConfiguracionContableConceptoSyncQuery(pstrConcepto:=pstrConcepto,
                                                                                                      pstrNormaContable:=pstrNormaContable,
                                                                                                      pstrCuentaContableDBPositiva:=pstrCuentaContableDBPositiva,
                                                                                                      pstrCuentaContableDBNegativa:=pstrCuentaContableDBNegativa,
                                                                                                      pstrCuentaContableCRPositiva:=pstrCuentaContableCRPositiva,
                                                                                                      pstrCuentaContableCRNegativa:=pstrCuentaContableCRNegativa,
                                                                                                      pstrDetalleTipoTitulos:=pstrDetalleTipoTitulos,
                                                                                                      pintTipoProducto:=pintTipoProducto,
                                                                                                      pstrTipoInversion:=pstrTipoInversion,
                                                                                                        pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
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
                    ListaEncabezado = mdcProxy.ConfiguracionContableConceptos

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
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de ConfiguracionContableConcepto ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
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
'''<history>
''' ID caso de prueba:  Id_13
''' Descripción:        Se agregan los campos strCuentaContableDBPositiva, strCuentaContableDBNegativa, strCuentaContableCRPositiva, strCuentaContableCRNegativa.
''' Responsable:        Jorge Peña (Alcuadrado S.A.)
''' Fecha:              22 de Octubre/2014
''' Pruebas CB:         Jorge Peña - 22 de Octubre/2014 - Resultado OK
'''</history>
Public Class CamposBusquedaConfiguracionContableConcepto
    Implements INotifyPropertyChanged

    Private _strConcepto As String
    Public Property strConcepto() As String
        Get
            Return _strConcepto
        End Get
        Set(ByVal value As String)
            _strConcepto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strConcepto"))
        End Set
    End Property

    Private _strNormaContable As String
    Public Property strNormaContable() As String
        Get
            Return _strNormaContable
        End Get
        Set(ByVal value As String)
            _strNormaContable = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNormaContable"))
        End Set
    End Property

    Private _strTipoTitulos As String
    Public Property strTipoTitulos() As String
        Get
            Return _strTipoTitulos
        End Get
        Set(ByVal value As String)
            _strTipoTitulos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoTitulos"))
        End Set
    End Property

    '''<history>
    ''' Descripción:        Se agrega el campo intTipoProducto. CC 827
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              24 de Abril/2014
    ''' Pruebas CB:         Jorge Peña - 24 de Abril/2014 - Resultado OK
    '''</history>
    Private _intTipoProducto As System.Nullable(Of Integer)
    Public Property intTipoProducto() As System.Nullable(Of Integer)
        Get
            Return _intTipoProducto
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intTipoProducto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intTipoProducto"))
        End Set
    End Property

    Private _strTipoInversion As String
    Public Property strTipoInversion() As String
        Get
            Return _strTipoInversion
        End Get
        Set(ByVal value As String)
            _strTipoInversion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoInversion"))
        End Set
    End Property

    Private _strCuentaContableDBPositiva As String
    Public Property strCuentaContableDBPositiva() As String
        Get
            Return _strCuentaContableDBPositiva
        End Get
        Set(ByVal value As String)
            _strCuentaContableDBPositiva = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strCuentaContableDBPositiva"))
        End Set
    End Property

    Private _strCuentaContableDBNegativa As String
    Public Property strCuentaContableDBNegativa() As String
        Get
            Return _strCuentaContableDBNegativa
        End Get
        Set(ByVal value As String)
            _strCuentaContableDBNegativa = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strCuentaContableDBNegativa"))
        End Set
    End Property

    Private _strCuentaContableCRPositiva As String
    Public Property strCuentaContableCRPositiva() As String
        Get
            Return _strCuentaContableCRPositiva
        End Get
        Set(ByVal value As String)
            _strCuentaContableCRPositiva = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strCuentaContableCRPositiva"))
        End Set
    End Property

    Private _strCuentaContableCRNegativa As String
    Public Property strCuentaContableCRNegativa() As String
        Get
            Return _strCuentaContableCRNegativa
        End Get
        Set(ByVal value As String)
            _strCuentaContableCRNegativa = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strCuentaContableCRNegativa"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class