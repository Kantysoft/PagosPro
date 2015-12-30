Imports System.Data.SqlClient
'Imports iTextSharp
'Imports iTextSharp.text
'Imports iTextSharp.text.pdf
'Imports iTextSharp.text.Image
'Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet

Public Class ExportarRangoFecha

    Private Sub ExportarRangoFecha_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        buscarExportar()
    End Sub
    Public Sub buscarExportar()
        Dim sql As String
        sql = "select opg_idopgcli from [SBDASIPT].[dbo].[AAOrdenPago2] where opg_fec_pago between '" & dateDesde.Text & "' and '" & dateHasta.Text & "'"
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

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        buscarExportar()
    End Sub
    Public Sub comprobacionExportarcion2()
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
        Dim firma2 As String
        ''''''''''''''''
        Dim sql As String
        sql = "SELECT [tdd_bnf_id],[bnf_numdoc],[suc_entrega],[opg_idopgcli],[opg_ordenalt],[opg_imp_pago],[cta_cuentadebito],[opg_cuentapago],[mpg_id],[opg_mar_regchq],[opg_fec_pago],[opg_fec_pagodiferido],[usr_firma1],[usr_firma2],[usr_firma3] FROM [dbo].[AAOrdenPago2] WHERE [opg_fec_pago] between '" & dateDesde.Text & "' and '" & dateHasta.Text & "'"
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
                    fechaPagoDiferido = reader(11)
                    'Else
                    fechaPagoDiferido = ""
                    'End If
                    firma1 = reader(12)
                    firma2 = reader(13)
                    ' MessageBox.Show(firma2)
                    'exportar(tipoDoc, nroDoc, sucursalEntrega, ordenPago, razonSocial, importePago, cuentaDebito, cuentaPago, formaPago, chequeMarcaRegistracion, fechaPago, fechaPagoDiferido, firma1, firma2)
                End While
            End Using
        End Using
        MessageBox.Show("EXPORTADO CORRECTAMENTE")
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
            ' For Each row As DataGridViewRow In DataGridView1.Rows
            'nroIngreso = "1111"
            'nroIngreso = row.Cells(0).Value.ToString
            'concepto = row.Cells(1).Value.ToString.PadRight(20)
            'fecha = row.Cells(2).Value.ToString.PadRight(20)
            'MessageBox.Show("nroIngreso" & nroIngreso)
            'strStreamWriter.WriteLine(nroIngreso)
            'Next
            'strStreamWriter.WriteLine(cierre)
            strStreamWriter.Close() ' cerramos


        Catch ex As Exception
            MsgBox("Error al Guardar la informacion en el archivo. " & ex.ToString, MsgBoxStyle.Critical, Application.ProductName)
            strStreamWriter.Close() ' cerramos
        End Try

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
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
        Dim firma2 As String
        Dim firma3 As String
        ''''''''''''''''
        Dim sql As String
        sql = "SELECT [tdd_bnf_id],[bnf_numdoc],[suc_entrega],[opg_idopgcli],[opg_ordenalt],[opg_imp_pago],[cta_cuentadebito],[opg_cuentapago],[mpg_id],[opg_mar_regchq],[opg_fec_pago],[opg_fec_pagodiferido],[usr_firma1],[usr_firma2],[usr_firma3] FROM [dbo].[AAOrdenPago2] WHERE [opg_fec_pago] between '" & dateDesde.Text & "' and '" & dateHasta.Text & "'"
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
                    fechaPagoDiferido = reader(11)
                    'Else
                    fechaPagoDiferido = ""

                    'End If
                    firma1 = reader(12)
                    firma2 = reader(13)
                    firma3 = ""
                    ' MessageBox.Show(firma2)
                    exportar(tipoDoc, nroDoc, sucursalEntrega, ordenPago, razonSocial, importePago, cuentaDebito, cuentaPago, formaPago, chequeMarcaRegistracion, fechaPago, fechaPagoDiferido, firma1, firma2, firma3)
                End While
            End Using
        End Using
        MessageBox.Show("EXPORTADO CORRECTAMENTE")
    End Sub
    Public Sub exportar(ByVal tipoDoc As String, ByVal nroDoc As String, ByVal sucursalEntrega As String, ByVal ordenPago As String, ByVal razonSocial As String, ByVal importePago As Double, ByRef cuentaDebito As String, ByVal cuentaPago As String, ByVal formaPago As String, ByVal chequeMarcaRegistracion As String, ByVal fechaPago As Date, ByVal fechaPagoDiferido As String, ByVal firma1 As String, ByVal firma2 As String, ByVal firma3 As String)
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
            strStreamWriter.WriteLine(tipoDoc & vbTab & nroDoc & vbTab & sucursalEntrega & vbTab & ordenPago & vbTab & razonSocial & vbTab & importePago & vbTab & cuentaDebito & vbTab & cuentaPago & vbTab & formaPago & vbTab & chequeMarcaRegistracion & vbTab & fechaPago & vbTab & fechaPagoDiferido & vbTab & firma1 & vbTab & firma2 & vbTab & firma3)
            'Dim cierre As String
            'cierre = "----------------------------------------------------------------------"
            '' strStreamWriter.WriteLine("Primera linea en un archivo txt desde visual basic.Net")
            'strStreamWriter.WriteLine(nroPedido & tipoPedido & formatoEnvio & transportista & vin)
            'strStreamWriter.WriteLine(cierre)
            'strStreamWriter.WriteLine(cuenta & banco)

            'strStreamWriter.WriteLine(cierre)
            strStreamWriter.Close() ' cerramos


        Catch ex As Exception
            MsgBox("Error al Guardar la informacion en el archivo. " & ex.ToString, MsgBoxStyle.Critical, Application.ProductName)
            strStreamWriter.Close() ' cerramos
        End Try

    End Sub
End Class