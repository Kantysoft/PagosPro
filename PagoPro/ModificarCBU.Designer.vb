<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ModificarCBU
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
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Cargar = New System.Windows.Forms.Button()
        Me.txtCbu = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtDni = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.txtBuscarCbu = New System.Windows.Forms.TextBox()
        Me.dni = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.razonSocial = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CBU = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.laberTIpo = New System.Windows.Forms.Label()
        Me.labelUsuario = New System.Windows.Forms.Label()
        Me.btnModificar = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.Dni4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.nombre = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txtBuscarProveedor = New System.Windows.Forms.TextBox()
        Me.GroupBox3.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Button1)
        Me.GroupBox3.Controls.Add(Me.btnModificar)
        Me.GroupBox3.Controls.Add(Me.Cargar)
        Me.GroupBox3.Controls.Add(Me.txtCbu)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.txtNombre)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.txtDni)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 272)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(290, 171)
        Me.GroupBox3.TabIndex = 37
        Me.GroupBox3.TabStop = False
        '
        'Cargar
        '
        Me.Cargar.Location = New System.Drawing.Point(6, 120)
        Me.Cargar.Name = "Cargar"
        Me.Cargar.Size = New System.Drawing.Size(75, 23)
        Me.Cargar.TabIndex = 7
        Me.Cargar.Text = "Cargar"
        Me.Cargar.UseVisualStyleBackColor = True
        '
        'txtCbu
        '
        Me.txtCbu.Location = New System.Drawing.Point(95, 74)
        Me.txtCbu.Name = "txtCbu"
        Me.txtCbu.Size = New System.Drawing.Size(169, 20)
        Me.txtCbu.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 81)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "CBU"
        '
        'txtNombre
        '
        Me.txtNombre.Location = New System.Drawing.Point(95, 45)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(169, 20)
        Me.txtNombre.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Razon Social"
        '
        'txtDni
        '
        Me.txtDni.Location = New System.Drawing.Point(95, 16)
        Me.txtDni.Name = "txtDni"
        Me.txtDni.Size = New System.Drawing.Size(169, 20)
        Me.txtDni.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Nro Documento"
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dni, Me.razonSocial, Me.CBU})
        Me.DataGridView1.Location = New System.Drawing.Point(339, 40)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(486, 393)
        Me.DataGridView1.TabIndex = 38
        '
        'txtBuscarCbu
        '
        Me.txtBuscarCbu.Location = New System.Drawing.Point(339, 12)
        Me.txtBuscarCbu.Name = "txtBuscarCbu"
        Me.txtBuscarCbu.Size = New System.Drawing.Size(486, 20)
        Me.txtBuscarCbu.TabIndex = 39
        '
        'dni
        '
        Me.dni.HeaderText = "DNI"
        Me.dni.Name = "dni"
        '
        'razonSocial
        '
        Me.razonSocial.HeaderText = "RAZON SOCIAL"
        Me.razonSocial.Name = "razonSocial"
        Me.razonSocial.Width = 140
        '
        'CBU
        '
        Me.CBU.HeaderText = "CBU"
        Me.CBU.Name = "CBU"
        Me.CBU.Width = 165
        '
        'laberTIpo
        '
        Me.laberTIpo.AutoSize = True
        Me.laberTIpo.Location = New System.Drawing.Point(12, 40)
        Me.laberTIpo.Name = "laberTIpo"
        Me.laberTIpo.Size = New System.Drawing.Size(32, 13)
        Me.laberTIpo.TabIndex = 52
        Me.laberTIpo.Text = "TIPO"
        '
        'labelUsuario
        '
        Me.labelUsuario.AutoSize = True
        Me.labelUsuario.Location = New System.Drawing.Point(12, 9)
        Me.labelUsuario.Name = "labelUsuario"
        Me.labelUsuario.Size = New System.Drawing.Size(56, 13)
        Me.labelUsuario.TabIndex = 51
        Me.labelUsuario.Text = "USUARIO"
        '
        'btnModificar
        '
        Me.btnModificar.Location = New System.Drawing.Point(87, 120)
        Me.btnModificar.Name = "btnModificar"
        Me.btnModificar.Size = New System.Drawing.Size(75, 23)
        Me.btnModificar.TabIndex = 12
        Me.btnModificar.Text = "Modificar"
        Me.btnModificar.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(168, 120)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 13
        Me.Button1.Text = "Salir"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'DataGridView2
        '
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Dni4, Me.nombre})
        Me.DataGridView2.Location = New System.Drawing.Point(15, 102)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.Size = New System.Drawing.Size(287, 164)
        Me.DataGridView2.TabIndex = 53
        '
        'Dni4
        '
        Me.Dni4.HeaderText = "DNI"
        Me.Dni4.Name = "Dni4"
        '
        'nombre
        '
        Me.nombre.HeaderText = "NOMBRE"
        Me.nombre.Name = "nombre"
        '
        'txtBuscarProveedor
        '
        Me.txtBuscarProveedor.Location = New System.Drawing.Point(12, 76)
        Me.txtBuscarProveedor.Name = "txtBuscarProveedor"
        Me.txtBuscarProveedor.Size = New System.Drawing.Size(290, 20)
        Me.txtBuscarProveedor.TabIndex = 54
        '
        'ModificarCBU
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(848, 455)
        Me.Controls.Add(Me.txtBuscarProveedor)
        Me.Controls.Add(Me.DataGridView2)
        Me.Controls.Add(Me.laberTIpo)
        Me.Controls.Add(Me.labelUsuario)
        Me.Controls.Add(Me.txtBuscarCbu)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.GroupBox3)
        Me.Name = "ModificarCBU"
        Me.Text = "ModificarCBU"
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Cargar As System.Windows.Forms.Button
    Friend WithEvents txtCbu As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtNombre As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtDni As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents txtBuscarCbu As System.Windows.Forms.TextBox
    Friend WithEvents dni As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents razonSocial As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CBU As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents laberTIpo As System.Windows.Forms.Label
    Friend WithEvents labelUsuario As System.Windows.Forms.Label
    Friend WithEvents btnModificar As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents Dni4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents nombre As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txtBuscarProveedor As System.Windows.Forms.TextBox
End Class
