Imports System.Data.SqlClient
'Imports iTextSharp
'Imports iTextSharp.text
'Imports iTextSharp.text.pdf
'Imports iTextSharp.text.Image
'Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet

Public Class EliminarCuenta

    Private Sub Eliminar_Click(sender As System.Object, e As System.EventArgs) Handles Eliminar.Click
        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()
                conn.Open()
                'consulta sql que inserta una nueva tupla en la tabla AAOrdenNro
                'con los nuevos datos nro,causa de emision en la ventana, un string viatico Cargo
                'origen y la fecha actual.
                cmd.CommandText = "DELETE FROM [dbo].[AACuentas]" +
           "WHERE id=" & txtCodigo.Text & "" +
                        cmd.ExecuteScalar()
            End Using
        End Using
        MessageBox.Show("SE CARGO DE MANERA CORRECTA LA CUENTA")
        Dim j As Integer = DataGridView1.CurrentCell.RowIndex
        DataGridView1.Rows.RemoveAt(j)

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub EliminarCuenta_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        cargarDataGrid()
    End Sub
    Public Sub cargarDataGrid()
        Dim sql As String
        sql = "select id, descripcion from AACuentas"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = Sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Do While reader.Read()
                    Dim fila(3) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    DataGridView1.Rows.Add(fila)
                Loop
            End Using
        End Using
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_Disposed(sender As Object, e As System.EventArgs) Handles DataGridView1.Disposed

    End Sub

    Private Sub DataGridView1_DoubleClick(sender As Object, e As System.EventArgs) Handles DataGridView1.DoubleClick
        Dim I As Integer = DataGridView1.CurrentCell.RowIndex
        txtCodigo.Text = DataGridView1.Rows(I).Cells(0).Value
        txtDescripcion.Text = DataGridView1.Rows(I).Cells(0).Value

    End Sub
End Class