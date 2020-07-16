
Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports OpenRiaServices.DomainServices.EntityFramework

<MetadataTypeAttribute(GetType(tblProductos.tblProductosMetadata))>
Partial Public Class tblProductos
    Friend NotInheritable Class tblProductosMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property strNombreProducto As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strPrefijoProducto As String

    End Class
End Class

<MetadataTypeAttribute(GetType(tblProductosVencimientos.tblProductosVencimientosMetadata))>
Partial Public Class tblProductosVencimientos
    Friend NotInheritable Class tblProductosVencimientosMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property strVencimiento As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strNemotecnico As String

    End Class
End Class

<MetadataTypeAttribute(GetType(tblDatosTributarios.tblDatosTributariosMetadata))>
Partial Public Class tblDatosTributarios
    Friend NotInheritable Class tblDatosTributariosMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property strNombre As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strDescripcion As String

    End Class
End Class

<MetadataTypeAttribute(GetType(tblCodComEnc.tblCodComEncMetadata))>
Partial Public Class tblCodComEnc
    Friend NotInheritable Class tblCodComEncMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property strCodComEnc As String

    End Class
End Class

<MetadataTypeAttribute(GetType(tblPreciosDerivados.tblPreciosDerivadosMetadata))>
Partial Public Class tblPreciosDerivados
    Friend NotInheritable Class tblPreciosDerivadosMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property dtmFechaRegistro As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strNemotecnico As String

        <Required(AllowEmptyStrings:=True)>
        Public Property numPrecio As Decimal

        <Required(AllowEmptyStrings:=True)>
        Public Property numStrike As Decimal

    End Class
End Class

<MetadataTypeAttribute(GetType(tblMensajesCumPolitica.tblMensajesCumPoliticaMetadata))>
Partial Public Class tblMensajesCumPolitica
    Friend NotInheritable Class tblMensajesCumPoliticaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property strMensaje As String

    End Class
End Class

<MetadataTypeAttribute(GetType(tblTipoEjercicio.tblTipoEjercicioMetadata))>
Partial Public Class tblTipoEjercicio
    Friend NotInheritable Class tblTipoEjercicioMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property strCodigoTipoEjercicio As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strNombreTipoEjercicio As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strDescripcion As String

    End Class
End Class

<MetadataTypeAttribute(GetType(tblTipoOrdenEjecucion.tblTipoOrdenEjecucionMetadata))>
Partial Public Class tblTipoOrdenEjecucion
    Friend NotInheritable Class tblTipoOrdenEjecucionMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property strCodigoTipoOrdenEjecucion As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strNombreTipoOrdenEjecucion As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strDescripcion As String

    End Class
End Class

<MetadataTypeAttribute(GetType(tblTipoOrdenNaturaleza.tblTipoOrdenNaturalezaMetadata))>
Partial Public Class tblTipoOrdenNaturaleza
    Friend NotInheritable Class tblTipoOrdenNaturalezaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property strCodigoTipoOrdenNaturaleza As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strNombreTipoOrdenNaturaleza As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strDescripcion As String

    End Class
End Class

<MetadataTypeAttribute(GetType(tblTipoOrdenDuracion.tblTipoOrdenDuracionMetadata))>
Partial Public Class tblTipoOrdenDuracion
    Friend NotInheritable Class tblTipoOrdenDuracionMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property strCodigoTipoOrdenDuracion As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strNombreTipoOrdenDuracion As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strDescripcion As String

    End Class
End Class

<MetadataTypeAttribute(GetType(tblTipoOpcionProducto.tblTipoOpcionProductoMetadata))>
Partial Public Class tblTipoOpcionProducto
    Friend NotInheritable Class tblTipoOpcionProductoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property strCodigoTipoOpcion As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strNombreTipoOpcion As String

    End Class
End Class

<MetadataTypeAttribute(GetType(tblFinalidadOperaciones.tblFinalidadOperacionesMetadata))>
Partial Public Class tblFinalidadOperaciones
    Friend NotInheritable Class tblFinalidadOperacionesMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property strDescripcion As String

    End Class
End Class

<MetadataTypeAttribute(GetType(tblTipoFinalidad.tblTipoFinalidadMetadata))>
Partial Public Class tblTipoFinalidad
    Friend NotInheritable Class tblTipoFinalidadMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property strDescripcion As String

    End Class
End Class


