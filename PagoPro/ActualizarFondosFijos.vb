Imports System.Data.SqlClient
Imports System.Data.Odbc
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.Image
Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet
Public Class ActualizarFondosFijos
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

    Public Sub cargarCombo()
        sql = "select Descripcion from AASituaciones where  Descripcion like '%Fondos%'"

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










    Public Sub cargarDataAlquiler()


        DataGridView1.Columns.Add("fecha", "FECHA")
        DataGridView1.Columns.Add("comprobante", "COMPROBANTE")
        DataGridView1.Columns.Add("detalle", "DETALLE")
        DataGridView1.Columns.Add("monto", "MONTO")


        adjuntarDatos()
        'cargarNombres()
        calculoTotalAlquiler()
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
        sql = "SELECT [chp_NroCheq],[chpctb_Cod],[chp_Importe],[pro_RazSoc],[pro_CUIT]  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
             "inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
             "inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]" &
             "where CausaEmi.cem_Desc='" & adjuntarAnio() & "'"

        DataGridView1.Columns.Add("nombre", "IMPORTE ")
        DataGridView1.Columns.Add("ctaCte", "CUENTA CORRIENTE")
        DataGridView1.Columns.Add("dias", "CHEQUES")
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
                    fila(0) = reader(2)
                    fila(1) = reader(1)
                    fila(2) = reader(0)
                    fila(3) = reader(4) & "." & reader(3)

                    DataGridView1.Rows.Add(fila)

                Loop

            End Using
        End Using
        'calculoMonto()
        'calculoTotalAlquiler()
    End Sub

    Public Sub calculoMonto()
        Dim monto As Double


        For Each row As DataGridViewRow In DataGridView1.Rows
            monto = 0

            For Each column As DataGridViewColumn In DataGridView1.Columns
                Try
                    monto += row.Cells(column.Index).Value
                Catch ex As Exception

                End Try

            Next

            row.Cells("MONTO").Value = monto
        Next
    End Sub

    Private Sub btnSalir_Click(sender As System.Object, e As System.EventArgs) Handles btnSalir.Click
        Me.Close()

    End Sub

    Private Sub chkProv_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkProv.CheckedChanged
        If chkProv.Checked Then
            chkNac.Enabled = False
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
            chkNac.Enabled = True
            txtSaliente.Text = "0"
        End If
    End Sub

    Private Sub chkNac_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkNac.CheckedChanged
        If chkNac.Checked Then
            chkProv.Enabled = False
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
            chkProv.Enabled = True
            txtSaliente.Text = "0"
        End If
    End Sub

    Private Sub btnImprimir_Click(sender As System.Object, e As System.EventArgs) Handles btnImprimir.Click

        If chkNac.Checked Or chkProv.Checked Then
            If txtCodigo.Text = Nothing Then
                MessageBox.Show("Debe ingresar un codigo valido", "AVISO")
            Else
                If cmbTipoOrden.Text.Equals("Fondos Fijos") Then
                    If chkDisp.Checked Or chkRes.Checked Then
                        If chkAutorizado.Checked Then
                            imprimirOrdenCargoChequesHistorial()
                        Else
                            Select Case MsgBox("¿Esta seguro que desea generar un pago?", MsgBoxStyle.YesNo, "AVISO")
                                Case MsgBoxResult.Yes
                                    'cargarApertura()
                                    impirmirOrdenCargoPrimero()
                                    'imprimirPago()
                                Case MsgBoxResult.No
                                    MessageBox.Show("Accion cancelada por el usuario", "INFORMACION")
                            End Select

                        End If
                    Else
                        MessageBox.Show("Debe seleccionar una opcion", "DISPOSICION O RESOLUCION??")
                    End If

                Else

                    If cmbTipoOrden.Text.Equals("Historial Fondos") Then
                        If chkAutorizado.Checked Then
                            imprimirOrdenCargoChequesHistorial()
                        Else
                            impirmirOrdenCargoPrimeroHistorial()
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

    Private Sub btnBuscar_Click(sender As System.Object, e As System.EventArgs) Handles btnBuscar.Click
        DataGridView2.Rows.Clear()
        DataGridView2.Columns.Clear()
        DataGridView1.Rows.Clear()
        DataGridView1.Columns.Clear()
        If txtCodigo.Text = Nothing Then
            MessageBox.Show("Debe ingresar un codigo valido", "AVISO")
        Else
            Try
                If cmbTipoOrden.SelectedItem = "Fondos Fijos" Then
                    If chkAutorizado.Checked Then
                        ordenCargo()
                        GroupBox5.Visible = True
                        cargarDataRendicion()
                    Else
                        adjuntarDatos()
                        GroupBox5.Visible = True
                        adjuntarDatosRendicionAlquiler()
                    End If
                Else
                    If cmbTipoOrden.SelectedItem = "Historial Fondos" Then
                        If chkAutorizado.Checked Then
                            ordenCargo()
                            GroupBox5.Visible = True

                            cargarDataRendicion()
                        Else

                            adjuntarDatos()
                            GroupBox5.Visible = True

                            adjuntarDatosRendicionAlquiler()
                        End If
                    Else
                        MessageBox.Show("Debe seleccionar un tipo de orden valido", "AVISO")
                    End If

                End If

            Catch ex As Exception
                MessageBox.Show("Por favor verifique el número de entrada", "ERROR")
            End Try
        End If
    End Sub

    Public Sub cargarDataRendicion()
        sql = "SELECT [chp_NroCheq],[chpctb_Cod],[chp_Importe],[pro_RazSoc],[chp_FVto],[pro_CUIT]  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
              "inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
              "inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]" &
              "where CausaEmi.cem_Desc='" & adjuntarAnio() & "'"

        DataGridView2.Columns.Add("fecha", "FECHA EMISION")
        DataGridView2.Columns.Add("destinatario", "BENEFICIARIO")
        DataGridView2.Columns.Add("ctaCte", "CUENTA CORRIENTE")
        DataGridView2.Columns.Add("dias", "NRO CHEQUE")
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
                    fila(1) = reader(3) & " DNI/CUIT/CUIL:" & reader(5)
                    fila(2) = reader(1)
                    fila(3) = reader(0)
                    fila(4) = reader(2)

                    DataGridView2.Rows.Add(fila)

                Loop

            End Using
        End Using
        calcularMonto2()
    End Sub
    Private Sub calcularMonto2()
        Dim Total As Double = 0
        For Each row As DataGridViewRow In Me.DataGridView2.Rows
            Total += row.Cells(4).Value
        Next
        txtTotalGastos.Text = Total.ToString
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
                    fila(0) = "Pago a proveedores"
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

    Public Sub imprimirPago()

        Dim origen As String

        If chkNac.Checked Then
            origen = "OPN"
        End If
        If chkProv.Checked Then
            origen = "OPP"
        End If
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

        Dim Documento As New Document 'Declaracion del documento
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

        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

        Documento.Add(New Paragraph(" ")) 'Salto de linea

        tablademo.SetWidthPercentage({200, 300, 70}, PageSize.A4) 'Ajusta el tamaño de cada columna

        tablademo.AddCell(New Paragraph("IMPUTACION        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        tablademo.AddCell(New Paragraph("DETALLE           ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
        tablademo.AddCell(New Paragraph("IMPORTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        Documento.Add(tablademo) 'Agrega la tabla al documento

        Dim tablademo2 As New PdfPTable(3) 'declara la tabla con 4 Columnas
        tablademo2.SetWidthPercentage({200, 300, 70}, PageSize.A4) 'Ajusta el tamaño de cada columna

        For Each row As DataGridViewRow In DataGridView1.Rows
            For Each column As DataGridViewColumn In DataGridView1.Columns
                If column.Index <= 3 Then
                    tablademo2.AddCell(New Paragraph(row.Cells(column.Index).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                End If

            Next

        Next
        Documento.Add(tablademo2) 'Agrega la tabla al documento

        Documento.Add(New Paragraph(" ")) 'Salto de linea

        Dim tablademo3 As New PdfPTable(3) 'declara la tabla con 4 Columnas
        tablademo3.SetWidthPercentage({200, 300, 70}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo3.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

        tablademo3.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        tablademo3.AddCell(New Paragraph(txtTotalFinal.Text)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        Documento.Add(tablademo3) 'Agrega la tabla al documento

        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea

        parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("        DEPTO CONTABLE  " & "                             " & "DIRECCION SERVICIO " & "                             " & vbCr & "         Y LIQUIDACIONES   " & "                              " & "ADMINISTRATIVO") 'Texto que se insertara

        'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        'parrafo.Add("Direccion Administracion " & vbCr & "Finanzas") 'Texto que se insertara

        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Dim tablademo5 As New PdfPTable(5) 'declara la tabla con 4 Columnas
        tablademo5.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo5.AddCell(New Paragraph("F. EMISION        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

        tablademo5.AddCell(New Paragraph("BENEFICIARIO        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo5.AddCell(New Paragraph("CH. Y/O AUT.        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo5.AddCell(New Paragraph("NRO CTA        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo5.AddCell(New Paragraph("IMPORTE        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        Documento.Add(tablademo5) 'Agrega la tabla al documento

        Dim tablademo4 As New PdfPTable(5) 'declara la tabla con 4 Columnas
        tablademo4.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

        For Each row As DataGridViewRow In DataGridView1.Rows
            For Each column As DataGridViewColumn In DataGridView1.Columns
                If column.Index <= 5 Then
                    tablademo4.AddCell(New Paragraph("________________")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                End If

            Next

        Next
        Documento.Add(tablademo4) 'Agrega la tabla al documento

        Documento.Add(New Paragraph(" ")) 'Salto de linea

        parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("RECIBI CONFORME EL IMPORTE:________________ " & vbCr & "ACLARACION DE FIRMA:________________________") 'Texto que se insertara

        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("DEPTO. TESORERIA") 'Texto que se insertara

        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

        Documento.Close() 'Cierra el documento
        System.Diagnostics.Process.Start("OrdenPago.pdf") 'Abre el archivo DEMO.PDF
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

    End Sub

    Public Sub cargarConceptos()
        sql = " SELECT ori_Cod, ori_Desc from Origenes"


        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader

                dataConceptos.Columns.Add("codigo", "CODIGO")
                dataConceptos.Columns.Add("descripcion", "DESCRIPCION")

                Do While reader.Read()
                    Dim fila(2) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    dataConceptos.Rows.Add(fila)
                Loop

            End Using
        End Using
    End Sub

    Public Sub cargarBancos()
        sql = " SELECT ctbbco_Cod, ctb_Cod,ctb_Desc from CtaBan"


        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader

                dataBancos.Columns.Add("codigo", "CODIGO")
                dataBancos.Columns.Add("nroCta", "NRO CTA")
                dataBancos.Columns.Add("descripcion", "DESCRIPCION")

                Do While reader.Read()
                    Dim fila(3) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    fila(2) = reader(2)

                    dataBancos.Rows.Add(fila)
                Loop

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

        sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='Fondos Fijos'"

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
            sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='Fondos Fijos'"
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

            Dim Documento As New Document 'Declaracion del documento
            Dim parrafo As New Paragraph ' Declaracion de un parrafo
            Dim tablademo As New PdfPTable(4) 'declara la tabla con 4 Columnas

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
            parrafo.Add("ORDEN DE PAGO NRO:" & nroCargo & "                             ") 'Texto que se insertara
            parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("FECHA:" & Date.Now) 'Texto que se insertara

            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente



            Documento.Add(New Paragraph(" ")) 'Salto de linea



            Documento.Add(New Paragraph(" ")) 'Salto de linea

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
                        tablademo4.AddCell(New Paragraph(row1.Cells(column.Index).Value, FontFactory.GetFont("Arial", 10))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                    End If

                Next

            Next
            Documento.Add(tablademo4) 'Agrega la tabla al documento
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            tablademo.SetWidthPercentage({270, 100, 100, 120}, PageSize.A4) 'Ajusta el tamaño de cada columna

            tablademo.AddCell(New Paragraph("IMPORTE EXPRESADOS EN LETRAS", FontFactory.GetFont("Arial", 9))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

            tablademo.AddCell(New Paragraph("CUENTA CORRIENTE", FontFactory.GetFont("Arial", 9))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
            tablademo.AddCell(New Paragraph("NRO DE CHEQUE", FontFactory.GetFont("Arial", 9))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
            tablademo.AddCell(New Paragraph("DESTINATARIO", FontFactory.GetFont("Arial", 9))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

            Documento.Add(tablademo) 'Agrega la tabla al documento

            Dim tablademo2 As New PdfPTable(4) 'declara la tabla con 4 Columnas
            tablademo2.SetWidthPercentage({270, 100, 100, 120}, PageSize.A4) 'Ajusta el tamaño de cada columna

            For Each row As DataGridViewRow In DataGridView1.Rows
                For Each column As DataGridViewColumn In DataGridView1.Columns
                    If column.Index <= 4 Then
                        tablademo2.AddCell(New Paragraph(row.Cells(column.Index).Value, FontFactory.GetFont("Arial", 10))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                    End If

                Next

            Next
            Documento.Add(tablademo2) 'Agrega la tabla al documento


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
            'If nroCargo.ToString.Length = 1 Then
            '    nroCargo = "0000000" & nroCargo
            'End If
            'If nroCargo.ToString.Length = 2 Then
            '    nroCargo = "000000" & nroCargo
            'End If
            'If nroCargo.ToString.Length = 3 Then
            '    nroCargo = "00000" & nroCargo
            'End If
            'If nroCargo.ToString.Length = 4 Then
            '    nroCargo = "0000" & nroCargo
            'End If
            'If nroCargo.ToString.Length = 5 Then
            '    nroCargo = "000" & nroCargo
            'End If
            'If nroCargo.ToString.Length = 6 Then
            '    nroCargo = "00" & nroCargo
            'End If
            'If nroCargo.ToString.Length = 7 Then
            '    nroCargo = "0" & nroCargo
            'End If
            'If nroCargo.ToString.Length = 8 Then
            '    nroCargo = nroCargo
            'End If
            'sql = "update Talonar set tal_ActualNro='" & nroCargo & "' where tal_Cod='" & origen & "'"
            'Using conn As New SqlConnection(CONNSTR)
            '    Using cmd4 As SqlCommand = conn.CreateCommand()

            '        conn.Open()

            '        cmd4.CommandText = sql
            '        cmd4.ExecuteScalar()

            '    End Using
            'End Using

            Documento.Close() 'Cierra el documento
            System.Diagnostics.Process.Start("OrdenCargo.pdf") 'Abre el archivo DEMO.PDF
            'almacenarOrden(nroCargo, origen)
        End If

    End Sub

    Public Sub impirmirOrdenCargoPrimero()

        Dim origen As String

        If chkNac.Checked Then
            origen = "OPN"
        End If
        If chkProv.Checked Then
            origen = "OPP"
        End If
        'sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='Fondos Fijos'"
        sql = "Select Concepto from AAOrdenNro where NroIngreso like '%" & cmbAnio.Text & "%' and NacPro='" & origen & "'"
        'se declara la variable bandera de tipo boolean
        Dim BANDERA As Boolean = False

        'se conecta con la base de datos y ejecuta la consulta
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read() And BANDERA = False
                    'pregunta si el concepto obtenido esta vacio se le asigna falso a la variable booleana
                    If reader(0).ToString.Equals("") Or reader(0).ToString.Equals(Nothing) Then
                        BANDERA = False
                    End If
                    ''si tiene un valor null se le asgina false a la variable booleana 
                    If IsDBNull(reader(0)) Then
                        BANDERA = False
                    End If
                    'MessageBox.Show(reader(0))
                    If (reader(0).ToString = txtSaliente.Text) Then
                        BANDERA = True
                    Else
                        BANDERA = False
                    End If
                End While
            End Using
        End Using

        '        Dim BANDERA As Boolean

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
            '    'en caso de seleccionar un si
            '    Case MsgBoxResult.Yes
            '        'Elimina la tupla existente en la base de datos y despues inserta la nueva orden de pago
            '        reemplazarImprimirFondoFijo()
            '    Case MsgBoxResult.No
            '        MessageBox.Show("Accion cancelada por el usuario", "INFORMACION")
            'End Select
            'impirmirOrdenCargoPrimeroHistorial()
            MessageBox.Show("YA EXISTE UNA ORDEN CON ESE NUMERO")
        Else
            Dim nroCargo As String
            nroCargo = txtSaliente.Text
            Dim nro As String
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

                sql = "update Talonar set tal_ActualNro='" & nro & "' where tal_Cod='" & origen & "'"
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

            If almacenarOrdenPago(nroCargo, origen) Then

                Try
                    Dim Documento As New Document(PageSize.LEGAL, 60, 5, 35, 5) 'Declaracion del documento
                    Dim parrafo As New Paragraph ' Declaracion de un parrafo
                    Dim tablademo As New PdfPTable(4) 'declara la tabla con 4 Columnas
                    Dim nro2 As String

                    pdf.PdfWriter.GetInstance(Documento, New FileStream("OrdenCargo.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

                    Documento.Open() 'Abre documento para su escritura

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
                    parrafo.Add("ORDEN DE PAGO NRO:" & nroCargo & "                             ") 'Texto que se insertara
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
                    Documento.Add(New Paragraph(" DETALLES DE LOS FONDOS: ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Dim tablademo6 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                    tablademo6.HorizontalAlignment = ALIGN_LEFT
                    tablademo6.SetWidthPercentage({100, 150, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                    tablademo6.AddCell(New Paragraph("FECHA        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                    tablademo6.AddCell(New Paragraph("COMPROBANTE")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                    tablademo6.AddCell(New Paragraph("DETALLE       ")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10


                    tablademo6.AddCell(New Paragraph("IMPORTE")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

                    Documento.Add(tablademo6) 'Agrega la tabla al documento
                    Dim tablademo4 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                    tablademo4.HorizontalAlignment = ALIGN_LEFT
                    tablademo4.SetWidthPercentage({100, 150, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

                    For Each row1 As DataGridViewRow In DataGridView2.Rows
                        For Each column As DataGridViewColumn In DataGridView2.Columns
                            If column.Index <= 4 Then
                                Dim paraParrrafo As String

                                If IsNumeric(row1.Cells(column.Index).Value) And Not column.Index = 1 Then

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

                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    sql = "SELECT [sdc_Desc] FROM [SBDASIPT].[dbo].[SegCabC] inner join [SBDASIPT].dbo.[SegDetC] ON SegCabC.scc_ID=SegDetC.sdcscc_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=SegCabC.scccem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio()
                    Using conn As New SqlConnection(CONNSTR)
                        Using cmd4 As SqlCommand = conn.CreateCommand()
                            conn.Open()
                            cmd4.CommandText = sql
                            Dim reader As SqlDataReader = Nothing
                            reader = cmd4.ExecuteReader
                            Do While reader.Read()
                                Dim Imp As String
                                Imp = " IMPUTACION: " & reader(0)
                                Documento.Add(New Paragraph(Imp)) 'Salto de linea
                            Loop

                        End Using
                    End Using

                    Documento.Add(New Paragraph(" ")) 'Salto de linea



                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL")) 'Salto de linea

                    Documento.Close() 'Cierra el documento
                    System.Diagnostics.Process.Start("OrdenCargo.pdf") 'Abre el archivo DEMO.PDF

                Catch ex As Exception
                    MessageBox.Show("Compruebe que no se encuentra otro documento abierto", "AVISO")
                End Try

            End If

        End If

    End Sub
    Public Sub reemplazarImprimirFondoFijo()
        Dim sql As String
        sql = "Select Id from AAOrdenNro where Tipo='FFijosPago' and NroIngreso=" & adjuntarAnio()
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
        reemplazarFondosFijos()
        MessageBox.Show("El remplazo se logro de manera correcta")
    End Sub
    Public Sub reemplazarFondosFijos()
        Dim origen As String

        If chkNac.Checked Then
            origen = "OPN"
        End If
        If chkProv.Checked Then
            origen = "OPP"
        End If
        sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='Fondos Fijos'"

        Dim BANDERA As Boolean

        'Using conn As New SqlConnection(CONNSTR)
        '    Using cmd4 As SqlCommand = conn.CreateCommand()

        '        conn.Open()

        '        cmd4.CommandText = sql

        '        Dim reader As SqlDataReader = Nothing
        '        reader = cmd4.ExecuteReader
        '        If reader.Read() Then
        '            '            If reader(0).ToString.Equals("") Or reader(0).ToString.Equals(Nothing) Then
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
        'If BANDERA Then

        '    'impirmirOrdenCargoPrimeroHistorial()
        ' Else
        Dim nroCargo As String
        nroCargo = txtSaliente.Text
        Dim nro As String
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

            sql = "update Talonar set tal_ActualNro='" & nro & "' where tal_Cod='" & origen & "'"
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

        If almacenarOrdenPago(nroCargo, origen) Then

            Try
                Dim Documento As New Document(PageSize.LEGAL, 60, 5, 35, 5) 'Declaracion del documento
                Dim parrafo As New Paragraph ' Declaracion de un parrafo
                Dim tablademo As New PdfPTable(4) 'declara la tabla con 4 Columnas
                Dim nro2 As String

                pdf.PdfWriter.GetInstance(Documento, New FileStream("OrdenCargo.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

                Documento.Open() 'Abre documento para su escritura

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
                parrafo.Add("ORDEN DE PAGO NRO:" & nroCargo & "                             ") 'Texto que se insertara
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
                Documento.Add(New Paragraph(" DETALLES DE LOS FONDOS: ")) 'Salto de linea
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                Dim tablademo6 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                tablademo6.HorizontalAlignment = ALIGN_LEFT
                tablademo6.SetWidthPercentage({100, 150, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                tablademo6.AddCell(New Paragraph("FECHA        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                tablademo6.AddCell(New Paragraph("COMPROBANTE")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                tablademo6.AddCell(New Paragraph("DETALLE       ")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10


                tablademo6.AddCell(New Paragraph("IMPORTE")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

                Documento.Add(tablademo6) 'Agrega la tabla al documento
                Dim tablademo4 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                tablademo4.HorizontalAlignment = ALIGN_LEFT
                tablademo4.SetWidthPercentage({100, 150, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

                For Each row1 As DataGridViewRow In DataGridView2.Rows
                    For Each column As DataGridViewColumn In DataGridView2.Columns
                        If column.Index <= 4 Then
                            Dim paraParrrafo As String

                            If IsNumeric(row1.Cells(column.Index).Value) And Not column.Index = 1 Then

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

                Documento.Add(New Paragraph(" ")) 'Salto de linea
                sql = "SELECT [sdc_Desc] FROM [SBDASIPT].[dbo].[SegCabC] inner join [SBDASIPT].dbo.[SegDetC] ON SegCabC.scc_ID=SegDetC.sdcscc_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=SegCabC.scccem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio()
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()
                        conn.Open()
                        cmd4.CommandText = sql
                        Dim reader As SqlDataReader = Nothing
                        reader = cmd4.ExecuteReader
                        Do While reader.Read()
                            Dim Imp As String
                            Imp = " IMPUTACION: " & reader(0)
                            Documento.Add(New Paragraph(Imp)) 'Salto de linea
                        Loop

                    End Using
                End Using

                Documento.Add(New Paragraph(" ")) 'Salto de linea



                Documento.Add(New Paragraph(" ")) 'Salto de linea
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                Documento.Add(New Paragraph("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL")) 'Salto de linea

                Documento.Close() 'Cierra el documento
                System.Diagnostics.Process.Start("OrdenCargo.pdf") 'Abre el archivo DEMO.PDF

            Catch ex As Exception
                MessageBox.Show("Compruebe que no se encuentra otro documento abierto", "AVISO")
            End Try

        End If

        '        End If

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

    Public Sub ordenCargo()
        sql = "SELECT [chp_NroCheq],[chpctb_Cod],[chp_Importe],[pro_RazSoc]  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
              "inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
              "inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]" &
              "where CausaEmi.cem_Desc='" & adjuntarAnio() & "'"

        DataGridView1.Columns.Add("nombre", "IMPORTE EN LETRAS EXPRESADO EN PESOS")
        DataGridView1.Columns.Add("ctaCte", "CUENTA CORRIENTE")
        DataGridView1.Columns.Add("dias", "CHEQUES")
        DataGridView1.Columns.Add("destinatario", "DESTINATARIO")
        DataGridView1.Columns.Add("recibo", "RECIBI CONFORME")



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
                    fila(4) = ""
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


        DataGridView1.Columns.Add("fecha", "FECHA")
        DataGridView1.Columns.Add("comprobante", "COMPROBANTE")
        DataGridView1.Columns.Add("detalle", "DETALLE")
        DataGridView1.Columns.Add("monto", "DEVENGADO")
        DataGridView1.Columns.Add("pagar", "A PAGAR")
        DataGridView1.Columns.Add("reintegrar", "A REINTEGRAR")
        DataGridView1.Columns.Add("firma", "FIRMA")


        adjuntarDatosRendicion()
        'calculoMonto()
        calculoTotalAlquilerRendicion()
    End Sub

    Public Sub adjuntarDatosRendicion()
        sql = "SELECT [cco_FEmision],[cco_Nro],[ccopro_RazSoc],[cco_ImpMonLoc] FROM [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " GROUP BY [cco_FEmision],[cco_Nro],[ccopro_RazSoc],[cco_ImpMonLoc]"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader

                Do While reader.Read()
                    Dim fila(7) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    fila(2) = reader(2)
                    fila(3) = reader(3) * (-1)
                    fila(4) = "0"
                    fila(5) = "0"
                    fila(6) = ""

                    DataGridView1.Rows.Add(fila)

                Loop
            End Using
        End Using

    End Sub

    Public Sub imprimirRendicion()
        Dim Documento As New Document()
        Dim parrafo As New Paragraph

        Documento.SetPageSize(iTextSharp.text.PageSize.A4.Rotate())
        pdf.PdfWriter.GetInstance(Documento, New FileStream("Rendicion.pdf", FileMode.Create))

        Documento.Open()
        sql = "Select tal_ActualNro from Talonar where tal_Cod='RVI'"
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



        'CUERPO DE LA TABLA

        For Each row As DataGridViewRow In DataGridView1.Rows

            'COMIENZO DEL ENCABEZADO

            parrafo.Alignment = Element.ALIGN_LEFT
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT)
            parrafo.Add("PROVINCIAL DE MISIONES" & vbCr & "SISTEMA PROVINCIAL" & vbCr & "DE TELEDUCACION Y DESARROLLO")

            Documento.Add(parrafo)

            parrafo.Clear()

            Dim imagendemo As iTextSharp.text.Image

            imagendemo = iTextSharp.text.Image.GetInstance("descarga.jpg")
            imagendemo.SetAbsolutePosition(740, 500)
            imagendemo.ScaleAbsoluteWidth(100)
            imagendemo.ScaleAbsoluteHeight(100)
            Documento.Add(imagendemo)

            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea



            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("ORDEN NRO:" & nroCargo & "  ")

            parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("NRO DE ENTRADA: " & txtCodigo.Text & "                         ") 'Texto que se insertara

            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("FECHA: " & Date.Now) 'Texto que se insertara

            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

            Documento.Add(New Paragraph(" ")) 'Salto de linea




            'FIN DEL ENCABEZADO

            'ENCABEZADO DE LA TABLA 

            Dim tablademo As New PdfPTable(6) 'declara la tabla con 4 Columnas
            tablademo.SetWidthPercentage({100, 140, 70, 70, 70, 70}, PageSize.A4)
            tablademo.HorizontalAlignment = Element.ALIGN_JUSTIFIED
            tablademo.AddCell(New Paragraph("NOMBRE        ", FontFactory.GetFont("Arial", 12)))
            tablademo.AddCell(New Paragraph("ALQUILER", FontFactory.GetFont("Arial", 12)))
            tablademo.AddCell(New Paragraph("DEVENGADO", FontFactory.GetFont("Arial", 12)))
            tablademo.AddCell(New Paragraph("A PAGAR", FontFactory.GetFont("Arial", 12)))
            tablademo.AddCell(New Paragraph("A REINTEGRAR", FontFactory.GetFont("Arial", 12)))
            tablademo.AddCell(New Paragraph("FIRMA", FontFactory.GetFont("Arial", 12)))

            Documento.Add(tablademo) 'Agrega la tabla al documento

            'FIN DEL ENCABEZADO DE LA TABLA

            Dim tablademo2 As New PdfPTable(6) 'declara la tabla con 4 Columnas
            tablademo2.SetWidthPercentage({100, 140, 70, 70, 70, 70}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tablademo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED

            For Each column As DataGridViewColumn In DataGridView1.Columns
                If column.Index <= 6 Then
                    tablademo2.AddCell(New Paragraph(row.Cells(column.Index).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                End If

            Next
            Documento.Add(tablademo2)



            Dim tablademo3 As New PdfPTable(6) 'declara la tabla con 4 Columnas

            tablademo3.SetWidthPercentage({100, 140, 70, 70, 70, 70}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tablademo3.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo3.AddCell(New Paragraph(row.Cells(1).Value.ToString)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
            If row.Cells(2).Value = Nothing Then
                tablademo3.AddCell(New Paragraph("0"))
            Else
                tablademo3.AddCell(New Paragraph(row.Cells(2).Value.ToString))
            End If
            If row.Cells(3).Value = Nothing Then
                tablademo3.AddCell(New Paragraph("0"))
            Else
                tablademo3.AddCell(New Paragraph(row.Cells(3).Value.ToString))
            End If
            If row.Cells(4).Value = Nothing Then
                tablademo3.AddCell(New Paragraph("0"))
            Else
                tablademo3.AddCell(New Paragraph(row.Cells(4).Value.ToString))
            End If


            If row.Cells(5).Value = Nothing Then
                tablademo3.AddCell(New Paragraph(""))
            Else
                tablademo3.AddCell(New Paragraph(row.Cells(5).Value.ToString))
            End If

            tablademo3.AddCell(New Paragraph("")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

            Documento.Add(tablademo3) 'Agrega la tabla al documento

            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea

            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado

            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("PLANILLA DE GASTOS") 'Texto que se insertara

            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea

            If row.Cells(1).Value = Nothing Or row.Cells(0).Value = Nothing Then
                Documento.Add(New Paragraph(" NO HUBO DECLARACION DE GASTOS ")) 'Salto de linea
            Else
                Dim tablademo4 As New PdfPTable(2) 'declara la tabla con 4 Columnas
                tablademo4.HorizontalAlignment = ALIGN_LEFT
                tablademo4.SetWidthPercentage({100, 300}, PageSize.A4) 'Ajusta el tamaño de cada columna

                For Each row1 As DataGridViewRow In DataGridView2.Rows
                    For Each column As DataGridViewColumn In DataGridView2.Columns
                        If column.Index <= 3 Then
                            tablademo4.AddCell(New Paragraph(row1.Cells(column.Index).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                        End If

                    Next

                Next
                Documento.Add(tablademo4) 'Agrega la tabla al documento
            End If
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea

            parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("          Encargado Rendicion " & "                             " & "Direccion Administracion " & vbCr & "Ordenes de Cargo         " & "                                  " & "Finanzas") 'Texto que se insertara

            'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
            'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            'parrafo.Add("Direccion Administracion " & vbCr & "Finanzas") 'Texto que se insertara

            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

            Documento.NewPage()
            If row.Cells("reintegrar").Value = 0 Or row.Cells("reintegrar").Value = Nothing Then

            Else
                'cargarReintegro(row.Index)
            End If

        Next

        'Agrega la tabla al documento

        'FIN DEL CUERPO DE LA TABLA

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
        sql = "update Talonar set tal_ActualNro='" & nroCargo & "' where tal_Cod='RVI'"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql
                cmd4.ExecuteScalar()

            End Using
        End Using


        Documento.Close() 'Cierra el documento
        System.Diagnostics.Process.Start("Rendicion.pdf")

    End Sub

    Public Sub adjuntarDatosRendicionAlquiler()
        DataGridView2.Columns.Add("fecha", "FECHA")
        DataGridView2.Columns.Add("comprobante", "COMPROBANTE")
        DataGridView2.Columns.Add("detalle", "DETALLE")
        DataGridView2.Columns.Add("monto", "MONTO")
        sql = "SELECT [cco_FEmision],[cco_Nro],[ccopro_RazSoc],[cco_ImpMonLoc],[ccopro_CUIT],[cco_CodPvt],[ccotco_Cod] FROM [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " GROUP BY [cco_FEmision],[cco_Nro],[ccopro_RazSoc],[cco_ImpMonLoc],[ccopro_CUIT],[cco_CodPvt],[ccotco_Cod] "
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

                    DataGridView2.Rows.Add(fila)




                Loop
            End Using
        End Using
        'calcularReintegros()
        'agregarReintegros()
        calculoTotalAlquilerRendicion()
    End Sub

    Public Sub calculoTotalAlquilerRendicion()

        Dim Total As Double = 0


        For Each row As DataGridViewRow In Me.DataGridView2.Rows
            Total += row.Cells(3).Value
        Next

        txtTotalGastos.Text = Total.ToString

    End Sub

    Public Sub calcularReintegros()
        Dim Total As Double = 0


        For Each row As DataGridViewRow In Me.DataGridView2.Rows
            Total += row.Cells("MONTO").Value
        Next

        txtTotalGastos.Text = Total.ToString
    End Sub

    Public Sub agregarReintegros()
        Dim total As Double = 0
        For Each row2 As DataGridViewRow In Me.DataGridView1.Rows
            total = 0
            For Each row As DataGridViewRow In Me.DataGridView2.Rows

                If row.Cells(3).Value = row2.Cells(0).Value Then
                    total += row.Cells("MONTO").Value

                End If
            Next
            If total > row2.Cells("MONTO").Value Then

                Dim Pago As Double = total - row2.Cells("MONTO").Value

                row2.Cells("pagar").Value = Pago

            Else
                If total < row2.Cells("MONTO").Value Then
                    Dim reintegro As Double = total - row2.Cells("MONTO").Value
                    row2.Cells("reintegrar").Value = reintegro
                Else
                    If total = row2.Cells("MONTO").Value Then
                        row2.Cells("reintegrar").Value = 0
                        row2.Cells("pagar").Value = 0
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub GenerarCompras_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        cargarCombo()
        GroupBox5.Visible = False
        GroupBox7.Visible = False
        GroupBox8.Visible = False
        cargarComboAnio()
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
                cmd.Parameters.Add("@Tipo", SqlDbType.VarChar, 50).Value = "FFijosPago"
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
                    cmd.Parameters.Add("@Tipo", SqlDbType.VarChar, 50).Value = "Fondos Fijos"
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

#Region "HISTORIAL"

    Public Sub imprimirOrdenCargoChequesHistorial()
        Dim fechona As String
        sql = "Select Concepto,Fecha from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='Fondos Fijos' "
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                If reader.Read() Then
                    fechona = reader(1).ToString
                End If
            End Using
        End Using
        Dim Documento As New Document 'Declaracion del documento
        Dim parrafo As New Paragraph ' Declaracion de un parrafo
        Dim tablademo As New PdfPTable(4) 'declara la tabla con 4 Columnas

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
        parrafo.Add("ORDEN DE PAGO NRO:" & dameNroHistorial("Fondos Fijos") & "                             ") 'Texto que se insertara
        parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("FECHA:" & fechona) 'Texto que se insertara(esto ya habiamos visto)

        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

        Documento.Add(New Paragraph(" ")) 'Salto de linea

        Documento.Add(New Paragraph(" ")) 'Salto de linea

        Dim tablademo5 As New PdfPTable(5) 'declara la tabla con 4 Columnas
        tablademo5.SetWidthPercentage({100, 180, 100, 100, 80}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo5.AddCell(New Paragraph("F. EMISION        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

        tablademo5.AddCell(New Paragraph("BENEFICIARIO        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo5.AddCell(New Paragraph("NRO CTA        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo5.AddCell(New Paragraph("CH. Y/O AUT.        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        tablademo5.AddCell(New Paragraph("IMPORTE        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        Documento.Add(tablademo5) 'Agrega la tabla al documento

        Dim tablademo4 As New PdfPTable(5) 'declara la tabla con 4 Columnas
        tablademo4.SetWidthPercentage({100, 180, 100, 100, 80}, PageSize.A4) 'Ajusta el tamaño de cada columna
        For Each row1 As DataGridViewRow In DataGridView2.Rows
            For Each column As DataGridViewColumn In DataGridView2.Columns
                If column.Index <= 5 Then
                    Dim paraParrrafo As String

                    If IsNumeric(row1.Cells(column.Index).Value) And Not column.Index = 3 Then

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
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        'tablademo.SetWidthPercentage({270, 100, 100, 120}, PageSize.A4) 'Ajusta el tamaño de cada columna

        'tablademo.AddCell(New Paragraph("IMPORTE EXPRESADOS EN LETRAS", FontFactory.GetFont("Arial", 9))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        'tablademo.AddCell(New Paragraph("CUENTA CORRIENTE", FontFactory.GetFont("Arial", 9))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
        'tablademo.AddCell(New Paragraph("NRO DE CHEQUE", FontFactory.GetFont("Arial", 9))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8


        'tablademo.AddCell(New Paragraph("DESTINATARIO", FontFactory.GetFont("Arial", 9))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        'Documento.Add(tablademo) 'Agrega la tabla al documento

        'Dim tablademo2 As New PdfPTable(4) 'declara la tabla con 4 Columnas
        'tablademo2.SetWidthPercentage({270, 100, 100, 120}, PageSize.A4) 'Ajusta el tamaño de cada columna

        'For Each row As DataGridViewRow In DataGridView1.Rows
        '    For Each column As DataGridViewColumn In DataGridView1.Columns
        '        If column.Index <= 4 Then
        '            Dim paraParrrafo As String

        '            If IsNumeric(row.Cells(column.Index).Value) And Not column.Index = 2 Then

        '                paraParrrafo = Format(row.Cells(column.Index).Value, "###,##0.00")
        '                Dim miCelda2 As New PdfPCell
        '                parrafo.Add(paraParrrafo)
        '                miCelda2.AddElement(parrafo)
        '                miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
        '                tablademo2.AddCell(miCelda2)
        '                parrafo.Clear()

        '            Else

        '                paraParrrafo = row.Cells(column.Index).Value
        '                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        '                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
        '                parrafo.Add(paraParrrafo) 'Texto que se insertara
        '                tablademo2.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        '                parrafo.Clear()

        '            End If
        '        End If

        '    Next

        'Next
        'Documento.Add(tablademo2) 'Agrega la tabla al documento
        Dim GranTotal As Double

        GranTotal = Double.Parse(txtTotalGastos.Text)
        Dim miGranTotal As String

        miGranTotal = Format(GranTotal, "###,##0.00")
        Documento.Add(New Paragraph("Total:$" & miGranTotal & " (Pesos " & Numalet.ToCardinal(GranTotal) & ")")) 'Salto de linea
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

    Public Sub impirmirOrdenCargoPrimeroHistorial()
        Dim fechona As String
        sql = "Select Concepto,Fecha from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='Fondos Fijos' "
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
        Dim Documento As New Document(PageSize.LEGAL, 60, 5, 35, 5) 'Declaracion del documento
        Dim parrafo As New Paragraph ' Declaracion de un parrafo
        Dim tablademo As New PdfPTable(4) 'declara la tabla con 4 Columnas

        pdf.PdfWriter.GetInstance(Documento, New FileStream("OrdenCargo.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

        Documento.Open() 'Abre documento para su escritura

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
        parrafo.Add("ORDEN DE PAGO NRO:" & dameNroHistorial("Fondos Fijos") & "                             ") 'Texto que se insertara
        parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("FECHA:" & fechona) 'Texto que se insertara(esta no toques)

        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" DETALLES DE LOS FONDOS: ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Dim tablademo6 As New PdfPTable(4) 'declara la tabla con 4 Columnas
        tablademo6.HorizontalAlignment = ALIGN_LEFT
        tablademo6.SetWidthPercentage({100, 150, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo6.AddCell(New Paragraph("FECHA        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        tablademo6.AddCell(New Paragraph("COMPROBANTE")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
        tablademo6.AddCell(New Paragraph("DETALLE       ")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10


        tablademo6.AddCell(New Paragraph("IMPORTE")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        Documento.Add(tablademo6) 'Agrega la tabla al documento
        Dim tablademo4 As New PdfPTable(4) 'declara la tabla con 4 Columnas
        tablademo4.HorizontalAlignment = ALIGN_LEFT
        tablademo4.SetWidthPercentage({100, 150, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

        For Each row1 As DataGridViewRow In DataGridView2.Rows
            For Each column As DataGridViewColumn In DataGridView2.Columns
                If column.Index <= 4 Then
                    Dim paraParrrafo As String

                    If IsNumeric(row1.Cells(column.Index).Value) And Not column.Index = 1 Then

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

        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" IMPUTACION Y DETALLE DEL RESPONSABLE: ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea

        tablademo.SetWidthPercentage({70, 150, 150, 250}, PageSize.A4) 'Ajusta el tamaño de cada columna

        tablademo.AddCell(New Paragraph("IMPORTE  ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        tablademo.AddCell(New Paragraph("CUENTA CORRIENTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
        tablademo.AddCell(New Paragraph("CHEQUE        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        tablademo.AddCell(New Paragraph("DESTINATARIO", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        Documento.Add(tablademo) 'Agrega la tabla al documento

        Dim tablademo2 As New PdfPTable(4) 'declara la tabla con 4 Columnas
        tablademo2.SetWidthPercentage({70, 150, 150, 250}, PageSize.A4) 'Ajusta el tamaño de cada columna

        For Each row As DataGridViewRow In DataGridView1.Rows
            For Each column As DataGridViewColumn In DataGridView1.Columns
                If column.Index <= 4 Then
                    Dim paraParrrafo As String

                    If IsNumeric(row.Cells(column.Index).Value) And Not column.Index = 1 Then

                        paraParrrafo = Format(row.Cells(column.Index).Value, "###,##0.00")
                        Dim miCelda2 As New PdfPCell
                        parrafo.Add(paraParrrafo)
                        miCelda2.AddElement(parrafo)
                        miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                        tablademo2.AddCell(miCelda2)
                        parrafo.Clear()

                    Else

                        paraParrrafo = row.Cells(column.Index).Value
                        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                        parrafo.Add(paraParrrafo) 'Texto que se insertara
                        tablademo2.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                        parrafo.Clear()

                    End If
                End If

            Next

        Next
        Documento.Add(tablademo2) 'Agrega la tabla al documento

        Documento.Add(New Paragraph(" ")) 'Salto de linea



        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL")) 'Salto de linea

        Documento.Close() 'Cierra el documento
        System.Diagnostics.Process.Start("OrdenCargo.pdf") 'Abre el archivo DEMO.PDF

    End Sub

#End Region

    Private Sub DataGridView2_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick

    End Sub

    Private Sub cmbTipoOrden_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbTipoOrden.SelectedIndexChanged

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

    Private Sub DataGridView1_DataMemberChanged(sender As Object, e As System.EventArgs) Handles DataGridView1.DataMemberChanged

    End Sub

    Private Sub DataGridView1_DoubleClick(sender As Object, e As System.EventArgs) Handles DataGridView1.DoubleClick
        Dim I As Integer = DataGridView1.CurrentCell.RowIndex
        ActualizarNro.txtNroPago.Text = txtCodigo.Text
        If ((chkAutorizado.Checked) And (cmbTipoOrden.Text.Equals("Fondos Fijos"))) Then
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