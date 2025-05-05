Imports ketan.ketan.Cliente

Public Class frmConfig
    Private primeraConfig As Boolean = False
    Private impresoraManual As Impresora
    Dim WithEvents impresora_tcp As New ketan.ketan.Cliente

    Public Sub New(configPrevia As Boolean)
        InitializeComponent()
        primeraConfig = True
        Call MessageBox.Show("Por favor, configure el programa y precione GUARDAR")
    End Sub

    Public Sub New()

        ' Llamada necesaria para el diseñador.
        InitializeComponent()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Private Sub frmConfig_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If gsParam.HayConfigPrevia Then
            txtLetraCodBar.Text = gsParam.LetraCodBar
            txtDriverImpresoraPrincipal1.Text = gsParam.DriverImpresoraPrincipal(0)
            txtDriverImpresoraPrincipal2.Text = gsParam.DriverImpresoraPrincipal(1)
            txtDriverImpresoraPrincipal3.Text = gsParam.DriverImpresoraPrincipal(2)
            txtDriverImpresoraPrincipal5.Text = gsParam.DriverImpresoraPrincipal(4)
            txtDriverImpresoraPrincipal6.Text = gsParam.DriverImpresoraPrincipal(5)
            txtDriverImpresoraPrincipal7.Text = gsParam.DriverImpresoraPrincipal(6)
            txtDriverImpresoraPrincipal8.Text = gsParam.DriverImpresoraPrincipal(7)
            txtDriverImpresoraPrincipal9.Text = gsParam.DriverImpresoraPrincipal(8)
            txtDriverImpresoraPrincipal10.Text = gsParam.DriverImpresoraPrincipal(9)

            txtDriverImpresoraSecundaria1.Text = gsParam.DriverImpresoraSecundaria(0)
            txtDriverImpresoraSecundaria2.Text = gsParam.DriverImpresoraSecundaria(1)
            txtDriverImpresoraSecundaria3.Text = gsParam.DriverImpresoraSecundaria(2)
            txtDriverImpresoraSecundaria5.Text = gsParam.DriverImpresoraSecundaria(4)
            txtDriverImpresoraSecundaria6.Text = gsParam.DriverImpresoraSecundaria(5)
            txtDriverImpresoraSecundaria7.Text = gsParam.DriverImpresoraSecundaria(6)
            txtDriverImpresoraSecundaria8.Text = gsParam.DriverImpresoraSecundaria(7)
            txtDriverImpresoraSecundaria9.Text = gsParam.DriverImpresoraSecundaria(8)
            txtDriverImpresoraSecundaria10.Text = gsParam.DriverImpresoraSecundaria(9)
            txtConnSQL.Text = gsParam.ConnSQL
            txtUbicacionLogs.Text = gsParam.UbicacionLogs
            txtCantDBajoInsumo.Text = gsParam.CantidadDespuesBajoInsumo.ToString
            txtTandem.Text = gsParam.Tandem.ToString
            txtIPPLC.Text = gsParam.IPPLC
            chkDeshablin1.Checked = gsParam.Deshablin1(0)
            chkDeshablin2.Checked = gsParam.Deshablin1(1)
            chkDeshablin3.Checked = gsParam.Deshablin1(2)
            chkDeshablin5.Checked = gsParam.Deshablin1(4)
            chkDeshablin6.Checked = gsParam.Deshablin1(5)
            chkDeshablin7.Checked = gsParam.Deshablin1(6)
            chkDeshablin8.Checked = gsParam.Deshablin1(7)
            chkDeshablin9.Checked = gsParam.Deshablin1(8)
            chkDeshablin10.Checked = gsParam.Deshablin1(9)

            chkHab11.Checked = gsParam.DeshabImp(0, 0)
            chkHab12.Checked = gsParam.DeshabImp(0, 1)
            chkHab21.Checked = gsParam.DeshabImp(1, 0)
            chkHab22.Checked = gsParam.DeshabImp(1, 1)
            chkHab31.Checked = gsParam.DeshabImp(2, 0)
            chkHab32.Checked = gsParam.DeshabImp(2, 1)
            chkHab51.Checked = gsParam.DeshabImp(4, 0)
            chkHab52.Checked = gsParam.DeshabImp(4, 1)
            chkHab61.Checked = gsParam.DeshabImp(5, 0)
            chkHab62.Checked = gsParam.DeshabImp(5, 1)
            chkHab71.Checked = gsParam.DeshabImp(6, 0)
            chkHab72.Checked = gsParam.DeshabImp(6, 1)
            chkHab81.Checked = gsParam.DeshabImp(7, 0)
            chkHab82.Checked = gsParam.DeshabImp(7, 1)
            chkHab91.Checked = gsParam.DeshabImp(8, 0)
            chkHab92.Checked = gsParam.DeshabImp(8, 1)
            chkHab101.Checked = gsParam.DeshabImp(9, 0)
            chkHab102.Checked = gsParam.DeshabImp(9, 1)
        End If
    End Sub

    Private Sub cmdCancelar_Click(sender As Object, e As EventArgs) Handles cmdCancelar.Click
        If primeraConfig Then
            Call MessageBox.Show("No puede iniciar la aplicación sin haber hecho la primer configuración")
            End
        End If
        Me.Dispose()
    End Sub

    Private Sub cmdTest_Click(sender As Object, e As EventArgs) Handles cmdTest.Click
        If Repositorio.test(txtConnSQL.Text) Then
            Call MessageBox.Show("Conexión exitosa!")
        Else
            Call MessageBox.Show("Error de conexión")
        End If
    End Sub

    Private Sub cmdBuscarUbicacionLogs_Click(sender As Object, e As EventArgs) Handles cmdBuscarUbicacionLogs.Click
        FolderBrowserDialog.ShowNewFolderButton = True
        FolderBrowserDialog.Description = "Carpeta en donde se creará la carpeta de LOGS"
        If FolderBrowserDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtUbicacionLogs.Text = FolderBrowserDialog.SelectedPath
        End If
    End Sub

    Private Sub cmdAgregarUsuario_Click(sender As Object, e As EventArgs) Handles cmdAgregarUsuario.Click
        Dim nuevo As New frmNuevoUsuario
        nuevo.ShowDialog()
    End Sub

    Private Sub cmdEditarUsuario_Click(sender As Object, e As EventArgs) Handles cmdEditarUsuario.Click
        Dim editar As New frmEditarUsuarios
        editar.ShowDialog()
    End Sub

    Private Sub cmdGuardar_Click(sender As Object, e As EventArgs) Handles cmdGuardar.Click
        Dim contador As Integer = 0
        For Each c1 As Control In Me.Controls
            If TypeOf c1 Is GroupBox Then
                For Each c2 As Control In c1.Controls
                    If TypeOf c2 Is TextBox Then
                        If c2.Text = "" Then
                            Call MessageBox.Show("Debe completar TODOS los campos")
                            c2.Focus()
                            Exit Sub
                        End If
                    End If
                Next
            End If
        Next

        gsParam.ConnSQL = txtConnSQL.Text
        gsParam.DriverImpresoraPrincipal(0) = txtDriverImpresoraPrincipal1.Text
        gsParam.DriverImpresoraPrincipal(1) = txtDriverImpresoraPrincipal2.Text
        gsParam.DriverImpresoraPrincipal(2) = txtDriverImpresoraPrincipal3.Text
        gsParam.DriverImpresoraPrincipal(4) = txtDriverImpresoraPrincipal5.Text
        gsParam.DriverImpresoraPrincipal(5) = txtDriverImpresoraPrincipal6.Text
        gsParam.DriverImpresoraPrincipal(6) = txtDriverImpresoraPrincipal7.Text
        gsParam.DriverImpresoraPrincipal(7) = txtDriverImpresoraPrincipal8.Text
        gsParam.DriverImpresoraPrincipal(8) = txtDriverImpresoraPrincipal9.Text
        gsParam.DriverImpresoraPrincipal(9) = txtDriverImpresoraPrincipal10.Text

        gsParam.DriverImpresoraSecundaria(0) = txtDriverImpresoraSecundaria1.Text
        gsParam.DriverImpresoraSecundaria(1) = txtDriverImpresoraSecundaria2.Text
        gsParam.DriverImpresoraSecundaria(2) = txtDriverImpresoraSecundaria3.Text
        gsParam.DriverImpresoraSecundaria(4) = txtDriverImpresoraSecundaria5.Text
        gsParam.DriverImpresoraSecundaria(5) = txtDriverImpresoraSecundaria6.Text
        gsParam.DriverImpresoraSecundaria(6) = txtDriverImpresoraSecundaria7.Text
        gsParam.DriverImpresoraSecundaria(7) = txtDriverImpresoraSecundaria8.Text
        gsParam.DriverImpresoraSecundaria(8) = txtDriverImpresoraSecundaria9.Text
        gsParam.DriverImpresoraSecundaria(9) = txtDriverImpresoraSecundaria10.Text
        gsParam.UbicacionLogs = txtUbicacionLogs.Text
        gsParam.LetraCodBar = txtLetraCodBar.Text
        gsParam.CantidadDespuesBajoInsumo = Integer.Parse(txtCantDBajoInsumo.Text)
        gsParam.Tandem = Integer.Parse(txtTandem.Text)
        gsParam.IPPLC = txtIPPLC.Text
        gsParam.HayConfigPrevia = True
        gsParam.Deshablin1(0) = chkDeshablin1.Checked
        gsParam.Deshablin1(1) = chkDeshablin2.Checked
        gsParam.Deshablin1(2) = chkDeshablin3.Checked
        gsParam.Deshablin1(4) = chkDeshablin5.Checked
        gsParam.Deshablin1(5) = chkDeshablin6.Checked
        gsParam.Deshablin1(6) = chkDeshablin7.Checked
        gsParam.Deshablin1(7) = chkDeshablin8.Checked
        gsParam.Deshablin1(8) = chkDeshablin9.Checked
        gsParam.Deshablin1(9) = chkDeshablin10.Checked

        gsParam.DeshabImp(0, 0) = chkHab11.Checked
        gsParam.DeshabImp(0, 1) = chkHab12.Checked
        gsParam.DeshabImp(1, 0) = chkHab21.Checked
        gsParam.DeshabImp(1, 1) = chkHab22.Checked
        gsParam.DeshabImp(2, 0) = chkHab31.Checked
        gsParam.DeshabImp(2, 1) = chkHab32.Checked
        gsParam.DeshabImp(4, 0) = chkHab51.Checked
        gsParam.DeshabImp(4, 1) = chkHab52.Checked
        gsParam.DeshabImp(5, 0) = chkHab61.Checked
        gsParam.DeshabImp(5, 1) = chkHab62.Checked
        gsParam.DeshabImp(6, 0) = chkHab71.Checked
        gsParam.DeshabImp(6, 1) = chkHab72.Checked
        gsParam.DeshabImp(7, 0) = chkHab81.Checked
        gsParam.DeshabImp(7, 1) = chkHab82.Checked
        gsParam.DeshabImp(8, 0) = chkHab91.Checked
        gsParam.DeshabImp(8, 1) = chkHab92.Checked
        gsParam.DeshabImp(9, 0) = chkHab101.Checked
        gsParam.DeshabImp(9, 1) = chkHab102.Checked
        Call guardarParametros()
        Me.Dispose()
    End Sub

    Private Sub guardarParametros()
        Call SaveSetting("KTNO", "Options", "sConnSQL", gsParam.ConnSQL)

        Call SaveSetting("KTNO", "Options", "sLetraCodBar", gsParam.LetraCodBar)
        Call SaveSetting("KTNO", "Options", "sUbicacionLogs", gsParam.UbicacionLogs)
        Call SaveSetting("KTNO", "Options", "DriverImpresoraPrincipal1", gsParam.DriverImpresoraPrincipal(0))
        Call SaveSetting("KTNO", "Options", "DriverImpresoraPrincipal2", gsParam.DriverImpresoraPrincipal(1))
        Call SaveSetting("KTNO", "Options", "DriverImpresoraPrincipal3", gsParam.DriverImpresoraPrincipal(2))
        Call SaveSetting("KTNO", "Options", "DriverImpresoraPrincipal5", gsParam.DriverImpresoraPrincipal(4))
        Call SaveSetting("KTNO", "Options", "DriverImpresoraPrincipal6", gsParam.DriverImpresoraPrincipal(5))
        Call SaveSetting("KTNO", "Options", "DriverImpresoraPrincipal7", gsParam.DriverImpresoraPrincipal(6))
        Call SaveSetting("KTNO", "Options", "DriverImpresoraPrincipal8", gsParam.DriverImpresoraPrincipal(7))
        Call SaveSetting("KTNO", "Options", "DriverImpresoraPrincipal9", gsParam.DriverImpresoraPrincipal(8))
        Call SaveSetting("KTNO", "Options", "DriverImpresoraPrincipal10", gsParam.DriverImpresoraPrincipal(9))

        Call SaveSetting("KTNO", "Options", "DriverImpresoraSecundaria1", gsParam.DriverImpresoraSecundaria(0))
        Call SaveSetting("KTNO", "Options", "DriverImpresoraSecundaria2", gsParam.DriverImpresoraSecundaria(1))
        Call SaveSetting("KTNO", "Options", "DriverImpresoraSecundaria3", gsParam.DriverImpresoraSecundaria(2))
        Call SaveSetting("KTNO", "Options", "DriverImpresoraSecundaria5", gsParam.DriverImpresoraSecundaria(4))
        Call SaveSetting("KTNO", "Options", "DriverImpresoraSecundaria6", gsParam.DriverImpresoraSecundaria(5))
        Call SaveSetting("KTNO", "Options", "DriverImpresoraSecundaria7", gsParam.DriverImpresoraSecundaria(6))
        Call SaveSetting("KTNO", "Options", "DriverImpresoraSecundaria8", gsParam.DriverImpresoraSecundaria(7))
        Call SaveSetting("KTNO", "Options", "DriverImpresoraSecundaria9", gsParam.DriverImpresoraSecundaria(8))
        Call SaveSetting("KTNO", "Options", "DriverImpresoraSecundaria10", gsParam.DriverImpresoraSecundaria(9))

        Call SaveSetting("KTNO", "Options", "sIPPLC", gsParam.IPPLC)
        Call SaveSetting("KTNO", "Options", "sTandem", gsParam.Tandem)
        Call SaveSetting("KTNO", "Options", "sCantidadDespuesBajoInsumo", gsParam.CantidadDespuesBajoInsumo)
        Call SaveSetting("KTNO", "Options", "sHayConfigPrevia", gsParam.HayConfigPrevia)

        Call SaveSetting("KTNO", "Options", "sDeshablin1", gsParam.Deshablin1(0))
        Call SaveSetting("KTNO", "Options", "sDeshablin2", gsParam.Deshablin1(1))
        Call SaveSetting("KTNO", "Options", "sDeshablin3", gsParam.Deshablin1(2))
        Call SaveSetting("KTNO", "Options", "sDeshablin5", gsParam.Deshablin1(4))
        Call SaveSetting("KTNO", "Options", "sDeshablin6", gsParam.Deshablin1(5))
        Call SaveSetting("KTNO", "Options", "sDeshablin7", gsParam.Deshablin1(6))
        Call SaveSetting("KTNO", "Options", "sDeshablin8", gsParam.Deshablin1(7))
        Call SaveSetting("KTNO", "Options", "sDeshablin9", gsParam.Deshablin1(8))
        Call SaveSetting("KTNO", "Options", "sDeshablin10", gsParam.Deshablin1(9))

        Call SaveSetting("KTNO", "Options", "sDeshabImp11", gsParam.DeshabImp(0, 0))
        Call SaveSetting("KTNO", "Options", "sDeshabImp12", gsParam.DeshabImp(0, 1))
        Call SaveSetting("KTNO", "Options", "sDeshabImp21", gsParam.DeshabImp(1, 0))
        Call SaveSetting("KTNO", "Options", "sDeshabImp22", gsParam.DeshabImp(1, 1))
        Call SaveSetting("KTNO", "Options", "sDeshabImp31", gsParam.DeshabImp(2, 0))
        Call SaveSetting("KTNO", "Options", "sDeshabImp32", gsParam.DeshabImp(2, 1))

        Call SaveSetting("KTNO", "Options", "sDeshabImp51", gsParam.DeshabImp(4, 0))
        Call SaveSetting("KTNO", "Options", "sDeshabImp52", gsParam.DeshabImp(4, 1))
        Call SaveSetting("KTNO", "Options", "sDeshabImp61", gsParam.DeshabImp(5, 0))
        Call SaveSetting("KTNO", "Options", "sDeshabImp62", gsParam.DeshabImp(5, 1))
        Call SaveSetting("KTNO", "Options", "sDeshabImp71", gsParam.DeshabImp(6, 0))
        Call SaveSetting("KTNO", "Options", "sDeshabImp72", gsParam.DeshabImp(6, 1))

        Call SaveSetting("KTNO", "Options", "sDeshabImp81", gsParam.DeshabImp(7, 0))
        Call SaveSetting("KTNO", "Options", "sDeshabImp82", gsParam.DeshabImp(7, 1))
        Call SaveSetting("KTNO", "Options", "sDeshabImp91", gsParam.DeshabImp(8, 0))
        Call SaveSetting("KTNO", "Options", "sDeshabImp92", gsParam.DeshabImp(8, 1))
        Call SaveSetting("KTNO", "Options", "sDeshabImp101", gsParam.DeshabImp(9, 0))
        Call SaveSetting("KTNO", "Options", "sDeshabImp102", gsParam.DeshabImp(9, 1))


    End Sub


    Private Sub cmdPing_Click(sender As Object, e As EventArgs) Handles cmdPing.Click
        Try
            If My.Computer.Network.Ping(Me.txtIPPLC.Text) Then
                MessageBox.Show("Conexion correcta", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Conexion erronea", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Net.NetworkInformation.PingException
            MessageBox.Show("Ocurrió el siguiente error:" & vbCrLf & ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("Ocurrió el siguiente error:" & vbCrLf & ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        ' impresoraManual = New Impresora(txtDriverImpresoraPrincipal1.Text, Impresora.tipo.manual)
        '  If impresoraManual.existeDriver = False Then
        '  Call MessageBox.Show("No se ecuentra el driver de la impresora manual: " + impresoraManual.Driver + " No se iniciará el etiquetado")
        '   Logs.nuevo("No se ecuentra el driver de la impresora automática: " + impresoraManual.Driver + " No se iniciará el etiquetado")
        ' Exit Sub
        ' Else
        'escribirMensaje("Impresora manual OK")
        ' Logs.nuevo("Impresora manual OK")
        'impresoraManual.imprimirprueba()

        ' End If
        Dim resultado As String = imprimirTCP(txtDriverImpresoraPrincipal1.Text)

        If resultado.Contains("OK") Then
            MessageBox.Show("Conexion correcta", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Conexion erronea: " + resultado, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        'impresoraManual = New Impresora(txtDriverImpresoraPrincipal2.Text, Impresora.tipo.manual)
        'If impresoraManual.existeDriver = False Then
        'Call MessageBox.Show("No se ecuentra el driver de la impresora manual: " + impresoraManual.Driver + " No se iniciará el etiquetado")
        'Logs.nuevo("No se ecuentra el driver de la impresora automática: " + impresoraManual.Driver + " No se iniciará el etiquetado")
        'Exit Sub
        'Else
        ''escribirMensaje("Impresora manual OK")
        'Logs.nuevo("Impresora manual OK")
        'impresoraManual.imprimirprueba()

        'End If
        Dim resultado As String = imprimirTCP(txtDriverImpresoraPrincipal2.Text)

        If resultado.Contains("OK") Then
            MessageBox.Show("Conexion correcta", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Conexion erronea: " + resultado, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        'impresoraManual = New Impresora(txtDriverImpresoraPrincipal3.Text, Impresora.tipo.manual)
        'If impresoraManual.existeDriver = False Then
        ' Call MessageBox.Show("No se ecuentra el driver de la impresora manual: " + impresoraManual.Driver + " No se iniciará el etiquetado")
        ' Logs.nuevo("No se ecuentra el driver de la impresora automática: " + impresoraManual.Driver + " No se iniciará el etiquetado")
        ' Exit Sub
        ' Else
        ' 'escribirMensaje("Impresora manual OK")
        ' Logs.nuevo("Impresora manual OK")
        ' impresoraManual.imprimirprueba()

        'End If
        Dim resultado As String = imprimirTCP(txtDriverImpresoraPrincipal3.Text)

        If resultado.Contains("OK") Then
            MessageBox.Show("Conexion correcta", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Conexion erronea: " + resultado, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        'impresoraManual = New Impresora(txtDriverImpresoraSecundaria1.Text, Impresora.tipo.manual)
        'If impresoraManual.existeDriver = False Then
        ' Call MessageBox.Show("No se ecuentra el driver de la impresora manual: " + impresoraManual.Driver + " No se iniciará el etiquetado")
        ' Logs.nuevo("No se ecuentra el driver de la impresora automática: " + impresoraManual.Driver + " No se iniciará el etiquetado")
        ' Exit Sub
        ' Else
        ' 'escribirMensaje("Impresora manual OK")
        ' Logs.nuevo("Impresora manual OK")
        ' impresoraManual.imprimirprueba()
        '
        '        End If
        Dim resultado As String = imprimirTCP(txtDriverImpresoraSecundaria1.Text)

        If resultado.Contains("OK") Then
            MessageBox.Show("Conexion correcta", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Conexion erronea: " + resultado, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        'impresoraManual = New Impresora(txtDriverImpresoraSecundaria2.Text, Impresora.tipo.manual)
        'If impresoraManual.existeDriver = False Then
        ' Call MessageBox.Show("No se ecuentra el driver de la impresora manual: " + impresoraManual.Driver + " No se iniciará el etiquetado")
        'Logs.nuevo("No se ecuentra el driver de la impresora automática: " + impresoraManual.Driver + " No se iniciará el etiquetado")
        'Exit Sub
        'Else
        'escribirMensaje("Impresora manual OK")
        ' Logs.nuevo("Impresora manual OK")
        'impresoraManual.imprimirprueba()

        'End If
        Dim resultado As String = imprimirTCP(txtDriverImpresoraSecundaria2.Text)

        If resultado.Contains("OK") Then
            MessageBox.Show("Conexion correcta", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Conexion erronea: " + resultado, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
        'impresoraManual = New Impresora(txtDriverImpresoraSecundaria3.Text, Impresora.tipo.manual)
        'If impresoraManual.existeDriver = False Then
        'Call MessageBox.Show("No se ecuentra el driver de la impresora manual: " + impresoraManual.Driver + " No se iniciará el etiquetado")
        'Logs.nuevo("No se ecuentra el driver de la impresora automática: " + impresoraManual.Driver + " No se iniciará el etiquetado")
        'Exit Sub
        'Else
        ''escribirMensaje("Impresora manual OK")
        'Logs.nuevo("Impresora manual OK")
        'impresoraManual.imprimirprueba()

        'End If
        Dim resultado As String = imprimirTCP(txtDriverImpresoraSecundaria3.Text)

        If resultado.Contains("OK") Then
            MessageBox.Show("Conexion correcta", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Conexion erronea: " + resultado, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Function imprimirTCP(ip As String) As String
        Dim resultado As String

        impresora_tcp = New ketan.ketan.Cliente
        impresora_tcp.IPDelHost = ip
        impresora_tcp.PuertoDelHost = "9100"
        Try
            impresora_tcp.Conectar()
            'MsgBox("Se conectó correctamente la impresora de salida")
        Catch ex As Exception
            Logs.nuevo("Error conectando con la impresora de ip:" + ip)
            resultado = "Error conectando con la impresora de ip:" + ip
            Return resultado
            Exit Function
        End Try
        Dim etiqueta As String = ""
        Dim sChr = "^"
        Dim sVto As String

        ''Guardo los datos a imprimir en una variable
        ' etiqueta += "~" + "HI" + vbCrLf
        etiqueta += sChr + "XA" + vbCrLf
        etiqueta += sChr + "WD" + vbCrLf
        etiqueta += sChr + "XZ" + vbCrLf
        resultado = impresora_tcp.EnviarDatos(etiqueta)

        If resultado.Contains("OK") Then
            impresora_tcp.Desconectar()
            Return resultado
        Else
            Logs.nuevo(resultado)
            impresora_tcp.Desconectar()
            Return resultado
        End If
        ' Threading.Thread.Sleep(30)


    End Function

    Private Sub chkDeshablin1_CheckedChanged(sender As Object, e As EventArgs)
        If chkDeshablin1.Checked Then
            gbImpresora1.Enabled = False
        Else
            gbImpresora1.Enabled = True

        End If
    End Sub

    Private Sub chkDeshablin2_CheckedChanged(sender As Object, e As EventArgs)
        If chkDeshablin2.Checked Then
            gbImpresora2.Enabled = False
        Else
            gbImpresora2.Enabled = True

        End If
    End Sub

    Private Sub chkDeshablin3_CheckedChanged(sender As Object, e As EventArgs)
        If chkDeshablin3.Checked Then
            gbImpresora3.Enabled = False
        Else
            gbImpresora3.Enabled = True

        End If
    End Sub

    Private Sub chkHab11_CheckedChanged(sender As Object, e As EventArgs)
        If chkHab11.Checked Then
            txtDriverImpresoraPrincipal1.Enabled = False
        Else
            txtDriverImpresoraPrincipal1.Enabled = True

        End If
    End Sub

    Private Sub chkHab21_CheckedChanged(sender As Object, e As EventArgs)
        If chkHab21.Checked Then
            txtDriverImpresoraPrincipal2.Enabled = False
        Else
            txtDriverImpresoraPrincipal2.Enabled = True

        End If
    End Sub

    Private Sub chkHab31_CheckedChanged(sender As Object, e As EventArgs)
        If chkHab31.Checked Then
            txtDriverImpresoraPrincipal3.Enabled = False
        Else
            txtDriverImpresoraPrincipal3.Enabled = True

        End If
    End Sub

    Private Sub chkHab12_CheckedChanged(sender As Object, e As EventArgs)
        If chkHab12.Checked Then
            txtDriverImpresoraSecundaria1.Enabled = False
        Else
            txtDriverImpresoraSecundaria1.Enabled = True

        End If
    End Sub

    Private Sub chkHab22_CheckedChanged(sender As Object, e As EventArgs)
        If chkHab22.Checked Then
            txtDriverImpresoraSecundaria2.Enabled = False
        Else
            txtDriverImpresoraSecundaria2.Enabled = True

        End If
    End Sub

    Private Sub chkHab32_CheckedChanged(sender As Object, e As EventArgs)
        If chkHab32.Checked Then
            txtDriverImpresoraSecundaria3.Enabled = False
        Else
            txtDriverImpresoraSecundaria3.Enabled = True

        End If
    End Sub

    Private Sub chkDeshablin3_CheckedChanged_1(sender As Object, e As EventArgs) Handles chkDeshablin3.CheckedChanged
        If chkDeshablin3.Checked Then
            gbImpresora3.Enabled = False
        Else
            gbImpresora3.Enabled = True

        End If
    End Sub

    Private Sub chkDeshablin1_CheckedChanged_1(sender As Object, e As EventArgs) Handles chkDeshablin1.CheckedChanged
        If chkDeshablin1.Checked Then
            gbImpresora1.Enabled = False
        Else
            gbImpresora1.Enabled = True

        End If
    End Sub

    Private Sub chkDeshablin2_CheckedChanged_1(sender As Object, e As EventArgs) Handles chkDeshablin2.CheckedChanged
        If chkDeshablin2.Checked Then
            gbImpresora2.Enabled = False
        Else
            gbImpresora2.Enabled = True

        End If
    End Sub

    Private Sub chkDeshablin5_CheckedChanged(sender As Object, e As EventArgs) Handles chkDeshablin5.CheckedChanged
        If chkDeshablin5.Checked Then
            gbImpresora5.Enabled = False
        Else
            gbImpresora5.Enabled = True

        End If
    End Sub

    Private Sub chkDeshablin6_CheckedChanged(sender As Object, e As EventArgs) Handles chkDeshablin6.CheckedChanged
        If chkDeshablin6.Checked Then
            gbImpresora6.Enabled = False
        Else
            gbImpresora6.Enabled = True

        End If
    End Sub

    Private Sub chkDeshablin7_CheckedChanged(sender As Object, e As EventArgs) Handles chkDeshablin7.CheckedChanged
        If chkDeshablin7.Checked Then
            gbImpresora7.Enabled = False
        Else
            gbImpresora7.Enabled = True

        End If
    End Sub

    Private Sub chkDeshablin8_CheckedChanged(sender As Object, e As EventArgs) Handles chkDeshablin8.CheckedChanged
        If chkDeshablin8.Checked Then
            gbImpresora8.Enabled = False
        Else
            gbImpresora8.Enabled = True

        End If
    End Sub

    Private Sub chkDeshablin9_CheckedChanged(sender As Object, e As EventArgs) Handles chkDeshablin9.CheckedChanged
        If chkDeshablin9.Checked Then
            gbImpresora9.Enabled = False
        Else
            gbImpresora9.Enabled = True

        End If
    End Sub

    Private Sub chkDeshablin10_CheckedChanged(sender As Object, e As EventArgs) Handles chkDeshablin10.CheckedChanged
        If chkDeshablin10.Checked Then
            gbImpresora10.Enabled = False
        Else
            gbImpresora10.Enabled = True

        End If
    End Sub

    Private Sub cmdtest11_Click(sender As Object, e As EventArgs) Handles cmdtest11.Click
        Dim resultado As String
        For i = 0 To 100
            resultado = imprimirTCP(txtDriverImpresoraPrincipal1.Text)
        Next


        If resultado.Contains("OK") Then
            MessageBox.Show("Conexion correcta", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Conexion erronea: " + resultado, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click

    End Sub
End Class