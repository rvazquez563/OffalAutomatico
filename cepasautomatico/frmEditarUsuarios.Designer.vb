<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEditarUsuarios
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.cmdSalir = New System.Windows.Forms.Button()
        Me.cmdEditarUsuario = New System.Windows.Forms.Button()
        Me.cmdEliminarUsuario = New System.Windows.Forms.Button()
        Me.lvUsuarios = New System.Windows.Forms.ListView()
        Me.Usuario = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Permisos = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.gbEditarUsuario = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.cmdGuardarCambios = New System.Windows.Forms.Button()
        Me.cbAcceso = New System.Windows.Forms.ComboBox()
        Me.txtContraseña = New System.Windows.Forms.TextBox()
        Me.txtUsuario = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gbEditarUsuario.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdSalir
        '
        Me.cmdSalir.Location = New System.Drawing.Point(266, 256)
        Me.cmdSalir.Name = "cmdSalir"
        Me.cmdSalir.Size = New System.Drawing.Size(104, 72)
        Me.cmdSalir.TabIndex = 11
        Me.cmdSalir.Text = "Salir"
        Me.cmdSalir.UseVisualStyleBackColor = True
        '
        'cmdEditarUsuario
        '
        Me.cmdEditarUsuario.Location = New System.Drawing.Point(266, 14)
        Me.cmdEditarUsuario.Name = "cmdEditarUsuario"
        Me.cmdEditarUsuario.Size = New System.Drawing.Size(104, 72)
        Me.cmdEditarUsuario.TabIndex = 10
        Me.cmdEditarUsuario.Text = "Editar seleccionado"
        Me.cmdEditarUsuario.UseVisualStyleBackColor = True
        '
        'cmdEliminarUsuario
        '
        Me.cmdEliminarUsuario.Location = New System.Drawing.Point(266, 136)
        Me.cmdEliminarUsuario.Name = "cmdEliminarUsuario"
        Me.cmdEliminarUsuario.Size = New System.Drawing.Size(104, 72)
        Me.cmdEliminarUsuario.TabIndex = 9
        Me.cmdEliminarUsuario.Text = "Eliminar seleccionado"
        Me.cmdEliminarUsuario.UseVisualStyleBackColor = True
        '
        'lvUsuarios
        '
        Me.lvUsuarios.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Usuario, Me.Permisos})
        Me.lvUsuarios.FullRowSelect = True
        Me.lvUsuarios.Location = New System.Drawing.Point(14, 14)
        Me.lvUsuarios.Name = "lvUsuarios"
        Me.lvUsuarios.Size = New System.Drawing.Size(221, 313)
        Me.lvUsuarios.TabIndex = 8
        Me.lvUsuarios.UseCompatibleStateImageBehavior = False
        Me.lvUsuarios.View = System.Windows.Forms.View.Details
        '
        'Usuario
        '
        Me.Usuario.Text = "Usuario"
        Me.Usuario.Width = 82
        '
        'Permisos
        '
        Me.Permisos.Text = "Tipo"
        Me.Permisos.Width = 103
        '
        'gbEditarUsuario
        '
        Me.gbEditarUsuario.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.gbEditarUsuario.Controls.Add(Me.Label4)
        Me.gbEditarUsuario.Controls.Add(Me.Button5)
        Me.gbEditarUsuario.Controls.Add(Me.cmdGuardarCambios)
        Me.gbEditarUsuario.Controls.Add(Me.cbAcceso)
        Me.gbEditarUsuario.Controls.Add(Me.txtContraseña)
        Me.gbEditarUsuario.Controls.Add(Me.txtUsuario)
        Me.gbEditarUsuario.Controls.Add(Me.Label3)
        Me.gbEditarUsuario.Controls.Add(Me.Label2)
        Me.gbEditarUsuario.Controls.Add(Me.Label1)
        Me.gbEditarUsuario.Location = New System.Drawing.Point(387, 51)
        Me.gbEditarUsuario.Name = "gbEditarUsuario"
        Me.gbEditarUsuario.Size = New System.Drawing.Size(275, 220)
        Me.gbEditarUsuario.TabIndex = 12
        Me.gbEditarUsuario.TabStop = False
        Me.gbEditarUsuario.Text = "Editar usuario"
        '
        'Label4
        '
        Me.Label4.ForeColor = System.Drawing.Color.Red
        Me.Label4.Location = New System.Drawing.Point(15, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(246, 29)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "La contraseña NO se puede modificar, se debe ingresar para realizar los cambios"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(151, 149)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(75, 57)
        Me.Button5.TabIndex = 13
        Me.Button5.Text = "Cancelar"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'cmdGuardarCambios
        '
        Me.cmdGuardarCambios.Location = New System.Drawing.Point(31, 149)
        Me.cmdGuardarCambios.Name = "cmdGuardarCambios"
        Me.cmdGuardarCambios.Size = New System.Drawing.Size(75, 57)
        Me.cmdGuardarCambios.TabIndex = 12
        Me.cmdGuardarCambios.Text = "Guardar cambios"
        Me.cmdGuardarCambios.UseVisualStyleBackColor = True
        '
        'cbAcceso
        '
        Me.cbAcceso.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbAcceso.FormattingEnabled = True
        Me.cbAcceso.Items.AddRange(New Object() {"Administrador", "Operador"})
        Me.cbAcceso.Location = New System.Drawing.Point(98, 112)
        Me.cbAcceso.Name = "cbAcceso"
        Me.cbAcceso.Size = New System.Drawing.Size(128, 23)
        Me.cbAcceso.TabIndex = 11
        '
        'txtContraseña
        '
        Me.txtContraseña.Location = New System.Drawing.Point(98, 80)
        Me.txtContraseña.Name = "txtContraseña"
        Me.txtContraseña.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtContraseña.Size = New System.Drawing.Size(128, 23)
        Me.txtContraseña.TabIndex = 10
        '
        'txtUsuario
        '
        Me.txtUsuario.Location = New System.Drawing.Point(98, 48)
        Me.txtUsuario.Name = "txtUsuario"
        Me.txtUsuario.Size = New System.Drawing.Size(128, 23)
        Me.txtUsuario.TabIndex = 9
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(5, 115)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(119, 15)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Nivel de acceso:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(28, 83)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 15)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Contraseña:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(46, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 15)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Usuario:"
        '
        'frmEditarUsuarios
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(380, 336)
        Me.Controls.Add(Me.gbEditarUsuario)
        Me.Controls.Add(Me.cmdSalir)
        Me.Controls.Add(Me.cmdEditarUsuario)
        Me.Controls.Add(Me.cmdEliminarUsuario)
        Me.Controls.Add(Me.lvUsuarios)
        Me.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmEditarUsuarios"
        Me.Text = "Editar usuarios"
        Me.gbEditarUsuario.ResumeLayout(False)
        Me.gbEditarUsuario.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmdSalir As System.Windows.Forms.Button
    Friend WithEvents cmdEditarUsuario As System.Windows.Forms.Button
    Friend WithEvents cmdEliminarUsuario As System.Windows.Forms.Button
    Friend WithEvents lvUsuarios As System.Windows.Forms.ListView
    Friend WithEvents Usuario As System.Windows.Forms.ColumnHeader
    Friend WithEvents Permisos As System.Windows.Forms.ColumnHeader
    Friend WithEvents gbEditarUsuario As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents cmdGuardarCambios As System.Windows.Forms.Button
    Friend WithEvents cbAcceso As System.Windows.Forms.ComboBox
    Friend WithEvents txtContraseña As System.Windows.Forms.TextBox
    Friend WithEvents txtUsuario As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
