Imports System.Data.SqlClient
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.Image
Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet
Public Class cargaPersonalizadaBeneficiario
    Public cbu2 As String
    Private Sub DataGridView1_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_DoubleClick(sender As Object, e As System.EventArgs) Handles DataGridView1.DoubleClick
        Dim I As Integer = DataGridView1.CurrentCell.RowIndex
        Dim fila(20) As Object
        fila(0) = DataGridView1.Rows(I).Cells(0).Value
        fila(1) = DataGridView1.Rows(I).Cells(1).Value
        fila(2) = DataGridView1.Rows(I).Cells(2).Value
        fila(3) = DataGridView1.Rows(I).Cells(3).Value
        fila(4) = DataGridView1.Rows(I).Cells(4).Value
        fila(5) = DataGridView1.Rows(I).Cells(5).Value
        fila(6) = DataGridView1.Rows(I).Cells(6).Value
        fila(7) = DataGridView1.Rows(I).Cells(7).Value
        fila(8) = DataGridView1.Rows(I).Cells(8).Value
        fila(9) = DataGridView1.Rows(I).Cells(9).Value
        fila(10) = DataGridView1.Rows(I).Cells(10).Value
        fila(11) = DataGridView1.Rows(I).Cells(11).Value
        fila(12) = DataGridView1.Rows(I).Cells(12).Value
        fila(13) = DataGridView1.Rows(I).Cells(13).Value
        DataGridView2.Rows.Add(fila)

    End Sub

    Private Sub cargaPersonalizadaBeneficiario_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        cargarDataGrid()
        GroupBox3.Visible = False
        identificarUsuario()

    End Sub
    Public Sub identificarUsuario()
        If (laberTIpo.Text.Equals("CONTROL2") Or laberTIpo.Text.Equals("CONTROL3")) Then
            '   btnModificar.Enabled = True
        Else
            '  btnModificar.Enabled = False
        End If
    End Sub
    Public Sub cargarDataGrid()
        Dim sql As String
        sql = "SELECT [pro_FecMod],[protdc_Cod],[pro_CUIT],[prosib_Cod],[pro_Piso],[prosiv_Cod],[pro_RazSoc],[pro_Direc],[pro_CodPos],[pro_NroIB],[pro_Cod]  FROM [dbo].[Proveed]"
        DataGridView1.Rows.Clear()
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    Dim fila(13) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    fila(2) = reader(2)
                    fila(3) = reader(3)
                    fila(4) = reader(4)
                    fila(5) = reader(5)
                    fila(6) = reader(6)
                    fila(7) = reader(7)
                    fila(10) = reader(8)
                    fila(11) = reader(9)
                    fila(12) = reader(10)
                    '''''''''''''''''''''''''''''''''''''''
                    ' buscarCbu(CStr(reader(2)))
                    Using conn2 As New SqlConnection(CONNSTR)
                        Using cmd2 As SqlCommand = conn2.CreateCommand()
                            conn2.Open()
                            cmd2.CommandText = "SELECT [cbu] FROM [SBDASIPT].[dbo].[AACbu3] where dni='" & reader(2) & "'"
                            Dim reader2 As SqlDataReader = Nothing
                            reader2 = cmd2.ExecuteReader
                            If reader2.Read() Then
                                fila(13) = reader2(0)
                            Else
                                fila(13) = ""
                            End If
                        End Using
                    End Using


                    'fila(13) = cbu2
                    'MsgBox(buscarCbu(CStr(reader(2))))
                    '''''''''''''''''''''''''''''''''''''''''
                    DataGridView1.Rows.Add(fila)
                End While
            End Using
        End Using
        DataGridView1.Rows(0).Selected = True
    End Sub
    Private Sub buscarCbu(ByVal dni As String)

        'Dim cbu As String
        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd.CommandText = "SELECT [cbu] FROM [SBDASIPT].[dbo].[AACbu3] where dni='" & dni & "'"
                Dim reader As SqlDataReader = Nothing
                reader = cmd.ExecuteReader
                If reader.Read() Then
                    cbu2 = reader(0)
                    MsgBox(cbu2)
                End If
            End Using
        End Using

        'Return cbu
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            GroupBox3.Visible = True
        Else
            GroupBox3.Visible = False

        End If

    End Sub

    Private Sub DataGridView2_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick
       
    End Sub
    Private Function noExisteCbu() As Boolean
        'Dim ban As Boolean = False
        'Using conn As New SqlConnection(CONNSTR)
        '    Using cmd As SqlCommand = conn.CreateCommand()
        '        conn.Open()
        '        cmd.CommandText = "SELECT [ID],[proveedor],[cbu] ,[dni] FROM [dbo].[AACbu3] WHERE [dni]='" & txtDni.Text & "'"
        '        Dim reader As SqlDataReader = Nothing
        '        reader = cmd.ExecuteReader
        '        If reader.Read() Then
        '            ban = True
        '        Else
        '            ban = False
        '        End If
        '    End Using
        'End Using
        'Return ban
    End Function
    Private Sub Cargar_Click(sender As System.Object, e As System.EventArgs) Handles Cargar.Click
        'If (noExisteCbu().Equals(False)) Then
        '    Using conn As New SqlConnection(CONNSTR)
        '        Using cmd As SqlCommand = conn.CreateCommand()
        '            conn.Open()
        '            cmd.CommandText = "INSERT INTO [dbo].[AACbu3] (proveedor,cbu,dni) VALUES ('" & txtNombre.Text & "','" & txtCbu.Text & "','" & txtDni.Text & "')"
        '            cmd.ExecuteScalar()
        '        End Using
        '    End Using
        '    MessageBox.Show("EL CBU DEL BENEFICIARIO FUE CARGADO CORRECTAMENTE ")
        'Else
        '    MessageBox.Show("EL CBU DE ESTE PROVEEDOR YA FUE INGRESADO")
        '    'Select Case MsgBox("¿E?", MsgBoxStyle.YesNo, "AVISO")
        '    '    'en caso de seleccionar un si
        '    '    Case MsgBoxResult.Yes
        '    '        Using conn As New SqlConnection(CONNSTR)
        '    '            Using cmd As SqlCommand = conn.CreateCommand()
        '    '                conn.Open()
        '    '                cmd.CommandText = "INSERT INTO [dbo].[AACbu3] (proveedor,cbu,dni) VALUES ('" & TextBox2.Text & "','" & txtCbu.Text & "','" & TextBox1.Text & "')"
        '    '                cmd.ExecuteScalar()
        '    '            End Using
        '    '        End Using
        '    '        MessageBox.Show("EL CBU DEL BENEFICIARIO FUE CARGADO CORRECTAMENTE ")
        '    '    Case MsgBoxResult.No
        '    '        MessageBox.Show("Accion cancelada por el usuario", "INFORMACION")
        '    'End Select
        'End If
        Dim unModificarCBU = New ModificarCBU()
        unModificarCBU.labelUsuario.Text = Me.labelUsuario.Text
        unModificarCBU.laberTIpo.Text = Me.laberTIpo.Text
        unModificarCBU.Show()
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        If (Not txtNroOrden.Text.Equals("")) Then
            comprobacionExportarcion()
        Else
            MessageBox.Show("DEBE INGRESAR UNA ORDEN DE PAGO PARA LA EXPORTACION")
            DataGridView2.Rows.Clear()
        End If
    End Sub
    Public Sub comprobacionExportarcion()
        Dim tipoDoc As String
        Dim nroDoc As String
        Dim codIBruto As String
        Dim codGan As String
        Dim codIVA As String
        Dim razonSocial As String
        Dim calle As String
        Dim puerta As String
        Dim unidadFuncional As String
        Dim cp As String
        Dim NIBrutos As String
        ''''''''''''''''
        For Each row As DataGridViewRow In DataGridView2.Rows
            Dim sql As String
            Dim prueba As String = row.Cells(2).Value

            sql = "SELECT [pro_FecMod],[protdc_Cod],[pro_CUIT],[prosib_Cod],[prosiv_Cod],[pro_RazSoc],[pro_Direc],[pro_Piso],[pro_CodPos],[pro_NroIB]  FROM [dbo].[Proveed] where  [pro_CUIT]='" & prueba & "'"
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql
                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    While reader.Read()
                        If (Not reader(2).Equals("")) Then
                            tipoDoc = reader(1).ToString
                            nroDoc = reader(2).ToString
                            codIBruto = reader(3).ToString
                            codGan = "1"
                            codIVA = reader(4).ToString
                            razonSocial = reader(5).ToString
                            calle = reader(6).ToString
                            puerta = "1"
                            unidadFuncional = reader(7).ToString
                            If (unidadFuncional.Equals("")) Then
                                unidadFuncional = "1"
                            End If
                            cp = reader(8).ToString
                            NIBrutos = reader(9).ToString
                            exportar(tipoDoc, nroDoc, codIBruto, codGan, codIVA, razonSocial, calle, puerta, unidadFuncional, cp, NIBrutos)
                        End If
                    End While
                End Using
            End Using
            'MessageBox.Show(tipoDoc)
        Next

        MessageBox.Show("EXPORTADO CORRECTAMENTE")
    End Sub
    Public Sub exportar(ByVal tipoDoc As String, ByVal nroDoc As String, ByVal codIBruto As String, ByVal codGan As String, ByRef codIVA As String, ByVal razonSocial As String, ByVal calle As String, ByVal puerta As String, ByVal unidadFuncional As String, ByVal cp As String, ByVal NIBrutos As String)
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
            'If Directory.Exists("Y:\Exportaciones") = False Then 'si no existe la carpeta se crea
            'Directory.CreateDirectory("Y:\Exportaciones")
            'End If

            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            'If chkCTES.Checked Then

            PathArchivo = "Z:\Sistema de impresion\Exportaciones\BNF-" & txtNroOrden.Text & ".txt" ' Se determina el nombre del archivo con la fecha actual
            'PathArchivo = "Y:\Exportaciones\BNF-" & txtNroOrden.Text & ".txt" '
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
                tipoDoc = tipoDoc.ToString()
            End If
            If (tipoDoc.Equals("5")) Then
                tipoDoc = "3"
            Else
                tipoDoc = tipoDoc.ToString()
            End If
            nroDoc = nroDoc.ToString
            codIBruto = codIBruto.ToString
            codGan = codGan.ToString
            codIVA = codIVA.ToString
            razonSocial = razonSocial
            calle = calle
            puerta = puerta.ToString
            'puerta = 15
            unidadFuncional = unidadFuncional.ToString
            'unidadFuncional = 16
            cp = cp.ToString
            NIBrutos = NIBrutos.ToString
            strStreamWriter.WriteLine(tipoDoc & vbTab & nroDoc & vbTab & codIBruto & vbTab & codGan & vbTab & codIVA & vbTab & razonSocial & vbTab & calle & vbTab & puerta & vbTab & unidadFuncional & vbTab & cp & vbTab & NIBrutos)

            For Each row As DataGridViewRow In DataGridView1.Rows
                nroIngreso = "1111"
            Next
            strStreamWriter.Close() ' cerramos
        Catch ex As Exception
            MsgBox("Error al Guardar la informacion en el archivo. " & ex.ToString, MsgBoxStyle.Critical, Application.ProductName)
            strStreamWriter.Close() ' cerramos
        End Try

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub txtNombreProveedor_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtNombreProveedor.TextChanged
        buscarBeneficiarioNombre(txtNombreProveedor.Text)
    End Sub
    Public Sub buscarBeneficiarioNombre(ByVal Nombre As String)
        'where [pro_RazSoc] like '%" & Nombre & "%'"
        Dim sql As String
        sql = "SELECT [pro_FecMod],[protdc_Cod],[pro_CUIT],[prosib_Cod],[pro_Piso],[prosiv_Cod],[pro_RazSoc],[pro_Direc],[pro_CodPos],[pro_NroIB],[pro_Cod]  FROM [dbo].[Proveed] where [pro_RazSoc] like '%" & Nombre & "%'"
        DataGridView1.Rows.Clear()
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    Dim fila(13) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    fila(2) = reader(2)
                    fila(3) = reader(3)
                    fila(4) = reader(4)
                    fila(5) = reader(5)
                    fila(6) = reader(6)
                    fila(7) = reader(7)
                    fila(10) = reader(8)
                    fila(11) = reader(9)
                    fila(12) = reader(10)
                    '''''''''''''''''''''''''''''''''''''''
                    ' buscarCbu(CStr(reader(2)))
                    Using conn2 As New SqlConnection(CONNSTR)
                        Using cmd2 As SqlCommand = conn2.CreateCommand()
                            conn2.Open()
                            cmd2.CommandText = "SELECT [cbu] FROM [SBDASIPT].[dbo].[AACbu3] where dni='" & reader(2) & "'"
                            Dim reader2 As SqlDataReader = Nothing
                            reader2 = cmd2.ExecuteReader
                            If reader2.Read() Then
                                fila(13) = reader2(0)
                            Else
                                fila(13) = ""
                            End If
                        End Using
                    End Using


                    'fila(13) = cbu2
                    'MsgBox(buscarCbu(CStr(reader(2))))
                    '''''''''''''''''''''''''''''''''''''''''
                    DataGridView1.Rows.Add(fila)
                End While
            End Using
        End Using
        DataGridView1.Rows(0).Selected = True
    End Sub

    Private Sub txtBuscarDni_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtBuscarDni.TextChanged
        buscarBeneficiario(txtBuscarDni.Text)
        'where [pro_CUIT] like '%" & nroDocumento & "%'"
    End Sub
    Public Sub buscarBeneficiario(ByVal nroDocumento As String)
        Dim sql As String
        sql = "SELECT [pro_FecMod],[protdc_Cod],[pro_CUIT],[prosib_Cod],[pro_Piso],[prosiv_Cod],[pro_RazSoc],[pro_Direc],[pro_CodPos],[pro_NroIB],[pro_Cod]  FROM [dbo].[Proveed] where [pro_CUIT] like '%" & nroDocumento & "%'"
        DataGridView1.Rows.Clear()
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    Dim fila(13) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    fila(2) = reader(2)
                    fila(3) = reader(3)
                    fila(4) = reader(4)
                    fila(5) = reader(5)
                    fila(6) = reader(6)
                    fila(7) = reader(7)
                    fila(10) = reader(8)
                    fila(11) = reader(9)
                    fila(12) = reader(10)
                    '''''''''''''''''''''''''''''''''''''''
                    ' buscarCbu(CStr(reader(2)))
                    Using conn2 As New SqlConnection(CONNSTR)
                        Using cmd2 As SqlCommand = conn2.CreateCommand()
                            conn2.Open()
                            cmd2.CommandText = "SELECT [cbu] FROM [SBDASIPT].[dbo].[AACbu3] where dni='" & reader(2) & "'"
                            Dim reader2 As SqlDataReader = Nothing
                            reader2 = cmd2.ExecuteReader
                            If reader2.Read() Then
                                fila(13) = reader2(0)
                            Else
                                fila(13) = ""
                            End If
                        End Using
                    End Using


                    'fila(13) = cbu2
                    'MsgBox(buscarCbu(CStr(reader(2))))
                    '''''''''''''''''''''''''''''''''''''''''
                    DataGridView1.Rows.Add(fila)
                End While
            End Using
        End Using
        DataGridView1.Rows(0).Selected = True
    End Sub

    Private Sub DataGridView2_CellContextMenuStripNeeded(sender As Object, e As System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventArgs) Handles DataGridView2.CellContextMenuStripNeeded

    End Sub

    Private Sub DataGridView2_ContextMenuStripChanged(sender As Object, e As System.EventArgs) Handles DataGridView2.ContextMenuStripChanged

    End Sub

    Private Sub DataGridView2_DoubleClick(sender As Object, e As System.EventArgs) Handles DataGridView2.DoubleClick
        'Dim I As Integer = DataGridView1.CurrentCell.RowIndex
        'Dim dni As String
        'Dim RazonSocial As String
        'dni = DataGridView1.Rows(I).Cells(2).Value.ToString
        'RazonSocial = DataGridView1.Rows(I).Cells(6).Value.ToString
        'txtDni.Text = dni
        'txtNombre.Text = RazonSocial
    End Sub

    Private Sub btnModificar_Click(sender As System.Object, e As System.EventArgs)
        modificarCbu()
    End Sub
    Public Sub modificarCbu()
        'Select Case MsgBox("ESTA A PUNTO DE SOBREESCRIBIR EL CBU ANTERIOR, ¿DESEA CONTINUAR?", MsgBoxStyle.YesNo, "AVISO")
        '    'en caso de elegir si
        '    Case MsgBoxResult.Yes
        '        sobreescribirCBU(txtDni.Text, txtCbu.Text)

        '    Case MsgBoxResult.No
        '        MessageBox.Show("ACCION CANCELADA POR EL USUARIO", "INFORMACION")
        'End Select
    End Sub

    Public Sub sobreescribirCBU(ByVal dni As String, ByVal cbu As String)
        'Using conn As New SqlConnection(CONNSTR)
        '    Using cmd As SqlCommand = conn.CreateCommand()
        '        conn.Open()
        '        cmd.CommandText = "UPDATE [dbo].[AACbu3]   SET [cbu] = '" & txtCbu.Text & " ' WHERE [dni]= '" & txtDni.Text & " '"
        '        cmd.ExecuteScalar()
        '    End Using
        'End Using
        'MessageBox.Show("EL CBU DEL BENEFICIARIO FUE ACTUALIZADO CORRECTAMENTE")
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        DataGridView2.Rows.Clear()
    End Sub
End Class