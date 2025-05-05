Public Class frmNuevoUsuario

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles cmdCrear.Click
        If txtContraseña.Text = "" Or txtContraseña.TextLength > 15 Then
            Call MessageBox.Show("Debe ingresar una contraseña menor o igual a 15 caracteres")
            txtContraseña.Focus()
            Exit Sub
        End If

        If txtUsuario.Text = "" Then
            Call MessageBox.Show("Debe ingresar un nombre de usuario")
            txtUsuario.Focus()
            Exit Sub
        End If

        If cbAcceso.Text = "" Then
            Call MessageBox.Show("Debe seleccionar un nivel de acceso")
            cbAcceso.Focus()
            Exit Sub
        End If

        Dim nuevoUsuario As New Usuario()
        nuevoUsuario.Nombre = txtUsuario.Text
        nuevoUsuario.Contraseña = SHA1.encriptar(txtContraseña.Text)
        nuevoUsuario.Acceso = cbAcceso.Text

        If nuevoUsuario.guardarUsuario Then
            MessageBox.Show("Usuario generado correctamente")
            txtUsuario.Text = ""
            txtContraseña.Text = ""
            cbAcceso.SelectedItem = 0
        Else
            MessageBox.Show("El nombre de usuario elegido ya existe")
        End If

    End Sub

    Private Sub cmdCancelar_Click(sender As Object, e As EventArgs) Handles cmdCancelar.Click
        Me.Dispose()
    End Sub
End Class