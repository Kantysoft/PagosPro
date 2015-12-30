Imports System.Data.SqlClient
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.Image
Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet

Public Class ModificarCBU
#Region "Cargar"
    Private Sub cargarDataGrid()
        Dim sql As String
        sql = "SELECT [dni],[proveedor],[cbu]  FROM [dbo].[AACbu3]"
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
                    DataGridView1.Rows.Add(fila)
                End While
            End Using
        End Using
        DataGridView1.Rows(0).Selected = True
    End Sub
    Private Sub cargarProveedor()
        Dim sql As String
        sql = "SELECT [pro_CUIT],[pro_RazSoc]  FROM [dbo].[Proveed]"
        DataGridView2.Rows.Clear()
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
                    'fila(2) = reader(2)
                    DataGridView2.Rows.Add(fila)
                End While
            End Using
        End Using
        DataGridView2.Rows(0).Selected = True
    End Sub
#End Region
#Region "Buscar"
    Private Sub buscarCBU()
        'where [pro_RazSoc] like '%" & Nombre & "%'"
        Dim sql As String
        sql = "SELECT [dni],[proveedor],[cbu]  FROM [dbo].[AACBU3] where [proveedor] like '%" & txtBuscarCbu.Text & "%' or [dni] like '%" & txtBuscarCbu.Text & "%' "
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
                    '''''''''''''''''''''''''''''''''''''''
                    DataGridView1.Rows.Add(fila)
                End While
            End Using
        End Using
        DataGridView1.Rows(0).Selected = True
    End Sub
    Private Sub buscarProveedor()
        'where [pro_RazSoc] like '%" & Nombre & "%'"
        Dim sql As String
        sql = "SELECT [pro_CUIT],[pro_RazSoc]  FROM [dbo].[Proveed] where [pro_RazSoc] like '%" & txtBuscarProveedor.Text & "%' or [pro_CUIT] like '%" & txtBuscarProveedor.Text & "%' "
        DataGridView2.Rows.Clear()
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
                    'fila(2) = reader(2)
                    '''''''''''''''''''''''''''''''''''''''
                    DataGridView2.Rows.Add(fila)
                End While
            End Using
        End Using
        DataGridView2.Rows(0).Selected = True
    End Sub
#End Region
    Private Sub ModificarCBU_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        cargarDataGrid()
        cargarProveedor()
        identificarUsuario()
    End Sub
    Public Sub identificarUsuario()
        If (laberTIpo.Text.Equals("CONTROL2") Or laberTIpo.Text.Equals("CONTROL3")) Then
            btnModificar.Enabled = True
        Else
            btnModificar.Enabled = False
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
        'Dim unModificarCBU = New ModificarCBU()
        'unModificarCBU.Show()
    End Sub

    Private Sub TextBox4_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtBuscarCbu.TextChanged
        buscarCbu()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_DoubleClick(sender As Object, e As System.EventArgs) Handles DataGridView1.DoubleClick
        Dim I As Integer = DataGridView1.CurrentCell.RowIndex
        Dim dni As String
        Dim RazonSocial As String
        Dim CBU As String
        dni = DataGridView1.Rows(I).Cells(0).Value.ToString
        RazonSocial = DataGridView1.Rows(I).Cells(1).Value.ToString
        CBU = DataGridView1.Rows(I).Cells(2).Value.ToString
        txtDni.Text = dni
        txtNombre.Text = RazonSocial
        txtCbu.Text = CBU
    End Sub

    Private Sub btnModificar_Click(sender As System.Object, e As System.EventArgs) Handles btnModificar.Click
        modificarCbu()
    End Sub
    Public Sub modificarCbu()
        Select Case MsgBox("ESTA A PUNTO DE SOBREESCRIBIR EL CBU ANTERIOR, ¿DESEA CONTINUAR?", MsgBoxStyle.YesNo, "AVISO")
            'en caso de elegir si
            Case MsgBoxResult.Yes
                sobreescribirCBU(txtDni.Text, txtCbu.Text)
                cargarDataGrid()
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

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtBuscarProveedor.TextChanged
        buscarProveedor()
    End Sub

    Private Sub DataGridView2_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick

    End Sub

    Private Sub DataGridView2_DoubleClick(sender As Object, e As System.EventArgs) Handles DataGridView2.DoubleClick
        Dim I As Integer = DataGridView2.CurrentCell.RowIndex
        txtDni.Text = DataGridView2.Rows(I).Cells(0).Value.ToString
        txtNombre.Text = DataGridView2.Rows(I).Cells(1).Value.ToString

    End Sub
End Class