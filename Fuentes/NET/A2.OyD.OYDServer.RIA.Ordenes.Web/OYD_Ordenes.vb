Imports System.Configuration
Imports System.Data.Linq.Mapping
Imports System.Data.Linq
Imports System.Reflection
Imports System.ComponentModel
Imports System.ServiceModel.DomainServices.Server
Imports A2.OyD.Infraestructura
Imports OpenRiaServices.DomainServices.Server

''' <summary>
''' Esta clase fue creada para poder extender los metodos new() de la clase oyddatacontext y poder descifrar la cadena de conexión.
''' Cuando se realizan cambios en el diseñador de consultas relacional (.dbml) los metodos new() vuelven a ser automáticamente generados y se presenta un error de que 
''' existen múltiples definiciones con firmas idénticas (‘Public Sub New()’ has multiple definitions with identical signatures).
''' LA SOLUCION es eliminar de la clase oyddatacontext todos los métodos New() y dejar los que se encuentran en esta clase.
''' </summary>
''' <remarks></remarks>

Partial Public Class OyDOrdenesDataContext

    Public Sub New()
        MyBase.New(ObtenerCadenaConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)
        OnCreated()
    End Sub
    Public Overrides Sub SubmitChanges(failureMode As Data.Linq.ConflictMode)
        Try
            MyBase.SubmitChanges(failureMode)
        Catch ex As Exception
            ManejarError(ex, "OyDOrdenesDataContext", "SubmitChanges")
        End Try
    End Sub



    <Global.System.Data.Linq.Mapping.FunctionAttribute(Name:="dbo.uspOyDNet_CargarArchivoRecomplementacion")>
    <ResultType(GetType(OyDOrdenes.tblValidacion))>
    <ResultType(GetType(OyDOrdenes.tblDistribucion))>
    Public Function uspOyDNet_CargarArchivoRecomplementacion(<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal pstrRuta As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal pstrNombreArchivo As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Int")> ByVal pintIdOrden As System.Nullable(Of Integer), <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal infosesion As String) As IMultipleResults
        Dim result As IExecuteResult = Me.ExecuteMethodCall(Me, CType(MethodInfo.GetCurrentMethod, MethodInfo), pstrRuta, pstrNombreArchivo, pintIdOrden, infosesion)
        Return CType(result.ReturnValue, IMultipleResults)
    End Function

    <Global.System.Data.Linq.Mapping.FunctionAttribute(Name:="dbo.uspOyDNet_CalcularRecomplementacion")>
    <ResultType(GetType(OyDOrdenes.tblDistribucionCliente))>
    <ResultType(GetType(OyDOrdenes.tblDistribucionPrecio))>
    <ResultType(GetType(OyDOrdenes.tblValidacionProceso))>
    Public Function uspOyDNet_CalcularRecomplementacion(<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal pstrRuta As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal pstrNombreArchivo As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Int")> ByVal pintIdOrden As System.Nullable(Of Integer), <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal infosesion As String) As IMultipleResults
        Dim result As IExecuteResult = Me.ExecuteMethodCall(Me, CType(MethodInfo.GetCurrentMethod, MethodInfo), pstrRuta, pstrNombreArchivo, pintIdOrden, infosesion)
        Return CType(result.ReturnValue, IMultipleResults)
    End Function

    <Global.System.Data.Linq.Mapping.FunctionAttribute(Name:="dbo.uspOyDNet_ActualizarRecomplementacion")>
    Public Function uspOyDNet_ActualizarRecomplementacion(<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal pstrRuta As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal pstrNombreArchivo As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Int")> ByVal pintIdOrden As System.Nullable(Of Integer), <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal infosesion As String) As Integer
        Dim result As IExecuteResult = Me.ExecuteMethodCall(Me, CType(MethodInfo.GetCurrentMethod, MethodInfo), pstrRuta, pstrNombreArchivo, pintIdOrden, infosesion)
        Return CType(result.ReturnValue, Integer)
    End Function

End Class

Namespace OyDOrdenes

    <Global.System.Data.Linq.Mapping.TableAttribute(Name:="dbo.tblResultadoCargaRecomplementacion")>
    Partial Public Class tblResultadoCargaRecomplementacion
        Implements System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged

        Private Shared emptyChangingEventArgs As PropertyChangingEventArgs = New PropertyChangingEventArgs(String.Empty)

        Private _intID As Integer

        Private _tblDistribucions As EntitySet(Of tblDistribucion)

        Private _tblValidacions As EntitySet(Of tblValidacion)

#Region "Extensibility Method Definitions"
        Partial Private Sub OnLoaded()
        End Sub
        Partial Private Sub OnValidate(action As System.Data.Linq.ChangeAction)
        End Sub
        Partial Private Sub OnCreated()
        End Sub
        Partial Private Sub OnintIDChanging(value As Integer)
        End Sub
        Partial Private Sub OnintIDChanged()
        End Sub
#End Region

        Public Sub New()
            MyBase.New()
            Me._tblDistribucions = New EntitySet(Of tblDistribucion)(AddressOf Me.attach_tblDistribucions, AddressOf Me.detach_tblDistribucions)
            Me._tblValidacions = New EntitySet(Of tblValidacion)(AddressOf Me.attach_tblValidacions, AddressOf Me.detach_tblValidacions)
            OnCreated()
        End Sub

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_intID", DbType:="Int NOT NULL", IsPrimaryKey:=True)>
        Public Property intID() As Integer
            Get
                Return Me._intID
            End Get
            Set(value As Integer)
                If ((Me._intID = value) _
                            = False) Then
                    Me.OnintIDChanging(value)
                    Me.SendPropertyChanging()
                    Me._intID = value
                    Me.SendPropertyChanged("intID")
                    Me.OnintIDChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.AssociationAttribute(Name:="tblResultadoCargaRecomplementacion_tblDistribucion", Storage:="_tblDistribucions", ThisKey:="intID", OtherKey:="intIDResultado")>
        <Include()>
        Public Property tblDistribucions() As EntitySet(Of tblDistribucion)
            Get
                Return Me._tblDistribucions
            End Get
            Set(value As EntitySet(Of tblDistribucion))
                Me._tblDistribucions.Assign(value)
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.AssociationAttribute(Name:="tblResultadoCargaRecomplementacion_tblValidacion", Storage:="_tblValidacions", ThisKey:="intID", OtherKey:="intIDResultado")>
        <Include()>
        Public Property tblValidacions() As EntitySet(Of tblValidacion)
            Get
                Return Me._tblValidacions
            End Get
            Set(value As EntitySet(Of tblValidacion))
                Me._tblValidacions.Assign(value)
            End Set
        End Property

        Public Event PropertyChanging As PropertyChangingEventHandler Implements System.ComponentModel.INotifyPropertyChanging.PropertyChanging

        Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

        Protected Overridable Sub SendPropertyChanging()
            If ((Me.PropertyChangingEvent Is Nothing) _
                        = False) Then
                RaiseEvent PropertyChanging(Me, emptyChangingEventArgs)
            End If
        End Sub

        Protected Overridable Sub SendPropertyChanged(ByVal propertyName As [String])
            If ((Me.PropertyChangedEvent Is Nothing) _
                        = False) Then
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
            End If
        End Sub

        Private Sub attach_tblDistribucions(ByVal entity As tblDistribucion)
            Me.SendPropertyChanging()
            entity.tblResultadoCargaRecomplementacion = Me
        End Sub

        Private Sub detach_tblDistribucions(ByVal entity As tblDistribucion)
            Me.SendPropertyChanging()
            entity.tblResultadoCargaRecomplementacion = Nothing
        End Sub

        Private Sub attach_tblValidacions(ByVal entity As tblValidacion)
            Me.SendPropertyChanging()
            entity.tblResultadoCargaRecomplementacion = Me
        End Sub

        Private Sub detach_tblValidacions(ByVal entity As tblValidacion)
            Me.SendPropertyChanging()
            entity.tblResultadoCargaRecomplementacion = Nothing
        End Sub
    End Class

    <Global.System.Data.Linq.Mapping.TableAttribute(Name:="dbo.tblDistribucion")>
    Partial Public Class tblDistribucion
        Implements System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged

        Private Shared emptyChangingEventArgs As PropertyChangingEventArgs = New PropertyChangingEventArgs(String.Empty)

        Private _intID As Integer

        Private _intIDResultado As Integer

        Private _strNroDocumento As String

        Private _intIdOrden As System.Nullable(Of Integer)

        Private _strIdCliente As String

        Private _dblCantidad As Decimal

        Private _dblPorcentajeComision As Decimal

        Private _dblPorcentajeDistribucion As Decimal

        Private _tblResultadoCargaRecomplementacion As EntityRef(Of tblResultadoCargaRecomplementacion)

#Region "Extensibility Method Definitions"
        Partial Private Sub OnLoaded()
        End Sub
        Partial Private Sub OnValidate(action As System.Data.Linq.ChangeAction)
        End Sub
        Partial Private Sub OnCreated()
        End Sub
        Partial Private Sub OnintIDChanging(value As Integer)
        End Sub
        Partial Private Sub OnintIDChanged()
        End Sub
        Partial Private Sub OnintIDResultadoChanging(value As Integer)
        End Sub
        Partial Private Sub OnintIDResultadoChanged()
        End Sub
        Partial Private Sub OnstrNroDocumentoChanging(value As String)
        End Sub
        Partial Private Sub OnstrNroDocumentoChanged()
        End Sub
        Partial Private Sub OnintIdOrdenChanging(value As System.Nullable(Of Integer))
        End Sub
        Partial Private Sub OnintIdOrdenChanged()
        End Sub
        Partial Private Sub OnstrIdClienteChanging(value As String)
        End Sub
        Partial Private Sub OnstrIdClienteChanged()
        End Sub
        Partial Private Sub OndblCantidadChanging(value As Decimal)
        End Sub
        Partial Private Sub OndblCantidadChanged()
        End Sub
        Partial Private Sub OndblPorcentajeComisionChanging(value As Decimal)
        End Sub
        Partial Private Sub OndblPorcentajeComisionChanged()
        End Sub
        Partial Private Sub OndblPorcentajeDistribucionChanging(value As Decimal)
        End Sub
        Partial Private Sub OndblPorcentajeDistribucionChanged()
        End Sub
#End Region

        Public Sub New()
            MyBase.New()
            Me._tblResultadoCargaRecomplementacion = CType(Nothing, EntityRef(Of tblResultadoCargaRecomplementacion))
            OnCreated()
        End Sub

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_intID", DbType:="Int NOT NULL", IsPrimaryKey:=True)>
        Public Property intID() As Integer
            Get
                Return Me._intID
            End Get
            Set(value As Integer)
                If ((Me._intID = value) _
                            = False) Then
                    Me.OnintIDChanging(value)
                    Me.SendPropertyChanging()
                    Me._intID = value
                    Me.SendPropertyChanged("intID")
                    Me.OnintIDChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_intIDResultado", DbType:="Int NOT NULL")>
        Public Property intIDResultado() As Integer
            Get
                Return Me._intIDResultado
            End Get
            Set(value As Integer)
                If ((Me._intIDResultado = value) _
                            = False) Then
                    If Me._tblResultadoCargaRecomplementacion.HasLoadedOrAssignedValue Then
                        Throw New System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException()
                    End If
                    Me.OnintIDResultadoChanging(value)
                    Me.SendPropertyChanging()
                    Me._intIDResultado = value
                    Me.SendPropertyChanged("intIDResultado")
                    Me.OnintIDResultadoChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strNroDocumento", DbType:="VarChar(15)")>
        Public Property strNroDocumento() As String
            Get
                Return Me._strNroDocumento
            End Get
            Set(value As String)
                If (String.Equals(Me._strNroDocumento, value) = False) Then
                    Me.OnstrNroDocumentoChanging(value)
                    Me.SendPropertyChanging()
                    Me._strNroDocumento = value
                    Me.SendPropertyChanged("strNroDocumento")
                    Me.OnstrNroDocumentoChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_intIdOrden", DbType:="Int")>
        Public Property intIdOrden() As System.Nullable(Of Integer)
            Get
                Return Me._intIdOrden
            End Get
            Set(value As System.Nullable(Of Integer))
                If (Me._intIdOrden.Equals(value) = False) Then
                    Me.OnintIdOrdenChanging(value)
                    Me.SendPropertyChanging()
                    Me._intIdOrden = value
                    Me.SendPropertyChanged("intIdOrden")
                    Me.OnintIdOrdenChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strIdCliente", DbType:="VarChar(17)")>
        Public Property strIdCliente() As String
            Get
                Return Me._strIdCliente
            End Get
            Set(value As String)
                If (String.Equals(Me._strIdCliente, value) = False) Then
                    Me.OnstrIdClienteChanging(value)
                    Me.SendPropertyChanging()
                    Me._strIdCliente = value
                    Me.SendPropertyChanged("strIdCliente")
                    Me.OnstrIdClienteChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_dblCantidad", DbType:="Decimal(18,4) NOT NULL")>
        Public Property dblCantidad() As Decimal
            Get
                Return Me._dblCantidad
            End Get
            Set(value As Decimal)
                If ((Me._dblCantidad = value) _
                            = False) Then
                    Me.OndblCantidadChanging(value)
                    Me.SendPropertyChanging()
                    Me._dblCantidad = value
                    Me.SendPropertyChanged("dblCantidad")
                    Me.OndblCantidadChanged()
                End If
            End Set
        End Property



        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_dblPorcentajeComision", DbType:="Decimal(18,4) NOT NULL")>
        Public Property dblPorcentajeComision() As Decimal
            Get
                Return Me._dblPorcentajeComision
            End Get
            Set(value As Decimal)
                If ((Me._dblPorcentajeComision = value) _
                            = False) Then
                    Me.OndblPorcentajeComisionChanging(value)
                    Me.SendPropertyChanging()
                    Me._dblPorcentajeComision = value
                    Me.SendPropertyChanged("dblPorcentajeComision")
                    Me.OndblPorcentajeComisionChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_dblPorcentajeDistribucion", DbType:="Decimal(18,4) NOT NULL")>
        Public Property dblPorcentajeDistribucion() As Decimal
            Get
                Return Me._dblPorcentajeDistribucion
            End Get
            Set(value As Decimal)
                If ((Me._dblPorcentajeDistribucion = value) _
                            = False) Then
                    Me.OndblPorcentajeDistribucionChanging(value)
                    Me.SendPropertyChanging()
                    Me._dblPorcentajeDistribucion = value
                    Me.SendPropertyChanged("dblPorcentajeDistribucion")
                    Me.OndblPorcentajeDistribucionChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.AssociationAttribute(Name:="tblResultadoCargaRecomplementacion_tblDistribucion", Storage:="_tblResultadoCargaRecomplementacion", ThisKey:="intIDResultado", OtherKey:="intID", IsForeignKey:=True)>
        Public Property tblResultadoCargaRecomplementacion() As tblResultadoCargaRecomplementacion
            Get
                Return Me._tblResultadoCargaRecomplementacion.Entity
            End Get
            Set(value As tblResultadoCargaRecomplementacion)
                Dim previousValue As tblResultadoCargaRecomplementacion = Me._tblResultadoCargaRecomplementacion.Entity
                If ((Object.Equals(previousValue, value) = False) _
                            OrElse (Me._tblResultadoCargaRecomplementacion.HasLoadedOrAssignedValue = False)) Then
                    Me.SendPropertyChanging()
                    If ((previousValue Is Nothing) _
                                = False) Then
                        Me._tblResultadoCargaRecomplementacion.Entity = Nothing
                        previousValue.tblDistribucions.Remove(Me)
                    End If
                    Me._tblResultadoCargaRecomplementacion.Entity = value
                    If ((value Is Nothing) _
                                = False) Then
                        value.tblDistribucions.Add(Me)
                        Me._intIDResultado = value.intID
                    Else
                        Me._intIDResultado = CType(Nothing, Integer)
                    End If
                    Me.SendPropertyChanged("tblResultadoCargaRecomplementacion")
                End If
            End Set
        End Property

        Public Event PropertyChanging As PropertyChangingEventHandler Implements System.ComponentModel.INotifyPropertyChanging.PropertyChanging

        Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

        Protected Overridable Sub SendPropertyChanging()
            If ((Me.PropertyChangingEvent Is Nothing) _
                        = False) Then
                RaiseEvent PropertyChanging(Me, emptyChangingEventArgs)
            End If
        End Sub

        Protected Overridable Sub SendPropertyChanged(ByVal propertyName As [String])
            If ((Me.PropertyChangedEvent Is Nothing) _
                        = False) Then
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class

    <Global.System.Data.Linq.Mapping.TableAttribute(Name:="dbo.tblValidacion")>
    Partial Public Class tblValidacion
        Implements System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged

        Private Shared emptyChangingEventArgs As PropertyChangingEventArgs = New PropertyChangingEventArgs(String.Empty)

        Private _intID As Integer

        Private _intIDResultado As Integer

        Private _strMensaje As String

        Private _logExitoso As System.Nullable(Of Boolean)

        Private _strTipo As String

        Private _intFila As System.Nullable(Of Integer)

        Private _intColumna As System.Nullable(Of Integer)

        Private _tblResultadoCargaRecomplementacion As EntityRef(Of tblResultadoCargaRecomplementacion)

#Region "Extensibility Method Definitions"
        Partial Private Sub OnLoaded()
        End Sub
        Partial Private Sub OnValidate(action As System.Data.Linq.ChangeAction)
        End Sub
        Partial Private Sub OnCreated()
        End Sub
        Partial Private Sub OnintIDChanging(value As Integer)
        End Sub
        Partial Private Sub OnintIDChanged()
        End Sub
        Partial Private Sub OnintIDResultadoChanging(value As Integer)
        End Sub
        Partial Private Sub OnintIDResultadoChanged()
        End Sub
        Partial Private Sub OnstrMensajeChanging(value As String)
        End Sub
        Partial Private Sub OnstrMensajeChanged()
        End Sub
        Partial Private Sub OnlogExitosoChanging(value As System.Nullable(Of Boolean))
        End Sub
        Partial Private Sub OnlogExitosoChanged()
        End Sub
        Partial Private Sub OnstrTipoChanging(value As String)
        End Sub
        Partial Private Sub OnstrTipoChanged()
        End Sub
        Partial Private Sub OnintFilaChanging(value As System.Nullable(Of Integer))
        End Sub
        Partial Private Sub OnintFilaChanged()
        End Sub
        Partial Private Sub OnintColumnaChanging(value As System.Nullable(Of Integer))
        End Sub
        Partial Private Sub OnintColumnaChanged()
        End Sub
#End Region

        Public Sub New()
            MyBase.New()
            Me._tblResultadoCargaRecomplementacion = CType(Nothing, EntityRef(Of tblResultadoCargaRecomplementacion))
            OnCreated()
        End Sub

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_intID", AutoSync:=AutoSync.OnInsert, DbType:="Int NOT NULL IDENTITY", IsPrimaryKey:=True, IsDbGenerated:=True)>
        Public Property intID() As Integer
            Get
                Return Me._intID
            End Get
            Set(value As Integer)
                If ((Me._intID = value) _
                            = False) Then
                    Me.OnintIDChanging(value)
                    Me.SendPropertyChanging()
                    Me._intID = value
                    Me.SendPropertyChanged("intID")
                    Me.OnintIDChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_intIDResultado", DbType:="Int NOT NULL")>
        Public Property intIDResultado() As Integer
            Get
                Return Me._intIDResultado
            End Get
            Set(value As Integer)
                If ((Me._intIDResultado = value) _
                            = False) Then
                    If Me._tblResultadoCargaRecomplementacion.HasLoadedOrAssignedValue Then
                        Throw New System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException()
                    End If
                    Me.OnintIDResultadoChanging(value)
                    Me.SendPropertyChanging()
                    Me._intIDResultado = value
                    Me.SendPropertyChanged("intIDResultado")
                    Me.OnintIDResultadoChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strMensaje", DbType:="VarChar(5000)")>
        Public Property strMensaje() As String
            Get
                Return Me._strMensaje
            End Get
            Set(value As String)
                If (String.Equals(Me._strMensaje, value) = False) Then
                    Me.OnstrMensajeChanging(value)
                    Me.SendPropertyChanging()
                    Me._strMensaje = value
                    Me.SendPropertyChanged("strMensaje")
                    Me.OnstrMensajeChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_logExitoso", DbType:="Bit")>
        Public Property logExitoso() As System.Nullable(Of Boolean)
            Get
                Return Me._logExitoso
            End Get
            Set(value As System.Nullable(Of Boolean))
                If (Me._logExitoso.Equals(value) = False) Then
                    Me.OnlogExitosoChanging(value)
                    Me.SendPropertyChanging()
                    Me._logExitoso = value
                    Me.SendPropertyChanged("logExitoso")
                    Me.OnlogExitosoChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strTipo", DbType:="VarChar(100)")>
        Public Property strTipo() As String
            Get
                Return Me._strTipo
            End Get
            Set(value As String)
                If (String.Equals(Me._strTipo, value) = False) Then
                    Me.OnstrTipoChanging(value)
                    Me.SendPropertyChanging()
                    Me._strTipo = value
                    Me.SendPropertyChanged("strTipo")
                    Me.OnstrTipoChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_intFila", DbType:="Int")>
        Public Property intFila() As System.Nullable(Of Integer)
            Get
                Return Me._intFila
            End Get
            Set(value As System.Nullable(Of Integer))
                If (Me._intFila.Equals(value) = False) Then
                    Me.OnintFilaChanging(value)
                    Me.SendPropertyChanging()
                    Me._intFila = value
                    Me.SendPropertyChanged("intFila")
                    Me.OnintFilaChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_intColumna", DbType:="Int")>
        Public Property intColumna() As System.Nullable(Of Integer)
            Get
                Return Me._intColumna
            End Get
            Set(value As System.Nullable(Of Integer))
                If (Me._intColumna.Equals(value) = False) Then
                    Me.OnintColumnaChanging(value)
                    Me.SendPropertyChanging()
                    Me._intColumna = value
                    Me.SendPropertyChanged("intColumna")
                    Me.OnintColumnaChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.AssociationAttribute(Name:="tblResultadoCargaRecomplementacion_tblValidacion", Storage:="_tblResultadoCargaRecomplementacion", ThisKey:="intIDResultado", OtherKey:="intID", IsForeignKey:=True)>
        Public Property tblResultadoCargaRecomplementacion() As tblResultadoCargaRecomplementacion
            Get
                Return Me._tblResultadoCargaRecomplementacion.Entity
            End Get
            Set(value As tblResultadoCargaRecomplementacion)
                Dim previousValue As tblResultadoCargaRecomplementacion = Me._tblResultadoCargaRecomplementacion.Entity
                If ((Object.Equals(previousValue, value) = False) _
                            OrElse (Me._tblResultadoCargaRecomplementacion.HasLoadedOrAssignedValue = False)) Then
                    Me.SendPropertyChanging()
                    If ((previousValue Is Nothing) _
                                = False) Then
                        Me._tblResultadoCargaRecomplementacion.Entity = Nothing
                        previousValue.tblValidacions.Remove(Me)
                    End If
                    Me._tblResultadoCargaRecomplementacion.Entity = value
                    If ((value Is Nothing) _
                                = False) Then
                        value.tblValidacions.Add(Me)
                        Me._intIDResultado = value.intID
                    Else
                        Me._intIDResultado = CType(Nothing, Integer)
                    End If
                    Me.SendPropertyChanged("tblResultadoCargaRecomplementacion")
                End If
            End Set
        End Property

        Public Event PropertyChanging As PropertyChangingEventHandler Implements System.ComponentModel.INotifyPropertyChanging.PropertyChanging

        Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

        Protected Overridable Sub SendPropertyChanging()
            If ((Me.PropertyChangingEvent Is Nothing) _
                        = False) Then
                RaiseEvent PropertyChanging(Me, emptyChangingEventArgs)
            End If
        End Sub

        Protected Overridable Sub SendPropertyChanged(ByVal propertyName As [String])
            If ((Me.PropertyChangedEvent Is Nothing) _
                        = False) Then
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class

    <Global.System.Data.Linq.Mapping.TableAttribute(Name:="dbo.tblResultadoCalculoRecomplementacion")>
    Partial Public Class tblResultadoCalculoRecomplementacion
        Implements System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged

        Private Shared emptyChangingEventArgs As PropertyChangingEventArgs = New PropertyChangingEventArgs(String.Empty)

        Private _intID As Integer

        Private _tblDistribucionClientes As EntitySet(Of tblDistribucionCliente)

        Private _tblDistribucionPrecios As EntitySet(Of tblDistribucionPrecio)

        Private _tblValidacionProcesos As EntitySet(Of tblValidacionProceso)

#Region "Extensibility Method Definitions"
        Partial Private Sub OnLoaded()
        End Sub
        Partial Private Sub OnValidate(action As System.Data.Linq.ChangeAction)
        End Sub
        Partial Private Sub OnCreated()
        End Sub
        Partial Private Sub OnintIDChanging(value As Integer)
        End Sub
        Partial Private Sub OnintIDChanged()
        End Sub
#End Region

        Public Sub New()
            MyBase.New()
            Me._tblDistribucionClientes = New EntitySet(Of tblDistribucionCliente)(AddressOf Me.attach_tblDistribucionClientes, AddressOf Me.detach_tblDistribucionClientes)
            Me._tblDistribucionPrecios = New EntitySet(Of tblDistribucionPrecio)(AddressOf Me.attach_tblDistribucionPrecios, AddressOf Me.detach_tblDistribucionPrecios)
            Me._tblValidacionProcesos = New EntitySet(Of tblValidacionProceso)(AddressOf Me.attach_tblValidacionProcesos, AddressOf Me.detach_tblValidacionProcesos)
            OnCreated()
        End Sub

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_intID", DbType:="Int NOT NULL", IsPrimaryKey:=True)>
        Public Property intID() As Integer
            Get
                Return Me._intID
            End Get
            Set(value As Integer)
                If ((Me._intID = value) _
                            = False) Then
                    Me.OnintIDChanging(value)
                    Me.SendPropertyChanging()
                    Me._intID = value
                    Me.SendPropertyChanged("intID")
                    Me.OnintIDChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.AssociationAttribute(Name:="tblResultadoCalculoRecomplementacion_tblDistribucionCliente", Storage:="_tblDistribucionClientes", ThisKey:="intID", OtherKey:="intIDResultado")>
        <Include()>
        Public Property tblDistribucionClientes() As EntitySet(Of tblDistribucionCliente)
            Get
                Return Me._tblDistribucionClientes
            End Get
            Set(value As EntitySet(Of tblDistribucionCliente))
                Me._tblDistribucionClientes.Assign(value)
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.AssociationAttribute(Name:="tblResultadoCalculoRecomplementacion_tblDistribucionPrecio", Storage:="_tblDistribucionPrecios", ThisKey:="intID", OtherKey:="intIDResultado")>
        <Include()>
        Public Property tblDistribucionPrecios() As EntitySet(Of tblDistribucionPrecio)
            Get
                Return Me._tblDistribucionPrecios
            End Get
            Set(value As EntitySet(Of tblDistribucionPrecio))
                Me._tblDistribucionPrecios.Assign(value)
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.AssociationAttribute(Name:="tblResultadoCalculoRecomplementacion_tblValidacionProceso", Storage:="_tblValidacionProcesos", ThisKey:="intID", OtherKey:="intIDResultado")>
        <Include()>
        Public Property tblValidacionProcesos() As EntitySet(Of tblValidacionProceso)
            Get
                Return Me._tblValidacionProcesos
            End Get
            Set(value As EntitySet(Of tblValidacionProceso))
                Me._tblValidacionProcesos.Assign(value)
            End Set
        End Property

        Public Event PropertyChanging As PropertyChangingEventHandler Implements System.ComponentModel.INotifyPropertyChanging.PropertyChanging

        Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

        Protected Overridable Sub SendPropertyChanging()
            If ((Me.PropertyChangingEvent Is Nothing) _
                        = False) Then
                RaiseEvent PropertyChanging(Me, emptyChangingEventArgs)
            End If
        End Sub

        Protected Overridable Sub SendPropertyChanged(ByVal propertyName As [String])
            If ((Me.PropertyChangedEvent Is Nothing) _
                        = False) Then
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
            End If
        End Sub

        Private Sub attach_tblDistribucionClientes(ByVal entity As tblDistribucionCliente)
            Me.SendPropertyChanging()
            entity.tblResultadoCalculoRecomplementacion = Me
        End Sub

        Private Sub detach_tblDistribucionClientes(ByVal entity As tblDistribucionCliente)
            Me.SendPropertyChanging()
            entity.tblResultadoCalculoRecomplementacion = Nothing
        End Sub

        Private Sub attach_tblDistribucionPrecios(ByVal entity As tblDistribucionPrecio)
            Me.SendPropertyChanging()
            entity.tblResultadoCalculoRecomplementacion = Me
        End Sub

        Private Sub detach_tblDistribucionPrecios(ByVal entity As tblDistribucionPrecio)
            Me.SendPropertyChanging()
            entity.tblResultadoCalculoRecomplementacion = Nothing
        End Sub

        Private Sub attach_tblValidacionProcesos(ByVal entity As tblValidacionProceso)
            Me.SendPropertyChanging()
            entity.tblResultadoCalculoRecomplementacion = Me
        End Sub

        Private Sub detach_tblValidacionProcesos(ByVal entity As tblValidacionProceso)
            Me.SendPropertyChanging()
            entity.tblResultadoCalculoRecomplementacion = Nothing
        End Sub
    End Class

    <Global.System.Data.Linq.Mapping.TableAttribute(Name:="dbo.tblDistribucionCliente")>
    Partial Public Class tblDistribucionCliente
        Implements System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged

        Private Shared emptyChangingEventArgs As PropertyChangingEventArgs = New PropertyChangingEventArgs(String.Empty)

        Private _intID As Integer

        Private _intIDResultado As Integer

        Private _strResultado As String

        Private _tblResultadoCalculoRecomplementacion As EntityRef(Of tblResultadoCalculoRecomplementacion)

#Region "Extensibility Method Definitions"
        Partial Private Sub OnLoaded()
        End Sub
        Partial Private Sub OnValidate(action As System.Data.Linq.ChangeAction)
        End Sub
        Partial Private Sub OnCreated()
        End Sub
        Partial Private Sub OnintIDChanging(value As Integer)
        End Sub
        Partial Private Sub OnintIDChanged()
        End Sub
        Partial Private Sub OnintIDResultadoChanging(value As Integer)
        End Sub
        Partial Private Sub OnintIDResultadoChanged()
        End Sub
        Partial Private Sub OnstrResultadoChanging(value As String)
        End Sub
        Partial Private Sub OnstrResultadoChanged()
        End Sub
#End Region

        Public Sub New()
            MyBase.New()
            Me._tblResultadoCalculoRecomplementacion = CType(Nothing, EntityRef(Of tblResultadoCalculoRecomplementacion))
            OnCreated()
        End Sub

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_intID", DbType:="Int NOT NULL", IsPrimaryKey:=True)>
        Public Property intID() As Integer
            Get
                Return Me._intID
            End Get
            Set(value As Integer)
                If ((Me._intID = value) _
                            = False) Then
                    Me.OnintIDChanging(value)
                    Me.SendPropertyChanging()
                    Me._intID = value
                    Me.SendPropertyChanged("intID")
                    Me.OnintIDChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_intIDResultado", DbType:="Int NOT NULL")>
        Public Property intIDResultado() As Integer
            Get
                Return Me._intIDResultado
            End Get
            Set(value As Integer)
                If ((Me._intIDResultado = value) _
                            = False) Then
                    If Me._tblResultadoCalculoRecomplementacion.HasLoadedOrAssignedValue Then
                        Throw New System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException()
                    End If
                    Me.OnintIDResultadoChanging(value)
                    Me.SendPropertyChanging()
                    Me._intIDResultado = value
                    Me.SendPropertyChanged("intIDResultado")
                    Me.OnintIDResultadoChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strResultado", DbType:="VarChar(MAX) NOT NULL", CanBeNull:=False)>
        Public Property strResultado() As String
            Get
                Return Me._strResultado
            End Get
            Set(value As String)
                If (String.Equals(Me._strResultado, value) = False) Then
                    Me.OnstrResultadoChanging(value)
                    Me.SendPropertyChanging()
                    Me._strResultado = value
                    Me.SendPropertyChanged("strResultado")
                    Me.OnstrResultadoChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.AssociationAttribute(Name:="tblResultadoCalculoRecomplementacion_tblDistribucionCliente", Storage:="_tblResultadoCalculoRecomplementacion", ThisKey:="intIDResultado", OtherKey:="intID", IsForeignKey:=True)>
        Public Property tblResultadoCalculoRecomplementacion() As tblResultadoCalculoRecomplementacion
            Get
                Return Me._tblResultadoCalculoRecomplementacion.Entity
            End Get
            Set(value As tblResultadoCalculoRecomplementacion)
                Dim previousValue As tblResultadoCalculoRecomplementacion = Me._tblResultadoCalculoRecomplementacion.Entity
                If ((Object.Equals(previousValue, value) = False) _
                            OrElse (Me._tblResultadoCalculoRecomplementacion.HasLoadedOrAssignedValue = False)) Then
                    Me.SendPropertyChanging()
                    If ((previousValue Is Nothing) _
                                = False) Then
                        Me._tblResultadoCalculoRecomplementacion.Entity = Nothing
                        previousValue.tblDistribucionClientes.Remove(Me)
                    End If
                    Me._tblResultadoCalculoRecomplementacion.Entity = value
                    If ((value Is Nothing) _
                                = False) Then
                        value.tblDistribucionClientes.Add(Me)
                        Me._intIDResultado = value.intID
                    Else
                        Me._intIDResultado = CType(Nothing, Integer)
                    End If
                    Me.SendPropertyChanged("tblResultadoCalculoRecomplementacion")
                End If
            End Set
        End Property

        Public Event PropertyChanging As PropertyChangingEventHandler Implements System.ComponentModel.INotifyPropertyChanging.PropertyChanging

        Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

        Protected Overridable Sub SendPropertyChanging()
            If ((Me.PropertyChangingEvent Is Nothing) _
                        = False) Then
                RaiseEvent PropertyChanging(Me, emptyChangingEventArgs)
            End If
        End Sub

        Protected Overridable Sub SendPropertyChanged(ByVal propertyName As [String])
            If ((Me.PropertyChangedEvent Is Nothing) _
                        = False) Then
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class

    <Global.System.Data.Linq.Mapping.TableAttribute(Name:="dbo.tblDistribucionPrecio")>
    Partial Public Class tblDistribucionPrecio
        Implements System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged

        Private Shared emptyChangingEventArgs As PropertyChangingEventArgs = New PropertyChangingEventArgs(String.Empty)

        Private _intID As Integer

        Private _intIDResultado As Integer

        Private _strResultado As String

        Private _tblResultadoCalculoRecomplementacion As EntityRef(Of tblResultadoCalculoRecomplementacion)

#Region "Extensibility Method Definitions"
        Partial Private Sub OnLoaded()
        End Sub
        Partial Private Sub OnValidate(action As System.Data.Linq.ChangeAction)
        End Sub
        Partial Private Sub OnCreated()
        End Sub
        Partial Private Sub OnintIDChanging(value As Integer)
        End Sub
        Partial Private Sub OnintIDChanged()
        End Sub
        Partial Private Sub OnintIDResultadoChanging(value As Integer)
        End Sub
        Partial Private Sub OnintIDResultadoChanged()
        End Sub
        Partial Private Sub OnstrResultadoChanging(value As String)
        End Sub
        Partial Private Sub OnstrResultadoChanged()
        End Sub
#End Region

        Public Sub New()
            MyBase.New()
            Me._tblResultadoCalculoRecomplementacion = CType(Nothing, EntityRef(Of tblResultadoCalculoRecomplementacion))
            OnCreated()
        End Sub

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_intID", DbType:="Int NOT NULL", IsPrimaryKey:=True)>
        Public Property intID() As Integer
            Get
                Return Me._intID
            End Get
            Set(value As Integer)
                If ((Me._intID = value) _
                            = False) Then
                    Me.OnintIDChanging(value)
                    Me.SendPropertyChanging()
                    Me._intID = value
                    Me.SendPropertyChanged("intID")
                    Me.OnintIDChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_intIDResultado", DbType:="Int NOT NULL")>
        Public Property intIDResultado() As Integer
            Get
                Return Me._intIDResultado
            End Get
            Set(value As Integer)
                If ((Me._intIDResultado = value) _
                            = False) Then
                    If Me._tblResultadoCalculoRecomplementacion.HasLoadedOrAssignedValue Then
                        Throw New System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException()
                    End If
                    Me.OnintIDResultadoChanging(value)
                    Me.SendPropertyChanging()
                    Me._intIDResultado = value
                    Me.SendPropertyChanged("intIDResultado")
                    Me.OnintIDResultadoChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strResultado", DbType:="VarChar(MAX) NOT NULL", CanBeNull:=False)>
        Public Property strResultado() As String
            Get
                Return Me._strResultado
            End Get
            Set(value As String)
                If (String.Equals(Me._strResultado, value) = False) Then
                    Me.OnstrResultadoChanging(value)
                    Me.SendPropertyChanging()
                    Me._strResultado = value
                    Me.SendPropertyChanged("strResultado")
                    Me.OnstrResultadoChanged()
                End If
            End Set
        End Property

        <Global.System.Data.Linq.Mapping.AssociationAttribute(Name:="tblResultadoCalculoRecomplementacion_tblDistribucionPrecio", Storage:="_tblResultadoCalculoRecomplementacion", ThisKey:="intIDResultado", OtherKey:="intID", IsForeignKey:=True)>
        Public Property tblResultadoCalculoRecomplementacion() As tblResultadoCalculoRecomplementacion
            Get
                Return Me._tblResultadoCalculoRecomplementacion.Entity
            End Get
            Set(value As tblResultadoCalculoRecomplementacion)
                Dim previousValue As tblResultadoCalculoRecomplementacion = Me._tblResultadoCalculoRecomplementacion.Entity
                If ((Object.Equals(previousValue, value) = False) _
                            OrElse (Me._tblResultadoCalculoRecomplementacion.HasLoadedOrAssignedValue = False)) Then
                    Me.SendPropertyChanging()
                    If ((previousValue Is Nothing) _
                                = False) Then
                        Me._tblResultadoCalculoRecomplementacion.Entity = Nothing
                        previousValue.tblDistribucionPrecios.Remove(Me)
                    End If
                    Me._tblResultadoCalculoRecomplementacion.Entity = value
                    If ((value Is Nothing) _
                                = False) Then
                        value.tblDistribucionPrecios.Add(Me)
                        Me._intIDResultado = value.intID
                    Else
                        Me._intIDResultado = CType(Nothing, Integer)
                    End If
                    Me.SendPropertyChanged("tblResultadoCalculoRecomplementacion")
                End If
            End Set
        End Property

        Public Event PropertyChanging As PropertyChangingEventHandler Implements System.ComponentModel.INotifyPropertyChanging.PropertyChanging

        Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

        Protected Overridable Sub SendPropertyChanging()
            If ((Me.PropertyChangingEvent Is Nothing) _
                        = False) Then
                RaiseEvent PropertyChanging(Me, emptyChangingEventArgs)
            End If
        End Sub

        Protected Overridable Sub SendPropertyChanged(ByVal propertyName As [String])
            If ((Me.PropertyChangedEvent Is Nothing) _
                        = False) Then
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class



End Namespace


