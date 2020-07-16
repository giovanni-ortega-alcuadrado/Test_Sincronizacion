''' <summary>
''' Enumerado para los estados de la alarma del visor de garantías
''' </summary>
''' <remarks></remarks>
Public Enum EstadosAlarma
    EstadoOk = 1
    EstadoAlarma = 2
    EstadoError = 3
End Enum


''' <summary>
''' Enumerador para el manejo de las acciones a ejecutar en bloqueos y desbloqueos para manejo recursivo
''' </summary>
''' <remarks></remarks>
Public Enum AccionEjecucion
    BloqueoSaldo = 1
    DesbloqueoSaldo = 2
    BloqueodesbloqueoTitulo = 3
    DistribuirBloqueoTitulo = 4
    DistribuirBloqueoSaldo = 5
End Enum