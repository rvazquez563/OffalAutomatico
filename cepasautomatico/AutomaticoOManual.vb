Public Class AutomaticoOManual
    Dim pallet As caja
    Dim I As Integer = 0
    Dim PLC As Libnodave_WinAC


    Public Sub New(p As caja, plc1 As Libnodave_WinAC)
        InitializeComponent()
        pallet = p
        PLC = plc1


    End Sub

    Private Sub AutomaticoOManual_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Dim lineas As List(Of Linea) = frmMain.Lineas
        cmbLinea.Items.Add("---SELECCIONE UNA LÍNEA---")
        'For Each l As Linea In lineas
        'If l.TieneOrden Then
        ' cmbLinea.Items.Add("Línea " + l.Numero + " - " + l.Orden.Numero.Trim)
        'Else
        'cmbLinea.Items.Add("Línea " + l.Numero + " - SIN ORDEN")
        'End If
        'Next
        If Not IsNothing(pallet) Then
            'Si es null es por que me entró un código que no pude descifrar
            'cmbLinea.SelectedIndex = Integer.Parse(pallet.Linea.Numero)
        Else
            cmbLinea.SelectedIndex = 0
        End If
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub RectangleShape1_Click(sender As Object, e As EventArgs) Handles RectangleShape1.Click

    End Sub

    Private Sub cmdReintentarAutomatico_Click(sender As Object, e As EventArgs) Handles cmdReintentarAutomatico.Click
        If cmbLinea.SelectedIndex = 0 Then
            Call MessageBox.Show("Debe seleccionar la línea para imprimir automáticamente")
            Exit Sub
        End If
        PLC.resetFinDeCiclo()
        PLC.resetFinDeCicloERR()

        'Creo el pallet a partir de la linea seleccionada
        ' Dim p As New Pallet(frmMain.Lineas.Item(cmbLinea.SelectedIndex - 1).Orden, frmMain.Lineas.Item(cmbLinea.SelectedIndex - 1))
        'frmMain.agregarAColaDeProduccion(p)
        PLC.resetFinDeCiclo()
        PLC.resetFinDeCicloERR()
        Me.DialogResult = Windows.Forms.DialogResult.Retry


    End Sub

    Private Sub cmdIntentarManual_Click(sender As Object, e As EventArgs) Handles cmdIntentarManual.Click
        Me.DialogResult = Windows.Forms.DialogResult.Yes
    End Sub

    Private Sub cmdEvacuarPalet_Click(sender As Object, e As EventArgs) Handles cmdEvacuarPalet.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub btSalir_Click(sender As Object, e As EventArgs) Handles btSalir.Click
        Me.DialogResult = Windows.Forms.DialogResult.Abort

    End Sub
End Class