Imports System.Data.SqlClient
'Imports iTextSharp
'Imports iTextSharp.text
'Imports iTextSharp.text.pdf
'Imports iTextSharp.text.Image
'Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet

Public Class EliminarOPG

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        buscarOPG(txtDocumento.Text, dateFechaBusqueda.Text)
    End Sub
    Public Sub buscarOPG(ByVal nroDocumento As String, ByVal fecha As String)
        Dim sql As String
        Dim doc As Integer = CInt(txtDocumento.Text)
        'sql = "SELECT [tdd_bnf_id],[bnf_numdoc],[suc_entrega],[opg_idopgcli],[opg_ordenalt],[opg_imp_pago],[cta_cuentadebito],[opg_cuentapago],[mpg_id],[opg_mar_regchq],[opg_fec_pago],[opg_fec_pagodiferido],[usr_firma1],[usr_firma2],[usr_firma3]  FROM [dbo].[AAOrdenPago] where [opg_fec_pago] =" & dateFechaBusqueda.Text & " and [bnf_numdoc]=" & txtDocumento.Text & ""
        sql = "SELECT [opg_fec_pago],[opg_ordenalt],[tdd_bnf_id],[bnf_numdoc],[suc_entrega],[opg_idopgcli],[opg_imp_pago],[cta_cuentadebito],[opg_cuentapago],[mpg_id],[opg_mar_regchq],[opg_fec_pago],[opg_fec_pagodiferido],[usr_firma1] FROM [dbo].[AAOrdenPago] where [bnf_numdoc]=" & doc & ""
        DataGridView2.Rows.Clear()
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    Dim fila(15) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    fila(2) = reader(2)
                    fila(3) = reader(3)
                    fila(4) = reader(4)
                    'fila(5) = reader(5)
                    fila(6) = reader(6)
                    fila(7) = reader(7)
                    fila(8) = reader(8)
                    fila(9) = reader(9)
                    fila(10) = reader(10)
                    fila(11) = reader(11)
                    fila(12) = reader(12)
                    fila(13) = reader(13)
                    DataGridView2.Rows.Add(fila)
                End While
            End Using
        End Using
    End Sub
End Class