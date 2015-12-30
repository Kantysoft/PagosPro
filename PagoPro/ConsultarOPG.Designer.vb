<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConsultarOPG
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
        Me.txtCausaEmision = New System.Windows.Forms.TextBox()
        Me.txtProveedor = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.fecha = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.nroCheque = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.razonSocial = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.tipoDoc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.nroDocumento = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.sucEntrega = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ordenPago = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.importePago = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cuentaDebito = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cuentaPago = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.formaPago = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.marcaCheque = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.fechaPago = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.fechaPagoDiferido = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.identidadInformate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.firmante2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.ordenPago2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.tipoDocumento = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.zona = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.secuencia = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.textoLibre = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.firma = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnFirma = New System.Windows.Forms.Button()
        Me.btnVer = New System.Windows.Forms.Button()
        Me.labelTipo = New System.Windows.Forms.Label()
        Me.labelUsuario = New System.Windows.Forms.Label()
        Me.btnActualizar = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.chkFirma2 = New System.Windows.Forms.CheckBox()
        Me.chkFirma1 = New System.Windows.Forms.CheckBox()
        Me.GroupBox1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtCausaEmision)
        Me.GroupBox1.Controls.Add(Me.txtProveedor)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(206, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(544, 100)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Criterios de Busqueda"
        '
        'txtCausaEmision
        '
        Me.txtCausaEmision.Location = New System.Drawing.Point(105, 53)
        Me.txtCausaEmision.Name = "txtCausaEmision"
        Me.txtCausaEmision.Size = New System.Drawing.Size(312, 20)
        Me.txtCausaEmision.TabIndex = 3
        '
        'txtProveedor
        '
        Me.txtProveedor.Location = New System.Drawing.Point(105, 19)
        Me.txtProveedor.Name = "txtProveedor"
        Me.txtProveedor.Size = New System.Drawing.Size(312, 20)
        Me.txtProveedor.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 60)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(74, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Causa Emsion"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Proveedor"
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.fecha, Me.nroCheque, Me.razonSocial, Me.tipoDoc, Me.nroDocumento, Me.sucEntrega, Me.ordenPago, Me.importePago, Me.cuentaDebito, Me.cuentaPago, Me.formaPago, Me.marcaCheque, Me.fechaPago, Me.fechaPagoDiferido, Me.identidadInformate, Me.firmante2})
        Me.DataGridView1.Location = New System.Drawing.Point(9, 19)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(821, 150)
        Me.DataGridView1.TabIndex = 1
        '
        'fecha
        '
        Me.fecha.HeaderText = "FECHA"
        Me.fecha.Name = "fecha"
        '
        'nroCheque
        '
        Me.nroCheque.HeaderText = "NRO TRANSACCION"
        Me.nroCheque.Name = "nroCheque"
        '
        'razonSocial
        '
        Me.razonSocial.HeaderText = "RAZON SOCIAL"
        Me.razonSocial.Name = "razonSocial"
        '
        'tipoDoc
        '
        Me.tipoDoc.HeaderText = "TIPO DOCUMENTO"
        Me.tipoDoc.Name = "tipoDoc"
        '
        'nroDocumento
        '
        Me.nroDocumento.HeaderText = "NRO DOCUMENTO"
        Me.nroDocumento.Name = "nroDocumento"
        '
        'sucEntrega
        '
        Me.sucEntrega.HeaderText = "SUCURSAL ENTREGA"
        Me.sucEntrega.Name = "sucEntrega"
        '
        'ordenPago
        '
        Me.ordenPago.HeaderText = "ORDEN PAGO"
        Me.ordenPago.Name = "ordenPago"
        '
        'importePago
        '
        Me.importePago.HeaderText = "IMPORTE PAGO"
        Me.importePago.Name = "importePago"
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
        'fechaPagoDiferido
        '
        Me.fechaPagoDiferido.HeaderText = "FECHA PAGO DIFERIDO"
        Me.fechaPagoDiferido.Name = "fechaPagoDiferido"
        '
        'identidadInformate
        '
        Me.identidadInformate.HeaderText = "FIRMANTE1"
        Me.identidadInformate.Name = "identidadInformate"
        '
        'firmante2
        '
        Me.firmante2.HeaderText = "FIRMANTE2"
        Me.firmante2.Name = "firmante2"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(303, 314)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Salir"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.DataGridView1)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 118)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(836, 181)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "OPG"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.DataGridView2)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 370)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(836, 177)
        Me.GroupBox3.TabIndex = 4
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Retencion/Liquidacion"
        '
        'DataGridView2
        '
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ordenPago2, Me.tipoDocumento, Me.zona, Me.secuencia, Me.textoLibre, Me.firma})
        Me.DataGridView2.Location = New System.Drawing.Point(9, 19)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.Size = New System.Drawing.Size(821, 150)
        Me.DataGridView2.TabIndex = 0
        '
        'ordenPago2
        '
        Me.ordenPago2.HeaderText = "ORDEN PAGO"
        Me.ordenPago2.Name = "ordenPago2"
        '
        'tipoDocumento
        '
        Me.tipoDocumento.HeaderText = "TIPO DOCUMENTO"
        Me.tipoDocumento.Name = "tipoDocumento"
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
        'textoLibre
        '
        Me.textoLibre.HeaderText = "TEXTO"
        Me.textoLibre.Name = "textoLibre"
        '
        'firma
        '
        Me.firma.HeaderText = "FIRMA"
        Me.firma.Name = "firma"
        '
        'btnFirma
        '
        Me.btnFirma.Location = New System.Drawing.Point(21, 314)
        Me.btnFirma.Name = "btnFirma"
        Me.btnFirma.Size = New System.Drawing.Size(114, 23)
        Me.btnFirma.TabIndex = 5
        Me.btnFirma.Text = "Asignar Firma"
        Me.btnFirma.UseVisualStyleBackColor = True
        '
        'btnVer
        '
        Me.btnVer.Location = New System.Drawing.Point(141, 314)
        Me.btnVer.Name = "btnVer"
        Me.btnVer.Size = New System.Drawing.Size(75, 23)
        Me.btnVer.TabIndex = 6
        Me.btnVer.Text = "Ver"
        Me.btnVer.UseVisualStyleBackColor = True
        '
        'labelTipo
        '
        Me.labelTipo.AutoSize = True
        Me.labelTipo.Location = New System.Drawing.Point(18, 43)
        Me.labelTipo.Name = "labelTipo"
        Me.labelTipo.Size = New System.Drawing.Size(32, 13)
        Me.labelTipo.TabIndex = 52
        Me.labelTipo.Text = "TIPO"
        '
        'labelUsuario
        '
        Me.labelUsuario.AutoSize = True
        Me.labelUsuario.Location = New System.Drawing.Point(18, 12)
        Me.labelUsuario.Name = "labelUsuario"
        Me.labelUsuario.Size = New System.Drawing.Size(56, 13)
        Me.labelUsuario.TabIndex = 51
        Me.labelUsuario.Text = "USUARIO"
        '
        'btnActualizar
        '
        Me.btnActualizar.Location = New System.Drawing.Point(222, 314)
        Me.btnActualizar.Name = "btnActualizar"
        Me.btnActualizar.Size = New System.Drawing.Size(75, 23)
        Me.btnActualizar.TabIndex = 53
        Me.btnActualizar.Text = "Actualizar"
        Me.btnActualizar.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(628, 314)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(214, 23)
        Me.Button2.TabIndex = 54
        Me.Button2.Text = "Exportar con rango de fecha"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'chkFirma2
        '
        Me.chkFirma2.AutoSize = True
        Me.chkFirma2.Location = New System.Drawing.Point(21, 95)
        Me.chkFirma2.Name = "chkFirma2"
        Me.chkFirma2.Size = New System.Drawing.Size(57, 17)
        Me.chkFirma2.TabIndex = 55
        Me.chkFirma2.Text = "Firma2"
        Me.chkFirma2.UseVisualStyleBackColor = True
        '
        'chkFirma1
        '
        Me.chkFirma1.AutoSize = True
        Me.chkFirma1.Location = New System.Drawing.Point(21, 72)
        Me.chkFirma1.Name = "chkFirma1"
        Me.chkFirma1.Size = New System.Drawing.Size(57, 17)
        Me.chkFirma1.TabIndex = 56
        Me.chkFirma1.Text = "Firma1"
        Me.chkFirma1.UseVisualStyleBackColor = True
        '
        'ConsultarOPG
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(874, 588)
        Me.Controls.Add(Me.chkFirma1)
        Me.Controls.Add(Me.chkFirma2)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.btnActualizar)
        Me.Controls.Add(Me.labelTipo)
        Me.Controls.Add(Me.labelUsuario)
        Me.Controls.Add(Me.btnVer)
        Me.Controls.Add(Me.btnFirma)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "ConsultarOPG"
        Me.Text = "ConsultarOPG"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtCausaEmision As System.Windows.Forms.TextBox
    Friend WithEvents txtProveedor As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents ordenPago2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents tipoDocumento As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents zona As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents secuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents textoLibre As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents firma As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnFirma As System.Windows.Forms.Button
    Friend WithEvents btnVer As System.Windows.Forms.Button
    Friend WithEvents labelTipo As System.Windows.Forms.Label
    Friend WithEvents labelUsuario As System.Windows.Forms.Label
    Friend WithEvents btnActualizar As System.Windows.Forms.Button
    Friend WithEvents fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents nroCheque As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents razonSocial As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents tipoDoc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents nroDocumento As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents sucEntrega As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ordenPago As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents importePago As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cuentaDebito As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cuentaPago As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents formaPago As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents marcaCheque As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents fechaPago As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents fechaPagoDiferido As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents identidadInformate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents firmante2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents chkFirma2 As System.Windows.Forms.CheckBox
    Friend WithEvents chkFirma1 As System.Windows.Forms.CheckBox
End Class
