Imports System.Data.Linq
Imports System.Reflection
Imports A2.OyD.OYDServer.RIA.Web.OyDClientes
Imports A2.OyD.Infraestructura

Partial Public Class OyDClientesDataContext
    Dim idcomitente As String
    Dim iddetallecomitente As String
    Public Sub New()
        MyBase.New(ObtenerCadenaConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)
        OnCreated()
    End Sub
    Public Overrides Sub SubmitChanges(failureMode As Data.Linq.ConflictMode)
        Try
            MyBase.SubmitChanges(failureMode)
        Catch ex As Exception
            ManejarError(ex, "OyDClientesDataContext", "SubmitChanges")
        End Try
    End Sub

    Private Sub InsertCliente(ByVal obj As OyDClientes.Cliente)

        ' Esta funcion fue creada para poder extender los metodos insercion de clientes y los detalles de clientes la clase oyddatacontext .
        ' Cuando se realizan cambios en el diseñador de consultas relacional (.dbml) las funciones InsertClientes y InsertCuentasCliente,InsertClientesReceptore,InsertClientesOrdenante,InsertClientesBeneficiarios,InsertClientesAficiones,
        ' InsertClientesDeportes,InsertClientesDireccione,InsertClientesDocumentosRequeridos,InsertClientesLOGHistoriaI,InsertClientesConocimientoEspecifico vuelven a ser automáticamente generados y se presenta un error de que 
        ' existen múltiples definiciones de la funcion 'has multiple definitions with identical signatures').
        ' LA SOLUCION es eliminar de la clase oyddatacontext estas dos funciones y dejar los que se encuentran en esta clase.
        Dim p1 As String = obj.IDComitente
        Dim p2 As Integer = obj.IDClientes
        Me.uspOyDNet_Clientes_Cliente_Actualizar(obj.Aprobacion, p1, obj.IDSucCliente, obj.TipoIdentificacion, CType(obj.NroDocumento, System.Nullable(Of Decimal)), obj.strNroDocumento, obj.IDPoblacionDoc, obj.IDDepartamentoDoc, obj.IDPaisDoc, obj.IDNacionalidad, obj.Nombre, obj.TipoVinculacion, CType(obj.Ingreso, System.Nullable(Of Date)), obj.Telefono1, obj.Telefono2, obj.Fax1, obj.Fax2, obj.Direccion, obj.Internet, obj.IDPoblacion, obj.IDDepartamento, obj.IdPais, CType(obj.MenorEdad, System.Nullable(Of Boolean)), obj.IDGrupo, obj.IDSubGrupo, obj.IdPatrimonio, CType(obj.OrigenPatrimonio, System.Nullable(Of Boolean)), obj.IdProfesion, CType(obj.RetFuente, System.Nullable(Of Boolean)), CType(obj.IngresoMensual, System.Nullable(Of Double)), CType(obj.Activo, System.Nullable(Of Boolean)), obj.IDConcepto, CType(obj.Concepto, System.Nullable(Of Date)), obj.Notas, CType(obj.SuperValores, System.Nullable(Of Date)), obj.Admonvalores, CType(obj.ContratoAV, System.Nullable(Of Date)), obj.NotasAdmon, obj.Tercero, CType(obj.Contribuyente, System.Nullable(Of Boolean)), obj.ActividadEconomica, CType(obj.Excluido, System.Nullable(Of Boolean)), CType(obj.Activos, System.Nullable(Of Double)), CType(obj.Patrimonio, System.Nullable(Of Double)), CType(obj.Utilidades, System.Nullable(Of Double)), CType(obj.FactorComisionCliente, System.Nullable(Of Double)), obj.IDSector, obj.IDSubSector, obj.DireccionEnvio, obj.EntregarA, obj.EMail, CType(obj.Nacimiento, System.Nullable(Of Date)), obj.EstadoCivil, obj.Sexo, obj.Ocupacion, obj.Cargo, obj.Empresa, obj.Secretaria, obj.RepresentanteLegal, obj.TipoReprLegal, obj.IDReprLegal, obj.CargoReprLegal, obj.TelefonoReprLegal, obj.DireccionReprLegal, obj.IDCiudadReprLegal, obj.ClaveInternet, CType(obj.ActivoRemoto, System.Nullable(Of Boolean)), obj.TipoReferencia, obj.RazonVinculacion, obj.RecomendadoPor, CType(obj.ActualizacionFicha, System.Nullable(Of Date)), obj.FormaPago, obj.IdCuentaCtble, obj.TituloCliente, obj.NoLlamarAlCliente, obj.EnviarInformeEconomico, obj.EnviarPortafolio, obj.Estrato, obj.NumeroHijos, CType(obj.NacimientoRepresentanteLegal, System.Nullable(Of Date)), obj.OrigenIngresos, obj.OrigenFondo, obj.DireccionOficina, obj.TelefonoOficina, obj.FaxOficina, obj.ApartadoAereo, obj.EntregaCorrespondencia, obj.TipoSegmento, CType(obj.Egresos, System.Nullable(Of Double)), obj.DetalleOIngresos, obj.codigoCiiu, CType(obj.OpMonedaExtranjera, System.Nullable(Of Boolean)), obj.TipoOperacion, obj.CualOtroTipoOper, obj.NroCuentaExt, obj.BancoExt, obj.Moneda, obj.CiudadExt, obj.PaisExt, obj.LugarEntrevista, obj.ObservacionEnt, CType(obj.Entrevista, System.Nullable(Of Date)), obj.ReceptorEntrevista, obj.TelefonoResidencia, obj.direccionResidencia, obj.RetornoValorSemestral, CType(obj.FiguraPublica, System.Nullable(Of Boolean)), CType(obj.PersonaAltoRiesgo, System.Nullable(Of Boolean)), obj.Suitability, obj.Apellido1, obj.Apellido2, obj.Nombre1, obj.Nombre2, obj.MovimientoEspecial, obj.Usuario, obj.TIER, obj.IndicadorAsalariado, obj.Clasificacion, CType(obj.ValorActivoLiquido, System.Nullable(Of Decimal)), CType(obj.SinAnimoDeLucro, System.Nullable(Of Boolean)), obj.ObjetivoInversion, obj.HorizonteInversion, obj.EdadCliente, obj.ConocimientoExperiencia, obj.ClasificacionInversionista, obj.Superintendencia, obj.IDSucursal, obj.EstadoCliente, obj.CtaMultipdto, obj.Periodicidad, obj.PerfilRiesgo, obj.TipoProducto, obj.TipoCliente, obj.Perfil, obj.Exceptuado, obj.CategoriaCliente, CType(obj.ComisionAcciones, System.Nullable(Of Decimal)), CType(obj.Apt, System.Nullable(Of Boolean)), obj.ClasificacionDocumento, obj.PrioridadPortafolio, obj.Referencia, obj.TipoMensajeria, CType(obj.FondoExtranjero, System.Nullable(Of Boolean)), CType(obj.ReplicarSafyrFondos, System.Nullable(Of Boolean)), CType(obj.ReplicarSafyrPortafolios, System.Nullable(Of Boolean)), CType(obj.ReplicarSafyrClientes, System.Nullable(Of Boolean)), CType(obj.ReplicarMercansoft, System.Nullable(Of Boolean)), CType(obj.Declarante, System.Nullable(Of Boolean)), CType(obj.AutoRetenedor, System.Nullable(Of Boolean)), CType(obj.ExentoGMF, System.Nullable(Of Boolean)), obj.IdCiudadNacimiento, CType(obj.FechaExpedicionDoc, System.Nullable(Of Date)), obj.IndicativoTelefono, CType(obj.AceptaCruces, System.Nullable(Of Boolean)), obj.TipoIntermediario, obj.CodReceptorSafyrFondos, obj.CodReceptorSafyrPortafolio, obj.CodReceptorSafyrClientes, obj.CodReceptorMercansoft, obj.TipoIdentificacionRB, obj.NroDocumentoRB, obj.NombreRB, CType(obj.Embajada, System.Nullable(Of Boolean)), CType(obj.ReplicarSantanderAgora, System.Nullable(Of Boolean)), CType(obj.RETEICA, System.Nullable(Of Boolean)), CType(obj.Comision, System.Nullable(Of Decimal)), CType(obj.PorcentajeComision, System.Nullable(Of Decimal)), obj.DescripcionCuenta, CType(obj.Pasivos, System.Nullable(Of Double)), obj.EMailReciboInstruccion, obj.NroSalarios, obj.PerfilSarlaft, CType(obj.DepEconomica, System.Nullable(Of Boolean)), obj.SegmentoComercial, obj.TipoCiudadano, CType(obj.AplicaFatca, System.Nullable(Of Boolean)), obj.TipoCiudadanoReprLegal, CType(obj.AplicaFatcaReprLegal, System.Nullable(Of Boolean)), obj.IDNacionalidadReprLegal, p2, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)), obj.UsuarioAprobador, CType(obj.CREE, System.Nullable(Of Boolean)), obj.strTipoClienteClasificacion, obj.strPerteneceGrupoEconomico, obj.strCalificacionRiesgoSuperior, obj.IDCustodio, CType(obj.CiudadanoResidenteDomicilio, System.Nullable(Of Boolean)), CType(obj.TransfiereACuentasEEUU, System.Nullable(Of Boolean)), obj.TitularCuentaEEUU, obj.EntidadTransferencia, CType(obj.EmpresaListadaBolsa, System.Nullable(Of Boolean)), obj.MercadoNegociaAcciones, CType(obj.SubsidiariaDeEntidad, System.Nullable(Of Boolean)), obj.EmpresaMatriz, obj.Regulador, CType(obj.InstitucionFinanciera, System.Nullable(Of Boolean)), CType(obj.AccionistaContribuyenteEEUU, System.Nullable(Of Boolean)), CType(obj.SinAnimoDeLucroFatca, System.Nullable(Of Boolean)), obj.GIIN, CType(obj.PrestadorServExcIVA, System.Nullable(Of Boolean)), CType(obj.EnvioFisico, System.Nullable(Of Boolean)), CType(obj.logSuperIntendencia, System.Nullable(Of Boolean)), CType(obj.PoderParaFirmar, System.Nullable(Of Boolean)), obj.AtencionOperativa, CType(obj.ClienteAutorizacion, System.Nullable(Of Boolean)), obj.codEmpresaExtranjera, obj.IDPaisExtranjera, CType(obj.AdmonInvExterior, System.Nullable(Of Boolean)), obj.FormaPago, CType(obj.AutorizaTratamiento, System.Nullable(Of Boolean)), CType(obj.TIN, String), CType(obj.NacimientoRepresentanteLegalFATCA, System.Nullable(Of Date)), CType(obj.AdministradoBanco, System.Nullable(Of Boolean)), obj.strInstruccionCRCC, obj.strNroRim)
        obj.IDComitente = p1
        obj.IDClientes = p2
        idcomitente = p1
    End Sub

    Private Sub InsertCuentasCliente(ByVal obj As OyDClientes.CuentasCliente)
        Dim p1 As Integer = obj.IDCuentasclientes
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.IDComitente
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_CuentasClientes_Actualizar(obj.Aprobacion, iddetallecomitente, obj.IDBanco, obj.NombreBanco, obj.NombreSucursal, obj.Cuenta, obj.TipoCuenta, obj.CodigoACH, obj.Titular, obj.TipoID, obj.NumeroID, CType(obj.TransferirA, System.Nullable(Of Boolean)), CType(obj.Referencia, System.Nullable(Of Boolean)), CType(obj.Dividendos, System.Nullable(Of Boolean)), CType(obj.Operaciones, System.Nullable(Of Boolean)), obj.IdConsecutivoSafyr, obj.IdConsecutivoClientes, obj.IdConsecutivoPortafolios, obj.Observacion, CType(obj.Activo, System.Nullable(Of Boolean)), CType(obj.logTitular, System.Nullable(Of Boolean)), CType(obj.logexcluirInteresDividendo, System.Nullable(Of Boolean)), obj.Usuario, p1, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)), CType(obj.UnicoTitular, System.Nullable(Of Boolean)))
        obj.IDCuentasclientes = p1
    End Sub

    Private Sub InsertClientesReceptore(ByVal obj As OyDClientes.ClientesReceptore)
        Dim p1 As Integer = obj.IDClientesReceptores
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.IDComitente
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_ClientesReceptores_Actualizar(obj.Aprobacion, iddetallecomitente, obj.IDReceptor, CType(obj.Lider, System.Nullable(Of Boolean)), obj.Usuario, CType(obj.Porcentaje, System.Nullable(Of Double)), p1, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDClientesReceptores = p1
    End Sub

    Private Sub InsertClientesOrdenante(ByVal obj As OyDClientes.ClientesOrdenante)
        Dim p1 As Integer = obj.IDClientesOrdenantes
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.IDComitente
        Else
            iddetallecomitente = idcomitente
        End If
        If obj.IDOrdenante = obj.IDComitente Then
            Me.uspOyDNet_Clientes_ClientesOrdenantes_Actualizar(obj.Aprobacion, iddetallecomitente, iddetallecomitente, CType(obj.lider, System.Nullable(Of Boolean)), obj.Usuario, CType(obj.Relacionado, System.Nullable(Of Boolean)), CType(obj.ValorLimitacionOperacion, System.Nullable(Of Decimal)), obj.Parentesco, p1, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
            obj.IDClientesOrdenantes = p1
        Else
            Me.uspOyDNet_Clientes_ClientesOrdenantes_Actualizar(obj.Aprobacion, iddetallecomitente, obj.IDOrdenante, CType(obj.lider, System.Nullable(Of Boolean)), obj.Usuario, CType(obj.Relacionado, System.Nullable(Of Boolean)), CType(obj.ValorLimitacionOperacion, System.Nullable(Of Decimal)), obj.Parentesco, p1, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
            obj.IDClientesOrdenantes = p1
        End If

    End Sub

    Private Sub InsertClientesBeneficiarios(ByVal obj As OyDClientes.ClientesBeneficiarios)
        Dim p1 As Integer = obj.IDBeneficiarios
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.IDComitente
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_ClientesBeneficiarios_Actualizar(obj.Aprobacion, iddetallecomitente, obj.TipoID, CType(obj.NroDocumento, System.Nullable(Of Decimal)), obj.Nombre, obj.Direccion, obj.Telefono, obj.Parentesco, CType(obj.Activo, System.Nullable(Of Boolean)), obj.Apellido1, obj.Apellido2, obj.Nombre1, obj.Nombre2, obj.IDCiudadDoc, obj.IdCiudadDomicilio, obj.IdConsecutivoSafyr, obj.IdConsecutivoClientes, obj.IdConsecutivoPortafolios, obj.Usuario, obj.TipoBeneficiario, p1, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDBeneficiarios = p1
    End Sub

    Private Sub InsertClientesAficiones(ByVal obj As OyDClientes.ClientesAficiones)
        Dim p1 As Integer = obj.IDClientesAficiones
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.IDComitente
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_ClientesAficiones_Actualizar(obj.Aprobacion, iddetallecomitente, obj.IDSucCliente, obj.Retorno, obj.Usuario, p1, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDClientesAficiones = p1
    End Sub
    Private Sub UpdateClientesAficiones(ByVal obj As OyDClientes.ClientesAficiones)
        Dim p1 As Integer = obj.IDClientesAficiones
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.IDComitente
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_ClientesAficiones_Actualizar(obj.Aprobacion, iddetallecomitente, obj.IDSucCliente, obj.Retorno, obj.Usuario, p1, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDClientesAficiones = p1
    End Sub

    Private Sub InsertClientesDeportes(ByVal obj As OyDClientes.ClientesDeportes)
        Dim p1 As Integer = obj.IDClientesDeportes
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.IDComitente
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_ClientesDeportes_Actualizar(obj.Aprobacion, iddetallecomitente, obj.IDSucCliente, obj.Retorno, obj.Usuario, p1, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDClientesDeportes = p1
    End Sub

    Private Sub UpdateClientesDeportes(ByVal obj As OyDClientes.ClientesDeportes)
        Dim p1 As Integer = obj.IDClientesDeportes
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.IDComitente
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_ClientesDeportes_Actualizar(obj.Aprobacion, iddetallecomitente, obj.IDSucCliente, obj.Retorno, obj.Usuario, p1, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDClientesDeportes = p1
    End Sub


    Private Sub InsertClientesDireccione(ByVal obj As OyDClientes.ClientesDireccione)
        Dim p1 As Integer = obj.Consecutivo
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.IDComitente
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_ClientesDirecciones_Actualizar(obj.Aprobacion, p1, iddetallecomitente, obj.Direccion, CType(obj.Telefono, System.Nullable(Of Long)), obj.Fax, obj.Ciudad, obj.Tipo, CType(obj.DireccionEnvio, System.Nullable(Of Boolean)), CType(obj.Activo, System.Nullable(Of Boolean)), obj.EntregaA, obj.IdConsecutivoSafyr, obj.IdConsecutivoClientes, obj.IdConsecutivoPortafolios, obj.Usuario, obj.Extension, obj.intClave_PorAprobar, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.Consecutivo = p1
    End Sub

    Private Sub InsertClientesDocumentosRequeridos(ByVal obj As OyDClientes.ClientesDocumentosRequeridos)
        Dim p1 As System.Nullable(Of Short) = obj.Documento
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.IDComitente
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_ClientesDocumentosR_Actualizar(p1, iddetallecomitente, obj.NombreDocumento, CType(obj.Entregado, System.Nullable(Of Boolean)), CType(obj.Entrega, System.Nullable(Of Date)), CType(obj.IniVigencia, System.Nullable(Of Date)), CType(obj.FinVigencia, System.Nullable(Of Date)), obj.IDDoctosCliente, obj.Usuario, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDDoctosCliente = p1.GetValueOrDefault
    End Sub
    Private Sub UpdateClientesDocumentosRequeridos(ByVal obj As OyDClientes.ClientesDocumentosRequeridos)
        Dim p1 As System.Nullable(Of Short) = obj.Documento
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.IDComitente
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_ClientesDocumentosR_Actualizar(p1, iddetallecomitente, obj.NombreDocumento, CType(obj.Entregado, System.Nullable(Of Boolean)), CType(obj.Entrega, System.Nullable(Of Date)), CType(obj.IniVigencia, System.Nullable(Of Date)), CType(obj.FinVigencia, System.Nullable(Of Date)), obj.IDDoctosCliente, obj.Usuario, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDDoctosCliente = p1.GetValueOrDefault
    End Sub

    Private Sub InsertClientesLOGHistoriaIR(ByVal obj As OyDClientes.ClientesLOGHistoriaIR)
        Dim p1 As Integer = obj.IDRegistro
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.ID
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_LOGHistoriaIR_Actualizar(p1, iddetallecomitente, obj.Comentario, obj.Usuario, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDRegistro = p1
    End Sub

    Private Sub InsertClientesConocimientoEspecifico(ByVal obj As OyDClientes.ClientesConocimientoEspecifico)
        Dim p1 As Integer = obj.IDClientesConocimiento
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.ID
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_ConocimientoEspecifico_Actualizar(obj.Aprobacion, iddetallecomitente, obj.CodigoConocimiento, obj.Usuario, p1, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDClientesConocimiento = p1
    End Sub
    Private Sub InsertTipoClient(ByVal obj As OyDClientes.TipoClient)
        Dim p1 As Integer = obj.IDTipoCliente
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.IDComitente
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_TipoCliente_Actualizar(obj.Aprobacion, iddetallecomitente, obj.IDTipoEntidad, obj.Usuario, p1, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDTipoCliente = p1
    End Sub
    Private Sub UpdateClientesConocimientoEspecifico(ByVal obj As OyDClientes.ClientesConocimientoEspecifico)
        Dim p1 As Integer = obj.IDClientesConocimiento
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.ID
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_ConocimientoEspecifico_Actualizar(obj.Aprobacion, iddetallecomitente, obj.CodigoConocimiento, obj.Usuario, p1, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDClientesConocimiento = p1
    End Sub
    Private Sub InsertClientesAccionistas(ByVal obj As OyDClientes.ClientesAccionistas)
        Dim p1 As Integer = obj.IDClientesAccionistas
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.IDComitente
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_ClientesAccionistas_Actualizar(obj.Aprobacion, iddetallecomitente, obj.TipoIdentificacion, obj.NroDocumento, obj.Direccion, obj.Telefono, obj.Apellido1, obj.Apellido2, obj.Nombre1, obj.Nombre2, obj.IDCiudad, obj.Usuario, p1, obj.Observaciones, CType(obj.Participacion, System.Nullable(Of Double)), obj.ClienteAccionista, obj.TipoVinculacionAccionista, obj.IDNacionalidad, obj.TipoCiudadano, CType(obj.AplicaFatca, System.Nullable(Of Boolean)), obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)), obj.PaisResidenciaFiscal, CType(obj.TIN, String), CType(obj.NacimientoAccionista, System.Nullable(Of Date)))
        obj.IDClientesAccionistas = p1
    End Sub

    Private Sub InsertClientesPersonasParaConfirmar(ByVal obj As OyDClientes.ClientesPersonasParaConfirmar)
        Dim p1 As Integer = obj.IDClientesPersonas
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.IDComitente
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_ClientesPersonas_Actualizar(obj.Aprobacion, iddetallecomitente, obj.TipoDocumento, CType(obj.NroDocumento, System.Nullable(Of Decimal)), obj.Telefono1, obj.Telefono2, obj.Cargo, obj.idPoblacion, obj.Apellido1, obj.Apellido2, obj.Nombre1, obj.Nombre2, p1, obj.Usuario, CType(Nothing, String), CType(Nothing, System.Nullable(Of Byte)))
        obj.IDClientesPersonas = p1
    End Sub
    Private Sub InsertClientesPersonasDepEconomica(ByVal obj As OyDClientes.ClientesPersonasDepEconomica)
        Dim p1 As Integer = obj.IDClientesPersDepEco
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.IDComitente
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_ClientesPersonasDepEconomica_Actualizar(obj.Aprobacion, iddetallecomitente, obj.IDComitentePerDepEco, obj.TipoDocumento, obj.NroDocumento, obj.Telefono1, obj.Telefono2, obj.Direccion, obj.idPoblacion, obj.Apellido1, obj.Apellido2, obj.Nombre1, obj.Nombre2, obj.Parentesco, CType(obj.Activo, System.Nullable(Of Boolean)), p1, obj.Usuario, CType(Nothing, String), CType(Nothing, System.Nullable(Of Byte)))
        obj.IDClientesPersDepEco = p1
    End Sub

    Private Sub InsertClientesProductos(ByVal obj As OyDClientes.ClientesProductos)
        Dim p1 As System.Nullable(Of Short) = obj.intIDClientesProductos
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.lngIDComitente
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_ClientesProductos_Actualizar(
            p1,
            iddetallecomitente,
            obj.strIDProducto,
            CType(obj.logOperaProducto, System.Nullable(Of Boolean)),
            obj.strUsuario,
            obj.strInfoSesion,
            CType(Nothing, System.Nullable(Of Byte)))
        obj.intIDClientesProductos = p1.GetValueOrDefault
    End Sub

    Private Sub UpdateClientesProductos(ByVal obj As OyDClientes.ClientesProductos)
        Dim p1 As System.Nullable(Of Short) = obj.intIDClientesProductos
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.lngIDComitente
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_ClientesProductos_Actualizar(
            p1,
            iddetallecomitente,
            obj.strIDProducto,
            CType(obj.logOperaProducto, System.Nullable(Of Boolean)),
            obj.strUsuario,
            obj.strInfoSesion,
            CType(Nothing, System.Nullable(Of Byte)))
        obj.intIDClientesProductos = p1.GetValueOrDefault
    End Sub

    Private Sub InsertClientesPaisesFATCA(ByVal obj As OyDClientes.ClientesPaisesFATCA)
        Dim p1 As System.Nullable(Of Integer) = obj.IDClientesPaises
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.IDComitente
        Else
            iddetallecomitente = idcomitente
        End If

        Me.uspOyDNet_Clientes_ClientesPaisesFATCA_Actualizar(obj.Aprobacion, p1, iddetallecomitente, CType(obj.IDPais, System.Nullable(Of Integer)), obj.NIF, obj.TipoCiudadano, obj.Usuario, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDClientesPaises = p1.GetValueOrDefault
    End Sub

    Private Sub UpdateClientesPaisesFATCA(ByVal obj As OyDClientes.ClientesPaisesFATCA)
        Dim p1 As System.Nullable(Of Integer) = obj.IDClientesPaises
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.IDComitente
        Else
            iddetallecomitente = idcomitente
        End If

        Me.uspOyDNet_Clientes_ClientesPaisesFATCA_Actualizar(obj.Aprobacion, p1, iddetallecomitente, CType(obj.IDPais, System.Nullable(Of Integer)), obj.NIF, obj.TipoCiudadano, obj.Usuario, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDClientesPaises = p1.GetValueOrDefault
    End Sub

    ''' <summary>
    ''' CFMA20180828 S-37836 Adicionamos codigo para que el sistema guarde la informacion correctamente en la pestaña ficha cliente 
    ''' </summary>
    ''' <param name="obj"></param>
    Private Sub InsertClientesFicha(ByVal obj As OyDClientes.ClientesFicha)
        Dim p1 As System.Nullable(Of Integer) = obj.IDClientesFicha
        If idcomitente = String.Empty Then
            iddetallecomitente = obj.IDComitente
        Else
            iddetallecomitente = idcomitente
        End If
        Me.uspOyDNet_Clientes_ClientesFicha_Actualizar(obj.Aprobacion, iddetallecomitente, obj.CodDocumento, obj.TipoReferencia, obj.Descripcion, obj.IDPoblacion, p1, obj.Periodicidad, obj.DiasEnvio, obj.Usuario, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDClientesFicha = p1.GetValueOrDefault
    End Sub



#Region "Traslado de Saldos"

    <Global.System.Data.Linq.Mapping.FunctionAttribute(Name:="dbo.uspOyDNet_Clientes_SaldoDisponible_ConsultarSaldo")>
    Public Function uspOyDNet_Clientes_SaldoDisponible_ConsultarSaldo(
                                                                     <Global.System.Data.Linq.Mapping.ParameterAttribute(Name:="pbitSaldoDisponibleChequeado", DbType:="Bit")> ByVal boolSaldoDisponibleChequeado As System.Nullable(Of Boolean), <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="varchar")> ByVal pstrCodigoCliente As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Datetime NOT NULL")> ByVal pdtmFecha As System.Nullable(Of Date),
                                                                     <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Money")> ByRef pcurSaldoCorte As Decimal, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal infosesion As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="TinyInt")> ByVal pintErrorPersonalizado As System.Nullable(Of Byte)) As Integer
        Dim result As IExecuteResult = Me.ExecuteMethodCall(Me, CType(MethodInfo.GetCurrentMethod, MethodInfo), boolSaldoDisponibleChequeado, pstrCodigoCliente, pdtmFecha, pcurSaldoCorte, infosesion, pintErrorPersonalizado)
        pcurSaldoCorte = CType(result.GetParameterValue(3), System.Nullable(Of Decimal))
        Return CType(result.ReturnValue, Integer)
    End Function

    Private Sub UpdateClientesCustodias(instance As ClientesCustodias)

    End Sub

#End Region



End Class
