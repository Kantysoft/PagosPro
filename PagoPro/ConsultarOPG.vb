Imports System.Data.SqlClient
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.Image
Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet

Public Class ConsultarOPG

    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtProveedor.TextChanged
        Dim sql
        DataGridView1.Rows.Clear()
        sql = "SELECT [opg_fec_pago],[id],[opg_ordenalt],[tdd_bnf_id],[bnf_numdoc],[suc_entrega],[opg_idopgcli],[opg_imp_pago],[cta_cuentadebito],[opg_cuentapago],[mpg_id],[opg_mar_regchq],[opg_fec_pago],[opg_fec_pagodiferido],[usr_firma1] FROM [dbo].[AAOrdenPago2] WHERE [opg_ordenalt] like '%" & txtProveedor.Text & "%'"

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Do While reader.Read()
                    Dim fila(20) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    fila(2) = reader(2)
                    fila(3) = reader(3)
                    fila(4) = reader(4)
                    fila(5) = reader(5)
                    fila(6) = reader(6)
                    fila(7) = reader(7)
                    fila(8) = reader(8)
                    fila(9) = reader(9)
                    fila(10) = reader(10)
                    fila(11) = reader(11)
                    fila(12) = reader(12)
                    fila(13) = reader(13)
                    fila(14) = reader(14)
                    DataGridView1.Rows.Add(fila)
                Loop
            End Using
        End Using
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Me.Dispose()
    End Sub

    Private Sub ConsultarOPG_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        cargarDataGrid()
        identificarUsuarios()
    End Sub
    Public Sub identificarUsuarios()
        If (labelTipo.Text.Equals("CONTROL1")) Then
            btnFirma.Visible = False
        End If
        If (labelTipo.Text.Equals("CONTROL2") Or labelTipo.Text.Equals("CONTROL3")) Then
            btnVer.Visible = False
        End If
        
    End Sub
    Public Sub cargarDataGrid()
        Dim sql
        DataGridView1.Rows.Clear()
        sql = "SELECT [opg_fec_pago],[id],[opg_ordenalt],[tdd_bnf_id],[bnf_numdoc],[suc_entrega],[opg_idopgcli],[opg_imp_pago],[cta_cuentadebito],[opg_cuentapago],[mpg_id],[opg_mar_regchq],[opg_fec_pago],[opg_fec_pagodiferido],[usr_firma1],[usr_firma2] FROM [dbo].[AAOrdenPago2] "
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Do While reader.Read()
                    Dim fila(20) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    fila(2) = reader(2)
                    fila(3) = reader(3)
                    fila(4) = reader(4)
                    fila(5) = reader(5)
                    fila(6) = reader(6)
                    fila(7) = reader(7)
                    fila(8) = reader(8)
                    fila(9) = reader(9)
                    fila(10) = reader(10)
                    fila(11) = reader(11)
                    fila(12) = reader(12)
                    fila(13) = reader(13)
                    fila(14) = reader(14)
                    fila(15) = reader(15)
                    DataGridView1.Rows.Add(fila)
                Loop
            End Using
        End Using
        DataGridView1.Rows(0).Selected = True
    End Sub
    Public Sub cargarDataGridFirma1()
        Dim sql
        DataGridView1.Rows.Clear()
        sql = "SELECT [opg_fec_pago],[id],[opg_ordenalt],[tdd_bnf_id],[bnf_numdoc],[suc_entrega],[opg_idopgcli],[opg_imp_pago],[cta_cuentadebito],[opg_cuentapago],[mpg_id],[opg_mar_regchq],[opg_fec_pago],[opg_fec_pagodiferido],[usr_firma1],[usr_firma2] FROM [dbo].[AAOrdenPago2] WHERE [usr_firma1]='' "
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Do While reader.Read()
                    Dim fila(20) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    fila(2) = reader(2)
                    fila(3) = reader(3)
                    fila(4) = reader(4)
                    fila(5) = reader(5)
                    fila(6) = reader(6)
                    fila(7) = reader(7)
                    fila(8) = reader(8)
                    fila(9) = reader(9)
                    fila(10) = reader(10)
                    fila(11) = reader(11)
                    fila(12) = reader(12)
                    fila(13) = reader(13)
                    fila(14) = reader(14)
                    fila(15) = reader(15)
                    DataGridView1.Rows.Add(fila)
                Loop
            End Using
        End Using
        DataGridView1.Rows(0).Selected = True
    End Sub
    Public Sub cargarDataGridFirma2()
        Dim sql
        DataGridView1.Rows.Clear()
        sql = "SELECT [opg_fec_pago],[id],[opg_ordenalt],[tdd_bnf_id],[bnf_numdoc],[suc_entrega],[opg_idopgcli],[opg_imp_pago],[cta_cuentadebito],[opg_cuentapago],[mpg_id],[opg_mar_regchq],[opg_fec_pago],[opg_fec_pagodiferido],[usr_firma1],[usr_firma2] FROM [dbo].[AAOrdenPago2] WHERE [usr_firma2]='' "
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Do While reader.Read()
                    Dim fila(20) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    fila(2) = reader(2)
                    fila(3) = reader(3)
                    fila(4) = reader(4)
                    fila(5) = reader(5)
                    fila(6) = reader(6)
                    fila(7) = reader(7)
                    fila(8) = reader(8)
                    fila(9) = reader(9)
                    fila(10) = reader(10)
                    fila(11) = reader(11)
                    fila(12) = reader(12)
                    fila(13) = reader(13)
                    fila(14) = reader(14)
                    fila(15) = reader(15)
                    DataGridView1.Rows.Add(fila)
                Loop
            End Using
        End Using
        DataGridView1.Rows(0).Selected = True
    End Sub


    Private Sub txtCausaEmision_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCausaEmision.TextChanged
        buscarPorCausaEmision()
    End Sub
    Public Sub buscarPorCausaEmision()
        Dim sql
        DataGridView1.Rows.Clear()
        sql = "SELECT [opg_fec_pago],[id],[opg_ordenalt],[tdd_bnf_id],[bnf_numdoc],[suc_entrega],[opg_idopgcli],[opg_imp_pago],[cta_cuentadebito],[opg_cuentapago],[mpg_id],[opg_mar_regchq],[opg_fec_pago],[opg_fec_pagodiferido],[usr_firma1] FROM [dbo].[AAOrdenPago2] WHERE [opg_idopgcli] LIKE '%" & txtCausaEmision.Text & "%'"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Do While reader.Read()
                    Dim fila(20) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    fila(2) = reader(2)
                    fila(3) = reader(3)
                    fila(4) = reader(4)
                    fila(5) = reader(5)
                    fila(6) = reader(6)
                    fila(7) = reader(7)
                    fila(8) = reader(8)
                    fila(9) = reader(9)
                    fila(10) = reader(10)
                    fila(11) = reader(11)
                    fila(12) = reader(12)
                    fila(13) = reader(13)
                    fila(14) = reader(14)
                    DataGridView1.Rows.Add(fila)
                Loop
            End Using
        End Using
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        buscarRetenciones()

    End Sub
    Public Sub buscarRetenciones()
        Dim I As Integer = DataGridView1.CurrentCell.RowIndex
        Dim ordenPago As String
        Dim tipoDni As String
        Dim zona As String
        Dim secuencia As String
        Dim texto As String
        Dim firma As String
        ordenPago = DataGridView1.Rows(I).Cells(6).Value.ToString
        Dim sql
        DataGridView2.Rows.Clear()
        sql = "SELECT [opg_idopgcli],[tipo_id],[zona_id],[secuencia_id],[texto],[usr_id]  FROM [dbo].[AARetenciones] where [opg_idopgcli]='" & ordenPago & "'"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Do While reader.Read()
                    Dim fila(20) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    fila(2) = reader(2)
                    fila(3) = reader(3)
                    fila(4) = reader(4)
                    fila(5) = reader(5)
                    DataGridView2.Rows.Add(fila)
                Loop
            End Using
        End Using

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs)
        comprobacionExportarcion()
    End Sub
    Public Sub comprobacionExportarcion()
        Dim tipoDoc As String
        Dim nroDoc As String
        Dim sucursalEntrega As String
        Dim ordenPago As String
        Dim razonSocial As String
        Dim importePago As Integer
        Dim cuentaDebito As String
        Dim cuentaPago As String
        Dim formaPago As Integer
        Dim chequeMarcaRegistracion As String
        Dim fechaPago As Date
        Dim fechaPagoDiferido As String
        Dim firma1 As String

        ''''''''''''''''
        Dim sql As String
        'sql = "SELECT [tdd_bnf_id],[bnf_numdoc],[suc_entrega],[opg_idopgcli],[opg_ordenalt],[opg_imp_pago],[cta_cuentadebito],[opg_cuentapago],[mpg_id],[opg_mar_regchq],[opg_fec_pago],[opg_fec_pagodiferido],[usr_firma1],[usr_firma2],[usr_firma3] FROM [dbo].[AAOrdenPago2] WHERE [opg_fec_pago] between '" & dateDesde.Text & "' and '" & dateHasta.Text & "'"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    tipoDoc = reader(0)
                    nroDoc = reader(1)
                    sucursalEntrega = reader(2)
                    ordenPago = reader(3).ToString
                    razonSocial = reader(4).ToString
                    importePago = reader(5).ToString
                    cuentaDebito = reader(6).ToString

                    cuentaPago = reader(7).ToString
                    formaPago = reader(8).ToString
                    chequeMarcaRegistracion = reader(9)
                    fechaPago = reader(10)
                    'If (chkEstadoPagoDiferido.Checked) Then
                    'fechaPagoDiferido = reader(11)
                    'Else
                    fechaPagoDiferido = ""
                    'End If
                    firma1 = reader(12)
                    ''''''''''''''''''''''''
                    exportar(tipoDoc, nroDoc, sucursalEntrega, ordenPago, razonSocial, importePago, cuentaDebito, cuentaPago, formaPago, chequeMarcaRegistracion, fechaPago, fechaPagoDiferido, firma1)
                End While
            End Using
        End Using
        MessageBox.Show("EXPORTADO CORRECTAMENTE")
    End Sub
    Public Sub exportar(ByVal tipoDoc As String, ByVal nroDoc As String, ByVal sucursalEntrega As String, ByVal ordenPago As String, ByVal razonSocial As String, ByVal importePago As Double, ByRef cuentaDebito As String, ByVal cuentaPago As String, ByVal formaPago As String, ByVal chequeMarcaRegistracion As String, ByVal fechaPago As Date, ByVal fechaPagoDiferido As String, ByVal firma1 As String)
        Dim sRenglon As String = Nothing
        Dim strStreamW As Stream = Nothing
        Dim strStreamWriter As StreamWriter = Nothing
        Dim ContenidoArchivo As String = Nothing

        '' Donde guardamos los paths de los archivos que vamos a estar utilizando ..

        Dim PathArchivo As String
        Dim nroIngreso As String
        Dim concepto As String
        Dim fecha As String
        Dim i As Integer
        Try
            If Directory.Exists("C:\Exportaciones") = False Then 'si no existe la carpeta se crea
                Directory.CreateDirectory("C:\Exportaciones")
            End If

            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            'If chkCTES.Checked Then
            PathArchivo = "C:\Exportaciones\OPG.txt" ' Se determina el nombre del archivo con la fecha actual

            ''verificamos si existe el archivo

            If File.Exists(PathArchivo) Then
                strStreamW = File.Open(PathArchivo, FileMode.Append) 'Abrimos el archivo
            Else
                strStreamW = File.Create(PathArchivo) ' lo creamos
            End If

            strStreamWriter = New StreamWriter(strStreamW, System.Text.Encoding.Default) ' tipo de codificacion para escritura


            ''asignamos las varibles
            If (tipoDoc.Equals("1")) Then
                tipoDoc = "10"
            Else
                tipoDoc = tipoDoc.ToString
            End If
            nroDoc = nroDoc.ToString
            '''''''''''''''''''''''''''''''''''''''''
            sucursalEntrega = sucursalEntrega.ToString
            If sucursalEntrega.ToString.Length = 1 Then
                'agrega 7 ceros  y despues adjunta el nroCargo
                sucursalEntrega = "00" & sucursalEntrega
            End If
            'pregunta si nroCargo tiene una longitud de 2
            If sucursalEntrega.ToString.Length = 2 Then
                'agrega 6 ceros  y despues adjunta el nroCargo
                sucursalEntrega = "0" & sucursalEntrega
            End If
            If sucursalEntrega.ToString.Length >= 3 Then
                'agrega 6 ceros  y despues adjunta el nroCargo
                sucursalEntrega = sucursalEntrega
            End If

            '''''''''''''''''''''''''''''''''''''''''
            ordenPago = ordenPago.ToString
            razonSocial = razonSocial
            importePago = CDec(importePago.ToString)
            cuentaDebito = cuentaDebito.ToString
            cuentaPago = cuentaPago.ToString

            formaPago = formaPago.ToString
            chequeMarcaRegistracion = chequeMarcaRegistracion.ToString
            fechaPago = fechaPago.ToString
            fechaPagoDiferido = fechaPagoDiferido.ToString
            firma1 = firma1.ToString

            'codigoProveedor = TextBox1.Text.PadRight(20)
            'descripcionProveedor = TextBox2.Text
            'cuenta = ComboBox2.SelectedItem.ToString.PadRight(20)
            'banco = TextBox5.Text

            'Dim nroIngreso As String
            'Dim concepto As String
            'Dim fecha As String
            'Dim transportista As String
            'Dim vin As String
            'Dim nroMotor As String
            'Dim sds As String


            'nroPedido = "01"  ' evaluar si es posible cambiar toString a valor numerico
            ''escribimos en el archivo

            'transportista = TextBox5.Text.PadRight(20)
            'vin = nroVinTxt.Text.ToString.PadRight(17)
            'nroMotor = nroMotorTxt.Text.ToString.PadRight(16)
            'sds = nroSDSTxt.Text.ToString.PadRight(10)
            ''responsablePedido = TextBox2.Text.ToString.PadRight(20)
            'strStreamWriter.WriteLine(nroPedido & tipoPedido & formatoEnvio & transportista & vin & nroMotor & sds & responsablePedido)
            'For Each row As DataGridViewRow In DataGridView1.Rows

            'id = ";"
            'reseller = ";"
            'shop_name = ";"
            'nroRegistro = "02"
            ''MessageBox.Show("QUE LOOOOCOOOO")
            'codigoArticulo = row.Cells(0).Value
            'If (codigoArticulo <> "") Then
            '    codigoArticulo = codigoArticulo.ToString.PadRight(21, "")

            'End If
            'If (codigoArticulo = "") Then
            '    codigoArticulo = "                     "

            'End If

            ' evaluar si es posible cambiar toString a valor numerico
            'codigoArticulo = FormatNumber(row.Cells(0).Value, 0, , , TriState.True)

            'DescripcionArticulo = row.Cells(1).Value
            'cantidadPedida = FormatNumber(row.Cells(2).Value, 0, , , TriState.True)
            'cantidadPedida2 = cantidadPedida.PadLeft(6, "0")


            'Next
            strStreamWriter.WriteLine(tipoDoc & vbTab & nroDoc & vbTab & sucursalEntrega & vbTab & ordenPago & vbTab & razonSocial & vbTab & importePago & vbTab & cuentaDebito & vbTab & cuentaPago & vbTab & formaPago & vbTab & chequeMarcaRegistracion & vbTab & fechaPago & vbTab & fechaPagoDiferido & vbTab & firma1)

            'Dim cierre As String
            'cierre = "----------------------------------------------------------------------"
            '' strStreamWriter.WriteLine("Primera linea en un archivo txt desde visual basic.Net")
            'strStreamWriter.WriteLine(nroPedido & tipoPedido & formatoEnvio & transportista & vin)
            'strStreamWriter.WriteLine(cierre)
            'strStreamWriter.WriteLine(cuenta & banco)
            For Each row As DataGridViewRow In DataGridView1.Rows
                nroIngreso = "1111"
                'nroIngreso = row.Cells(0).Value.ToString
                'concepto = row.Cells(1).Value.ToString.PadRight(20)
                'fecha = row.Cells(2).Value.ToString.PadRight(20)
                'MessageBox.Show("nroIngreso" & nroIngreso)
                'strStreamWriter.WriteLine(nroIngreso)
            Next
            'strStreamWriter.WriteLine(cierre)
            strStreamWriter.Close() ' cerramos


        Catch ex As Exception
            MsgBox("Error al Guardar la informacion en el archivo. " & ex.ToString, MsgBoxStyle.Critical, Application.ProductName)
            strStreamWriter.Close() ' cerramos
        End Try

    End Sub

    Private Sub Button2_Click_1(sender As System.Object, e As System.EventArgs) Handles btnFirma.Click
        buscar()
    End Sub
    Public Sub buscar()
        Dim I As Integer = DataGridView1.CurrentCell.RowIndex
        Dim transaccion As Integer = DataGridView1.Rows(I).Cells(1).Value

        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd.CommandText = "SELECT [id],[tdd_bnf_id],[bnf_numdoc],[suc_entrega],[opg_idopgcli],[opg_ordenalt],[opg_imp_pago],[cta_cuentadebito],[opg_cuentapago],[mpg_id],[opg_mar_regchq],[opg_fec_pago],[opg_fec_pagodiferido],[usr_firma1],[usr_firma2],[usr_firma3] FROM [SBDASIPT].[dbo].[AAOrdenPago2] WHERE [id]=" & transaccion & ""
                Dim reader As SqlDataReader = Nothing
                reader = cmd.ExecuteReader
                If reader.Read() Then
                    buscarOPG.txtFecha.Text = reader(11)
                    buscarOPG.ComboRazonSocial.Text = reader(5)
                    buscarOPG.txtTipoDocumento.Text = reader(1)
                    buscarOPG.txtNroDocumento.Text = reader(2)
                    buscarOPG.txtSucEntrega.Text = reader(3)
                    buscarOPG.txtOrdenPago.Text = reader(4)
                    buscarOPG.txtOrdenPagoBis.Text = reader(5)
                    buscarOPG.txtImportePago.Text = reader(6)
                    buscarOPG.txtCuentaDebito.Text = reader(7)
                    buscarOPG.txtCuentaPago.Text = reader(8)
                    buscarOPG.ComboFormaPago.Text = reader(9)
                    buscarOPG.DateFechaPago.Text = reader(11)
                    buscarOPG.DateFechaPagoDiferido.Text = reader(11)
                    buscarOPG.txtFirmante1.Text = reader(13)
                    'If (Not reader(13).Equals("") Or Not reader(13).Equals(Nothing)) Then
                    ' buscarOPG.txtFirmante1.Enabled = False
                    'End If
                    buscarOPG.txtFirmante2.Text = reader(14)
                    'If (Not reader(13).Equals("")) Then
                    ' buscarOPG.txtFirmante2.Enabled = False
                    'End If

                    buscarOPG.labelUsuario.Text = labelUsuario.Text
                    buscarOPG.laberTIpo.Text = labelTipo.Text
                    buscarOPG.LabelTransaccion.Text = reader(0)
                    buscarOPG.Show()
                End If
            End Using
        End Using

    End Sub

    Private Sub btnVer_Click(sender As System.Object, e As System.EventArgs) Handles btnVer.Click
        buscar()
    End Sub

    Private Sub btnActualizar_Click(sender As System.Object, e As System.EventArgs) Handles btnActualizar.Click
        cargarDataGrid()
    End Sub

    Private Sub Button2_Click_2(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        ExportarRangoFecha.Show()
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkFirma1.CheckedChanged
        If (chkFirma1.Checked) Then
            cargarDataGridFirma1()
            chkFirma2.Enabled = False
        Else
            cargarDataGrid()
            chkFirma2.Enabled = True
        End If

    End Sub

    Private Sub chkFirma2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkFirma2.CheckedChanged
        If (chkFirma2.Checked) Then
            cargarDataGridFirma2()
            chkFirma1.Enabled = False
        Else
            cargarDataGrid()
            chkFirma1.Enabled = True
        End If

    End Sub

    Private Sub DataGridView1_DoubleClick(sender As Object, e As System.EventArgs) Handles DataGridView1.DoubleClick
        Dim I As Integer = DataGridView1.CurrentCell.RowIndex
        Dim dni As String
        Dim NroTransaccion As String
        Dim RazonSocial As String
        Dim ordenPago As String
        Dim monto As String

        Dim telefono As String
        Dim certificado As String
        Dim fechaVencimiento As String
        Dim expediente As String
        NroTransaccion = DataGridView1.Rows(I).Cells(1).Value.ToString
        dni = DataGridView1.Rows(I).Cells(4).Value.ToString
        RazonSocial = DataGridView1.Rows(I).Cells(2).Value.ToString
        ordenPago = DataGridView1.Rows(I).Cells(6).Value.ToString
        monto = DataGridView1.Rows(I).Cells(7).Value.ToString

        DeleteOPG.txtNroTransaccion.Text = NroTransaccion
        DeleteOPG.txtDni.Text = dni
        DeleteOPG.txtNombre.Text = RazonSocial
        DeleteOPG.txtOrdenPago.Text = ordenPago
        DeleteOPG.txtMonto.Text = monto
        DeleteOPG.Show()


        
    End Sub
End Class