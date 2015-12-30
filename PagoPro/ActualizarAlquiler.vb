Imports System.Data.SqlClient
Imports System.Data.Odbc
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.Image
Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet

Public Class ActualizarAlquiler

    'VARIABLES PARA CONSULTAS SQL
    Dim sql As String
    Dim sql2 As String
    Dim sql3 As String
    Dim sql4 As String

    'VARIABLES DE CONECCION DEL ODBC
    Dim da As New OdbcDataAdapter
    Dim da2 As New OdbcDataAdapter
    Dim da3 As New OdbcDataAdapter

    'VARIABLES PARA CONTROLES COMUNES
    Dim anio As String
    Dim indiceRow As String
    Dim indiceColumn As String


#Region "CODIGOS AUXILIARES"

    Public Sub cargarCombo()
        sql = "select Descripcion from AASituaciones where  Descripcion like '%Alquiler%'"

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader

                Do While reader.Read()
                    Dim fila(1) As Object

                    fila(0) = reader(0)
                    cmbTipoOrden.Items.Add(fila(0))

                Loop

            End Using
        End Using
    End Sub

    Public Function adjuntarAnio() As String
        Dim codigo As String
        Dim fecha As String
        'fecha = Date.Now.Year
        fecha = cmbA.SelectedItem
        If txtCodigo.Text.Length = 1 Then
            codigo = fecha & "000" & txtCodigo.Text
        End If
        If txtCodigo.Text.Length = 2 Then
            codigo = fecha & "00" & txtCodigo.Text
        End If
        If txtCodigo.Text.Length = 3 Then
            codigo = fecha & "0" & txtCodigo.Text
        End If
        If txtCodigo.Text.Length = 4 Then
            codigo = fecha & txtCodigo.Text
        End If
        If txtCodigo.Text.Length > 4 Then
            MessageBox.Show("El codigo solicitado supera la extension requerida")
            codigo = "0000"
        End If
        Return codigo
    End Function





    Public Sub cargarDataAlquiler()
        DataGridView1.Columns.Add("fecha", "FECHA")
        DataGridView1.Columns.Add("comprobante", "COMPROBANTE")
        DataGridView1.Columns.Add("detalle", "DETALLE")
        DataGridView1.Columns.Add("monto", "MONTO")
        DataGridView1.Columns.Item("monto").DefaultCellStyle.Format = "###,##0.00"
        Dim tipo As String

        sql2 = "SELECT [ccotco_Cod]  FROM [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " group by [ccotco_Cod]"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql2

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                If reader.Read() Then
                    tipo = reader(0)
                End If
            End Using
        End Using
        Try
            If tipo.Equals("DDL") Then
                sql = "SELECT [cco_FEmision],[cco_Nro],[ccopro_RazSoc],[cco_ImpMonLoc],[ccopro_CUIT],[cco_CodPvt],[ccotco_Cod] FROM [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " GROUP BY [cco_FEmision],[cco_Nro],[ccopro_RazSoc],[cco_ImpMonLoc],[ccopro_CUIT],[cco_CodPvt],[ccotco_Cod]"
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()

                        conn.Open()

                        cmd4.CommandText = sql

                        Dim reader As SqlDataReader = Nothing
                        reader = cmd4.ExecuteReader

                        Do While reader.Read()
                            Dim fila(4) As Object


                            fila(0) = reader(0)
                            fila(1) = reader(1)
                            fila(2) = reader(2) & "  CUIT:" & reader(4)
                            fila(3) = reader(3) * (-1)

                            DataGridView2.Rows.Add(fila)

                        Loop
                    End Using
                End Using
            Else
                sql = "SELECT [cco_FEmision],[cco_Nro],[ccopro_RazSoc],[cco_ImpMonLoc],[ccopro_CUIT],[cco_CodPvt],[ccotco_Cod] FROM [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " GROUP BY [cco_FEmision],[cco_Nro],[ccopro_RazSoc],[cco_ImpMonLoc],[ccopro_CUIT],[cco_CodPvt],[ccotco_Cod]"
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()

                        conn.Open()

                        cmd4.CommandText = sql

                        Dim reader As SqlDataReader = Nothing
                        reader = cmd4.ExecuteReader

                        Do While reader.Read()
                            Dim fila(4) As Object


                            fila(0) = reader(0)
                            fila(1) = reader(6) & "." & reader(5) & "." & reader(1)
                            fila(2) = reader(2) & "  CUIT:" & reader(4)
                            fila(3) = reader(3) * (-1)

                            DataGridView1.Rows.Add(fila)

                        Loop
                    End Using
                End Using
            End If
        Catch ex As Exception

        End Try
        calculoTotalAlquilerRendicion()
    End Sub

    Public Sub cargarNombres()
        sql = " SELECT  distinct CabCompra.ccopro_RazSoc FROM CabCompra  INNER JOIN CausaEmi ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod INNER JOIN  ItemComp ON CabCompra.cco_ID=ItemComp.icocco_ID  WHERE  CausaEmi.cem_Desc='" & adjuntarAnio() & "' group BY CabCompra.ccopro_RazSoc"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Do While reader.Read()
                    Dim fila(1) As Object
                    fila(0) = reader(0)
                    DataGridView1.Rows.Add(fila)
                Loop

            End Using
        End Using
        adjuntarDatos()
    End Sub

    Public Sub adjuntarDatos()
        sql = "SELECT [cco_ID],[ccopro_RazSoc],[cco_ImpMonLoc],[ico_Desc],ItemComp.ico_CantUM1,[ico_NetoLoc] FROM [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " GROUP BY [cco_ID],[ccopro_RazSoc],[cco_ImpMonLoc],[ico_Desc],ItemComp.ico_CantUM1,[ico_NetoLoc]"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader

                Do While reader.Read()
                    Dim day As Double = 0
                    For Each column As DataGridViewColumn In DataGridView1.Columns

                        For Each row As DataGridViewRow In DataGridView1.Rows

                            If row.Cells(0).Value.Equals(reader(1)) Then

                                If column.Name.Contains(reader(3)) Then
                                    indiceColumn = column.Index
                                    row.Cells(reader(3)).Value = reader(5) * -1
                                End If

                                'row.Cells(1).Value = reader(3)


                            End If
                        Next
                    Next
                Loop
            End Using
        End Using
        calculoMonto()
    End Sub

    Public Sub calculoMonto()
        Dim monto As Double
        Dim dia As Double
        Dim MontoFinal As Double

        For Each row As DataGridViewRow In DataGridView1.Rows
            monto = 0
            MontoFinal = 0
            dia = 0
            For Each column As DataGridViewColumn In DataGridView1.Columns
                Try
                    monto += row.Cells(column.Index).Value
                Catch ex As Exception

                End Try

            Next

            row.Cells("MONTO").Value = monto
        Next
    End Sub

    Public Sub generarOrdenPago()
        sql = " SELECT distinct  ItemComp.ico_Desc FROM CabCompra  INNER JOIN CausaEmi ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod INNER JOIN  ItemComp ON CabCompra.cco_ID=ItemComp.icocco_ID  WHERE  CausaEmi.cem_Desc='" & adjuntarAnio() & "'ORDER BY ItemComp.ico_Desc"


        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader

                DataGridView1.Columns.Add("Imputacion", "IMPUTACION")
                DataGridView1.Columns.Add("nombre", "NOMBRE")
                DataGridView1.Columns.Add("importe", "IMPORTE")


            End Using
        End Using

        cargarNombresPago()

    End Sub

    Public Sub cargarNombresPago()
        sql = " SELECT  distinct CabCompra.ccopro_RazSoc FROM CabCompra  INNER JOIN CausaEmi ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod INNER JOIN  ItemComp ON CabCompra.cco_ID=ItemComp.icocco_ID  WHERE  CausaEmi.cem_Desc='" & adjuntarAnio() & "' group BY CabCompra.ccopro_RazSoc"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Do While reader.Read()
                    Dim fila(2) As Object
                    fila(0) = "PAGO ALQUILERES"
                    fila(1) = reader(0)
                    DataGridView1.Rows.Add(fila)
                Loop

            End Using
        End Using
        adjuntarPagos()
    End Sub

    Public Sub adjuntarPagos()
        sql = "SELECT [ccopro_RazSoc],sum([ico_NetoLoc]) FROM [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " GROUP BY [ccopro_RazSoc]"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader

                Do While reader.Read()

                    For Each column As DataGridViewColumn In DataGridView1.Columns

                        For Each row As DataGridViewRow In DataGridView1.Rows

                            If row.Cells(1).Value.Equals(reader(0)) Then

                                indiceColumn = column.Index
                                row.Cells("IMPORTE").Value = reader(1) * -1

                            End If

                        Next

                    Next


                Loop
            End Using
        End Using
        calcularMontoPago()
    End Sub

    Public Sub calcularMontoPago()
        Dim Total As Double = 0


        For Each row As DataGridViewRow In Me.DataGridView1.Rows
            Total += row.Cells("IMPORTE").Value
        Next

        txtTotalFinal.Text = Total.ToString
    End Sub

    Public Sub calculoTotalAlquiler()

        Dim Total As Double = 0


        For Each row As DataGridViewRow In Me.DataGridView1.Rows
            Total += row.Cells("MONTO").Value
        Next

        txtTotalFinal.Text = Total.ToString

    End Sub

    Public Sub ordenCargo()
        sql = "SELECT [chp_NroCheq],[chpctb_Cod],[chp_Importe],[pro_RazSoc]  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
              "inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
              "inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]" &
              "where CausaEmi.cem_Desc='" & adjuntarAnio() & "'"

        DataGridView1.Columns.Add("nombre", "IMPORTE EN LETRAS EXPRESADO EN PESOS")
        DataGridView1.Columns.Add("ctaCte", "CUENTA CORRIENTE")
        DataGridView1.Columns.Add("cheques", "CHEQUES")
        DataGridView1.Columns.Add("destinatario", "DESTINATARIO")




        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader


                Dim count As Integer
                count = 0
                Do While reader.Read()

                    Dim fila(5) As Object
                    fila(0) = Numalet.ToCardinal(reader(2))
                    fila(1) = reader(1)
                    fila(2) = reader(0)
                    fila(3) = reader(3)

                    DataGridView1.Rows.Add(fila)

                Loop

            End Using
        End Using

    End Sub

    Public Function dameNroTalonario() As String
        sql = "SELECT [Talonar].[tal_ActualNro] FROM  [SBDASIPT].[dbo].[Talonar] inner join [SBDASIPT].[dbo].[AASituaciones]  on AASituaciones.Cod_tal=Talonar.tal_Cod where AASituaciones.Descripcion='Alquileres'"
        Dim nroOrden As String
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                reader.Read()

                If reader.HasRows Then
                    nroOrden = reader(0) + 1
                Else

                End If

            End Using
        End Using

        Return nroOrden
    End Function

    Public Function dameNroPago() As String

        sql = "SELECT [Talonar].[tal_ActualNro] FROM  [SBDASIPT].[dbo].[Talonar] inner join [SBDASIPT].[dbo].[AASituaciones]  on AASituaciones.Cod_tal=Talonar.tal_Cod where AASituaciones.Descripcion='Pago'"

        Dim nroOrden As String
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                reader.Read()

                If reader.HasRows Then
                    nroOrden = reader(0) + 1
                Else

                End If

            End Using
        End Using

        Return nroOrden
    End Function

    Public Sub generarRendicion()
        sql = " SELECT distinct  ItemComp.ico_Desc FROM CabCompra  INNER JOIN CausaEmi ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod INNER JOIN  ItemComp ON CabCompra.cco_ID=ItemComp.icocco_ID  WHERE  CausaEmi.cem_Desc='" & adjuntarAnio() & "'ORDER BY ItemComp.ico_Desc"


        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader

                DataGridView1.Columns.Add("nombre", "NOMBRE")


                Dim count As Integer
                count = 0
                Do While reader.Read()

                    DataGridView1.Columns.Add(reader(0), reader(0))

                    'indice = DataGridView1.Columns("viatico" & count).Index.ToString()
                    count = count + 1
                Loop
                DataGridView1.Columns.Add("monto", "DEVENGADO")
                DataGridView1.Columns.Add("pagar", "A PAGAR")
                DataGridView1.Columns.Add("reintegrar", "A REINTEGRAR")
                DataGridView1.Columns.Add("firma", "FIRMA")
            End Using
        End Using

        cargarNombresRendicion()

        'cargarComboNombres()
        calculoMonto()
        calculoTotalAlquilerRendicion()
    End Sub

    Public Sub cargarNombresRendicion()
        sql = " SELECT  distinct CabCompra.ccopro_RazSoc FROM CabCompra  INNER JOIN CausaEmi ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod INNER JOIN  ItemComp ON CabCompra.cco_ID=ItemComp.icocco_ID  WHERE  CausaEmi.cem_Desc='" & adjuntarAnio() & "' group BY CabCompra.ccopro_RazSoc"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Do While reader.Read()
                    Dim fila(1) As Object
                    fila(0) = reader(0)
                    DataGridView1.Rows.Add(fila)
                Loop

            End Using
        End Using
        adjuntarDatosRendicion()

    End Sub

    Public Sub adjuntarDatosRendicion()
        sql = "SELECT [cco_ID],[ccopro_RazSoc],[cco_ImpMonLoc],[ico_Desc],ItemComp.ico_CantUM1,[ico_NetoLoc] FROM [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " GROUP BY [cco_ID],[ccopro_RazSoc],[cco_ImpMonLoc],[ico_Desc],ItemComp.ico_CantUM1,[ico_NetoLoc]"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader

                Do While reader.Read()
                    Dim day As Double = 0
                    For Each column As DataGridViewColumn In DataGridView1.Columns

                        For Each row As DataGridViewRow In DataGridView1.Rows

                            If row.Cells(0).Value.Equals(reader(1)) Then
                                If column.Name.Contains(reader(3)) Then
                                    indiceColumn = column.Index
                                    row.Cells(reader(3)).Value = reader(5) * -1


                                End If

                            End If

                        Next

                    Next


                Loop
            End Using
        End Using

    End Sub

    Public Sub cargarDataRendicion()
        sql = "SELECT [chp_NroCheq],[chpctb_Cod],[chp_Importe],[pro_RazSoc],[chp_FVto]  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
              "inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
              "inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]" &
              "where CausaEmi.cem_Desc='" & adjuntarAnio() & "'"

        DataGridView2.Columns.Add("fecha", "FECHA EMISION")
        DataGridView2.Columns.Add("destinatario", "BENEFICIARIO")
        DataGridView2.Columns.Add("ctaCte", "CUENTA CORRIENTE")
        DataGridView2.Columns.Add("cheques", "NRO CHEQUE")
        DataGridView2.Columns.Add("nombre", "IMPORTE")




        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader


                Dim count As Integer
                count = 0
                Do While reader.Read()

                    Dim fila(5) As Object
                    fila(0) = reader(4)
                    fila(1) = reader(3)
                    fila(2) = reader(1)
                    fila(3) = reader(0)
                    fila(4) = reader(2)

                    DataGridView2.Rows.Add(fila)

                Loop

            End Using
        End Using
        calculoTotalAlquilerRendicion2()
    End Sub

    Public Sub calculoTotalAlquilerRendicion2()

        Dim Total As Double = 0


        For Each row As DataGridViewRow In Me.DataGridView2.Rows
            Total += row.Cells(4).Value
        Next

        txtTotalFinal.Text = Total.ToString

    End Sub

    Public Sub calculoTotalAlquilerRendicion()

        Dim Total As Double = 0


        For Each row As DataGridViewRow In Me.DataGridView1.Rows
            Total += row.Cells(3).Value
        Next

        txtTotalFinal.Text = Total.ToString

    End Sub

    Private Sub almacenarOrden(ByRef nro As String, ByRef origen As String)
        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd.CommandText = "insert into AAOrdenNro(" +
                               "[NroIngreso]" +
                               ",[Concepto]" +
                               ",[Tipo]" +
                               ",[Fecha]" +
                               ",[NacPro]" +
                                ") VALUES (" +
                                "@NroIngreso" +
                                ",@Concepto" +
                                ",@Tipo" +
                                ",@Fecha" +
                                ",@NacPro" +
                                ")"


                cmd.Parameters.Add("@Concepto", SqlDbType.Int).Value = nro
                cmd.Parameters.Add("@NroIngreso", SqlDbType.Int).Value = adjuntarAnio()
                cmd.Parameters.Add("@Tipo", SqlDbType.VarChar, 50).Value = "Alquiler"
                cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = Date.Now
                cmd.Parameters.Add("@NacPro", SqlDbType.VarChar, 3).Value = origen

                cmd.ExecuteScalar()


            End Using
        End Using

    End Sub

    Private Function almacenarOrdenPago(ByRef nro As String, ByRef origen As String) As Boolean

        Dim bandera As Boolean

        Try
            Using conn As New SqlConnection(CONNSTR)
                Using cmd As SqlCommand = conn.CreateCommand()

                    conn.Open()

                    cmd.CommandText = "insert into AAOrdenNro(" +
                                   "[NroIngreso]" +
                                   ",[Concepto]" +
                                   ",[Tipo]" +
                                   ",[Fecha]" +
                                   ",[NacPro]" +
                                    ") VALUES (" +
                                    "@NroIngreso" +
                                    ",@Concepto" +
                                    ",@Tipo" +
                                    ",@Fecha" +
                                    ",@NacPro" +
                                    ")"


                    cmd.Parameters.Add("@Concepto", SqlDbType.Int).Value = nro
                    cmd.Parameters.Add("@NroIngreso", SqlDbType.Int).Value = adjuntarAnio()
                    cmd.Parameters.Add("@Tipo", SqlDbType.VarChar, 50).Value = "AlquilerPago"
                    cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = Date.Now
                    cmd.Parameters.Add("@NacPro", SqlDbType.VarChar, 3).Value = origen

                    cmd.ExecuteScalar()


                End Using
            End Using
            bandera = True
            MessageBox.Show("Los datos de la orden se almacenaron correctamente", "Exito")
        Catch ex As Exception
            bandera = False
            MessageBox.Show("Los datos de la orden no se almacenaron correctamente, verifiquelos", "AVISO")
        End Try
        Return bandera
    End Function

    Private Sub almacenarDisp()
        If chkDisp.Checked Then
            sql = "update OrdenNro set Disposicion='" & txtNro.Text & "' where Tipo='AlquilerPago' and NroIngreso='" & adjuntarAnio() & "'"
        End If

        If chkRes.Checked Then
            sql = "update AAOrdenNro set Resolucion='" & txtNro.Text & "' where Tipo='AlquilerPago' and NroIngreso='" & adjuntarAnio() & "'"
        End If

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql
                cmd4.ExecuteScalar()

            End Using
        End Using
    End Sub

#End Region

#Region "IMPRIMIR"

    Public Sub imprimirPago()
        Dim BANDERA As Boolean = False
        BANDERA = False

        Dim Origen As String
        If chkNac.Checked Then
            Origen = "OPN"
        End If
        If chkProv.Checked Then
            Origen = "OPP"
        End If

        'sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='AlquilerPago'"
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        sql = "Select Concepto from AAOrdenNro where NroIngreso like '%" & cmbA.Text & "%' and NacPro='" & Origen & "'"

        'se declara la variable bandera de tipo boolean
        'se conecta con la base de datos y ejecuta la consulta
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader

                While (reader.Read() And BANDERA.Equals(False))

                    'pregunta si el concepto obtenido esta vacio se le asigna falso a la variable booleana
                    If reader(0).ToString.Equals("") Or reader(0).ToString.Equals(Nothing) Then
                        BANDERA = False
                    End If
                    ''si tiene un valor null se le asgina false a la variable booleana 
                    If IsDBNull(reader(0)) Then
                        BANDERA = False
                    End If
                    'MessageBox.Show(reader(0))
                    If (reader(0).ToString.Equals(txtSaliente.Text)) Then
                        BANDERA = True
                    Else
                        BANDERA = False
                    End If
                End While
            End Using
        End Using
        ''''''''''''''''''''''''''''''''

        'Dim BANDERA As Boolean

        'Using conn As New SqlConnection(CONNSTR)
        '    Using cmd4 As SqlCommand = conn.CreateCommand()
        '        conn.Open()
        '        cmd4.CommandText = sql
        '        Dim reader As SqlDataReader = Nothing
        '        reader = cmd4.ExecuteReader
        '        If reader.Read() Then
        '            If reader(0).ToString.Equals("") Or reader(0).ToString.Equals(Nothing) Then
        '                BANDERA = False
        '            Else
        '                If IsDBNull(reader(0)) Then
        '                    BANDERA = False
        '                Else
        '                    BANDERA = True
        '                End If
        '            End If
        '        Else
        '            BANDERA = False
        '        End If

        '    End Using
        'End Using
        If BANDERA Then
            'Select Case MsgBox("¿Ya existe una orden con este numero desea reemplazarla?", MsgBoxStyle.YesNo, "AVISO")
            'en caso de seleccionar un si
            '    Case MsgBoxResult.Yes
            'Elimina la tupla existente en la base de datos y despues inserta la nueva orden de pago
            'reemplazarImprimirAlquiler()
            '   Case MsgBoxResult.No
            MessageBox.Show("YA EXISTE UNA ORDEN DE PAGO CON ESTE NUMERO", "INFORMACION")
            'End Select

            'imprimirPagoHistorial()
        Else
            Dim nroCargo As String
            Dim nroOrden As String
            nroCargo = txtSaliente.Text
            Try
                If nroCargo.ToString.Length = 1 Then
                    nroOrden = "0000000" & nroCargo
                End If
                If nroCargo.ToString.Length = 2 Then
                    nroOrden = "000000" & nroCargo
                End If
                If nroCargo.ToString.Length = 3 Then
                    nroOrden = "00000" & nroCargo
                End If
                If nroCargo.ToString.Length = 4 Then
                    nroOrden = "0000" & nroCargo
                End If
                If nroCargo.ToString.Length = 5 Then
                    nroOrden = "000" & nroCargo
                End If
                If nroCargo.ToString.Length = 6 Then
                    nroOrden = "00" & nroCargo
                End If
                If nroCargo.ToString.Length = 7 Then
                    nroOrden = "0" & nroCargo
                End If
                If nroCargo.ToString.Length = 8 Then
                    nroOrden = nroCargo
                End If

                sql = "update Talonar set tal_ActualNro='" & nroOrden & "' where tal_Cod='" & Origen & "'"
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()

                        conn.Open()

                        cmd4.CommandText = sql
                        cmd4.ExecuteScalar()

                    End Using
                End Using
                MessageBox.Show("El talonario se actualizo de manera correcta", "EXITO")
            Catch ex As Exception
                MessageBox.Show("Un error provoco que no se actualice el Talonario de ordenes de pago, se recomienda que lo haga de manera manual", "AVISO")
            End Try

            If almacenarOrdenPago(nroCargo, Origen) Then
                Try
                    Dim Documento As New Document(PageSize.A4, 60, 5, 35, 5) 'Declaracion del documento
                    Dim parrafo As New Paragraph ' Declaracion de un parrafo
                    Dim tablademo As New PdfPTable(3) 'declara la tabla con 4 Columnas


                    pdf.PdfWriter.GetInstance(Documento, New FileStream("OrdenPago.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

                    Documento.Open() 'Abre documento para su escritura




                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("PROVINCIAL DE MISIONES" & vbCr & "SISTEMA PROVINCIAL" & vbCr & "DE TELEDUCACION Y DESARROLLO") 'Texto que se insertara
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                    Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

                    imagendemo = iTextSharp.text.Image.GetInstance("descarga.jpg") 'Dirreccion a la imagen que se hace referencia
                    imagendemo.SetAbsolutePosition(450, 750) 'Posicion en el eje cartesiano
                    imagendemo.ScaleAbsoluteWidth(100) 'Ancho de la imagen
                    imagendemo.ScaleAbsoluteHeight(100) 'Altura de la imagen
                    Documento.Add(imagendemo) ' Agrega la imagen al documento

                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("ORDEN DE PAGO NRO:" & nroCargo & "                             ") 'Texto que se insertara
                    parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("FECHA:" & Date.Now) 'Texto que se insertara

                    'Documento.Add(parrafo) 'Agrega el parrafo al documento
                    'parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    'Documento.Add(New Paragraph(" ")) 'Salto de linea
                    'Documento.Add(New Paragraph("ORDEN CARGO ASOCIADA: " & Nro, FontFactory.GetFont("Arial", 9))) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea

                    If chkDisp.Checked Then
                        Documento.Add(New Paragraph("Disposición Nro:" & txtNro.Text)) 'Salto de linea
                    End If
                    If chkRes.Checked Then
                        Documento.Add(New Paragraph("Resolución Nro:" & txtNro.Text)) 'Salto de linea
                    End If
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Dim tablademo6 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                    tablademo6.HorizontalAlignment = ALIGN_LEFT
                    tablademo6.SetWidthPercentage({100, 200, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                    tablademo6.AddCell(New Paragraph("FECHA        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                    tablademo6.AddCell(New Paragraph("COMPROBANTE")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                    tablademo6.AddCell(New Paragraph("DETALLE       ")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10


                    tablademo6.AddCell(New Paragraph("IMPORTE")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

                    Documento.Add(tablademo6) 'Agrega la tabla al documento
                    Dim tablademo4 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                    tablademo4.HorizontalAlignment = ALIGN_LEFT
                    tablademo4.SetWidthPercentage({100, 200, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

                    For Each row1 As DataGridViewRow In DataGridView1.Rows
                        For Each column As DataGridViewColumn In DataGridView1.Columns
                            If column.Index <= 4 Then
                                Dim paraParrrafo As String

                                If IsNumeric(row1.Cells(column.Index).Value) Then

                                    paraParrrafo = Format(row1.Cells(column.Index).Value, "###,##0.00")
                                    Dim miCelda2 As New PdfPCell
                                    parrafo.Add(paraParrrafo)
                                    miCelda2.AddElement(parrafo)
                                    miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                                    tablademo4.AddCell(miCelda2)
                                    parrafo.Clear()

                                Else

                                    paraParrrafo = row1.Cells(column.Index).Value
                                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                    parrafo.Add(paraParrrafo) 'Texto que se insertara
                                    tablademo4.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()

                                End If
                            End If

                        Next

                    Next
                    Documento.Add(tablademo4) 'Agrega la tabla al documento

                    Dim tablademo3 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                    tablademo3.SetWidthPercentage({100, 200, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                    tablademo3.HorizontalAlignment = ALIGN_LEFT
                    tablademo3.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

                    tablademo3.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    tablademo3.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    Dim nrito As Double
                    nrito = Double.Parse(txtTotalFinal.Text)
                    Dim paraParrafo As String
                    paraParrafo = Format(nrito, "###,##0.00")
                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente

                    Dim miCelda3 As New PdfPCell
                    parrafo.Add(paraParrafo)
                    miCelda3.AddElement(parrafo)
                    miCelda3.HorizontalAlignment = Element.ALIGN_RIGHT
                    tablademo3.AddCell(miCelda3) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    parrafo.Clear()

                    Documento.Add(tablademo3) 'Agrega la tabla al documento
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph("SON PESOS: " & Numalet.ToCardinal(txtTotalFinal.Text))) 'Salto de linea
                    Documento.Add(New Paragraph(" A CUENTA DE:  ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ALQUILER EN LA CIUDAD DE " & txtDestino.Text)) 'Salto de linea
                    Documento.Add(New Paragraph(" PERIODO " & txtPeriodo.Text)) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL")) 'Salto de linea

                    'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    'parrafo.Add("Direccion Administracion " & vbCr & "Finanzas") 'Texto que se insertara

                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente



                    Documento.Close() 'Cierra el documento
                    System.Diagnostics.Process.Start("OrdenPago.pdf") 'Abre el archivo DEMO.PDF

                Catch ex As Exception
                    MessageBox.Show("Compruebe que no tiene un documento abierto previamente", "AVISO")
                End Try
            Else
            End If
            almacenarDisp()


        End If


    End Sub
    Public Sub reemplazarImprimirAlquiler()
        Dim sql As String
        sql = "Select Id from AAOrdenNro where Tipo='ViaticoCargo' and NroIngreso=" & adjuntarAnio()
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                If reader.Read() Then
                    eliminar(reader(0))

                End If
            End Using
        End Using
        reemplazarAlquiler()
        MessageBox.Show("El remplazo se logro de manera correcta")
    End Sub
    Public Sub reemplazarAlquiler()
        Dim Origen As String
        If chkNac.Checked Then
            Origen = "OPN"
        End If
        If chkProv.Checked Then
            Origen = "OPP"
        End If

        sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='AlquilerPago'"

        Dim BANDERA As Boolean

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                If reader.Read() Then
                    If reader(0).ToString.Equals("") Or reader(0).ToString.Equals(Nothing) Then
                        BANDERA = False
                    Else
                        If IsDBNull(reader(0)) Then
                            BANDERA = False
                        Else
                            BANDERA = True
                        End If
                    End If
                Else
                    BANDERA = False
                End If

            End Using
        End Using
        If BANDERA Then
            Select Case MsgBox("¿Ya existe una orden con este numero desea reemplazarla?", MsgBoxStyle.YesNo, "AVISO")
                'en caso de seleccionar un si
                Case MsgBoxResult.Yes
                    'Elimina la tupla existente en la base de datos y despues inserta la nueva orden de pago
                    reemplazarImprimirAlquiler()
                Case MsgBoxResult.No
                    MessageBox.Show("Accion cancelada por el usuario", "INFORMACION")
            End Select

            'imprimirPagoHistorial()
        Else
            Dim nroCargo As String
            Dim nroOrden As String
            nroCargo = txtSaliente.Text
            Try
                If nroCargo.ToString.Length = 1 Then
                    nroOrden = "0000000" & nroCargo
                End If
                If nroCargo.ToString.Length = 2 Then
                    nroOrden = "000000" & nroCargo
                End If
                If nroCargo.ToString.Length = 3 Then
                    nroOrden = "00000" & nroCargo
                End If
                If nroCargo.ToString.Length = 4 Then
                    nroOrden = "0000" & nroCargo
                End If
                If nroCargo.ToString.Length = 5 Then
                    nroOrden = "000" & nroCargo
                End If
                If nroCargo.ToString.Length = 6 Then
                    nroOrden = "00" & nroCargo
                End If
                If nroCargo.ToString.Length = 7 Then
                    nroOrden = "0" & nroCargo
                End If
                If nroCargo.ToString.Length = 8 Then
                    nroOrden = nroCargo
                End If

                sql = "update Talonar set tal_ActualNro='" & nroOrden & "' where tal_Cod='" & Origen & "'"
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()

                        conn.Open()

                        cmd4.CommandText = sql
                        cmd4.ExecuteScalar()

                    End Using
                End Using
                MessageBox.Show("El talonario se actualizo de manera correcta", "EXITO")
            Catch ex As Exception
                MessageBox.Show("Un error provoco que no se actualice el Talonario de ordenes de pago, se recomienda que lo haga de manera manual", "AVISO")
            End Try

            If almacenarOrdenPago(nroCargo, Origen) Then
                Try
                    Dim Documento As New Document(PageSize.A4, 60, 5, 35, 5) 'Declaracion del documento
                    Dim parrafo As New Paragraph ' Declaracion de un parrafo
                    Dim tablademo As New PdfPTable(3) 'declara la tabla con 4 Columnas


                    pdf.PdfWriter.GetInstance(Documento, New FileStream("OrdenPago.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

                    Documento.Open() 'Abre documento para su escritura

                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("PROVINCIAL DE MISIONES" & vbCr & "SISTEMA PROVINCIAL" & vbCr & "DE TELEDUCACION Y DESARROLLO") 'Texto que se insertara
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                    Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

                    imagendemo = iTextSharp.text.Image.GetInstance("descarga.jpg") 'Dirreccion a la imagen que se hace referencia
                    imagendemo.SetAbsolutePosition(450, 750) 'Posicion en el eje cartesiano
                    imagendemo.ScaleAbsoluteWidth(100) 'Ancho de la imagen
                    imagendemo.ScaleAbsoluteHeight(100) 'Altura de la imagen
                    Documento.Add(imagendemo) ' Agrega la imagen al documento

                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("ORDEN DE PAGO NRO:" & nroCargo & "                             ") 'Texto que se insertara
                    parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("FECHA:" & Date.Now) 'Texto que se insertara

                    'Documento.Add(parrafo) 'Agrega el parrafo al documento
                    'parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    'Documento.Add(New Paragraph(" ")) 'Salto de linea
                    'Documento.Add(New Paragraph("ORDEN CARGO ASOCIADA: " & Nro, FontFactory.GetFont("Arial", 9))) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea

                    If chkDisp.Checked Then
                        Documento.Add(New Paragraph("Disposición Nro:" & txtNro.Text)) 'Salto de linea
                    End If
                    If chkRes.Checked Then
                        Documento.Add(New Paragraph("Resolución Nro:" & txtNro.Text)) 'Salto de linea
                    End If
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Dim tablademo6 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                    tablademo6.HorizontalAlignment = ALIGN_LEFT
                    tablademo6.SetWidthPercentage({100, 200, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                    tablademo6.AddCell(New Paragraph("FECHA        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                    tablademo6.AddCell(New Paragraph("COMPROBANTE")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                    tablademo6.AddCell(New Paragraph("DETALLE       ")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10


                    tablademo6.AddCell(New Paragraph("IMPORTE")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

                    Documento.Add(tablademo6) 'Agrega la tabla al documento
                    Dim tablademo4 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                    tablademo4.HorizontalAlignment = ALIGN_LEFT
                    tablademo4.SetWidthPercentage({100, 200, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

                    For Each row1 As DataGridViewRow In DataGridView1.Rows
                        For Each column As DataGridViewColumn In DataGridView1.Columns
                            If column.Index <= 4 Then
                                Dim paraParrrafo As String

                                If IsNumeric(row1.Cells(column.Index).Value) Then

                                    paraParrrafo = Format(row1.Cells(column.Index).Value, "###,##0.00")
                                    Dim miCelda2 As New PdfPCell
                                    parrafo.Add(paraParrrafo)
                                    miCelda2.AddElement(parrafo)
                                    miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                                    tablademo4.AddCell(miCelda2)
                                    parrafo.Clear()

                                Else

                                    paraParrrafo = row1.Cells(column.Index).Value
                                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                    parrafo.Add(paraParrrafo) 'Texto que se insertara
                                    tablademo4.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()

                                End If
                            End If

                        Next

                    Next
                    Documento.Add(tablademo4) 'Agrega la tabla al documento

                    Dim tablademo3 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                    tablademo3.SetWidthPercentage({100, 200, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                    tablademo3.HorizontalAlignment = ALIGN_LEFT
                    tablademo3.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

                    tablademo3.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    tablademo3.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    Dim nrito As Double
                    nrito = Double.Parse(txtTotalFinal.Text)
                    Dim paraParrafo As String
                    paraParrafo = Format(nrito, "###,##0.00")
                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente

                    Dim miCelda3 As New PdfPCell
                    parrafo.Add(paraParrafo)
                    miCelda3.AddElement(parrafo)
                    miCelda3.HorizontalAlignment = Element.ALIGN_RIGHT
                    tablademo3.AddCell(miCelda3) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    parrafo.Clear()

                    Documento.Add(tablademo3) 'Agrega la tabla al documento
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph("SON PESOS: " & Numalet.ToCardinal(txtTotalFinal.Text))) 'Salto de linea
                    Documento.Add(New Paragraph(" A CUENTA DE:  ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ALQUILER EN LA CIUDAD DE " & txtDestino.Text)) 'Salto de linea
                    Documento.Add(New Paragraph(" PERIODO " & txtPeriodo.Text)) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL")) 'Salto de linea

                    'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    'parrafo.Add("Direccion Administracion " & vbCr & "Finanzas") 'Texto que se insertara

                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente



                    Documento.Close() 'Cierra el documento
                    System.Diagnostics.Process.Start("OrdenPago.pdf") 'Abre el archivo DEMO.PDF

                Catch ex As Exception
                    MessageBox.Show("Compruebe que no tiene un documento abierto previamente", "AVISO")
                End Try
            Else
            End If
            almacenarDisp()


        End If


    End Sub

    Public Sub eliminar(ByVal id As Integer)
        Dim sql As String
        sql = "DELETE FROM AAOrdenNro WHERE Id=" & id & " "
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                cmd4.ExecuteScalar()
            End Using
        End Using
    End Sub
    Public Sub imprimirOrdenCargoCheques()
        Dim origen As String

        If chkNac.Checked Then
            origen = "OPN"
        End If
        If chkProv.Checked Then
            origen = "OPP"
        End If

        sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='AlquilerPago'"

        Dim BANDERA As Boolean

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                If reader.Read() Then
                    If reader(0).ToString.Equals("") Or reader(0).ToString.Equals(Nothing) Then
                        BANDERA = False
                    Else
                        If IsDBNull(reader(0)) Then
                            BANDERA = False
                        Else
                            BANDERA = True
                        End If
                    End If
                Else
                    BANDERA = False
                End If

            End Using
        End Using
        If BANDERA Then
            imprimirOrdenCargoChequesHistorial()
        Else
            Dim Documento As New Document(PageSize.A4, 60, 5, 35, 5) 'Declaracion del documento
            Dim parrafo As New Paragraph ' Declaracion de un parrafo
            Dim tablademo As New PdfPTable(4) 'declara la tabla con 4 Columnas
            sql = "Select tal_ActualNro from Talonar where tal_Cod='" & origen & "'"
            Dim nroCargo As String

            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()

                    conn.Open()

                    cmd4.CommandText = sql

                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    reader.Read()
                    nroCargo = reader.Item(0) + 1
                End Using
            End Using
            pdf.PdfWriter.GetInstance(Documento, New FileStream("OrdenPagoAlquiler.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

            Documento.Open() 'Abre documento para su escritura

            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("PROVINCIAL DE MISIONES" & vbCr & "SISTEMA PROVINCIAL" & vbCr & "DE TELEDUCACION Y DESARROLLO") 'Texto que se insertara
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

            Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

            imagendemo = iTextSharp.text.Image.GetInstance("descarga.jpg") 'Dirreccion a la imagen que se hace referencia
            imagendemo.SetAbsolutePosition(450, 750) 'Posicion en el eje cartesiano
            imagendemo.ScaleAbsoluteWidth(100) 'Ancho de la imagen
            imagendemo.ScaleAbsoluteHeight(100) 'Altura de la imagen
            Documento.Add(imagendemo) ' Agrega la imagen al documento

            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("ORDEN DE PAGO NRO:" & dameNroHistorial("AlquilerPago") & "                             ") 'Texto que se insertara
            parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("FECHA:" & Date.Now) 'Texto que se insertara

            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Dim res As String
            Try
                sql = "Select Disposicion from AAOrdenNro where Tipo='AlquilerPago' and NroIngreso=" & adjuntarAnio()
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()

                        conn.Open()

                        cmd4.CommandText = sql

                        Dim reader As SqlDataReader = Nothing
                        reader = cmd4.ExecuteReader
                        If reader.Read() Then
                            res = reader(0)
                            Documento.Add(New Paragraph("Disposición Nro:" & res)) 'Salto de linea
                        End If
                    End Using
                End Using
            Catch ex As Exception

            End Try
            Try
                sql = "Select Resolucion from AAOrdenNro where Tipo='AlquilerPago' and NroIngreso=" & adjuntarAnio()
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()

                        conn.Open()

                        cmd4.CommandText = sql

                        Dim reader As SqlDataReader = Nothing
                        reader = cmd4.ExecuteReader
                        If reader.Read() Then
                            res = reader(0)
                            Documento.Add(New Paragraph("Resolución Nro:" & res)) 'Salto de linea
                        End If
                    End Using
                End Using

            Catch ex As Exception

            End Try



            Documento.Add(New Paragraph(" ")) 'Salto de linea

            'tablademo.SetWidthPercentage({270, 100, 80, 120}, PageSize.A4) 'Ajusta el tamaño de cada columna

            'tablademo.AddCell(New Paragraph("IMPORTE EN LETRAS EXPRESADOS EN PESOS", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            'tablademo.AddCell(New Paragraph("CUENTA CORRIENTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
            'tablademo.AddCell(New Paragraph("NRO DE CHEQUE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
            'tablademo.AddCell(New Paragraph("DESTINATARIO", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

            'Documento.Add(tablademo) 'Agrega la tabla al documento

            'Dim tablademo2 As New PdfPTable(4) 'declara la tabla con 4 Columnas
            'tablademo2.SetWidthPercentage({270, 100, 80, 120}, PageSize.A4) 'Ajusta el tamaño de cada columna

            'For Each row As DataGridViewRow In DataGridView1.Rows
            '    For Each column As DataGridViewColumn In DataGridView1.Columns
            '        If column.Index <= 4 Then
            '            tablademo2.AddCell(New Paragraph(row.Cells(column.Index).Value, FontFactory.GetFont("Arial", 10))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            '        End If

            '    Next

            'Next
            'Documento.Add(tablademo2) 'Agrega la tabla al documento

            'Documento.Add(New Paragraph(" ")) 'Salto de linea
            'Documento.Add(New Paragraph(" ")) 'Salto de linea

            'Documento.Add(New Paragraph(" ")) 'Salto de linea

            Dim tablademo5 As New PdfPTable(5) 'declara la tabla con 4 Columnas
            tablademo5.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tablademo5.AddCell(New Paragraph("F. EMISION        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

            tablademo5.AddCell(New Paragraph("BENEFICIARIO        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            tablademo5.AddCell(New Paragraph("NRO CTA        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            tablademo5.AddCell(New Paragraph("CH. Y/O AUT.        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

            tablademo5.AddCell(New Paragraph("IMPORTE        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            Documento.Add(tablademo5) 'Agrega la tabla al documento

            Dim tablademo4 As New PdfPTable(5) 'declara la tabla con 4 Columnas
            tablademo4.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
            For Each row1 As DataGridViewRow In DataGridView2.Rows
                For Each column As DataGridViewColumn In DataGridView2.Columns
                    If column.Index <= 5 Then
                        Dim paraParrrafo As String
                        If IsNumeric(row1.Cells(column.Index).Value) And Not column.Name.Equals("cheques") Then

                            paraParrrafo = Format(row1.Cells(column.Index).Value, "###,##0.00")
                            Dim miCelda2 As New PdfPCell
                            parrafo.Add(paraParrrafo)
                            miCelda2.AddElement(parrafo)

                            miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                            tablademo4.AddCell(miCelda2)
                            parrafo.Clear()
                        Else
                            paraParrrafo = row1.Cells(column.Index).Value
                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                            parrafo.Add(paraParrrafo) 'Texto que se insertara
                            tablademo4.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            parrafo.Clear()
                        End If
                    End If

                Next

            Next
            Documento.Add(tablademo4) 'Agrega la tabla al documento
            Dim tabla9 As New PdfPTable(5)
            'tabla9.HorizontalAlignment = ALIGN_LEFT
            tabla9.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tabla9.AddCell("TOTAL")
            tabla9.AddCell(" ")
            tabla9.AddCell(" ")
            tabla9.AddCell(" ")
            Dim miParrafo As String
            Dim miTotal As Double
            miTotal = Double.Parse(txtTotalFinal.Text)
            miParrafo = Format(miTotal, "###,##0.00")
            Dim miCelda As New PdfPCell
            parrafo.Add(miParrafo)
            miCelda.AddElement(parrafo)

            miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
            tabla9.AddCell(miCelda)
            Documento.Add(tabla9)
            parrafo.Clear()
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Dim subTotal As Double


            subTotal = Double.Parse(txtTotalFinal.Text)

            Dim miGranTotal As String
            miGranTotal = Format(subTotal, "###,##0.00")
            Documento.Add(New Paragraph("Total:$" & miGranTotal & " (Pesos " & Numalet.ToCardinal(subTotal) & ")")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph("Observaciones"))
            Documento.Add(New Paragraph(" ")) 'Salto de linea

            Dim tablademo6 As New PdfPTable(2) 'declara la tabla con 4 Columnas
            tablademo6.SetWidthPercentage({150, 400}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tablademo6.AddCell(New Paragraph("        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo6.AddCell(New Paragraph("NOTA:" & vbCr & "        " & vbCr & "        " & vbCr & "        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            Documento.Add(tablademo6)
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("RECIBI CONFORME EL IMPORTE:________________ " & vbCr & "ACLARACION DE FIRMA:________________________") 'Texto que se insertara
            Documento.Add(New Paragraph(" "))
            Documento.Add(New Paragraph(" "))
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("DEPTO. TESORERIA") 'Texto que se insertara

            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente


            Documento.Close() 'Cierra el documento
            System.Diagnostics.Process.Start("OrdenPagoAlquiler.pdf") 'Abre el archivo DEMO.PDF

            If nroCargo.ToString.Length = 1 Then
                nroCargo = "0000000" & nroCargo
            End If
            If nroCargo.ToString.Length = 2 Then
                nroCargo = "000000" & nroCargo
            End If
            If nroCargo.ToString.Length = 3 Then
                nroCargo = "00000" & nroCargo
            End If
            If nroCargo.ToString.Length = 4 Then
                nroCargo = "0000" & nroCargo
            End If
            If nroCargo.ToString.Length = 5 Then
                nroCargo = "000" & nroCargo
            End If
            If nroCargo.ToString.Length = 6 Then
                nroCargo = "00" & nroCargo
            End If
            If nroCargo.ToString.Length = 7 Then
                nroCargo = "0" & nroCargo
            End If
            If nroCargo.ToString.Length = 8 Then
                nroCargo = nroCargo
            End If

            sql = "update Talonar set tal_ActualNro='" & nroCargo & "' where tal_Cod='" & origen & "'"
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()

                    conn.Open()

                    cmd4.CommandText = sql
                    cmd4.ExecuteScalar()

                End Using
            End Using

            almacenarOrden(nroCargo, origen)
        End If



    End Sub

#End Region

#Region "HISTORIAL"

    Public Sub imprimirOrdenCargoChequesHistorial()
        Dim fechona As String
        sql = "Select Concepto,Fecha from AAOrdenNro where Tipo='Alquiler' and NroIngreso=" & adjuntarAnio()
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                If reader.Read() Then
                    fechona = reader(1)
                End If
            End Using
        End Using

        Dim Documento As New Document(PageSize.A4, 60, 5, 35, 5) 'Declaracion del documento
        Dim parrafo As New Paragraph ' Declaracion de un parrafo
        Dim tablademo As New PdfPTable(4) 'declara la tabla con 4 Columnas
        Dim nroPago As String
        nroPago = dameNroTalonario()
        pdf.PdfWriter.GetInstance(Documento, New FileStream("OrdenCargo.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

        Documento.Open() 'Abre documento para su escritura

        parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("PROVINCIAL DE MISIONES" & vbCr & "SISTEMA PROVINCIAL" & vbCr & "DE TELEDUCACION Y DESARROLLO") 'Texto que se insertara
        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

        Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

        imagendemo = iTextSharp.text.Image.GetInstance("descarga.jpg") 'Dirreccion a la imagen que se hace referencia
        imagendemo.SetAbsolutePosition(450, 750) 'Posicion en el eje cartesiano
        imagendemo.ScaleAbsoluteWidth(100) 'Ancho de la imagen
        imagendemo.ScaleAbsoluteHeight(100) 'Altura de la imagen
        Documento.Add(imagendemo) ' Agrega la imagen al documento

        parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("ORDEN DE PAGO NRO:" & dameNroHistorial("AlquilerPago") & "                             ") 'Texto que se insertara
        parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("FECHA:" & fechona) 'Texto que se insertara

        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Dim res As String
        Try
            sql = "Select Disposicion from AAOrdenNro where Tipo='AlquilerPago' and NroIngreso=" & adjuntarAnio()
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()

                    conn.Open()

                    cmd4.CommandText = sql

                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    If reader.Read() Then
                        res = reader(0)
                        Documento.Add(New Paragraph("Disposición Nro:" & res)) 'Salto de linea
                    End If
                End Using
            End Using
        Catch ex As Exception

        End Try
        Try
            sql = "Select Resolucion from AAOrdenNro where Tipo='AlquilerPago' and NroIngreso=" & adjuntarAnio()
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()

                    conn.Open()

                    cmd4.CommandText = sql

                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    If reader.Read() Then
                        res = reader(0)
                        Documento.Add(New Paragraph("Resolución Nro:" & res)) 'Salto de linea
                    End If
                End Using
            End Using

        Catch ex As Exception

        End Try


        Documento.Add(New Paragraph(" ")) 'Salto de linea

        'tablademo.SetWidthPercentage({270, 100, 80, 120}, PageSize.A4) 'Ajusta el tamaño de cada columna

        'tablademo.AddCell(New Paragraph("IMPORTE EN LETRAS EXPRESADOS EN PESOS", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        'tablademo.AddCell(New Paragraph("CUENTA CORRIENTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
        'tablademo.AddCell(New Paragraph("NRO DE CHEQUE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
        'tablademo.AddCell(New Paragraph("DESTINATARIO", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        'Documento.Add(tablademo) 'Agrega la tabla al documento

        'Dim tablademo2 As New PdfPTable(4) 'declara la tabla con 4 Columnas
        'tablademo2.SetWidthPercentage({270, 100, 80, 120}, PageSize.A4) 'Ajusta el tamaño de cada columna

        'For Each row As DataGridViewRow In DataGridView1.Rows
        '    For Each column As DataGridViewColumn In DataGridView1.Columns
        '        If column.Index <= 4 Then
        '            tablademo2.AddCell(New Paragraph(row.Cells(column.Index).Value, FontFactory.GetFont("Arial", 10))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        '        End If

        '    Next

        'Next
        'Documento.Add(tablademo2) 'Agrega la tabla al documento

        'Documento.Add(New Paragraph(" ")) 'Salto de linea
        'Documento.Add(New Paragraph(" ")) 'Salto de linea

        'Documento.Add(New Paragraph(" ")) 'Salto de linea

        Dim tablademo5 As New PdfPTable(5) 'declara la tabla con 4 Columnas
        tablademo5.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo5.AddCell(New Paragraph("F. EMISION        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

        tablademo5.AddCell(New Paragraph("BENEFICIARIO        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo5.AddCell(New Paragraph("NRO CTA        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo5.AddCell(New Paragraph("CH. Y/O AUT.        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        tablademo5.AddCell(New Paragraph("IMPORTE        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        Documento.Add(tablademo5) 'Agrega la tabla al documento

        Dim tablademo4 As New PdfPTable(5) 'declara la tabla con 4 Columnas
        tablademo4.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        For Each row1 As DataGridViewRow In DataGridView2.Rows
            For Each column As DataGridViewColumn In DataGridView2.Columns
                If column.Index <= 5 Then
                    Dim paraParrrafo As String
                    If IsNumeric(row1.Cells(column.Index).Value) And Not column.Name.Equals("cheques") Then

                        paraParrrafo = Format(row1.Cells(column.Index).Value, "###,##0.00")
                        Dim miCelda2 As New PdfPCell
                        parrafo.Add(paraParrrafo)
                        miCelda2.AddElement(parrafo)

                        miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                        tablademo4.AddCell(miCelda2)
                        parrafo.Clear()
                    Else
                        paraParrrafo = row1.Cells(column.Index).Value
                        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                        parrafo.Add(paraParrrafo) 'Texto que se insertara
                        tablademo4.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                        parrafo.Clear()
                    End If
                End If

            Next

        Next
        Documento.Add(tablademo4) 'Agrega la tabla al documento
        Dim tabla9 As New PdfPTable(5)
        'tabla9.HorizontalAlignment = ALIGN_LEFT
        tabla9.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tabla9.AddCell("TOTAL")
        tabla9.AddCell(" ")
        tabla9.AddCell(" ")
        tabla9.AddCell(" ")
        Dim miParrafo As String
        Dim miTotal As Double
        miTotal = Double.Parse(txtTotalFinal.Text)
        miParrafo = Format(miTotal, "###,##0.00")
        Dim miCelda As New PdfPCell
        parrafo.Add(miParrafo)
        miCelda.AddElement(parrafo)

        miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
        tabla9.AddCell(miCelda)
        Documento.Add(tabla9)
        parrafo.Clear()
        Documento.Add(New Paragraph(" ")) 'Salto de linea

        Dim subTotal As Double


        subTotal = Double.Parse(txtTotalFinal.Text)

        Dim miGranTotal As String
        miGranTotal = Format(subTotal, "###,##0.00")
        Documento.Add(New Paragraph("Total:$" & miGranTotal & " (Pesos " & Numalet.ToCardinal(subTotal) & ")")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph("Observaciones"))
        Documento.Add(New Paragraph(" ")) 'Salto de linea

        Dim tablademo6 As New PdfPTable(2) 'declara la tabla con 4 Columnas
        tablademo6.SetWidthPercentage({150, 400}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo6.AddCell(New Paragraph("        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        tablademo6.AddCell(New Paragraph("NOTA:" & vbCr & "        " & vbCr & "        " & vbCr & "        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        Documento.Add(tablademo6)
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("RECIBI CONFORME EL IMPORTE:________________ " & vbCr & "ACLARACION DE FIRMA:________________________") 'Texto que se insertara
        Documento.Add(New Paragraph(" "))
        Documento.Add(New Paragraph(" "))
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("DEPTO. TESORERIA") 'Texto que se insertara

        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente


        Documento.Close() 'Cierra el documento
        System.Diagnostics.Process.Start("OrdenCargo.pdf") 'Abre el archivo DEMO.PDF

    End Sub

    Public Sub imprimirPagoHistorial()
        Dim sql As String
        Dim fechona As String
        Dim Nro As String
        sql = "Select Concepto,Fecha from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='Alquiler' "
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                If reader.Read() Then
                    Nro = reader(0)
                    fechona = reader(1)
                End If
            End Using
        End Using
        Dim Documento As New Document(PageSize.A4, 60, 5, 35, 5) 'Declaracion del documento
        Dim parrafo As New Paragraph ' Declaracion de un parrafo
        Dim tablademo As New PdfPTable(3) 'declara la tabla con 4 Columnas

        pdf.PdfWriter.GetInstance(Documento, New FileStream("OrdenPago.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

        Documento.Open() 'Abre documento para su escritura

        parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("PROVINCIAL DE MISIONES" & vbCr & "SISTEMA PROVINCIAL" & vbCr & "DE TELEDUCACION Y DESARROLLO") 'Texto que se insertara
        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

        Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

        imagendemo = iTextSharp.text.Image.GetInstance("descarga.jpg") 'Dirreccion a la imagen que se hace referencia
        imagendemo.SetAbsolutePosition(450, 750) 'Posicion en el eje cartesiano
        imagendemo.ScaleAbsoluteWidth(100) 'Ancho de la imagen
        imagendemo.ScaleAbsoluteHeight(100) 'Altura de la imagen
        Documento.Add(imagendemo) ' Agrega la imagen al documento

        parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("ORDEN DE PAGO NRO:" & dameNroHistorial("AlquilerPago") & "                             ") 'Texto que se insertara
        parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("FECHA:" & fechona) 'Texto que se insertara

        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Dim res As String
        Try
            sql = "Select Disposicion from AAOrdenNro where Tipo='AlquilerPago' and NroIngreso=" & adjuntarAnio()
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql
                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    If reader.Read() Then
                        res = reader(0)
                        Documento.Add(New Paragraph("Disposición Nro:" & res)) 'Salto de linea
                    End If
                End Using
            End Using
        Catch ex As Exception

        End Try
        Try
            sql = "Select Resolucion from AAOrdenNro where Tipo='AlquilerPago' and NroIngreso=" & adjuntarAnio()
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()

                    conn.Open()

                    cmd4.CommandText = sql

                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    If reader.Read() Then
                        res = reader(0)
                        Documento.Add(New Paragraph("Resolución Nro:" & res)) 'Salto de linea
                    End If
                End Using
            End Using
        Catch ex As Exception

        End Try



        Documento.Add(New Paragraph(" ")) 'Salto de linea

        Dim tablademo6 As New PdfPTable(4) 'declara la tabla con 4 Columnas
        tablademo6.HorizontalAlignment = ALIGN_LEFT
        tablademo6.SetWidthPercentage({100, 200, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo6.AddCell(New Paragraph("FECHA        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        tablademo6.AddCell(New Paragraph("COMPROBANTE")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
        tablademo6.AddCell(New Paragraph("DETALLE       ")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10


        tablademo6.AddCell(New Paragraph("IMPORTE")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        Documento.Add(tablademo6) 'Agrega la tabla al documento
        Dim tablademo4 As New PdfPTable(4) 'declara la tabla con 4 Columnas
        tablademo4.HorizontalAlignment = ALIGN_LEFT
        tablademo4.SetWidthPercentage({100, 200, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

        For Each row1 As DataGridViewRow In DataGridView1.Rows
            For Each column As DataGridViewColumn In DataGridView1.Columns
                If column.Index <= 4 Then
                    Dim paraParrrafo As String

                    If IsNumeric(row1.Cells(column.Index).Value) Then

                        paraParrrafo = Format(row1.Cells(column.Index).Value, "###,##0.00")
                        Dim miCelda2 As New PdfPCell
                        parrafo.Add(paraParrrafo)
                        miCelda2.AddElement(parrafo)
                        miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                        tablademo4.AddCell(miCelda2)
                        parrafo.Clear()

                    Else

                        paraParrrafo = row1.Cells(column.Index).Value
                        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                        parrafo.Add(paraParrrafo) 'Texto que se insertara
                        tablademo4.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                        parrafo.Clear()

                    End If
                End If

            Next

        Next
        Documento.Add(tablademo4) 'Agrega la tabla al documento

        Dim tablademo3 As New PdfPTable(4) 'declara la tabla con 4 Columnas
        tablademo3.SetWidthPercentage({100, 200, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo3.HorizontalAlignment = ALIGN_LEFT
        tablademo3.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

        tablademo3.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo3.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        Dim nrito As Double
        nrito = Double.Parse(txtTotalFinal.Text)
        Dim paraParrafo As String
        paraParrafo = Format(nrito, "###,##0.00")
        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente

        Dim miCelda3 As New PdfPCell
        parrafo.Add(paraParrafo)
        miCelda3.AddElement(parrafo)
        miCelda3.HorizontalAlignment = Element.ALIGN_RIGHT
        tablademo3.AddCell(miCelda3) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        parrafo.Clear()

        Documento.Add(tablademo3) 'Agrega la tabla al documento
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph("SON PESOS: " & Numalet.ToCardinal(txtTotalFinal.Text))) 'Salto de linea
        Documento.Add(New Paragraph(" A CUENTA DE:  ")) 'Salto de linea
        Documento.Add(New Paragraph(" ALQUILER EN LA CIUDAD DE " & txtDestino.Text)) 'Salto de linea
        Documento.Add(New Paragraph(" PERIODO: " & txtPeriodo.Text)) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL")) 'Salto de linea

        'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        'parrafo.Add("Direccion Administracion " & vbCr & "Finanzas") 'Texto que se insertara

        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

        Documento.Close() 'Cierra el documento
        System.Diagnostics.Process.Start("OrdenPago.pdf") 'Abre el archivo DEMO.PDF
    End Sub

    Private Function dameNroHistorial(ByRef Tipo As String) As String
        Dim Nro As String
        sql = "Select Concepto from AAOrdenNro where Tipo='" & Tipo & "' and NroIngreso=" & adjuntarAnio()
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                If reader.Read() Then
                    Nro = reader(0)
                End If
            End Using
        End Using
        Return Nro
    End Function

#End Region

    Private Sub btnBuscar_Click(sender As System.Object, e As System.EventArgs) Handles btnBuscar.Click
        DataGridView2.Rows.Clear()
        DataGridView2.Columns.Clear()
        DataGridView1.Rows.Clear()
        DataGridView1.Columns.Clear()
        If txtCodigo.Text = Nothing Then
            MessageBox.Show("Debe ingresar un codigo valido", "AVISO")
        Else
            'Try
            If cmbTipoOrden.SelectedItem = "Alquileres" Then
                If chkAutorizado.Checked Then
                    ordenCargo()
                    GroupBox5.Visible = True
                    cargarDataRendicion()
                Else
                    cargarDataAlquiler()

                End If
            Else
                If cmbTipoOrden.SelectedItem = "Historial Alquileres" Then
                    If chkAutorizado.Checked Then
                        ordenCargo()
                        GroupBox5.Visible = True
                        cargarDataRendicion()
                    Else
                        cargarDataAlquiler()

                    End If
                Else
                    MessageBox.Show("Debe seleccionar un tipo de orden valido", "AVISO")
                End If

            End If
            'Catch ex As Exception
            '    MessageBox.Show("Por Favor Verifique el número de entrada", "ERROR")
            'End Try
        End If
    End Sub

    Private Sub btnSalir_Click(sender As System.Object, e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub ActualizarAlquiler_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        cargarCombo()
        GroupBox5.Visible = False
        cargarComboAnio()
    End Sub

    Private Sub btnImprimir_Click(sender As System.Object, e As System.EventArgs) Handles btnImprimir.Click
        If chkNac.Checked Or chkProv.Checked Then
            If txtCodigo.Text = Nothing Then
                MessageBox.Show("Debe ingresar un codigo valido", "AVISO")
            Else
                If cmbTipoOrden.Text.Equals("Alquileres") Then
                    If chkDisp.Checked Or chkRes.Checked Then
                        If chkAutorizado.Checked Then
                            imprimirOrdenCargoCheques()
                        Else
                            'Select Case MsgBox("¿Esta seguro que desea generar un pago?", MsgBoxStyle.YesNo, "AVISO")
                            '   Case MsgBoxResult.Yes
                            'cargarApertura()
                            imprimirPago()
                            '    Case MsgBoxResult.No
                            'MessageBox.Show("Accion cancelada por el usuario", "INFORMACION")
                            'End Select
                        End If
                    Else
                        MessageBox.Show("Debe seleccionar una opcion", "DISPOSICION O RESOLUCION??")
                    End If
                Else
                    If cmbTipoOrden.Text.Equals("Historial Alquileres") Then
                        If chkAutorizado.Checked Then
                            imprimirOrdenCargoChequesHistorial()
                        Else
                            imprimirPagoHistorial()
                        End If
                    Else
                        MessageBox.Show("Debe seleccionar un tipo de orden", "AVISO")
                    End If

                End If
            End If
        Else
            MessageBox.Show("Debe seleccionar una opcion", "NACIONAL O PROVINCIAL??")
        End If
    End Sub

    Private Sub cargarComboAnio()
        sql = "select Descripcion from AAanio"

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader

                Do While reader.Read()
                    Dim fila(1) As Object

                    fila(0) = reader(0)
                    cmbA.Items.Add(fila(0))

                Loop

            End Using
        End Using
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim I As Integer = DataGridView1.CurrentCell.RowIndex
        ActualizarNro.txtNroPago.Text = txtCodigo.Text
        If ((chkAutorizado.Checked) And (cmbTipoOrden.Text.Equals("Alquileres"))) Then
            ActualizarNro.txtDestinatario.Text = DataGridView1.Rows(I).Cells(3).Value.ToString
            ActualizarNro.txtNroCuenta.Text = DataGridView1.Rows(I).Cells(1).Value.ToString
            ActualizarNro.txtAnteriorNroCheque.Text = DataGridView1.Rows(I).Cells(2).Value.ToString
            ActualizarNro.Show()
        Else
            'ActualizarNro.txtNroCuenta.Text = DataGridView1.Rows(I).Cells(1).Value.ToString
            'ActualizarNro.txtAnteriorNroCheque.Text = DataGridView1.Rows(I).Cells(2).Value.ToString
            'ActualizarNro.txtDestinatario.Text = DataGridView1.Rows(I).Cells(3).Value.ToString

        End If
    End Sub

    Private Sub DataGridView1_DoubleClick(sender As Object, e As System.EventArgs) Handles DataGridView1.DoubleClick
        Dim I As Integer = DataGridView1.CurrentCell.RowIndex
        ActualizarNro.txtNroPago.Text = txtCodigo.Text
        If ((chkAutorizado.Checked) And (cmbTipoOrden.Text.Equals("Alquileres"))) Then
            ActualizarNro.txtDestinatario.Text = DataGridView1.Rows(I).Cells(3).Value.ToString
            ActualizarNro.txtNroCuenta.Text = DataGridView1.Rows(I).Cells(1).Value.ToString
            ActualizarNro.txtAnteriorNroCheque.Text = DataGridView1.Rows(I).Cells(2).Value.ToString
            ActualizarNro.Show()
        Else
            'ActualizarNro.txtNroCuenta.Text = DataGridView1.Rows(I).Cells(1).Value.ToString
            'ActualizarNro.txtAnteriorNroCheque.Text = DataGridView1.Rows(I).Cells(2).Value.ToString
            'ActualizarNro.txtDestinatario.Text = DataGridView1.Rows(I).Cells(3).Value.ToString

        End If
    End Sub
End Class