Imports System.Data.SqlClient
Imports System.IO

Public Class DeleteOPG

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Eliminar_Click(sender As System.Object, e As System.EventArgs) Handles Eliminar.Click
        delete()

    End Sub
    Private Sub delete()
        Using conn1 As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn1.CreateCommand()
                conn1.Open()
                cmd.CommandText = "DELETE FROM [dbo].[AAOrdenPago2]   WHERE id= " & txtNroTransaccion.Text & ""
                cmd.ExecuteScalar()
                conn1.Close()
            End Using
        End Using
        'txt.Text = ""
        'txtNombre.Text = ""
        'txtTelefono.Text = ""
        'txtCertificado.Text = ""
        MessageBox.Show("SE A ELIMINADO LA ORDEN DE PAGO CORRECTAMENTE")
        'cargarDataGrid()

    End Sub
End Class