<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Beneficiario
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
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.fecha = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.tipoDoc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.nroDoc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.codIBruto = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.condicionGanancia = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.condicionIVA = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.razonSocial = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.calle = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.puerta = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.unidadFuncional = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cp = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.nIBruto = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.id = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cbu = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.DateHasta = New System.Windows.Forms.DateTimePicker()
        Me.dateDesde = New System.Windows.Forms.DateTimePicker()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.txtBuscarDni = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.btnModificar = New System.Windows.Forms.Button()
        Me.Cargar = New System.Windows.Forms.Button()
        Me.txtCbu = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtDni = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtNombreProveedor = New System.Windows.Forms.TextBox()
        Me.laberTIpo = New System.Windows.Forms.Label()
        Me.labelUsuario = New System.Windows.Forms.Label()
        Me.GroupBox2.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.DataGridView1)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 93)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1005, 263)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Beneficiarios"
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.fecha, Me.tipoDoc, Me.nroDoc, Me.codIBruto, Me.condicionGanancia, Me.condicionIVA, Me.razonSocial, Me.calle, Me.puerta, Me.unidadFuncional, Me.cp, Me.nIBruto, Me.id, Me.cbu})
        Me.DataGridView1.Location = New System.Drawing.Point(9, 19)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(990, 235)
        Me.DataGridView1.TabIndex = 0
        '
        'fecha
        '
        Me.fecha.HeaderText = "FECHA"
        Me.fecha.Name = "fecha"
        '
        'tipoDoc
        '
        Me.tipoDoc.HeaderText = "TIPO DOCUMENTO"
        Me.tipoDoc.Name = "tipoDoc"
        '
        'nroDoc
        '
        Me.nroDoc.HeaderText = "NRO DOCUMENTO"
        Me.nroDoc.Name = "nroDoc"
        '
        'codIBruto
        '
        Me.codIBruto.HeaderText = "condIBruto"
        Me.codIBruto.Name = "codIBruto"
        '
        'condicionGanancia
        '
        Me.condicionGanancia.HeaderText = "CONDICION GANANCIA"
        Me.condicionGanancia.Name = "condicionGanancia"
        '
        'condicionIVA
        '
        Me.condicionIVA.HeaderText = "CONDICION IVA"
        Me.condicionIVA.Name = "condicionIVA"
        '
        'razonSocial
        '
        Me.razonSocial.HeaderText = "RAZON SOCIAL"
        Me.razonSocial.Name = "razonSocial"
        '
        'calle
        '
        Me.calle.HeaderText = "CALLE"
        Me.calle.Name = "calle"
        '
        'puerta
        '
        Me.puerta.HeaderText = "PUERTA"
        Me.puerta.Name = "puerta"
        '
        'unidadFuncional
        '
        Me.unidadFuncional.HeaderText = "UNIDAD FUNCIONAL"
        Me.unidadFuncional.Name = "unidadFuncional"
        '
        'cp
        '
        Me.cp.HeaderText = "CP"
        Me.cp.Name = "cp"
        '
        'nIBruto
        '
        Me.nIBruto.HeaderText = "Nº I.Bruto"
        Me.nIBruto.Name = "nIBruto"
        '
        'id
        '
        Me.id.HeaderText = "CODIGO"
        Me.id.Name = "id"
        '
        'cbu
        '
        Me.cbu.HeaderText = "CBU"
        Me.cbu.Name = "cbu"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(180, 30)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(35, 13)
        Me.Label13.TabIndex = 28
        Me.Label13.Text = "Hasta"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(26, 30)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(38, 13)
        Me.Label12.TabIndex = 27
        Me.Label12.Text = "Desde"
        '
        'DateHasta
        '
        Me.DateHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateHasta.Location = New System.Drawing.Point(221, 23)
        Me.DateHasta.Name = "DateHasta"
        Me.DateHasta.Size = New System.Drawing.Size(98, 20)
        Me.DateHasta.TabIndex = 26
        '
        'dateDesde
        '
        Me.dateDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dateDesde.Location = New System.Drawing.Point(79, 23)
        Me.dateDesde.Name = "dateDesde"
        Me.dateDesde.Size = New System.Drawing.Size(98, 20)
        Me.dateDesde.TabIndex = 25
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(335, 19)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 24
        Me.Button3.Text = "Exportar "
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(416, 19)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 29
        Me.Button2.Text = "Salir"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'txtBuscarDni
        '
        Me.txtBuscarDni.Location = New System.Drawing.Point(369, 12)
        Me.txtBuscarDni.Name = "txtBuscarDni"
        Me.txtBuscarDni.Size = New System.Drawing.Size(253, 20)
        Me.txtBuscarDni.TabIndex = 33
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(337, 19)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(26, 13)
        Me.Label18.TabIndex = 34
        Me.Label18.Text = "DNI"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button2)
        Me.GroupBox1.Controls.Add(Me.Button3)
        Me.GroupBox1.Controls.Add(Me.dateDesde)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.DateHasta)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Location = New System.Drawing.Point(21, 505)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(497, 72)
        Me.GroupBox1.TabIndex = 35
        Me.GroupBox1.TabStop = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.btnModificar)
        Me.GroupBox3.Controls.Add(Me.Cargar)
        Me.GroupBox3.Controls.Add(Me.txtCbu)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.txtNombre)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.txtDni)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Location = New System.Drawing.Point(100, 391)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(390, 108)
        Me.GroupBox3.TabIndex = 36
        Me.GroupBox3.TabStop = False
        '
        'btnModificar
        '
        Me.btnModificar.Location = New System.Drawing.Point(285, 52)
        Me.btnModificar.Name = "btnModificar"
        Me.btnModificar.Size = New System.Drawing.Size(75, 23)
        Me.btnModificar.TabIndex = 8
        Me.btnModificar.Text = "Modificar"
        Me.btnModificar.UseVisualStyleBackColor = True
        '
        'Cargar
        '
        Me.Cargar.Location = New System.Drawing.Point(285, 16)
        Me.Cargar.Name = "Cargar"
        Me.Cargar.Size = New System.Drawing.Size(75, 23)
        Me.Cargar.TabIndex = 7
        Me.Cargar.Text = "Cargar CBU"
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
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(12, 375)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(82, 17)
        Me.CheckBox1.TabIndex = 6
        Me.CheckBox1.Text = "Cargar CBU"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(324, 57)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(44, 13)
        Me.Label4.TabIndex = 37
        Me.Label4.Text = "Nombre"
        '
        'txtNombreProveedor
        '
        Me.txtNombreProveedor.Location = New System.Drawing.Point(369, 50)
        Me.txtNombreProveedor.Name = "txtNombreProveedor"
        Me.txtNombreProveedor.Size = New System.Drawing.Size(253, 20)
        Me.txtNombreProveedor.TabIndex = 38
        '
        'laberTIpo
        '
        Me.laberTIpo.AutoSize = True
        Me.laberTIpo.Location = New System.Drawing.Point(18, 50)
        Me.laberTIpo.Name = "laberTIpo"
        Me.laberTIpo.Size = New System.Drawing.Size(32, 13)
        Me.laberTIpo.TabIndex = 52
        Me.laberTIpo.Text = "TIPO"
        '
        'labelUsuario
        '
        Me.labelUsuario.AutoSize = True
        Me.labelUsuario.Location = New System.Drawing.Point(18, 19)
        Me.labelUsuario.Name = "labelUsuario"
        Me.labelUsuario.Size = New System.Drawing.Size(56, 13)
        Me.labelUsuario.TabIndex = 51
        Me.labelUsuario.Text = "USUARIO"
        '
        'Beneficiario
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1029, 589)
        Me.Controls.Add(Me.laberTIpo)
        Me.Controls.Add(Me.labelUsuario)
        Me.Controls.Add(Me.txtNombreProveedor)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.txtBuscarDni)
        Me.Controls.Add(Me.GroupBox2)
        Me.Name = "Beneficiario"
        Me.Text = "Beneficiario"
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents DateHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents dateDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents txtBuscarDni As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents txtCbu As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtNombre As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtDni As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Cargar As System.Windows.Forms.Button
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtNombreProveedor As System.Windows.Forms.TextBox
    Friend WithEvents btnModificar As System.Windows.Forms.Button
    Friend WithEvents laberTIpo As System.Windows.Forms.Label
    Friend WithEvents labelUsuario As System.Windows.Forms.Label
    Friend WithEvents fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents tipoDoc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents nroDoc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents codIBruto As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents condicionGanancia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents condicionIVA As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents razonSocial As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents calle As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents puerta As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents unidadFuncional As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cp As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents nIBruto As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents id As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cbu As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
