Imports System.Data.SqlClient
'Imports iTextSharp
'Imports iTextSharp.text
'Imports iTextSharp.text.pdf
'Imports iTextSharp.text.Image
'Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet

Public Class cambiarContraseña

    Dim sql As String
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Guardar_Click(sender As System.Object, e As System.EventArgs) Handles Guardar.Click
        actualizarCuenta(txtUsuario.Text, txtContraseñaAnterior.Text, txtNuevaContraseña.Text)
    End Sub
  
    Public Sub actualizarCuenta(ByVal usuario As String, ByVal pass As String, ByRef nuevaPass As String)
        If (validarUsuario(usuario, pass)) Then
            'sql = "update Talonar set tal_ActualNro='" & nroOrden & "' where tal_Cod='" & origen & "'"
            sql = "UPDATE [dbo].[AALogin] SET [usuario] ='" & usuario & "'  ,[pass] ='" & nuevaPass & "' WHERE [usuario]='" & usuario & "' and [pass]='" & pass & "'"
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql
                    cmd4.ExecuteScalar()
                End Using
            End Using
            MessageBox.Show("LA CONTRASEÑA DE LA CUENTA FUE CAMBIADA CORRECTAMENTE")
        Else
            MessageBox.Show("ERROR LA CONTRASEÑA ANTERIOR NO ES CORRECTA")
        End If
    End Sub
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

    Private Sub cambiarContraseña_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        txtUsuario.Enabled = False
    End Sub
End Class