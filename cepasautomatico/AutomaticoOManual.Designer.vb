<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AutomaticoOManual
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmdReintentarAutomatico = New System.Windows.Forms.Button()
        Me.cmbLinea = New System.Windows.Forms.ComboBox()
        Me.cmdIntentarManual = New System.Windows.Forms.Button()
        Me.cmdEvacuarPalet = New System.Windows.Forms.Button()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.RectangleShape1 = New Microsoft.VisualBasic.PowerPacks.RectangleShape()
        Me.btSalir = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(441, 70)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Hubo un error al intentar etiquetar automáticamente el pallet  actual. Seleccione" & _
    " la opción que desee"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.cmdReintentarAutomatico)
        Me.GroupBox1.Controls.Add(Me.cmbLinea)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 99)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(441, 79)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Reintentar aplicación automática"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(147, 15)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Seleccione una línea"
        '
        'cmdReintentarAutomatico
        '
        Me.cmdReintentarAutomatico.Enabled = False
        Me.cmdReintentarAutomatico.Location = New System.Drawing.Point(297, 11)
        Me.cmdReintentarAutomatico.Name = "cmdReintentarAutomatico"
        Me.cmdReintentarAutomatico.Size = New System.Drawing.Size(138, 60)
        Me.cmdReintentarAutomatico.TabIndex = 1
        Me.cmdReintentarAutomatico.Text = "Reintentar aplicación automática"
        Me.cmdReintentarAutomatico.UseVisualStyleBackColor = True
        '
        'cmbLinea
        '
        Me.cmbLinea.FormattingEnabled = True
        Me.cmbLinea.Location = New System.Drawing.Point(6, 48)
        Me.cmbLinea.Name = "cmbLinea"
        Me.cmbLinea.Size = New System.Drawing.Size(285, 23)
        Me.cmbLinea.TabIndex = 0
        '
        'cmdIntentarManual
        '
        Me.cmdIntentarManual.Location = New System.Drawing.Point(12, 184)
        Me.cmdIntentarManual.Name = "cmdIntentarManual"
        Me.cmdIntentarManual.Size = New System.Drawing.Size(125, 51)
        Me.cmdIntentarManual.TabIndex = 2
        Me.cmdIntentarManual.Text = "Aplicar manualmente"
        Me.cmdIntentarManual.UseVisualStyleBackColor = True
        '
        'cmdEvacuarPalet
        '
        Me.cmdEvacuarPalet.Location = New System.Drawing.Point(328, 184)
        Me.cmdEvacuarPalet.Name = "cmdEvacuarPalet"
        Me.cmdEvacuarPalet.Size = New System.Drawing.Size(125, 51)
        Me.cmdEvacuarPalet.TabIndex = 3
        Me.cmdEvacuarPalet.Text = "Evacuar pallet"
        Me.cmdEvacuarPalet.UseVisualStyleBackColor = True
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.RectangleShape1})
        Me.ShapeContainer1.Size = New System.Drawing.Size(465, 244)
        Me.ShapeContainer1.TabIndex = 4
        Me.ShapeContainer1.TabStop = False
        '
        'RectangleShape1
        '
        Me.RectangleShape1.BorderColor = System.Drawing.Color.Blue
        Me.RectangleShape1.BorderWidth = 5
        Me.RectangleShape1.Location = New System.Drawing.Point(2, 2)
        Me.RectangleShape1.Name = "RectangleShape1"
        Me.RectangleShape1.Size = New System.Drawing.Size(461, 240)
        '
        'btSalir
        '
        Me.btSalir.Location = New System.Drawing.Point(168, 184)
        Me.btSalir.Name = "btSalir"
        Me.btSalir.Size = New System.Drawing.Size(125, 51)
        Me.btSalir.TabIndex = 5
        Me.btSalir.Text = "Salir"
        Me.btSalir.UseVisualStyleBackColor = True
        '
        'AutomaticoOManual
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(465, 244)
        Me.Controls.Add(Me.btSalir)
        Me.Controls.Add(Me.cmdEvacuarPalet)
        Me.Controls.Add(Me.cmdIntentarManual)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ShapeContainer1)
        Me.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "AutomaticoOManual"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Error de etiquetado"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmdReintentarAutomatico As System.Windows.Forms.Button
    Friend WithEvents cmbLinea As System.Windows.Forms.ComboBox
    Friend WithEvents cmdIntentarManual As System.Windows.Forms.Button
    Friend WithEvents cmdEvacuarPalet As System.Windows.Forms.Button
    Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Friend WithEvents RectangleShape1 As Microsoft.VisualBasic.PowerPacks.RectangleShape
    Friend WithEvents btSalir As System.Windows.Forms.Button
End Class
