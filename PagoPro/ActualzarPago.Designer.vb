<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ActualzarPago
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
        Me.txtSaliente = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkRes = New System.Windows.Forms.CheckBox()
        Me.chkDisp = New System.Windows.Forms.CheckBox()
        Me.txtNro = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.GroupBox9 = New System.Windows.Forms.GroupBox()
        Me.dtFechaIngreso = New System.Windows.Forms.DateTimePicker()
        Me.chkProv = New System.Windows.Forms.CheckBox()
        Me.chkNac = New System.Windows.Forms.CheckBox()
        Me.GroupBox8 = New System.Windows.Forms.GroupBox()
        Me.txtFact = New System.Windows.Forms.TextBox()
        Me.chkActa = New System.Windows.Forms.CheckBox()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.DataGridView3 = New System.Windows.Forms.DataGridView()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.txtTotalFinal = New System.Windows.Forms.TextBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.txtTotalGastos = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbA = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbTipoOrden = New System.Windows.Forms.ComboBox()
        Me.chkAutorizado = New System.Windows.Forms.CheckBox()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnImprimir = New System.Windows.Forms.Button()
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtSaliente
        '
        Me.txtSaliente.Location = New System.Drawing.Point(106, 508)
        Me.txtSaliente.Name = "txtSaliente"
        Me.txtSaliente.Size = New System.Drawing.Size(51, 20)
        Me.txtSaliente.TabIndex = 69
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(20, 511)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 13)
        Me.Label3.TabIndex = 68
        Me.Label3.Text = "Orden Saliente:"
        '
        'chkRes
        '
        Me.chkRes.AutoSize = True
        Me.chkRes.Location = New System.Drawing.Point(106, 9)
        Me.chkRes.Name = "chkRes"
        Me.chkRes.Size = New System.Drawing.Size(79, 17)
        Me.chkRes.TabIndex = 67
        Me.chkRes.Text = "Resolucion"
        Me.chkRes.UseVisualStyleBackColor = True
        '
        'chkDisp
        '
        Me.chkDisp.AutoSize = True
        Me.chkDisp.Location = New System.Drawing.Point(20, 9)
        Me.chkDisp.Name = "chkDisp"
        Me.chkDisp.Size = New System.Drawing.Size(80, 17)
        Me.chkDisp.TabIndex = 66
        Me.chkDisp.Text = "Disposicion"
        Me.chkDisp.UseVisualStyleBackColor = True
        '
        'txtNro
        '
        Me.txtNro.Location = New System.Drawing.Point(234, 7)
        Me.txtNro.Name = "txtNro"
        Me.txtNro.Size = New System.Drawing.Size(97, 20)
        Me.txtNro.TabIndex = 65
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(194, 10)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(34, 13)
        Me.Label6.TabIndex = 64
        Me.Label6.Text = "NRO:"
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.dtFechaIngreso)
        Me.GroupBox9.Location = New System.Drawing.Point(259, 231)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(102, 59)
        Me.GroupBox9.TabIndex = 63
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "Fecha Ingreso"
        '
        'dtFechaIngreso
        '
        Me.dtFechaIngreso.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtFechaIngreso.Location = New System.Drawing.Point(6, 19)
        Me.dtFechaIngreso.Name = "dtFechaIngreso"
        Me.dtFechaIngreso.Size = New System.Drawing.Size(84, 20)
        Me.dtFechaIngreso.TabIndex = 0
        '
        'chkProv
        '
        Me.chkProv.AutoSize = True
        Me.chkProv.Location = New System.Drawing.Point(106, 32)
        Me.chkProv.Name = "chkProv"
        Me.chkProv.Size = New System.Drawing.Size(72, 17)
        Me.chkProv.TabIndex = 62
        Me.chkProv.Text = "Provincial"
        Me.chkProv.UseVisualStyleBackColor = True
        '
        'chkNac
        '
        Me.chkNac.AutoSize = True
        Me.chkNac.Location = New System.Drawing.Point(32, 32)
        Me.chkNac.Name = "chkNac"
        Me.chkNac.Size = New System.Drawing.Size(68, 17)
        Me.chkNac.TabIndex = 61
        Me.chkNac.Text = "Nacional"
        Me.chkNac.UseVisualStyleBackColor = True
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.txtFact)
        Me.GroupBox8.Location = New System.Drawing.Point(23, 231)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(230, 59)
        Me.GroupBox8.TabIndex = 60
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "Nro Factura"
        '
        'txtFact
        '
        Me.txtFact.Location = New System.Drawing.Point(9, 20)
        Me.txtFact.Name = "txtFact"
        Me.txtFact.Size = New System.Drawing.Size(215, 20)
        Me.txtFact.TabIndex = 42
        '
        'chkActa
        '
        Me.chkActa.AutoSize = True
        Me.chkActa.Location = New System.Drawing.Point(531, 487)
        Me.chkActa.Name = "chkActa"
        Me.chkActa.Size = New System.Drawing.Size(61, 30)
        Me.chkActa.TabIndex = 59
        Me.chkActa.Text = "Imprimir" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Acta"
        Me.chkActa.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.chkActa.UseVisualStyleBackColor = True
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.DataGridView3)
        Me.GroupBox7.Location = New System.Drawing.Point(389, 282)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(440, 178)
        Me.GroupBox7.TabIndex = 58
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Vista Retenciones"
        '
        'DataGridView3
        '
        Me.DataGridView3.AllowUserToAddRows = False
        Me.DataGridView3.AllowUserToDeleteRows = False
        Me.DataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView3.Location = New System.Drawing.Point(6, 19)
        Me.DataGridView3.Name = "DataGridView3"
        Me.DataGridView3.Size = New System.Drawing.Size(428, 150)
        Me.DataGridView3.TabIndex = 0
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.txtTotalFinal)
        Me.GroupBox4.Location = New System.Drawing.Point(643, 228)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(145, 51)
        Me.GroupBox4.TabIndex = 57
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Importe"
        '
        'txtTotalFinal
        '
        Me.txtTotalFinal.Location = New System.Drawing.Point(41, 19)
        Me.txtTotalFinal.Name = "txtTotalFinal"
        Me.txtTotalFinal.Size = New System.Drawing.Size(98, 20)
        Me.txtTotalFinal.TabIndex = 25
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txtTotalGastos)
        Me.GroupBox3.Location = New System.Drawing.Point(245, 485)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(132, 52)
        Me.GroupBox3.TabIndex = 56
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Total"
        '
        'txtTotalGastos
        '
        Me.txtTotalGastos.Location = New System.Drawing.Point(24, 19)
        Me.txtTotalGastos.Name = "txtTotalGastos"
        Me.txtTotalGastos.Size = New System.Drawing.Size(100, 20)
        Me.txtTotalGastos.TabIndex = 0
        Me.txtTotalGastos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.DataGridView1)
        Me.GroupBox2.Location = New System.Drawing.Point(287, 32)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(501, 188)
        Me.GroupBox2.TabIndex = 55
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Vista Previa"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(6, 19)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(489, 151)
        Me.DataGridView1.TabIndex = 2
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.cmbA)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.cmbTipoOrden)
        Me.GroupBox1.Controls.Add(Me.chkAutorizado)
        Me.GroupBox1.Controls.Add(Me.btnBuscar)
        Me.GroupBox1.Controls.Add(Me.txtCodigo)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(23, 55)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(236, 170)
        Me.GroupBox1.TabIndex = 54
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Busqueda"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(7, 61)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(26, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Año"
        '
        'cmbA
        '
        Me.cmbA.FormattingEnabled = True
        Me.cmbA.Location = New System.Drawing.Point(109, 53)
        Me.cmbA.Name = "cmbA"
        Me.cmbA.Size = New System.Drawing.Size(121, 21)
        Me.cmbA.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(7, 83)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(78, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Tipo de Orden:"
        '
        'cmbTipoOrden
        '
        Me.cmbTipoOrden.FormattingEnabled = True
        Me.cmbTipoOrden.Location = New System.Drawing.Point(109, 80)
        Me.cmbTipoOrden.Name = "cmbTipoOrden"
        Me.cmbTipoOrden.Size = New System.Drawing.Size(122, 21)
        Me.cmbTipoOrden.TabIndex = 7
        '
        'chkAutorizado
        '
        Me.chkAutorizado.AutoSize = True
        Me.chkAutorizado.Location = New System.Drawing.Point(9, 116)
        Me.chkAutorizado.Name = "chkAutorizado"
        Me.chkAutorizado.Size = New System.Drawing.Size(76, 17)
        Me.chkAutorizado.TabIndex = 6
        Me.chkAutorizado.Text = "Autorizado"
        Me.chkAutorizado.UseVisualStyleBackColor = True
        '
        'btnBuscar
        '
        Me.btnBuscar.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btnBuscar.Location = New System.Drawing.Point(123, 107)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(107, 54)
        Me.btnBuscar.TabIndex = 2
        Me.btnBuscar.Text = "Generar"
        Me.btnBuscar.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'txtCodigo
        '
        Me.txtCodigo.Location = New System.Drawing.Point(108, 27)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(122, 20)
        Me.txtCodigo.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Nro Mesa Entrada:"
        '
        'btnImprimir
        '
        Me.btnImprimir.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btnImprimir.Location = New System.Drawing.Point(627, 483)
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.Size = New System.Drawing.Size(107, 54)
        Me.btnImprimir.TabIndex = 53
        Me.btnImprimir.Text = "Imprimir"
        Me.btnImprimir.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.btnImprimir.UseVisualStyleBackColor = True
        '
        'btnSalir
        '
        Me.btnSalir.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btnSalir.Location = New System.Drawing.Point(740, 483)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(107, 54)
        Me.btnSalir.TabIndex = 52
        Me.btnSalir.Text = "Salir"
        Me.btnSalir.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.btnSalir.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.GroupBox6)
        Me.GroupBox5.Controls.Add(Me.DataGridView2)
        Me.GroupBox5.Location = New System.Drawing.Point(23, 301)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(354, 178)
        Me.GroupBox5.TabIndex = 51
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Planilla de Gastos"
        '
        'GroupBox6
        '
        Me.GroupBox6.Location = New System.Drawing.Point(360, 0)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(200, 100)
        Me.GroupBox6.TabIndex = 38
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "GroupBox6"
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Location = New System.Drawing.Point(9, 22)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.Size = New System.Drawing.Size(328, 150)
        Me.DataGridView2.TabIndex = 0
        '
        'ActualzarPago
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(978, 551)
        Me.Controls.Add(Me.txtSaliente)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.chkRes)
        Me.Controls.Add(Me.chkDisp)
        Me.Controls.Add(Me.txtNro)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.GroupBox9)
        Me.Controls.Add(Me.chkProv)
        Me.Controls.Add(Me.chkNac)
        Me.Controls.Add(Me.GroupBox8)
        Me.Controls.Add(Me.chkActa)
        Me.Controls.Add(Me.GroupBox7)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnImprimir)
        Me.Controls.Add(Me.btnSalir)
        Me.Controls.Add(Me.GroupBox5)
        Me.Name = "ActualzarPago"
        Me.Text = "ActualzarPago"
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox8.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtSaliente As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkRes As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisp As System.Windows.Forms.CheckBox
    Friend WithEvents txtNro As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents dtFechaIngreso As System.Windows.Forms.DateTimePicker
    Friend WithEvents chkProv As System.Windows.Forms.CheckBox
    Friend WithEvents chkNac As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents txtFact As System.Windows.Forms.TextBox
    Friend WithEvents chkActa As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents DataGridView3 As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents txtTotalFinal As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents txtTotalGastos As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbA As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbTipoOrden As System.Windows.Forms.ComboBox
    Friend WithEvents chkAutorizado As System.Windows.Forms.CheckBox
    Friend WithEvents btnBuscar As System.Windows.Forms.Button
    Friend WithEvents txtCodigo As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnImprimir As System.Windows.Forms.Button
    Friend WithEvents btnSalir As System.Windows.Forms.Button
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
End Class
