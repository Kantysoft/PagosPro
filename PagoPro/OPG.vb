Imports System.Data.SqlClient
'Imports iTextSharp
'Imports iTextSharp.text
'Imports iTextSharp.text.pdf
'Imports iTextSharp.text.Image
'Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet

Public Class OPG

    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtOrdenPagoBis.TextChanged

    End Sub

    Private Sub TextBox5_TextChanged(sender As System.Object, e As System.EventArgs) Handles ComboCheque.TextChanged

    End Sub

    Private Sub Label21_Click(sender As System.Object, e As System.EventArgs) Handles Label21.Click

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs)
        comprobacionExportarcion()
    End Sub
    Public Sub comprobacionExportarcion()
        'Dim tipoDoc As String
        'Dim nroDoc As String
        'Dim sucursalEntrega As String
        'Dim ordenPago As String
        'Dim razonSocial As String
        'Dim importePago As Integer
        'Dim cuentaDebito As String
        'Dim cuentaPago As String
        'Dim formaPago As Integer
        'Dim chequeMarcaRegistracion As String
        'Dim fechaPago As Date
        'Dim fechaPagoDiferido As String
        'Dim firma1 As String
        'Dim firma2 As String
        ' ''''''''''''''''
        'Dim sql As String
        'sql = "SELECT [tdd_bnf_id],[bnf_numdoc],[suc_entrega],[opg_idopgcli],[opg_ordenalt],[opg_imp_pago],[cta_cuentadebito],[opg_cuentapago],[mpg_id],[opg_mar_regchq],[opg_fec_pago],[opg_fec_pagodiferido],[usr_firma1],[usr_firma2],[usr_firma3] FROM [dbo].[AAOrdenPago2] WHERE [opg_fec_pago] between '" & dateDesde.Text & "' and '" & dateHasta.Text & "'"
        'Using conn As New SqlConnection(CONNSTR)
        '    Using cmd4 As SqlCommand = conn.CreateCommand()
        '        conn.Open()
        '        cmd4.CommandText = sql
        '        Dim reader As SqlDataReader = Nothing
        '        reader = cmd4.ExecuteReader
        '        While reader.Read()
        '            tipoDoc = reader(0)
        '            nroDoc = reader(1)
        '            sucursalEntrega = reader(2)
        '            ordenPago = reader(3).ToString
        '            razonSocial = reader(4).ToString
        '            importePago = reader(5).ToString
        '            cuentaDebito = reader(6).ToString

        '            cuentaPago = reader(7).ToString
        '            formaPago = reader(8).ToString
        '            chequeMarcaRegistracion = reader(9)
        '            fechaPago = reader(10)
        '            If (chkEstadoPagoDiferido.Checked) Then
        '                fechaPagoDiferido = reader(11)
        '            Else
        '                fechaPagoDiferido = ""
        '            End If
        '            firma1 = reader(12)
        '            firma2 = reader(13)
        '            ' MessageBox.Show(firma2)
        '            exportar(tipoDoc, nroDoc, sucursalEntrega, ordenPago, razonSocial, importePago, cuentaDebito, cuentaPago, formaPago, chequeMarcaRegistracion, fechaPago, fechaPagoDiferido, firma1, firma2)
        '        End While
        '    End Using
        'End Using
        'MessageBox.Show("EXPORTADO CORRECTAMENTE")
    End Sub
    Public Sub exportar(ByVal tipoDoc As String, ByVal nroDoc As String, ByVal sucursalEntrega As String, ByVal ordenPago As String, ByVal razonSocial As String, ByVal importePago As Double, ByRef cuentaDebito As String, ByVal cuentaPago As String, ByVal formaPago As String, ByVal chequeMarcaRegistracion As String, ByVal fechaPago As Date, ByVal fechaPagoDiferido As String, ByVal firma1 As String, ByVal firma2 As String)
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
            firma2 = firma2.ToString
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
            strStreamWriter.WriteLine(tipoDoc & vbTab & nroDoc & vbTab & sucursalEntrega & vbTab & ordenPago & vbTab & razonSocial & vbTab & importePago & vbTab & cuentaDebito & vbTab & cuentaPago & vbTab & formaPago & vbTab & chequeMarcaRegistracion & vbTab & fechaPago & vbTab & fechaPagoDiferido & vbTab & firma1 & vbTab & firma2)
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

    Private Sub botonCargar_Click(sender As System.Object, e As System.EventArgs) Handles botonCargar.Click
        cargarOPG()
    End Sub
    Public Sub cargarOPG()
        If (noHayError()) Then
            Dim fechaAlta As String = txtFecha.Text
            Dim tipoDocumento As String = txtTipoDocumento.Text
            Dim numeroDocumento As String = txtNroDocumento.Text
            Dim sucursalEntrega As String = txtSucEntrega.Text
            Dim ordenPago As String = txtOrdenPago.Text
            Dim razonSocial As String = txtOrdenPagoBis.Text
            Dim importePago As String = txtImportePago.Text
            Dim cuentaDebito As String
            Dim cuentaPago As String
            'If (CheckBox1.Checked) Then
            cuentaDebito = txtCuentaDebito.Text
            'Else
            '   cuentaDebito = ""
            'End If
            'If (chkCuentaPago.Checked) Then
            cuentaPago = txtCuentaPago.Text
            ' Else
            'cuentaPago = ""

            'End If

            Dim modalidadPago As String = ComboFormaPago.SelectedItem.ToString
            Dim marcaRegistracionCheque As String = ComboCheque.Text
            Dim fechaPago As Date = DateFechaPago.Text
            Dim fechaPagoDiferido As Date = DateFechaPagoDiferido.Text
            Dim firma As String = txtFirmante1.Text
            'Dim firma2 As String = txtFirmante2.Text
            'MessageBox.Show(firma2)'
            Using conn As New SqlConnection(CONNSTR)
                Using cmd As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    'consulta sql que inserta una nueva tupla en la tabla AAOrdenNro
                    'con los nuevos datos nro,causa de emision en la ventana, un string viatico Cargo
                    'origen y la fecha actual.
                    cmd.CommandText = "INSERT INTO [dbo].[AAOrdenPago2] ([tdd_bnf_id]" +
                    ",[bnf_numdoc]" +
                    ",[suc_entrega]" +
                    ",[opg_idopgcli]" +
                    ",[opg_ordenalt]" +
                     ",[opg_imp_pago]" +
                    ",[cta_cuentadebito]" +
                    ",[opg_cuentapago]" +
                    ",[mpg_id]" +
                    ",[opg_mar_regchq]" +
                    ",[opg_fec_pago]" +
                    ",[opg_fec_pagodiferido]" +
                    ",[usr_firma1]" +
                    ",[usr_firma2]" +
                    ",[usr_firma3]" +
                      ")VALUES(" +
                    "@tdd_bnf_id" +
                    ",@bnf_numdoc" +
                    ",@suc_entrega" +
                    ",@opg_idopgcli" +
                    ",@opg_ordenalt" +
                    ",@opg_imp_pago" +
                    ",@cta_cuentadebito" +
                    ",@opg_cuentapago" +
                    ",@mpg_id" +
                    ",@opg_mar_regchq" +
                    ",@opg_fec_pago" +
                    ",@opg_fec_pagodiferido" +
                    ",@usr_firma1" +
                    ",@usr_firma2" +
                    ",@usr_firma3" +
                         ")"
                    cmd.Parameters.Add("@tdd_bnf_id", SqlDbType.VarChar, 50).Value = tipoDocumento
                    cmd.Parameters.Add("@bnf_numdoc", SqlDbType.VarChar, 50).Value = numeroDocumento
                    cmd.Parameters.Add("@suc_entrega", SqlDbType.VarChar, 50).Value = sucursalEntrega
                    cmd.Parameters.Add("@opg_idopgcli", SqlDbType.VarChar, 30).Value = ordenPago
                    cmd.Parameters.Add("@opg_ordenalt", SqlDbType.VarChar, 40).Value = razonSocial
                    cmd.Parameters.Add("@opg_imp_pago", SqlDbType.VarChar, 50).Value = importePago
                    cmd.Parameters.Add("@cta_cuentadebito", SqlDbType.VarChar, 50).Value = cuentaDebito
                    cmd.Parameters.Add("@opg_cuentapago", SqlDbType.VarChar, 50).Value = cuentaPago
                    cmd.Parameters.Add("@mpg_id", SqlDbType.VarChar, 50).Value = modalidadPago
                    cmd.Parameters.Add("@opg_mar_regchq", SqlDbType.VarChar, 50).Value = marcaRegistracionCheque
                    cmd.Parameters.Add("@opg_fec_pago", SqlDbType.Date).Value = fechaPago
                    cmd.Parameters.Add("@opg_fec_pagodiferido", SqlDbType.Date).Value = fechaPagoDiferido
                    cmd.Parameters.Add("@usr_firma1", SqlDbType.VarChar, 10).Value = firma
                    cmd.Parameters.Add("@usr_firma2", SqlDbType.VarChar, 10).Value = ""
                    cmd.Parameters.Add("@usr_firma3", SqlDbType.VarChar, 10).Value = ""

                    cmd.ExecuteScalar()
                End Using
            End Using
            MessageBox.Show("se guardo de manera correcta la orden")
            Dim sql As String
            sql = "SELECT max(id) FROM [dbo].[AAOrdenPago2]  "
            'declara una variable booleana
            'se conecta con la base de datos y ejecuta la consulta
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql
                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    While reader.Read()
                        MessageBox.Show("El numero de transaccion es " & reader(0))
                    End While
                End Using
            End Using
        Else
            MessageBox.Show("Hay datos necesarios que nos se cargaron verifique las cajas de texto")
        End If
    End Sub
    Private Function noHayError() As Boolean
        Dim ban As Boolean = False
        If ((txtTipoDocumento.Text <> "") And (txtNroDocumento.Text <> "") And (txtSucEntrega.Text <> "") And (txtOrdenPago.Text <> "") And (txtImportePago.Text <> "") And (txtCuentaPago.Text <> "") And (ComboFormaPago.Text <> "")) Then
            ban = True
        Else
            ban = False
        End If
        Return ban
    End Function

    Private Sub OPG_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'iNICIO DEL LOAD
        cargarComboRazonSocial()
        cargarComboModoPago()
        'txtCuentaDebito.Enabled = False
        'txtCuentaPago.Enabled = False
        txtSecuencia.Text = "1"
        cargarComboCuentas()
        ComboCheque.Enabled = False
        identificarUsuarios()
        'txtFirmante1.Enabled = True
        'txtFirmante2.Visible = False
        LabelFirmaPor.Visible = False
    End Sub
    Public Sub identificarUsuarios()
        If (laberTIpo.Text.Equals("CONTROL1")) Then
            txtFirmante1.Enabled = False
            txtFirmante2.Enabled = False

        End If
        If (laberTIpo.Text.Equals("CONTROL2")) Then
            txtFirmante1.Enabled = True
            txtFirmante2.Enabled = False
        End If
        If (laberTIpo.Text.Equals("CONTROL3")) Then
            txtFirmante1.Enabled = False

        End If
        
    End Sub
    Public Sub buscarCantidadFirmantes()
        Dim sql As String
        sql = "SELECT [opg_idopgcli],[usr_firma1],[usr_firma2],[usr_firma3]  FROM [SBDASIPT].[dbo].[AAOrdenPago2] "
        'declara una variable booleana
        'se conecta con la base de datos y ejecuta la consulta
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    Dim fila(3) As Object
                    ' If()
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    fila(2) = reader(2)
                End While
            End Using
        End Using
    End Sub
    Public Sub cargarComboCuentas()
        Dim sql As String
        sql = "Select descripcion from [SBDASIPT].[dbo].[AACuentas] "
        'declara una variable booleana
        'se conecta con la base de datos y ejecuta la consulta
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    Dim fila(3) As Object
                    fila(0) = reader(0)
                    txtCuentaDebito.Items.Add(fila(0))
                End While
            End Using
        End Using
    End Sub
    Public Sub cargarComboModoPago()
        'Dim sql As String
        'sql = "Select tcp_Cod,tcp_Desc from [manager].[dbo].[TipoCobPag] "
        ''declara una variable booleana
        'se conecta con la base de datos y ejecuta la consulta
        'Using conn As New SqlConnection(CONNSTR)
        ' Using cmd4 As SqlCommand = conn.CreateCommand()
        'conn.Open()
        'cmd4.CommandText = Sql
        'Dim reader As SqlDataReader = Nothing
        'reader = cmd4.ExecuteReader
        'While reader.Read()
        'Dim fila(3) As Object
        'fila(0) = reader(0)
        ComboFormaPago.Items.Add(2)
        ComboFormaPago.Items.Add(4)
        'End While
        'End Using
        'End Using
    End Sub
    Public Sub cargarComboRazonSocial()
        Dim sql As String
        sql = "SELECT [pro_RazSoc] FROM [dbo].[Proveed]"
        'declara una variable booleana
        'se conecta con la base de datos y ejecuta la consulta
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    Dim fila(3) As Object
                    fila(0) = reader(0)
                    ComboRazonSocial.Items.Add(fila(0))
                End While
            End Using
        End Using
    End Sub

    Private Sub GroupBox1_Enter(sender As System.Object, e As System.EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub txtTipoDocumento_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtTipoDocumento.KeyPress
        
    End Sub

    Public Sub buscarDescripcion(ByRef tipoDoc As Integer)
        Dim sql As String
        sql = "Select tdc_Cod,tdc_Desc from [manager].[dbo].[TipoDocum] where tdc_Cod=" & tipoDoc & ""
        'declara una variable booleana
        'se conecta con la base de datos y ejecuta la consulta
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    Dim fila(3) As Object
                    labelTipoDocumento.Text = reader(1)
                End While
            End Using
        End Using

    End Sub
    Private Sub txtTipoDocumento_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtTipoDocumento.TextChanged
        Dim tipoDoc As Integer = CInt(txtTipoDocumento.Text)
        buscarDescripcion(tipoDoc)
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        cargarDataGrid()
        Dim con As Integer = CInt(txtSecuencia.Text)
        con = con + 1
        txtSecuencia.Text = CStr(con)
    End Sub
    Public Sub cargarDataGrid()
        If (txtOrdenPago2.Text.Equals("") Or txtTipo.Text.Equals("") Or txtZona.Text.Equals("") Or txtSecuencia.Text.Equals("") Or txtTexto.Text.Equals("") Or txtFirma2.Text.Equals("")) Then
            MessageBox.Show("EXISTEN VALORES QUE NO FUERON INGRESADOS A SUS RESPECTIVOS CAMPOS ")
        Else

            ''''''''''
            Dim ordenPago As String = txtOrdenPago2.Text
            Dim tipo As Integer = CInt(txtTipo.Text)
            Dim zona As Integer = CInt(txtZona.Text)
            Dim secuencia As Integer = CInt(txtSecuencia.Text)
            Dim texto As String = txtTexto.Text
            Dim firma As String = txtFirma2.Text
            guardar(ordenPago, tipo, zona, secuencia, texto, firma)

            Dim fila(10) As Object
            fila(0) = ordenPago
            fila(1) = tipo
            fila(2) = zona
            fila(3) = secuencia
            fila(4) = texto
            fila(5) = firma
            DataGridView1.Rows.Add(fila)
        End If
    End Sub
    Public Sub guardar(ByVal ordenPago As String, ByVal tipo As Integer, ByVal zona As Integer, ByVal secuencia As Integer, ByVal texto As String, ByVal firma As String)
        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()
                conn.Open()
                'consulta sql que inserta una nueva tupla en la tabla AAOrdenNro
                'con los nuevos datos nro,causa de emision en la ventana, un string viatico Cargo
                'origen y la fecha actual.
                cmd.CommandText = "INSERT INTO [dbo].[AARetenciones]" +
           "([opg_idopgcli]" +
           ",[tipo_id]" +
           ",[zona_id]" +
           ",[secuencia_id]" +
           ",[texto]" +
           ",[usr_id])" +
                "VALUES(" +
                "@opg_idopgcli" +
                ",@tipo_id" +
                ",@zona_id" +
                ",@secuencia_id" +
                ",@texto" +
                ",@usr_id" +
                  ")"
                cmd.Parameters.Add("@opg_idopgcli", SqlDbType.VarChar, 30).Value = ordenPago
                cmd.Parameters.Add("@tipo_id", SqlDbType.Int).Value = tipo
                cmd.Parameters.Add("@zona_id", SqlDbType.Int).Value = zona
                cmd.Parameters.Add("@secuencia_id", SqlDbType.Int).Value = secuencia
                cmd.Parameters.Add("@texto", SqlDbType.VarChar, 50).Value = texto
                cmd.Parameters.Add("@usr_id", SqlDbType.VarChar, 10).Value = firma
                cmd.ExecuteScalar()
            End Using
        End Using
        MessageBox.Show("se guardo de manera correcta la orden")
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs)

        buscarExportar()

    End Sub
    Public Sub buscarExportar()
        'Dim sql As String
        'sql = "select opg_idopgcli from [SBDASIPT].[dbo].[AAOrdenPago2] where opg_fec_pago between '" & dateDesde.Text & "' and '" & dateHasta.Text & "'"
        'Using conn As New SqlConnection(CONNSTR)
        '    Using cmd4 As SqlCommand = conn.CreateCommand()
        '        conn.Open()
        '        cmd4.CommandText = sql
        '        Dim reader As SqlDataReader = Nothing
        '        reader = cmd4.ExecuteReader
        '        'MessageBox.Show("probando")
        '        While reader.Read()
        '            MessageBox.Show(reader(0))
        '            exportar2(reader(0))
        '        End While
        '    End Using
        'End Using


    End Sub
    Public Sub exportar2(ByVal ordenPago As String)
        Dim sql As String
        sql = "SELECT [opg_idopgcli],[tipo_id],[zona_id],[secuencia_id] ,[texto],[usr_id] FROM [SBDASIPT].[dbo].[AARetenciones] where [opg_idopgcli]= '" & ordenPago & "'"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    exportar3(reader(0), reader(1), reader(2), reader(3), reader(4), reader(5))
                End While
            End Using
        End Using
        MessageBox.Show("EXPORTADO CORRECTAMENTE")
    End Sub
    Public Sub exportar3(ByVal opg_idopgcli As String, ByVal tipo_id As Integer, ByVal zona_id As Integer, ByVal secuencia_id As Integer, ByVal texto As String, ByVal usr_id As String)
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
            PathArchivo = "C:\Exportaciones\RTN.txt" ' Se determina el nombre del archivo con la fecha actual

            ''verificamos si existe el archivo

            If File.Exists(PathArchivo) Then
                strStreamW = File.Open(PathArchivo, FileMode.Append) 'Abrimos el archivo
            Else
                strStreamW = File.Create(PathArchivo) ' lo creamos
            End If

            strStreamWriter = New StreamWriter(strStreamW, System.Text.Encoding.Default) ' tipo de codificacion para escritura

            ''asignamos las varibles
            opg_idopgcli = opg_idopgcli.ToString
            tipo_id = tipo_id.ToString
            zona_id = zona_id.ToString
            secuencia_id = secuencia_id.ToString
            texto = texto.ToString.PadRight(150)
            usr_id = usr_id.ToString

            strStreamWriter.WriteLine(opg_idopgcli & vbTab & tipo_id & vbTab & zona_id & vbTab & secuencia_id & vbTab & texto & vbTab & usr_id)
            'Dim cierre As String
            'cierre = "----------------------------------------------------------------------"
            'strStreamWriter.WriteLine(cierre)
            strStreamWriter.Close() ' cerramos

        Catch ex As Exception
            MsgBox("Error al Guardar la informacion en el archivo. " & ex.ToString, MsgBoxStyle.Critical, Application.ProductName)
            strStreamWriter.Close() ' cerramos
        End Try
    End Sub

    Private Sub ComboFormaPago_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboFormaPago.SelectedIndexChanged
        'Dim sql As String
        'sql = "Select tcp_Cod,tcp_Desc from [manager].[dbo].[TipoCobPag] where tcp_Cod='" & ComboFormaPago.Text & "'"
        'declara una variable booleana
        'se conecta con la base de datos y ejecuta la consulta
        'Using conn As New SqlConnection(CONNSTR)
        'Using cmd4 As SqlCommand = conn.CreateCommand()
        'conn.Open()
        'cmd4.CommandText = Sql
        'Dim reader As SqlDataReader = Nothing
        'reader = cmd4.ExecuteReader
        'While reader.Read()
        'Dim fila(3) As Object
        If (ComboFormaPago.SelectedItem.ToString.Equals("2")) Then
            Label28.Text = "Transferencia Bancaria"
        End If
        If (ComboFormaPago.SelectedItem.ToString.Equals("4")) Then
            Label28.Text = "Cheque"
        End If

        'End While
        'End Using
        'End Using
    End Sub

    Private Sub ComboRazonSocial_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboRazonSocial.SelectedIndexChanged
        cargarDatosBeneficiario()
        txtOrdenPagoBis.Text = ComboRazonSocial.Text
    End Sub
    Public Sub cargarDatosBeneficiario()
        Dim sql As String
        sql = "SELECT [pro_RazSoc],[protdc_Cod],[pro_CUIT] FROM [dbo].[Proveed] where [pro_RazSoc]='" & ComboRazonSocial.Text & "'"
        'declara una variable booleana
        'se conecta con la base de datos y ejecuta la consulta
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    Dim fila(3) As Object
                    txtTipoDocumento.Text = reader(1)
                    txtNroDocumento.Text = reader(2)
                End While
            End Using
        End Using
        buscarCbu(txtNroDocumento.Text)
    End Sub
    Public Sub buscarCbu(ByVal dni As String)
        Dim sql As String
        sql = "Dim sql As String"
        sql = "SELECT [cbu]  FROM [dbo].[AACbu3] where dni= '" & dni & "' "
        'declara una variable booleana
        'se conecta con la base de datos y ejecuta la consulta
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    Dim fila(3) As Object
                    txtCuentaPago.Text = reader(0)

                End While
            End Using
        End Using

    End Sub

    Private Sub txtOrdenPago2_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtOrdenPago2.TextChanged

    End Sub

    Private Sub txtTexto_Resize(sender As Object, e As System.EventArgs) Handles txtTexto.Resize

    End Sub

    Private Sub txtTexto_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtTexto.TextChanged

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs)
        'If (CheckBox1.Checked) Then
        '    txtCuentaDebito.Enabled = True
        'Else
        '    txtCuentaDebito.Enabled = False
        'End If
    End Sub

    Private Sub txtOrdenPago_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtOrdenPago.TextChanged
        txtOrdenPago2.Text = txtOrdenPago.Text()
    End Sub

    Private Sub chkCuentaPago_CheckedChanged(sender As System.Object, e As System.EventArgs)
        'If (chkCuentaPago.Checked) Then
        '    txtCuentaPago.Enabled = True

        'Else
        '    txtCuentaPago.Enabled = False
        'End If
    End Sub

    Private Sub laberTIpo_Click(sender As System.Object, e As System.EventArgs) Handles laberTIpo.Click

    End Sub

    Private Sub Button1_Click_1(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class