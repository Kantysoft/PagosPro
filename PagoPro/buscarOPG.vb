Imports System.Data.SqlClient
Imports System.IO
Imports NuGet
Public Class buscarOPG
    Dim sql As String
    Private Sub buscarOPG_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        cargarDeNuevo()
        cargarComboRazonSocial()
        cargarComboCuentas()
        cargarComboModoPago()
        'txtFirmante1.Enabled = False
        'txtFirmante2.Enabled = False
        identificarUsuarios()
        If (txtFirmante1.Text.Equals(labelUsuario.Text) And Not txtFirmante1.Text.Equals("")) Then
            txtFirmante2.Enabled = False
        End If
    End Sub
    Private Sub cargarDeNuevo()
        cargarComboRazonSocial()
        cargarComboCuentas()
        cargarComboModoPago()
        'txtFirmante1.Enabled = False
        'txtFirmante2.Enabled = False
        identificarUsuarios()
        If (txtFirmante1.Text.Equals(labelUsuario.Text) And Not txtFirmante1.Text.Equals("")) Then
            txtFirmante2.Enabled = False
        End If
    End Sub
    Public Sub identificarUsuarios()
        If (laberTIpo.Text.Equals("CONTROL1")) Then
            txtFirmante1.Enabled = False
            txtFirmante2.Enabled = False
        End If
        If (laberTIpo.Text.Equals("CONTROL2")) Then
            buscarCantidadFirmantes()
            'txtFirmante2.Enabled = False
        End If
        If (laberTIpo.Text.Equals("CONTROL3")) Then
            txtFirmante1.Enabled = False
        End If
        If (buscarFirmas()) Then
            btnExportar.Enabled = True
            GroupBox2.Visible = True
        Else
            btnExportar.Enabled = False
            GroupBox2.Visible = False
        End If
    End Sub
    Public Sub buscarCantidadFirmantes()

        Dim sql As String
        sql = "SELECT [opg_idopgcli],[usr_firma1],[usr_firma2],[usr_firma3]  FROM [SBDASIPT].[dbo].[AAOrdenPago2] where opg_idopgcli='" & txtOrdenPago.Text & "'"
        'declara una variable booleana
        'se conecta con la base de datos y ejecuta la consulta
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    If (Not reader(1).Equals("")) Then
                        txtFirmante1.Enabled = False

                    Else
                        txtFirmante1.Enabled = True
                    End If
                    If (Not reader(2).Equals("")) Then
                        txtFirmante2.Enabled = False

                    Else
                        txtFirmante2.Enabled = True
                    End If

                    If (reader(1).Equals("") And reader(2).Equals("")) Then
                        txtFirmante2.Enabled = False
                    End If
                End While
            End Using
        End Using
    End Sub
    Function buscarFirmas() As Boolean
        Dim ban As Boolean = False
        sql = "SELECT [usr_firma1],[usr_firma2] FROM [dbo].[AAOrdenPago2] where [id]=" & LabelTransaccion.Text & ""
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                If reader.Read() Then
                    If (Not reader(0).Equals("") And Not reader(1).Equals("")) Then
                        ban = True
                    End If
                End If
            End Using
        End Using
        Return ban
    End Function
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
    Public Sub cargarComboModoPago()
        Dim sql As String
        sql = "Select tcp_Cod,tcp_Desc from [manager].[dbo].[TipoCobPag] "
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
                    ComboFormaPago.Items.Add(fila(0))
                End While
            End Using
        End Using
    End Sub

    Private Sub ComboFormaPago_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboFormaPago.SelectedIndexChanged
        Dim sql As String
        sql = "Select tcp_Cod,tcp_Desc from [manager].[dbo].[TipoCobPag] where tcp_Cod='" & ComboFormaPago.Text & "'"
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
                    Label28.Text = reader(1)
                End While
            End Using
        End Using
    End Sub

    Private Sub btnSalir_Click(sender As System.Object, e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub txtFirmante2_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtFirmante2.KeyPress
        If (txtFirmante2.Text.Equals(labelUsuario.Text)) Then
            btnExportar.Visible = True
        End If
    End Sub

    Private Sub txtFirmante2_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtFirmante2.TextChanged
        If (txtFirmante2.Text.Equals(labelUsuario.Text)) Then
            btnExportar.Visible = True
        End If
    End Sub

    Private Sub botonGuadar_Click(sender As System.Object, e As System.EventArgs) Handles botonGuadar.Click
        actualizarOPG()
        cargarDeNuevo()
        Me.Refresh()
    End Sub
    Public Sub actualizarOPG()
        'sql = "UPDATE [dbo].[AAOrdenPago2]  SET [tdd_bnf_id] = '" & txtTipoDocumento.Text & "' ,[bnf_numdoc] = '" & txtNroDocumento.Text & "',[suc_entrega] = '" & txtSucEntrega.Text & "',[opg_idopgcli] = '" & txtOrdenPago.Text & "',[opg_ordenalt] = '" & ComboRazonSocial.Text & "',[opg_imp_pago] = " & txtImportePago.Text & ",[cta_cuentadebito] = '" & txtCuentaDebito.Text & "',[opg_cuentapago] = '" & txtCuentaPago.Text & "',[mpg_id] = '" & ComboFormaPago.Text & "',[opg_mar_regchq] = '' ,[opg_fec_pago] = '" & txtFecha.Text & "',[opg_fec_pagodiferido] = '',[usr_firma1] = '" & txtFirmante1.Text & "',[usr_firma2] = '" & txtFirmante2.Text & "',[usr_firma3] = '' WHERE [id]='" & LabelTransaccion.Text & "'"
        If (controlAnterior()) Then

            sql = "UPDATE [dbo].[AAOrdenPago2]  SET [opg_imp_pago]='" & txtImportePago.Text & "',[tdd_bnf_id] = '" & txtTipoDocumento.Text & "',[bnf_numdoc] = '" & txtNroDocumento.Text & "',[suc_entrega] = '" & txtSucEntrega.Text & "',[opg_idopgcli] = '" & txtOrdenPago.Text & "',[opg_ordenalt] = '" & ComboRazonSocial.Text & "',[opg_cuentapago] = '" & txtCuentaPago.Text & "',[mpg_id] = '" & ComboFormaPago.Text & "',[opg_mar_regchq] = '' ,[opg_fec_pago] = '" & txtFecha.Text & "',[opg_fec_pagodiferido] = '',[usr_firma1] = '" & txtFirmante1.Text & "',[usr_firma2] = '" & txtFirmante2.Text & "',[usr_firma3] = '' WHERE [id]='" & LabelTransaccion.Text & "'"
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql
                    cmd4.ExecuteScalar()
                End Using
            End Using
            MessageBox.Show("LA ORDEN DE PAGO SE ACTUALIZO CORRECTAMENTE")
        Else
            MessageBox.Show("ESTE USUARIO YA FIRMO")
        End If
    End Sub
    Private Function controlAnterior() As Boolean
        Dim sql As String
        Dim ban As Boolean
        ban = True
        sql = "SELECT [opg_idopgcli],[usr_firma1],[usr_firma2],[usr_firma3]  FROM [SBDASIPT].[dbo].[AAOrdenPago2] where opg_idopgcli='" & txtOrdenPago.Text & "'"
        'declara una variable booleana
        'se conecta con la base de datos y ejecuta la consulta
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    If (Not txtFirmante2.Text.Equals("")) Then
                        If (reader(1).Equals(txtFirmante2.Text)) Then
                            ban = False
                        End If
                    End If
                End While
            End Using
        End Using
        Return ban
    End Function
    Private Sub btnExportar_Click(sender As System.Object, e As System.EventArgs) Handles btnExportar.Click
        comprobacionExportarcion()
        exportarRetencion()
    End Sub
    Public Sub comprobacionExportarcion()
        Dim tipoDoc As String
        Dim nroDoc As String
        Dim sucursalEntrega As String
        Dim ordenPago As String
        Dim razonSocial As String
        Dim importePago As String
        Dim cuentaDebito As String
        Dim cuentaPago As String
        Dim formaPago As Integer
        Dim chequeMarcaRegistracion As String
        Dim fechaPago As Date
        Dim fechaPagoDiferido As String
        Dim firma1 As String
        Dim firma2 As String
        Dim firma3 As String
        ''''''''''''''''
        'For Each row As DataGridViewRow In DataGridView2.Rows
        Dim sql As String
        'Dim prueba As String = row.Cells("DataGridViewTextBoxColumn16").Value
        sql = "SELECT [tdd_bnf_id],[bnf_numdoc],[suc_entrega],[opg_idopgcli],[opg_ordenalt],[opg_imp_pago],[cta_cuentadebito],[opg_cuentapago],[mpg_id],[opg_mar_regchq],[opg_fec_pago],[opg_fec_pagodiferido],[usr_firma1],[usr_firma2],[usr_firma3] FROM [dbo].[AAOrdenPago2] WHERE [id]= '" & LabelTransaccion.Text & "' "
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
                    firma2 = reader(13)
                    firma3 = ""
                    ''''''''''''''''''''''''
                    exportar(tipoDoc, nroDoc, sucursalEntrega, ordenPago, razonSocial, importePago, cuentaDebito, cuentaPago, formaPago, chequeMarcaRegistracion, fechaPago, fechaPagoDiferido, firma1, firma2, firma3)
                End While
            End Using
        End Using
        'Next
        MessageBox.Show("EXPORTADO CORRECTAMENTE")
    End Sub
    Public Sub exportar(ByVal tipoDoc As String, ByVal nroDoc As String, ByVal sucursalEntrega As String, ByVal ordenPago As String, ByVal razonSocial As String, ByVal importePago As String, ByRef cuentaDebito As String, ByVal cuentaPago As String, ByVal formaPago As String, ByVal chequeMarcaRegistracion As String, ByVal fechaPago As Date, ByVal fechaPagoDiferido As String, ByVal firma1 As String, ByVal firma2 As String, ByVal firma3 As String)
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
            If Directory.Exists("Z:\Sistema de impresion\Exportaciones") = False Then 'si no existe la carpeta se crea
                Directory.CreateDirectory("Z:\Sistema de impresion\Exportaciones")
            End If
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            'If chkCTES.Checked Then
            PathArchivo = "Z:\Sistema de impresion\Exportaciones\OPG-" & txtOrdenPago.Text & ".txt" ' Se determina el nombre del archivo con la fecha actual

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
            importePago = importePago.ToString
            cuentaDebito = cuentaDebito.ToString
            cuentaPago = cuentaPago.ToString

            formaPago = formaPago.ToString
            chequeMarcaRegistracion = chequeMarcaRegistracion.ToString
            fechaPago = fechaPago.ToString
            fechaPagoDiferido = fechaPagoDiferido.ToString
            firma1 = firma1.ToString
            firma2 = firma2.ToString
            strStreamWriter.WriteLine(tipoDoc & vbTab & nroDoc & vbTab & sucursalEntrega & vbTab & ordenPago & vbTab & razonSocial & vbTab & importePago & vbTab & cuentaDebito & vbTab & cuentaPago & vbTab & formaPago & vbTab & chequeMarcaRegistracion & vbTab & fechaPago & vbTab & fechaPagoDiferido & vbTab & firma1 & vbTab & firma2 & vbTab & firma3)
            strStreamWriter.Close() ' cerramos
        Catch ex As Exception
            MsgBox("Error al Guardar la informacion en el archivo. " & ex.ToString, MsgBoxStyle.Critical, Application.ProductName)
            strStreamWriter.Close() ' cerramos
        End Try

    End Sub
    Private Sub exportarRetencion()
        Dim sql As String
        sql = "select opg_idopgcli from [SBDASIPT].[dbo].[AAOrdenPago2] where opg_idopgcli='" & txtOrdenPago.Text & "'"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                'MessageBox.Show("probando")
                While reader.Read()
                    MessageBox.Show(reader(0))
                    exportarRetencion2(reader(0))
                End While
            End Using
        End Using

    End Sub
    Public Sub exportarRetencion2(ByVal ordenPago As String)
        Dim sql As String
        sql = "SELECT [opg_idopgcli],[tipo_id],[zona_id],[secuencia_id] ,[texto],[usr_id] FROM [SBDASIPT].[dbo].[AARetenciones] where [opg_idopgcli]= '" & ordenPago & "'"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    exportarRetencion3(reader(0), reader(1), reader(2), reader(3), reader(4), reader(5))
                End While
            End Using
        End Using
        MessageBox.Show("EXPORTADO CORRECTAMENTE")
    End Sub
    Public Sub exportarRetencion3(ByVal opg_idopgcli As String, ByVal tipo_id As Integer, ByVal zona_id As Integer, ByVal secuencia_id As Integer, ByVal texto As String, ByVal usr_id As String)
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
            If Directory.Exists("Z:\Sistema de impresion\Exportaciones") = False Then 'si no existe la carpeta se crea
                Directory.CreateDirectory("Z:\Sistema de impresion\Exportaciones")
            End If

            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            'If chkCTES.Checked Then
            PathArchivo = "Z:\Sistema de impresion\RTN-" & txtOrdenPago.Text & ".txt" ' Se determina el nombre del archivo con la fecha actual

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
    Public Sub exportar2(ByVal tipoDoc As String, ByVal nroDoc As String, ByVal sucursalEntrega As String, ByVal ordenPago As String, ByVal razonSocial As String, ByVal importePago As Double, ByRef cuentaDebito As String, ByVal cuentaPago As String, ByVal formaPago As String, ByVal chequeMarcaRegistracion As String, ByVal fechaPago As Date, ByVal fechaPagoDiferido As String, ByVal firma1 As String, ByVal firma2 As String)
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
            If Directory.Exists("Z:\Sistema de impresion\Exportaciones") = False Then 'si no existe la carpeta se crea
                Directory.CreateDirectory("Z:\Sistema de impresion\Exportaciones")
            End If
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            'If chkCTES.Checked Then
            PathArchivo = "Z:\Sistema de impresion\\OPG-" & txtOrdenPago.Text & ".txt" ' Se determina el nombre del archivo con la fecha actual

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
            strStreamWriter.WriteLine(tipoDoc & vbTab & nroDoc & vbTab & sucursalEntrega & vbTab & ordenPago & vbTab & razonSocial & vbTab & importePago & vbTab & cuentaDebito & vbTab & cuentaPago & vbTab & formaPago & vbTab & chequeMarcaRegistracion & vbTab & fechaPago & vbTab & fechaPagoDiferido & vbTab & firma1 & vbTab & firma2)
            strStreamWriter.Close() ' cerramos
        Catch ex As Exception
            MsgBox("Error al Guardar la informacion en el archivo. " & ex.ToString, MsgBoxStyle.Critical, Application.ProductName)
            strStreamWriter.Close() ' cerramos
        End Try
    End Sub
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        registrar()
    End Sub
    Public Sub registrar()
        sql = "UPDATE [dbo].[AAOrdenPago2]  SET [usr_firma3] = '" & txtRegistrar.Text & "' WHERE [id]=" & LabelTransaccion.Text & ""
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                cmd4.ExecuteScalar()
            End Using
        End Using
        MessageBox.Show("LA ORDEN DE PAGO SE ACTUALIZO CORRECTAMENTE")
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
End Class
