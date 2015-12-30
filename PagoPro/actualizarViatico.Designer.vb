<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class actualizarViatico
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
        Me.chkRes = New System.Windows.Forms.CheckBox()
        Me.chkDisp = New System.Windows.Forms.CheckBox()
        Me.txtNro = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.GroupBox14 = New System.Windows.Forms.GroupBox()
        Me.txtOtrosGastos = New System.Windows.Forms.TextBox()
        Me.chkProv = New System.Windows.Forms.CheckBox()
        Me.chkNac = New System.Windows.Forms.CheckBox()
        Me.GroupBox11 = New System.Windows.Forms.GroupBox()
        Me.GroupBox13 = New System.Windows.Forms.GroupBox()
        Me.dataBancosPago = New System.Windows.Forms.DataGridView()
        Me.GroupBox12 = New System.Windows.Forms.GroupBox()
        Me.dataConceptosPago = New System.Windows.Forms.DataGridView()
        Me.chkPago = New System.Windows.Forms.CheckBox()
        Me.GroupBox10 = New System.Windows.Forms.GroupBox()
        Me.chkReintegro = New System.Windows.Forms.CheckBox()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.dataConceptos = New System.Windows.Forms.DataGridView()
        Me.GroupBox8 = New System.Windows.Forms.GroupBox()
        Me.dataBancos = New System.Windows.Forms.DataGridView()
        Me.btnCalcular = New System.Windows.Forms.Button()
        Me.GroupBox9 = New System.Windows.Forms.GroupBox()
        Me.txtMontoReintegro = New System.Windows.Forms.TextBox()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.txtDestino = New System.Windows.Forms.TextBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.fecha = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.descripcion = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.monto = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NOMBREGRID = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.dtLlegada = New System.Windows.Forms.DateTimePicker()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.dtSalida = New System.Windows.Forms.DateTimePicker()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.txtViaticos = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtPeaje = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtCombustible = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtTotalFinal = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtDia = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnImprimir = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmbA = New System.Windows.Forms.Label()
        Me.cmbAnio = New System.Windows.Forms.ComboBox()
        Me.txtSaliente = New System.Windows.Forms.TextBox()
        Me.chkAutorizado = New System.Windows.Forms.CheckBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbTipoOrden = New System.Windows.Forms.ComboBox()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox14.SuspendLayout()
        Me.GroupBox11.SuspendLayout()
        Me.GroupBox13.SuspendLayout()
        CType(Me.dataBancosPago, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox12.SuspendLayout()
        CType(Me.dataConceptosPago, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox10.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        CType(Me.dataConceptos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox8.SuspendLayout()
        CType(Me.dataBancos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkRes
        '
        Me.chkRes.AutoSize = True
        Me.chkRes.Location = New System.Drawing.Point(95, 58)
        Me.chkRes.Name = "chkRes"
        Me.chkRes.Size = New System.Drawing.Size(79, 17)
        Me.chkRes.TabIndex = 44
        Me.chkRes.Text = "Resolucion"
        Me.chkRes.UseVisualStyleBackColor = True
        '
        'chkDisp
        '
        Me.chkDisp.AutoSize = True
        Me.chkDisp.Location = New System.Drawing.Point(9, 58)
        Me.chkDisp.Name = "chkDisp"
        Me.chkDisp.Size = New System.Drawing.Size(80, 17)
        Me.chkDisp.TabIndex = 43
        Me.chkDisp.Text = "Disposicion"
        Me.chkDisp.UseVisualStyleBackColor = True
        '
        'txtNro
        '
        Me.txtNro.Location = New System.Drawing.Point(49, 32)
        Me.txtNro.Name = "txtNro"
        Me.txtNro.Size = New System.Drawing.Size(97, 20)
        Me.txtNro.TabIndex = 42
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(9, 35)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(34, 13)
        Me.Label11.TabIndex = 41
        Me.Label11.Text = "NRO:"
        '
        'GroupBox14
        '
        Me.GroupBox14.Controls.Add(Me.txtOtrosGastos)
        Me.GroupBox14.Location = New System.Drawing.Point(284, 296)
        Me.GroupBox14.Name = "GroupBox14"
        Me.GroupBox14.Size = New System.Drawing.Size(224, 45)
        Me.GroupBox14.TabIndex = 40
        Me.GroupBox14.TabStop = False
        Me.GroupBox14.Text = "Otros Gastos:"
        '
        'txtOtrosGastos
        '
        Me.txtOtrosGastos.Location = New System.Drawing.Point(51, 19)
        Me.txtOtrosGastos.Name = "txtOtrosGastos"
        Me.txtOtrosGastos.Size = New System.Drawing.Size(89, 20)
        Me.txtOtrosGastos.TabIndex = 0
        '
        'chkProv
        '
        Me.chkProv.AutoSize = True
        Me.chkProv.Location = New System.Drawing.Point(86, 12)
        Me.chkProv.Name = "chkProv"
        Me.chkProv.Size = New System.Drawing.Size(72, 17)
        Me.chkProv.TabIndex = 39
        Me.chkProv.Text = "Provincial"
        Me.chkProv.UseVisualStyleBackColor = True
        '
        'chkNac
        '
        Me.chkNac.AutoSize = True
        Me.chkNac.Location = New System.Drawing.Point(12, 12)
        Me.chkNac.Name = "chkNac"
        Me.chkNac.Size = New System.Drawing.Size(68, 17)
        Me.chkNac.TabIndex = 38
        Me.chkNac.Text = "Nacional"
        Me.chkNac.UseVisualStyleBackColor = True
        '
        'GroupBox11
        '
        Me.GroupBox11.Controls.Add(Me.GroupBox13)
        Me.GroupBox11.Controls.Add(Me.GroupBox12)
        Me.GroupBox11.Controls.Add(Me.chkPago)
        Me.GroupBox11.Location = New System.Drawing.Point(576, 293)
        Me.GroupBox11.Name = "GroupBox11"
        Me.GroupBox11.Size = New System.Drawing.Size(561, 203)
        Me.GroupBox11.TabIndex = 37
        Me.GroupBox11.TabStop = False
        Me.GroupBox11.Text = "Pagos"
        '
        'GroupBox13
        '
        Me.GroupBox13.Controls.Add(Me.dataBancosPago)
        Me.GroupBox13.Location = New System.Drawing.Point(308, 61)
        Me.GroupBox13.Name = "GroupBox13"
        Me.GroupBox13.Size = New System.Drawing.Size(238, 136)
        Me.GroupBox13.TabIndex = 2
        Me.GroupBox13.TabStop = False
        Me.GroupBox13.Text = "Banco"
        '
        'dataBancosPago
        '
        Me.dataBancosPago.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataBancosPago.Location = New System.Drawing.Point(6, 26)
        Me.dataBancosPago.Name = "dataBancosPago"
        Me.dataBancosPago.Size = New System.Drawing.Size(226, 104)
        Me.dataBancosPago.TabIndex = 0
        '
        'GroupBox12
        '
        Me.GroupBox12.Controls.Add(Me.dataConceptosPago)
        Me.GroupBox12.Location = New System.Drawing.Point(20, 61)
        Me.GroupBox12.Name = "GroupBox12"
        Me.GroupBox12.Size = New System.Drawing.Size(282, 136)
        Me.GroupBox12.TabIndex = 1
        Me.GroupBox12.TabStop = False
        Me.GroupBox12.Text = "Conceptos"
        '
        'dataConceptosPago
        '
        Me.dataConceptosPago.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataConceptosPago.Location = New System.Drawing.Point(6, 23)
        Me.dataConceptosPago.Name = "dataConceptosPago"
        Me.dataConceptosPago.Size = New System.Drawing.Size(270, 107)
        Me.dataConceptosPago.TabIndex = 0
        '
        'chkPago
        '
        Me.chkPago.AutoSize = True
        Me.chkPago.Location = New System.Drawing.Point(24, 27)
        Me.chkPago.Name = "chkPago"
        Me.chkPago.Size = New System.Drawing.Size(85, 17)
        Me.chkPago.TabIndex = 0
        Me.chkPago.Text = "Cargar Pago"
        Me.chkPago.UseVisualStyleBackColor = True
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.chkReintegro)
        Me.GroupBox10.Controls.Add(Me.GroupBox7)
        Me.GroupBox10.Controls.Add(Me.GroupBox8)
        Me.GroupBox10.Location = New System.Drawing.Point(802, 12)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(335, 278)
        Me.GroupBox10.TabIndex = 36
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "Reintegros"
        '
        'chkReintegro
        '
        Me.chkReintegro.AutoSize = True
        Me.chkReintegro.Location = New System.Drawing.Point(16, 19)
        Me.chkReintegro.Name = "chkReintegro"
        Me.chkReintegro.Size = New System.Drawing.Size(106, 17)
        Me.chkReintegro.TabIndex = 15
        Me.chkReintegro.Text = "Cargar Reintegro"
        Me.chkReintegro.UseVisualStyleBackColor = True
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.dataConceptos)
        Me.GroupBox7.Location = New System.Drawing.Point(10, 42)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(319, 102)
        Me.GroupBox7.TabIndex = 9
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Conceptos"
        '
        'dataConceptos
        '
        Me.dataConceptos.AllowUserToAddRows = False
        Me.dataConceptos.AllowUserToDeleteRows = False
        Me.dataConceptos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataConceptos.Location = New System.Drawing.Point(8, 14)
        Me.dataConceptos.Name = "dataConceptos"
        Me.dataConceptos.ReadOnly = True
        Me.dataConceptos.Size = New System.Drawing.Size(302, 80)
        Me.dataConceptos.TabIndex = 0
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.dataBancos)
        Me.GroupBox8.Location = New System.Drawing.Point(10, 150)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(319, 85)
        Me.GroupBox8.TabIndex = 10
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "Banco"
        '
        'dataBancos
        '
        Me.dataBancos.AllowUserToAddRows = False
        Me.dataBancos.AllowUserToDeleteRows = False
        Me.dataBancos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataBancos.Location = New System.Drawing.Point(8, 19)
        Me.dataBancos.Name = "dataBancos"
        Me.dataBancos.ReadOnly = True
        Me.dataBancos.Size = New System.Drawing.Size(302, 60)
        Me.dataBancos.TabIndex = 0
        '
        'btnCalcular
        '
        Me.btnCalcular.Location = New System.Drawing.Point(349, 601)
        Me.btnCalcular.Name = "btnCalcular"
        Me.btnCalcular.Size = New System.Drawing.Size(75, 23)
        Me.btnCalcular.TabIndex = 35
        Me.btnCalcular.Text = "Calcular"
        Me.btnCalcular.UseVisualStyleBackColor = True
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.txtMontoReintegro)
        Me.GroupBox9.Location = New System.Drawing.Point(430, 585)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(106, 39)
        Me.GroupBox9.TabIndex = 34
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "Total"
        '
        'txtMontoReintegro
        '
        Me.txtMontoReintegro.Location = New System.Drawing.Point(16, 13)
        Me.txtMontoReintegro.Name = "txtMontoReintegro"
        Me.txtMontoReintegro.Size = New System.Drawing.Size(84, 20)
        Me.txtMontoReintegro.TabIndex = 0
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.txtDestino)
        Me.GroupBox6.Location = New System.Drawing.Point(284, 347)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(224, 40)
        Me.GroupBox6.TabIndex = 33
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Destino:"
        '
        'txtDestino
        '
        Me.txtDestino.Location = New System.Drawing.Point(9, 14)
        Me.txtDestino.Name = "txtDestino"
        Me.txtDestino.Size = New System.Drawing.Size(208, 20)
        Me.txtDestino.TabIndex = 0
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.DataGridView2)
        Me.GroupBox5.Location = New System.Drawing.Point(22, 401)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(558, 178)
        Me.GroupBox5.TabIndex = 32
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Planilla de Gastos y Viaticos:"
        '
        'DataGridView2
        '
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.fecha, Me.descripcion, Me.monto, Me.NOMBREGRID})
        Me.DataGridView2.Location = New System.Drawing.Point(9, 22)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.Size = New System.Drawing.Size(539, 150)
        Me.DataGridView2.TabIndex = 0
        '
        'fecha
        '
        Me.fecha.HeaderText = "FECHA"
        Me.fecha.Name = "fecha"
        '
        'descripcion
        '
        Me.descripcion.HeaderText = "DESCRIPCION"
        Me.descripcion.Name = "descripcion"
        Me.descripcion.Width = 300
        '
        'monto
        '
        Me.monto.HeaderText = "MONTO"
        Me.monto.Name = "monto"
        '
        'NOMBREGRID
        '
        Me.NOMBREGRID.HeaderText = "NOMBRE"
        Me.NOMBREGRID.Name = "NOMBREGRID"
        Me.NOMBREGRID.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.NOMBREGRID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.dtLlegada)
        Me.GroupBox4.Controls.Add(Me.Label6)
        Me.GroupBox4.Controls.Add(Me.dtSalida)
        Me.GroupBox4.Controls.Add(Me.Label5)
        Me.GroupBox4.Location = New System.Drawing.Point(12, 293)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(266, 96)
        Me.GroupBox4.TabIndex = 31
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Periodo:"
        '
        'dtLlegada
        '
        Me.dtLlegada.Location = New System.Drawing.Point(60, 54)
        Me.dtLlegada.Name = "dtLlegada"
        Me.dtLlegada.Size = New System.Drawing.Size(200, 20)
        Me.dtLlegada.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 60)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(48, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Llegada:"
        '
        'dtSalida
        '
        Me.dtSalida.Location = New System.Drawing.Point(60, 21)
        Me.dtSalida.Name = "dtSalida"
        Me.dtSalida.Size = New System.Drawing.Size(200, 20)
        Me.dtSalida.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 27)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(39, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Salida:"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txtViaticos)
        Me.GroupBox3.Controls.Add(Me.Label9)
        Me.GroupBox3.Controls.Add(Me.txtPeaje)
        Me.GroupBox3.Controls.Add(Me.Label8)
        Me.GroupBox3.Controls.Add(Me.txtCombustible)
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Controls.Add(Me.txtTotalFinal)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.txtDia)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Location = New System.Drawing.Point(284, 230)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(512, 57)
        Me.GroupBox3.TabIndex = 30
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Totales:"
        '
        'txtViaticos
        '
        Me.txtViaticos.Location = New System.Drawing.Point(336, 17)
        Me.txtViaticos.Name = "txtViaticos"
        Me.txtViaticos.Size = New System.Drawing.Size(56, 20)
        Me.txtViaticos.TabIndex = 9
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(292, 24)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(47, 13)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Viaticos:"
        '
        'txtPeaje
        '
        Me.txtPeaje.Location = New System.Drawing.Point(232, 17)
        Me.txtPeaje.Name = "txtPeaje"
        Me.txtPeaje.Size = New System.Drawing.Size(54, 20)
        Me.txtPeaje.TabIndex = 7
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(198, 24)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(37, 13)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Peaje:"
        '
        'txtCombustible
        '
        Me.txtCombustible.Location = New System.Drawing.Point(137, 17)
        Me.txtCombustible.Name = "txtCombustible"
        Me.txtCombustible.Size = New System.Drawing.Size(55, 20)
        Me.txtCombustible.TabIndex = 5
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(73, 24)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(67, 13)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Combustible:"
        '
        'txtTotalFinal
        '
        Me.txtTotalFinal.Location = New System.Drawing.Point(439, 17)
        Me.txtTotalFinal.Name = "txtTotalFinal"
        Me.txtTotalFinal.Size = New System.Drawing.Size(59, 20)
        Me.txtTotalFinal.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(398, 24)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(45, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Importe:"
        '
        'txtDia
        '
        Me.txtDia.Location = New System.Drawing.Point(34, 17)
        Me.txtDia.Name = "txtDia"
        Me.txtDia.Size = New System.Drawing.Size(33, 20)
        Me.txtDia.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Dias:"
        '
        'btnImprimir
        '
        Me.btnImprimir.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btnImprimir.Location = New System.Drawing.Point(918, 540)
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.Size = New System.Drawing.Size(107, 54)
        Me.btnImprimir.TabIndex = 29
        Me.btnImprimir.Text = "Imprimir"
        Me.btnImprimir.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.btnImprimir.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.DataGridView1)
        Me.GroupBox2.Location = New System.Drawing.Point(269, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(501, 188)
        Me.GroupBox2.TabIndex = 28
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Vista Previa:"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(6, 19)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(489, 151)
        Me.DataGridView1.TabIndex = 2
        '
        'btnSalir
        '
        Me.btnSalir.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btnSalir.Location = New System.Drawing.Point(1030, 539)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(107, 54)
        Me.btnSalir.TabIndex = 27
        Me.btnSalir.Text = "Salir"
        Me.btnSalir.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.btnSalir.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmbA)
        Me.GroupBox1.Controls.Add(Me.cmbAnio)
        Me.GroupBox1.Controls.Add(Me.txtSaliente)
        Me.GroupBox1.Controls.Add(Me.chkAutorizado)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.cmbTipoOrden)
        Me.GroupBox1.Controls.Add(Me.btnBuscar)
        Me.GroupBox1.Controls.Add(Me.txtCodigo)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 77)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(251, 210)
        Me.GroupBox1.TabIndex = 26
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Busqueda:"
        '
        'cmbA
        '
        Me.cmbA.AutoSize = True
        Me.cmbA.Location = New System.Drawing.Point(9, 56)
        Me.cmbA.Name = "cmbA"
        Me.cmbA.Size = New System.Drawing.Size(26, 13)
        Me.cmbA.TabIndex = 29
        Me.cmbA.Text = "Año"
        '
        'cmbAnio
        '
        Me.cmbAnio.FormattingEnabled = True
        Me.cmbAnio.Location = New System.Drawing.Point(108, 53)
        Me.cmbAnio.Name = "cmbAnio"
        Me.cmbAnio.Size = New System.Drawing.Size(121, 21)
        Me.cmbAnio.TabIndex = 28
        '
        'txtSaliente
        '
        Me.txtSaliente.Location = New System.Drawing.Point(10, 174)
        Me.txtSaliente.Name = "txtSaliente"
        Me.txtSaliente.Size = New System.Drawing.Size(53, 20)
        Me.txtSaliente.TabIndex = 27
        '
        'chkAutorizado
        '
        Me.chkAutorizado.AutoSize = True
        Me.chkAutorizado.Location = New System.Drawing.Point(10, 122)
        Me.chkAutorizado.Name = "chkAutorizado"
        Me.chkAutorizado.Size = New System.Drawing.Size(76, 17)
        Me.chkAutorizado.TabIndex = 6
        Me.chkAutorizado.Text = "Autorizado"
        Me.chkAutorizado.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(8, 153)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(80, 13)
        Me.Label12.TabIndex = 26
        Me.Label12.Text = "Orden Saliente:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(8, 85)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(78, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Tipo de Orden:"
        '
        'cmbTipoOrden
        '
        Me.cmbTipoOrden.FormattingEnabled = True
        Me.cmbTipoOrden.Location = New System.Drawing.Point(108, 82)
        Me.cmbTipoOrden.Name = "cmbTipoOrden"
        Me.cmbTipoOrden.Size = New System.Drawing.Size(122, 21)
        Me.cmbTipoOrden.TabIndex = 3
        '
        'btnBuscar
        '
        Me.btnBuscar.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btnBuscar.Location = New System.Drawing.Point(108, 136)
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
        'actualizarViatico
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1153, 639)
        Me.Controls.Add(Me.chkRes)
        Me.Controls.Add(Me.chkDisp)
        Me.Controls.Add(Me.txtNro)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.GroupBox14)
        Me.Controls.Add(Me.chkProv)
        Me.Controls.Add(Me.chkNac)
        Me.Controls.Add(Me.GroupBox11)
        Me.Controls.Add(Me.GroupBox10)
        Me.Controls.Add(Me.btnCalcular)
        Me.Controls.Add(Me.GroupBox9)
        Me.Controls.Add(Me.GroupBox6)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.btnImprimir)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.btnSalir)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "actualizarViatico"
        Me.Text = "actualizarViatico"
        Me.GroupBox14.ResumeLayout(False)
        Me.GroupBox14.PerformLayout()
        Me.GroupBox11.ResumeLayout(False)
        Me.GroupBox11.PerformLayout()
        Me.GroupBox13.ResumeLayout(False)
        CType(Me.dataBancosPago, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox12.ResumeLayout(False)
        CType(Me.dataConceptosPago, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox10.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        CType(Me.dataConceptos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox8.ResumeLayout(False)
        CType(Me.dataBancos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox9.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkRes As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisp As System.Windows.Forms.CheckBox
    Friend WithEvents txtNro As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents GroupBox14 As System.Windows.Forms.GroupBox
    Friend WithEvents txtOtrosGastos As System.Windows.Forms.TextBox
    Friend WithEvents chkProv As System.Windows.Forms.CheckBox
    Friend WithEvents chkNac As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox11 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox13 As System.Windows.Forms.GroupBox
    Friend WithEvents dataBancosPago As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox12 As System.Windows.Forms.GroupBox
    Friend WithEvents dataConceptosPago As System.Windows.Forms.DataGridView
    Friend WithEvents chkPago As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Friend WithEvents chkReintegro As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents dataConceptos As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents dataBancos As System.Windows.Forms.DataGridView
    Friend WithEvents btnCalcular As System.Windows.Forms.Button
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents txtMontoReintegro As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents txtDestino As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents descripcion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents monto As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NOMBREGRID As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents dtLlegada As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents dtSalida As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents txtViaticos As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtPeaje As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtCombustible As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtTotalFinal As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtDia As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnImprimir As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents btnSalir As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmbA As System.Windows.Forms.Label
    Friend WithEvents cmbAnio As System.Windows.Forms.ComboBox
    Friend WithEvents txtSaliente As System.Windows.Forms.TextBox
    Friend WithEvents chkAutorizado As System.Windows.Forms.CheckBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbTipoOrden As System.Windows.Forms.ComboBox
    Friend WithEvents btnBuscar As System.Windows.Forms.Button
    Friend WithEvents txtCodigo As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
