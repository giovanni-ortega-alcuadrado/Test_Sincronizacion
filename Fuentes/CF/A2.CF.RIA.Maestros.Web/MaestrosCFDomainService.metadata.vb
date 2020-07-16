
Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports A2.OyD.OYDServer.RIA.Web
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Linq
Imports System.Linq

Namespace CFMaestros
    <MetadataTypeAttribute(GetType(Lista.ListaMetadata))> _
    Partial Public Class Lista
        Friend NotInheritable Class ListaMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IDComisionista", Description:="IDComisionista")> _
            Public Property IDComisionista As Integer

            <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            Public Property IDSucComisionista As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Tópico)")> _
            <StringLength(20, ErrorMessage:="El campo {0} permite una longitud máxima de 20 caracteres.")> _
              <Display(Name:="Tópico")> _
            Public Property Topico As String

            <Required(ErrorMessage:="Este campo es requerido. (Nombre)")> _
              <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50 caracteres.")> _
              <Display(Name:="Nombre")> _
            Public Property Descripcion As String

            <Required(ErrorMessage:="Este campo es requerido. (Retorno)")> _
              <StringLength(80, ErrorMessage:="El campo {0} permite una longitud máxima de 80 caracteres.")> _
              <Display(Name:="Retorno")> _
            Public Property Retorno As String

            <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            Public Property Actualizacion As DateTime

            <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

            <Display(Name:="Activo")> _
            Public Property Activo As Boolean

            <Required(ErrorMessage:="Este campo es requerido. (IDLista)")> _
              <Display(Name:="Lista")> _
            Public Property IDLista As Integer

            <Display(Name:="Comentario")> _
            Public Property Comentario As String

        End Class
    End Class

    <MetadataTypeAttribute(GetType(Parametro.ParametroMetadata))> _
    Partial Public Class Parametro
        Friend NotInheritable Class ParametroMetadata
            Private Sub New()
                MyBase.New()
            End Sub


            <Display(Name:="Código")> _
            Public Property IDparametro As Integer

            '<Editable(True)> _
            'JEPM 20150713 Comentada la linea Requiered para quitar el estilo "Negrita" en el label
            '<Required(ErrorMessage:="Este campo es requerido. (Parámetro)")> _
            <Display(Name:="Parámetro")> _
            Public Property Parametro As String

            <Display(Name:="Valor")> _
            Public Property Valor As String

            <Display(Name:="Descripción")> _
            Public Property Descripcion As String

            'JEPM 20150713 Comentada la linea Requiered para quitar el estilo "Negrita" en el label
            '<Required(ErrorMessage:="Este campo es requerido. (Tipo)")> _
            <Display(Name:="Tipo")> _
            Public Property Tipo As String

            <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

            <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            Public Property Actualizacion As DateTime

        End Class
    End Class

    <MetadataTypeAttribute(GetType(Paise.PaiseMetadata))> _
    Partial Public Class Paise
        Friend NotInheritable Class PaiseMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IDComisionista", Description:="IDComisionista")> _
            Public Property IDComisionista As Integer

            <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            Public Property IDSucComisionista As Integer

            <Display(Name:="ID")> _
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Nombre)")> _
            <Display(Name:="Nombre")> _
            Public Property Nombre As String

            <Display(Name:="Código ISO")> _
            Public Property Codigo_ISO As String

            <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            Public Property Actualizacion As DateTime

            <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

            <Display(Name:="Pais")> _
            Public Property IDPais As Integer

            <Display(Name:="Código Dane")> _
            Public Property CodigoDane As String

            'JEPM20150902
            <Display(Name:="Nivel riesgo")> _
            Public Property NivelRiesgo As Integer
            'JEPM20150902
            <Display(Name:="Zona económica")> _
            Public Property ZonaEconomica As String

        End Class
    End Class

    <MetadataTypeAttribute(GetType(Departamento.DepartamentoMetadata))> _
    Partial Public Class Departamento
        Friend NotInheritable Class DepartamentoMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IDComisionista", Description:="IDComisionista")> _
            Public Property IDComisionista As Integer

            <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            Public Property IDSucComisionista As Integer

            <Display(Name:="Pais")> _
            Public Property IDPais As Integer

            <Display(Name:="ID")> _
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Nombre)")> _
            <StringLength(40, ErrorMessage:="El campo permite máximo 40 caracteres..")> _
            <Display(Name:="Nombre")> _
            Public Property Nombre As String

            <Display(Name:="Codigo Dane DPTO.")> _
            <StringLength(2, ErrorMessage:="El campo permite máximo 2 caracteres..")> _
            Public Property Codigo_DaneDEPTO As String

            <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            Public Property Actualizacion As DateTime

            <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

            <Display(Name:="Departamento")> _
            Public Property IDDepartamento As Integer

        End Class
    End Class

    <MetadataTypeAttribute(GetType(Ciudade.CiudadeMetadata))> _
    Partial Public Class Ciudade
        Friend NotInheritable Class CiudadeMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IDComisionista", Description:="IDComisionista")> _
            Public Property IDComisionista As Integer

            <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            Public Property IDSucComisionista As Integer


            <Required(ErrorMessage:="Este campo es requerido. (Código)")> _
              <Display(Name:="Código")> _
            Public Property IDCodigo As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Nombre)")> _
              <Display(Name:="Nombre")> _
            Public Property Nombre As String

            <Display(Name:="Es Capital")> _
            Public Property EsCapital As Boolean

            <Required(ErrorMessage:="Este campo es requerido. (Departamento)")> _
              <Display(Name:="Departamento")> _
            Public Property IDdepartamento As Integer

            <Display(Name:="Código DANE")> _
            Public Property CodigoDANE As String

            <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            Public Property Actualizacion As DateTime

            <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

            <Display(Name:="Ciudad")> _
            Public Property IDCiudad As Integer

        End Class
    End Class

    <MetadataTypeAttribute(GetType(CalificacionesCalificadoraOld.CalificacionesCalificadoraOldMetadata))> _
    Partial Public Class CalificacionesCalificadoraOld
        Friend NotInheritable Class CalificacionesCalificadoraOldMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Required(ErrorMessage:="Este campo es requerido. (Código Super)")> _
              <Display(Name:="Código Super", Description:="Código Super")> _
            Public Property intCodigoSuper As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Código calificadora)")> _
            <Display(Name:="Código calificadora")> _
            Public Property intCodCalificadora As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Calificación)")> _
            <Display(Name:="Calificación")> _
            Public Property intIDCalificacion As Integer


            <Display(Name:="Código Calificación Calificadora", Description:="Código Calificación Calificadora")> _
            Public Property intIdCalificaCalificadora As Integer



        End Class
    End Class

    <MetadataTypeAttribute(GetType(EntidadesCuentasDepositoOld.EntidadesCuentasDepositoOldMetadata))> _
    Partial Public Class EntidadesCuentasDepositoOld
        Friend NotInheritable Class EntidadesCuentasDepositoOldMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Required(ErrorMessage:="Este campo es requerido. (Entidad)")> _
              <Display(Name:="Entidad", Description:="Entidad")> _
            Public Property intIDEntidad As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Deposito)")> _
            <Display(Name:="Deposito")> _
            Public Property strDeposito As String

            <Required(ErrorMessage:="Este campo es requerido. (strCuentaDeposito)")> _
            <Display(Name:="Cuenta Deposito")> _
            Public Property strCuentaDeposito As String


           



        End Class
    End Class

    <MetadataTypeAttribute(GetType(ListaConfiguracion.ListaConfiguracionMetadata))> _
    Partial Public Class ListaConfiguracion
        Friend NotInheritable Class ListaConfiguracionMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IDListaConfiguracion", Description:="intIdListaConfiguracion")> _
            Public Property IDListaConfiguracion As Integer

            <Display(Name:="Tópico")> _
            Public Property Topico As String

            <Display(Name:="TipoDato")> _
            Public Property TipoDato As String

            <Display(Name:="longitud")> _
            Public Property longitud As Integer

            <Display(Name:="Modificable")> _
            Public Property Modificable As Boolean

            <Display(Name:="Descripcion")> _
            Public Property Descripcion As String

            <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

            <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            Public Property Actualizacion As DateTime

            <Display(Name:="LongitudDescripcion")> _
            Public Property LongitudDescripcion As Integer

            <Display(Name:="SQLValidacion")> _
            Public Property SQLValidacion As String

        End Class
    End Class
End Namespace

