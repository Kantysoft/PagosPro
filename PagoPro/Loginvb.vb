Imports System.Data.SqlClient
'Imports iTextSharp
'Imports iTextSharp.text
'Imports iTextSharp.text.pdf
'Imports iTextSharp.text.Image
'Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet

Public Class Loginvb

    Private Sub Ingresar_Click(sender As System.Object, e As System.EventArgs) Handles Ingresar.Click
        If (validarUsuario(txtUsuario.Text, txtPass.Text)) Then
            MenuPrincipal.labelUsuario.Text = txtUsuario.Text
            MenuPrincipal.LabelTipo.Text = buscarTipoUsuario(txtUsuario.Text, txtPass.Text)
            MenuPrincipal.Show()
            Me.Close()
        Else
            MessageBox.Show("USUARIO O CONTRASEÑA INCORRECTA")

        End If


    End Sub
    Function buscarTipoUsuario(ByVal usuario As String, ByVal contraseña As String) As String
        Dim sql As String
        Dim tipo As String = ""
        sql = "SELECT [tipo]  FROM [SBDASIPT].[dbo].[AALogin]WHERE [usuario]='" & usuario & "' and [pass]='" & contraseña & "'"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                If reader.Read() Then
                    tipo = reader(0)
                End If
            End Using
        End Using
        Return tipo
    End Function
    Function validarUsuario(ByVal usuario As String, ByVal contraseña As String) As Boolean
        Dim sql As String
        Dim ban As Boolean = False
        sql = "SELECT [id],[usuario],[pass]  FROM [SBDASIPT].[dbo].[AALogin]WHERE [usuario]='" & usuario & "' and [pass]='" & contraseña & "'"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                If reader.Read() Then
                    ban = True
                End If
            End Using
        End Using
        Return ban
    End Function

    Private Sub butonSalir_Click(sender As System.Object, e As System.EventArgs) Handles butonSalir.Click
        Me.Close()
    End Sub

    Private Sub txtPass_Enter(sender As Object, e As System.EventArgs) Handles txtPass.Enter
       

    End Sub

    Private Sub txtPass_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtPass.KeyPress
        'Ingresar.Focus()
    End Sub

    Private Sub txtPass_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtPass.TextChanged

    End Sub
End Class