Imports A2Utilidades.Mensajes
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Globalization
Imports System.ComponentModel
Imports Telerik.Windows.Controls

Partial Public Class BuscadorEspecie
    Inherits UserControl

#Region "Inicialización"

    Public Sub New()
        Try
            InitializeComponent()

            acbEspecies.SearchText = ""

            mobjVM = CType(Me.LayoutRoot.Resources(STR_NOMBRE_VIEW_MODEL), BuscadorEspecieViewModel)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización del buscador de especies.", Me.ToString(), "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorEspecie_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Dim logBuscar As Boolean = False

        Try
            If Not Program.IsDesignMode() Then
                If Me.mobjVM.Inicializado = False Then
                    '// Validar si el control ha sido inicializado es la primera ejecución para asegurar que no se vuelvan a inicializar los controles
                    '   Se valida el valor de la propiedad en el VM porque cuando la propiedad en el control ha sido asignada por Binding se 
                    '   dispara en momento diferente a cuando se asigna un valor fijo. Si se asigna un valor fijo y se deja esta instrucción
                    '   se lanzaría dos veces la asignación de la propiedad IdItem del VM 
                    If Not Me.GetValue(NemotecnicoDep) Is Nothing And Me.mobjVM.Nemotecnico.Equals(String.Empty) Then '// En el VM la propiedad Nemotecnico se inicializa en ""
                        Me.mobjVM.Nemotecnico = Me.GetValue(NemotecnicoDep).ToString
                    ElseIf Not Me.GetValue(NemotecnicoDep) Is Nothing Then
                        logBuscar = True
                        txtFiltroISIN.Text = String.Empty
                        Me.mobjVM.consultarEspecies()
                    End If

                    Me.grDatosEsp.Visibility = Visibility.Collapsed
                    Me.stackFaciales.Visibility = Visibility.Collapsed

                    If Me.mobjVM.Nemotecnico.Equals(String.Empty) And Me.BuscarAlIniciar And logBuscar = False Then
                        txtFiltroISIN.Text = String.Empty
                        Me.mobjVM.consultarEspecies()
                    End If

                    Me.mobjVM.Inicializado = True

                    Me.mobjVM.ViewBuscadorEspecie = Me
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga inicial del buscador de especies.", Me.ToString(), "BuscadorEspecie_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Evento que se lanza cuando se selecciona una especie
    ''' </summary>
    ''' <param name="pstrNemotecnico">Nemotécnico que se está buscando si es una búsqueda específica</param>
    ''' <param name="pobjEspecie">Especie seleccionada</param>
    ''' <remarks></remarks>
    Public Event especieAsignada(ByVal pstrNemotecnico As String, ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
    Public Event especieAsignadaControlOrigen(ByVal pstrControlOrigen As String, ByVal pstrNemotecnico As String, ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)

    ''' <summary>
    ''' Evento que se lanza cuando se selecciona el nemotecnico
    ''' </summary>
    ''' <param name="pstrNemotecnico">Nemotécnico que se está buscando si es una búsqueda específica</param>
    ''' <param name="pstrNombreEspecie">Nombre del nemotecnico seleccionado</param>
    ''' <remarks></remarks>
    Public Event nemotecnicoAsignado(ByVal pstrNemotecnico As String, ByVal pstrNombreEspecie As String)

    ''' <summary>
    ''' Evento que se lanza cuando se selecciona el nemotecnico
    ''' </summary>
    ''' <param name="pobjNemotecnico">Nemotécnico que se está buscando si es una búsqueda específica</param>
    ''' <param name="pstrNemotecnico">Nombre del nemotecnico seleccionado</param>
    ''' <remarks></remarks>
    Public Event EspecieAgrupadaAsignado(ByVal pobjNemotecnico As EspeciesAgrupadas, ByVal pstrNemotecnico As String)

#End Region

#Region "Constantes"

    Private Const STR_NOMBRE_VIEW_MODEL As String = "VM"
    Private Const STR_TEXTO_ABRIR_POPUP As String = " " '// CCM20120305 - Eliminar espacio en blanco asignado para que abra el popup cuando carga la búsqueda

#End Region

#Region "Variables"

    Public mlogDigitandoFiltro As Boolean = False
    Private mlogPrimeraBusqueada As Boolean = True
    Private mlogCerrarBusquedaISIN As Boolean = True
    Public WithEvents mobjVM As BuscadorEspecieViewModel ' Referencia al view model del control
    Private mlogConsultarIsinesPlus As Boolean = False
#End Region

#Region "Propiedades"

    Private Shared NemotecnicoDep As DependencyProperty = DependencyProperty.Register("Nemotecnico", GetType(String), GetType(BuscadorEspecie), New PropertyMetadata(AddressOf cambioPropiedadDep))
    Private Shared ReadOnly ClaseOrdenDep As DependencyProperty = DependencyProperty.Register("ClaseOrden", GetType(BuscadorEspecieViewModel.ClasesEspecie), GetType(BuscadorEspecie), New PropertyMetadata(BuscadorEspecieViewModel.ClasesEspecie.T, New PropertyChangedCallback(AddressOf ClaseOrdenChanged)))
    Private Shared EstadoEspecieDep As DependencyProperty = DependencyProperty.Register("EstadoEspecie", GetType(BuscadorEspecieViewModel.EstadosEspecie), GetType(BuscadorEspecie), New PropertyMetadata(AddressOf cambioPropiedadDep))
    Private Shared CargarEspeciesRestriccionDep As DependencyProperty = DependencyProperty.Register("CargarEspeciesRestriccion", GetType(Boolean), GetType(BuscadorEspecie), New PropertyMetadata(AddressOf cambioPropiedadDep))
    Private Shared ReadOnly TipoNegocioDep As DependencyProperty = DependencyProperty.Register("TipoNegocio", GetType(String), GetType(BuscadorEspecie), New PropertyMetadata("", New PropertyChangedCallback(AddressOf TipoNegocioChanged)))
    Private Shared ReadOnly TipoProductoDep As DependencyProperty = DependencyProperty.Register("TipoProducto", GetType(String), GetType(BuscadorEspecie), New PropertyMetadata("", New PropertyChangedCallback(AddressOf TipoProductoChanged)))
    Private Shared ReadOnly EditandoDep As DependencyProperty = DependencyProperty.Register("Editando", GetType(Boolean), GetType(BuscadorEspecie), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf EditandoChanged)))
    Private Shared ReadOnly BorrarEspecieDep As DependencyProperty = DependencyProperty.Register("BorrarEspecie", GetType(Boolean), GetType(BuscadorEspecie), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf BorrarEspecieChanged)))
    Private Shared ReadOnly EspecieBuscarDep As DependencyProperty = DependencyProperty.Register("EspecieBuscar", GetType(String), GetType(BuscadorEspecie), New PropertyMetadata("", New PropertyChangedCallback(AddressOf EspecieBuscarChanged)))
    Private Shared ReadOnly HabilitarConsultaISINDep As DependencyProperty = DependencyProperty.Register("HabilitarConsultaISIN", GetType(Boolean), GetType(BuscadorEspecie), New PropertyMetadata(True, New PropertyChangedCallback(AddressOf HabilitarConsultaISINChanged)))
    Private Shared ReadOnly ConsultarIsinesDep As DependencyProperty = DependencyProperty.Register("ConsultarIsines", GetType(Boolean), GetType(BuscadorEspecie), New PropertyMetadata(True, New PropertyChangedCallback(AddressOf ConsultarIsinesChanged)))
    Private Shared ReadOnly ConsultandoIsinesPlusDep As DependencyProperty = DependencyProperty.Register("ConsultandoIsinesPlus", GetType(Boolean), GetType(BuscadorEspecie), New PropertyMetadata(True, New PropertyChangedCallback(AddressOf ConsultandoIsinesPlusChanged)))
    Private Shared ReadOnly FechaEmisionDep As DependencyProperty = DependencyProperty.Register("FechaEmision", GetType(Nullable(Of DateTime)), GetType(BuscadorEspecie), New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf FechaEmisionChanged)))
    Private Shared ReadOnly FechaVencimientoDep As DependencyProperty = DependencyProperty.Register("FechaVencimiento", GetType(Nullable(Of DateTime)), GetType(BuscadorEspecie), New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf FechaVencimientoChanged)))
    Private Shared ReadOnly ModalidadDep As DependencyProperty = DependencyProperty.Register("Modalidad", GetType(String), GetType(BuscadorEspecie), New PropertyMetadata("", New PropertyChangedCallback(AddressOf ModalidadChanged)))
    Private Shared ReadOnly MostrarLimpiarDep As DependencyProperty = DependencyProperty.Register("MostrarLimpiar", GetType(Boolean), GetType(BuscadorEspecie), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf MostrarLimpiarChanged)))

    ''' <summary>
    ''' Nemotécnico de la especie que se busca si se trata de una búsqueda específica
    ''' </summary>
    Public Property Nemotecnico As String
        Get
            Return (CType(Me.GetValue(NemotecnicoDep), String))
        End Get
        Set(ByVal value As String)
            Me.SetValue(NemotecnicoDep, value)
            Me.mobjVM.Nemotecnico = value
        End Set
    End Property

    ''' <summary>
    ''' ClaseOrden al cual pertenencen las especies que se desea consultar (acciones, renta fija)
    ''' </summary>
    Public Property ClaseOrden As BuscadorEspecieViewModel.ClasesEspecie
        Get
            Return (CType(Me.GetValue(ClaseOrdenDep), BuscadorEspecieViewModel.ClasesEspecie))
        End Get
        Set(ByVal value As BuscadorEspecieViewModel.ClasesEspecie)
            Me.SetValue(ClaseOrdenDep, value)
            If Not IsNothing(Me.mobjVM) Then
                Me.mobjVM.ClaseOrden = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Modificado por Juan David Correa
    ''' Se adiciona el cambio de propiedad para la clase de la
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Shared Sub ClaseOrdenChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorEspecie = DirectCast(d, BuscadorEspecie)

            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.ClaseOrden = obj.ClaseOrden
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorEspecie", "ClaseOrdenChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Estado de las especies que se desea consultar (activas, inactivas, todas)
    ''' </summary>
    Public Property EstadoEspecie As BuscadorEspecieViewModel.EstadosEspecie
        Get
            Return (CType(Me.GetValue(EstadoEspecieDep), BuscadorEspecieViewModel.EstadosEspecie))
        End Get
        Set(ByVal value As BuscadorEspecieViewModel.EstadosEspecie)
            Me.SetValue(EstadoEspecieDep, value)
            Me.mobjVM.EstadoEspecie = value
        End Set
    End Property

    Private _mstrAgrupamiento As String = String.Empty
    ''' <summary>
    ''' Permite indicar otra condición de búsqueda. No tiene una definición particular.
    ''' </summary>
    Public Property Agrupamiento As String
        Get
            Return (_mstrAgrupamiento)
        End Get
        Set(ByVal value As String)
            _mstrAgrupamiento = value
            Me.mobjVM.Agrupamiento = value
        End Set
    End Property

    Private _mlogVerDetalle As Boolean = False
    ''' <summary>
    ''' Indica si en la parte inferior del buscador se depliega el detalle del elemento seleccionado
    ''' </summary>
    Public Property VerDetalle As Boolean
        Get
            Return (_mlogVerDetalle)
        End Get
        Set(ByVal value As Boolean)
            _mlogVerDetalle = value
        End Set
    End Property

    Private _mlogBuscarAlIniciar As Boolean = False
    ''' <summary>
    ''' Indica si al iniciar el control lanza una consulta de comitentes. Solamente aplica si IdComitente no se envía
    ''' </summary>
    Public Property BuscarAlIniciar As Boolean
        Get
            Return (_mlogBuscarAlIniciar)
        End Get
        Set(ByVal value As Boolean)
            _mlogBuscarAlIniciar = value
        End Set
    End Property

    ''' <summary>
    ''' Especie seleccionada en el buscador
    ''' </summary>
    Public ReadOnly Property EspecieActiva() As OYDUtilidades.BuscadorEspecies
        Get
            Return (mobjVM.EspecieSeleccionada)
        End Get
    End Property

    ''' <summary>
    ''' Tipo de negocio por el cual se realizara el filtro de las especies.
    ''' Desarrollado por Juan David Correa
    ''' Fecha 21 de septiembre del 2012.
    ''' </summary>
    Public Property TipoNegocio As String
        Get
            Return CStr(GetValue(TipoNegocioDep))
        End Get
        Set(ByVal value As String)
            SetValue(TipoNegocioDep, value)
        End Set
    End Property

    Private Shared Sub TipoNegocioChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorEspecie = DirectCast(d, BuscadorEspecie)

            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.TipoNegocio = obj.TipoNegocio
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorEspecie", "TipoNegocioChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    ''' <summary>
    ''' Tipo de producto por el cual se realizara el filtro de las especies.
    ''' Desarrollado por Juan David Correa
    ''' Fecha 21 de septiembre del 2012.
    ''' </summary>
    Public Property TipoProducto As String
        Get
            Return CStr(GetValue(TipoProductoDep))
        End Get
        Set(ByVal value As String)
            SetValue(TipoProductoDep, value)
        End Set
    End Property

    Private Shared Sub TipoProductoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorEspecie = DirectCast(d, BuscadorEspecie)

            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.TipoProducto = obj.TipoProducto
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorEspecie", "TipoProductoChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    ''' Saber si se encuentra en modo edición para lanzar la consulta de especies.
    ''' Desarrollado por Juan David Correa
    ''' Fecha 21 de septiembre del 2012.
    Public Property Editando As Boolean
        Get
            Return CBool(GetValue(EditandoDep))
        End Get
        Set(ByVal value As Boolean)
            SetValue(EditandoDep, value)
        End Set
    End Property

    Private Shared Sub EditandoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorEspecie = DirectCast(d, BuscadorEspecie)

            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.Editando = obj.Editando
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorEspecie", "EditandoChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Borrar los datos de la especie seleccionada.
    ''' Desarrollado por Juan David Correa
    ''' Fecha 21 de septiembre del 2012.
    ''' </summary>
    Public Property BorrarEspecie As Boolean
        Get
            Return CBool(GetValue(BorrarEspecieDep))
        End Get
        Set(ByVal value As Boolean)
            SetValue(BorrarEspecieDep, value)
        End Set
    End Property

    Private Shared Sub BorrarEspecieChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorEspecie = DirectCast(d, BuscadorEspecie)

            If Not IsNothing(obj.BorrarEspecie) Then
                If obj.BorrarEspecie Then
                    If Not IsNothing(obj.mobjVM) Then
                        obj.mobjVM.ListaBusquedaControl = Nothing
                        obj.mobjVM.Isines = Nothing
                        obj.mobjVM.ItemSeleccionadoBuscador = Nothing
                        obj.acbEspecies.SearchText = String.Empty
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorEspecie", "BorrarEspecieChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Buscar una especie en especifico.
    ''' Desarrollado por Juan David Correa
    ''' Fecha 21 de octubre del 2012.
    ''' </summary>
    Public Property EspecieBuscar As String
        Get
            Return CStr(GetValue(EspecieBuscarDep))
        End Get
        Set(ByVal value As String)
            SetValue(EspecieBuscarDep, value)
        End Set
    End Property

    Private Shared Sub EspecieBuscarChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorEspecie = DirectCast(d, BuscadorEspecie)

            If Not String.IsNullOrEmpty(obj.EspecieBuscar) Then
                If Not IsNothing(obj.mobjVM) Then
                    If obj.ConsultarIsines Then
                        obj.ConsultandoIsinesPlus = True
                        obj.mobjVM.CondicionFiltro = obj.EspecieBuscar
                        obj.mobjVM.Nemotecnico = obj.EspecieBuscar
                        obj.mobjVM.MostrarConsultando = Visibility.Collapsed
                        obj.mobjVM.logConsultarIsines = obj.ConsultarIsines
                    End If
                    obj.acbEspecies.SearchText = LTrim(RTrim(obj.EspecieBuscar))
                    obj.acbEspecies.IsDropDownOpen = False
                    obj.mobjVM.consultarEspecies()


                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorEspecie", "EspecieBuscarChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Indica si se debe de mostrar la consulta de ISINES
    ''' </summary>
    Public Property HabilitarConsultaISIN As Boolean
        Get
            Return CBool(GetValue(HabilitarConsultaISINDep))
        End Get
        Set(ByVal value As Boolean)
            SetValue(HabilitarConsultaISINDep, value)

        End Set
    End Property

    Private Shared Sub HabilitarConsultaISINChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorEspecie = DirectCast(d, BuscadorEspecie)

            If Not IsNothing(obj.HabilitarConsultaISIN) Then
                obj.mobjVM.HabilitarConsultaISINES = obj.HabilitarConsultaISIN
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorEspecie", "HabilitarConsultaISINChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private _mlogCargarEspeciesRestriccion As Boolean = False
    ''' <summary>
    ''' Indica si al iniciar el control lanza una consulta de comitentes. Solamente aplica si IdComitente no se envía
    ''' </summary>
    Public Property CargarEspeciesRestriccion As Boolean
        Get
            Return (_mlogCargarEspeciesRestriccion)
        End Get
        Set(ByVal value As Boolean)
            _mlogCargarEspeciesRestriccion = value
            mobjVM.CargarEspeciesConReestriccion = _mlogCargarEspeciesRestriccion
        End Set
    End Property

    Private _mlogTraerEspeciesVencidas As Boolean = False
    ''' <summary>
    ''' Indica si se debe de traer especies con la fecha de vencimiento inferior a la fecha actual
    ''' </summary>
    Public Property TraerEspeciesVencidas As Boolean
        Get
            Return (_mlogTraerEspeciesVencidas)
        End Get
        Set(ByVal value As Boolean)
            _mlogTraerEspeciesVencidas = value
            mobjVM.TraerEspeciesVencidas = _mlogTraerEspeciesVencidas
        End Set
    End Property

    Public Property MostrarLimpiar As Boolean
        Get
            Return CBool(GetValue(MostrarLimpiarDep))
        End Get
        Set(ByVal value As Boolean)
            SetValue(MostrarLimpiarDep, value)
        End Set
    End Property

    Private Shared Sub MostrarLimpiarChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorEspecie = DirectCast(d, BuscadorEspecie)

            If Not IsNothing(obj.mobjVM) Then
                If obj.MostrarLimpiar Then
                    obj.cmbLimpiar.Visibility = Visibility.Visible
                Else
                    obj.cmbLimpiar.Visibility = Visibility.Collapsed
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorGenerico", "BorrarItemChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#Region "Consultando Isines"
    ''' <summary>
    ''' Consultando Isines Plus PARA VALIDAR que esta CARGANDO
    ''' </summary>
    ''' 
    Public Property ConsultandoIsinesPlus As Boolean
        Get
            Return CBool(GetValue(ConsultandoIsinesPlusDep))
        End Get
        Set(ByVal value As Boolean)
            SetValue(ConsultandoIsinesPlusDep, value)
        End Set
    End Property

    Private Shared Sub ConsultandoIsinesPlusChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorEspecie", "ConsultandoIsinesPlusChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
#End Region

#Region "Consultar Isines"
    Public Property ConsultarIsines As Boolean
        Get
            Return CBool(GetValue(ConsultarIsinesDep))
        End Get
        Set(ByVal value As Boolean)
            SetValue(ConsultarIsinesDep, value)
        End Set
    End Property
    Private Shared Sub ConsultarIsinesChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorEspecie = DirectCast(d, BuscadorEspecie)

            If obj.ConsultarIsines Then
                obj.mlogConsultarIsinesPlus = True
            Else
                obj.mlogConsultarIsinesPlus = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorEspecie", "ConsultarIsinesChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
#End Region

#Region "Fecha Emision"
    Public Property FechaEmision As Nullable(Of DateTime)
        Get
            Return CDate(GetValue(FechaEmisionDep))
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            SetValue(FechaEmisionDep, value)
        End Set
    End Property
    Private Shared Sub FechaEmisionChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorEspecie = DirectCast(d, BuscadorEspecie)
            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.FechaEmision = obj.FechaEmision
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorEspecie", "FechaEmisionChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
#End Region
#Region "Fecha Vencimiento"
    Public Property FechaVencimiento As Nullable(Of DateTime)
        Get
            Return CDate(GetValue(FechaVencimientoDep))
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            SetValue(FechaVencimientoDep, value)
        End Set
    End Property
    Private Shared Sub FechaVencimientoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorEspecie = DirectCast(d, BuscadorEspecie)
            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.FechaVencimiento = obj.FechaVencimiento
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorEspecie", "FechaVencimientoChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
#End Region
#Region "Modalidad"
    Public Property Modalidad As String
        Get
            Return CStr(GetValue(ModalidadDep))
        End Get
        Set(ByVal value As String)
            SetValue(ModalidadDep, value)
        End Set
    End Property
    Private Shared Sub ModalidadChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorEspecie = DirectCast(d, BuscadorEspecie)
            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.CondicionFiltro = obj.EspecieBuscar
                obj.mobjVM.Modalidad = obj.Modalidad
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorEspecie", "ModalidadChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
#End Region
#End Region

#Region "Callback"

    ''' <summary>
    ''' Procedimiento de Call back que se lanza cuando alguna de las dependency properties se modifica
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Shared Sub cambioPropiedadDep(ByVal sender As Object, ByVal args As DependencyPropertyChangedEventArgs)
        Dim obj As BuscadorEspecie = DirectCast(sender, BuscadorEspecie)
        If Not IsNothing(args) Then
            If Not IsNothing(args.Property) Then
                If args.Property.Name = "EstadoEspecie" Then
                    obj.EstadoEspecie = args.NewValue
                ElseIf args.Property.Name = "CargarEspeciesRestriccion" Then
                    obj.CargarEspeciesRestriccion = args.NewValue
                End If
            End If
        End If
    End Sub

#End Region

#Region "Eventos controles"

    ''' <summary>
    ''' Este evento se dispara cuando la especie activa cambia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub acbEspecies_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Dim objSeleccion As EspeciesAgrupadas

        Try
            If Not IsNothing(CType(sender, RadAutoCompleteBox).SelectedItem) Then
                objSeleccion = CType(CType(CType(sender, RadAutoCompleteBox).SelectedItem, clsEspecies_Buscador).ItemBusqueda, EspeciesAgrupadas)
                If objSeleccion Is Nothing Then
                    Me.grDatosEsp.Visibility = Visibility.Collapsed
                    Me.stackFaciales.Visibility = Visibility.Collapsed
                Else
                    If Me.VerDetalle Then
                        Me.grDatosEsp.Visibility = Visibility.Visible
                        If HabilitarConsultaISIN Then
                            Me.stackFaciales.Visibility = Visibility.Visible
                        Else
                            Me.stackFaciales.Visibility = Visibility.Collapsed
                        End If
                    End If

                    '// Desactivar indicador que permite identificar que el usuario digitó una condición de filtro
                    '   Solamente se modifica cuando entra por el else porque si se seleccionó una opción de la lista del autocomplete nunca puede ser nothing
                    '   porque debe ser un elemento de la lista.
                    mlogDigitandoFiltro = False
                    acbEspecies.IsDropDownOpen = False
                End If
            Else
                Me.grDatosEsp.Visibility = Visibility.Collapsed
                Me.stackFaciales.Visibility = Visibility.Collapsed
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar la especie seleccionada.", Me.Name, "acbEspecies_SelectionChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Evento que se dispara cuando el usuario da clic en el botón buscar, para ejecutar una búsqueda de especies
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''
    Private Sub cmdBuscar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            mlogDigitandoFiltro = False
            Me.mobjVM.CondicionFiltro = Me.acbEspecies.SearchText
            txtFiltroISIN.Text = String.Empty
            Me.mobjVM.consultarEspecies()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar especies que cumplan con el filtro indicado.", Me.Name, "cmdBuscar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub acbEspecies_KeyDown(sender As System.Object, e As System.Windows.Input.KeyEventArgs)
        Try
            If e.Key = Key.Tab Then
                If Not IsNothing(Me.mobjVM.ItemSeleccionadoBuscador) Then
                    If LTrim(RTrim(Me.mobjVM.ItemSeleccionadoBuscador.ItemBusqueda.Nemotecnico)) <> LTrim(RTrim(Me.acbEspecies.SearchText)) Then
                        Me.mobjVM.CondicionFiltro = Me.acbEspecies.SearchText
                        mlogDigitandoFiltro = False
                        txtFiltroISIN.Text = String.Empty
                        Me.mobjVM.consultarEspecies()
                    End If
                Else
                    Me.mobjVM.CondicionFiltro = Me.acbEspecies.SearchText
                    mlogDigitandoFiltro = False
                    txtFiltroISIN.Text = String.Empty
                    Me.mobjVM.consultarEspecies()
                End If
            End If
        Catch ex As Exception
            Me.cmdBuscar.IsEnabled = True
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar las especies que cumplan con el filtro indicado.", Me.Name, "acbEspecies_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub acbEspecies_KeyUp(sender As System.Object, e As System.Windows.Input.KeyEventArgs)
        Try
            If e.Key = Key.Enter Then
                If Not IsNothing(Me.mobjVM.ItemSeleccionadoBuscador) Then
                    If LTrim(RTrim(Me.mobjVM.ItemSeleccionadoBuscador.ItemBusqueda.Nemotecnico)) <> LTrim(RTrim(Me.acbEspecies.SearchText)) Then
                        Me.mobjVM.CondicionFiltro = Me.acbEspecies.SearchText
                        mlogDigitandoFiltro = False
                        txtFiltroISIN.Text = String.Empty
                        Me.mobjVM.consultarEspecies()
                    End If
                Else
                    Me.mobjVM.CondicionFiltro = Me.acbEspecies.SearchText
                    mlogDigitandoFiltro = False
                    txtFiltroISIN.Text = String.Empty
                    Me.mobjVM.consultarEspecies()
                End If
            ElseIf e.Key = Key.Back Then
                If Not IsNothing(e) Then
                    If Not IsNothing(e.OriginalSource) Then
                        Dim strTexto As String = CType(e.OriginalSource, System.Windows.Controls.TextBox).Text
                        If Me.acbEspecies.SearchText <> strTexto Then
                            Me.acbEspecies.SearchText = strTexto
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Me.cmdBuscar.IsEnabled = True
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar las especies que cumplan con el filtro indicado.", Me.Name, "acbEspecies_KeyUp", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Este evento se ejecuta cuando finaliza una carga de especies a través del servicio web y es alimentada la lista del autocomplete
    ''' </summary>
    ''' <param name="plogNroEspecies"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub mobjVM_CargaEspeciesCompleta(ByVal plogNroEspecies As Integer, ByVal plogBusquedaNemoEspecifico As Boolean) Handles mobjVM.CargaEspeciesCompleta

        Me.cmdBuscar.IsEnabled = True
        Me.acbEspecies.IsEnabled = True
        txtFiltroISIN.Text = String.Empty

        Try
            If plogNroEspecies = 0 Then
                mostrarMensaje("No hay especies que cumplan con la condición de búsqueda", Program.TituloSistema)
            Else
                'Modificado por Juan David Correa
                'Descripción se adiciona la condición para cargar la especie por defecto si solo se encuentra un registro.
                If Not IsNothing(mobjVM.ListaBusquedaControl) Then
                    If mobjVM.ListaBusquedaControl.Count = 1 Then
                        'acbEspecies.SearchText = LTrim(RTrim(mobjVM.Especies.FirstOrDefault.Nemotecnico))
                        mobjVM.ItemSeleccionadoBuscador = mobjVM.ListaBusquedaControl.First
                        mobjVM.consultarIsines(mobjVM.ItemSeleccionadoBuscador.ItemBusqueda.Nemotecnico)
                        acbEspecies.IsDropDownOpen = False
                    Else
                        If plogBusquedaNemoEspecifico = False Or Me._mlogBuscarAlIniciar Then
                            'If Me.acbEspecies.SearchText.Trim().Equals(String.Empty) Then
                            '    Me.acbEspecies.SearchText = STR_TEXTO_ABRIR_POPUP '// CCM20120305 - Eliminar espacio en blanco asignado para que abra el popup cuando carga la búsqueda - Asignar la constante y no el valor fijo " "
                            'End If
                            acbEspecies.Focus()
                            If Me.Visibility = Visibility.Visible Then
                                acbEspecies.IsDropDownOpen = True
                            End If
                            acbEspecies.Populate(acbEspecies.SearchText)
                        End If
                    End If
                End If

            End If

            '// Desactivar indicador que permite identificar que el usuario digitó una condición de filtro
            mlogDigitandoFiltro = False
        Catch ex As Exception
            mlogDigitandoFiltro = False
            mostrarErrorAplicacion(Program.Usuario, "Falló la actualización de la especie seleccionada", Me.Name, "mobjVM_CargaEspeciesCompleta", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Este evento se dispara cuando en el ViewModel se actualiza una propiedad
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub mobjVM_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles mobjVM.PropertyChanged
        Try
            Select Case e.PropertyName.ToLower ''''NEW
                Case "listabusquedacontrol"
                    If mlogDigitandoFiltro = False Then
                        If Not mobjVM.ListaBusquedaControl Is Nothing Then
                            If mobjVM.ListaBusquedaControl.Count = 1 Then
                                acbEspecies.SelectedItem = mobjVM.ListaBusquedaControl.First
                            End If
                        End If
                    End If
                Case "nemotecnicoseleccionado"

                    '// Si la propiedad cambio porque el usuario está digitando un nuevo filtro no se lanza el evento porque aún no se sabe que nemotécnico se seleccionará.
                    If mlogDigitandoFiltro = False Then
                        If Not mobjVM.NemotecnicoSeleccionado Is Nothing Then
                            'mobjVM.consultarIsines(mobjVM.NemotecnicoSeleccionado.Nemotecnico)
                            RaiseEvent nemotecnicoAsignado(mobjVM.NemotecnicoSeleccionado.Nemotecnico, mobjVM.NemotecnicoSeleccionado.Especie)
                            RaiseEvent EspecieAgrupadaAsignado(mobjVM.NemotecnicoSeleccionado, mobjVM.NemotecnicoSeleccionado.Nemotecnico)
                        End If
                    End If
                Case "isines"
                    If Not mobjVM.Especies Is Nothing Then
                        If HabilitarConsultaISIN Then
                            'Me.cboIsines.SelectedIndex = -1

                            'If Me.cboIsines.Items.Count > 0 Then
                            '    Me.cboIsines.IsOpen = True
                            'End If

                            If Not IsNothing(mobjVM.Isines) Then
                                If mlogConsultarIsinesPlus = False Then
                                    If mobjVM.Isines.Count > 0 Then
                                        If mobjVM.Isines.Count = 1 Then
                                            mobjVM.EspecieSeleccionada = mobjVM.Isines.First
                                        Else
                                            If Me.Visibility = Visibility.Visible Then
                                                Me.cboIsines.IsOpen = True
                                            End If
                                        End If
                                    End If
                                Else
                                    If mobjVM.Isines.Count > 0 Then
                                        If Me.Visibility = Visibility.Visible Then
                                            Me.cboIsines.IsOpen = True
                                        End If
                                        Me.ConsultandoIsinesPlus = False
                                        mobjVM.logConsultarIsines = False
                                    End If
                                End If

                            End If
                        Else
                            'Me.cboIsines.SelectedIndex = 0
                            If mlogCerrarBusquedaISIN Then
                                Me.cboIsines.IsOpen = False
                            End If

                            If Not IsNothing(mobjVM.Isines) Then
                                If mobjVM.Isines.Count > 0 Then
                                    mobjVM.EspecieSeleccionada = mobjVM.Isines.First
                                End If
                            End If
                            'If Not IsNothing(mobjVM.EspecieSeleccionada) Then
                            '    mobjVM.EspecieSeleccionada.CantidadISIN = Me.cboIsines.Items.Count
                            'End If
                            If Not IsNothing(mobjVM.EspecieSeleccionada) Then
                                If Not IsNothing(mobjVM.Isines) Then
                                    mobjVM.EspecieSeleccionada.CantidadISIN = mobjVM.Isines.Count
                                Else
                                    mobjVM.EspecieSeleccionada.CantidadISIN = 0
                                End If
                            End If
                        End If
                    End If
                Case "especieseleccionada"

                    '// Si la propiedad cambio porque el usuario está digitando un nuevo filtro no se lanza el evento porque aún no se sabe que nemotécnico se seleccionará.
                    If mlogDigitandoFiltro = False Then
                        If mobjVM.EspecieSeleccionada Is Nothing And mobjVM.Buscando = False Then
                            '// Si está en proceso de búsqueda (true) no se lanza el evento hasta que este termine
                            RaiseEvent especieAsignada(mobjVM.Nemotecnico, Nothing)
                            RaiseEvent especieAsignadaControlOrigen(Me.Name, mobjVM.Nemotecnico, Nothing)
                            If Me.VerDetalle Then
                                Me.grDatosEsp.Visibility = Visibility.Visible
                                If HabilitarConsultaISIN Then
                                    Me.stackFaciales.Visibility = Visibility.Visible
                                Else
                                    Me.stackFaciales.Visibility = Visibility.Collapsed
                                End If
                            End If
                        ElseIf Not mobjVM.EspecieSeleccionada Is Nothing Then
                            RaiseEvent especieAsignada(mobjVM.EspecieSeleccionada.Nemotecnico, mobjVM.EspecieSeleccionada)
                            RaiseEvent especieAsignadaControlOrigen(Me.Name, mobjVM.EspecieSeleccionada.Nemotecnico, mobjVM.EspecieSeleccionada)
                            If Me.VerDetalle Then
                                Me.grDatosEsp.Visibility = Visibility.Visible
                                If HabilitarConsultaISIN Then
                                    Me.stackFaciales.Visibility = Visibility.Visible
                                Else
                                    Me.stackFaciales.Visibility = Visibility.Collapsed
                                End If
                            End If
                        End If

                        If mlogCerrarBusquedaISIN Then
                            Me.cboIsines.IsOpen = False
                        End If

                    End If
                Case "especieseleccionada"

                    '// Si la propiedad cambio porque el usuario está digitando un nuevo filtro no se lanza el evento porque aún no se sabe que nemotécnico se seleccionará.
                    If mlogDigitandoFiltro = False Then
                        If Not mobjVM.EspecieSeleccionada Is Nothing Then
                            'mobjVM.consultarIsines(mobjVM.NemotecnicoSeleccionado.Nemotecnico)
                            RaiseEvent especieAsignada(mobjVM.EspecieSeleccionada.Nemotecnico, mobjVM.EspecieSeleccionada)
                            RaiseEvent especieAsignadaControlOrigen(Me.Name, mobjVM.EspecieSeleccionada.Nemotecnico, mobjVM.EspecieSeleccionada)
                        End If
                    End If

            End Select
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Falló la sincronización del evento cuando cambio la propiedad " & e.PropertyName.ToLower, Me.Name, "mobjVM_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Este evento se ejecuta cuando el usuario modifica el texto del filtro de búsqueda (texto del autocomplete).
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub acbEspecies_TextChanged(sender As Object, e As EventArgs) Handles acbEspecies.SearchTextChanged
        Try
            If mlogDigitandoFiltro = False Then
                '// Activar el indicador que permite indicar que el usuario está digitando una nueva condición de búsqueda
                mlogDigitandoFiltro = True
                mobjVM.Buscando = False
                Me.Nemotecnico = String.Empty
                Me.grDatosEsp.Visibility = Visibility.Collapsed
                Me.stackFaciales.Visibility = Visibility.Collapsed
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtFiltroISIN_KeyDown(sender As System.Object, e As System.Windows.Input.KeyEventArgs)
        Try
            If e.Key = Key.Tab Then
                If Not IsNothing(Me.mobjVM.NemotecnicoSeleccionado) Then
                    mobjVM.consultarIsinesFiltro(mobjVM.NemotecnicoSeleccionado.Nemotecnico, Me.txtFiltroISIN.Text)
                End If
            End If
        Catch ex As Exception
            Me.cmdBuscar.IsEnabled = True
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar las especies que cumplan con el filtro indicado.", Me.Name, "acbEspecies_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtFiltroISIN_KeyUp(sender As System.Object, e As System.Windows.Input.KeyEventArgs)
        Try
            If e.Key = Key.Enter Then
                If Not IsNothing(Me.mobjVM.NemotecnicoSeleccionado) Then
                    mlogCerrarBusquedaISIN = False
                    mobjVM.consultarIsinesFiltro(mobjVM.NemotecnicoSeleccionado.Nemotecnico, Me.txtFiltroISIN.Text)
                    mlogCerrarBusquedaISIN = True
                End If
            ElseIf e.Key = Key.Back Then
                If Not IsNothing(e) Then
                    If Not IsNothing(e.OriginalSource) Then
                        Dim strTexto As String = CType(e.OriginalSource, System.Windows.Controls.TextBox).Text
                        If Me.txtFiltroISIN.Text <> strTexto Then
                            Me.txtFiltroISIN.Text = strTexto
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Me.cmdBuscar.IsEnabled = True
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar las especies que cumplan con el filtro indicado.", Me.Name, "acbEspecies_KeyUp", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnFiltroISIN_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM.NemotecnicoSeleccionado) Then
                mlogCerrarBusquedaISIN = False
                mobjVM.consultarIsinesFiltro(mobjVM.NemotecnicoSeleccionado.Nemotecnico, Me.txtFiltroISIN.Text)
                mlogCerrarBusquedaISIN = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al aplicar el filtro del ISIN", Me.Name, "btnFiltroISIN_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CmbLimpiar_Click(sender As Object, e As RoutedEventArgs)
        Try
            LimpiarRegistro()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Falló la limpieza de los valores.", Me.Name, "CmbLimpiar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Metodos"
    Public Sub Limpiar()
        Try
            LimpiarRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorGenerico", "BorrarItemChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub LimpiarRegistro()
        mobjVM.ListaBusquedaControl = Nothing
        mobjVM.Isines = Nothing
        mobjVM.ItemSeleccionadoBuscador = Nothing
        mobjVM.EspecieSeleccionada = Nothing
        acbEspecies.SearchText = String.Empty
        RaiseEvent especieAsignada(mobjVM.Nemotecnico, Nothing)
        RaiseEvent especieAsignadaControlOrigen(Me.Name, mobjVM.Nemotecnico, Nothing)
    End Sub

#End Region

    ''' <summary>
    ''' Metodo para Obtener el cierre del combo box de ISIN.
    ''' Desarrollado Juan David Correa
    ''' Fecha 21 de septiembre del 2012
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cboIsines_DropDownClosed(sender As System.Object, e As System.EventArgs)
        Try
            If Not String.IsNullOrEmpty(TipoNegocio) Then
                If mobjVM.TipoNegocio.ToLower = "c" Or mobjVM.TipoNegocio.ToLower = "s" Then
                    If Not IsNothing(mobjVM.Isines) Then
                        'If mobjVM.Isines.FirstOrDefault.ISIN <> mobjVM.EspecieSeleccionada.ISIN Then
                        '    mobjVM.EspecieSeleccionada = mobjVM.Isines.FirstOrDefault
                        'End If

                        '// Si la propiedad cambio porque el usuario está digitando un nuevo filtro no se lanza el evento porque aún no se sabe que nemotécnico se seleccionará.
                        If mobjVM.EspecieSeleccionada Is Nothing And mobjVM.Buscando = False Then
                            '// Si está en proceso de búsqueda (true) no se lanza el evento hasta que este termine
                            RaiseEvent especieAsignada(mobjVM.Nemotecnico, Nothing)
                            RaiseEvent especieAsignadaControlOrigen(Me.Name, mobjVM.Nemotecnico, Nothing)
                        ElseIf Not mobjVM.EspecieSeleccionada Is Nothing Then
                            RaiseEvent especieAsignada(mobjVM.EspecieSeleccionada.Nemotecnico, mobjVM.EspecieSeleccionada)
                            RaiseEvent especieAsignadaControlOrigen(Me.Name, mobjVM.EspecieSeleccionada.Nemotecnico, mobjVM.EspecieSeleccionada)
                        End If
                    End If
                End If

            End If

            acbEspecies.IsDropDownOpen = False
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al seleccionar el Isin", Me.Name, "cboIsines_SelectionChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TextBlock_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            Me.cboIsines.IsOpen = False
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al seleccionar el Isin", Me.Name, "TextBlock_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
End Class

Public Class clsFiltroBuscadorEspecies
    Inherits FilteringBehavior

    Public Overrides Function FindMatchingItems(searchText As String, items As IList, escapedItems As IEnumerable(Of Object), textSearchPath As String, textSearchMode As TextSearchMode) As IEnumerable(Of Object)
        If Not String.IsNullOrEmpty(searchText) Then
            Dim results = items.OfType(Of clsEspecies_Buscador).Where(Function(x) x.ItemBusqueda.Nemotecnico.ToUpper(CultureInfo.InvariantCulture).Contains(searchText.ToUpper) _
                                                                                     Or x.ItemBusqueda.Especie.ToUpper(CultureInfo.InvariantCulture).Contains(searchText.ToUpper)
                                                                                     )

            Return results.Where(Function(x) Not escapedItems.Contains(x))
        Else
            Dim results = items.OfType(Of clsEspecies_Buscador)
            Return results.Where(Function(x) Not escapedItems.Contains(x))
        End If
    End Function

End Class

Public Class clsEspecies_Buscador
    Implements INotifyPropertyChanged

    Private _ItemBusqueda As EspeciesAgrupadas
    Public Property ItemBusqueda() As EspeciesAgrupadas
        Get
            Return _ItemBusqueda
        End Get
        Set(ByVal value As EspeciesAgrupadas)
            _ItemBusqueda = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ItemBusqueda"))
        End Set
    End Property

    Private _DescripcionBuscador As String
    Public Property DescripcionBuscador() As String
        Get
            Return _DescripcionBuscador
        End Get
        Set(ByVal value As String)
            _DescripcionBuscador = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescripcionBuscador"))
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
