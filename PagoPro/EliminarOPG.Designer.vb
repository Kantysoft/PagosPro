<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EliminarOPG
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkEstadoPagoDiferido = New System.Windows.Forms.CheckBox()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.labelTipoDocumento = New System.Windows.Forms.Label()
        Me.txtFirmante3 = New System.Windows.Forms.TextBox()
        Me.botonCargar = New System.Windows.Forms.Button()
        Me.txtFirmante2 = New System.Windows.Forms.TextBox()
        Me.txtFirmante1 = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.ComboFormaPago = New System.Windows.Forms.ComboBox()
        Me.ComboRazonSocial = New System.Windows.Forms.ComboBox()
        Me.DateFechaPagoDiferido = New System.Windows.Forms.DateTimePicker()
        Me.DateFechaPago = New System.Windows.Forms.DateTimePicker()
        Me.ComboCheque = New System.Windows.Forms.TextBox()
        Me.txtCuentaPago = New System.Windows.Forms.TextBox()
        Me.txtCuentaDebito = New System.Windows.Forms.TextBox()
        Me.txtImportePago = New System.Windows.Forms.TextBox()
        Me.txtOrdenPagoBis = New System.Windows.Forms.TextBox()
        Me.txtOrdenPago = New System.Windows.Forms.TextBox()
        Me.txtSucEntrega = New System.Windows.Forms.TextBox()
        Me.txtNroDocumento = New System.Windows.Forms.TextBox()
        Me.txtTipoDocumento = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtFecha = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.ordenPago = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.tipo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.zona = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.secuencia = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.texto = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.firma = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.id = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtDocumento = New System.Windows.Forms.TextBox()
        Me.dateFechaBusqueda = New System.Windows.Forms.DateTimePicker()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.fecha = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.razonSocial = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.tipo_Doc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.nroDocumento = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.sucEntrega = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.OPagoR = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.importe = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cuentaDebito = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cuentaPago = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.formaPago = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.marcaCheque = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.fechaPago = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.fechePagoDiferido = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.nroOrdenPago = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkEstadoPagoDiferido)
        Me.GroupBox1.Controls.Add(Me.Label27)
        Me.GroupBox1.Controls.Add(Me.labelTipoDocumento)
        Me.GroupBox1.Controls.Add(Me.txtFirmante3)
        Me.GroupBox1.Controls.Add(Me.botonCargar)
        Me.GroupBox1.Controls.Add(Me.txtFirmante2)
        Me.GroupBox1.Controls.Add(Me.txtFirmante1)
        Me.GroupBox1.Controls.Add(Me.Label17)
        Me.GroupBox1.Controls.Add(Me.Label16)
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.ComboFormaPago)
        Me.GroupBox1.Controls.Add(Me.ComboRazonSocial)
        Me.GroupBox1.Controls.Add(Me.DateFechaPagoDiferido)
        Me.GroupBox1.Controls.Add(Me.DateFechaPago)
        Me.GroupBox1.Controls.Add(Me.ComboCheque)
        Me.GroupBox1.Controls.Add(Me.txtCuentaPago)
        Me.GroupBox1.Controls.Add(Me.txtCuentaDebito)
        Me.GroupBox1.Controls.Add(Me.txtImportePago)
        Me.GroupBox1.Controls.Add(Me.txtOrdenPagoBis)
        Me.GroupBox1.Controls.Add(Me.txtOrdenPago)
        Me.GroupBox1.Controls.Add(Me.txtSucEntrega)
        Me.GroupBox1.Controls.Add(Me.txtNroDocumento)
        Me.GroupBox1.Controls.Add(Me.txtTipoDocumento)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtFecha)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(23, 209)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(712, 309)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Orden Pago"
        '
        'chkEstadoPagoDiferido
        '
        Me.chkEstadoPagoDiferido.AutoSize = True
        Me.chkEstadoPagoDiferido.Location = New System.Drawing.Point(540, 168)
        Me.chkEstadoPagoDiferido.Name = "chkEstadoPagoDiferido"
        Me.chkEstadoPagoDiferido.Size = New System.Drawing.Size(56, 17)
        Me.chkEstadoPagoDiferido.TabIndex = 38
        Me.chkEstadoPagoDiferido.Text = "Activo"
        Me.chkEstadoPagoDiferido.UseVisualStyleBackColor = True
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(540, 87)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(0, 13)
        Me.Label27.TabIndex = 37
        '
        'labelTipoDocumento
        '
        Me.labelTipoDocumento.AutoSize = True
        Me.labelTipoDocumento.Location = New System.Drawing.Point(236, 90)
        Me.labelTipoDocumento.Name = "labelTipoDocumento"
        Me.labelTipoDocumento.Size = New System.Drawing.Size(0, 13)
        Me.labelTipoDocumento.TabIndex = 36
        '
        'txtFirmante3
        '
        Me.txtFirmante3.Location = New System.Drawing.Point(400, 255)
        Me.txtFirmante3.Name = "txtFirmante3"
        Me.txtFirmante3.Size = New System.Drawing.Size(134, 20)
        Me.txtFirmante3.TabIndex = 35
        '
        'botonCargar
        '
        Me.botonCargar.Location = New System.Drawing.Point(111, 267)
        Me.botonCargar.Name = "botonCargar"
        Me.botonCargar.Size = New System.Drawing.Size(75, 23)
        Me.botonCargar.TabIndex = 13
        Me.botonCargar.Text = "Cargar"
        Me.botonCargar.UseVisualStyleBackColor = True
        '
        'txtFirmante2
        '
        Me.txtFirmante2.Location = New System.Drawing.Point(400, 228)
        Me.txtFirmante2.Name = "txtFirmante2"
        Me.txtFirmante2.Size = New System.Drawing.Size(134, 20)
        Me.txtFirmante2.TabIndex = 34
        '
        'txtFirmante1
        '
        Me.txtFirmante1.Location = New System.Drawing.Point(400, 199)
        Me.txtFirmante1.Name = "txtFirmante1"
        Me.txtFirmante1.Size = New System.Drawing.Size(134, 20)
        Me.txtFirmante1.TabIndex = 33
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(300, 258)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(85, 13)
        Me.Label17.TabIndex = 32
        Me.Label17.Text = "ident. Firmante 3"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(300, 231)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(85, 13)
        Me.Label16.TabIndex = 31
        Me.Label16.Text = "ident. Firmante 2"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(300, 202)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(85, 13)
        Me.Label15.TabIndex = 30
        Me.Label15.Text = "ident. Firmante 1"
        '
        'ComboFormaPago
        '
        Me.ComboFormaPago.FormattingEnabled = True
        Me.ComboFormaPago.Location = New System.Drawing.Point(400, 82)
        Me.ComboFormaPago.Name = "ComboFormaPago"
        Me.ComboFormaPago.Size = New System.Drawing.Size(134, 21)
        Me.ComboFormaPago.TabIndex = 29
        '
        'ComboRazonSocial
        '
        Me.ComboRazonSocial.FormattingEnabled = True
        Me.ComboRazonSocial.Location = New System.Drawing.Point(104, 56)
        Me.ComboRazonSocial.Name = "ComboRazonSocial"
        Me.ComboRazonSocial.Size = New System.Drawing.Size(120, 21)
        Me.ComboRazonSocial.TabIndex = 28
        '
        'DateFechaPagoDiferido
        '
        Me.DateFechaPagoDiferido.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateFechaPagoDiferido.Location = New System.Drawing.Point(400, 166)
        Me.DateFechaPagoDiferido.Name = "DateFechaPagoDiferido"
        Me.DateFechaPagoDiferido.Size = New System.Drawing.Size(134, 20)
        Me.DateFechaPagoDiferido.TabIndex = 27
        '
        'DateFechaPago
        '
        Me.DateFechaPago.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateFechaPago.Location = New System.Drawing.Point(400, 136)
        Me.DateFechaPago.Name = "DateFechaPago"
        Me.DateFechaPago.Size = New System.Drawing.Size(134, 20)
        Me.DateFechaPago.TabIndex = 26
        '
        'ComboCheque
        '
        Me.ComboCheque.Location = New System.Drawing.Point(400, 109)
        Me.ComboCheque.Name = "ComboCheque"
        Me.ComboCheque.Size = New System.Drawing.Size(134, 20)
        Me.ComboCheque.TabIndex = 25
        '
        'txtCuentaPago
        '
        Me.txtCuentaPago.Location = New System.Drawing.Point(400, 52)
        Me.txtCuentaPago.Name = "txtCuentaPago"
        Me.txtCuentaPago.Size = New System.Drawing.Size(134, 20)
        Me.txtCuentaPago.TabIndex = 23
        '
        'txtCuentaDebito
        '
        Me.txtCuentaDebito.Location = New System.Drawing.Point(400, 22)
        Me.txtCuentaDebito.Name = "txtCuentaDebito"
        Me.txtCuentaDebito.Size = New System.Drawing.Size(134, 20)
        Me.txtCuentaDebito.TabIndex = 22
        '
        'txtImportePago
        '
        Me.txtImportePago.Location = New System.Drawing.Point(108, 224)
        Me.txtImportePago.Name = "txtImportePago"
        Me.txtImportePago.Size = New System.Drawing.Size(116, 20)
        Me.txtImportePago.TabIndex = 21
        '
        'txtOrdenPagoBis
        '
        Me.txtOrdenPagoBis.Location = New System.Drawing.Point(106, 195)
        Me.txtOrdenPagoBis.Name = "txtOrdenPagoBis"
        Me.txtOrdenPagoBis.Size = New System.Drawing.Size(118, 20)
        Me.txtOrdenPagoBis.TabIndex = 20
        '
        'txtOrdenPago
        '
        Me.txtOrdenPago.Location = New System.Drawing.Point(106, 169)
        Me.txtOrdenPago.Name = "txtOrdenPago"
        Me.txtOrdenPago.Size = New System.Drawing.Size(118, 20)
        Me.txtOrdenPago.TabIndex = 19
        '
        'txtSucEntrega
        '
        Me.txtSucEntrega.Location = New System.Drawing.Point(106, 142)
        Me.txtSucEntrega.Name = "txtSucEntrega"
        Me.txtSucEntrega.Size = New System.Drawing.Size(118, 20)
        Me.txtSucEntrega.TabIndex = 18
        '
        'txtNroDocumento
        '
        Me.txtNroDocumento.Location = New System.Drawing.Point(106, 115)
        Me.txtNroDocumento.Name = "txtNroDocumento"
        Me.txtNroDocumento.Size = New System.Drawing.Size(118, 20)
        Me.txtNroDocumento.TabIndex = 17
        '
        'txtTipoDocumento
        '
        Me.txtTipoDocumento.Location = New System.Drawing.Point(106, 87)
        Me.txtTipoDocumento.Name = "txtTipoDocumento"
        Me.txtTipoDocumento.Size = New System.Drawing.Size(118, 20)
        Me.txtTipoDocumento.TabIndex = 16
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(300, 169)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(104, 13)
        Me.Label14.TabIndex = 14
        Me.Label14.Text = "Fecha Pago Diferido"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(300, 145)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(65, 13)
        Me.Label13.TabIndex = 13
        Me.Label13.Text = "Fecha Pago"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(300, 118)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(103, 13)
        Me.Label12.TabIndex = 12
        Me.Label12.Text = "Marca Reg. Cheque"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(300, 94)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(64, 13)
        Me.Label11.TabIndex = 11
        Me.Label11.Text = "Forma Pago"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(300, 59)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(69, 13)
        Me.Label10.TabIndex = 10
        Me.Label10.Text = "Cuenta Pago"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(300, 25)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(75, 13)
        Me.Label9.TabIndex = 9
        Me.Label9.Text = "Cuenta Debito"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 224)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(70, 13)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Importe Pago"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 198)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(97, 13)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "O.Pago/R.Soc(bis)"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 172)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(102, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Nro Orden Pago(>0)"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 142)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(69, 13)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Suc. Entrega"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 118)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Nro Doc"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 90)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Tipo Doc"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 59)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Razon Social"
        '
        'txtFecha
        '
        Me.txtFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.txtFecha.Location = New System.Drawing.Point(104, 19)
        Me.txtFecha.Name = "txtFecha"
        Me.txtFecha.Size = New System.Drawing.Size(120, 20)
        Me.txtFecha.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(92, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Fecha Referancia"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.DataGridView1)
        Me.GroupBox2.Location = New System.Drawing.Point(32, 537)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(672, 200)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Retenciones"
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ordenPago, Me.tipo, Me.zona, Me.secuencia, Me.texto, Me.firma, Me.id})
        Me.DataGridView1.Location = New System.Drawing.Point(6, 19)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(657, 164)
        Me.DataGridView1.TabIndex = 13
        '
        'ordenPago
        '
        Me.ordenPago.HeaderText = "ORDEN PAGO"
        Me.ordenPago.Name = "ordenPago"
        '
        'tipo
        '
        Me.tipo.HeaderText = "TIPO"
        Me.tipo.Name = "tipo"
        '
        'zona
        '
        Me.zona.HeaderText = "ZONA"
        Me.zona.Name = "zona"
        '
        'secuencia
        '
        Me.secuencia.HeaderText = "SECUENCIA"
        Me.secuencia.Name = "secuencia"
        '
        'texto
        '
        Me.texto.HeaderText = "TEXTO"
        Me.texto.Name = "texto"
        '
        'firma
        '
        Me.firma.HeaderText = "FIRMA"
        Me.firma.Name = "firma"
        '
        'id
        '
        Me.id.HeaderText = "ID"
        Me.id.Name = "id"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(35, 19)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(82, 13)
        Me.Label18.TabIndex = 3
        Me.Label18.Text = "Nro Documento"
        '
        'txtDocumento
        '
        Me.txtDocumento.Location = New System.Drawing.Point(123, 16)
        Me.txtDocumento.Name = "txtDocumento"
        Me.txtDocumento.Size = New System.Drawing.Size(124, 20)
        Me.txtDocumento.TabIndex = 4
        '
        'dateFechaBusqueda
        '
        Me.dateFechaBusqueda.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dateFechaBusqueda.Location = New System.Drawing.Point(302, 16)
        Me.dateFechaBusqueda.Name = "dateFechaBusqueda"
        Me.dateFechaBusqueda.Size = New System.Drawing.Size(124, 20)
        Me.dateFechaBusqueda.TabIndex = 5
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(259, 22)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(37, 13)
        Me.Label19.TabIndex = 6
        Me.Label19.Text = "Fecha"
        '
        'DataGridView2
        '
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.fecha, Me.razonSocial, Me.tipo_Doc, Me.nroDocumento, Me.sucEntrega, Me.OPagoR, Me.importe, Me.cuentaDebito, Me.cuentaPago, Me.formaPago, Me.marcaCheque, Me.fechaPago, Me.fechePagoDiferido, Me.nroOrdenPago})
        Me.DataGridView2.Location = New System.Drawing.Point(23, 53)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.Size = New System.Drawing.Size(702, 150)
        Me.DataGridView2.TabIndex = 7
        '
        'fecha
        '
        Me.fecha.HeaderText = "FECHA"
        Me.fecha.Name = "fecha"
        '
        'razonSocial
        '
        Me.razonSocial.HeaderText = "RAZON SOCIAL"
        Me.razonSocial.Name = "razonSocial"
        '
        'tipo_Doc
        '
        Me.tipo_Doc.HeaderText = "TIPO DOCUMENTO"
        Me.tipo_Doc.Name = "tipo_Doc"
        '
        'nroDocumento
        '
        Me.nroDocumento.HeaderText = "NRODOCUMENTO"
        Me.nroDocumento.Name = "nroDocumento"
        '
        'sucEntrega
        '
        Me.sucEntrega.HeaderText = "SUCURSAL ENTREGA"
        Me.sucEntrega.Name = "sucEntrega"
        '
        'OPagoR
        '
        Me.OPagoR.HeaderText = "OPGR"
        Me.OPagoR.Name = "OPagoR"
        '
        'importe
        '
        Me.importe.HeaderText = "IMPORTE"
        Me.importe.Name = "importe"
        '
        'cuentaDebito
        '
        Me.cuentaDebito.HeaderText = "CUENTA DEBITO"
        Me.cuentaDebito.Name = "cuentaDebito"
        '
        'cuentaPago
        '
        Me.cuentaPago.HeaderText = "CUENTA PAGO"
        Me.cuentaPago.Name = "cuentaPago"
        '
        'formaPago
        '
        Me.formaPago.HeaderText = "FORMA PAGO"
        Me.formaPago.Name = "formaPago"
        '
        'marcaCheque
        '
        Me.marcaCheque.HeaderText = "MARCA CHEQUE"
        Me.marcaCheque.Name = "marcaCheque"
        '
        'fechaPago
        '
        Me.fechaPago.HeaderText = "FECHA PAGO"
        Me.fechaPago.Name = "fechaPago"
        '
        'fechePagoDiferido
        '
        Me.fechePagoDiferido.HeaderText = "FechaDiferido"
        Me.fechePagoDiferido.Name = "fechePagoDiferido"
        '
        'nroOrdenPago
        '
        Me.nroOrdenPago.HeaderText = "NRO ORDEN PAGO"
        Me.nroOrdenPago.Name = "nroOrdenPago"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(443, 14)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 8
        Me.Button1.Text = "Buscar"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'EliminarOPG
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(747, 749)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.DataGridView2)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.dateFechaBusqueda)
        Me.Controls.Add(Me.txtDocumento)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "EliminarOPG"
        Me.Text = "EliminarOPG"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkEstadoPagoDiferido As System.Windows.Forms.CheckBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents labelTipoDocumento As System.Windows.Forms.Label
    Friend WithEvents txtFirmante3 As System.Windows.Forms.TextBox
    Friend WithEvents botonCargar As System.Windows.Forms.Button
    Friend WithEvents txtFirmante2 As System.Windows.Forms.TextBox
    Friend WithEvents txtFirmante1 As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents ComboFormaPago As System.Windows.Forms.ComboBox
    Friend WithEvents ComboRazonSocial As System.Windows.Forms.ComboBox
    Friend WithEvents DateFechaPagoDiferido As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateFechaPago As System.Windows.Forms.DateTimePicker
    Friend WithEvents ComboCheque As System.Windows.Forms.TextBox
    Friend WithEvents txtCuentaPago As System.Windows.Forms.TextBox
    Friend WithEvents txtCuentaDebito As System.Windows.Forms.TextBox
    Friend WithEvents txtImportePago As System.Windows.Forms.TextBox
    Friend WithEvents txtOrdenPagoBis As System.Windows.Forms.TextBox
    Friend WithEvents txtOrdenPago As System.Windows.Forms.TextBox
    Friend WithEvents txtSucEntrega As System.Windows.Forms.TextBox
    Friend WithEvents txtNroDocumento As System.Windows.Forms.TextBox
    Friend WithEvents txtTipoDocumento As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtFecha As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents ordenPago As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents tipo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents zona As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents secuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents texto As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents firma As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents id As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtDocumento As System.Windows.Forms.TextBox
    Friend WithEvents dateFechaBusqueda As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents razonSocial As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents tipo_Doc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents nroDocumento As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents sucEntrega As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OPagoR As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents importe As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cuentaDebito As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cuentaPago As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents formaPago As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents marcaCheque As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents fechaPago As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents fechePagoDiferido As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents nroOrdenPago As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Button1 As System.Windows.Forms.Button
End Class
