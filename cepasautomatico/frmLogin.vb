Public Class frmlogin


    Private _usuario As Usuario

    Sub New(ByRef usuario As Usuario)
        InitializeComponent()
        _usuario = usuario
    End Sub

    Private Sub cmdIngresar_Click(sender As Object, e As EventArgs) Handles cmdIngresar.Click
        If txtPassword.Text = "" Or txtUsuario.Text = "" Then
            Call MessageBox.Show("Debe ingresar usuario y contraseña")
        End If
        If _usuario.loguearUsuario(txtUsuario.Text, txtPassword.Text) Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Else
            Me.DialogResult = Windows.Forms.DialogResult.No
        End If

    End Sub

    Private Sub frmLoguear_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        'Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub frmLoguear_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub txtUsuario_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtUsuario.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txtPassword.Focus()
        End If
    End Sub

    Private Sub txtPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cmdIngresar_Click(Nothing, Nothing)
        End If
    End Sub
End Class
