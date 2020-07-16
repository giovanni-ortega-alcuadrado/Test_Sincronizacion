Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: CustodiaView.xaml.vb
'Generado el : 06/17/2011 11:58:21
'Propiedad de Alcuadrado S.A. 2010
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class CustodiaView
    Inherits UserControl
    Dim logcargainicial As Boolean = True
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorGenericoLista

    Public Sub New()
        Me.DataContext = New CustodiaViewModel
InitializeComponent()
    End Sub

    Private Sub Custodia_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If logcargainicial Then
            ' logcargainicial = False
            cm.GridTransicion = grdGridForma
            cm.GridViewRegistros = datapager1
            AddHandler CType(Me.DataContext, CustodiaViewModel).PropertyChanged, AddressOf onPropertyChanged

            'cm.DF = df
            CType(Me.DataContext, CustodiaViewModel).NombreView = Me.ToString
        Else
            If Not CType(Me.DataContext, CustodiaViewModel).Editando And CType(Me.DataContext, CustodiaViewModel).visNavegando = "Visible" Then
                CType(Me.DataContext, CustodiaViewModel).RefrescarForma()
            End If
        End If

    End Sub
    Private Sub onPropertyChanged(sender As Object, e As PropertyChangedEventArgs)
        If e.PropertyName = "LogHabilitarReceptor" Then
            OcultarColumnas()
        End If
    End Sub

    'Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
    '    df.CancelEdit()
    ''    If Not IsNothing(df.ValidationSummary) Then
    '        df.ValidationSummary.DataContext = Nothing
    '    End If
    '    mensaje.IsOpen = False
    'End Sub

    'Private Sub cm_EventoConfirmarGrabacion(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarGrabacion
    '    df.ValidateItem()
    '    If df.ValidationSummary.HasErrors Then
    '        df.CancelEdit()
    '    Else
    '        df.CommitEdit()
    '    End If
    'End Sub

    'Private Sub cm_EventoConfirmarBorrado(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarBorrado
    '    'df.Focus()
    '    'Dim s = CType(df.FindName("txtConceptoAnulacion"), TextBox)
    '    's.Focus()
    '    '  mensaje.IsOpen = True
    'End Sub
    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                CType(Me.DataContext, CustodiaViewModel).IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                CType(Me.DataContext, CustodiaViewModel).IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los combos.",
                                 Me.ToString(), "cm_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub AceptarCerrar(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        mensaje.IsOpen = False
    End Sub
    Private Sub RechazarCerrar(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        mensaje.IsOpen = False
    End Sub

    ''' <summary>
    ''' Funcion que se ejecuta cuando se termina de realizar la busqueda de clientes por parte del buscador de clientes para actualizar las propiedades
    ''' de la clase cb con la informacion seleccionada en el Builder de busqueda de Clientes.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Modificado por   : Juan Carlos Soto.
    ''' Descripción      : Se adiciona la logica para controlar la asignacion del comitente, segun el id del buscador de clientes. Se maneja un Id para el modo edicion y otro para el modo de busqueda.
    ''' Fecha            : Febrero 19/2013
    ''' Pruebas Negocio  : Juan Carlos Soto Cruz - Febrero 20/2013 - Resultado Ok 
    '''  
    ''' ID Modificación  : 000001
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se modifica el método con el fin de garanizar que el "Comitente" seleccionado en el encabezado de la custodia debe quedar seleccionado en el detalle.
    ''' Fecha            : Febrero 28/2013
    ''' Pruebas Negocio  : Yeiny Adenis Marín Zapata - Febrero 28/2013 - Resultado Ok 
    ''' 
    ''' ID Modificación  : 000002
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se modifica método para controlar si el comitente es buscado por la lupita o si el usuario digito el ID.
    ''' Fecha            : Febrero 28/2013
    ''' Pruebas Negocio  : Yeiny Adenis Marín Zapata - Febrero 28/2013 - Resultado Ok 
    ''' 
    ''' ID Modificación  : 000003
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se agrega validación para inhabilitar los campos del cliente cuando éste es encontrado desde el buscador
    ''' Fecha            : Marzo 13/2013
    ''' Pruebas Negocio  : Yeiny Adenis Marín Zapata - Marzo 13/2013 - Resultado Ok 
    ''' 
    ''' ID Modificación  : 000004
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se asigna el valor que tenga configurado el cliente en el campo AdmonValores a la propiedad mstrAdmonValores 
    ''' Fecha            : Marzo 27/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 27/2013 - Resultado Ok
    ''' 
    ''' ID Modificación  : 000005
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se incluye variable que indica si el cliente del detalle ya fue buscado a través de la lupita del cliente en el encabezado. 
    ''' Fecha            : Abril 10/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Abril 10/2013 - Resultado Ok  
    ''' 
    ''' Descipción:        Se retorna la propiedad dtmFechaCierrePortafolio para mostrarlo en la pantalla.
    ''' Responsable:       Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             24 de AGosto/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - 24 de AGosto/2015 - Resultado OK
    ''' </history>
    Private Sub Buscador_finalizoBusquedaClientes(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjItem) Then
                Select Case pstrClaseControl
                    Case "IDComitente1"
                        CType(Me.DataContext, CustodiaViewModel).cb.Comitente = pobjItem.IdComitente
                        CType(Me.DataContext, CustodiaViewModel).cb.Nombre = pobjItem.Nombre
                    Case "IDComitente"
                        CType(Me.DataContext, CustodiaViewModel).CustodiSelected.strTipoCompania = pobjItem.strTipoCompania
                        CType(Me.DataContext, CustodiaViewModel).ValidarHabilitarReceptor()

                        CType(Me.DataContext, CustodiaViewModel)._mlogBuscarClienteEncabezado = False 'ID Modificación 000002
                        CType(Me.DataContext, CustodiaViewModel)._mLogBuscarClienteDetalle = False 'ID Modificación 000005

                        Dim logEsNumerico As Boolean = True

                        Try
                            Dim lngNroConvertido As Decimal? = pobjItem.NroDocumento
                        Catch ex As Exception
                            logEsNumerico = False
                        End Try

                        CType(Me.DataContext, CustodiaViewModel).CustodiSelected.Comitente = pobjItem.IdComitente
                        CType(Me.DataContext, CustodiaViewModel).CustodiSelected.Nombre = pobjItem.Nombre
                        CType(Me.DataContext, CustodiaViewModel).CustodiSelected.Direccion = pobjItem.DireccionEnvio
                        CType(Me.DataContext, CustodiaViewModel).CustodiSelected.Telefono1 = pobjItem.Telefono
                        CType(Me.DataContext, CustodiaViewModel).CustodiSelected.dtmFechaCierrePortafolio = pobjItem.dtmFechaCierrePortafolio

                        If logEsNumerico Then
                            CType(Me.DataContext, CustodiaViewModel).CustodiSelected.NroDocumento = pobjItem.NroDocumento
                        Else
                            CType(Me.DataContext, CustodiaViewModel).CustodiSelected.NroDocumento = "0"
                        End If

                        CType(Me.DataContext, CustodiaViewModel).CustodiSelected.TipoIdentificacion = pobjItem.CodTipoIdentificacion
                        CType(Me.DataContext, CustodiaViewModel).HabilitarCamposCliente = False 'ID Modificación 000001
                        CType(Me.DataContext, CustodiaViewModel)._mlogBuscarClienteEncabezado = True 'ID Modificación 000002
                        If (CType(Me.DataContext, CustodiaViewModel)._mLogAgregarRegistroDetalle) Then  'ID Modificación 000001
                            CType(Me.DataContext, CustodiaViewModel).NuevoRegistroDetalle()
                            CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Comitente = pobjItem.IdComitente
                            CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Nombre = pobjItem.Nombre
                            CType(Me.DataContext, CustodiaViewModel).LimpiarCamposEspecie()

                            Select Case pobjItem.AdmonValores 'ID Modificación 000004
                                Case "T", "N"
                                    CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Admonvalores = "T" 'Todos
                                Case "A"
                                    CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Admonvalores = "A" 'Renta Variable
                                Case "R"
                                    CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Admonvalores = "C" 'Renta Fija
                            End Select

                            CType(Me.DataContext, CustodiaViewModel)._mLogBuscarClienteDetalle = True 'ID Modificación 000005
                            CType(Me.DataContext, CustodiaViewModel)._mLogAgregarRegistroDetalle = False
                        End If
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el clientes.",
                                 Me.ToString(), "Buscador_finalizoBusquedaClientes", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
    Private Sub OcultarColumnas()

        For Each Columna As Telerik.Windows.Controls.GridViewDataColumn In dgDetalleCustodia.Columns
            If Columna.Header = "Receptor" Then
                Dim habilitarReceptor As Boolean = CType(Me.DataContext, CustodiaViewModel).LogHabilitarReceptor
                If habilitarReceptor Then
                    Columna.IsVisible = True
                Else
                    Columna.IsVisible = False
                End If
            End If
        Next

    End Sub
    ''' <summary>
    ''' Al seleccionar  el cliente en el detalle de la custodia debe autodiligenciar el campo Nombre (Cliente)
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Se modifica el método con el fin de garanizar que al seleccionar el cliente se autodiligencie el campo Nombre. Cada vez que se selecciona un cliente
    ''' se limpian los campos que se autocompletan según la especie, debido a que no todos los clientes pueden seleccionar las especies del mismo tipo (Acciones, RF) 
    ''' ID Modificación  : 000001
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se adiciona lógica para asignarle el nombre del cliente al control nombre del detalle.
    ''' Fecha            : Febrero 28/2013
    ''' Pruebas Negocio  : Yeiny Adenis Marín Zapata - Febrero 28/2013 - Resultado Ok 
    ''' 
    ''' ID Modificación  : 000002
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se asigna el valor que tenga configurado el cliente en el campo AdmonValores a la propiedad mstrAdmonValores 
    ''' Fecha            : Marzo 27/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 27/2013 - Resultado Ok 
    ''' 
    ''' ID Modificación  : 000003
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se incluye variable que indica si el cliente del detalle fue buscado a través de la lupita. 
    ''' Fecha            : Abril 10/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Abril 10/2013 - Resultado Ok  
    ''' ID Modificación  : 000004
    ''' Agregado por     : Natalia Andrea Otalvaro Castrillon.
    ''' Descripción      : Se incluye logica para el caso 1027, cuando el cliente en detalle custodis tenga la misma cuenta sociada la deja por defecto, si tiene una cuenta diferente la borra. 
    ''' Fecha            : Julio 28 /2014
    ''' Pruebas CB       . Natalia Andrea Otalvaro Castrillon- Julio 28/2014 - Resultado Ok  
    ''' </history>
    ''' 
    Private Sub Buscador_finalizoBusquedaClientesdetalle(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then

            CType(Me.DataContext, CustodiaViewModel)._mLogBuscarClienteDetalle = False 'ID Modificación  : 000003
            CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Comitente = pobjItem.IdComitente
            'CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Comitente = pobjItem.Nombre
            CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Nombre = pobjItem.Nombre 'ID Modificación  : 000001
            'CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.IdCuentaDeceval = Nothing 'ID Modificación  : 000004
            'CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Fondo = Nothing
            'CType(Me.DataContext, CustodiaViewModel).LimpiarCamposEspecie()

            Select Case pobjItem.AdmonValores 'ID Modificación  : 000002
                Case "T", "N"
                    CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Admonvalores = "T" 'Todos
                Case "A"
                    CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Admonvalores = "A" 'Renta Variable
                Case "R"
                    CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Admonvalores = "C" 'Renta Fija
            End Select

            CType(Me.DataContext, CustodiaViewModel)._mLogBuscarClienteDetalle = True 'ID Modificación  : 000003

            CType(Me.DataContext, CustodiaViewModel).VerificarCuentaDecevalCliente()

            CType(Me.DataContext, CustodiaViewModel).CustodiSelected.strTipoCompania = pobjItem.strTipoCompania

            CType(Me.DataContext, CustodiaViewModel).ValidarHabilitarReceptor()
        End If
    End Sub

    ''' <summary></summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' ID Modificación  : 000001
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se invoca el método autocompletar algunos campos en el detalle según la especie seleccionada.
    ''' Fecha            : Febrero 28/2013
    ''' Pruebas Negocio  : Yeiny Adenis Marín Zapata - Febrero 28/2013 - Resultado Ok 
    ''' 
    ''' ID Modificación  : 000002
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se modifica método para controlar si la especie es buscada por la lupita o si el usuario digito el ID.
    ''' Fecha            : Marzo 21/2013
    ''' Pruebas Negocio  : Yeiny Adenis Marín Zapata - Marzo 21/2013 - Resultado Ok 
    ''' 
    ''' ID Modificación  : 000003
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se comenta la línea de código que autocompleta el ISIN, ya que este campo se autollena con el método AutocompletarCamposSegunEspecie
    '''                    Se comenta la línea de código que autocompleta el Fungible, ya que este campo no se usa en esta versión.
    ''' Fecha            : Marzo 21/2013
    ''' Pruebas Negocio  : Yeiny Adenis Marín Zapata - Marzo 21/2013 - Resultado Ok 
    ''' </history>
    '''
    Private Sub Buscador_finalizoBusquedaespecies(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjItem) Then
            'ID Modificación 000002
            CType(Me.DataContext, CustodiaViewModel)._mlogBuscarEspecie = False
            CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.IdEspecie = pobjItem.Nemotecnico

            'ID Modificación  : 000003

            'CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.ISIN = pobjItem.ISIN
            'If Not IsNothing(pobjItem.IDFungible) Then
            '    CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Fungible = pobjItem.IDFungible
            'End If
            'CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Comitente = pobjItem.Nombre

            CType(Me.DataContext, CustodiaViewModel).AutocompletarCamposSegunEspecie(pobjItem) 'ID Modificación  : 000001
            CType(Me.DataContext, CustodiaViewModel)._mlogBuscarEspecie = True 'ID Modificación  : 000002
        End If
    End Sub
    Private Sub BuscadorGenerico_finalizoBusquedaCuenta(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.IdCuentaDeceval = CInt(pobjItem.IdItem)
        End If
    End Sub

    Private Sub BuscadorCuentas_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If IsNothing(CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Comitente) Then
            'C1.Silverlight.C1MessageBox.Show("Debe escoger un comitente", "OyDserver")
            A2Utilidades.Mensajes.mostrarMensaje("Debe escoger un comitente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else

            DirectCast(sender, A2ComunesControl.BuscadorGenericoListaButon).Agrupamiento = CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Comitente
        End If

    End Sub
    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub
    

    Private Sub Button_Click_BuscadorLista(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not IsNothing(CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected) Then
            mobjBuscadorLst = New A2ComunesControl.BuscadorGenericoLista("ISIESPECIEISINFUNGIBLEN", "ESPECIEISINFUNGIBLE", "ESPECIEISINFUNGIBLE", A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.A, CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.IdEspecie, "", "")
            Program.Modal_OwnerMainWindowsPrincipal(mobjBuscadorLst)
            mobjBuscadorLst.ShowDialog()

        End If
    End Sub
    ''' <history>
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se hace un llamado al método que autocompleta los datos asociados al ISIN y a la especie, cada vez que el usuario selecciona un ISIN.
    ''' Fecha            : Mayo 05/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Mayo 05/2013 - Resultado Ok 
    ''' </history>
    Private Sub mobjBuscadorLst_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjBuscadorLst.Closed
        If Not mobjBuscadorLst.ItemSeleccionado Is Nothing Then

            Select Case mobjBuscadorLst.CampoBusqueda.ToLower
                Case "isin"
                    CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.ISIN = mobjBuscadorLst.ItemSeleccionado.IdItem
                    Dim strEspecieSeleccionada = CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.IdEspecie
                    If Not String.IsNullOrEmpty(strEspecieSeleccionada) Then
                        CType(Me.DataContext, CustodiaViewModel).BuscarEspecie(strEspecieSeleccionada, String.Empty)
                    End If
                Case Else
            End Select
        End If
    End Sub

    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se agrega método para validar que el usuario solo pueda ingresar Números en el campo Comitente.
    ''' Fecha            : Marzo 06/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 06/2013 - Resultado Ok 
    ''' </history>
    Private Sub txtComitenteBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
            e.Handled = True
        End If
    End Sub

    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se agrega método para validar que el usuario solo pueda ingresar Números en el campo Comitente.
    ''' Fecha            : Marzo 22/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 22/03/2013 - Resultado Ok 
    ''' </history>

    Private Sub Buscador_Especie_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            If CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Admonvalores = "C" Then
                CType(sender, A2ComunesControl.BuscadorEspecieListaButon).ClaseOrden = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.C
            ElseIf CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Admonvalores = "A" Then
                CType(sender, A2ComunesControl.BuscadorEspecieListaButon).ClaseOrden = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.A
            Else
                CType(sender, A2ComunesControl.BuscadorEspecieListaButon).ClaseOrden = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.T
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en Buscador_Especie_GotFocus",
                                 Me.ToString(), "Buscador_Especie_GotFocus", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se agrega método seleccionar la cuenta depósito y autodiligenciar el fondo, según la cuenta.
    ''' Fecha            : Marzo 22/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 22/03/2013 - Resultado Ok 
    ''' </history>
    Private Sub BuscadorGenerico_finalizoBusquedaCuentasDeposito(ByVal pintCuentaDeposito As Integer, ByVal pobjCuentaDeposito As OYDUtilidades.BuscadorCuentasDeposito)
        If Not IsNothing(pobjCuentaDeposito) Then
            CType(Me.DataContext, CustodiaViewModel)._mStrFondo = pobjCuentaDeposito.Deposito
            CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.IdCuentaDeceval = CInt(pobjCuentaDeposito.NroCuentaDeposito)
            CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Fondo = pobjCuentaDeposito.Deposito
        End If
    End Sub

    Private Sub cmbFondo_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If Not String.IsNullOrEmpty(CType(Me.DataContext, CustodiaViewModel)._mStrFondo) Then
            If Not CType(Me.DataContext, CustodiaViewModel)._mStrFondo.Equals(CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.Fondo) Then
                CType(Me.DataContext, CustodiaViewModel).DetalleCustodiaSelected.IdCuentaDeceval = Nothing
            End If
        End If
    End Sub

    Private Sub Button_BuscarReceptor(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, CustodiaViewModel).BuscarReceptor()
    End Sub

End Class


