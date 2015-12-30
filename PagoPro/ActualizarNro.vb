Imports System.Data.SqlClient
Imports System.Data.Odbc
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.Image
Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet
Public Class ActualizarNro

    Private Sub Label4_Click(sender As System.Object, e As System.EventArgs) Handles Label4.Click

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Me.Dispose()

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim sql As String
        Dim causaEmision As String
        Dim beneficiario As String
        Dim NroCuenta As String
        Dim NroCheque As String
        Dim anterior As String
        causaEmision = txtNroPago.Text
        beneficiario = txtDestinatario.Text
        NroCuenta = txtNroCuenta.Text
        NroCheque = txtNroCheque.Text
        anterior = txtAnteriorNroCheque.Text
        If (Not NroCheque.Equals("")) Then
            sql = " UPDATE [dbo].[ChequesP] SET [chp_NroCheq] = '" & NroCheque & "'  where [chp_NroCheq]= '" & anterior & "'"
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql
                    cmd4.ExecuteScalar()
                End Using
            End Using

            MessageBox.Show("SE ACTUALIZO DE MANERA CORRECTA EL CHEQUE")
            Me.Close()
        Else
            MessageBox.Show("NO INGRESO NINGUN NRO DE CHEQUE")
        End If
    End Sub

    Private Sub ActualizarNro_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class