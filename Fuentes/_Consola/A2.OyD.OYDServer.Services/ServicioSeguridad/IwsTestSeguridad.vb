Imports System.ServiceModel

' NOTE: You can use the "Rename" command on the context menu to change the interface name "IwsTestSeguridad" in both code and config file together.
<ServiceContract()>
Public Interface IwsTestSeguridad

    <OperationContract()>
    Function ValidacionSeguridad(ByVal pstrUsuario As String, ByVal pstrToken As String) As String

End Interface
