Imports System.IO
Imports System.Threading
Imports System.Xml
Imports libnodave
Imports ketan.ketan.Cliente
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

'cambio lectura datos entrada agrego timeOutDatosEntrada y achico el intervalo tmrPLC
'1.0.0.2 11/7/19 Agrego tres datos mas a mercaderia_insumos(orden, fechvenc, fechprod)
'agrego boton de actualizar listado 
'agrego proceso automatico y siempre activo de cargado de varibles en plc 
'envio todas las variables a las dos etiquetas
'iCantfajas si es 0 lleva unicamente principal
'           si es 1 lleva dos eti pero X en el principio codbar(no faja)
'           si es 2 lleva dos eti y faja 
'           si es 3 lleva solamente la secundaria 
'           si es 4 lleva solo secundaria y no faja 
' tengo problema variable en guardar caja tarda mas de lo esperado 
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' 1.0.0.3 06/08/19 Agrego cuatro variables descripcion(2,3,4 y 5) en mercaderia para enviar a la impresora
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' 1.0.1.3 22/08/19 Se agrega escribir el 0 de hay caja imediatamente despues de recibir el 1.
'                  Como no registraba el cambio de estado escribo en 0 el estado anterior luego de poner en 0 hay caja
' 1.1.1.3 03/09/19 Se agrega version tandem con una espera en el cambio de tandem 
' 1.1.1.4 04/09/19 Agrego para ver status de las dos etiquetadoras y acctualizar mercaderia
' 1.1.2.0 26/12/19 Se realiza una implementacion de enviar a la impresora directo por TCP/IP ya que habia conflictos
'                  con el driver de impresion. Se agrega chkbox para habilitar o deshabilitar linea.
' 1.1.2.1 19/05/20 Agrego: 1- Numero de respuesta al log cuando una lectura da error
'                          2- Control de lectura si no da error proceso 
'                          3- Si la el proceso de las 3 lineas es menos a 30 milisegundos le agrego una pausa para no 
'                             saturar la red 
'                          4- Limpio el buffer de lectura del PLC antes del cambio de linea
' 1.1.2.2 25/06/20 Cambio la letra que poniamos en el codigo de barras por la letra "D" para turno dia y la letra "N"
'                  para turno noche. Saco el textbox de la configuracion y pongo dos radio button en pantalla principal 
'                  para que el operador pueda cambiar el turno.Agrego año al codigo de barras
' 1.1.2.3 20/07/20 Agrego los campos que le faltaban a la tabla de salida que habian sido agregados a la tabla de entrada en algun momento  
' 1.1.2.4 16/09/20 Cambio la condicion que eestaba mal de cambio de dia cuando vuelvo a numerar de caja 0 a caja 1 (noentraba nunca)
'                  Agrego el reintento de 10 veces en obtenercajamaxima en caso de error
'                  En caso de null obtenercajamaxima no devuelve mas error, devuelve 0 en caso de error reintento.
' 1.1.2.5 18/09/20 Me pide poner una fecha y usar de vez en cuando la fecha que pasan y de vez en cuando la fecha que quiere el operario
'                  Agrego un chkbox y un textbox para poner una fecha deseada.
'                  A partir de ahora voy a calcular yo la fecha de vencimiento y el lote 
' 1.1.2.6 10/12/20 Agrego habilitar por impresora.
' 1.1.2.7 18/03/21 Agrego algunos logs.
' 1.1.2.8 31/03/21  Cambio composicion del codigo de barras paso de 7 digitos de codigo de producto a 5 y agrego el año que antes no estaba 

' 1.2.0.0

Public Class frmMain
    Dim objec As Object = New Object()
    Dim WithEvents PLC As Libnodave_WinAC
    Private usuario As New Usuario
    Private ImpresoraPrimaria(9) As Impresora
    Private ImpresoraSecundaria(9) As Impresora
    Private ImpresoraActual(9) As Boolean           ' si es true primaria si es false secundaria
    Private contadordespuesdebajoinsumo(1, 9) As Integer
    Private pistonafuera(1, 9) As Boolean
    Private frenoarriba(9) As Boolean
    Public Shared swlineas(9) As Stopwatch
    Private miLinea As Integer = 0
    Dim listaordenes As List(Of Mercaderia)
    Dim listaordenes1 As List(Of Mercaderia)
    Dim listacajas As List(Of caja)
    Public Shared HAY_CAJA(9) As Boolean
    Public Shared codigo_caja(9) As Integer
    Private tcpThd As Thread
    Delegate Sub SetTextCallback()
    Dim inicio As Boolean = False
    Private tiempo As Integer = 0
    Dim WithEvents impresora_tcp As New ketan.ketan.Cliente
    Dim datetimenaw As DateTime
    Dim datetimeanterior As DateTime
    Dim iniciado As Boolean
    ' Private noimprime(2) As Boolean

#Region "GENERAL"
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Logs.nuevo("INICIO")
        lblVersion.Text = "ETIQUETADO OFFAL V" + Me.ProductVersion
        Try
            levantarOpciones()
        Catch ex As Exception

        End Try
        If gsParam.HayConfigPrevia = False Then
            Dim config As New frmConfig
            config.ShowDialog()
        End If

        cmdDetener.Enabled = False
        Cargar_Ordenes()

        Cargar_Cajas(1, lvcajas1)
        Cargar_Cajas(2, lvcajas2)
        Cargar_Cajas(3, lvcajas3)
        Cargar_Cajas(5, lvcajas5)
        Cargar_Cajas(6, lvcajas6)
        Cargar_Cajas(7, lvcajas7)
        Cargar_Cajas(8, lvcajas8)
        Cargar_Cajas(9, lvcajas9)
        Cargar_Cajas(10, lvcajas10)

        ''rbTodas.Checked = True

        CheckForIllegalCrossThreadCalls = False
        rbEmp31.Checked = True
        rbEmp21.Checked = True
        rbEmp11.Checked = True
        rbEmp51.Checked = True
        rbEmp61.Checked = True
        rbEmp71.Checked = True
        rbEmp81.Checked = True
        rbEmp91.Checked = True
        rbEmp101.Checked = True

        If gsParam.LetraCodBar = "D" Then
            rbTurnodia.Checked = True
        ElseIf gsParam.LetraCodBar = "N" Then
            rbTurnoN.Checked = True
        Else
            rbTurnodia.Checked = True
            gsParam.LetraCodBar = "D"
            Call SaveSetting("KTNO", "Options", "sLetraCodBar", gsParam.LetraCodBar)
        End If


    End Sub

    Private Sub rbTurnodia_CheckedChanged(sender As Object, e As EventArgs) Handles rbTurnodia.CheckedChanged
        If rbTurnodia.Checked Then
            Logs.nuevo("el usuario selecciono turno dia")
            gsParam.LetraCodBar = "D"
            Call SaveSetting("KTNO", "Options", "sLetraCodBar", gsParam.LetraCodBar)
        End If
    End Sub



    Private Sub rbTurnoN_CheckedChanged(sender As Object, e As EventArgs) Handles rbTurnoN.CheckedChanged
        If rbTurnoN.Checked Then
            Logs.nuevo("el usuario selecciono turno noche")
            gsParam.LetraCodBar = "N"
            Call SaveSetting("KTNO", "Options", "sLetraCodBar", gsParam.LetraCodBar)
        End If
    End Sub
    Public Sub levantarOpciones()
        gsParam.ConnSQL = GetSetting("KTNO", "Options", "sConnSQL", "data source=RODRIGO-BANGHO;initial catalog=OFFAL;integrated security=SSPI")
        gsParam.UbicacionLogs = GetSetting("KTNO", "Options", "sUbicacionLogs", Directory.GetCurrentDirectory)
        gsParam.HayConfigPrevia = GetSetting("KTNO", "Options", "sHayConfigPrevia", "0")
        gsParam.IPPLC = GetSetting("KTNO", "Options", "sIPPLC", "192.168.0.200")

        gsParam.DriverImpresoraPrincipal(0) = GetSetting("KTNO", "Options", "DriverImpresoraPrincipal1", "GENERICO")
        gsParam.DriverImpresoraPrincipal(1) = GetSetting("KTNO", "Options", "DriverImpresoraPrincipal2", "GENERICO")
        gsParam.DriverImpresoraPrincipal(2) = GetSetting("KTNO", "Options", "DriverImpresoraPrincipal3", "GENERICO")
        gsParam.DriverImpresoraPrincipal(4) = GetSetting("KTNO", "Options", "DriverImpresoraPrincipal5", "GENERICO")
        gsParam.DriverImpresoraPrincipal(5) = GetSetting("KTNO", "Options", "DriverImpresoraPrincipal6", "GENERICO")
        gsParam.DriverImpresoraPrincipal(6) = GetSetting("KTNO", "Options", "DriverImpresoraPrincipal7", "GENERICO")
        gsParam.DriverImpresoraPrincipal(7) = GetSetting("KTNO", "Options", "DriverImpresoraPrincipal8", "GENERICO")
        gsParam.DriverImpresoraPrincipal(8) = GetSetting("KTNO", "Options", "DriverImpresoraPrincipal9", "GENERICO")
        gsParam.DriverImpresoraPrincipal(9) = GetSetting("KTNO", "Options", "DriverImpresoraPrincipal10", "GENERICO")

        gsParam.DriverImpresoraSecundaria(0) = GetSetting("KTNO", "Options", "DriverImpresoraSecundaria1", "GENERICO")
        gsParam.DriverImpresoraSecundaria(1) = GetSetting("KTNO", "Options", "DriverImpresoraSecundaria2", "GENERICO")
        gsParam.DriverImpresoraSecundaria(2) = GetSetting("KTNO", "Options", "DriverImpresoraSecundaria3", "GENERICO")
        gsParam.DriverImpresoraSecundaria(4) = GetSetting("KTNO", "Options", "DriverImpresoraSecundaria5", "GENERICO")
        gsParam.DriverImpresoraSecundaria(5) = GetSetting("KTNO", "Options", "DriverImpresoraSecundaria6", "GENERICO")
        gsParam.DriverImpresoraSecundaria(6) = GetSetting("KTNO", "Options", "DriverImpresoraSecundaria7", "GENERICO")
        gsParam.DriverImpresoraSecundaria(7) = GetSetting("KTNO", "Options", "DriverImpresoraSecundaria8", "GENERICO")
        gsParam.DriverImpresoraSecundaria(8) = GetSetting("KTNO", "Options", "DriverImpresoraSecundaria9", "GENERICO")
        gsParam.DriverImpresoraSecundaria(9) = GetSetting("KTNO", "Options", "DriverImpresoraSecundaria10", "GENERICO")

        gsParam.LetraCodBar = GetSetting("KTNO", "Options", "sLetraCodBar", "D")
        gsParam.Tandem = GetSetting("KTNO", "Options", "sTandem", "5000")
        gsParam.CantidadDespuesBajoInsumo = GetSetting("KTNO", "Options", "sCantidadDespuesBajoInsumo", "100")
        gsParam.Deshablin1(0) = GetSetting("KTNO", "Options", "sDeshablin1", "0")
        gsParam.Deshablin1(1) = GetSetting("KTNO", "Options", "sDeshablin2", "0")
        gsParam.Deshablin1(2) = GetSetting("KTNO", "Options", "sDeshablin3", "0")
        gsParam.Deshablin1(4) = GetSetting("KTNO", "Options", "sDeshablin5", "0")
        gsParam.Deshablin1(5) = GetSetting("KTNO", "Options", "sDeshablin6", "0")
        gsParam.Deshablin1(6) = GetSetting("KTNO", "Options", "sDeshablin7", "0")
        gsParam.Deshablin1(7) = GetSetting("KTNO", "Options", "sDeshablin8", "0")
        gsParam.Deshablin1(8) = GetSetting("KTNO", "Options", "sDeshablin9", "0")
        gsParam.Deshablin1(9) = GetSetting("KTNO", "Options", "sDeshablin10", "0")

        gsParam.DeshabImp(0, 0) = GetSetting("KTNO", "Options", "sDeshabImp11", "0")
        gsParam.DeshabImp(0, 1) = GetSetting("KTNO", "Options", "sDeshabImp12", "0")
        gsParam.DeshabImp(1, 0) = GetSetting("KTNO", "Options", "sDeshabImp21", "0")
        gsParam.DeshabImp(1, 1) = GetSetting("KTNO", "Options", "sDeshabImp22", "0")
        gsParam.DeshabImp(2, 0) = GetSetting("KTNO", "Options", "sDeshabImp31", "0")
        gsParam.DeshabImp(2, 1) = GetSetting("KTNO", "Options", "sDeshabImp32", "0")
        gsParam.DeshabImp(4, 0) = GetSetting("KTNO", "Options", "sDeshabImp51", "0")
        gsParam.DeshabImp(4, 1) = GetSetting("KTNO", "Options", "sDeshabImp52", "0")
        gsParam.DeshabImp(5, 0) = GetSetting("KTNO", "Options", "sDeshabImp61", "0")
        gsParam.DeshabImp(5, 1) = GetSetting("KTNO", "Options", "sDeshabImp62", "0")
        gsParam.DeshabImp(6, 0) = GetSetting("KTNO", "Options", "sDeshabImp71", "0")
        gsParam.DeshabImp(6, 1) = GetSetting("KTNO", "Options", "sDeshabImp72", "0")
        gsParam.DeshabImp(7, 0) = GetSetting("KTNO", "Options", "sDeshabImp81", "0")
        gsParam.DeshabImp(7, 1) = GetSetting("KTNO", "Options", "sDeshabImp82", "0")
        gsParam.DeshabImp(8, 0) = GetSetting("KTNO", "Options", "sDeshabImp91", "0")
        gsParam.DeshabImp(8, 1) = GetSetting("KTNO", "Options", "sDeshabImp92", "0")
        gsParam.DeshabImp(9, 0) = GetSetting("KTNO", "Options", "sDeshabImp101", "0")
        gsParam.DeshabImp(9, 1) = GetSetting("KTNO", "Options", "sDeshabImp102", "0")

    End Sub
    Private Sub frmMain_Closing(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        ' Determine if text has changed in the textbox by comparing to original text.
        If inicio Then
            ' Display a MsgBox asking the user to save changes or abort.
            MsgBox("No se puede cerrar el programa cuando se inició el ciclo", vbInformation, True)
            e.Cancel = True
        Else

        End If
    End Sub 'Form1_Closing
    Public Sub escribirMensaje(Msj As String, Optional mostrarHora As Boolean = False, Optional limpiarAnterior As Boolean = False)
        Dim hora As String
        Dim mensaje As String
        If limpiarAnterior = True Then

            lblInmediato.Items.Clear()
        End If
        If mostrarHora Then
            hora = DateTime.Now.Hour.ToString.PadLeft(2, "0") + ":" + DateTime.Now.Minute.ToString.PadLeft(2, "0") + ":" + DateTime.Now.Second.ToString.PadLeft(2, "0")
            mensaje = Msj & " - " & hora
        Else
            mensaje = Msj
        End If

        lblInmediato.Items.Add(mensaje)
        lblInmediato.TopIndex = lblInmediato.Items.Count - 1
    End Sub
    Public Sub escribirMensaje(Msj As String, linea As Integer, Optional mostrarHora As Boolean = False, Optional limpiarAnterior As Boolean = False)
        Dim hora As String
        Dim mensaje As String
        If limpiarAnterior = True Then

            Select Case (linea)
                Case 0
                    ListBox1.Items.Clear()
                Case 1
                    ListBox2.Items.Clear()
                Case 2
                    ListBox3.Items.Clear()
                Case 4
                    ListBox5.Items.Clear()
                Case 5
                    ListBox6.Items.Clear()
                Case 6
                    ListBox7.Items.Clear()
                Case 7
                    ListBox8.Items.Clear()
                Case 8
                    ListBox9.Items.Clear()
                Case 9
                    ListBox10.Items.Clear()
            End Select
            ''lblInmediato.Items.Clear()
        End If
        If mostrarHora Then
            hora = DateTime.Now.Hour.ToString.PadLeft(2, "0") + ":" + DateTime.Now.Minute.ToString.PadLeft(2, "0") + ":" + DateTime.Now.Second.ToString.PadLeft(2, "0")
            mensaje = Msj & " - " & hora
        Else
            mensaje = Msj
        End If
        Select Case (linea)
            Case 0
                ListBox1.Items.Add(mensaje)
                ListBox1.TopIndex = ListBox1.Items.Count - 1
            Case 1
                ListBox2.Items.Add(mensaje)
                ListBox2.TopIndex = ListBox2.Items.Count - 1
            Case 2
                ListBox3.Items.Add(mensaje)
                ListBox3.TopIndex = ListBox3.Items.Count - 1
            Case 4
                ListBox5.Items.Add(mensaje)
                ListBox5.TopIndex = ListBox5.Items.Count - 1
            Case 5
                ListBox6.Items.Add(mensaje)
                ListBox6.TopIndex = ListBox6.Items.Count - 1
            Case 6
                ListBox7.Items.Add(mensaje)
                ListBox7.TopIndex = ListBox7.Items.Count - 1
            Case 7
                ListBox8.Items.Add(mensaje)
                ListBox8.TopIndex = ListBox8.Items.Count - 1
            Case 8
                ListBox9.Items.Add(mensaje)
                ListBox9.TopIndex = ListBox9.Items.Count - 1
            Case 9
                ListBox10.Items.Add(mensaje)
                ListBox10.TopIndex = ListBox10.Items.Count - 1
        End Select

    End Sub
    Private Sub cmdConfig_Click_1(sender As Object, e As EventArgs) Handles cmdConfig.Click
        If usuario.Logueado And usuario.Acceso.TrimEnd(" ") = "Administrador" Then
            Dim config As New frmConfig
            config.ShowDialog()
        Else
            Call MessageBox.Show("Debe loguearse como administrador para modificar la configuración")
        End If
    End Sub
    Private Sub cmdLoguearDesloguear_Click_1(sender As Object, e As EventArgs) Handles cmdLoguearDesloguear.Click
        Dim dr As DialogResult
        If usuario.Logueado = False Then
            Dim frm As New frmlogin(usuario)
            dr = frm.ShowDialog
            If dr = Windows.Forms.DialogResult.OK Then
                lblUsuario.Text = usuario.Nombre
                cmdLoguearDesloguear.Text = "Desconectarse"
                If usuario.Acceso.Equals("Operador") Then
                    cmdConfig.Enabled = False
                Else
                    cmdConfig.Enabled = True
                End If
            End If
        Else
            lblUsuario.Text = "Desconectado"
            cmdLoguearDesloguear.Text = "Login"
            usuario.desloguearUsuario()
        End If
    End Sub
    Private Sub cmdSalir_Click(sender As Object, e As EventArgs) Handles cmdSalir.Click
        If inicio Then
            escribirMensaje("No se puede cerrar el programa cuando se inició el ciclo")
            Exit Sub
        End If
        Me.Dispose()
    End Sub
    Private Sub tmrHora_Tick(sender As Object, e As EventArgs) Handles tmrHora.Tick
        If tiempo = 0 Then
            tiempo = 60
            lblFechaYHora.Text = "Fecha: " + DateTime.Now.Day.ToString.PadLeft(2, "0") + "/" + DateTime.Now.Month.ToString.PadLeft(2, "0") + "/" + DateTime.Now.Year.ToString + "    Hora: " + DateTime.Now.Hour.ToString.PadLeft(2, "0") + ":" + DateTime.Now.Minute.ToString.PadLeft(2, "0")
        Else
            tiempo -= 1
        End If
    End Sub
    Private Sub cmdDetener_Click(sender As Object, e As EventArgs) Handles cmdDetener.Click
        Logs.nuevo("el usuario detuvo manualmente")
        detener()
    End Sub
    Private Sub cmdInicioEtiquetado_Click(sender As Object, e As EventArgs) Handles cmdInicioEtiquetado.Click
        Logs.nuevo("el usuario inicio manualmente")
        iniciar()
    End Sub
    Private Sub dtpFechaProduccion_ValueChanged(sender As Object, e As EventArgs) Handles dtpFechaProduccion.ValueChanged
        If dtpFechaProduccion.Checked = True Then


            lblFechaYHora.BackColor = Color.GreenYellow
            Dim fechaprod As Date = dtpFechaProduccion.Value
            Logs.nuevo("el usuario " + usuario.Nombre + " seleccionó fecha de produccion manual: " + fechaprod.Day.ToString.PadLeft(2, "0") + "-" + fechaprod.Month.ToString.PadLeft(2, "0") + "-" + fechaprod.Year.ToString.PadLeft(4, "0"))
            'MsgBox(fechaprod.Day.ToString.PadLeft(2, "0") + "-" + fechaprod.Month.ToString.PadLeft(2, "0") + "-" + fechaprod.Year.ToString.PadLeft(4, "0"))
            'MsgBox((fechaprod.AddMonths(18)).Day.ToString.PadLeft(2, "0") + "-" + (fechaprod.AddMonths(18)).Month.ToString.PadLeft(2, "0") + "-" + (fechaprod.AddMonths(18)).Year.ToString.PadLeft(4, "0"))
            'MsgBox(fechaprod.Year.ToString.PadLeft(4, "0") + fechaprod.Month.ToString.PadLeft(2, "0") + fechaprod.Day.ToString.PadLeft(2, "0"))
            'Dim v As String = "100"
            'Dim f As String = "18-09-2020"
            ''Dim a As Date = dtpFechaProduccion.Value
            'MsgBox((Convert.ToDateTime(f)).AddDays(Convert.ToInt32(v)).ToShortDateString)
            'MsgBox((Convert.ToDateTime(f)).Year.ToString.PadLeft(4, "0") + (Convert.ToDateTime(f)).Month.ToString.PadLeft(2, "0") + (Convert.ToDateTime(f)).Day.ToString.PadLeft(2, "0"))
        Else
            Logs.nuevo("el usuario " + usuario.Nombre + " dejo fecha de produccion automatica")
            lblFechaYHora.BackColor = Color.Transparent
        End If
        'lblFechaYHora.BackColor = Color.GreenYellow
    End Sub
#End Region
#Region "MERCADERIA"
    Private Sub cmdActualizar_Click(sender As Object, e As EventArgs) Handles cmdActualizar.Click
        Logs.nuevo("el usuario actualizo mercaderia")
        Cargar_Ordenes()
    End Sub
    Private Sub Cargar_Ordenes()
        Repositorio.Actulizarpedido()

        lvOrdenes.Clear()
        lvOrdenes.Columns.Add("Mercaderia")
        lvOrdenes.Columns.Item(0).Width = 120
        lvOrdenes.Columns.Add("Código caja")
        lvOrdenes.Columns.Item(0).Width = 120
        lvOrdenes.Columns.Add("Descripción")
        lvOrdenes.Columns.Item(1).Width = 240
        lvOrdenes.Columns.Add("Código")
        lvOrdenes.Columns.Item(2).Width = 120
        lvOrdenes.Columns.Add("ICantFajas")
        lvOrdenes.Columns.Item(3).Width = 120
        lvOrdenes.Columns.Add("Presentacion ")
        lvOrdenes.Columns.Item(4).Width = 120
        lvOrdenes.Columns.Add("Etiqueta")
        lvOrdenes.Columns.Item(5).Width = 120

        lvOrdenes.FullRowSelect = True
        lvOrdenes.MultiSelect = False
        lvOrdenes.Scrollable = True
        lvOrdenes.View = View.Details
        listaordenes = Repositorio.obtenertodaslasordenes()

        For Each orden In listaordenes

            Dim item As New ListViewItem(
                {
                orden.Mercaderia.ToString,
                orden.CodCaja.ToString,
                orden.Descripcion,
                orden.Codigo,
                orden.ICantFajas.ToString,
                orden.Presentacion, orden.Etiqueta})
            lvOrdenes.Items.Add(item)
        Next

    End Sub
    Private Sub cmdCargarPLC_Click(sender As Object, e As EventArgs) Handles cmdCargarPLC.Click
        'a pedido de ellos hago todo el proceso automatico y siempre mantengo activo el cargado del listado 
        Logs.nuevo("el usuario cargo mercaderia en el plc")
        Dim sistiniciado As Boolean = False 'identifico si esta iniciado o no para ponerlo en automatico nuevamente

        If inicio Then
            detener()
            sistiniciado = True
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If conectarPLC() Then
            escribirMensaje("Se conectó correctamente el PLC en la IP: " + gsParam.IPPLC)
            Logs.nuevo("Se conectó correctamente el PLC en la IP: " + gsParam.IPPLC)
        Else
            MessageBox.Show("Error conectando con el PLC. No se iniciará el etiquetado")
            Exit Sub
        End If
        escribirConfigPLC()
        PLC.Desconectar()


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If sistiniciado Then
            iniciar()
            sistiniciado = False
        End If



    End Sub
    Public Sub escribirConfigPLC()
        'Para escribir el dato en 1°byte = 0; 2°byte = 0; 3°byte = 1 o 0; 4°byte = valor - 256

        listaordenes = Repositorio.obtenertodaslasordenes()
        Dim n As Integer = 0 'cantidad del listado 
        Dim j As Integer = 114 'direccion
        For Each orden In listaordenes
            n += 1
            Dim i As Integer = 0
            PLC.BufferEscritura(i) = 0
            i += 1
            PLC.BufferEscritura(i) = 0
            i += 1
            If orden.Mercaderia Mod 256 >= 1 Then
                PLC.BufferEscritura(i) = Convert.ToInt32(Math.Truncate(orden.Mercaderia / 256))
                i += 1
                PLC.BufferEscritura(i) = Math.Abs(orden.Mercaderia Mod 256)
                i += 1
            Else
                PLC.BufferEscritura(i) = 0
                i += 1
                PLC.BufferEscritura(i) = orden.Mercaderia
                i += 1
            End If
            PLC.BufferEscritura(i) = 32
            i += 1
            PLC.BufferEscritura(i) = 32
            i += 1

            Dim vectordebytesdelstring As Byte()
            vectordebytesdelstring = System.Text.Encoding.Unicode.GetBytes(orden.Codigo.ToString + orden.Descripcion)
            For Each a As Char In (orden.Codigo.ToString + " " + orden.Descripcion)
                PLC.BufferEscritura(i) = Asc(a)
                i += 1
            Next


            Do
                PLC.BufferEscritura(i) = 32
                i += 1
                If i = 105 Then
                    Exit Do

                End If
            Loop
            i += 1
            Dim codCajaValue As Integer = orden.CodCaja
            'Logs.nuevo("codcaja: " + codCajaValue.ToString())
            Dim sintValue As SByte = Convert.ToSByte(Math.Max(-128, Math.Min(127, codCajaValue)))
            PLC.BufferEscritura(i) = sintValue
            i += 1
            PLC.BufferEscritura(i) = 32


            PLC.EscribirBytesDB(400, j, i)
            j += 114
        Next
        While n <= 99
            n += 1
            Dim i As Integer = 0
            PLC.BufferEscritura(i) = 0
            i += 1
            PLC.BufferEscritura(i) = 0
            i += 1

            PLC.BufferEscritura(i) = 0
            i += 1
            PLC.BufferEscritura(i) = 0
            i += 1

            PLC.BufferEscritura(i) = 32
            i += 1
            PLC.BufferEscritura(i) = 32
            i += 1

            Dim vectordebytesdelstring As Byte()
            vectordebytesdelstring = System.Text.Encoding.Unicode.GetBytes(" ")
            For Each a As Char In (" ")
                PLC.BufferEscritura(i) = Asc(a)
                i += 1
            Next


            Do
                PLC.BufferEscritura(i) = 32
                i += 1
                If i = 105 Then
                    Exit Do

                End If
            Loop

            PLC.EscribirBytesDB(400, j, i)
            j += 114
        End While
    End Sub

#End Region
#Region "LINEAS"
    Private Sub Cargar_Cajas(linea As Integer, cajas As ListView)
        cajas.Clear()
        cajas.Columns.Add("Código de barras")
        cajas.Columns.Item(0).Width = 140
        cajas.Columns.Add("Fecha")
        cajas.Columns.Item(1).Width = 120

        cajas.Columns.Add("Descripción")
        cajas.Columns.Item(2).Width = 120
        cajas.Columns.Add("Mercaderia")
        cajas.Columns.Item(3).Width = 120
        cajas.Columns.Add("Linea")
        cajas.Columns.Item(4).Width = 120
        cajas.Columns.Add("ETIQUETA")
        cajas.Columns.Item(5).Width = 120

        cajas.FullRowSelect = True
        cajas.MultiSelect = False
        cajas.Scrollable = True
        cajas.View = View.Details
        listacajas = Repositorio.obtenertodalascajas(linea)
        For Each caja In listacajas
            Try
                Dim item As New ListViewItem({caja.Codbar.ToString, caja.HoraFecha.ToShortDateString, caja.Descripcion, caja.Mercaderia.ToString, caja.Linea.ToString, caja.Etiqueta})
                cajas.Items.Add(item)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        Next

    End Sub
    'Private Sub rbL1_CheckedChanged(sender As Object, e As EventArgs)
    '    If rbL1.Checked Then
    '        Cargar_Cajas(1)
    '    End If
    'End Sub
    'Private Sub rbL2_CheckedChanged(sender As Object, e As EventArgs)
    '    If rbL2.Checked Then
    '        Cargar_Cajas(2)
    '    End If
    'End Sub
    'Private Sub rbL3_CheckedChanged(sender As Object, e As EventArgs)
    '    If rbL3.Checked Then
    '        Cargar_Cajas(3)
    '    End If
    'End Sub
    'Private Sub rbTodas_CheckedChanged(sender As Object, e As EventArgs)
    '    If rbTodas.Checked Then
    '        Cargar_Cajas(0)
    '    End If
    'End Sub
    Public Sub actualizardatosenpantalla(cajactual As caja, linea As Integer)
        If linea = 0 Then
            lblMerc1.Text = cajactual.Mercaderia.ToString
            lblCod1.Text = cajactual.Codigo.ToString
            lblDesc1.Text = cajactual.Descripcion.ToString
            If ImpresoraActual(linea) Then
                lblEtiqutadora11.Font = New Font("Consolas", 12, FontStyle.Underline Or FontStyle.Italic Or FontStyle.Bold)
                lblEtiqutadora12.Font = New Font("Consolas", 12, FontStyle.Italic Or FontStyle.Bold)
            Else
                lblEtiqutadora12.Font = New Font("Consolas", 12, FontStyle.Underline Or FontStyle.Italic Or FontStyle.Bold)
                lblEtiqutadora11.Font = New Font("Consolas", 12, FontStyle.Italic Or FontStyle.Bold)
            End If
        ElseIf linea = 1 Then
            lblMerc2.Text = cajactual.Mercaderia.ToString
            lblCod2.Text = cajactual.Codigo.ToString
            lblDesc2.Text = cajactual.Descripcion.ToString

            If ImpresoraActual(linea) Then
                lblEtiqutadora21.Font = New Font("Consolas", 12, FontStyle.Underline Or FontStyle.Italic Or FontStyle.Bold)
                lblEtiqutadora22.Font = New Font("Consolas", 12, FontStyle.Italic Or FontStyle.Bold)
            Else
                lblEtiqutadora22.Font = New Font("Consolas", 12, FontStyle.Underline Or FontStyle.Italic Or FontStyle.Bold)
                lblEtiqutadora21.Font = New Font("Consolas", 12, FontStyle.Italic Or FontStyle.Bold)
            End If
        ElseIf linea = 2 Then
            lblMerc3.Text = cajactual.Mercaderia.ToString
            lblCod3.Text = cajactual.Codigo.ToString
            lblDesc3.Text = cajactual.Descripcion.ToString

            If ImpresoraActual(linea) Then
                lblEtiqutadora31.Font = New Font("Consolas", 12, FontStyle.Underline Or FontStyle.Italic Or FontStyle.Bold)
                lblEtiqutadora32.Font = New Font("Consolas", 12, FontStyle.Italic Or FontStyle.Bold)
            Else
                lblEtiqutadora32.Font = New Font("Consolas", 12, FontStyle.Underline Or FontStyle.Italic Or FontStyle.Bold)
                lblEtiqutadora31.Font = New Font("Consolas", 12, FontStyle.Italic Or FontStyle.Bold)
            End If
        ElseIf linea = 4 Then
            lblMerc5.Text = cajactual.Mercaderia.ToString
            lblCod5.Text = cajactual.Codigo.ToString
            lblDesc5.Text = cajactual.Descripcion.ToString

            If ImpresoraActual(linea) Then
                lblEtiqutadora51.Font = New Font("Consolas", 12, FontStyle.Underline Or FontStyle.Italic Or FontStyle.Bold)
                lblEtiqutadora52.Font = New Font("Consolas", 12, FontStyle.Italic Or FontStyle.Bold)
            Else
                lblEtiqutadora52.Font = New Font("Consolas", 12, FontStyle.Underline Or FontStyle.Italic Or FontStyle.Bold)
                lblEtiqutadora51.Font = New Font("Consolas", 12, FontStyle.Italic Or FontStyle.Bold)
            End If
        ElseIf linea = 5 Then
            lblMerc6.Text = cajactual.Mercaderia.ToString
            lblCod6.Text = cajactual.Codigo.ToString
            lblDesc6.Text = cajactual.Descripcion.ToString

            If ImpresoraActual(linea) Then
                lblEtiqutadora61.Font = New Font("Consolas", 12, FontStyle.Underline Or FontStyle.Italic Or FontStyle.Bold)
                lblEtiqutadora62.Font = New Font("Consolas", 12, FontStyle.Italic Or FontStyle.Bold)
            Else
                lblEtiqutadora62.Font = New Font("Consolas", 12, FontStyle.Underline Or FontStyle.Italic Or FontStyle.Bold)
                lblEtiqutadora61.Font = New Font("Consolas", 12, FontStyle.Italic Or FontStyle.Bold)
            End If
        ElseIf linea = 6 Then
            lblMerc7.Text = cajactual.Mercaderia.ToString
            lblCod7.Text = cajactual.Codigo.ToString
            lblDesc7.Text = cajactual.Descripcion.ToString

            If ImpresoraActual(linea) Then
                lblEtiqutadora71.Font = New Font("Consolas", 12, FontStyle.Underline Or FontStyle.Italic Or FontStyle.Bold)
                lblEtiqutadora72.Font = New Font("Consolas", 12, FontStyle.Italic Or FontStyle.Bold)
            Else
                lblEtiqutadora72.Font = New Font("Consolas", 12, FontStyle.Underline Or FontStyle.Italic Or FontStyle.Bold)
                lblEtiqutadora71.Font = New Font("Consolas", 12, FontStyle.Italic Or FontStyle.Bold)
            End If
        ElseIf linea = 7 Then
            lblMerc8.Text = cajactual.Mercaderia.ToString
            lblCod8.Text = cajactual.Codigo.ToString
            lblDesc8.Text = cajactual.Descripcion.ToString

            If ImpresoraActual(linea) Then
                lblEtiqutadora81.Font = New Font("Consolas", 12, FontStyle.Underline Or FontStyle.Italic Or FontStyle.Bold)
                lblEtiqutadora82.Font = New Font("Consolas", 12, FontStyle.Italic Or FontStyle.Bold)
            Else
                lblEtiqutadora82.Font = New Font("Consolas", 12, FontStyle.Underline Or FontStyle.Italic Or FontStyle.Bold)
                lblEtiqutadora81.Font = New Font("Consolas", 12, FontStyle.Italic Or FontStyle.Bold)
            End If
        ElseIf linea = 8 Then
            lblMerc9.Text = cajactual.Mercaderia.ToString
            lblCod9.Text = cajactual.Codigo.ToString
            lblDesc9.Text = cajactual.Descripcion.ToString

            If ImpresoraActual(linea) Then
                lblEtiqutadora91.Font = New Font("Consolas", 12, FontStyle.Underline Or FontStyle.Italic Or FontStyle.Bold)
                lblEtiqutadora92.Font = New Font("Consolas", 12, FontStyle.Italic Or FontStyle.Bold)
            Else
                lblEtiqutadora92.Font = New Font("Consolas", 12, FontStyle.Underline Or FontStyle.Italic Or FontStyle.Bold)
                lblEtiqutadora91.Font = New Font("Consolas", 12, FontStyle.Italic Or FontStyle.Bold)
            End If
        Else
            lblMerc10.Text = cajactual.Mercaderia.ToString
            lblCod10.Text = cajactual.Codigo.ToString
            lblDesc10.Text = cajactual.Descripcion.ToString
            If ImpresoraActual(linea) Then
                lblEtiqutadora101.Font = New Font("Consolas", 12, FontStyle.Underline Or FontStyle.Italic Or FontStyle.Bold)
                lblEtiqutadora102.Font = New Font("Consolas", 12, FontStyle.Italic Or FontStyle.Bold)
            Else
                lblEtiqutadora102.Font = New Font("Consolas", 12, FontStyle.Underline Or FontStyle.Italic Or FontStyle.Bold)
                lblEtiqutadora101.Font = New Font("Consolas", 12, FontStyle.Italic Or FontStyle.Bold)
            End If
        End If

    End Sub
#End Region
#Region "PROCESO"
    Private Sub threat() 'linea As Integer)
        ''datetimenaw = DateTime.Now

        '' listaordenes1 = Repositorio.obtenertodaslasordenes() 'lo pongo antes del do 
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Thread.Sleep(50)
        '  Logs.nuevo("Linea " + Linea.ToString + " abierta")
        While inicio
            'Thread.Sleep(10)
            Dim noimprime As Boolean = True
            Dim sw As New Stopwatch
            sw.Start()

            If gsParam.Deshablin1(miLinea) = False And (miLinea <> 3) Then

                'Verifico conectividad
                'Levanto marcas posibles de hay caja
                'Logs.nuevo("llego aca")
                If swlineas(miLinea).IsRunning And swlineas(miLinea).ElapsedMilliseconds < gsParam.Tandem Then 'And (frenoarriba(linea) Or pistonafuera(0, linea) Or pistonafuera(1, linea)) Then
                    'si el contador esta activo y el mismo no paso los 5 segundos
                    sw.Stop()
                    If swlineas(miLinea).ElapsedMilliseconds > ((gsParam.Tandem / 5) * 4) Then
                        Logs.nuevo("cambio de linea:" + swlineas(miLinea).ElapsedMilliseconds.ToString + " de la linea:  " + (miLinea + 1).ToString)
                    End If
                Else
                    'si el contador no esta activo o el mismo paso los 5 segundos
                    If swlineas(miLinea).IsRunning Then

                        leerPLC(miLinea)


                        If pistonafuera(0, miLinea) Or pistonafuera(1, miLinea) Or frenoarriba(miLinea) Then
                            Logs.nuevo("REVISAR PISTONES Y FRENO por linea " + (miLinea + 1).ToString)
                            escribirMensaje("REVISAR PISTONES Y FRENO por miLinea " + (miLinea + 1).ToString, miLinea)
                            If pistonafuera(0, miLinea) Then
                                Logs.nuevo("PISTON 1 AFUERA por linea " + (miLinea + 1).ToString)
                                escribirMensaje("PISTON 1 AFUERA por linea " + (miLinea + 1).ToString, miLinea)
                            End If
                            If pistonafuera(1, miLinea) Then
                                Logs.nuevo("PISTON 2 AFUERA por linea " + (miLinea + 1).ToString)
                                escribirMensaje("PISTON 2 AFUERA por linea " + (miLinea + 1).ToString, miLinea)
                            End If
                            If frenoarriba(miLinea) Then
                                Logs.nuevo("FRENO ARRIBA por linea " + (miLinea + 1).ToString)
                                escribirMensaje("FRENO ARRIBA por linea " + (miLinea + 1).ToString, miLinea)
                            End If
                            PLC.BufferLectura(miLinea) = Nothing

                            GoTo cambiolinea
                        End If
                        swlineas(miLinea).Stop()
                        Logs.nuevo("cambio de linea" + swlineas(miLinea).ElapsedMilliseconds.ToString)
                    End If
                    Dim PudoLeer As Boolean = False

                    PudoLeer = mObtener_Variables_PLC(miLinea)


                    'Logs.nuevo("pudo leer: " + PudoLeer.ToString)
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Dim cajaactual As caja = Nothing
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '1:Consulto si hay caja
                    If HAY_CAJA(miLinea) And PudoLeer Then
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Logs.nuevo("HAY CAJA: El ciclo de etiquetado tardo " + sw.ElapsedMilliseconds.ToString + " por linea " + (miLinea + 1).ToString)
                        escribirMensaje("Hay caja linea " + (miLinea + 1).ToString, miLinea)

                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '2: Genero etiqueta
                        For Each orden In listaordenes1
                            If codigo_caja(miLinea) = orden.Mercaderia Then
                                If dtpFechaProduccion.Checked Then
                                    SyncLock objec
                                        cajaactual = New caja(orden, miLinea, dtpFechaProduccion.Value)
                                    End SyncLock
                                Else
                                    SyncLock objec
                                        cajaactual = New caja(orden, miLinea)
                                    End SyncLock

                                End If
                                If cajaactual.Numero = 0 Then
                                    Cargar_Ordenes()
                                    cajaactual = Nothing
                                    listaordenes1 = Nothing
                                    listaordenes1 = Repositorio.obtenertodaslasordenes()
                                    For Each orden1 In listaordenes1
                                        If codigo_caja(miLinea) = orden1.Mercaderia Then
                                            If dtpFechaProduccion.Checked Then
                                                SyncLock objec
                                                    cajaactual = New caja(orden, miLinea, dtpFechaProduccion.Value)
                                                End SyncLock
                                            Else
                                                SyncLock objec
                                                    cajaactual = New caja(orden, miLinea)
                                                End SyncLock
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        Next
                        'If sw.ElapsedMilliseconds > 200 Then
                        Logs.nuevo("Buscar: El ciclo de etiquetado tardo " + sw.ElapsedMilliseconds.ToString + " por linea " + (miLinea + 1).ToString)
                        'escribirMensaje("Ciclo PLC " & sw.ElapsedMilliseconds.ToString, False)
                        'End If

                        If cajaactual Is Nothing Then
                            escribirMensaje("No existe codigo asociado a " + codigo_caja(miLinea).ToString, miLinea)
                            Logs.nuevo("No existe codigo asociado a " + codigo_caja(miLinea).ToString)
                            HAY_CAJA(miLinea) = False
                            PLC.Reset(miLinea)
                        Else
                            'If sw.ElapsedMilliseconds > 200 Then
                            Logs.nuevo("Crear:El ciclo de etiquetado tardo " + sw.ElapsedMilliseconds.ToString + " por linea " + (miLinea + 1).ToString)
                            'escribirMensaje("Ciclo PLC " & sw.ElapsedMilliseconds.ToString, False)
                            'End If


                            escribirMensaje("Pertenece a:" + cajaactual.Descripcion.ToString, miLinea)
                            actualizardatosenpantalla(cajaactual, miLinea)
                            Logs.nuevo("Numero de caja" + cajaactual.Numero.ToString)
                            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            '3: Mando a imprimir 
                            'If cajaactual.ICantFajas <= 2 Then
                            'ImpresoraPrimaria(linea).imprimir(cajaactual)
                            'End If
                            'ImpresoraPrimaria(linea).imprimir(cajaactual)
                            'If cajaactual.ICantFajas >= 1 Then
                            'ImpresoraSecundaria(linea).imprimir1(cajaactual, usuario)
                            'End If
                            'If sw.ElapsedMilliseconds > 200 Then
                            'Logs.nuevo("Imprimir: El ciclo de etiquetado tardo " + sw.ElapsedMilliseconds.ToString + " por linea " + (linea + 1).ToString)
                            'escribirMensaje("Ciclo PLC " & sw.ElapsedMilliseconds.ToString, False)
                            'End If
                            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            '3: Mando a imprimir 
                            'TANDEM 
                            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            'control de variables en hay caja
                            If PLC.BufferLectura(miLinea)(6) = 1 And PLC.BufferLectura(miLinea)(8) = 0 Then
                                Logs.nuevo("Zebra 1 online linea " + (miLinea + 1).ToString)
                            ElseIf PLC.BufferLectura(miLinea)(6) = 0 And PLC.BufferLectura(miLinea)(8) = 0 Then
                                Logs.nuevo("Zebra 1 ofline linea " + (miLinea + 1).ToString)
                            ElseIf PLC.BufferLectura(miLinea)(6) = 0 And PLC.BufferLectura(miLinea)(8) = 1 Then
                                Logs.nuevo("Zebra 1 ofline linea " + (miLinea + 1).ToString)
                            Else
                                Logs.nuevo("Zebra 1 ofline linea " + (miLinea + 1).ToString)
                            End If
                            If PLC.BufferLectura(miLinea)(13) = 1 And PLC.BufferLectura(miLinea)(15) = 0 Then
                                Logs.nuevo("Zebra 2 online linea " + (miLinea + 1).ToString)
                            ElseIf PLC.BufferLectura(miLinea)(13) = 0 And PLC.BufferLectura(miLinea)(15) = 0 Then
                                Logs.nuevo("Zebra 2 ofline linea " + (miLinea + 1).ToString)
                            ElseIf PLC.BufferLectura(miLinea)(13) = 0 And PLC.BufferLectura(miLinea)(15) = 1 Then
                                Logs.nuevo("Zebra 2 ofline linea " + (miLinea + 1).ToString)
                            Else
                                Logs.nuevo("Zebra 2 ofline linea " + (miLinea + 1).ToString)
                            End If
                            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            'para impresora principal

                            If ImpresoraActual(miLinea) And PLC.BufferLectura(miLinea)(6) = 1 And PLC.BufferLectura(miLinea)(8) = 0 Then
                                Logs.nuevo("Ingresa etiquetadora 1")
                                'si esta activa la primaria y esta online 
                                If PLC.BufferLectura(miLinea)(7) = 1 Then
                                    'si hay bajo insumo me fijo si llego al tope de bajo insumo
                                    If contadordespuesdebajoinsumo(0, miLinea) >= gsParam.CantidadDespuesBajoInsumo Then
                                        'si llego al tope de bajo insumo Me fijo si esta online la secundaria 
                                        'IMPORTANTE: no deberia llegar nunca aca a no ser que no se halla reseteado el contador
                                        'qImpresoraPrimaria(linea).imprimir(cajaactual)
                                        If PLC.BufferLectura(miLinea)(13) = 1 And PLC.BufferLectura(miLinea)(15) = 0 Then
                                            ' si esta online la secundaria paso a la secundaria y limpio bufer de la primaria
                                            Logs.nuevo("CAMBIO DE ETIQUETADORA")
                                            ImpresoraActual(miLinea) = False
                                            SyncLock objec

                                                imprimirTCP(cajaactual, gsParam.DriverImpresoraPrincipal(miLinea))
                                            End SyncLock

                                            'ImpresoraPrimaria(linea).imprimir(cajaactual)
                                            'ImpresoraPrimaria(linea).LimpiarBuffer()
                                            swlineas(miLinea) = New Stopwatch
                                            swlineas(miLinea).Start()
                                            contadordespuesdebajoinsumo(0, miLinea) = 0
                                            'ImpresoraSecundaria(linea).imprimir(cajaactual)
                                        Else
                                            'sigo hasta que reviento 
                                            SyncLock objec
                                                imprimirTCP(cajaactual, gsParam.DriverImpresoraPrincipal(miLinea))
                                            End SyncLock

                                            ' ImpresoraPrimaria(linea).imprimir(cajaactual)
                                        End If

                                    Else
                                        'si no llego al tope de bajo insumo imprimo e incremento el contador 
                                        contadordespuesdebajoinsumo(0, miLinea) += 1
                                        SyncLock objec
                                            imprimirTCP(cajaactual, gsParam.DriverImpresoraPrincipal(miLinea))
                                        End SyncLock

                                        'ImpresoraPrimaria(linea).imprimir(cajaactual)
                                        'Me fijo si llega al tope 
                                        If contadordespuesdebajoinsumo(0, miLinea) = gsParam.CantidadDespuesBajoInsumo Then
                                            'si llega al tope me fijo si la otra esta habilitada 
                                            If PLC.BufferLectura(miLinea)(13) = 1 And PLC.BufferLectura(miLinea)(15) = 0 Then
                                                ' si la otra esta habilitada hago el cambio y la pongo en espera
                                                Logs.nuevo("CAMBIO DE ETIQUETADORA")
                                                ImpresoraActual(miLinea) = False
                                                'ImpresoraPrimaria(linea).LimpiarBuffer()
                                                swlineas(miLinea) = New Stopwatch
                                                swlineas(miLinea).Start()
                                                contadordespuesdebajoinsumo(0, miLinea) = 0
                                            End If
                                        End If
                                    End If
                                Else
                                    'si no esta bajo insumo procedo normal
                                    SyncLock objec
                                        imprimirTCP(cajaactual, gsParam.DriverImpresoraPrincipal(miLinea))
                                    End SyncLock

                                    'ImpresoraPrimaria(linea).imprimir(cajaactual)
                                End If


                                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                'impresora secundaria 
                            ElseIf ImpresoraActual(miLinea) = False And PLC.BufferLectura(miLinea)(13) = 1 And PLC.BufferLectura(miLinea)(15) = 0 Then ' zebra online y zebra error
                                Logs.nuevo("Ingresa etiquetadora 2")
                                'si esta activa la primaria y esta online 
                                If PLC.BufferLectura(miLinea)(14) = 1 Then
                                    'si hay bajo insumo me fijo si llego al tope de bajo insumo
                                    If contadordespuesdebajoinsumo(1, miLinea) >= gsParam.CantidadDespuesBajoInsumo Then
                                        'si llego al tope de bajo insumo Me fijo si esta online la secundaria 
                                        'ImpresoraSecundaria(linea).imprimir(cajaactual)
                                        'IMPORTANTE: no deberia llegar nunca aca a no ser que no se halla reseteado el contador o que la otra este offline
                                        If PLC.BufferLectura(miLinea)(6) = 1 And PLC.BufferLectura(miLinea)(8) = 0 Then
                                            ' si esta online la secundaria paso a la primaria y limpio bufer de la primaria

                                            Logs.nuevo("CAMBIO DE ETIQUETADORA")
                                            ImpresoraActual(miLinea) = True
                                            SyncLock objec
                                                imprimirTCP(cajaactual, gsParam.DriverImpresoraSecundaria(miLinea))
                                            End SyncLock

                                            'ImpresoraSecundaria(linea).imprimir(cajaactual)
                                            'ImpresoraSecundaria(linea).LimpiarBuffer()
                                            swlineas(miLinea) = New Stopwatch
                                            swlineas(miLinea).Start()
                                            contadordespuesdebajoinsumo(1, miLinea) = 0
                                            'ImpresoraSecundaria(linea).imprimir(cajaactual)
                                        Else
                                            'sigo hasta que reviento 
                                            SyncLock objec
                                                imprimirTCP(cajaactual, gsParam.DriverImpresoraSecundaria(miLinea))
                                            End SyncLock

                                            'ImpresoraSecundaria(linea).imprimir(cajaactual)
                                        End If

                                    Else
                                        'si no llego al tope de bajo insumo imprimo e incremento el contador 
                                        contadordespuesdebajoinsumo(1, miLinea) += 1
                                        SyncLock objec
                                            imprimirTCP(cajaactual, gsParam.DriverImpresoraSecundaria(miLinea))
                                        End SyncLock
                                        ' ImpresoraSecundaria(linea).imprimir(cajaactual)
                                        'Me fijo si llega al tope 
                                        If contadordespuesdebajoinsumo(1, miLinea) = gsParam.CantidadDespuesBajoInsumo Then
                                            'si llega al tope me fijo si la otra esta habilitada 
                                            If PLC.BufferLectura(miLinea)(6) = 1 And PLC.BufferLectura(miLinea)(8) = 0 Then
                                                ' si la otra esta habilitada hago el cambio y la pongo en espera
                                                Logs.nuevo("CAMBIO DE ETIQUETADORA")
                                                ImpresoraActual(miLinea) = True
                                                'ImpresoraSecundaria(linea).LimpiarBuffer()
                                                swlineas(miLinea) = New Stopwatch
                                                swlineas(miLinea).Start()
                                                contadordespuesdebajoinsumo(1, miLinea) = 0

                                            End If
                                        End If
                                    End If
                                Else
                                    'si no esta bajo insumo procedo normal 
                                    SyncLock objec
                                        imprimirTCP(cajaactual, gsParam.DriverImpresoraSecundaria(miLinea))
                                    End SyncLock

                                    'ImpresoraSecundaria(linea).imprimir(cajaactual)
                                End If
                            Else
                                Logs.nuevo("NO ESTA ACTIVA NINGUNA DE LAS DOS ")
                                If ImpresoraActual(miLinea) Then
                                    'ImpresoraActual(linea) = False
                                    Logs.nuevo("CAMBIO DE ETIQUETADORA")
                                    Logs.nuevo("aca no imprimo")
                                    ImpresoraActual(miLinea) = False
                                    'ImpresoraPrimaria(linea).LimpiarBuffer()

                                    swlineas(miLinea) = New Stopwatch
                                    swlineas(miLinea).Start()
                                    noimprime = False
                                Else
                                    Logs.nuevo("CAMBIO DE ETIQUETADORA")
                                    Logs.nuevo("aca no imprimo")
                                    ImpresoraActual(miLinea) = True
                                    'ImpresoraSecundaria(linea).LimpiarBuffer()
                                    swlineas(miLinea) = New Stopwatch
                                    swlineas(miLinea).Start()
                                    noimprime = False
                                End If
                            End If

                            Logs.nuevo("Imprimir: El ciclo de etiquetado tardo " + sw.ElapsedMilliseconds.ToString + " por linea " + (miLinea + 1).ToString)

                            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            '4: Guardo en bd y hago un conteo 
                            If noimprime Then
                                cajaactual.guardarCaja()
                                HAY_CAJA(miLinea) = False
                                PLC.Reset(miLinea)
                                'If sw.ElapsedMilliseconds > 200 Then
                                Logs.nuevo("Guardar: El ciclo de etiquetado tardo " + sw.ElapsedMilliseconds.ToString + " por linea " + (miLinea + 1).ToString)
                                'escribirMensaje("Ciclo PLC " & sw.ElapsedMilliseconds.ToString, False)
                                'End If
                            Else
                                HAY_CAJA(miLinea) = False
                                Logs.nuevo("no imprimio por cambio")
                                PLC.Reset(miLinea)
                            End If
                        End If
                    Else
                        HAY_CAJA(miLinea) = False
                    End If
                    'Logs.nuevo("El ciclo de etiquetado tardo " + sw.ElapsedMilliseconds.ToString + " por linea " + (linea + 1).ToString)
                    sw.Stop()
                End If

            End If
            ''For m As Int16 = 0 To 100
            PLC.BufferLectura(miLinea) = Nothing
            ''Next


cambiolinea:
            'Thread.Sleep(50)
            Do
                miLinea = miLinea + 1
                If miLinea > 9 Then
                    ' Salgo de este loop para incrementar linea y empezar de nuevo:
                    miLinea = 0
                    'Para que la encuesta a las 3 lineas no sea menos a 30 milisegundos
                    datetimenaw = DateTime.Now
                    If datetimenaw.Hour = datetimeanterior.Hour Then
                        If datetimenaw.Minute = datetimeanterior.Minute Then
                            If datetimenaw.Second = datetimeanterior.Second Then
                                Dim resta As Integer = datetimenaw.Millisecond - datetimeanterior.Millisecond
                                If resta < 30 Then
                                    'Logs.nuevo("El ciclo de las 3 lineas es " + resta.ToString)
                                    Threading.Thread.Sleep(1)
                                End If
                            End If
                        End If
                    End If
                    datetimeanterior = datetimenaw

                End If
                Exit Do
            Loop



        End While
    End Sub 'PROCESOO
    Private Sub leerPLC(linea As Integer)
        Dim numdb As Integer = 0
        ' Dim sw As New Stopwatch


        ' sw.Start()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Primero leo 

        PLC.LeerBytesDB(linea)



        If PLC.BufferLectura(linea)(16) = 1 Then
            pistonafuera(0, linea) = True
        ElseIf PLC.BufferLectura(linea)(16) = 0 Then
            pistonafuera(0, linea) = False

        End If
        If PLC.BufferLectura(linea)(17) = 1 Then
            pistonafuera(1, linea) = True
        ElseIf PLC.BufferLectura(linea)(17) = 0 Then
            pistonafuera(1, linea) = False

        End If
        'la logica con el freno es al reves abre valvula cuando 
        If PLC.BufferLectura(linea)(18) = 0 Then
            frenoarriba(linea) = True
        ElseIf PLC.BufferLectura(linea)(18) = 1 Then
            frenoarriba(linea) = False
        End If
    End Sub
    Private Function mObtener_Variables_PLC(Linea As Integer) As Boolean

        'Pido las variables del PLC
        Dim Pudoleer As Boolean = False
        If gsParam.ModoDebug Then
            Exit Function
        End If

        Dim numdb As Integer = 0
        Dim sw As New Stopwatch



        sw.Start()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Primero leo 

        Pudoleer = PLC.LeerBytesDB(Linea)


        If Pudoleer Then


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Ya con las variables cargadas en el buffer llamo a la funcion que procesa ese array
            PLC.procesarEstadoPLC(Linea)
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'en produccion 
            If PLC.BufferLectura(Linea)(9) = 1 Then
                If Linea = 0 Then
                    osEnProduccion1.FillColor = Color.Green
                ElseIf Linea = 1 Then
                    osEnProduccion2.FillColor = Color.Green
                ElseIf Linea = 2 Then
                    osEnProduccion3.FillColor = Color.Green
                ElseIf Linea = 4 Then
                    osEnProduccion5.FillColor = Color.Green
                ElseIf Linea = 5 Then
                    osEnProduccion6.FillColor = Color.Green
                ElseIf Linea = 6 Then
                    osEnProduccion7.FillColor = Color.Green
                ElseIf Linea = 7 Then
                    osEnProduccion8.FillColor = Color.Green
                ElseIf Linea = 8 Then
                    osEnProduccion9.FillColor = Color.Green
                ElseIf Linea = 9 Then
                    osEnProduccion10.FillColor = Color.Green
                End If
            ElseIf PLC.BufferLectura(Linea)(9) = 0 Then
                If Linea = 0 Then
                    Try

                        osEnProduccion1.FillColor = Color.Red

                    Catch ex As Exception
                        Logs.nuevo(ex.Message)
                    End Try
                ElseIf Linea = 1 Then
                    osEnProduccion2.FillColor = Color.Red
                ElseIf Linea = 2 Then
                    osEnProduccion3.FillColor = Color.Red
                ElseIf Linea = 4 Then
                    osEnProduccion5.FillColor = Color.Red
                ElseIf Linea = 5 Then
                    osEnProduccion6.FillColor = Color.Red
                ElseIf Linea = 6 Then
                    osEnProduccion7.FillColor = Color.Red
                ElseIf Linea = 7 Then
                    osEnProduccion8.FillColor = Color.Red
                ElseIf Linea = 8 Then
                    osEnProduccion9.FillColor = Color.Red
                ElseIf Linea = 9 Then
                    osEnProduccion10.FillColor = Color.Red
                End If

            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'zebra online


            If PLC.BufferLectura(Linea)(6) = 1 And PLC.BufferLectura(Linea)(8) = 0 Then
                If Linea = 0 Then
                    osZOnline1.FillColor = Color.Green
                ElseIf Linea = 1 Then
                    osZOnline2.FillColor = Color.Green
                ElseIf Linea = 2 Then
                    osZOnline3.FillColor = Color.Green
                ElseIf Linea = 4 Then
                    osZOnline5.FillColor = Color.Green
                ElseIf Linea = 5 Then
                    osZOnline6.FillColor = Color.Green
                ElseIf Linea = 6 Then
                    osZOnline7.FillColor = Color.Green
                ElseIf Linea = 7 Then
                    osZOnline8.FillColor = Color.Green
                ElseIf Linea = 8 Then
                    osZOnline9.FillColor = Color.Green
                ElseIf Linea = 9 Then
                    osZOnline10.FillColor = Color.Green
                End If
            ElseIf PLC.BufferLectura(Linea)(6) = 0 And PLC.BufferLectura(linea)(8) = 0 Then
                If Linea = 0 Then
                    osZOnline1.FillColor = Color.Yellow
                ElseIf Linea = 1 Then
                    osZOnline2.FillColor = Color.Yellow
                ElseIf Linea = 2 Then
                    osZOnline3.FillColor = Color.Yellow
                ElseIf Linea = 4 Then
                    osZOnline5.FillColor = Color.Yellow
                ElseIf Linea = 5 Then
                    osZOnline6.FillColor = Color.Yellow
                ElseIf Linea = 6 Then
                    osZOnline7.FillColor = Color.Yellow
                ElseIf Linea = 7 Then
                    osZOnline8.FillColor = Color.Yellow
                ElseIf Linea = 8 Then
                    osZOnline9.FillColor = Color.Yellow
                ElseIf Linea = 9 Then
                    osZOnline10.FillColor = Color.Yellow
                End If

            ElseIf PLC.BufferLectura(Linea)(6) = 0 And PLC.BufferLectura(Linea)(8) = 1 Then
                If Linea = 0 Then
                    osZOnline1.FillColor = Color.Red
                ElseIf Linea = 1 Then
                    osZOnline2.FillColor = Color.Red
                ElseIf Linea = 2 Then
                    osZOnline3.FillColor = Color.Red
                ElseIf Linea = 4 Then
                    osZOnline5.FillColor = Color.Red
                ElseIf Linea = 5 Then
                    osZOnline6.FillColor = Color.Red
                ElseIf Linea = 6 Then
                    osZOnline7.FillColor = Color.Red
                ElseIf Linea = 7 Then
                    osZOnline8.FillColor = Color.Red
                ElseIf Linea = 8 Then
                    osZOnline9.FillColor = Color.Red
                ElseIf Linea = 9 Then
                    osZOnline10.FillColor = Color.Red
                End If
            Else
                If Linea = 0 Then
                    osZOnline1.FillColor = Color.Red
                ElseIf Linea = 1 Then
                    osZOnline2.FillColor = Color.Red
                ElseIf Linea = 2 Then
                    osZOnline3.FillColor = Color.Red
                ElseIf Linea = 4 Then
                    osZOnline5.FillColor = Color.Red
                ElseIf Linea = 5 Then
                    osZOnline6.FillColor = Color.Red
                ElseIf Linea = 6 Then
                    osZOnline7.FillColor = Color.Red
                ElseIf Linea = 7 Then
                    osZOnline8.FillColor = Color.Red
                ElseIf Linea = 8 Then
                    osZOnline9.FillColor = Color.Red
                ElseIf Linea = 9 Then
                    osZOnline10.FillColor = Color.Red
                End If
            End If

            'bajo insumo 
            If PLC.BufferLectura(Linea)(7) = 1 Then

                If Linea = 0 Then
                    osBajoInsumo1.FillColor = Color.Yellow
                ElseIf Linea = 1 Then
                    osBajoInsumo2.FillColor = Color.Yellow
                ElseIf Linea = 2 Then
                    osBajoInsumo3.FillColor = Color.Yellow
                ElseIf Linea = 4 Then
                    osBajoInsumo5.FillColor = Color.Yellow
                ElseIf Linea = 5 Then
                    osBajoInsumo6.FillColor = Color.Yellow
                ElseIf Linea = 6 Then
                    osBajoInsumo7.FillColor = Color.Yellow
                ElseIf Linea = 7 Then
                    osBajoInsumo8.FillColor = Color.Yellow
                ElseIf Linea = 8 Then
                    osBajoInsumo9.FillColor = Color.Yellow
                ElseIf Linea = 9 Then
                    osBajoInsumo10.FillColor = Color.Yellow
                End If

            ElseIf PLC.BufferLectura(linea)(7) = 0 Then
                contadordespuesdebajoinsumo(0, Linea) = 0
                If Linea = 0 Then
                    osBajoInsumo1.FillColor = Color.Transparent
                ElseIf Linea = 1 Then
                    osBajoInsumo2.FillColor = Color.Transparent
                ElseIf Linea = 2 Then
                    osBajoInsumo3.FillColor = Color.Transparent
                ElseIf Linea = 4 Then
                    osBajoInsumo5.FillColor = Color.Transparent
                ElseIf Linea = 5 Then
                    osBajoInsumo6.FillColor = Color.Transparent
                ElseIf Linea = 6 Then
                    osBajoInsumo7.FillColor = Color.Transparent
                ElseIf Linea = 7 Then
                    osBajoInsumo8.FillColor = Color.Transparent
                ElseIf Linea = 8 Then
                    osBajoInsumo9.FillColor = Color.Transparent
                ElseIf Linea = 9 Then
                    osBajoInsumo10.FillColor = Color.Transparent
                End If

            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'zebra online2


            If PLC.BufferLectura(Linea)(13) = 1 And PLC.BufferLectura(Linea)(15) = 0 Then
                If Linea = 0 Then
                    osZOnline12.FillColor = Color.Green
                ElseIf Linea = 1 Then
                    osZOnline22.FillColor = Color.Green
                ElseIf Linea = 2 Then
                    osZOnline32.FillColor = Color.Green
                ElseIf Linea = 4 Then
                    osZOnline52.FillColor = Color.Green
                ElseIf Linea = 5 Then
                    osZOnline62.FillColor = Color.Green
                ElseIf Linea = 6 Then
                    osZOnline72.FillColor = Color.Green
                ElseIf Linea = 7 Then
                    osZOnline82.FillColor = Color.Green
                ElseIf Linea = 8 Then
                    osZOnline92.FillColor = Color.Green
                ElseIf Linea = 9 Then
                    osZOnline102.FillColor = Color.Green
                End If
            ElseIf PLC.BufferLectura(Linea)(13) = 0 And PLC.BufferLectura(linea)(15) = 0 Then
                If Linea = 0 Then
                    osZOnline12.FillColor = Color.Yellow
                ElseIf Linea = 1 Then
                    osZOnline22.FillColor = Color.Yellow
                ElseIf Linea = 2 Then
                    osZOnline32.FillColor = Color.Yellow
                ElseIf Linea = 4 Then
                    osZOnline52.FillColor = Color.Yellow
                ElseIf Linea = 5 Then
                    osZOnline62.FillColor = Color.Yellow
                ElseIf Linea = 6 Then
                    osZOnline72.FillColor = Color.Yellow
                ElseIf Linea = 7 Then
                    osZOnline82.FillColor = Color.Yellow
                ElseIf Linea = 8 Then
                    osZOnline92.FillColor = Color.Yellow
                ElseIf Linea = 9 Then
                    osZOnline10.FillColor = Color.Yellow
                End If

            ElseIf PLC.BufferLectura(Linea)(13) = 0 And PLC.BufferLectura(Linea)(15) = 1 Then
                If Linea = 0 Then
                    osZOnline12.FillColor = Color.Red
                ElseIf Linea = 1 Then
                    osZOnline22.FillColor = Color.Red
                ElseIf Linea = 2 Then
                    osZOnline32.FillColor = Color.Red
                ElseIf Linea = 4 Then
                    osZOnline52.FillColor = Color.Red
                ElseIf Linea = 5 Then
                    osZOnline62.FillColor = Color.Red
                ElseIf Linea = 6 Then
                    osZOnline72.FillColor = Color.Red
                ElseIf Linea = 7 Then
                    osZOnline82.FillColor = Color.Red
                ElseIf Linea = 8 Then
                    osZOnline92.FillColor = Color.Red
                ElseIf Linea = 9 Then
                    osZOnline102.FillColor = Color.Red
                End If
            Else
                If Linea = 0 Then
                    osZOnline12.FillColor = Color.Red
                ElseIf Linea = 1 Then
                    osZOnline22.FillColor = Color.Red
                ElseIf Linea = 2 Then
                    osZOnline32.FillColor = Color.Red
                ElseIf Linea = 4 Then
                    osZOnline52.FillColor = Color.Red
                ElseIf Linea = 5 Then
                    osZOnline62.FillColor = Color.Red
                ElseIf Linea = 6 Then
                    osZOnline72.FillColor = Color.Red
                ElseIf Linea = 7 Then
                    osZOnline82.FillColor = Color.Red
                ElseIf Linea = 8 Then
                    osZOnline92.FillColor = Color.Red
                ElseIf Linea = 9 Then
                    osZOnline102.FillColor = Color.Red
                End If
            End If
            'bajo insumo2
            If PLC.BufferLectura(Linea)(14) = 1 Then
                If Linea = 0 Then
                    osBajoInsumo12.FillColor = Color.Yellow
                ElseIf Linea = 1 Then
                    osBajoInsumo22.FillColor = Color.Yellow
                ElseIf Linea = 2 Then
                    osBajoInsumo32.FillColor = Color.Yellow
                ElseIf Linea = 4 Then
                    osBajoInsumo52.FillColor = Color.Yellow
                ElseIf Linea = 5 Then
                    osBajoInsumo62.FillColor = Color.Yellow
                ElseIf Linea = 6 Then
                    osBajoInsumo72.FillColor = Color.Yellow
                ElseIf Linea = 7 Then
                    osBajoInsumo82.FillColor = Color.Yellow
                ElseIf Linea = 8 Then
                    osBajoInsumo92.FillColor = Color.Yellow
                ElseIf Linea = 9 Then
                    osBajoInsumo102.FillColor = Color.Yellow
                End If
            ElseIf PLC.BufferLectura(linea)(14) = 0 Then
                contadordespuesdebajoinsumo(1, Linea) = 0
                If Linea = 0 Then
                    osBajoInsumo12.FillColor = Color.Transparent
                ElseIf Linea = 1 Then
                    osBajoInsumo22.FillColor = Color.Transparent
                ElseIf Linea = 2 Then
                    osBajoInsumo32.FillColor = Color.Transparent
                ElseIf Linea = 4 Then
                    osBajoInsumo52.FillColor = Color.Transparent
                ElseIf Linea = 5 Then
                    osBajoInsumo62.FillColor = Color.Transparent
                ElseIf Linea = 6 Then
                    osBajoInsumo72.FillColor = Color.Transparent
                ElseIf Linea = 7 Then
                    osBajoInsumo82.FillColor = Color.Transparent
                ElseIf Linea = 8 Then
                    osBajoInsumo92.FillColor = Color.Transparent
                ElseIf Linea = 9 Then
                    osBajoInsumo102.FillColor = Color.Transparent
                End If

            End If
            'leo, si hay caja inmediatamente la desmarco 

            If HAY_CAJA(Linea) Then
                Logs.nuevo("Hay caja linea " + (Linea + 1).ToString + " con codigo " + codigo_caja(Linea).ToString)
                'PLC.Reset(0)
            End If
            mObtener_Variables_PLC = Pudoleer

        Else
            mObtener_Variables_PLC = Pudoleer
        End If

        If sw.ElapsedMilliseconds > 500 Then
            Logs.nuevo("El ciclo de lectura del PLC tardo " & sw.ElapsedMilliseconds.ToString)
            'escribirMensaje("Ciclo PLC " & sw.ElapsedMilliseconds.ToString, False)
        End If
        sw.Stop()

    End Function
    Private Sub iniciar()

        ''On Error Resume Next
        If usuario.Logueado = False Then
            Call MessageBox.Show("Debe estar logueado para poder iniciar")
            cmdLoguearDesloguear_Click_1(Nothing, Nothing)
            Exit Sub
        End If
        Logs.nuevo("----------------INICIO EL ETIQUETADO AUTOMÁTICO---------------")
        Dim debug As Boolean = True
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Impresoras 
        escribirMensaje("--------------------------------------", False, True)
        escribirMensaje("Iniciando etiquetado...", True)
        For i As Integer = 0 To 9
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ImpresoraPrimaria(i) = New Impresora(gsParam.DriverImpresoraPrincipal(i), Impresora.tipo.aplicadora)
            If (i <> 3) Then
                If gsParam.Deshablin1(i) = False Then
                    If gsParam.DeshabImp(i, 0) = False Then
                        If My.Computer.Network.Ping(gsParam.DriverImpresoraPrincipal(i)) = False Then
                            Call MessageBox.Show("No hay ping impresora automática: " + gsParam.DriverImpresoraPrincipal(i) + " No se iniciará el etiquetado")
                            'Call MessageBox.Show("No se ecuentra el driver de la impresora automática: " + ImpresoraPrimaria(i).Driver + " No se iniciará el etiquetado")
                            Exit Sub
                        Else
                            escribirMensaje("Impresora automática OK")
                            Logs.nuevo("Impresora automática OK")
                            ImpresoraPrimaria(i).LimpiarBuffer(i)
                            contadordespuesdebajoinsumo(0, i) = 0
                        End If
                    End If

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ImpresoraSecundaria(i) = New Impresora(gsParam.DriverImpresoraSecundaria(i), Impresora.tipo.manual)
                    If gsParam.DeshabImp(i, 1) = False Then
                        If My.Computer.Network.Ping(gsParam.DriverImpresoraSecundaria(i)) = False Then
                            Call MessageBox.Show("No hay ping impresora automática: " + gsParam.DriverImpresoraSecundaria(i) + " No se iniciará el etiquetado")
                            'Call MessageBox.Show("No se ecuentra el driver de la impresora manual: " + ImpresoraSecundaria(2).Driver + " No se iniciará el etiquetado")
                            Exit Sub
                        Else
                            escribirMensaje("Impresora secundaria OK")
                            Logs.nuevo("Impresora secundaria OK")
                            contadordespuesdebajoinsumo(1, i) = 0
                        End If
                    End If


                End If
                swlineas(i) = New Stopwatch
            End If


            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Next
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Conecto PLC
        ImpresoraActual(0) = rbEmp11.Checked
        ImpresoraActual(1) = rbEmp21.Checked
        ImpresoraActual(2) = rbEmp31.Checked
        ImpresoraActual(4) = rbEmp51.Checked
        ImpresoraActual(5) = rbEmp61.Checked
        ImpresoraActual(6) = rbEmp71.Checked
        ImpresoraActual(7) = rbEmp81.Checked
        ImpresoraActual(8) = rbEmp91.Checked
        ImpresoraActual(9) = rbEmp101.Checked
        If conectarPLC() Then
            escribirMensaje("Se conectó correctamente el PLC en la IP: " + gsParam.IPPLC)
            Logs.nuevo("Se conectó correctamente el PLC en la IP: " + gsParam.IPPLC)

        Else
            MessageBox.Show("Error conectando con el PLC. No se iniciará el etiquetado")
            Exit Sub
        End If
        For i As Integer = 0 To 9
            HAY_CAJA(i) = False
        Next
        escribirConfigPLC()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'busco ordenes y las mando 
        Cargar_Ordenes()
        'cmdCargarPLC.Enabled = False
        cmdDetener.Enabled = True
        cmdInicioEtiquetado.Enabled = False
        gbTurno.Enabled = False
        cmdConfig.Enabled = False
        iniciado = True
        listaordenes1 = Repositorio.obtenertodaslasordenes()
        'For i As Integer = 0 To 9
        '    If Not gsParam.Deshablin1(i) And i <> 3 Then
        '        Logs.nuevo("Hilo " + i.ToString())
        '        escribirMensaje("Hilo " + i.ToString(), i)
        '        tcpThd(i) = New Thread(AddressOf threat)
        '        tcpThd(i).Start(i)
        '        Logs.nuevo("Hilo " + i.ToString() + " en Start")

        '    End If
        'Next
        tcpThd = New Thread(AddressOf threat)
        tcpThd.Start()
        inicio = True
        Logs.nuevo("----------------DETENGO EL ETIQUETADO AUTOMÁTICO---------------")
    End Sub
    Private Function conectarPLC() As Boolean
        PLC = New Libnodave_WinAC
        If PLC.Conectar(102, gsParam.IPPLC, 0, 1) Then

            escribirMensaje("Conexión creada correctamente.", False)
            Return True
            'Si se coencta bien inicio la lectura

        Else
            escribirMensaje("Fallo al iniciar el PLC", False)
            Logs.nuevo("Falló la conexión con el PLC: " + PLC.Msj)
            Return False
            'Si se conecta mal no inicio nada
        End If
    End Function
    'Private Sub Desconectarthreat()
    '    tcpThd.Abort()
    'End Sub
    Private Sub detener()
        inicio = False
        Try
            PLC.Desconectar()

        Catch ex As Exception
            Logs.nuevo("Error cerrando PLC: " + ex.Message)
        End Try

        For i As Integer = 0 To 9
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ImpresoraPrimaria(i) = Nothing
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ImpresoraSecundaria(i) = Nothing
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Next
        'End If
        iniciado = False
        'tcpThd.Abort()
        'For i As Integer = 0 To 9
        tcpThd = Nothing '(i) = Nothing
        'Next

        'cmdCargarPLC.Enabled = True
        cmdDetener.Enabled = False
        cmdInicioEtiquetado.Enabled = True
        gbTurno.Enabled = True
        cmdConfig.Enabled = True
        Logs.nuevo("----------------DETENGO EL ETIQUETADO AUTOMÁTICO---------------")
    End Sub

    'Private Function imprimirTCP(p As caja, ip As String) As Boolean
    '    Dim resultado As String '= "OK"

    '    impresora_tcp = New ketan.ketan.Cliente
    '    impresora_tcp.IPDelHost = ip
    '    impresora_tcp.PuertoDelHost = "9100"
    '    Try
    '        impresora_tcp.Conectar()
    '        'MsgBox("Se conectó correctamente la impresora de salida")
    '    Catch ex As Exception
    '        Logs.nuevo("Error conectando con la impresora de ip:" + ip)
    '        Return False
    '        Exit Function
    '    End Try
    '    Dim etiqueta As String = ""
    '    Dim sChr = "^"
    '    Dim sVto As String
    '    ' Dim vto As DateTime = DateTime.Now
    '    Dim ultimoCampo As String
    '    If (p.Numero <> 0) Then
    '        ''Guardo los datos a imprimir en una variable
    '        ' etiqueta += "~" + "HI" + vbCrLf
    '        etiqueta += sChr + "XA" + vbCrLf
    '        etiqueta += sChr + "XF" + "E:" + p.Etiqueta.ToString + vbCrLf                                   'secundaria
    '        etiqueta += sChr + "FN1" + sChr + "FD" + p.Descripcion + sChr + "FS" + vbCrLf                       'Descripcion
    '        etiqueta += sChr + "FN2" + sChr + "FD" + p.Codigo + sChr + "FS" + vbCrLf                            'Codigo 
    '        etiqueta += sChr + "FN3" + sChr + "FD" + p.Codbar + sChr + "FS" + vbCrLf                            'Codigo datamatrix
    '        etiqueta += sChr + "FN4" + sChr + "FD>:" + p.Codbar + sChr + "FS" + vbCrLf                          'Codigo de barras
    '        etiqueta += sChr + "FN5" + sChr + "FD" + p.FechaProduccion.ToString + sChr + "FS" + vbCrLf          'fecha de produccion    
    '        etiqueta += sChr + "FN6" + sChr + "FD" + p.FechaVencimiento.ToString + sChr + "FS" + vbCrLf         'fecha de vencimiento
    '        etiqueta += sChr + "FN7" + sChr + "FD" + p.Orden.ToString + sChr + "FS" + vbCrLf                    'nro de orden
    '        etiqueta += sChr + "FN8" + sChr + "FD" + p.Descripcion2.ToString + sChr + "FS" + vbCrLf                    'nro de orden
    '        etiqueta += sChr + "FN9" + sChr + "FD" + p.Descripcion3.ToString + sChr + "FS" + vbCrLf                    'nro de orden
    '        etiqueta += sChr + "FN10" + sChr + "FD" + p.Descripcion4.ToString + sChr + "FS" + vbCrLf                    'nro de orden
    '        etiqueta += sChr + "FN11" + sChr + "FD" + p.Descripcion5.ToString + sChr + "FS" + vbCrLf
    '        etiqueta += sChr + "PQ1+0+1+Y" + vbCrLf
    '        etiqueta += sChr + "XZ" + vbCrLf

    '    Else
    '        etiqueta += sChr + "XA" + vbCrLf
    '        etiqueta += sChr + "XF" + "E:" + p.Etiqueta.ToString + vbCrLf                                   'secundaria
    '        etiqueta += sChr + "FN1" + sChr + "FD" + p.Descripcion + sChr + "FS" + vbCrLf                       'Descripcion
    '        etiqueta += sChr + "FN2" + sChr + "FD" + p.Codigo + sChr + "FS" + vbCrLf                            'Codigo 
    '        'etiqueta += sChr + "FN3" + sChr + "FD" + p.Codbar + sChr + "FS" + vbCrLf                            'Codigo datamatrix
    '        'etiqueta += sChr + "FN4" + sChr + "FD>:" + p.Codbar + sChr + "FS" + vbCrLf                          'Codigo de barras
    '        etiqueta += sChr + "FN5" + sChr + "FD" + p.FechaProduccion.ToString + sChr + "FS" + vbCrLf          'fecha de produccion    
    '        etiqueta += sChr + "FN6" + sChr + "FD" + p.FechaVencimiento.ToString + sChr + "FS" + vbCrLf         'fecha de vencimiento
    '        etiqueta += sChr + "FN7" + sChr + "FD" + p.Orden.ToString + sChr + "FS" + vbCrLf                    'nro de orden
    '        etiqueta += sChr + "FN8" + sChr + "FD" + p.Descripcion2.ToString + sChr + "FS" + vbCrLf                    'nro de orden
    '        etiqueta += sChr + "FN9" + sChr + "FD" + p.Descripcion3.ToString + sChr + "FS" + vbCrLf                    'nro de orden
    '        etiqueta += sChr + "FN10" + sChr + "FD" + p.Descripcion4.ToString + sChr + "FS" + vbCrLf                    'nro de orden
    '        etiqueta += sChr + "FN11" + sChr + "FD" + p.Descripcion5.ToString + sChr + "FS" + vbCrLf
    '        etiqueta += sChr + "PQ1+0+1+Y" + vbCrLf
    '        etiqueta += sChr + "XZ" + vbCrLf

    '    End If
    '    resultado = impresora_tcp.EnviarDatos(etiqueta)

    '    If resultado.Contains("OK") Then
    '        impresora_tcp.Desconectar()
    '        Return True
    '    Else
    '        'Logs.nuevo(resultado)
    '        impresora_tcp.Desconectar()
    '        Return False
    '    End If
    '    ' Threading.Thread.Sleep(30)


    'End Function
    Private Function imprimirTCP(p As caja, ip As String) As Boolean
        Dim resultado As String '= "OK"
        Dim timeoutMs As Integer = 1500 ' 1.5 segundos máximo de espera

        impresora_tcp = New ketan.ketan.Cliente
        impresora_tcp.IPDelHost = ip
        impresora_tcp.PuertoDelHost = "9100"

        Try
            ' Implementación del timeout para la conexión
            Dim connected As Boolean = False
            Dim connectionDelegate As New System.Threading.ThreadStart(
            Sub()
                Try
                    impresora_tcp.Conectar()
                    connected = True
                Catch ex As Exception
                    Logs.nuevo("Error en conexión async: " + ex.Message)
                End Try
            End Sub)

            Dim connectionThread As New System.Threading.Thread(connectionDelegate)
            connectionThread.Start()

            ' Esperar un tiempo limitado para la conexión
            If Not connectionThread.Join(timeoutMs) Then
                ' Si pasó el tiempo máximo, abortar y continuar
                connectionThread.Abort()
                Logs.nuevo("Timeout conectando con la impresora de ip:" + ip)
                Return False
            End If

            ' Verificar si la conexión tuvo éxito
            If Not connected Then
                Logs.nuevo("No se pudo conectar con la impresora de ip:" + ip)
                Return False
            End If

            ' Continuar con el proceso normal de impresión
            Dim etiqueta As String = ""
            Dim sChr = "^"
            Dim sVto As String
            ' Dim vto As DateTime = DateTime.Now
            Dim ultimoCampo As String

            If (p.Numero <> 0) Then
                ''Guardo los datos a imprimir en una variable
                ' etiqueta += "~" + "HI" + vbCrLf
                etiqueta += sChr + "XA" + vbCrLf
                etiqueta += sChr + "XF" + "E:" + p.Etiqueta.ToString + vbCrLf                                   'secundaria
                etiqueta += sChr + "FN1" + sChr + "FD" + p.Descripcion + sChr + "FS" + vbCrLf                       'Descripcion
                etiqueta += sChr + "FN2" + sChr + "FD" + p.Codigo + sChr + "FS" + vbCrLf                            'Codigo 
                etiqueta += sChr + "FN3" + sChr + "FD" + p.Codbar + sChr + "FS" + vbCrLf                            'Codigo datamatrix
                etiqueta += sChr + "FN4" + sChr + "FD>:" + p.Codbar + sChr + "FS" + vbCrLf                          'Codigo de barras
                etiqueta += sChr + "FN5" + sChr + "FD" + p.FechaProduccion.ToString + sChr + "FS" + vbCrLf          'fecha de produccion    
                etiqueta += sChr + "FN6" + sChr + "FD" + p.FechaVencimiento.ToString + sChr + "FS" + vbCrLf         'fecha de vencimiento
                etiqueta += sChr + "FN7" + sChr + "FD" + p.Orden.ToString + sChr + "FS" + vbCrLf                    'nro de orden
                etiqueta += sChr + "FN8" + sChr + "FD" + p.Descripcion2.ToString + sChr + "FS" + vbCrLf             'nro de orden
                etiqueta += sChr + "FN9" + sChr + "FD" + p.Descripcion3.ToString + sChr + "FS" + vbCrLf             'nro de orden
                etiqueta += sChr + "FN10" + sChr + "FD" + p.Descripcion4.ToString + sChr + "FS" + vbCrLf            'nro de orden
                etiqueta += sChr + "FN11" + sChr + "FD" + p.Descripcion5.ToString + sChr + "FS" + vbCrLf
                etiqueta += sChr + "PQ1+0+1+Y" + vbCrLf
                etiqueta += sChr + "XZ" + vbCrLf
            Else
                etiqueta += sChr + "XA" + vbCrLf
                etiqueta += sChr + "XF" + "E:" + p.Etiqueta.ToString + vbCrLf                                   'secundaria
                etiqueta += sChr + "FN1" + sChr + "FD" + p.Descripcion + sChr + "FS" + vbCrLf                       'Descripcion
                etiqueta += sChr + "FN2" + sChr + "FD" + p.Codigo + sChr + "FS" + vbCrLf                            'Codigo 
                'etiqueta += sChr + "FN3" + sChr + "FD" + p.Codbar + sChr + "FS" + vbCrLf                            'Codigo datamatrix
                'etiqueta += sChr + "FN4" + sChr + "FD>:" + p.Codbar + sChr + "FS" + vbCrLf                          'Codigo de barras
                etiqueta += sChr + "FN5" + sChr + "FD" + p.FechaProduccion.ToString + sChr + "FS" + vbCrLf          'fecha de produccion    
                etiqueta += sChr + "FN6" + sChr + "FD" + p.FechaVencimiento.ToString + sChr + "FS" + vbCrLf         'fecha de vencimiento
                etiqueta += sChr + "FN7" + sChr + "FD" + p.Orden.ToString + sChr + "FS" + vbCrLf                    'nro de orden
                etiqueta += sChr + "FN8" + sChr + "FD" + p.Descripcion2.ToString + sChr + "FS" + vbCrLf             'nro de orden
                etiqueta += sChr + "FN9" + sChr + "FD" + p.Descripcion3.ToString + sChr + "FS" + vbCrLf             'nro de orden
                etiqueta += sChr + "FN10" + sChr + "FD" + p.Descripcion4.ToString + sChr + "FS" + vbCrLf            'nro de orden
                etiqueta += sChr + "FN11" + sChr + "FD" + p.Descripcion5.ToString + sChr + "FS" + vbCrLf
                etiqueta += sChr + "PQ1+0+1+Y" + vbCrLf
                etiqueta += sChr + "XZ" + vbCrLf
            End If

            ' También aplicar timeout para el envío de datos
            Dim sendComplete As Boolean = False
            Dim sendDelegate As New System.Threading.ThreadStart(
            Sub()
                Try
                    resultado = impresora_tcp.EnviarDatos(etiqueta)
                    sendComplete = True
                Catch ex As Exception
                    Logs.nuevo("Error en envío async: " + ex.Message)
                End Try
            End Sub)

            Dim sendThread As New System.Threading.Thread(sendDelegate)
            sendThread.Start()

            ' Esperar un tiempo limitado para el envío
            If Not sendThread.Join(timeoutMs) Then
                sendThread.Abort()
                Logs.nuevo("Timeout enviando datos a la impresora de ip:" + ip)
                impresora_tcp.Desconectar()
                Return False
            End If

            If Not sendComplete Then
                Logs.nuevo("No se pudieron enviar datos a la impresora de ip:" + ip)
                impresora_tcp.Desconectar()
                Return False
            End If

            If resultado IsNot Nothing AndAlso resultado.Contains("OK") Then
                impresora_tcp.Desconectar()
                Return True
            Else
                Logs.nuevo("Respuesta incorrecta de impresora: " + If(resultado IsNot Nothing, resultado, "null"))
                impresora_tcp.Desconectar()
                Return False
            End If
        Catch ex As Exception
            Logs.nuevo("Error conectando con la impresora de ip:" + ip + ": " + ex.Message)
            Try
                impresora_tcp.Desconectar()
            Catch disconnectEx As Exception
                Logs.nuevo("Error al desconectar: " + disconnectEx.Message)
            End Try
            Return False
        End Try
    End Function
    Private Sub TabPage5_Click(sender As Object, e As EventArgs) Handles TabPage5.Click

    End Sub

    Private Sub TabPage6_Click(sender As Object, e As EventArgs) Handles TabPage6.Click

    End Sub

    Private Sub rbEmp51_CheckedChanged(sender As Object, e As EventArgs) Handles rbEmp51.CheckedChanged

    End Sub

    Private Sub RadioButton13_CheckedChanged(sender As Object, e As EventArgs) Handles rbEmp92.CheckedChanged

    End Sub

    Private Sub Label29_Click(sender As Object, e As EventArgs) Handles lblEtiqutadora52.Click

    End Sub

    Private Sub Label62_Click(sender As Object, e As EventArgs) Handles lblEtiqutadora82.Click

    End Sub

    Private Sub tabpage_TabIndexChanged(sender As Object, e As EventArgs) Handles tabpage.TabIndexChanged

    End Sub

    Private Sub tabpage_Selected(sender As Object, e As TabControlEventArgs) Handles tabpage.Selected
        Select Case (tabpage.SelectedIndex)
            Case 0
                Cargar_Cajas(tabpage.SelectedIndex + 1, lvcajas1)
            Case 1
                Cargar_Cajas(tabpage.SelectedIndex + 1, lvcajas2)
            Case 2
                Cargar_Cajas(tabpage.SelectedIndex + 1, lvcajas3)


            Case 4
                Cargar_Cajas(tabpage.SelectedIndex + 1, lvcajas5)
            Case 5
                Cargar_Cajas(tabpage.SelectedIndex + 1, lvcajas6)
            Case 6
                Cargar_Cajas(tabpage.SelectedIndex + 1, lvcajas7)
            Case 7
                Cargar_Cajas(tabpage.SelectedIndex + 1, lvcajas8)
            Case 8
                Cargar_Cajas(tabpage.SelectedIndex + 1, lvcajas9)
            Case 9
                Cargar_Cajas(tabpage.SelectedIndex + 1, lvcajas10)
        End Select
    End Sub


#End Region

End Class
