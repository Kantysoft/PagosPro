Imports System.Data.SqlClient
Imports System.Data.Odbc
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.Image
Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet

Public Class ActualizarContrato
    Dim sql As String
    Dim sql2 As String
    Dim sql3 As String
    Dim sql4 As String


    Dim da As New OdbcDataAdapter
    Dim da2 As New OdbcDataAdapter
    Dim da3 As New OdbcDataAdapter

    Dim anio As String
    Dim indiceRow As String
    Dim indiceColumn As String
    Dim ccoMf As String

    Private Sub btnSalir_Click(sender As System.Object, e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
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
                    cmbAnio.Items.Add(fila(0))

                Loop

            End Using
        End Using

    End Sub


    Private Sub btnImprimir_Click(sender As System.Object, e As System.EventArgs) Handles btnImprimir.Click
        If chkNac.Checked Or chkProv.Checked Then
            If chkDisp.Checked Or chkRes.Checked Then
                If chkHistorial.Checked Or ChkNuevo.Checked Then
                    If chkAutorizado.Checked Then
                        imprimirOrdenCargoCheques()
                    Else
                        imprimirPago()
                    End If
                Else
                    MessageBox.Show("Debe seleccionar una opcion", "Nuevo o Historial??")
                End If
            Else
                MessageBox.Show("Debe seleccionar una opcion", "RESOLUCION O DISPOSICION??")
            End If
        Else
            MessageBox.Show("Debe seleccionar una opcion", "NACIONAL O PROVINCIAL??")
        End If

    End Sub

    Private Sub btnBuscar_Click(sender As System.Object, e As System.EventArgs) Handles btnBuscar.Click
        If txtCodigo.Text = "" Then
            MessageBox.Show("Debe ingresar un codigo valido", "AVISO")
        Else
            If ChkNuevo.Checked Then
                If chkAutorizado.Checked Then
                    DataGridView1.Columns.Clear()
                    DataGridView3.Columns.Clear()
                    DataGridView1.Rows.Clear()
                    DataGridView3.Rows.Clear()
                    cargarDataRendicion()
                    cargardataTransferencias()
                    generarRendicion2()
                    CargarRetencion()
                    GroupBox3.Text = "Transferencia:"
                Else
                    DataGridView1.Columns.Clear()
                    DataGridView1.Rows.Clear()
                    DataGridView2.Columns.Clear()
                    DataGridView2.Rows.Clear()
                    DataGridView3.Columns.Clear()
                    DataGridView3.Rows.Clear()
                    DataGridView4.Columns.Clear()
                    DataGridView4.Rows.Clear()
                    'MessageBox.Show("tetas")
                    cargarNombresRendicion()

                    GroupBox3.Visible = True
                    GroupBox3.Text = "Impuesto al Sello:"
                    generarRendicion2()
                End If
            Else
                If chkHistorial.Checked Then

                    DataGridView1.Columns.Clear()
                    DataGridView1.Rows.Clear()
                    DataGridView2.Columns.Clear()
                    DataGridView2.Rows.Clear()
                    DataGridView3.Columns.Clear()
                    DataGridView3.Rows.Clear()
                    DataGridView4.Columns.Clear()
                    DataGridView4.Rows.Clear()
                    traerHistorico()
                    generarRendicion2()

                Else
                    MessageBox.Show("Debe seleccionar si desea generar un nuevo pago o bien imprimir un historico", "AVISO")
                End If
            End If

            'Catch ex As Exception
            '    MessageBox.Show("Por favor verifique el numero de entrada", "ERROR")
            'End Try

        End If
    End Sub

    Public Sub cargardataTransferencias()
        GroupBox3.Visible = True
        'DataGridView2.Rows.Clear()
        'DataGridView2.Columns.Clear()
        'DataGridView4.Columns.Clear()
        sql = "SELECT mfocmf_FMov,mfoctb_Cod,pro_RazSoc,mfo_ImpMonElem   FROM [SBDASIPT].[dbo].[CabMovF] inner join SBDASIPT.dbo.CabCompra on CabMovF.cmf_ID=CabCompra.ccocmf_ID " &
              "inner join [SBDASIPT].dbo.CausaEmi on CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
              "inner join [SBDASIPT].[dbo].[MovF] on  [CabMovF].cmf_ID=[MovF].mfocmf_ID " &
              "inner join [SBDASIPT].[dbo].[Proveed] on MovF.mfopro_Cod=Proveed.pro_Cod " &
              "where CausaEmi.cem_Desc='" & adjuntarAnio() & "' and mfobco_cod not like 'NULL'"
        'DataGridView2.Columns.Add("fecha", "FECHA EMISION")
        'DataGridView2.Columns.Add("destinatario", "BENEFICIARIO")
        'DataGridView2.Columns.Add("ctaCte", "CUENTA CORRIENTE")
        'DataGridView2.Columns.Add("dias", "NRO CHEQUE")
        'DataGridView2.Columns.Add("importe", "IMPORTE")
        'DataGridView2.Columns.Item("IMPORTE").DefaultCellStyle.Format = "#####0.00"
        'DataGridView4.Columns.Add("fecha", "FECHA EMISION")
        'DataGridView4.Columns.Add("destinatario", "BENEFICIARIO")
        'DataGridView4.Columns.Add("ctaCte", "CUENTA CORRIENTE")
        'DataGridView4.Columns.Add("dias", "NRO CHEQUE")
        'DataGridView4.Columns.Add("importe", "IMPORTE")
        'DataGridView4.Columns.Item("IMPORTE").DefaultCellStyle.Format = "#####0.00"
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
                    fila(0) = reader(0)
                    fila(1) = reader(2)
                    fila(2) = reader(1)
                    fila(3) = "AUTORIZACION"
                    fila(4) = reader(3) * -1
                    DataGridView1.Rows.Add(fila)
                Loop
            End Using
        End Using
    End Sub

    Private Sub MisContratos_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        'GroupBox3.Visible = False
        'txtSubTotal.Visible = False
        'Label4.Visible = False
        'txtSubSellos.Visible = False
        'Label5.Visible = False
        cargarComboAnio()

        GroupBox8.Visible = False
        historialOrden.Visible = False

    End Sub

#Region " SIN AUTORIZAR SIN SELLOS"
    Public Sub generarRendicion()

        DataGridView1.Columns.Add("detalle", "DETALLE")
        DataGridView1.Columns.Add("Imputacion", "IMPUTACION")
        DataGridView1.Columns.Add("factura", "FACTURA")
        DataGridView3.Columns.Add("detalle", "DETALLE")
        DataGridView3.Columns.Add("banco", "BANCO")
        DataGridView3.Columns.Add("factura", "FACTURA")
        DataGridView1.Columns.Add("monto", "MONTO")
        DataGridView1.Columns.Item("MONTO").DefaultCellStyle.Format = "#####0.00"
        DataGridView3.Columns.Add("monto", "MONTO")
        DataGridView3.Columns.Item("MONTO").DefaultCellStyle.Format = "#####0.00"
        DataGridView1.Columns.Add("Emitido", "EMITIDO")

        sql = " SELECT distinct  ItemComp.ico_Desc FROM CabCompra  INNER JOIN CausaEmi ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod INNER JOIN  ItemComp ON CabCompra.cco_ID=ItemComp.icocco_ID  WHERE  CausaEmi.cem_Desc='" & adjuntarAnio() & "' and ItemComp.ico_Desc not like 'Impuesto%'  ORDER BY ItemComp.ico_Desc"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
            End Using
        End Using

        cargarNombresRendicion()

    End Sub

    Public Sub cargarNombresRendicion()
        DataGridView1.Columns.Add("detalle", "DETALLE")
        DataGridView1.Columns.Add("Imputacion", "IMPUTACION")
        DataGridView1.Columns.Add("factura", "FACTURA")
        DataGridView3.Columns.Add("detalle", "DETALLE")
        DataGridView3.Columns.Add("banco", "BANCO")
        DataGridView3.Columns.Add("factura", "FACTURA")
        DataGridView1.Columns.Add("monto", "MONTO")
        DataGridView1.Columns.Item("MONTO").DefaultCellStyle.Format = "#####0.00"
        DataGridView3.Columns.Add("monto", "MONTO")
        DataGridView3.Columns.Item("MONTO").DefaultCellStyle.Format = "#####0.00"
        DataGridView1.Columns.Add("cuit", "CUIT")
        DataGridView1.Columns.Add("Emitido", "EMITIDO")

        sql = "SELECT  distinct CabCompra.ccopro_RazSoc,[ccopro_CUIT] FROM [SBDASIPT].dbo.CabCompra  INNER JOIN [SBDASIPT].dbo.CausaEmi ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod INNER JOIN  [SBDASIPT].dbo.ItemComp ON CabCompra.cco_ID=ItemComp.icocco_ID  WHERE   CausaEmi.cem_Desc='" & adjuntarAnio() & "' group BY CabCompra.ccopro_RazSoc,CabCompra.[ccopro_CUIT]"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Do While reader.Read()
                    Dim fila(12) As Object
                    fila(0) = reader(0) & " CUIT:" & reader(1)
                    fila(4) = reader(1)
                    DataGridView1.Rows.Add(fila)

                Loop
            End Using
        End Using

        adjuntarDetalle()

        'adjuntarDatosRendicion()

    End Sub

    Public Sub adjuntarDetalle()
        Dim sql5 As String
        Dim cabecera As String
        For Each row As DataGridViewRow In DataGridView1.Rows
            sql4 = "SELECT CabCompra.ccopro_RazSoc ,[cco_Nro],CabCompra.[ccopro_CUIT],ItemComp.ico_Desc,[ico_NetoLoc],cco_CodPro " +
        "FROM [SBDASIPT].[dbo].[CabCompra]  " +
        "inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID  " +
        "inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod " +
        "where [CausaEmi].cem_Desc='" & adjuntarAnio() & "' and ([ccotco_Cod] <>'OP'  and [ccotco_Cod] <>'SLL') " +
        "group by CabCompra.ccopro_RazSoc ,[cco_Nro],CabCompra.[ccopro_CUIT],ItemComp.ico_Desc,[ico_NetoLoc],cco_CodPro " +
            "Except " +
            "SELECT CabCompra.ccopro_RazSoc ,[cco_Nro],CabCompra.[ccopro_CUIT],ItemComp.ico_Desc,[ico_NetoLoc],cco_CodPro " +
        "FROM [SBDASIPT].[dbo].[CabCompra]  " +
        "inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID  " +
        "inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod " +
        "inner join [SBDASIPT].dbo.[RelCompCpr] on CabCompra.cco_ID=RelCompCpr.rcccco_IDCol1 " +
        "inner join [SBDASIPT].dbo.SegCabC on SegCabC.scc_ID=RelCompCpr.rcccco_IDCol2 " +
        "where [CausaEmi].cem_Desc='" & adjuntarAnio() & "' and ([ccotco_Cod] ='OP'  and [ccotco_Cod] ='SLL') " +
        "group by CabCompra.ccopro_RazSoc ,[cco_Nro],CabCompra.[ccopro_CUIT],ItemComp.ico_Desc,[ico_NetoLoc],cco_CodPro"
            Using conn4 As New SqlConnection(CONNSTR)
                Using cmd8 As SqlCommand = conn4.CreateCommand()
                    conn4.Open()
                    cmd8.CommandText = sql4
                    Dim reader0 As SqlDataReader = Nothing
                    reader0 = cmd8.ExecuteReader
                    Dim miImputacion As String
                    While reader0.Read()

                        Dim day As Double = 0

                        Dim comparo2 As String

                        comparo2 = reader0(0) & " CUIT:" & reader0(2)
                        miImputacion = reader0(3)

                        If row.Cells(0).Value.Equals(comparo2) Then

                            'sql5 = " select cco_SaldoMonCC from [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[Proveed] on cco_codPro = pro_cod where cco_Nro ='" & reader0(1) & "'  and cco_CodPro= '" & reader0(4) & "'"
                            Dim aux2 As Double
                            sql5 = " select cco_SaldoMonCC,cco_Nro from [SBDASIPT].[dbo].[CabCompra]inner join [SBDASIPT].[dbo].[Proveed] on cco_CodPro = pro_Cod where cco_Nro ='" & reader0(1) & "' and cco_CodPro= '" & reader0(5) & "' and ccotco_Cod ='FAC'"
                            'sql5 = "Select Codigo from AAContratos where Detalle like '%Factura3 Nro: " & reader0(1) & "%' and Nombre like '%" & comparo2 & "%' and Causa like '%" & txtCodigo.Text & "%'" '
                            Using conn9 As New SqlConnection(CONNSTR)
                                Using cmd9 As SqlCommand = conn9.CreateCommand()
                                    conn9.Open()
                                    cmd9.CommandText = sql5
                                    Dim reader9 As SqlDataReader = Nothing
                                    reader9 = cmd9.ExecuteReader

                                    While reader9.Read()
                                        If reader9(0).ToString.Equals("") Then
                                            aux2 = reader9(4)
                                            row.Cells("FACTURA").Value = row.Cells("FACTURA").Value & " Factura Nro: " & reader0(1) & " "
                                            row.Cells("Imputacion").Value = row.Cells("Imputacion").Value & miImputacion & " "
                                            row.Cells("MONTO").Value = row.Cells("MONTO").Value + reader0(4) * -1
                                        Else
                                            'If (reader9(0) <> 0) Then

                                            row.Cells("FACTURA").Value = row.Cells("FACTURA").Value & " Factura Nro: " & reader0(1) & " "
                                            row.Cells("Imputacion").Value = row.Cells("Imputacion").Value & miImputacion & " "
                                            row.Cells("MONTO").Value = row.Cells("MONTO").Value + reader0(4) * -1
                                            'End If

                                        End If
                                        '         Else

                                        'If row.Cells("FACTURA").Value = Nothing Then

                                        '    row.Cells("FACTURA").Value = row.Cells("FACTURA").Value & " Factura Nro: " & reader0(1) & " "
                                        '    row.Cells("Imputacion").Value = row.Cells("Imputacion").Value & miImputacion & " "
                                        '    row.Cells("MONTO").Value = reader0(4) * -1

                                        'Else
                                        '    If row.Cells("FACTURA").Value.ToString.Contains(reader0(4)) Then

                                        '    Else
                                        '        If row.Cells("FACTURA").Value.ToString.Contains(reader0(1)) Then
                                        '            'row.Cells("FACTURA").Value = row.Cells("FACTURA").Value & " Factura Nro: " & reader0(1) & " "
                                        '            row.Cells("Imputacion").Value = row.Cells("Imputacion").Value & miImputacion & " "
                                        '            row.Cells("MONTO").Value = row.Cells("MONTO").Value + reader0(4) * -1
                                        '        Else
                                        '            'row.Cells("FACTURA").Value = row.Cells("FACTURA").Value & " Factura Nro: " & reader0(1) & " "
                                        '            'row.Cells("Imputacion").Value = row.Cells("Imputacion").Value & miImputacion & " "
                                        '            'row.Cells("MONTO").Value = row.Cells("MONTO").Value + reader0(4) * -1
                                        '        End If

                                        '    End If
                                        'End If

                                    End While
                                End Using
                            End Using
                        End If

                        miImputacion = ""
                    End While


                End Using
            End Using

        Next


    End Sub
    Public Sub comprobar(ByRef aux As String)
        Dim sql5 As String
        sql5 = "select cco_Nro,cco_CodPro,pro_Cod,ccotco_Cod from [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[Proveed] on cco_codPro = pro_cod where cco_Nro ='" & aux & "' and ccotco_Cod = 'FAC' "

        Using conn9 As New SqlConnection(CONNSTR)
            Using cmd9 As SqlCommand = conn9.CreateCommand()
                conn9.Open()
                cmd9.CommandText = sql5
                Dim reader9 As SqlDataReader = Nothing
                reader9 = cmd9.ExecuteReader
                While reader9.Read()

                End While

            End Using
        End Using
    End Sub



    Public Sub limpiarGrid()
        For n As Integer = DataGridView1.Rows.Count - 1 To 0 Step -1
            Dim row As DataGridViewRow = DataGridView1.Rows(n)
            If (row.Cells(2).Value Is Nothing) Then
                If (row.Cells(3).Value Is Nothing) Then
                    DataGridView1.Rows.Remove(row)
                End If

            End If
        Next
    End Sub

    Public Sub adjuntarDatosRendicion()
        Dim nombre As String
        Dim resto As Double
        Dim sql4 As String
        For Each row As DataGridViewRow In DataGridView1.Rows
            sql = "SELECT [cco_ID],[ccopro_RazSoc],[cco_ImpMonLoc],[ico_Desc],ItemComp.ico_CantUM1,[ico_NetoLoc],[cco_Nro],CabCompra.[ccopro_CUIT] " &
"FROM [SBDASIPT].[dbo].[CabCompra] " +
"inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID " +
"inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod " +
"where [CausaEmi].cem_Desc='" & adjuntarAnio() & "' and not ItemComp.ico_Desc like '%Impuesto%' and ( CabCompra.ccotco_Cod not like '%SLL%' or CabCompra.ccotco_Cod not like '%OP%') " +
" GROUP BY [cco_ID],[ccopro_RazSoc],[cco_ImpMonLoc],[ico_Desc],ItemComp.ico_CantUM1,[ico_NetoLoc],[cco_Nro],CabCompra.[ccopro_CUIT]   " +
"Except " +
"SELECT [cco_ID],[ccopro_RazSoc],[cco_ImpMonLoc],[ico_Desc],ItemComp.ico_CantUM1,[ico_NetoLoc],[cco_Nro],CabCompra.[ccopro_CUIT] " +
"FROM [SBDASIPT].[dbo].[CabCompra] " +
"inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID " +
"inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod " +
"inner join [SBDASIPT].dbo.[RelCompCpr] on CabCompra.cco_ID=RelCompCpr.rcccco_IDCol1 " +
"where [CausaEmi].cem_Desc='" & adjuntarAnio() & "' and CabCompra.ccotco_Cod  like '%SLL%' or CabCompra.ccotco_Cod  like '%OP%' " +
" GROUP BY [cco_ID],[ccopro_RazSoc],[cco_ImpMonLoc],[ico_Desc],ItemComp.ico_CantUM1,[ico_NetoLoc],[cco_Nro],CabCompra.[ccopro_CUIT]   order by ccopro_RazSoc"

            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql
                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader

                    Dim day As Double = 0
                    Dim subtotal As Double
                    'For Each column As DataGridViewColumn In DataGridView1.Columns

                    subtotal = 0
                    While reader.Read()

                        Dim comparo As String

                        comparo = reader(1) & " CUIT:" & reader(7)
                        nombre = reader(1)
                        Dim sql5 As String
                        If row.Cells(0).Value.Equals(comparo) Then
                            If row.Cells(2).Value.ToString.Contains(reader(6)) Then


                                If reader(5) > 0 Then
                                    row.Cells(reader(4)).Value = reader(5) * -1
                                Else
                                    sql5 = "Select Codigo from AAContratos where Detalle like '%Factura3 Nro: " & reader(6) & "%' and Nombre like '%" & comparo & "%' and Causa like '%" & txtCodigo.Text & "%'"
                                    Using conn9 As New SqlConnection(CONNSTR)
                                        Using cmd9 As SqlCommand = conn9.CreateCommand()
                                            conn9.Open()
                                            cmd9.CommandText = sql5
                                            Dim reader9 As SqlDataReader = Nothing
                                            reader9 = cmd9.ExecuteReader
                                            If reader9.Read() Then

                                            Else
                                                subtotal = subtotal + reader(5) * -1
                                            End If
                                        End Using
                                    End Using 'indiceColumn = column.Index

                                End If
                            End If
                        End If
                        'row.Cells("DIAS").Value = day
                        'End If
                        'Next
                        'Try

                        '    sql4 = "SELECT mfo_ImpMonLoc FROM [SBDASIPT].[dbo].[CabMovF] inner join [SBDASIPT].dbo.MovF on CabMovF.cmf_ID=MovF.mfocmf_ID WHERE cmf_Desc like '% " & nombre.Substring(0, 14) & "%' and mfo_ImpMonLoc>1"

                        '    Using conn2 As New SqlConnection(CONNSTR)
                        '        Using cmd2 As SqlCommand = conn2.CreateCommand()
                        '            conn2.Open()
                        '            cmd2.CommandText = sql4
                        '            Dim reader2 As SqlDataReader = Nothing
                        '            reader2 = cmd2.ExecuteReader

                        '            If reader.Read Then
                        '                resto = reader2(0)
                        '            End If
                        '        End Using
                        '    End Using
                        'Catch ex As Exception
                        '    resto = 0
                        'End Try

                        If resto > 0 Then
                            row.Cells(3).Value = subtotal
                        Else
                            row.Cells(3).Value = subtotal
                        End If
                    End While

                End Using
            End Using
        Next
        'calculoMonto2()
        'calculoMonto()

    End Sub
    Public Sub calculoMonto()
        If DataGridView1.Rows.Count > 16 Then

            sql = "SELECT sum([cco_ImpMonLoc]) FROM [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " and ItemComp.ico_Desc not like 'Impuesto%' and NOT EXISTS(SELECT [Codigo],[Orden],[Causa],[Nombre],[Detalle],[Monto],[Emitido],[Imputacion] FROM [SBDASIPT].[dbo].[AAContratos])"
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()

                    conn.Open()

                    cmd4.CommandText = sql

                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader

                    Do While reader.Read()
                        Try
                            txtTotalFinal.Text = Format(Convert.ToDecimal(reader(0) * -1), "standard")
                        Catch ex As Exception

                        End Try

                    Loop
                End Using
            End Using

            txtSubTotal.Visible = True
            Label4.Visible = True
            Dim subT As Decimal
            subT = 0
            Try
                For i = 0 To 16
                    subT = subT + DataGridView1.Rows(i).Cells("MONTO").Value
                Next
                txtSubTotal.Text = Format(Convert.ToDecimal(subT), "standard")
            Catch ex As Exception

            End Try

        Else
            txtSubTotal.Visible = False
            Label4.Visible = False
            Try

                sql = "SELECT sum([cco_ImpMonLoc]) FROM [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " and ItemComp.ico_Desc not like 'Impuesto%' and NOT EXISTS(SELECT [Codigo],[Orden],[Causa],[Nombre],[Detalle],[Monto],[Emitido],[Imputacion] FROM [SBDASIPT].[dbo].[AAContratos])"
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()

                        conn.Open()

                        cmd4.CommandText = sql

                        Dim reader As SqlDataReader = Nothing
                        reader = cmd4.ExecuteReader

                        Do While reader.Read()
                            txtTotalFinal.Text = Format(Convert.ToDecimal(reader(0) * -1), "standard")
                        Loop
                    End Using
                End Using

            Catch ex As Exception

            End Try
        End If

    End Sub
    Public Sub calculoMonto2()
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
            'dia = row.Cells("DIAS").Value
            MontoFinal = monto - dia
            row.Cells("MONTO").Value = MontoFinal
        Next
    End Sub
#End Region

#Region "SIN AUTORIZAR CON SELLOS"

    Public Sub generarRendicion2()

        DataGridView2.Rows.Clear()
        DataGridView2.Columns.Clear()
        DataGridView4.Rows.Clear()
        DataGridView4.Columns.Clear()

        sql = " SELECT distinct  ItemComp.ico_Desc FROM CabCompra  INNER JOIN CausaEmi ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod INNER JOIN  ItemComp ON CabCompra.cco_ID=ItemComp.icocco_ID  WHERE  CausaEmi.cem_Desc='" & adjuntarAnio() & "' and ItemComp.ico_Desc  like 'Impuesto%' ORDER BY ItemComp.ico_Desc"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                DataGridView2.Columns.Add("detalle", "DETALLE")

                DataGridView2.Columns.Add("factura", "FACTURA")
                DataGridView4.Columns.Add("detalle", "DETALLE")

                DataGridView4.Columns.Add("factura", "FACTURA")
                Dim count As Integer
                count = 0
                Do While reader.Read()
                    DataGridView2.Columns.Add(reader(0), reader(0))
                    DataGridView2.Columns.Item(reader(0)).DefaultCellStyle.Format = "#####0.00"
                    DataGridView4.Columns.Add(reader(0), reader(0))
                    DataGridView4.Columns.Item(reader(0)).DefaultCellStyle.Format = "#####0.00"
                    'indice = DataGridView1.Columns("viatico" & count).Index.ToString()
                    count = count + 1
                Loop
                DataGridView2.Columns.Add("Emitido", "EMITIDO")

            End Using
        End Using
        cargarNombresRendicion2()
    End Sub

    Public Sub cargarNombresRendicion2()
        sql = " SELECT  distinct CabCompra.ccopro_RazSoc,[ccopro_CUIT] FROM CabCompra  INNER JOIN CausaEmi ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod INNER JOIN  ItemComp ON CabCompra.cco_ID=ItemComp.icocco_ID  WHERE  CausaEmi.cem_Desc='" & adjuntarAnio() & "' group BY CabCompra.ccopro_RazSoc,[ccopro_CUIT]"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Do While reader.Read()
                    Dim fila(1) As Object
                    fila(0) = reader(0) & " CUIT:" & reader(1)
                    DataGridView2.Rows.Add(fila)
                Loop
            End Using
        End Using
        adjuntarDetalle2()
        adjuntarDatosRendicion2()
    End Sub

    Public Sub adjuntarDetalle2()
        sql = "SELECT CabCompra.ccopro_RazSoc,[cco_Nro],CabCompra.[ccopro_CUIT] FROM [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " and ItemComp.ico_Desc  like 'Impuesto%' GROUP BY ccopro_RazSoc,[cco_Nro],[ccopro_CUIT]"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Do While reader.Read()
                    For Each row As DataGridViewRow In DataGridView2.Rows
                        Dim day As Double = 0
                        If row.Cells(0).Value.Equals(reader(0) & " CUIT:" & reader(2)) Then
                            row.Cells("FACTURA").Value = row.Cells("FACTURA").Value & " Recibo Nro: " & reader(1) & " "
                        End If
                    Next
                Loop
            End Using
        End Using

    End Sub

    Public Sub adjuntarDatosRendicion2()
        sql = "SELECT [cco_ID],[ccopro_RazSoc],[cco_ImpMonLoc],[ico_Desc],ItemComp.ico_CantUM1,[ico_NetoLoc],[cco_Nro],[ccopro_CUIT] FROM [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " GROUP BY [cco_ID],[ccopro_RazSoc],[cco_ImpMonLoc],[ico_Desc],ItemComp.ico_CantUM1,[ico_NetoLoc],[cco_Nro],[ccopro_CUIT]"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Do While reader.Read()
                    Dim day As Double = 0
                    For Each column As DataGridViewColumn In DataGridView2.Columns
                        For Each row As DataGridViewRow In DataGridView2.Rows
                            If row.Cells(0).Value.Equals(reader(1) & " CUIT:" & reader(7)) Then
                                If column.Name.Contains(reader(3)) Then
                                    indiceColumn = column.Index
                                    If reader(5) > 0 Then
                                        row.Cells(reader(3)).Value = row.Cells(reader(3)).Value + reader(5)
                                    Else
                                        row.Cells(reader(3)).Value = (row.Cells(reader(3)).Value + reader(5)) * -1
                                    End If
                                End If
                                'row.Cells("DIAS").Value = day
                            End If
                        Next
                    Next
                Loop
            End Using
        End Using
        For Each row As DataGridViewRow In DataGridView2.Rows
            sql2 = "Select Emitido,Nombre from AAImpuestoSello where Nombre='" & row.Cells("Detalle").Value & "'"
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql2
                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    Do While reader.Read()
                        If row.Cells("Detalle").Value.ToString.Equals(reader(1)) Then
                            row.Cells("Emitido").Value = reader(0)
                        End If
                    Loop
                End Using
            End Using
        Next
        'calculoMonto4()
    End Sub

    Public Sub calculoMonto4()
        If DataGridView2.Rows.Count > 16 Then
            Label5.Visible = True
            txtSubSellos.Visible = True
            sql = "SELECT sum([cco_ImpMonLoc]) FROM [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " and ItemComp.ico_Desc  like 'Impuesto%' and NOT EXISTS(SELECT [Codigo],[Orden],[Causa],[Nombre],[Detalle],[Monto],[Emitido],[Imputacion] FROM [SBDASIPT].[dbo].[AAContratos])"
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql
                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    Do While reader.Read()
                        If IsDBNull(reader(0)) Then

                        Else
                            txtMontoSellos.Text = Format(Convert.ToDecimal(reader(0)), "standard")
                        End If

                    Loop
                End Using
            End Using

            Dim subT As Decimal
            subT = 0
            For i = 0 To 15
                subT = subT + DataGridView2.Rows(i).Cells(2).Value
            Next

            txtSubSellos.Text = Format(Convert.ToDecimal(subT), "standard")

        Else
            Try
                sql = "SELECT sum([cco_ImpMonLoc]) FROM [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " and ItemComp.ico_Desc  like 'Impuesto%' and NOT EXISTS(SELECT [codigo],[Causa],[Emitido],[nroOrden],[cli_Cod FROM [SBDASIPT].[dbo].[AAContratos])"
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()
                        conn.Open()
                        cmd4.CommandText = sql
                        Dim reader As SqlDataReader = Nothing
                        reader = cmd4.ExecuteReader
                        Do While reader.Read()
                            txtMontoSellos.Text = Format(Convert.ToDecimal(reader(0)), "standard")
                        Loop
                    End Using
                End Using
            Catch ex As Exception

            End Try

        End If
    End Sub

#End Region

#Region "PAGOS"

    Public Sub cargarDataRendicion()
        DataGridView1.Rows.Clear()
        DataGridView1.Columns.Clear()
        sql = "SELECT [chp_NroCheq],[chpctb_Cod],[chp_Importe],[pro_RazSoc],[chp_FVto],[pro_CUIT]  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
              "inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
              "inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]" &
              "where CausaEmi.cem_Desc='" & adjuntarAnio() & "'"
        DataGridView1.Columns.Add("fecha", "FECHA EMISION")
        DataGridView1.Columns.Add("destinatario", "BENEFICIARIO")
        DataGridView1.Columns.Add("ctaCte", "CUENTA CORRIENTE")
        DataGridView1.Columns.Add("Cheques", "NRO CHEQUE")
        DataGridView1.Columns.Add("importe", "IMPORTE")
        DataGridView1.Columns.Item("IMPORTE").DefaultCellStyle.Format = "#####0.00"
        DataGridView3.Columns.Add("fecha", "FECHA EMISION")
        DataGridView3.Columns.Add("destinatario", "BENEFICIARIO")
        DataGridView3.Columns.Add("ctaCte", "CUENTA CORRIENTE")
        DataGridView3.Columns.Add("Cheques", "NRO CHEQUE")
        DataGridView3.Columns.Add("importe", "IMPORTE")
        DataGridView3.Columns.Item("IMPORTE").DefaultCellStyle.Format = "#####0.00"

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
                    fila(1) = reader(3) & " CUIT:" & reader(5)
                    fila(2) = reader(1)
                    fila(3) = reader(0)
                    fila(4) = reader(2)
                    DataGridView1.Rows.Add(fila)
                Loop
            End Using
        End Using

        sql = "SELECT mfocmf_FMov,mfoctb_Cod,pro_RazSoc,mfo_ImpMonElem   FROM [SBDASIPT].[dbo].[CabMovF] inner join SBDASIPT.dbo.CabCompra on CabMovF.cmf_ID=CabCompra.ccocmf_ID " &
            "inner join [SBDASIPT].dbo.CausaEmi on CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
            "inner join [SBDASIPT].[dbo].[MovF] on  [CabMovF].cmf_ID=[MovF].mfocmf_ID " &
            "inner join [SBDASIPT].[dbo].[Proveed] on MovF.mfopro_Cod=Proveed.pro_Cod " &
            "where CausaEmi.cem_Desc='" & adjuntarAnio() & "' and mfobco_cod  like 'NULL'"

        Dim montoFinal As Double
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
                    fila(0) = reader(0)
                    fila(1) = reader(2)
                    fila(2) = reader(1)
                    fila(3) = "AUTORIZACION"
                    fila(4) = reader(3) * -1
                    DataGridView1.Rows.Add(fila)
                    montoFinal = montoFinal + fila(3)
                Loop
            End Using
        End Using

        calculoMonto()
    End Sub

#End Region

#Region "Imprimir"

    Private Sub imprimirPagoHIstorial(ByRef Nro As String)
        Dim FECHA As String
        Try
            sql = "Select Fecha from AAOrdenNro where Tipo='Contratos' and NroIngreso=" & adjuntarAnio()
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()

                    conn.Open()

                    cmd4.CommandText = sql

                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    If reader.Read() Then
                        FECHA = reader(0)
                    End If
                End Using
            End Using
        Catch ex As Exception
            FECHA = Date.Now
        End Try


        Dim Documento As New Document(PageSize.LEGAL, 60, 5, 35, 5) 'Declaracion del documento
        Dim parrafo As New Paragraph ' Declaracion de un parrafo
        Try
            pdf.PdfWriter.GetInstance(Documento, New FileStream("OrdenPago.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF
        Catch ex As Exception
            MessageBox.Show("Compruebe que no exista un documento abierto previamente", "AVISO")
        End Try


        Documento.Open() 'Abre documento para su escritura


        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX        COMIENZO ENCABEZADO XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX


        parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("PROVINCIAL DE MISIONES" & vbCr & "SISTEMA PROVINCIAL" & vbCr & "DE TELEDUCACION Y DESARROLLO") 'Texto que se insertara
        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen
        imagendemo = iTextSharp.text.Image.GetInstance("descarga.jpg") 'Dirreccion a la imagen que se hace referencia
        imagendemo.SetAbsolutePosition(490, 920) 'Posicion en el eje cartesiano
        imagendemo.ScaleAbsoluteWidth(100) 'Ancho de la imagen
        imagendemo.ScaleAbsoluteHeight(100) 'Altura de la imagen
        Documento.Add(imagendemo) ' Agrega la imagen al documento
        parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("ORDEN DE PAGO NRO:" & Nro & "                             ") 'Texto que se insertara
        parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("FECHA EMIISION:" & FECHA) 'Texto que se insertara
        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Dim res As String
        Try
            sql = "Select Disposicion from AAOrdenNro where Tipo='Contratos' and NroIngreso=" & adjuntarAnio()
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
            sql = "Select Resolucion from AAOrdenNro where Tipo='Contratos' and NroIngreso=" & adjuntarAnio()
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

        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX        FIN ENCABEZADO XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Dim tablademo As New PdfPTable(4) 'declara la tabla con 4 Columnas
        tablademo.SetWidthPercentage({100, 150, 250, 90}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo.AddCell(New Paragraph("IMPUTACION   ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        tablademo.AddCell(New Paragraph("NOMBRE   ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        tablademo.AddCell(New Paragraph("DETALLE           ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
        tablademo.AddCell(New Paragraph("IMPORTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        Documento.Add(tablademo) 'Agrega la tabla al documento

        If DataGridView1.Rows.Count < 19 Then
            Dim tablademo2 As New PdfPTable(4) 'declara la tabla con 4 Columnas
            tablademo2.SetWidthPercentage({100, 150, 250, 90}, PageSize.A4) 'Ajusta el tamaño de cada columna

            'XXXXXXXXXXXXXXXXXXXXXXXXXX COMIENZO ZONA DEL DESASTRE XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

            'Try
            For Each row As DataGridViewRow In DataGridView3.Rows

                tablademo2.AddCell(New Paragraph(row.Cells(3).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                tablademo2.AddCell(New Paragraph(row.Cells(2).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                tablademo2.AddCell(New Paragraph(row.Cells(4).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                Dim montu As Decimal
                Dim paraParrrafo As String
                montu = Format(Convert.ToDecimal(row.Cells(5).Value), "standard")
                paraParrrafo = Format(montu, "###,##0.00")
                Dim miCelda2 As New PdfPCell
                parrafo.Add(paraParrrafo)
                miCelda2.AddElement(parrafo)
                miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                tablademo2.AddCell(miCelda2) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                parrafo.Clear()
            Next
            Documento.Add(tablademo2) 'Agrega la tabla al documento

            'Catch ex As Exception

            'End Try
        Else
            'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX COMIENZO DE MAS DE 23 REGISTROS EN EL DATAGRID XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            Dim tablademo2 As New PdfPTable(4) 'declara la tabla con 4 Columnas
            tablademo2.SetWidthPercentage({100, 150, 250, 90}, PageSize.A4) 'Ajusta el tamaño de cada columna
            Try
                For i = 0 To 16

                    tablademo2.AddCell(New Paragraph(DataGridView3.Rows(i).Cells(3).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    tablademo2.AddCell(New Paragraph(DataGridView3.Rows(i).Cells(2).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    'COMIENZO A CONTROLAR LAS ESTRUCTURAS
                    tablademo2.AddCell(New Paragraph(DataGridView3.Rows(i).Cells(4).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

                    Dim montu As Decimal
                    montu = Format(Convert.ToDecimal(DataGridView3.Rows(i).Cells(5).Value), "standard")
                    Dim miParrafos As String
                    miParrafos = Format(montu, "###,##0.00")
                    parrafo.Add(miParrafos)
                    Dim celda9 As New PdfPCell
                    celda9.AddElement(parrafo)
                    celda9.HorizontalAlignment = Element.ALIGN_RIGHT

                    tablademo2.AddCell(celda9) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    parrafo.Clear()
                Next
            Catch ex As Exception

            End Try
            Documento.Add(tablademo2) 'Agrega la tabla al documento
            Dim tablademo10 As New PdfPTable(4) 'declara la tabla con 4 Columnas
            tablademo10.SetWidthPercentage({100, 150, 250, 90}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tablademo10.AddCell(New Paragraph("SUBTOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo10.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            tablademo10.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            tablademo10.AddCell(New Paragraph(txtSubTotal.Text)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            Documento.Add(tablademo10) 'Agrega la tabla al documento


            Dim tablademo4 As New PdfPTable(4) 'declara la tabla con 4 Columnas
            tablademo4.SetWidthPercentage({100, 150, 250, 90}, PageSize.A4) 'Ajusta el tamaño de cada columna

            Try
                tablademo4.AddCell(New Paragraph("SUBTOTAL       ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                tablademo4.AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                tablademo4.AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                tablademo4.AddCell(New Paragraph(txtSubTotal.Text, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                For i = 17 To DataGridView1.Rows.Count


                    tablademo4.AddCell(New Paragraph(DataGridView3.Rows(i).Cells(3).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    tablademo4.AddCell(New Paragraph(DataGridView3.Rows(i).Cells(2).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    'COMIENZO A CONTROLAR LAS ESTRUCTURAS
                    tablademo4.AddCell(New Paragraph(DataGridView3.Rows(i).Cells(4).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10


                    Dim montu As Decimal
                    Dim paraParrrafo As String
                    montu = Format(Convert.ToDecimal(DataGridView3.Rows(i).Cells(5).Value), "standard")
                    paraParrrafo = Format(montu, "###,##0.00")
                    Dim miCelda2 As New PdfPCell
                    parrafo.Add(paraParrrafo)
                    miCelda2.AddElement(parrafo)
                    miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                    tablademo4.AddCell(miCelda2) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    parrafo.Clear()
                Next
            Catch ex As Exception

            End Try
            Documento.Add(tablademo4) 'Agrega la tabla al documento
            Dim tablademo11 As New PdfPTable(4) 'declara la tabla con 4 Columnas
            tablademo11.SetWidthPercentage({100, 150, 250, 90}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tablademo11.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo11.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            tablademo11.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            Dim paraparrafo As String
            Dim miTotal As Double
            miTotal = Total2.Text
            paraparrafo = Format(miTotal, "###,##0.00")
            Dim miCelda3 As New PdfPCell
            parrafo.Add(paraparrafo)
            miCelda3.AddElement(parrafo)
            miCelda3.HorizontalAlignment = Element.ALIGN_RIGHT
            tablademo11.AddCell(miCelda3) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            parrafo.Clear()
            Documento.Add(tablademo11) 'Agrega la tabla al documento

            'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX FIN DE MAS DE 23 REGISTROS EN EL DATAGRID XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        End If

        'XXXXXXXXXXXXXXXXXXXXXXXXXX FIN ZONA DEL DESASTRE XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph("   ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph("SON:$" & Total2.Text & " (PESOS:" & Numalet.ToCardinal(Total2.Text) & ")")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea

        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL")) 'Salto de linea
        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        Documento.Close() 'Cierra el documento


        Documento.Close() 'Cierra el documento
        System.Diagnostics.Process.Start("OrdenPago.pdf") 'Abre el archivo DEMO.PDF

        'XXXXXXXXXXXXXXXXXXXXXXXXXX            COMIENZO SEGUNDO DOCUMENTO       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        Dim Documento2 As New Document 'Declaracion del documento
        Dim parrafo2 As New Paragraph ' Declaracion de un parrafo


        pdf.PdfWriter.GetInstance(Documento2, New FileStream("OrdenPagoSellos.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

        Documento2.Open() 'Abre documento para su escritura

        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX        COMIENZO ENCABEZADO SEGUNDO DOCUMENTO   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        parrafo2.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
        parrafo2.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo2.Add("PROVINCIAL DE MISIONES" & vbCr & "SISTEMA PROVINCIAL" & vbCr & "DE TELEDUCACION Y DESARROLLO") 'Texto que se insertara
        Documento2.Add(parrafo2) 'Agrega el parrafo al documento
        parrafo2.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        Dim imagendemo2 As iTextSharp.text.Image 'Declaracion de una imagen
        imagendemo2 = iTextSharp.text.Image.GetInstance("descarga.jpg") 'Dirreccion a la imagen que se hace referencia
        imagendemo2.SetAbsolutePosition(450, 750) 'Posicion en el eje cartesiano
        imagendemo2.ScaleAbsoluteWidth(100) 'Ancho de la imagen
        imagendemo2.ScaleAbsoluteHeight(100) 'Altura de la imagen
        Documento2.Add(imagendemo2) ' Agrega la imagen al documento
        parrafo2.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
        parrafo2.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo2.Add("ORDEN DE PAGO NRO:" & Nro & "                             ") 'Texto que se insertara
        parrafo2.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
        parrafo2.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo2.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
        parrafo2.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        parrafo2.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo2.Add("FECHA:" & Date.Now) 'Texto que se insertara
        Documento2.Add(parrafo2) 'Agrega el parrafo al documento
        parrafo2.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        Documento2.Add(New Paragraph(" ")) 'Salto de linea

        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX        FIN ENCABEZADO XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        Documento2.Add(New Paragraph(" ")) 'Salto de linea
        Documento2.Add(New Paragraph("LIQUIDACIÓN DE IMPUESTOS AL SELLO")) 'Salto de linea
        Documento2.Add(New Paragraph(" ")) 'Salto de linea
        'XXXXXXXXXXXXXXXXXXXXXXXXXXXX INICIO SEGUNDA ZONA DE DESASTRE XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        'XXXXXXXXXXXX ENCABEZADO DE TABLA XXXXXXXXXXXXXXXXXXXX
        Dim tablademo7 As New PdfPTable(3) 'declara la tabla con 4 Columnas
        tablademo7.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo7.AddCell(New Paragraph("NOMBRE   ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        tablademo7.AddCell(New Paragraph("DETALLE   ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        tablademo7.AddCell(New Paragraph("IMPUESTO AL SELLO           ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8

        Documento2.Add(tablademo7) 'Agrega la tabla al documento
        'XXXXXXXXXXXXXXXXXX FIN ENCABEZADO DE TABLA XXXXXXXXXXXXXXXXXXXXXXXXXX
        If DataGridView4.Rows.Count < 15 Then
            Dim tablademo8 As New PdfPTable(3) 'declara la tabla con 4 Columnas
            tablademo8.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
            For Each row1 As DataGridViewRow In DataGridView4.Rows
                Dim munto As Double
                munto = row1.Cells(2).Value
                tablademo8.AddCell(New Paragraph(row1.Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                tablademo8.AddCell(New Paragraph(row1.Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                Dim paraParrafo As String
                paraParrafo = Format(munto, "###,##0.00")
                parrafo.Add(paraParrafo)
                Dim miCelda4 As New PdfPCell
                miCelda4.AddElement(parrafo)
                miCelda4.HorizontalAlignment = Element.ALIGN_RIGHT

                tablademo8.AddCell(miCelda4) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                parrafo.Clear()

            Next
            Documento2.Add(tablademo8) 'Agrega la tabla al documento
            Dim tablademo0 As New PdfPTable(3) 'declara la tabla con 4 Columnas
            tablademo0.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tablademo0.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo0.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            Dim parraParrafo As String

            Dim sanTotal As Double
            sanTotal = Format(Convert.ToDecimal(TotalSellos2.Text), "standard")
            parraParrafo = Format(sanTotal, "###,##0.00")
            parrafo.Add(parraParrafo)

            Dim celda As New PdfPCell
            celda.AddElement(parrafo)
            celda.HorizontalAlignment = Element.ALIGN_RIGHT
            tablademo0.AddCell(celda) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            Documento2.Add(tablademo0) 'Agrega la tabla al documento
            parrafo.Clear()
        Else

            Dim tablademo8 As New PdfPTable(3) 'declara la tabla con 4 Columnas
            tablademo8.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
            For i = 0 To 15
                Dim munto As Decimal
                munto = Format(Convert.ToDecimal(DataGridView4.Rows(i).Cells(2).Value), "standard")
                tablademo8.AddCell(New Paragraph(DataGridView4.Rows(i).Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                tablademo8.AddCell(New Paragraph(DataGridView4.Rows(i).Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                Dim paraParrafo4 As String
                paraParrafo4 = Format(munto, "###,##0.00")
                parrafo.Add(paraParrafo4)

                Dim celda As New PdfPCell
                celda.AddElement(parrafo)
                celda.HorizontalAlignment = Element.ALIGN_RIGHT
                tablademo8.AddCell(celda) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                parrafo.Clear()

            Next
            Documento2.Add(tablademo8) 'Agrega la tabla al documento
            Dim tablademo0 As New PdfPTable(3) 'declara la tabla con 4 Columnas
            tablademo0.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tablademo0.AddCell(New Paragraph("SUBTOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo0.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            Dim paraparrafo As String
            Dim solito As Double
            solito = Double.Parse(TotalSellos2.Text)
            paraparrafo = Format(solito, "###,##0.00")
            parrafo.Add(paraparrafo)
            Dim celda1 As New PdfPCell
            celda1.AddElement(parrafo)
            celda1.HorizontalAlignment = Element.ALIGN_RIGHT
            tablademo0.AddCell(celda1) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            Documento2.Add(tablademo0) 'Agrega la tabla al documento
            parrafo.Clear()
            'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX SEGUNDA PAGINA XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            Dim tablademo18 As New PdfPTable(3) 'declara la tabla con 4 Columnas
            tablademo18.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
            For i = 15 To DataGridView4.Rows.Count - 1
                Dim munto As Decimal
                munto = Format(Convert.ToDecimal(DataGridView4.Rows(i).Cells(2).Value), "standard")
                Dim paraParrafo2 As String
                paraParrafo2 = Format(munto, "###,##0.00")
                parrafo.Add(paraParrafo2)
                Dim celda9 As New PdfPCell
                celda9.AddElement(parrafo)
                celda9.HorizontalAlignment = Element.ALIGN_RIGHT
                tablademo18.AddCell(New Paragraph(DataGridView4.Rows(i).Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                tablademo18.AddCell(New Paragraph(DataGridView4.Rows(i).Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                tablademo18.AddCell(celda9) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                parrafo.Clear()
            Next
            Documento2.Add(tablademo18) 'Agrega la tabla al documento
            Dim tablademo20 As New PdfPTable(3) 'declara la tabla con 4 Columnas
            tablademo20.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tablademo20.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo20.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            Dim paraParrafo1 As String
            Dim sanTotal As Double
            sanTotal = Format(Convert.ToDecimal(TotalSellos2.Text), "standard")

            paraParrafo1 = Format(sanTotal, "###,##0.00")
            parrafo.Add(paraParrafo1)
            Dim celda2 As New PdfPCell
            celda2.AddElement(parrafo)
            celda2.HorizontalAlignment = Element.ALIGN_RIGHT
            tablademo20.AddCell(celda2) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            Documento2.Add(tablademo20) 'Agrega la tabla al documento
            parrafo.Clear()
        End If

        'XXXXXXXXXXXXXXXXXXXXXXXXXX FIN SEGUNDA ZONA DE DESASTRE XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        Documento2.Add(New Paragraph(" ")) 'Salto de linea
        Documento2.Add(New Paragraph("SON:$" & TotalSellos2.Text & " (Pesos " & Numalet.ToCardinal(TotalSellos2.Text) & ")")) 'Salto de linea
        Documento2.Add(New Paragraph(" ")) 'Salto de linea
        Documento2.Add(New Paragraph("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL")) 'Salto de linea
        Documento2.Add(parrafo2) 'Agrega el parrafo al documento
        parrafo2.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        Documento2.Close() 'Cierra el documento
        System.Diagnostics.Process.Start("OrdenPagoSellos.pdf") 'Abre el archivo DEMO.PDF
    End Sub

    Public Sub imprimirPago()
        Dim Origen As String
        If chkNac.Checked Then
            Origen = "OPN"
        End If
        If chkProv.Checked Then
            Origen = "OPP"
        End If
        Dim BANDERA As Boolean = False

        'If chkHistorial.Checked Then
        '    BANDERA = True
        'End If
        'If ChkNuevo.Checked Then
        '    BANDERA = False
        'End If
        ''''''''''''''''''''''''''''''''''''''''''
        'If (chkEspecial.Checked) Then
        '    BANDERA = False
        'End If
        '''''''''''''''''''''''''''''''''''''''''''''''
        If BANDERA Then
            '' imprimirPagoHIstorial(DataGridView3.Rows(0).Cells(0).Value)
            'Select Case MsgBox("¿Ya existe una orden con este numero desea reemplazarla?", MsgBoxStyle.YesNo, "AVISO")
            '    'en caso de seleccionar un si
            '    Case MsgBoxResult.Yes
            '        'Elimina la tupla existente en la base de datos y despues inserta la nueva orden de pago
            '        reemplazarImprimirContratos()
            '    Case MsgBoxResult.No
            '        MessageBox.Show("Accion cancelada por el usuario", "INFORMACION")
            'End Select
            'MessageBox.Show("YA EXISTE UN CONTRATO CON ESTE NUMERO DE ORDEN")

        Else
            Dim nroCargo As String
            Dim Documento As New Document(PageSize.LEGAL, 60, 5, 35, 5) 'Declaracion del documento
            Dim parrafo As New Paragraph ' Declaracion de un parrafo
            Dim nro As String
            nroCargo = txtSaliente.Text
            Try
                If nroCargo.ToString.Length = 1 Then
                    nro = "0000000" & nroCargo
                End If
                If nroCargo.ToString.Length = 2 Then
                    nro = "000000" & nroCargo
                End If
                If nroCargo.ToString.Length = 3 Then
                    nro = "00000" & nroCargo
                End If
                If nroCargo.ToString.Length = 4 Then
                    nro = "0000" & nroCargo
                End If
                If nroCargo.ToString.Length = 5 Then
                    nro = "000" & nroCargo
                End If
                If nroCargo.ToString.Length = 6 Then
                    nro = "00" & nroCargo
                End If
                If nroCargo.ToString.Length = 7 Then
                    nro = "0" & nroCargo
                End If
                If nroCargo.ToString.Length = 8 Then
                    nro = nroCargo
                End If

                sql = "update Talonar set tal_ActualNro='" & nro & "' where tal_Cod='" & Origen & "'"
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

            If almacenarOrden(nroCargo, Origen) Then
                If almacenarDisp() Then
                    If almacenarContrato(nroCargo) Then
                        'If almacenarSellos(nroCargo) Then
                        Try

                            pdf.PdfWriter.GetInstance(Documento, New FileStream("OrdenPago.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

                            Documento.Open() 'Abre documento para su escritura
                            'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX        COMIENZO ENCABEZADO XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                            parrafo.Add("PROVINCIAL DE MISIONES" & vbCr & "SISTEMA PROVINCIAL" & vbCr & "DE TELEDUCACION Y DESARROLLO") 'Texto que se insertara
                            Documento.Add(parrafo) 'Agrega el parrafo al documento
                            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                            Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen
                            imagendemo = iTextSharp.text.Image.GetInstance("descarga.jpg") 'Dirreccion a la imagen que se hace referencia
                            imagendemo.SetAbsolutePosition(490, 920) 'Posicion en el eje cartesiano
                            imagendemo.ScaleAbsoluteWidth(100) 'Ancho de la imagen
                            imagendemo.ScaleAbsoluteHeight(100) 'Altura de la imagen
                            Documento.Add(imagendemo) ' Agrega la imagen al documento
                            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                            parrafo.Add("ORDEN DE PAGO NRO:" & txtSaliente.Text & "                             ") 'Texto que se insertara
                            parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                            parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                            parrafo.Add("FECHA:" & Date.Now) 'Texto que se insertara
                            Documento.Add(parrafo) 'Agrega el parrafo al documento
                            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                            Documento.Add(New Paragraph(" ")) 'Salto de linea
                            If chkDisp.Checked Then
                                Documento.Add(New Paragraph("Disposición Nro:" & txtNro.Text)) 'Salto de linea
                            End If
                            If chkRes.Checked Then
                                Documento.Add(New Paragraph("Resolución Nro:" & txtNro.Text)) 'Salto de linea
                            End If
                            Documento.Add(New Paragraph(" ")) 'Salto de linea

                            'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX        FIN ENCABEZADO XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                            Dim tablademo As New PdfPTable(4) 'declara la tabla con 4 Columnas
                            tablademo.SetWidthPercentage({100, 150, 250, 90}, PageSize.A4) 'Ajusta el tamaño de cada columna
                            tablademo.AddCell(New Paragraph("IMPUTACION   ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            tablademo.AddCell(New Paragraph("NOMBRE   ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            tablademo.AddCell(New Paragraph("DETALLE           ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                            tablademo.AddCell(New Paragraph("IMPORTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                            Documento.Add(tablademo) 'Agrega la tabla al documento

                            If DataGridView3.Rows.Count < 19 Then
                                Dim tablademo2 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                                tablademo2.SetWidthPercentage({100, 150, 250, 90}, PageSize.A4) 'Ajusta el tamaño de cada columna

                                'XXXXXXXXXXXXXXXXXXXXXXXXXX COMIENZO ZONA DEL DESASTRE XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                                For Each row As DataGridViewRow In DataGridView3.Rows
                                    tablademo2.AddCell(New Paragraph(row.Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    tablademo2.AddCell(New Paragraph(row.Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    tablademo2.AddCell(New Paragraph(row.Cells(2).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    Dim montu As Decimal
                                    Dim paraParrrafo As String
                                    montu = Format(Convert.ToDecimal(row.Cells(3).Value), "standard")
                                    paraParrrafo = Format(montu, "###,##0.00")
                                    Dim miCelda2 As New PdfPCell
                                    parrafo.Add(paraParrrafo)
                                    miCelda2.AddElement(parrafo)
                                    miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                                    tablademo2.AddCell(miCelda2) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    parrafo.Clear()
                                Next
                                Documento.Add(tablademo2) 'Agrega la tabla al documento

                            Else
                                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX COMIENZO DE MAS DE 23 REGISTROS EN EL DATAGRID XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                                Dim tablademo2 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                                tablademo2.SetWidthPercentage({100, 150, 250, 90}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                Try
                                    For i = 0 To 16

                                        tablademo2.AddCell(New Paragraph(DataGridView3.Rows(i).Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                        tablademo2.AddCell(New Paragraph(DataGridView3.Rows(i).Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                        tablademo2.AddCell(New Paragraph(DataGridView3.Rows(i).Cells(2).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                        Dim montu As Decimal
                                        montu = Format(Convert.ToDecimal(DataGridView3.Rows(i).Cells(3).Value), "standard")
                                        Dim miParrafos As String
                                        miParrafos = Format(montu, "###,##0.00")
                                        parrafo.Add(miParrafos)
                                        Dim celda9 As New PdfPCell
                                        celda9.AddElement(parrafo)
                                        celda9.HorizontalAlignment = Element.ALIGN_RIGHT

                                        tablademo2.AddCell(celda9) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                        parrafo.Clear()
                                    Next
                                Catch ex As Exception

                                End Try
                                Documento.Add(tablademo2) 'Agrega la tabla al documento
                                Dim tablademo10 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                                tablademo10.SetWidthPercentage({100, 150, 250, 90}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                tablademo10.AddCell(New Paragraph("SUBTOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo10.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                tablademo10.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

                                tablademo10.AddCell(New Paragraph(txtSubTotal2.Text)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                Documento.Add(tablademo10) 'Agrega la tabla al documento

                                Dim tablademo4 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                                tablademo4.SetWidthPercentage({100, 150, 250, 90}, PageSize.A4) 'Ajusta el tamaño de cada columna



                                Try
                                    tablademo4.AddCell(New Paragraph("SUBTOTAL       ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    tablademo4.AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    tablademo4.AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    tablademo4.AddCell(New Paragraph(txtSubTotal2.Text, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    For i = 17 To DataGridView3.Rows.Count
                                        tablademo4.AddCell(New Paragraph(DataGridView3.Rows(i).Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                        tablademo4.AddCell(New Paragraph(DataGridView3.Rows(i).Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                        tablademo4.AddCell(New Paragraph(DataGridView3.Rows(i).Cells(2).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                        Dim montu As Decimal
                                        Dim paraParrrafo As String
                                        montu = Format(Convert.ToDecimal(DataGridView3.Rows(i).Cells(3).Value), "standard")
                                        paraParrrafo = Format(montu, "###,##0.00")
                                        Dim miCelda2 As New PdfPCell
                                        parrafo.Add(paraParrrafo)
                                        miCelda2.AddElement(parrafo)
                                        miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                                        tablademo4.AddCell(miCelda2) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                        parrafo.Clear()
                                    Next
                                Catch ex As Exception

                                End Try
                                Documento.Add(tablademo4) 'Agrega la tabla al documento
                                Dim tablademo11 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                                tablademo11.SetWidthPercentage({100, 150, 250, 90}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                tablademo11.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo11.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                tablademo11.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                Dim paraparrafo As String
                                paraparrafo = Format(Total2.Text, "###,##0.00")
                                Dim miCelda3 As New PdfPCell
                                parrafo.Add(paraparrafo)
                                miCelda3.AddElement(parrafo)
                                miCelda3.HorizontalAlignment = Element.ALIGN_RIGHT
                                tablademo11.AddCell(miCelda3) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                parrafo.Clear()
                                Documento.Add(tablademo11) 'Agrega la tabla al documento




                                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX FIN DE MAS DE 23 REGISTROS EN EL DATAGRID XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX


                            End If


                            'XXXXXXXXXXXXXXXXXXXXXXXXXX FIN ZONA DEL DESASTRE XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX


                            Documento.Add(New Paragraph(" ")) 'Salto de linea
                            Documento.Add(New Paragraph("SON:" & Total2.Text & " (PESOS:" & Numalet.ToCardinal(Total2.Text) & ")")) 'Salto de linea
                            Documento.Add(New Paragraph(" ")) 'Salto de linea

                            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevament
                            If (TotalSellos2.Text <> 0) Then

                                'XXXXXXXXXXXXXXXXXXXXXXXXXX            COMIENZO SEGUNDO DOCUMENTO       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                                'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                                'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                                If chkDisp.Checked Then
                                    Documento.Add(New Paragraph("Disposición Nro:" & txtNro.Text)) 'Salto de linea
                                End If
                                If chkRes.Checked Then
                                    Documento.Add(New Paragraph("Resolución Nro:" & txtNro.Text)) 'Salto de linea
                                End If 'TETAS

                                Documento.Add(New Paragraph(" ")) 'Salto de linea
                                Documento.Add(New Paragraph("LIQUIDACIÓN DE IMPUESTOS AL SELLO")) 'Salto de linea
                                Documento.Add(New Paragraph(" ")) 'Salto de linea
                                'XXXXXXXXXXXXXXXXXXXXXXXXXXXX XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                                'XXXXXXXXXXXX ENCABEZADO DE TABLA XXXXXXXXXXXXXXXXXXXX
                                Dim tablademo07 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                                tablademo07.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                tablademo07.AddCell(New Paragraph("NOMBRE   ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo07.AddCell(New Paragraph("DETALLE   ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo07.AddCell(New Paragraph("IMPUESTO AL SELLO           ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8

                                Documento.Add(tablademo07) 'Agrega la tabla al documento

                                If DataGridView4.Rows.Count < 15 Then
                                    Dim tablademo8 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                                    tablademo8.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                    For Each row1 As DataGridViewRow In DataGridView4.Rows
                                        Dim munto As Double
                                        munto = row1.Cells(2).Value
                                        tablademo8.AddCell(New Paragraph(row1.Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo8.AddCell(New Paragraph(row1.Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        Dim paraParrafo As String
                                        paraParrafo = Format(munto, "###,##0.00")
                                        parrafo.Add(paraParrafo)
                                        Dim miCelda4 As New PdfPCell
                                        miCelda4.AddElement(parrafo)
                                        miCelda4.HorizontalAlignment = Element.ALIGN_RIGHT

                                        tablademo8.AddCell(miCelda4) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        parrafo.Clear()

                                    Next
                                    Documento.Add(tablademo8) 'Agrega la tabla al documento
                                    Dim tablademo0 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                                    tablademo0.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                    tablademo0.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    Dim parraParrafo As String

                                    Dim sanTotal As Double
                                    sanTotal = Format(Convert.ToDecimal(TotalSellos2.Text), "standard")
                                    parraParrafo = Format(sanTotal, "###,##0.00")
                                    parrafo.Add(parraParrafo)

                                    Dim celda As New PdfPCell
                                    celda.AddElement(parrafo)
                                    celda.HorizontalAlignment = Element.ALIGN_RIGHT
                                    tablademo0.AddCell(celda) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    Documento.Add(tablademo0) 'Agrega la tabla al documento
                                    parrafo.Clear()
                                Else

                                    Dim tablademo8 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                                    tablademo8.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                    For i = 0 To 15
                                        Dim munto As Decimal
                                        munto = Format(Convert.ToDecimal(DataGridView4.Rows(i).Cells(2).Value), "standard")
                                        tablademo8.AddCell(New Paragraph(DataGridView4.Rows(i).Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo8.AddCell(New Paragraph(DataGridView4.Rows(i).Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        Dim paraParrafo4 As String
                                        paraParrafo4 = Format(munto, "###,##0.00")
                                        parrafo.Add(paraParrafo4)

                                        Dim celda As New PdfPCell
                                        celda.AddElement(parrafo)
                                        celda.HorizontalAlignment = Element.ALIGN_RIGHT
                                        tablademo8.AddCell(celda) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        parrafo.Clear()

                                    Next
                                    Documento.Add(tablademo8) 'Agrega la tabla al documento
                                    Dim tablademo0 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                                    tablademo0.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                    tablademo0.AddCell(New Paragraph("SUBTOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    Dim paraparrafo As String
                                    Dim solito As Double
                                    solito = Double.Parse(TotalSellos2.Text)
                                    paraparrafo = Format(solito, "###,##0.00")
                                    parrafo.Add(paraparrafo)
                                    Dim celda1 As New PdfPCell
                                    celda1.AddElement(parrafo)
                                    celda1.HorizontalAlignment = Element.ALIGN_RIGHT
                                    tablademo0.AddCell(celda1) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    Documento.Add(tablademo0) 'Agrega la tabla al documento
                                    parrafo.Clear()
                                    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX SEGUNDA PAGINA XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                                    Dim tablademo18 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                                    tablademo18.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                    For i = 15 To DataGridView4.Rows.Count - 1
                                        Dim munto As Decimal
                                        munto = Format(Convert.ToDecimal(DataGridView4.Rows(i).Cells(2).Value), "standard")
                                        Dim paraParrafo2 As String
                                        paraParrafo2 = Format(munto, "###,##0.00")
                                        parrafo.Add(paraParrafo2)
                                        Dim celda9 As New PdfPCell
                                        celda9.AddElement(parrafo)
                                        celda9.HorizontalAlignment = Element.ALIGN_RIGHT
                                        tablademo18.AddCell(New Paragraph(DataGridView4.Rows(i).Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo18.AddCell(New Paragraph(DataGridView4.Rows(i).Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo18.AddCell(celda9) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        parrafo.Clear()
                                    Next
                                    Documento.Add(tablademo18) 'Agrega la tabla al documento
                                    Dim tablademo20 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                                    tablademo20.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                    tablademo20.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo20.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    Dim paraParrafo1 As String
                                    Dim sanTotal As Double
                                    sanTotal = Format(Convert.ToDecimal(TotalSellos2.Text), "standard")

                                    paraParrafo1 = Format(sanTotal, "###,##0.00")
                                    parrafo.Add(paraParrafo1)
                                    Dim celda2 As New PdfPCell
                                    celda2.AddElement(parrafo)
                                    celda2.HorizontalAlignment = Element.ALIGN_RIGHT
                                    tablademo20.AddCell(celda2) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    Documento.Add(tablademo20) 'Agrega la tabla al documento
                                    parrafo.Clear()
                                End If
                                Documento.Add(New Paragraph(" ")) 'Salto de linea
                                Documento.Add(New Paragraph("SON:" & TotalSellos2.Text & " (Pesos " & Numalet.ToCardinal(TotalSellos2.Text) & ")")) 'Salto de linea
                                Documento.Add(New Paragraph(" ")) 'Salto de linea

                                Documento.Add(parrafo) 'Agrega el parrafo al documento
                                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                            End If
                            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                            'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                            'System.Diagnostics.Process.Start("OrdenPagoSellos.pdf") 'Abre el archivo DEMO.PDF

                            Documento.Add(New Paragraph(" ")) 'Salto de linea
                            Documento.Add(New Paragraph(" ")) 'Salto de linea
                            Documento.Add(New Paragraph("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL")) 'Salto de linea
                            Documento.Add(parrafo) 'Agrega el parrafo al documento
                            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente


                            Documento.Close() 'Cierra el documento


                            Documento.Close() 'Cierra el documento
                            System.Diagnostics.Process.Start("OrdenPago.pdf") 'Abre el archivo DEMO.PDF

                            'XXXXXXXXXXXXXXXXXXXXXXXXXX            COMIENZO SEGUNDO DOCUMENTO       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                        Catch ex As Exception
                            MessageBox.Show("Compruebe que no se encuentre el mismo documento abierto", "AVISO")
                        End Try

                    End If
                Else
                End If
            Else
                MessageBox.Show("No se almacenaron los datos de la Disposicion/Resolucion", "AVISO")
            End If
            '    Else
            'End If
        End If
    End Sub

    Public Sub reemplazarImprimirContratos()
        Dim sql As String
        sql = "Select Id from AAOrdenNro where Tipo='ViaticoPago' and NroIngreso=" & adjuntarAnio()
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
        reemplazarPago()
        MessageBox.Show("El remplazo se logro de manera correcta")
    End Sub
    Public Sub reemplazarPago()
        Dim Origen As String
        If chkNac.Checked Then
            Origen = "OPN"
        End If
        If chkProv.Checked Then
            Origen = "OPP"
        End If

        sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='Contratos'"

        Dim BANDERA As Boolean
        If chkHistorial.Checked Then
            BANDERA = True
        End If
        If ChkNuevo.Checked Then
            BANDERA = False
        End If
        'If BANDERA Then

        'Else
        Dim nroCargo As String
        Dim Documento As New Document(PageSize.LEGAL, 60, 5, 35, 5) 'Declaracion del documento
        Dim parrafo As New Paragraph ' Declaracion de un parrafo
        Dim nro As String
        nroCargo = txtSaliente.Text
        Try
            If nroCargo.ToString.Length = 1 Then
                nro = "0000000" & nroCargo
            End If
            If nroCargo.ToString.Length = 2 Then
                nro = "000000" & nroCargo
            End If
            If nroCargo.ToString.Length = 3 Then
                nro = "00000" & nroCargo
            End If
            If nroCargo.ToString.Length = 4 Then
                nro = "0000" & nroCargo
            End If
            If nroCargo.ToString.Length = 5 Then
                nro = "000" & nroCargo
            End If
            If nroCargo.ToString.Length = 6 Then
                nro = "00" & nroCargo
            End If
            If nroCargo.ToString.Length = 7 Then
                nro = "0" & nroCargo
            End If
            If nroCargo.ToString.Length = 8 Then
                nro = nroCargo
            End If

            sql = "update Talonar set tal_ActualNro='" & nro & "' where tal_Cod='" & Origen & "'"
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

        If almacenarOrden(nroCargo, Origen) Then
            If almacenarDisp() Then
                If almacenarContrato(nroCargo) Then
                    'If almacenarSellos(nroCargo) Then
                    Try

                        pdf.PdfWriter.GetInstance(Documento, New FileStream("OrdenPago.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

                        Documento.Open() 'Abre documento para su escritura
                        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX        COMIENZO ENCABEZADO XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                        parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                        parrafo.Add("PROVINCIAL DE MISIONES" & vbCr & "SISTEMA PROVINCIAL" & vbCr & "DE TELEDUCACION Y DESARROLLO") 'Texto que se insertara
                        Documento.Add(parrafo) 'Agrega el parrafo al documento
                        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                        Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen
                        imagendemo = iTextSharp.text.Image.GetInstance("descarga.jpg") 'Dirreccion a la imagen que se hace referencia
                        imagendemo.SetAbsolutePosition(490, 920) 'Posicion en el eje cartesiano
                        imagendemo.ScaleAbsoluteWidth(100) 'Ancho de la imagen
                        imagendemo.ScaleAbsoluteHeight(100) 'Altura de la imagen
                        Documento.Add(imagendemo) ' Agrega la imagen al documento
                        parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                        parrafo.Add("ORDEN DE PAGO NRO:" & txtSaliente.Text & "                             ") 'Texto que se insertara
                        parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                        parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
                        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                        parrafo.Add("FECHA:" & Date.Now) 'Texto que se insertara
                        Documento.Add(parrafo) 'Agrega el parrafo al documento
                        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                        Documento.Add(New Paragraph(" ")) 'Salto de linea
                        If chkDisp.Checked Then
                            Documento.Add(New Paragraph("Disposición Nro:" & txtNro.Text)) 'Salto de linea
                        End If
                        If chkRes.Checked Then
                            Documento.Add(New Paragraph("Resolución Nro:" & txtNro.Text)) 'Salto de linea
                        End If
                        Documento.Add(New Paragraph(" ")) 'Salto de linea

                        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX        FIN ENCABEZADO XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                        Dim tablademo As New PdfPTable(4) 'declara la tabla con 4 Columnas
                        tablademo.SetWidthPercentage({100, 150, 250, 90}, PageSize.A4) 'Ajusta el tamaño de cada columna
                        tablademo.AddCell(New Paragraph("IMPUTACION   ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                        tablademo.AddCell(New Paragraph("NOMBRE   ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                        tablademo.AddCell(New Paragraph("DETALLE           ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                        tablademo.AddCell(New Paragraph("IMPORTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                        Documento.Add(tablademo) 'Agrega la tabla al documento

                        If DataGridView3.Rows.Count < 19 Then
                            Dim tablademo2 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                            tablademo2.SetWidthPercentage({100, 150, 250, 90}, PageSize.A4) 'Ajusta el tamaño de cada columna

                            'XXXXXXXXXXXXXXXXXXXXXXXXXX COMIENZO ZONA DEL DESASTRE XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                            For Each row As DataGridViewRow In DataGridView3.Rows
                                tablademo2.AddCell(New Paragraph(row.Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                tablademo2.AddCell(New Paragraph(row.Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                tablademo2.AddCell(New Paragraph(row.Cells(2).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                Dim montu As Decimal
                                Dim paraParrrafo As String
                                montu = Format(Convert.ToDecimal(row.Cells(3).Value), "standard")
                                paraParrrafo = Format(montu, "###,##0.00")
                                Dim miCelda2 As New PdfPCell
                                parrafo.Add(paraParrrafo)
                                miCelda2.AddElement(parrafo)
                                miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                                tablademo2.AddCell(miCelda2) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                parrafo.Clear()
                            Next
                            Documento.Add(tablademo2) 'Agrega la tabla al documento

                        Else
                            'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX COMIENZO DE MAS DE 23 REGISTROS EN EL DATAGRID XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                            Dim tablademo2 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                            tablademo2.SetWidthPercentage({100, 150, 250, 90}, PageSize.A4) 'Ajusta el tamaño de cada columna
                            Try
                                For i = 0 To 16

                                    tablademo2.AddCell(New Paragraph(DataGridView3.Rows(i).Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    tablademo2.AddCell(New Paragraph(DataGridView3.Rows(i).Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    tablademo2.AddCell(New Paragraph(DataGridView3.Rows(i).Cells(2).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    Dim montu As Decimal
                                    montu = Format(Convert.ToDecimal(DataGridView3.Rows(i).Cells(3).Value), "standard")
                                    Dim miParrafos As String
                                    miParrafos = Format(montu, "###,##0.00")
                                    parrafo.Add(miParrafos)
                                    Dim celda9 As New PdfPCell
                                    celda9.AddElement(parrafo)
                                    celda9.HorizontalAlignment = Element.ALIGN_RIGHT

                                    tablademo2.AddCell(celda9) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    parrafo.Clear()
                                Next
                            Catch ex As Exception

                            End Try
                            Documento.Add(tablademo2) 'Agrega la tabla al documento
                            Dim tablademo10 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                            tablademo10.SetWidthPercentage({100, 150, 250, 90}, PageSize.A4) 'Ajusta el tamaño de cada columna
                            tablademo10.AddCell(New Paragraph("SUBTOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            tablademo10.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                            tablademo10.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

                            tablademo10.AddCell(New Paragraph(txtSubTotal2.Text)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                            Documento.Add(tablademo10) 'Agrega la tabla al documento

                            Dim tablademo4 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                            tablademo4.SetWidthPercentage({100, 150, 250, 90}, PageSize.A4) 'Ajusta el tamaño de cada columna



                            Try
                                tablademo4.AddCell(New Paragraph("SUBTOTAL       ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                tablademo4.AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                tablademo4.AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                tablademo4.AddCell(New Paragraph(txtSubTotal2.Text, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                For i = 17 To DataGridView3.Rows.Count
                                    tablademo4.AddCell(New Paragraph(DataGridView3.Rows(i).Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    tablademo4.AddCell(New Paragraph(DataGridView3.Rows(i).Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    tablademo4.AddCell(New Paragraph(DataGridView3.Rows(i).Cells(2).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    Dim montu As Decimal
                                    Dim paraParrrafo As String
                                    montu = Format(Convert.ToDecimal(DataGridView3.Rows(i).Cells(3).Value), "standard")
                                    paraParrrafo = Format(montu, "###,##0.00")
                                    Dim miCelda2 As New PdfPCell
                                    parrafo.Add(paraParrrafo)
                                    miCelda2.AddElement(parrafo)
                                    miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                                    tablademo4.AddCell(miCelda2) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                    parrafo.Clear()
                                Next
                            Catch ex As Exception

                            End Try
                            Documento.Add(tablademo4) 'Agrega la tabla al documento
                            Dim tablademo11 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                            tablademo11.SetWidthPercentage({100, 150, 250, 90}, PageSize.A4) 'Ajusta el tamaño de cada columna
                            tablademo11.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            tablademo11.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                            tablademo11.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                            Dim paraparrafo As String
                            paraparrafo = Format(Total2.Text, "###,##0.00")
                            Dim miCelda3 As New PdfPCell
                            parrafo.Add(paraparrafo)
                            miCelda3.AddElement(parrafo)
                            miCelda3.HorizontalAlignment = Element.ALIGN_RIGHT
                            tablademo11.AddCell(miCelda3) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                            parrafo.Clear()
                            Documento.Add(tablademo11) 'Agrega la tabla al documento




                            'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX FIN DE MAS DE 23 REGISTROS EN EL DATAGRID XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX


                        End If


                        'XXXXXXXXXXXXXXXXXXXXXXXXXX FIN ZONA DEL DESASTRE XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX


                        Documento.Add(New Paragraph(" ")) 'Salto de linea
                        Documento.Add(New Paragraph("SON:" & Total2.Text & " (PESOS:" & Numalet.ToCardinal(Total2.Text) & ")")) 'Salto de linea
                        Documento.Add(New Paragraph(" ")) 'Salto de linea

                        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevament
                        If (TotalSellos2.Text <> 0) Then

                            'XXXXXXXXXXXXXXXXXXXXXXXXXX            COMIENZO SEGUNDO DOCUMENTO       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                            If chkDisp.Checked Then
                                Documento.Add(New Paragraph("Disposición Nro:" & txtNro.Text)) 'Salto de linea
                            End If
                            If chkRes.Checked Then
                                Documento.Add(New Paragraph("Resolución Nro:" & txtNro.Text)) 'Salto de linea
                            End If 'TETAS

                            Documento.Add(New Paragraph(" ")) 'Salto de linea
                            Documento.Add(New Paragraph("LIQUIDACIÓN DE IMPUESTOS AL SELLO")) 'Salto de linea
                            Documento.Add(New Paragraph(" ")) 'Salto de linea
                            'XXXXXXXXXXXXXXXXXXXXXXXXXXXX XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                            'XXXXXXXXXXXX ENCABEZADO DE TABLA XXXXXXXXXXXXXXXXXXXX
                            Dim tablademo07 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                            tablademo07.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
                            tablademo07.AddCell(New Paragraph("NOMBRE   ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            tablademo07.AddCell(New Paragraph("DETALLE   ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            tablademo07.AddCell(New Paragraph("IMPUESTO AL SELLO           ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8

                            Documento.Add(tablademo07) 'Agrega la tabla al documento

                            If DataGridView4.Rows.Count < 15 Then
                                Dim tablademo8 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                                tablademo8.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                For Each row1 As DataGridViewRow In DataGridView4.Rows
                                    Dim munto As Double
                                    munto = row1.Cells(2).Value
                                    tablademo8.AddCell(New Paragraph(row1.Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo8.AddCell(New Paragraph(row1.Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    Dim paraParrafo As String
                                    paraParrafo = Format(munto, "###,##0.00")
                                    parrafo.Add(paraParrafo)
                                    Dim miCelda4 As New PdfPCell
                                    miCelda4.AddElement(parrafo)
                                    miCelda4.HorizontalAlignment = Element.ALIGN_RIGHT

                                    tablademo8.AddCell(miCelda4) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()

                                Next
                                Documento.Add(tablademo8) 'Agrega la tabla al documento
                                Dim tablademo0 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                                tablademo0.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                tablademo0.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo0.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                Dim parraParrafo As String

                                Dim sanTotal As Double
                                sanTotal = Format(Convert.ToDecimal(TotalSellos2.Text), "standard")
                                parraParrafo = Format(sanTotal, "###,##0.00")
                                parrafo.Add(parraParrafo)

                                Dim celda As New PdfPCell
                                celda.AddElement(parrafo)
                                celda.HorizontalAlignment = Element.ALIGN_RIGHT
                                tablademo0.AddCell(celda) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                Documento.Add(tablademo0) 'Agrega la tabla al documento
                                parrafo.Clear()
                            Else

                                Dim tablademo8 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                                tablademo8.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                For i = 0 To 15
                                    Dim munto As Decimal
                                    munto = Format(Convert.ToDecimal(DataGridView4.Rows(i).Cells(2).Value), "standard")
                                    tablademo8.AddCell(New Paragraph(DataGridView4.Rows(i).Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo8.AddCell(New Paragraph(DataGridView4.Rows(i).Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    Dim paraParrafo4 As String
                                    paraParrafo4 = Format(munto, "###,##0.00")
                                    parrafo.Add(paraParrafo4)

                                    Dim celda As New PdfPCell
                                    celda.AddElement(parrafo)
                                    celda.HorizontalAlignment = Element.ALIGN_RIGHT
                                    tablademo8.AddCell(celda) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()

                                Next
                                Documento.Add(tablademo8) 'Agrega la tabla al documento
                                Dim tablademo0 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                                tablademo0.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                tablademo0.AddCell(New Paragraph("SUBTOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo0.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                Dim paraparrafo As String
                                Dim solito As Double
                                solito = Double.Parse(TotalSellos2.Text)
                                paraparrafo = Format(solito, "###,##0.00")
                                parrafo.Add(paraparrafo)
                                Dim celda1 As New PdfPCell
                                celda1.AddElement(parrafo)
                                celda1.HorizontalAlignment = Element.ALIGN_RIGHT
                                tablademo0.AddCell(celda1) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                Documento.Add(tablademo0) 'Agrega la tabla al documento
                                parrafo.Clear()
                                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX SEGUNDA PAGINA XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                                Dim tablademo18 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                                tablademo18.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                For i = 15 To DataGridView4.Rows.Count - 1
                                    Dim munto As Decimal
                                    munto = Format(Convert.ToDecimal(DataGridView4.Rows(i).Cells(2).Value), "standard")
                                    Dim paraParrafo2 As String
                                    paraParrafo2 = Format(munto, "###,##0.00")
                                    parrafo.Add(paraParrafo2)
                                    Dim celda9 As New PdfPCell
                                    celda9.AddElement(parrafo)
                                    celda9.HorizontalAlignment = Element.ALIGN_RIGHT
                                    tablademo18.AddCell(New Paragraph(DataGridView4.Rows(i).Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo18.AddCell(New Paragraph(DataGridView4.Rows(i).Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo18.AddCell(celda9) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()
                                Next
                                Documento.Add(tablademo18) 'Agrega la tabla al documento
                                Dim tablademo20 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                                tablademo20.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                tablademo20.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo20.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                Dim paraParrafo1 As String
                                Dim sanTotal As Double
                                sanTotal = Format(Convert.ToDecimal(TotalSellos2.Text), "standard")

                                paraParrafo1 = Format(sanTotal, "###,##0.00")
                                parrafo.Add(paraParrafo1)
                                Dim celda2 As New PdfPCell
                                celda2.AddElement(parrafo)
                                celda2.HorizontalAlignment = Element.ALIGN_RIGHT
                                tablademo20.AddCell(celda2) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                                Documento.Add(tablademo20) 'Agrega la tabla al documento
                                parrafo.Clear()
                            End If
                            Documento.Add(New Paragraph(" ")) 'Salto de linea
                            Documento.Add(New Paragraph("SON:" & TotalSellos2.Text & " (Pesos " & Numalet.ToCardinal(TotalSellos2.Text) & ")")) 'Salto de linea
                            Documento.Add(New Paragraph(" ")) 'Salto de linea

                            Documento.Add(parrafo) 'Agrega el parrafo al documento
                            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                        End If
                        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                        'System.Diagnostics.Process.Start("OrdenPagoSellos.pdf") 'Abre el archivo DEMO.PDF

                        Documento.Add(New Paragraph(" ")) 'Salto de linea
                        Documento.Add(New Paragraph(" ")) 'Salto de linea
                        Documento.Add(New Paragraph("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL")) 'Salto de linea
                        Documento.Add(parrafo) 'Agrega el parrafo al documento
                        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente


                        Documento.Close() 'Cierra el documento


                        Documento.Close() 'Cierra el documento
                        System.Diagnostics.Process.Start("OrdenPago.pdf") 'Abre el archivo DEMO.PDF

                        'XXXXXXXXXXXXXXXXXXXXXXXXXX            COMIENZO SEGUNDO DOCUMENTO       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                    Catch ex As Exception
                        MessageBox.Show("Compruebe que no se encuentre el mismo documento abierto", "AVISO")
                    End Try

                End If
            Else
            End If
        Else
            MessageBox.Show("No se almacenaron los datos de la Disposicion/Resolucion", "AVISO")
        End If
        '    Else
        'End If
        'End If
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
    Public Sub imprimirImpuestoSello()
        Dim Documento As New Document(PageSize.LEGAL, 60, 5, 35, 5) 'Declaracion del documento
        Dim parrafo As New Paragraph ' Declaracion de un parrafo

        pdf.PdfWriter.GetInstance(Documento, New FileStream("OrdenPago.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

        Documento.Open() 'Abre documento para su escritura

        If chkDisp.Checked Then
            Documento.Add(New Paragraph("Disposición Nro:" & txtNro.Text)) 'Salto de linea
        End If
        If chkRes.Checked Then
            Documento.Add(New Paragraph("Resolución Nro:" & txtNro.Text)) 'Salto de linea
        End If 'TETAS
        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph("LIQUIDACIÓN DE IMPUESTOS AL SELLO")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        'XXXXXXXXXXXXXXXXXXXXXXXXXXXX XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        'XXXXXXXXXXXX ENCABEZADO DE TABLA XXXXXXXXXXXXXXXXXXXX
        Dim tablademo07 As New PdfPTable(3) 'declara la tabla con 4 Columnas
        tablademo07.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo07.AddCell(New Paragraph("NOMBRE   ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        tablademo07.AddCell(New Paragraph("DETALLE   ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        tablademo07.AddCell(New Paragraph("IMPUESTO AL SELLO           ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8

        Documento.Add(tablademo07) 'Agrega la tabla al documento

        If DataGridView4.Rows.Count < 15 Then
            Dim tablademo8 As New PdfPTable(3) 'declara la tabla con 4 Columnas
            tablademo8.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
            For Each row1 As DataGridViewRow In DataGridView4.Rows
                Dim munto As Double
                munto = row1.Cells(2).Value
                tablademo8.AddCell(New Paragraph(row1.Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                tablademo8.AddCell(New Paragraph(row1.Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                Dim paraParrafo As String
                paraParrafo = Format(munto, "###,##0.00")
                parrafo.Add(paraParrafo)
                Dim miCelda4 As New PdfPCell
                miCelda4.AddElement(parrafo)
                miCelda4.HorizontalAlignment = Element.ALIGN_RIGHT

                tablademo8.AddCell(miCelda4) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                parrafo.Clear()

            Next
            Documento.Add(tablademo8) 'Agrega la tabla al documento
            Dim tablademo0 As New PdfPTable(3) 'declara la tabla con 4 Columnas
            tablademo0.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tablademo0.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo0.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            Dim parraParrafo As String

            Dim sanTotal As Double
            sanTotal = Format(Convert.ToDecimal(TotalSellos2.Text), "standard")
            parraParrafo = Format(sanTotal, "###,##0.00")
            parrafo.Add(parraParrafo)

            Dim celda As New PdfPCell
            celda.AddElement(parrafo)
            celda.HorizontalAlignment = Element.ALIGN_RIGHT
            tablademo0.AddCell(celda) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            Documento.Add(tablademo0) 'Agrega la tabla al documento
            parrafo.Clear()
        Else

            Dim tablademo8 As New PdfPTable(3) 'declara la tabla con 4 Columnas
            tablademo8.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
            For i = 0 To 15
                Dim munto As Decimal
                munto = Format(Convert.ToDecimal(DataGridView4.Rows(i).Cells(2).Value), "standard")
                tablademo8.AddCell(New Paragraph(DataGridView4.Rows(i).Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                tablademo8.AddCell(New Paragraph(DataGridView4.Rows(i).Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                Dim paraParrafo4 As String
                paraParrafo4 = Format(munto, "###,##0.00")
                parrafo.Add(paraParrafo4)

                Dim celda As New PdfPCell
                celda.AddElement(parrafo)
                celda.HorizontalAlignment = Element.ALIGN_RIGHT
                tablademo8.AddCell(celda) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                parrafo.Clear()

            Next
            Documento.Add(tablademo8) 'Agrega la tabla al documento
            Dim tablademo0 As New PdfPTable(3) 'declara la tabla con 4 Columnas
            tablademo0.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tablademo0.AddCell(New Paragraph("SUBTOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo0.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            Dim paraparrafo As String
            Dim solito As Double
            solito = Double.Parse(TotalSellos2.Text)
            paraparrafo = Format(solito, "###,##0.00")
            parrafo.Add(paraparrafo)
            Dim celda1 As New PdfPCell
            celda1.AddElement(parrafo)
            celda1.HorizontalAlignment = Element.ALIGN_RIGHT
            tablademo0.AddCell(celda1) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            Documento.Add(tablademo0) 'Agrega la tabla al documento
            parrafo.Clear()
            'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX SEGUNDA PAGINA XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            Dim tablademo18 As New PdfPTable(3) 'declara la tabla con 4 Columnas
            tablademo18.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
            For i = 15 To DataGridView4.Rows.Count - 1
                Dim munto As Decimal
                munto = Format(Convert.ToDecimal(DataGridView4.Rows(i).Cells(2).Value), "standard")
                Dim paraParrafo2 As String
                paraParrafo2 = Format(munto, "###,##0.00")
                parrafo.Add(paraParrafo2)
                Dim celda9 As New PdfPCell
                celda9.AddElement(parrafo)
                celda9.HorizontalAlignment = Element.ALIGN_RIGHT
                tablademo18.AddCell(New Paragraph(DataGridView4.Rows(i).Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                tablademo18.AddCell(New Paragraph(DataGridView4.Rows(i).Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                tablademo18.AddCell(celda9) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                parrafo.Clear()
            Next
            Documento.Add(tablademo18) 'Agrega la tabla al documento
            Dim tablademo20 As New PdfPTable(3) 'declara la tabla con 4 Columnas
            tablademo20.SetWidthPercentage({200, 250, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tablademo20.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo20.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            Dim paraParrafo1 As String
            Dim sanTotal As Double
            sanTotal = Format(Convert.ToDecimal(TotalSellos2.Text), "standard")

            paraParrafo1 = Format(sanTotal, "###,##0.00")
            parrafo.Add(paraParrafo1)
            Dim celda2 As New PdfPCell
            celda2.AddElement(parrafo)
            celda2.HorizontalAlignment = Element.ALIGN_RIGHT
            tablademo20.AddCell(celda2) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            Documento.Add(tablademo20) 'Agrega la tabla al documento
            parrafo.Clear()
        End If
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph("SON:" & TotalSellos2.Text & " (Pesos " & Numalet.ToCardinal(TotalSellos2.Text) & ")")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea

        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

        'System.Diagnostics.Process.Start("OrdenPagoSellos.pdf") 'Abre el archivo DEMO.PDF

        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL")) 'Salto de linea
        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        Documento.Close() 'Cierra el documento
        System.Diagnostics.Process.Start("OrdenPago.pdf") 'Abre el archivo DEMO.PDF
    End Sub
    Private Function almacenarDisp() As Boolean
        Dim bandera As Boolean
        Try

            If chkDisp.Checked Then
                sql = "update AAOrdenNro set Disposicion='" & txtNro.Text & "' where Tipo='Contratos' and NroIngreso='" & adjuntarAnio() & "'"
            End If

            If chkRes.Checked Then
                sql = "update AAOrdenNro set Resolucion='" & txtNro.Text & "' where Tipo='Contratos' and NroIngreso='" & adjuntarAnio() & "'"
            End If

            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()

                    conn.Open()

                    cmd4.CommandText = sql
                    cmd4.ExecuteScalar()

                End Using
            End Using
            bandera = True
        Catch ex As Exception
            bandera = False
        End Try
        Return bandera
    End Function

    Public Sub CargarRetencion()
        sql = "SELECT ccopro_RazSoc,cco_Nro,dicres_Art,dicres_Cod,dic_Imp1 " &
           "FROM [SBDASIPT].[dbo].[CabCompra] " &
           "inner join [SBDASIPT].dbo.CausaEmi on CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
           "inner join [SBDASIPT].dbo.DetIvaCpr ON CabCompra.cco_ID=DetIvaCpr.diccco_ID " &
           "where CausaEmi.cem_Desc='" & adjuntarAnio() & "' and dicres_Art not like '%NULL%' and dicres_Cod not like '%NULL%'"

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                RetencionPrevia.Rows.Clear()
                RetencionPrevia.Columns.Clear()
                RetencionPrevia.Columns.Add("nomb", "NOMBRE")
                RetencionPrevia.Columns.Add("nro", "NUMERO")
                RetencionPrevia.Columns.Add("art", "ARTICULO")
                RetencionPrevia.Columns.Add("cod", "CODIGO")
                RetencionPrevia.Columns.Add("imp", "IMPORTE")

                While reader.Read()
                    'GroupBox7.Visible = True
                    Dim fila(4) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    fila(2) = reader(2)
                    fila(3) = reader(3)
                    fila(4) = reader(4)
                    RetencionPrevia.Rows.Add(fila)
                End While

                DataGridView5.Rows.Clear()
                DataGridView5.Columns.Clear()
                DataGridView5.Columns.Add("nomb", "NOMBRE")
                DataGridView5.Columns.Add("nro", "NUMERO")
                DataGridView5.Columns.Add("art", "ARTICULO")
                DataGridView5.Columns.Add("cod", "CODIGO")
                DataGridView5.Columns.Add("imp", "IMPORTE")

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

        Dim Nro As String
        sql = "Select Concepto from AAOrdenNro where Tipo='Contratos' and NroIngreso=" & adjuntarAnio()
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
        Dim Documento As New Document 'Declaracion del documento
        Dim parrafo As New Paragraph ' Declaracion de un parrafo
        Dim tablademo As New PdfPTable(4) 'declara la tabla con 4 Columnas
        Dim subtotal As Double
        Dim subtotal2 As Double
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
        parrafo.Add("ORDEN DE PAGO NRO:" & txtNroOrden.Text & "                             ") 'Texto que se insertara
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
        res = txtNro.Text
        If chkDisp.Checked Then
            Documento.Add(New Paragraph("Disposición Nro:" & res)) 'Salto de linea
        End If
        If chkRes.Checked Then
            Documento.Add(New Paragraph("Resolución Nro:" & res)) 'Salto de linea
        End If


        Documento.Add(New Paragraph(" ")) 'Salto de linea

        Documento.Add(New Paragraph(" ")) 'Salto de linea

        Documento.Add(New Paragraph(" ")) 'Salto de linea

        Dim tablademo5 As New PdfPTable(5) 'declara la tabla con 4 Columnas
        tablademo5.SetWidthPercentage({100, 150, 100, 110, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo5.AddCell(New Paragraph("F. EMISION        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

        tablademo5.AddCell(New Paragraph("BENEFICIARIO        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo5.AddCell(New Paragraph("NRO CTA        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo5.AddCell(New Paragraph("CH. Y/O AUT.        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        tablademo5.AddCell(New Paragraph("IMPORTE        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        Documento.Add(tablademo5) 'Agrega la tabla al documento

        'Dim tablademo9 As New PdfPTable(5) 'declara la tabla con 4 Columnas
        'tablademo9.SetWidthPercentage({100, 150, 100, 110, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        'For Each row1 As DataGridViewRow In DataGridView3.Rows
        '    For Each column As DataGridViewColumn In DataGridView3.Columns
        '        If column.Index <= 5 Then
        '            Dim paraParrrafo As String
        '            If IsNumeric(row1.Cells(column.Index).Value) And column.Name.Equals("importe") Then

        '                paraParrrafo = Format(row1.Cells(column.Index).Value, "###,##0.00")
        '                Dim miCelda2 As New PdfPCell
        '                parrafo.Add(paraParrrafo)
        '                miCelda2.AddElement(parrafo)
        '                miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
        '                tablademo9.AddCell(miCelda2)
        '                parrafo.Clear()

        '            Else
        '                If column.Index = 0 Or column.Index = 1 Then

        '                Else
        '                    paraParrrafo = row1.Cells(column.Index).Value
        '                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        '                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
        '                    parrafo.Add(paraParrrafo) 'Texto que se insertara
        '                    tablademo9.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        '                    parrafo.Clear()
        '                End If


        '            End If
        '        End If

        '    Next

        'Next
        'Documento.Add(tablademo9) 'Agrega la tabla al documento

        Dim tablademo4 As New PdfPTable(5) 'declara la tabla con 4 Columnas
        tablademo4.SetWidthPercentage({100, 150, 100, 110, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        For Each row1 As DataGridViewRow In DataGridView3.Rows
            For Each column As DataGridViewColumn In DataGridView3.Columns
                If column.Index <= 5 Then
                    Dim paraParrrafo As String
                    If IsNumeric(row1.Cells(column.Index).Value) And column.Index = 4 Then
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
        tabla9.SetWidthPercentage({100, 150, 100, 110, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tabla9.AddCell("SUBTOTAL")
        tabla9.AddCell(" ")
        tabla9.AddCell(" ")
        tabla9.AddCell(" ")
        Dim miParrafo As String
        Dim miTotal As Double
        miTotal = Double.Parse(Total2.Text)
        miParrafo = Format(miTotal, "###,##0.00")
        Dim miCelda As New PdfPCell
        parrafo.Add(miParrafo)
        miCelda.AddElement(parrafo)

        miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
        tabla9.AddCell(miCelda)
        Documento.Add(tabla9)
        parrafo.Clear()

        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph("IMPUESTO AL SELLO:")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Dim tablademo7 As New PdfPTable(3) 'declara la tabla con 4 Columnas
        tablademo7.SetWidthPercentage({150, 150, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo7.AddCell(New Paragraph("BENEFICIARIO        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        tablademo7.AddCell(New Paragraph("DETALLE        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo7.AddCell(New Paragraph("IMPORTE        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        Documento.Add(tablademo7) 'Agrega la tabla al documento
        Try


            Dim tabla7 As New PdfPTable(3)
            tabla7.SetWidthPercentage({150, 150, 100}, PageSize.A4)
            For Each row As DataGridViewRow In DataGridView4.Rows
                For Each column As DataGridViewColumn In DataGridView4.Columns
                    If column.Index <= 3 Then
                        Dim paraParrrafo As String
                        If IsNumeric(row.Cells(column.Index).Value) And column.Index = 2 Then

                            Dim ketchup As Double
                            ketchup = Double.Parse(row.Cells(column.Index).Value)
                            paraParrrafo = Format(ketchup, "###,##0.00")
                            Dim miCelda2 As New PdfPCell
                            parrafo.Add(paraParrrafo)
                            miCelda2.AddElement(parrafo)
                            miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                            tabla7.AddCell(miCelda2)
                            parrafo.Clear()

                        Else

                            paraParrrafo = row.Cells(column.Index).Value
                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                            parrafo.Add(paraParrrafo) 'Texto que se insertara
                            tabla7.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            parrafo.Clear()

                        End If
                    End If
                Next
            Next
            Documento.Add(tabla7)

            Dim tabla10 As New PdfPTable(3)
            'tabla9.HorizontalAlignment = ALIGN_LEFT
            tabla10.SetWidthPercentage({150, 150, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tabla10.AddCell("SUBTOTAL")
            tabla10.AddCell(" ")

            Dim miParrafo2 As String
            Dim miTotal2 As Double
            miTotal2 = Double.Parse(TotalSellos2.Text)
            miParrafo2 = Format(miTotal2, "###,##0.00")
            Dim miCelda3 As New PdfPCell
            parrafo.Add(miParrafo2)
            miCelda3.AddElement(parrafo)

            miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
            tabla10.AddCell(miCelda3)
            Documento.Add(tabla10)
            parrafo.Clear()
        Catch ex As Exception

        End Try
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        If chkRetencion.Checked Then
            Documento.Add(New Paragraph("DATOS DE RETENCIONES: ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea

            Dim tablademo8 As New PdfPTable(5) 'declara la tabla con 4 Columnas
            tablademo8.SetWidthPercentage({150, 100, 100, 100, 80}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tablademo8.AddCell(New Paragraph("NOMBRE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo8.AddCell(New Paragraph("NUMERO", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo8.AddCell(New Paragraph("ARTICULO", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo8.AddCell(New Paragraph("CODIGO", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo8.AddCell(New Paragraph("IMPORTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            Documento.Add(tablademo8) 'Agrega la tabla al documento
            Dim tablademo9 As New PdfPTable(5) 'declara la tabla con 4 Columnas
            tablademo9.SetWidthPercentage({150, 100, 100, 100, 80}, PageSize.A4) 'Ajusta el tamaño de cada columna
            For Each row As DataGridViewRow In DataGridView5.Rows
                For Each column As DataGridViewColumn In DataGridView5.Columns
                    If column.Index <= 5 Then
                        Dim paraParrrafo As String
                        If IsNumeric(row.Cells(column.Index).Value) And column.Index = 4 Then

                            paraParrrafo = Format(row.Cells(column.Index).Value, "###,##0.00")
                            Dim miCelda2 As New PdfPCell
                            parrafo.Add(paraParrrafo)
                            miCelda2.AddElement(parrafo)

                            miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                            tablademo9.AddCell(miCelda2)
                            parrafo.Clear()
                        Else
                            paraParrrafo = row.Cells(column.Index).Value
                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                            parrafo.Add(paraParrrafo) 'Texto que se insertara
                            tablademo9.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            parrafo.Clear()
                        End If
                    End If

                Next
            Next
            Documento.Add(tablademo9) 'Agrega la tabla al documento
        End If

        Documento.Add(New Paragraph(" ")) 'Salto de linea
        For Each row9 As DataGridViewRow In DataGridView3.Rows
            subtotal = subtotal + row9.Cells(4).Value '''' modificacion por Nestor
        Next
        Dim subtotal3 As Double
        For Each row8 As DataGridViewRow In DataGridView4.Rows
            subtotal2 = subtotal2 + row8.Cells(2).Value
        Next
        If chkRetencion.Checked Then
            For Each row87 As DataGridViewRow In DataGridView5.Rows
                subtotal3 = subtotal3 + row87.Cells(4).Value
            Next
        End If
        Dim monto As Double
        If chkRetencion.Checked Then
            monto = subtotal + subtotal2 + subtotal3
        Else
            monto = subtotal + subtotal2
        End If
        ' monto = Format(Convert.ToDecimal(Total2.Text + TotalSellos2.Text))

        Documento.Add(New Paragraph("Total Final:$ " & monto & "  ( Pesos: " & Numalet.ToCardinal(monto) & ")")) 'Salto de linea

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

#End Region

#Region "CODIGOS AUXILIARES"
    Public Function adjuntarAnio() As String
        Dim codigo As String
        Dim fecha As String
        'fecha = Date.Now.Year
        fecha = cmbAnio.SelectedItem
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
    Public Sub actualizarTalonario()

        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 3).Value = "DDL"
                cmd.Parameters.Add("@ActualNro", SqlDbType.VarChar, 8).Value = dameNroPago()
                cmd.CommandText = "update Talonar set " +
                                "tal_ActualNro=" + "@ActualNro " +
                                "where tal_Cod=@Codigo"
                cmd.ExecuteScalar()
            End Using
        End Using
    End Sub
    Public Function dameNroPago() As String

        sql = "SELECT [Talonar].[tal_ActualNro] FROM  [SBDASIPT].[dbo].[Talonar] where tal_Cod='DDL'"
        Dim final As String
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
        If nroOrden.Length = 1 Then
            final = "0000000" & nroOrden
        End If
        If nroOrden.Length = 2 Then
            final = "000000" & nroOrden
        End If
        If nroOrden.Length = 3 Then
            final = "00000" & nroOrden
        End If
        If nroOrden.Length = 4 Then
            final = "0000" & nroOrden
        End If
        If nroOrden.Length = 5 Then
            final = "000" & nroOrden
        End If
        If nroOrden.Length = 6 Then
            final = "00" & nroOrden
        End If
        If nroOrden.Length = 7 Then
            final = "0" & nroOrden
        End If
        If nroOrden.Length = 8 Then
            final = nroOrden
        End If
        Return final
    End Function
#End Region

    Private Function almacenarOrden(ByRef nro As String, ByRef origen As String) As Boolean
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
                    cmd.Parameters.Add("@Tipo", SqlDbType.VarChar, 50).Value = "Contratos"
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

    Private Sub chkDisp_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkDisp.CheckedChanged
        If chkDisp.Checked Then
            chkRes.Enabled = False
        Else
            chkRes.Enabled = True
        End If
    End Sub

    Private Sub chkRes_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkRes.CheckedChanged
        If chkRes.Checked Then
            chkDisp.Enabled = False
        Else
            chkDisp.Enabled = True
        End If
    End Sub

    Private Function almacenarContrato(ByRef nroOrden As String) As Boolean
        Dim bandera As Boolean
        Try

            For Each row As DataGridViewRow In DataGridView3.Rows
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd As SqlCommand = conn.CreateCommand()

                        conn.Open()

                        cmd.CommandText = "insert into AAContratos(" +
                                       "[Orden]" +
                                        ",[Causa]" +
                                        ",[Nombre]" +
                                        ",[Detalle]" +
                                        ",[Monto]" +
                                        ",[Emitido]" +
                                        ",[Imputacion]" +
                                        ") VALUES (" +
                                        "@Orden" +
                                        ",@Causa" +
                                        ",@Nombre" +
                                        ",@Detalle" +
                                        ",@Monto" +
                                        ",@Emitido" +
                                        ",@Imputacion" +
                                        ")"
                        cmd.Parameters.Add("@Orden", SqlDbType.Int).Value = nroOrden
                        cmd.Parameters.Add("@Causa", SqlDbType.VarChar, 10).Value = txtCodigo.Text
                        cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 100).Value = row.Cells(0).Value
                        cmd.Parameters.Add("@Detalle", SqlDbType.VarChar, 100).Value = row.Cells(2).Value
                        cmd.Parameters.Add("@Monto", SqlDbType.Float).Value = row.Cells(3).Value
                        cmd.Parameters.Add("@Emitido", SqlDbType.VarChar, 2).Value = "SI"
                        cmd.Parameters.Add("@Imputacion", SqlDbType.VarChar, 50).Value = row.Cells(1).Value
                        cmd.ExecuteScalar()

                    End Using
                End Using
            Next
            MessageBox.Show("Los datos de los contratos fueron almacenados correctamente", "EXITO")
            bandera = True
        Catch ex As Exception
            MessageBox.Show("Los datos de los contratos no se almacenaron de manera correcta, por favor verifiquelos", "AVISO")
            bandera = False
        End Try
        Return bandera
    End Function

    Private Function almacenarSellos(ByRef NroOrden As String) As Boolean
        Dim bandera As Boolean

        Try

            If DataGridView4.Rows.Count > 0 Then
                For Each row As DataGridViewRow In DataGridView4.Rows
                    Using conn As New SqlConnection(CONNSTR)
                        Using cmd As SqlCommand = conn.CreateCommand()

                            conn.Open()

                            cmd.CommandText = "insert into AAImpuestoSello(" +
                                           "[Orden]" +
                                           ",[Causa]" +
                                           ",[Nombre]" +
                                           ",[Detalle]" +
                                           ",[Impuesto]" +
                                            ") VALUES (" +
                                           "@Orden" +
                                           ",@Causa" +
                                           ",@Nombre" +
                                           ",@Detalle" +
                                           ",@Impuesto" +
                                            ")"
                            cmd.Parameters.Add("@Orden", SqlDbType.Int).Value = NroOrden
                            cmd.Parameters.Add("@Causa", SqlDbType.VarChar, 10).Value = txtCodigo.Text
                            cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = row.Cells(0).Value
                            cmd.Parameters.Add("@Detalle", SqlDbType.VarChar, 50).Value = row.Cells(1).Value
                            cmd.Parameters.Add("@Impuesto", SqlDbType.Float).Value = row.Cells(2).Value

                            cmd.ExecuteScalar()

                        End Using
                    End Using
                Next
                MessageBox.Show("Los datos de los Impuestos al sello fueron almacenados correctamente", "EXITO")
                bandera = True
            Else

            End If
        Catch ex As Exception
            MessageBox.Show("Los datos de los Impuestos al sello no se almacenaron de manera correcta, por favor verifiquelos", "AVISO")
            bandera = False
        End Try
        Return bandera
    End Function

    Private Sub AgregarMonto()
        Dim Total As Double
        For Each row As DataGridViewRow In DataGridView3.Rows
            Total = Total + row.Cells("Monto").Value
        Next
        Total2.Text = Total
    End Sub

    Private Sub AgregarMonto2()
        Dim Total As Double
        For Each row As DataGridViewRow In DataGridView3.Rows
            Total = Total + row.Cells("Importe").Value
        Next
        Total2.Text = Total
    End Sub

    Private Sub AgregarMontoSello()
        Dim Total As Double
        If ChkNuevo.Checked Then
            For Each row As DataGridViewRow In DataGridView4.Rows
                Total = Total + row.Cells(2).Value
            Next
        End If
        'If chkHistorial.Checked And chkAutorizado.Checked Then
        '    For Each row As DataGridViewRow In DataGridView4.Rows
        '        Total = Total + row.Cells(2).Value
        '    Next
        'End If
        If chkHistorial.Checked Then
            For Each row As DataGridViewRow In DataGridView4.Rows
                Total = Total + row.Cells(2).Value
            Next
        End If
        TotalSellos2.Text = Total
    End Sub

    Private Sub DataGridView1_CellContentDoubleClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick

    End Sub

    Private Sub DataGridView1_DoubleClick(sender As Object, e As System.EventArgs) Handles DataGridView1.DoubleClick
        Dim I As Integer = DataGridView1.CurrentCell.RowIndex
        Dim Emitido As String
        ' AsistenteContrato.txtCodigo.Text = txtCodigo.Text
        'AsistenteContrato.cmbAnio.Text = cmbAnio.SelectedItem.ToString
        'AsistenteContrato.txtNombre.Text = DataGridView1.Rows(I).Cells(0).Value
        'AsistenteContrato.txtCuit.Text = DataGridView1.Rows(I).Cells(4).Value
        'AsistenteContrato.cargarNombresRendicion()
        'AsistenteContrato.Show()

        'If ChkNuevo.Checked Then
        '    Try
        '        Emitido = DataGridView1.Rows(I).Cells(4).Value
        '        If Emitido.Equals("SI") Then
        '            MessageBox.Show("El elemento seleccionado ya fue emitido", "AVISO")
        '        Else

        If chkAutorizado.Checked Then
            Dim fila(5) As Object
            fila(0) = DataGridView1.Rows(I).Cells(0).Value
            fila(1) = DataGridView1.Rows(I).Cells(1).Value
            fila(2) = DataGridView1.Rows(I).Cells(2).Value
            fila(3) = DataGridView1.Rows(I).Cells(3).Value
            fila(4) = DataGridView1.Rows(I).Cells(4).Value
            DataGridView3.Rows.Add(fila)
            AgregarMonto2()
        Else
            Dim fila(5) As Object
            fila(0) = DataGridView1.Rows(I).Cells(0).Value
            fila(1) = DataGridView1.Rows(I).Cells(1).Value
            fila(2) = DataGridView1.Rows(I).Cells(2).Value
            fila(3) = DataGridView1.Rows(I).Cells(3).Value
            fila(4) = DataGridView1.Rows(I).Cells(4).Value
            DataGridView3.Rows.Add(fila)
            AgregarMonto()
        End If
        '   End If
        'Catch ex As Exception
        '        If chkAutorizado.Checked Then
        '            Dim fila(5) As Object
        '            fila(0) = DataGridView1.Rows(I).Cells(0).Value
        '            fila(1) = DataGridView1.Rows(I).Cells(1).Value
        '            fila(2) = DataGridView1.Rows(I).Cells(2).Value
        '            fila(3) = DataGridView1.Rows(I).Cells(3).Value
        '            fila(4) = DataGridView1.Rows(I).Cells(4).Value
        '            DataGridView3.Rows.Add(fila)
        '            AgregarMonto2()
        '        Else
        '            Dim fila(5) As Object
        '            fila(0) = DataGridView1.Rows(I).Cells(0).Value
        '            fila(1) = DataGridView1.Rows(I).Cells(1).Value
        '            fila(2) = DataGridView1.Rows(I).Cells(2).Value
        '            fila(3) = DataGridView1.Rows(I).Cells(3).Value
        '            fila(4) = DataGridView1.Rows(I).Cells(4).Value
        '            DataGridView3.Rows.Add(fila)
        '            AgregarMonto()
        '        End If
        '    End Try
        'Else
        '    If chkHistorial.Checked Then
        '        txtNroOrdenHist.Text = DataGridView1.Rows(I).Cells(0).Value
        '        Dim fila(5) As Object
        '        fila(0) = DataGridView1.Rows(I).Cells(0).Value
        '        fila(1) = DataGridView1.Rows(I).Cells(1).Value
        '        fila(2) = DataGridView1.Rows(I).Cells(2).Value
        '        fila(3) = DataGridView1.Rows(I).Cells(3).Value
        '        fila(4) = DataGridView1.Rows(I).Cells(4).Value
        '        fila(5) = DataGridView1.Rows(I).Cells(5).Value
        '        DataGridView3.Rows.Add(fila)
        '        AgregarMonto()
        '    End If
        'End If


    End Sub

    Private Sub DataGridView2_DoubleClick(sender As Object, e As System.EventArgs) Handles DataGridView2.DoubleClick
        Dim I As Integer = DataGridView2.CurrentCell.RowIndex
        Dim Emitido As String

        Try
            Emitido = DataGridView2.Rows(I).Cells(3).Value
            If Emitido.Equals("SI") Then
                MessageBox.Show("El elemento seleccionado ya fue emitido", "AVISO")
            Else
                If chkAutorizado.Checked Then
                    Dim fila(5) As Object
                    fila(0) = DataGridView2.Rows(I).Cells(0).Value
                    fila(1) = DataGridView2.Rows(I).Cells(1).Value
                    fila(2) = DataGridView2.Rows(I).Cells(2).Value
                    fila(3) = DataGridView2.Rows(I).Cells(3).Value
                    fila(4) = DataGridView2.Rows(I).Cells(4).Value
                    DataGridView4.Rows.Add(fila)
                    AgregarMontoSello()
                Else
                    Dim fila(4) As Object
                    fila(0) = DataGridView2.Rows(I).Cells(0).Value
                    fila(1) = DataGridView2.Rows(I).Cells(1).Value
                    fila(2) = DataGridView2.Rows(I).Cells(2).Value
                    'fila(3) = DataGridView2.Rows(I).Cells(3).Value
                    DataGridView4.Rows.Add(fila)
                    AgregarMontoSello()
                End If
            End If
        Catch ex As Exception
            If chkAutorizado.Checked Then
                Dim fila(4) As Object
                fila(0) = DataGridView2.Rows(I).Cells(0).Value
                fila(1) = DataGridView2.Rows(I).Cells(1).Value
                fila(2) = DataGridView2.Rows(I).Cells(2).Value
                fila(3) = DataGridView2.Rows(I).Cells(3).Value
                DataGridView4.Rows.Add(fila)
                AgregarMontoSello()
            Else
                Dim fila(4) As Object
                fila(0) = DataGridView2.Rows(I).Cells(0).Value
                fila(1) = DataGridView2.Rows(I).Cells(1).Value
                fila(2) = DataGridView2.Rows(I).Cells(2).Value
                'fila(3) = DataGridView2.Rows(I).Cells(3).Value
                DataGridView4.Rows.Add(fila)
                AgregarMontoSello()
            End If
        End Try

    End Sub

    Private Sub chkHistorial_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkHistorial.CheckedChanged
        If chkHistorial.Checked Then
            ChkNuevo.Enabled = False
            historialOrden.Visible = True
        Else
            ChkNuevo.Enabled = True
            historialOrden.Visible = False
        End If
    End Sub

    Private Sub ChkNuevo_CheckedChanged(sender As Object, e As System.EventArgs) Handles ChkNuevo.CheckedChanged
        If ChkNuevo.Checked Then
            chkHistorial.Enabled = False
        Else
            chkHistorial.Enabled = True
        End If
    End Sub

#Region "HISTORICO"

    Private Sub traerHistorico()

        DataGridView1.Columns.Clear()
        DataGridView1.Rows.Clear()
        DataGridView1.Columns.Add("Orden", "ORDEN")
        DataGridView1.Columns.Add("Causa", "CAUSA")
        DataGridView1.Columns.Add("detalle", "DETALLE")
        DataGridView1.Columns.Add("Imputacion", "IMPUTACION")
        DataGridView1.Columns.Add("factura", "FACTURA")
        DataGridView1.Columns.Add("monto", "MONTO")
        DataGridView1.Columns.Item("MONTO").DefaultCellStyle.Format = "#####0.00"

        sql = " SELECT distinct  Orden,Causa,Nombre,Detalle,Monto,Imputacion,Codigo FROM AAContratos where Causa='" & txtCodigo.Text & "' and Orden='" & txtNroOrdenHist.Text & "'order by Codigo,Orden,Causa "
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Dim fila(6) As Object
                While reader.Read()

                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    fila(2) = reader(2)
                    fila(3) = reader(5)
                    fila(4) = reader(3)
                    fila(5) = reader(4)
                    DataGridView1.Rows.Add(fila)
                End While

            End Using
        End Using

        'DataGridView2.Columns.Clear()
        'DataGridView2.Rows.Clear()
        'DataGridView2.Columns.Add("Orden", "ORDEN NRO")
        'DataGridView2.Columns.Add("Causa", "CAUSA")
        'DataGridView2.Columns.Add("Nombre", "NOMBRE")
        'DataGridView2.Columns.Add("detalle", "DETALLE")
        'DataGridView2.Columns.Add("Impuesto", "IMPUESTO")
        'DataGridView2.Columns.Item("Impuesto").DefaultCellStyle.Format = "#####0.00"


        'sql = " SELECT distinct Orden,Causa,Nombre,Detalle,Impuesto from AAImpuestoSello where Causa='" & txtCodigo.Text & "'"
        'Using conn As New SqlConnection(CONNSTR)
        '    Using cmd4 As SqlCommand = conn.CreateCommand()
        '        conn.Open()
        '        cmd4.CommandText = sql
        '        Dim reader As SqlDataReader = Nothing
        '        reader = cmd4.ExecuteReader


        '        Dim count As Integer
        '        count = 0
        '        Do While reader.Read()
        '            Dim fila(5) As Object
        '            fila(0) = reader(0)
        '            fila(1) = reader(1)
        '            fila(2) = reader(2)
        '            fila(3) = reader(3)
        '            fila(4) = reader(4)
        '            DataGridView2.Rows.Add(fila)
        '        Loop


        '    End Using
        'End Using

    End Sub

    Private Sub cargarOrdenHist()

        DataGridView3.Columns.Clear()
        DataGridView3.Rows.Clear()

        DataGridView3.Columns.Add("Orden", "ORDEN")
        DataGridView3.Columns.Add("Causa", "CAUSA")
        DataGridView3.Columns.Add("detalle", "DETALLE")
        DataGridView3.Columns.Add("Imputacion", "IMPUTACION")
        DataGridView3.Columns.Add("factura", "FACTURA")
        DataGridView3.Columns.Add("monto", "MONTO")
        DataGridView3.Columns.Item("MONTO").DefaultCellStyle.Format = "#####0.00"

        sql = " SELECT  Orden,Causa,Nombre,Detalle,Monto,Imputacion FROM AAContratos where Orden='" & txtNroOrdenHist.Text & "'"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    Dim fila(6) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    fila(2) = reader(2)
                    fila(3) = reader(5)
                    fila(4) = reader(3)
                    fila(5) = reader(4)
                    DataGridView3.Rows.Add(fila)
                End While

            End Using
        End Using


        'DataGridView4.Columns.Clear()
        'DataGridView4.Rows.Clear()
        'DataGridView4.Columns.Add("Orden", "ORDEN NRO")
        'DataGridView4.Columns.Add("Causa", "CAUSA")
        'DataGridView4.Columns.Add("Nombre", "NOMBRE")
        'DataGridView4.Columns.Add("detalle", "DETALLE")
        'DataGridView4.Columns.Add("Impuesto", "IMPUESTO")
        'DataGridView4.Columns.Item("Impuesto").DefaultCellStyle.Format = "#####0.00"
        If chkAutorizado.Checked Then
            sql = " SELECT distinct Orden,Causa,Nombre,Detalle,Impuesto from AAImpuestoSello where Causa='" & txtCodigo.Text & "' and Orden='" & txtNroOrdenHist.Text & "'"
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
                        fila(0) = reader(0)
                        fila(1) = reader(1)
                        fila(2) = reader(2)
                        fila(3) = reader(3)
                        fila(4) = reader(4)
                        DataGridView4.Rows.Add(fila)
                    Loop

                End Using
            End Using
        Else
            sql = " SELECT distinct Orden,Causa,Nombre,Detalle,Impuesto from AAImpuestoSello where Causa='" & txtCodigo.Text & "' and Orden='" & txtNroOrdenHist.Text & "'"
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
                        fila(0) = reader(2)
                        fila(1) = reader(3)
                        fila(2) = reader(4)

                        DataGridView4.Rows.Add(fila)
                    Loop

                End Using
            End Using
        End If
        AgregarMonto()
        AgregarMontoSello()
    End Sub

#End Region

    Private Sub btnHistCargar_Click(sender As System.Object, e As System.EventArgs) Handles btnHistCargar.Click
        If txtNroOrdenHist.Text.Equals("") Then
            MessageBox.Show("Debe seleccionar una orden del grid", "AVISO")
        Else
            cargarOrdenHist()
        End If
    End Sub

    Private Sub chkAutorizado_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkAutorizado.CheckedChanged
        If chkAutorizado.Checked Then
            GroupBox8.Visible = True
        Else
            GroupBox8.Visible = False
        End If
    End Sub

    Private Sub RetencionPrevia_DoubleClick(sender As Object, e As System.EventArgs) Handles RetencionPrevia.DoubleClick
        Dim I As Integer = RetencionPrevia.CurrentCell.RowIndex

        If ChkNuevo.Checked Then

            Dim fila(5) As Object
            fila(0) = RetencionPrevia.Rows(I).Cells(0).Value
            fila(1) = RetencionPrevia.Rows(I).Cells(1).Value
            fila(2) = RetencionPrevia.Rows(I).Cells(2).Value
            fila(3) = RetencionPrevia.Rows(I).Cells(3).Value
            fila(4) = RetencionPrevia.Rows(I).Cells(4).Value
            DataGridView5.Rows.Add(fila)
            AgregarMonto3()

        Else

        End If

    End Sub

    Private Sub AgregarMonto3()
        Dim Total As Double
        For Each row As DataGridViewRow In DataGridView5.Rows
            Total = Total + row.Cells(4).Value
        Next
        txtRetencion.Text = Total
    End Sub

    Private Sub chkProv_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkProv.CheckedChanged
        If chkProv.Checked Then
            sql = "Select tal_ActualNro from Talonar where tal_Cod='OPP'"

            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()

                    conn.Open()

                    cmd4.CommandText = sql

                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    reader.Read()
                    txtSaliente.Text = reader.Item(0) + 1

                End Using
            End Using
        Else
            txtSaliente.Text = "0"
        End If
    End Sub

    Private Sub chkNac_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkNac.CheckedChanged
        If chkNac.Checked Then
            sql = "Select tal_ActualNro from Talonar where tal_Cod='OPN'"

            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()

                    conn.Open()

                    cmd4.CommandText = sql

                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    reader.Read()
                    txtSaliente.Text = reader.Item(0) + 1

                End Using
            End Using
        Else
            txtSaliente.Text = "0"
        End If
    End Sub

    Private Sub DataGridView3_RowsRemoved(sender As Object, e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles DataGridView3.RowsRemoved
        Total2.Text = 0
        For Each row As DataGridViewRow In DataGridView3.Rows
            If ChkNuevo.Checked Then
                If chkAutorizado.Checked Then
                    Total2.Text = Total2.Text + row.Cells(4).Value
                Else
                    Total2.Text = Total2.Text + row.Cells(3).Value
                End If

            End If
        Next
    End Sub

    Private Sub DataGridView4_RowsRemoved(sender As Object, e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles DataGridView4.RowsRemoved
        TotalSellos2.Text = 0
        For Each row As DataGridViewRow In DataGridView4.Rows
            TotalSellos2.Text = TotalSellos2.Text + row.Cells(2).Value
        Next
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Recalcular.Click

        'xxxxxxxxxxxxxxxxxxxxxxxxx
        Dim Total As Single
        Dim Col As Integer = Me.DataGridView3.CurrentCell.ColumnIndex
        For Each row As DataGridViewRow In Me.DataGridView3.Rows
            Total += Val(row.Cells(Col).Value)
        Next
        Total2.Text = Total.ToString

        'xxxxxxxxxxxxxxxxxxxxxxxxx

    End Sub

    Private Sub Recalcular_DoubleClick(sender As Object, e As System.EventArgs) Handles Recalcular.DoubleClick

    End Sub

End Class