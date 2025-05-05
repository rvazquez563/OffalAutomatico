Public Class frmEditarUsuarios

    Private listaUsuarios As List(Of Usuario)

    Private Sub frmEditarUsuario_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Cargar_Usuarios()
    End Sub

    Private Sub cmdEditarUsuario_Click(sender As Object, e As EventArgs) Handles cmdEditarUsuario.Click
        If lvUsuarios.SelectedItems.Count = 0 Then
            Call MessageBox.Show("Debe seleccionar un usuario para modificarlo")
        Else
            Dim p As New Point(24, 36)
            gbEditarUsuario.Location = p
            txtUsuario.Text = lvUsuarios.SelectedItems.Item(0).Text
            If lvUsuarios.SelectedItems.Item(0).SubItems(1).Text = "Administrador" Then
                cbAcceso.Text = cbAcceso.Items.Item(0)
            Else
                cbAcceso.Text = cbAcceso.Items.Item(1)
            End If


        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

    End Sub

    Private Sub Cargar_Usuarios()
        lvUsuarios.Clear()
        lvUsuarios.Columns.Add("Usuario")
        lvUsuarios.Columns.Add("Tipo")
        lvUsuarios.FullRowSelect = True
        lvUsuarios.MultiSelect = False
        lvUsuarios.Scrollable = True
        lvUsuarios.View = View.Details
        listaUsuarios = Repositorio.obtenerTodosLosUsuarios()
        For Each usr In listaUsuarios
            Dim item As New ListViewItem({usr.Nombre, usr.Acceso})
            lvUsuarios.Items.Add(item)
        Next
    End Sub

    Private Sub cmdGuardarCambios_Click(sender As Object, e As EventArgs) Handles cmdGuardarCambios.Click

    End Sub

    Private Sub cmdSalir_Click(sender As Object, e As EventArgs) Handles cmdSalir.Click
        Me.Dispose()
    End Sub

    Private Sub cmdEliminarUsuario_Click(sender As Object, e As EventArgs) Handles cmdEliminarUsuario.Click
        If lvUsuarios.SelectedItems.Count = 0 Then
            Call MessageBox.Show("Debe seleccionar un usuario para eliminarlo")
        Else
            If MessageBox.Show("Esta seguro que desea eleminar el usuario: " & lvUsuarios.SelectedItems.Item(0).Text & "?", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Dim usr As New Usuario
                usr.Nombre = lvUsuarios.SelectedItems.Item(0).Text
                If usr.eliminarUsuario() Then
                    Call MessageBox.Show("El usuario se eliminó correctamente")
                    Cargar_Usuarios()
                Else
                    MessageBox.Show("Error eliminando el usuario")
                End If

            End If
        End If
    End Sub
End Class