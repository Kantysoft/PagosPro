Imports System.Data.SqlClient
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.Image
Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet

Public Class Beneficiario

    Private Sub TxtTipoDoc_TextChanged(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub Label3_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub Label10_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs)
        Me.Close()
    End Sub

    
    Private Sub GroupBox1_Enter(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub Beneficiario_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        cargarDataGrid()
        GroupBox3.Visible = False
        identificarUsuario()
    End Sub
    Public Sub identificarUsuario()
        If (laberTIpo.Text.Equals("CONTROL2") Or laberTIpo.Text.Equals("CONTROL3")) Then
            btnModificar.Enabled = True
        Else
            btnModificar.Enabled = False
        End If
    End Sub
    Public Sub cargarDataGrid()
        Dim sql As String
        sql = "SELECT [pro_FecMod],[protdc_Cod],[pro_CUIT],[prosib_Cod],[pro_Piso],[prosiv_Cod],[pro_RazSoc],[pro_Direc],[pro_CodPos],[pro_NroIB],[pro_Cod]  FROM [dbo].[Proveed]"
        DataGridView1.Rows.Clear()
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = Sql
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
                    fila(13) = buscarCbu(reader(2))
                    DataGridView1.Rows.Add(fila)
                End While
            End Using
        End Using
        DataGridView1.Rows(0).Selected = True
    End Sub
    Function buscarCbu(ByVal dni As String) As String
        Dim cbu As String
        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd.CommandText = "SELECT [cbu] FROM [dbo].[AACbu3] WHERE [dni]='" & dni & "'"
                Dim reader As SqlDataReader = Nothing
                reader = cmd.ExecuteReader
                If reader.Read() Then
                    cbu = reader(0)
                End If
            End Using
        End Using
        Return cbu
    End Function

    Function equivalencia(ByVal tipo As Integer) As String
        Dim respuesta As String
        Dim sql As String
        sql = "Select tdc_Desc from [manager].[dbo].[TipoDocum] where tdc_Cod='" & tipo & "'"
        'declara una variable booleana

        'se conecta con la base de datos y ejecuta la consulta
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                If reader.Read() Then
                    respuesta = reader(0)
                    Return respuesta
                End If

            End Using
        End Using

    End Function

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs)
        comprobacionExportarcion()
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

        Dim sql As String
        sql = "SELECT [pro_FecMod],[protdc_Cod],[pro_CUIT],[prosib_Cod],[prosiv_Cod],[pro_RazSoc],[pro_Direc],[pro_Piso],[pro_CodPos],[pro_NroIB]  FROM [dbo].[Proveed] where  [pro_FecMod] between '" & dateDesde.Text & "' and '" & DateHasta.Text & "'"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
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
                End While
            End Using
        End Using
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

            If Directory.Exists("C:\Exportaciones") = False Then 'si no existe la carpeta se crea
                Directory.CreateDirectory("C:\Exportaciones")
            End If

            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            'If chkCTES.Checked Then
            PathArchivo = "C:\Exportaciones\BNF.txt" ' Se determina el nombre del archivo con la fecha actual

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

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs)

    End Sub
    Public Sub buscarBeneficiario(ByVal nroDocumento As String)
        Dim sql As String
        sql = "SELECT [pro_FecMod],[protdc_Cod],[pro_CUIT],[prosib_Cod],[pro_Piso],[prosiv_Cod],[pro_RazSoc],[pro_Direc],[pro_CodPos],[pro_NroIB]  FROM [dbo].[Proveed] where [pro_CUIT] like '%" & nroDocumento & "%'"
        DataGridView1.Rows.Clear()
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    Dim fila(12) As Object
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
                    DataGridView1.Rows.Add(fila)
                End While
            End Using
        End Using
        DataGridView1.Rows(0).Selected = True
        DataGridView1.Rows(0).Selected = True
    End Sub
    Public Sub buscarBeneficiarioNombre(ByVal Nombre As String)
        Dim sql As String
        sql = "SELECT [pro_FecMod],[protdc_Cod],[pro_CUIT],[prosib_Cod],[pro_Piso],[prosiv_Cod],[pro_RazSoc],[pro_Direc],[pro_CodPos],[pro_NroIB]  FROM [dbo].[Proveed] where [pro_RazSoc] like '%" & Nombre & "%'"
        DataGridView1.Rows.Clear()
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    Dim fila(12) As Object
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
                    DataGridView1.Rows.Add(fila)
                End While
            End Using
        End Using
        DataGridView1.Rows(0).Selected = True
        DataGridView1.Rows(0).Selected = True
    End Sub


    Private Sub Button2_Click_1(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub


    Private Sub Button3_Click_1(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        comprobacionExportarcion()
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs)
        Dim I As Integer = DataGridView1.CurrentCell.RowIndex
        Dim id As String
        id = DataGridView1.Rows(I).Cells(12).Value.ToString
        eliminarBeneficiario(id)
    End Sub
    Public Sub eliminarBeneficiario(ByVal id As String)
        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd.CommandText = "DELETE FROM AABeneficiarios WHERE id=" & id & " "
                cmd.ExecuteScalar()
            End Using
        End Using
        MessageBox.Show("EL BENEFICIARIO FUE ELIMINADO CORRECTAMENTE")
        cargarDataGrid()
    End Sub

    Private Sub txtBuscarDni_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtBuscarDni.TextChanged
        buscarBeneficiario(txtBuscarDni.Text)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            GroupBox3.Visible = True
        Else
            GroupBox3.Visible = False

        End If

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim I As Integer = DataGridView1.CurrentCell.RowIndex
        Dim dni As String
        Dim RazonSocial As String
        dni = DataGridView1.Rows(I).Cells(2).Value.ToString
        RazonSocial = DataGridView1.Rows(I).Cells(6).Value.ToString

        txtDni.Text = dni
        txtNombre.Text = RazonSocial
    End Sub

    Private Sub Cargar_Click(sender As System.Object, e As System.EventArgs) Handles Cargar.Click
        If (noExisteCbu().Equals(False)) Then
            Using conn As New SqlConnection(CONNSTR)
                Using cmd As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd.CommandText = "INSERT INTO [dbo].[AACbu3] (proveedor,cbu,dni) VALUES ('" & txtNombre.Text & "','" & txtCbu.Text & "','" & txtDni.Text & "')"
                    cmd.ExecuteScalar()
                End Using
            End Using
            MessageBox.Show("EL CBU DEL BENEFICIARIO FUE CARGADO CORRECTAMENTE ")
        Else
            MessageBox.Show("EL CBU DE ESTE PROVEEDOR YA FUE INGRESADO")
            'Select Case MsgBox("¿E?", MsgBoxStyle.YesNo, "AVISO")
            '    'en caso de seleccionar un si
            '    Case MsgBoxResult.Yes
            '        Using conn As New SqlConnection(CONNSTR)
            '            Using cmd As SqlCommand = conn.CreateCommand()
            '                conn.Open()
            '                cmd.CommandText = "INSERT INTO [dbo].[AACbu3] (proveedor,cbu,dni) VALUES ('" & TextBox2.Text & "','" & txtCbu.Text & "','" & TextBox1.Text & "')"
            '                cmd.ExecuteScalar()
            '            End Using
            '        End Using
            '        MessageBox.Show("EL CBU DEL BENEFICIARIO FUE CARGADO CORRECTAMENTE ")
            '    Case MsgBoxResult.No
            '        MessageBox.Show("Accion cancelada por el usuario", "INFORMACION")
            'End Select
        End If
    End Sub
    Private Function noExisteCbu() As Boolean
        Dim ban As Boolean = False
        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd.CommandText = "SELECT [ID],[proveedor],[cbu] ,[dni] FROM [dbo].[AACbu3] WHERE [dni]='" & txtDni.Text & "'"
                Dim reader As SqlDataReader = Nothing
                reader = cmd.ExecuteReader
                If reader.Read() Then
                    ban = True
                Else
                    ban = False
                End If
            End Using
        End Using
        Return ban
    End Function

    Private Sub TextBox3_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtNombreProveedor.TextChanged
        buscarBeneficiarioNombre(txtNombreProveedor.Text)
    End Sub


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs)
        cargaPersonalizadaBeneficiario.Show()
    End Sub

    Private Sub btnModificar_Click(sender As System.Object, e As System.EventArgs) Handles btnModificar.Click
        modificarCbu()
    End Sub
    Public Sub modificarCbu()
        Select Case MsgBox("ESTA A PUNTO DE SOBREESCRIBIR EL CBU ANTERIOR, ¿DESEA CONTINUAR?", MsgBoxStyle.YesNo, "AVISO")
            'en caso de elegir si
            Case MsgBoxResult.Yes
                sobreescribirCBU(txtDni.Text, txtCbu.Text)

            Case MsgBoxResult.No
                MessageBox.Show("ACCION CANCELADA POR EL USUARIO", "INFORMACION")
        End Select
    End Sub
    Public Sub sobreescribirCBU(ByVal dni As String, ByVal cbu As String)
        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd.CommandText = "UPDATE [dbo].[AACbu3]   SET [cbu] = '" & txtCbu.Text & " ' WHERE [dni]= '" & txtDni.Text & " '"
                cmd.ExecuteScalar()
            End Using
        End Using
        MessageBox.Show("EL CBU DEL BENEFICIARIO FUE ACTUALIZADO CORRECTAMENTE")
    End Sub

    Private Sub GroupBox2_Enter(sender As System.Object, e As System.EventArgs) Handles GroupBox2.Enter

    End Sub

    Private Sub Label18_Click(sender As System.Object, e As System.EventArgs) Handles Label18.Click

    End Sub

    Private Sub Label4_Click(sender As System.Object, e As System.EventArgs) Handles Label4.Click

    End Sub

    Private Sub laberTIpo_Click(sender As System.Object, e As System.EventArgs) Handles laberTIpo.Click

    End Sub

    Private Sub labelUsuario_Click(sender As System.Object, e As System.EventArgs) Handles labelUsuario.Click

    End Sub
End Class