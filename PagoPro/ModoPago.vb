Imports System.Data.SqlClient
'Imports iTextSharp
'Imports iTextSharp.text
'Imports iTextSharp.text.pdf
'Imports iTextSharp.text.Image
'Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet

Public Class ModoPago

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Me.Close()

    End Sub

    Private Sub ModoPago_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
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
                    fila(1) = reader(1)
                    DataGridView1.Rows.Add(fila)
                End While
            End Using
        End Using
    End Sub

    Private Sub GroupBox1_Enter(sender As System.Object, e As System.EventArgs) Handles GroupBox1.Enter

    End Sub
End Class