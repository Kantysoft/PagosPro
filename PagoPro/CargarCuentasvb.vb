Imports System.Data.SqlClient
'Imports iTextSharp
'Imports iTextSharp.text
'Imports iTextSharp.text.pdf
'Imports iTextSharp.text.Image
'Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet

Public Class CargarCuentasvb

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()
                conn.Open()
                'consulta sql que inserta una nueva tupla en la tabla AAOrdenNro
                'con los nuevos datos nro,causa de emision en la ventana, un string viatico Cargo
                'origen y la fecha actual.
                cmd.CommandText = "INSERT INTO [dbo].[AACuentas]" +
           "([descripcion])" +
                "VALUES(" +
                "@descripcion" +
                       ")"
                cmd.Parameters.Add("@descripcion", SqlDbType.VarChar, 50).Value = TextBox1.Text
                cmd.ExecuteScalar()
            End Using
        End Using
        MessageBox.Show("se guardo de manera correcta la orden")
    End Sub
End Class