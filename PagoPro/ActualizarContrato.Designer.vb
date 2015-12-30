<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ActualizarContrato
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
        Me.Label12 = New System.Windows.Forms.Label()
        Me.GroupBox9 = New System.Windows.Forms.GroupBox()
        Me.GroupBox11 = New System.Windows.Forms.GroupBox()
        Me.chkRetencion = New System.Windows.Forms.CheckBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtRetencion = New System.Windows.Forms.TextBox()
        Me.DataGridView5 = New System.Windows.Forms.DataGridView()
        Me.GroupBox10 = New System.Windows.Forms.GroupBox()
        Me.RetencionPrevia = New System.Windows.Forms.DataGridView()
        Me.GroupBox8 = New System.Windows.Forms.GroupBox()
        Me.txtNroOrden = New System.Windows.Forms.TextBox()
        Me.historialOrden = New System.Windows.Forms.GroupBox()
        Me.btnHistCargar = New System.Windows.Forms.Button()
        Me.txtNroOrdenHist = New System.Windows.Forms.TextBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.SubSellos2 = New System.Windows.Forms.TextBox()
        Me.DataGridView4 = New System.Windows.Forms.DataGridView()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TotalSellos2 = New System.Windows.Forms.TextBox()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.Recalcular = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtSubTotal2 = New System.Windows.Forms.TextBox()
        Me.Total2 = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.DataGridView3 = New System.Windows.Forms.DataGridView()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtSubTotal = New System.Windows.Forms.TextBox()
        Me.txtTotalFinal = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtSubSellos = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtMontoSellos = New System.Windows.Forms.TextBox()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.chkRes = New System.Windows.Forms.CheckBox()
        Me.chkDisp = New System.Windows.Forms.CheckBox()
        Me.txtNro = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.chkNac = New System.Windows.Forms.CheckBox()
        Me.chkProv = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkEspecial = New System.Windows.Forms.CheckBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.cmbAnio = New System.Windows.Forms.ComboBox()
        Me.chkHistorial = New System.Windows.Forms.CheckBox()
        Me.ChkNuevo = New System.Windows.Forms.CheckBox()
        Me.chkAutorizado = New System.Windows.Forms.CheckBox()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnImprimir = New System.Windows.Forms.Button()
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox11.SuspendLayout()
        CType(Me.DataGridView5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox10.SuspendLayout()
        CType(Me.RetencionPrevia, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox8.SuspendLayout()
        Me.historialOrden.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        CType(Me.DataGridView4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox6.SuspendLayout()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtSaliente
        '
        Me.txtSaliente.Location = New System.Drawing.Point(88, 460)
        Me.txtSaliente.Name = "txtSaliente"
        Me.txtSaliente.Size = New System.Drawing.Size(60, 20)
        Me.txtSaliente.TabIndex = 41
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(2, 463)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(80, 13)
        Me.Label12.TabIndex = 40
        Me.Label12.Text = "Orden Saliente:"
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.GroupBox11)
        Me.GroupBox9.Controls.Add(Me.GroupBox10)
        Me.GroupBox9.Location = New System.Drawing.Point(1045, 12)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(315, 625)
        Me.GroupBox9.TabIndex = 39
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "Retenciones:"
        '
        'GroupBox11
        '
        Me.GroupBox11.Controls.Add(Me.chkRetencion)
        Me.GroupBox11.Controls.Add(Me.Label11)
        Me.GroupBox11.Controls.Add(Me.txtRetencion)
        Me.GroupBox11.Controls.Add(Me.DataGridView5)
        Me.GroupBox11.Location = New System.Drawing.Point(6, 373)
        Me.GroupBox11.Name = "GroupBox11"
        Me.GroupBox11.Size = New System.Drawing.Size(303, 240)
        Me.GroupBox11.TabIndex = 1
        Me.GroupBox11.TabStop = False
        Me.GroupBox11.Text = "Impresion:"
        '
        'chkRetencion
        '
        Me.chkRetencion.AutoSize = True
        Me.chkRetencion.Location = New System.Drawing.Point(26, 214)
        Me.chkRetencion.Name = "chkRetencion"
        Me.chkRetencion.Size = New System.Drawing.Size(76, 17)
        Me.chkRetencion.TabIndex = 3
        Me.chkRetencion.Text = "IMPRIMIR"
        Me.chkRetencion.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(113, 217)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(34, 13)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "Total:"
        '
        'txtRetencion
        '
        Me.txtRetencion.Location = New System.Drawing.Point(164, 210)
        Me.txtRetencion.Name = "txtRetencion"
        Me.txtRetencion.Size = New System.Drawing.Size(100, 20)
        Me.txtRetencion.TabIndex = 1
        Me.txtRetencion.Text = "0"
        '
        'DataGridView5
        '
        Me.DataGridView5.AllowUserToAddRows = False
        Me.DataGridView5.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView5.Location = New System.Drawing.Point(6, 19)
        Me.DataGridView5.Name = "DataGridView5"
        Me.DataGridView5.Size = New System.Drawing.Size(291, 189)
        Me.DataGridView5.TabIndex = 0
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.RetencionPrevia)
        Me.GroupBox10.Location = New System.Drawing.Point(6, 20)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(303, 347)
        Me.GroupBox10.TabIndex = 0
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "Vista Previa"
        '
        'RetencionPrevia
        '
        Me.RetencionPrevia.AllowUserToAddRows = False
        Me.RetencionPrevia.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.RetencionPrevia.Location = New System.Drawing.Point(6, 20)
        Me.RetencionPrevia.Name = "RetencionPrevia"
        Me.RetencionPrevia.Size = New System.Drawing.Size(291, 295)
        Me.RetencionPrevia.TabIndex = 0
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.txtNroOrden)
        Me.GroupBox8.Location = New System.Drawing.Point(11, 394)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(71, 49)
        Me.GroupBox8.TabIndex = 27
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "Nro Orden"
        '
        'txtNroOrden
        '
        Me.txtNroOrden.Location = New System.Drawing.Point(6, 19)
        Me.txtNroOrden.Name = "txtNroOrden"
        Me.txtNroOrden.Size = New System.Drawing.Size(61, 20)
        Me.txtNroOrden.TabIndex = 7
        '
        'historialOrden
        '
        Me.historialOrden.Controls.Add(Me.btnHistCargar)
        Me.historialOrden.Controls.Add(Me.txtNroOrdenHist)
        Me.historialOrden.Location = New System.Drawing.Point(11, 315)
        Me.historialOrden.Name = "historialOrden"
        Me.historialOrden.Size = New System.Drawing.Size(113, 73)
        Me.historialOrden.TabIndex = 28
        Me.historialOrden.TabStop = False
        Me.historialOrden.Text = "Historial Orden:"
        '
        'btnHistCargar
        '
        Me.btnHistCargar.Location = New System.Drawing.Point(9, 44)
        Me.btnHistCargar.Name = "btnHistCargar"
        Me.btnHistCargar.Size = New System.Drawing.Size(75, 23)
        Me.btnHistCargar.TabIndex = 7
        Me.btnHistCargar.Text = "Cargar"
        Me.btnHistCargar.UseVisualStyleBackColor = True
        '
        'txtNroOrdenHist
        '
        Me.txtNroOrdenHist.Location = New System.Drawing.Point(9, 18)
        Me.txtNroOrdenHist.Name = "txtNroOrdenHist"
        Me.txtNroOrdenHist.Size = New System.Drawing.Size(100, 20)
        Me.txtNroOrdenHist.TabIndex = 7
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.GroupBox7)
        Me.GroupBox5.Controls.Add(Me.GroupBox6)
        Me.GroupBox5.Location = New System.Drawing.Point(603, 12)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(436, 625)
        Me.GroupBox5.TabIndex = 38
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Para Imprimir:"
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.Label9)
        Me.GroupBox7.Controls.Add(Me.SubSellos2)
        Me.GroupBox7.Controls.Add(Me.DataGridView4)
        Me.GroupBox7.Controls.Add(Me.Label10)
        Me.GroupBox7.Controls.Add(Me.TotalSellos2)
        Me.GroupBox7.Location = New System.Drawing.Point(6, 373)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(422, 240)
        Me.GroupBox7.TabIndex = 1
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Impuestos al Sello:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(88, 217)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(49, 13)
        Me.Label9.TabIndex = 25
        Me.Label9.Text = "Subtotal:"
        '
        'SubSellos2
        '
        Me.SubSellos2.Location = New System.Drawing.Point(143, 214)
        Me.SubSellos2.Name = "SubSellos2"
        Me.SubSellos2.Size = New System.Drawing.Size(100, 20)
        Me.SubSellos2.TabIndex = 27
        '
        'DataGridView4
        '
        Me.DataGridView4.AllowUserToAddRows = False
        Me.DataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView4.Location = New System.Drawing.Point(6, 19)
        Me.DataGridView4.Name = "DataGridView4"
        Me.DataGridView4.Size = New System.Drawing.Size(406, 189)
        Me.DataGridView4.TabIndex = 0
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(239, 217)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(65, 13)
        Me.Label10.TabIndex = 26
        Me.Label10.Text = "Total Sellos:"
        '
        'TotalSellos2
        '
        Me.TotalSellos2.Location = New System.Drawing.Point(310, 214)
        Me.TotalSellos2.Name = "TotalSellos2"
        Me.TotalSellos2.Size = New System.Drawing.Size(100, 20)
        Me.TotalSellos2.TabIndex = 24
        Me.TotalSellos2.Text = "0"
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.Recalcular)
        Me.GroupBox6.Controls.Add(Me.Label7)
        Me.GroupBox6.Controls.Add(Me.txtSubTotal2)
        Me.GroupBox6.Controls.Add(Me.Total2)
        Me.GroupBox6.Controls.Add(Me.Label8)
        Me.GroupBox6.Controls.Add(Me.DataGridView3)
        Me.GroupBox6.Location = New System.Drawing.Point(6, 20)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(422, 347)
        Me.GroupBox6.TabIndex = 0
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Vista Previa:"
        '
        'Recalcular
        '
        Me.Recalcular.Location = New System.Drawing.Point(168, 318)
        Me.Recalcular.Name = "Recalcular"
        Me.Recalcular.Size = New System.Drawing.Size(75, 23)
        Me.Recalcular.TabIndex = 11
        Me.Recalcular.Text = "Recalcular"
        Me.Recalcular.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(15, 324)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(49, 13)
        Me.Label7.TabIndex = 10
        Me.Label7.Text = "Subtotal:"
        '
        'txtSubTotal2
        '
        Me.txtSubTotal2.Location = New System.Drawing.Point(60, 321)
        Me.txtSubTotal2.Name = "txtSubTotal2"
        Me.txtSubTotal2.Size = New System.Drawing.Size(100, 20)
        Me.txtSubTotal2.TabIndex = 9
        '
        'Total2
        '
        Me.Total2.Location = New System.Drawing.Point(312, 321)
        Me.Total2.Name = "Total2"
        Me.Total2.Size = New System.Drawing.Size(100, 20)
        Me.Total2.TabIndex = 8
        Me.Total2.Text = "0"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(243, 324)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(63, 13)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "Monto total:"
        '
        'DataGridView3
        '
        Me.DataGridView3.AllowUserToAddRows = False
        Me.DataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView3.Location = New System.Drawing.Point(6, 20)
        Me.DataGridView3.Name = "DataGridView3"
        Me.DataGridView3.Size = New System.Drawing.Size(408, 295)
        Me.DataGridView3.TabIndex = 0
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.GroupBox2)
        Me.GroupBox4.Controls.Add(Me.GroupBox3)
        Me.GroupBox4.Location = New System.Drawing.Point(193, 12)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(404, 625)
        Me.GroupBox4.TabIndex = 37
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Pendientes:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.txtSubTotal)
        Me.GroupBox2.Controls.Add(Me.txtTotalFinal)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.DataGridView1)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 20)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(392, 347)
        Me.GroupBox2.TabIndex = 15
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Vista Previa:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(42, 323)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(49, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Subtotal:"
        '
        'txtSubTotal
        '
        Me.txtSubTotal.Location = New System.Drawing.Point(102, 320)
        Me.txtSubTotal.Name = "txtSubTotal"
        Me.txtSubTotal.Size = New System.Drawing.Size(100, 20)
        Me.txtSubTotal.TabIndex = 5
        '
        'txtTotalFinal
        '
        Me.txtTotalFinal.Location = New System.Drawing.Point(283, 317)
        Me.txtTotalFinal.Name = "txtTotalFinal"
        Me.txtTotalFinal.Size = New System.Drawing.Size(100, 20)
        Me.txtTotalFinal.TabIndex = 4
        Me.txtTotalFinal.Text = "0"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(214, 320)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Monto total:"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowDrop = True
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(6, 19)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(377, 296)
        Me.DataGridView1.TabIndex = 2
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.txtSubSellos)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.txtMontoSellos)
        Me.GroupBox3.Controls.Add(Me.DataGridView2)
        Me.GroupBox3.Location = New System.Drawing.Point(6, 373)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(392, 240)
        Me.GroupBox3.TabIndex = 14
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Impuestos al Sello:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(14, 217)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 13)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "Subtotal:"
        '
        'txtSubSellos
        '
        Me.txtSubSellos.Location = New System.Drawing.Point(69, 214)
        Me.txtSubSellos.Name = "txtSubSellos"
        Me.txtSubSellos.Size = New System.Drawing.Size(100, 20)
        Me.txtSubSellos.TabIndex = 16
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(165, 217)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 13)
        Me.Label2.TabIndex = 16
        Me.Label2.Text = "Total Sellos:"
        '
        'txtMontoSellos
        '
        Me.txtMontoSellos.Location = New System.Drawing.Point(236, 214)
        Me.txtMontoSellos.Name = "txtMontoSellos"
        Me.txtMontoSellos.Size = New System.Drawing.Size(100, 20)
        Me.txtMontoSellos.TabIndex = 1
        Me.txtMontoSellos.Text = "0"
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Location = New System.Drawing.Point(6, 19)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.Size = New System.Drawing.Size(377, 189)
        Me.DataGridView2.TabIndex = 0
        '
        'chkRes
        '
        Me.chkRes.AutoSize = True
        Me.chkRes.Location = New System.Drawing.Point(97, 75)
        Me.chkRes.Name = "chkRes"
        Me.chkRes.Size = New System.Drawing.Size(79, 17)
        Me.chkRes.TabIndex = 36
        Me.chkRes.Text = "Resolucion"
        Me.chkRes.UseVisualStyleBackColor = True
        '
        'chkDisp
        '
        Me.chkDisp.AutoSize = True
        Me.chkDisp.Location = New System.Drawing.Point(11, 75)
        Me.chkDisp.Name = "chkDisp"
        Me.chkDisp.Size = New System.Drawing.Size(80, 17)
        Me.chkDisp.TabIndex = 35
        Me.chkDisp.Text = "Disposicion"
        Me.chkDisp.UseVisualStyleBackColor = True
        '
        'txtNro
        '
        Me.txtNro.Location = New System.Drawing.Point(51, 49)
        Me.txtNro.Name = "txtNro"
        Me.txtNro.Size = New System.Drawing.Size(97, 20)
        Me.txtNro.TabIndex = 33
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(51, 49)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(97, 20)
        Me.TextBox1.TabIndex = 34
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(11, 52)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(34, 13)
        Me.Label6.TabIndex = 32
        Me.Label6.Text = "NRO:"
        '
        'chkNac
        '
        Me.chkNac.AutoSize = True
        Me.chkNac.Location = New System.Drawing.Point(14, 32)
        Me.chkNac.Name = "chkNac"
        Me.chkNac.Size = New System.Drawing.Size(68, 17)
        Me.chkNac.TabIndex = 31
        Me.chkNac.Text = "Nacional"
        Me.chkNac.UseVisualStyleBackColor = True
        '
        'chkProv
        '
        Me.chkProv.AutoSize = True
        Me.chkProv.Location = New System.Drawing.Point(88, 32)
        Me.chkProv.Name = "chkProv"
        Me.chkProv.Size = New System.Drawing.Size(72, 17)
        Me.chkProv.TabIndex = 30
        Me.chkProv.Text = "Provincial"
        Me.chkProv.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkEspecial)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.cmbAnio)
        Me.GroupBox1.Controls.Add(Me.chkHistorial)
        Me.GroupBox1.Controls.Add(Me.ChkNuevo)
        Me.GroupBox1.Controls.Add(Me.chkAutorizado)
        Me.GroupBox1.Controls.Add(Me.btnBuscar)
        Me.GroupBox1.Controls.Add(Me.txtCodigo)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(11, 98)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(176, 211)
        Me.GroupBox1.TabIndex = 29
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Busqueda:"
        '
        'chkEspecial
        '
        Me.chkEspecial.AutoSize = True
        Me.chkEspecial.Location = New System.Drawing.Point(6, 82)
        Me.chkEspecial.Name = "chkEspecial"
        Me.chkEspecial.Size = New System.Drawing.Size(166, 17)
        Me.chkEspecial.TabIndex = 19
        Me.chkEspecial.Text = "año pasado/nro Orden actual"
        Me.chkEspecial.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(9, 61)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(26, 13)
        Me.Label13.TabIndex = 18
        Me.Label13.Text = "Año"
        '
        'cmbAnio
        '
        Me.cmbAnio.FormattingEnabled = True
        Me.cmbAnio.Location = New System.Drawing.Point(57, 53)
        Me.cmbAnio.Name = "cmbAnio"
        Me.cmbAnio.Size = New System.Drawing.Size(108, 21)
        Me.cmbAnio.TabIndex = 17
        '
        'chkHistorial
        '
        Me.chkHistorial.AutoSize = True
        Me.chkHistorial.Location = New System.Drawing.Point(91, 128)
        Me.chkHistorial.Name = "chkHistorial"
        Me.chkHistorial.Size = New System.Drawing.Size(63, 17)
        Me.chkHistorial.TabIndex = 16
        Me.chkHistorial.Text = "Historial"
        Me.chkHistorial.UseVisualStyleBackColor = True
        '
        'ChkNuevo
        '
        Me.ChkNuevo.AutoSize = True
        Me.ChkNuevo.Location = New System.Drawing.Point(9, 105)
        Me.ChkNuevo.Name = "ChkNuevo"
        Me.ChkNuevo.Size = New System.Drawing.Size(58, 17)
        Me.ChkNuevo.TabIndex = 7
        Me.ChkNuevo.Text = "Nuevo"
        Me.ChkNuevo.UseVisualStyleBackColor = True
        '
        'chkAutorizado
        '
        Me.chkAutorizado.AutoSize = True
        Me.chkAutorizado.Location = New System.Drawing.Point(9, 128)
        Me.chkAutorizado.Name = "chkAutorizado"
        Me.chkAutorizado.Size = New System.Drawing.Size(76, 17)
        Me.chkAutorizado.TabIndex = 6
        Me.chkAutorizado.Text = "Autorizado"
        Me.chkAutorizado.UseVisualStyleBackColor = True
        '
        'btnBuscar
        '
        Me.btnBuscar.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btnBuscar.Location = New System.Drawing.Point(3, 151)
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
        Me.txtCodigo.Size = New System.Drawing.Size(57, 20)
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
        Me.btnImprimir.Location = New System.Drawing.Point(1128, 654)
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.Size = New System.Drawing.Size(107, 54)
        Me.btnImprimir.TabIndex = 43
        Me.btnImprimir.Text = "Imprimir"
        Me.btnImprimir.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.btnImprimir.UseVisualStyleBackColor = True
        '
        'btnSalir
        '
        Me.btnSalir.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btnSalir.Location = New System.Drawing.Point(1241, 654)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(107, 54)
        Me.btnSalir.TabIndex = 42
        Me.btnSalir.Text = "Salir"
        Me.btnSalir.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.btnSalir.UseVisualStyleBackColor = True
        '
        'ActualizarContrato
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1370, 749)
        Me.Controls.Add(Me.btnImprimir)
        Me.Controls.Add(Me.btnSalir)
        Me.Controls.Add(Me.txtSaliente)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.GroupBox9)
        Me.Controls.Add(Me.GroupBox8)
        Me.Controls.Add(Me.historialOrden)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.chkRes)
        Me.Controls.Add(Me.chkDisp)
        Me.Controls.Add(Me.txtNro)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.chkNac)
        Me.Controls.Add(Me.chkProv)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "ActualizarContrato"
        Me.Text = "ActualizarContrato"
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox11.ResumeLayout(False)
        Me.GroupBox11.PerformLayout()
        CType(Me.DataGridView5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox10.ResumeLayout(False)
        CType(Me.RetencionPrevia, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox8.PerformLayout()
        Me.historialOrden.ResumeLayout(False)
        Me.historialOrden.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        CType(Me.DataGridView4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtSaliente As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox11 As System.Windows.Forms.GroupBox
    Friend WithEvents chkRetencion As System.Windows.Forms.CheckBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtRetencion As System.Windows.Forms.TextBox
    Friend WithEvents DataGridView5 As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Friend WithEvents RetencionPrevia As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents txtNroOrden As System.Windows.Forms.TextBox
    Friend WithEvents historialOrden As System.Windows.Forms.GroupBox
    Friend WithEvents btnHistCargar As System.Windows.Forms.Button
    Friend WithEvents txtNroOrdenHist As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents SubSellos2 As System.Windows.Forms.TextBox
    Friend WithEvents DataGridView4 As System.Windows.Forms.DataGridView
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TotalSellos2 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents Recalcular As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtSubTotal2 As System.Windows.Forms.TextBox
    Friend WithEvents Total2 As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents DataGridView3 As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtSubTotal As System.Windows.Forms.TextBox
    Friend WithEvents txtTotalFinal As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtSubSellos As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtMontoSellos As System.Windows.Forms.TextBox
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents chkRes As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisp As System.Windows.Forms.CheckBox
    Friend WithEvents txtNro As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chkNac As System.Windows.Forms.CheckBox
    Friend WithEvents chkProv As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkEspecial As System.Windows.Forms.CheckBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents cmbAnio As System.Windows.Forms.ComboBox
    Friend WithEvents chkHistorial As System.Windows.Forms.CheckBox
    Friend WithEvents ChkNuevo As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutorizado As System.Windows.Forms.CheckBox
    Friend WithEvents btnBuscar As System.Windows.Forms.Button
    Friend WithEvents txtCodigo As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnImprimir As System.Windows.Forms.Button
    Friend WithEvents btnSalir As System.Windows.Forms.Button
End Class
