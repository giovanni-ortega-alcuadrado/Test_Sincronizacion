'-- CCM20131025 - Para utlizar en el Acerca de la consola y aplicación activa
Module Version
    Public Const GSTR_NAMESPACE_APLICACION As String = "NAMESPACE_APLICACION"  ' Nombre del namespace base de la aplicación activa - NAMESPACE_APLICACION
    Public Const GSTR_CLAVE_PARAM_CONEXION_DATOS As String = "A2CONSOLA_CNXBASEDATOS"  ' Nombre de la clave de los parámetros que contiene la conexión a la base de datos
    Public Const GSTR_CLAVE_PARAM_CARPETA_UPLOAD As String = "A2CONSOLA_CARPETAUPLOADS"  ' Nombre de la clave de los parámetros que contiene el nombre de la carpeta de Uploads
    Public Const GSTR_CLAVE_REF_COMPONENTE_APP As String = "REFERENCIA_PARAMETROS_CONSOLA" ' Parámetro que se busca en la configuración de la aplicación, retornada por el sistema de seguridad, para saber el componente a ejecutar para consultar los parámetros
    Public Const GSTR_CLAVE_USUARIO_AUT_ACERCADE As String = "MOSTRAR_DATOS_TECNICOS"
    Public Const GSTR_CLAVE_NOMBRE_APP As String = "APLICACIONNOMBRE"
    Public Const GSTR_CLAVE_VERSION_APP As String = "APLICACIONVERSION"
    Public Const GSTR_CLAVE_MSG_DERECHOSAUTOR As String = "APLICACIONDERECHOSAUTOR"
    Public Const GSTR_CLAVE_FECHA_APP As String = "APLICACIONFECHA"
    Public Const GSTR_CLAVE_SW_REPORTING_SERVICES As String = "A2VServicioRS"
    Public Const GSTR_CLAVE_SW_VISOR_RPTS As String = "A2VServicioParam"
    Public Const GSTR_CLAVE_BASE_DATOS_UTILS As String = "BDUTILIDADES"
    Public Const GSTR_CLAVE_SW_DOCUMENTOS As String = "URLServicioDocumentos"
    Public Const GSTR_CLAVE_SW_VISOR_SETEADOR As String = "RutaVisorXAP"
    Public Const GSTR_CLAVE_PARAM_APP_OYDPLUS_NOMBRE As String = "APLICACIONOYDPLUS_NOMBRE"
    Public Const GSTR_CLAVE_PARAM_APP_OYDPLUS_VERSION As String = "APLICACIONOYDPLUS_VERSION"
    Public Const GSTR_CLAVE_PARAM_APP_BASESERVICIOWPF As String = "SERVICIOSRIABASE_WPF"

End Module

'-- CCM20131025 - Para controlar versión de las dlls de la aplicación activa
Friend Class AssemblyDesactualizada
    Public Property Assembly As String
    Public Property VersionAssembly As String
    Public Property VersionRegistrada As String
End Class