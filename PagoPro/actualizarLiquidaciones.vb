Imports System.Data.SqlClient
Imports System.Data.Odbc
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.Image
Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet
Public Class actualizarLiquidaciones
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
#Region "Codigo Standar del formulario"

    Private Sub btnSalir_Click(sender As System.Object, e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnBuscar_Click(sender As System.Object, e As System.EventArgs) Handles btnBuscar.Click
        If txtCodigo.Text = "" Then
            MessageBox.Show("Debe ingresar un codigo valido", "AVISO")
        Else
            Try
                If chkNuevo.Checked Or chkHistorial.Checked Then
                    If chkNuevo.Checked Then
                        If chkAutorizado.Checked Then

                        Else
                            adjuntarDatos()
                        End If
                    End If
                    If chkHistorial.Checked Then
                        If chkAutorizado.Checked Then
                            cargarDataRendicion() 'aca esta la ppapa
                            cargardataTransferencias()
                        Else
                            cargarHistorial()
                        End If

                    End If
                Else
                    MessageBox.Show("Debe seleccionar si es un nuevo pago o un historial", "AVISO")
                End If

            Catch ex As Exception
                MessageBox.Show("Por favor verifique el número de entrada", "ERROR")
            End Try
        End If

    End Sub

    Private Sub cargarHistorial()
        Sql = "SELECT Orden,Causa,Nombre,Detalle,Monto  FROM AALiquidaciones " &
               "where Causa='" & txtCodigo.Text & "'"
        DataGridView1.Columns.Clear()
        DataGridView1.Rows.Clear()
        DataGridView1.Columns.Add("orden", "ORDEN")
        DataGridView1.Columns.Add("causa", "CAUSA")
        DataGridView1.Columns.Add("nombre", "NOMBRE")
        DataGridView1.Columns.Add("detalle", "DETALLE")
        DataGridView1.Columns.Add("monto", "MONTO")
        DataGridView1.Columns.Item("MONTO").DefaultCellStyle.Format = "#####0.00"
        DataGridView3.Columns.Clear()
        DataGridView3.Rows.Clear()
        DataGridView3.Columns.Add("nombre", "NOMBRE")
        DataGridView3.Columns.Add("detalle", "DETALLE")
        DataGridView3.Columns.Add("monto", "MONTO")
        DataGridView3.Columns.Item("MONTO").DefaultCellStyle.Format = "#####0.00"


        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = Sql
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
                    DataGridView1.Rows.Add(fila)
                Loop
            End Using
        End Using
    End Sub

    Private Sub GenerarContratos_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        GroupBox3.Visible = False
        cargarComboAnio()

    End Sub
    Private Sub cargarComboAnio()
        Sql = "select Descripcion from AAanio"

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = Sql

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

    Private Sub txtCodigo_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtCodigo.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            If txtCodigo.Text <> "" Then
                chkAutorizado.Focus()
            End If
        End If
    End Sub

    Private Sub chkAutorizado_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAutorizado.CheckedChanged
        If chkAutorizado.Checked Then
            GroupBox3.Visible = True
        Else
            GroupBox3.Visible = False
        End If
    End Sub

    Private Sub chkNac_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkNac.CheckedChanged
        If chkNac.Checked Then
            chkProv.Enabled = False
            Sql = "Select tal_ActualNro from Talonar where tal_Cod='OPN'"

            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()

                    conn.Open()

                    cmd4.CommandText = Sql

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

    Private Sub chkProv_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkProv.CheckedChanged
        If chkProv.Checked Then
            chkNac.Enabled = False
            Sql = "Select tal_ActualNro from Talonar where tal_Cod='OPP'"

            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()

                    conn.Open()

                    cmd4.CommandText = Sql

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

#End Region

    Public Sub cargarDataRendicion()
        DataGridView1.Rows.Clear()
        DataGridView1.Columns.Clear()

        DataGridView3.Rows.Clear()
        DataGridView3.Columns.Clear()


        Sql = "SELECT [chp_NroCheq],[chpctb_Cod],[chp_Importe],[pro_RazSoc],[chp_FVto]  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
              "inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
              "inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]" &
              "where CausaEmi.cem_Desc='" & adjuntarAnio() & "'"
        DataGridView1.Columns.Add("fecha", "FECHA EMISION")
        DataGridView1.Columns.Add("destinatario", "BENEFICIARIO")
        DataGridView1.Columns.Add("ctaCte", "CUENTA CORRIENTE")
        DataGridView1.Columns.Add("dias", "NRO CHEQUE")
        DataGridView1.Columns.Add("importe", "IMPORTE")
        DataGridView1.Columns.Item("IMPORTE").DefaultCellStyle.Format = "#####0.00"

        DataGridView3.Columns.Add("fecha", "FECHA EMISION")
        DataGridView3.Columns.Add("destinatario", "BENEFICIARIO")
        DataGridView3.Columns.Add("ctaCte", "CUENTA CORRIENTE")
        DataGridView3.Columns.Add("dias", "NRO CHEQUE")
        DataGridView3.Columns.Add("importe", "IMPORTE")
        DataGridView3.Columns.Item("IMPORTE").DefaultCellStyle.Format = "#####0.00"


        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = Sql
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
                    DataGridView1.Rows.Add(fila)
                Loop
            End Using
        End Using
    End Sub

    Public Sub cargardataTransferencias()
        DataGridView2.Rows.Clear()
        DataGridView2.Columns.Clear()
        DataGridView4.Rows.Clear()
        DataGridView4.Columns.Clear()
        Sql = "SELECT mfocmf_FMov,mfoctb_Cod,pro_RazSoc,mfo_ImpMonElem   FROM [SBDASIPT].[dbo].[CabMovF] inner join SBDASIPT.dbo.CabCompra on CabMovF.cmf_ID=CabCompra.ccocmf_ID " &
              "inner join [SBDASIPT].dbo.CausaEmi on CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
              "inner join [SBDASIPT].[dbo].[MovF] on  [CabMovF].cmf_ID=[MovF].mfocmf_ID " &
              "inner join [SBDASIPT].[dbo].[Proveed] on MovF.mfopro_Cod=Proveed.pro_Cod " &
              "where CausaEmi.cem_Desc='" & adjuntarAnio() & "' and mfobco_cod not like 'NULL'"
        DataGridView2.Columns.Add("fecha", "FECHA EMISION")
        DataGridView2.Columns.Add("destinatario", "BENEFICIARIO")
        DataGridView2.Columns.Add("ctaCte", "CUENTA CORRIENTE")
        DataGridView2.Columns.Add("dias", "NRO CHEQUE")
        DataGridView2.Columns.Add("importe", "IMPORTE")
        DataGridView2.Columns.Item("IMPORTE").DefaultCellStyle.Format = "#####0.00"

        DataGridView4.Columns.Add("fecha", "FECHA EMISION")
        DataGridView4.Columns.Add("destinatario", "BENEFICIARIO")
        DataGridView4.Columns.Add("ctaCte", "CUENTA CORRIENTE")
        DataGridView4.Columns.Add("dias", "NRO CHEQUE")
        DataGridView4.Columns.Add("importe", "IMPORTE")
        DataGridView4.Columns.Item("IMPORTE").DefaultCellStyle.Format = "#####0.00"

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = Sql
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
                    DataGridView2.Rows.Add(fila)
                Loop
            End Using
        End Using
    End Sub

    'VERIFICAR SI PUEDO ENTRAR POR ESTE LADO

#Region "VERIFICAR"

    Public Sub adjuntarDatos()
        DataGridView1.Columns.Clear()
        DataGridView1.Rows.Clear()
        DataGridView1.Columns.Add("nombre", "NOMBRE")
        DataGridView1.Columns.Add("detalle", "DETALLE")
        DataGridView1.Columns.Add("monto", "MONTO")
        DataGridView1.Columns.Item("MONTO").DefaultCellStyle.Format = "#####0.00"
        DataGridView3.Columns.Clear()
        DataGridView3.Rows.Clear()
        DataGridView3.Columns.Add("nombre", "NOMBRE")
        DataGridView3.Columns.Add("detalle", "DETALLE")
        DataGridView3.Columns.Add("monto", "MONTO")
        DataGridView3.Columns.Item("MONTO").DefaultCellStyle.Format = "#####0.00"

        Sql = "SELECT [cco_ID],[ccopro_RazSoc],[cco_ImpMonLoc],[ico_Desc],ItemComp.ico_CantUM1,[ico_NetoLoc] FROM [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " GROUP BY [cco_ID],[ccopro_RazSoc],[cco_ImpMonLoc],[ico_Desc],ItemComp.ico_CantUM1,[ico_NetoLoc] ORDER BY [cco_ImpMonLoc]"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = Sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader

                Do While reader.Read()
                    Dim fila(3) As String
                    fila(0) = reader(1)
                    fila(1) = reader(3)
                    fila(2) = reader(5) * -1
                    DataGridView1.Rows.Add(fila)
                Loop
            End Using
        End Using
        calculoMonto()
    End Sub

    Public Sub calculoMonto()

        'sql = "SELECT sum([cco_ImpMonLoc]) FROM [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio()
        'Using conn As New SqlConnection(CONNSTR)
        '    Using cmd4 As SqlCommand = conn.CreateCommand()

        '        conn.Open()

        '        cmd4.CommandText = sql

        '        Dim reader As SqlDataReader = Nothing
        '        reader = cmd4.ExecuteReader

        '        Do While reader.Read()
        '            txtTotalFinal.Text = reader(0) * -1
        '        Loop
        '    End Using
        'End Using
        Dim suma As Double
        suma = 0
        For Each row As DataGridViewRow In DataGridView1.Rows
            suma = suma + row.Cells(2).Value
        Next
        txtTotalFinal.Text = suma
    End Sub

#End Region

#Region "Imprimir"

    Public Sub imprimirPago()
        Dim Origen As String
        If chkNac.Checked Then
            Origen = "OPN"
        End If
        If chkProv.Checked Then
            Origen = "OPP"
        End If
        'sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='Liquidaciones'"

        Dim BANDERA As Boolean = False
        Sql = "Select Concepto from AAOrdenNro where NroIngreso like '%" & cmbAnio.Text & "%' and NacPro='" & Origen & "'"
        'declara una variable booleana

        ''se conecta con la base de datos y ejecuta la consulta
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = Sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While (reader.Read() And BANDERA = False)
                    ''pregunta si el resultado de la consulta es "" o Nothing
                    If reader(0).ToString.Equals("") Or reader(0).ToString.Equals(Nothing) Then
                        'se agrega falso a la variable
                        BANDERA = False
                    End If
                    'Else
                    '    'si el resultado de la consulta es igual a NULL
                    If IsDBNull(reader(0)) Then
                        'se le agrega  false a la variable booleana
                        BANDERA = False
                    End If
                    '    Else 'si no es ninguna de las anteriores se le asigna TRUE 
                    ''''''''''''''''''''''''''''''''''''''''''
                    If (reader(0) = txtSaliente.Text) Then
                        BANDERA = True
                    Else
                        BANDERA = False
                    End If
                    ' ''''''''''''''''''''''''''''''''''''''''''
                    '    End If
                    'End If
                    ''Else ' si no se puede leer el resultado de la consulta se asigna FALSE a la variable booleana

                    'End If
                End While
            End Using
        End Using
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
            MessageBox.Show("YA EXISTE UNA ORDEN CON ESTE NUMERO")
        End If

        'If BANDERA = False Then
        'If chkNuevo.Checked Then
        'Dim nroCargo As String
        'Try
        '    sql = "Select Orden from AALiquidaciones where Causa='" & txtCodigo.Text & "'"


        '    Using conn As New SqlConnection(CONNSTR)
        '        Using cmd4 As SqlCommand = conn.CreateCommand()

        '            conn.Open()

        '            cmd4.CommandText = sql

        '            Dim reader As SqlDataReader = Nothing
        '            reader = cmd4.ExecuteReader
        '            reader.Read()
        '            nroCargo = reader.Item(0)
        '        End Using
        '    End Using
        'Catch ex As Exception

        'End Try
        'Else
        'nroCargo = txtSaliente.Text

        'If almacenarLiquidacion(nroCargo) Then
        '    Try

        '        Dim Documento As New Document(PageSize.LEGAL, 60, 5, 35, 5) 'Declaracion del documento
        '        Dim parrafo As New Paragraph ' Declaracion de un parrafo
        '        Dim nroPago As String
        '        nroPago = dameNroPago()

        '        pdf.PdfWriter.GetInstance(Documento, New FileStream("OrdenPago.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

        '        Documento.Open() 'Abre documento para su escritura

        '        parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
        '        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        '        parrafo.Add("PROVINCIAL DE MISIONES" & vbCr & "SISTEMA PROVINCIAL" & vbCr & "DE TELEDUCACION Y DESARROLLO") 'Texto que se insertara
        '        Documento.Add(parrafo) 'Agrega el parrafo al documento
        '        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

        '        Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

        '        imagendemo = iTextSharp.text.Image.GetInstance("descarga.jpg") 'Dirreccion a la imagen que se hace referencia
        '        imagendemo.SetAbsolutePosition(490, 920) 'Posicion en el eje cartesiano
        '        imagendemo.ScaleAbsoluteWidth(100) 'Ancho de la imagen
        '        imagendemo.ScaleAbsoluteHeight(100) 'Altura de la imagen
        '        Documento.Add(imagendemo) ' Agrega la imagen al documento

        '        parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
        '        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        '        parrafo.Add("ORDEN DE PAGO NRO:" & nroCargo & "                             ") 'Texto que se insertara
        '        parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
        '        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        '        parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
        '        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        '        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        '        parrafo.Add("FECHA:" & Date.Now) 'Texto que se insertara

        '        Documento.Add(parrafo) 'Agrega el parrafo al documento
        '        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

        '        Documento.Add(New Paragraph(" ")) 'Salto de linea
        '        Dim tablademo As New PdfPTable(3) 'declara la tabla con 4 Columnas
        '        tablademo.SetWidthPercentage({200, 250, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

        '        tablademo.AddCell(New Paragraph("NOMBRE   ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

        '        tablademo.AddCell(New Paragraph("DETALLE           ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
        '        tablademo.AddCell(New Paragraph("IMPORTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        '        Documento.Add(tablademo) 'Agrega la tabla al documento

        '        Dim tablademo2 As New PdfPTable(3) 'declara la tabla con 4 Columnas
        '        tablademo2.SetWidthPercentage({200, 250, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        '        Dim counter As Integer

        '        For Each row As DataGridViewRow In DataGridView3.Rows

        '            tablademo2.AddCell(New Paragraph(row.Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        '            tablademo2.AddCell(New Paragraph(row.Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        '            Dim montu As Decimal
        '            Dim paraParrrafo As String
        '            montu = Format(Convert.ToDecimal(row.Cells(2).Value), "standard")
        '            paraParrrafo = Format(montu, "###,##0.00")
        '            Dim miCelda2 As New PdfPCell
        '            parrafo.Add(paraParrrafo)
        '            miCelda2.AddElement(parrafo)
        '            miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
        '            tablademo2.AddCell(miCelda2) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        '            parrafo.Clear()

        '        Next
        '        Documento.Add(tablademo2) 'Agrega la tabla al documento

        '        Dim tablademo11 As New PdfPTable(3) 'declara la tabla con 4 Columnas
        '        tablademo11.SetWidthPercentage({200, 250, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        '        tablademo11.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        '        tablademo11.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        '        Dim paraparrafo As String
        '        Dim montito As Double
        '        montito = txtTotalImp.Text
        '        paraparrafo = Format(montito, "###,##0.00")
        '        Dim miCelda3 As New PdfPCell
        '        parrafo.Add(paraparrafo)
        '        miCelda3.AddElement(parrafo)
        '        miCelda3.HorizontalAlignment = Element.ALIGN_RIGHT
        '        tablademo11.AddCell(miCelda3) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        '        parrafo.Clear()
        '        Documento.Add(tablademo11) 'Agrega la tabla al documento
        '        Documento.Add(New Paragraph(" ")) 'Salto de linea
        '        'Documento.Add(New Paragraph("SON PESOS: " & Numalet.ToCardinal(txtTotalFinal.Text))) 'Salto de linea
        '        Documento.Add(New Paragraph("   ")) 'Salto de linea
        '        Documento.Add(New Paragraph("SON:$" & txtTotalImp.Text & " (PESOS:" & Numalet.ToCardinal(txtTotalImp.Text) & ")")) 'Salto de linea
        '        Documento.Add(New Paragraph(" ")) 'Salto de linea
        '        Documento.Add(New Paragraph(" ")) 'Salto de linea
        '        Documento.Add(New Paragraph(" ")) 'Salto de linea
        '        Documento.Add(New Paragraph(" ")) 'Salto de linea
        '        Documento.Add(New Paragraph("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL")) 'Salto de linea

        '        'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        '        'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        '        'parrafo.Add("Direccion Administracion " & vbCr & "Finanzas") 'Texto que se insertara

        '        Documento.Add(parrafo) 'Agrega el parrafo al documento
        '        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente



        '        Documento.Close() 'Cierra el documento
        '        System.Diagnostics.Process.Start("OrdenPago.pdf") 'Abre el archivo DEMO.PDF

        '    Catch ex As Exception
        '        MessageBox.Show("Compruebe que no tiene un documento abierto previamente", "AVISO")
        '    End Try
        'Else

        'End If

        'End If
        If chkHistorial.Checked Then
            Try
                Sql = "Select Orden from AALiquidaciones where Causa='" & txtCodigo.Text & "'"
                Dim nroCargo As String

                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()

                        conn.Open()

                        cmd4.CommandText = Sql

                        Dim reader As SqlDataReader = Nothing
                        reader = cmd4.ExecuteReader
                        reader.Read()
                        nroCargo = reader.Item(0)
                    End Using
                End Using

                Dim Documento As New Document(PageSize.A4, 60, 5, 35, 5) 'Declaracion del documento
                Dim parrafo As New Paragraph ' Declaracion de un parrafo
                Dim nroPago As String
                nroPago = dameNroPago()

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
                Dim tablademo As New PdfPTable(3) 'declara la tabla con 4 Columnas
                tablademo.SetWidthPercentage({200, 250, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

                tablademo.AddCell(New Paragraph("NOMBRE   ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

                tablademo.AddCell(New Paragraph("DETALLE           ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                tablademo.AddCell(New Paragraph("IMPORTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

                Documento.Add(tablademo) 'Agrega la tabla al documento

                Dim tablademo2 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                tablademo2.SetWidthPercentage({200, 250, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                Dim counter As Integer

                For Each row As DataGridViewRow In DataGridView3.Rows

                    tablademo2.AddCell(New Paragraph(row.Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    tablademo2.AddCell(New Paragraph(row.Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

                    Dim montu As Decimal
                    Dim paraParrrafo As String
                    montu = Format(Convert.ToDecimal(row.Cells(2).Value), "standard")
                    paraParrrafo = Format(montu, "###,##0.00")
                    Dim miCelda2 As New PdfPCell
                    parrafo.Add(paraParrrafo)
                    miCelda2.AddElement(parrafo)
                    miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                    tablademo2.AddCell(miCelda2) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    parrafo.Clear()

                Next
                Documento.Add(tablademo2) 'Agrega la tabla al documento

                Dim tablademo11 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                tablademo11.SetWidthPercentage({200, 250, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                tablademo11.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                tablademo11.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

                Dim paraparrafo As String
                Dim montito As Double
                montito = txtTotalImp.Text
                paraparrafo = Format(montito, "###,##0.00")
                Dim miCelda3 As New PdfPCell
                parrafo.Add(paraparrafo)
                miCelda3.AddElement(parrafo)
                miCelda3.HorizontalAlignment = Element.ALIGN_RIGHT
                tablademo11.AddCell(miCelda3) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                parrafo.Clear()
                Documento.Add(tablademo11) 'Agrega la tabla al documento
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                'Documento.Add(New Paragraph("SON PESOS: " & Numalet.ToCardinal(txtTotalFinal.Text))) 'Salto de linea
                Documento.Add(New Paragraph("   ")) 'Salto de linea
                Documento.Add(New Paragraph("SON:$" & txtTotalImp.Text & " (PESOS:" & Numalet.ToCardinal(txtTotalImp.Text) & ")")) 'Salto de linea
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
        End If
        'Else
        If BANDERA = False Then
            If chkNuevo.Checked Then
                If chkNac.Checked Then
                    Origen = "OPN"
                End If
                If chkProv.Checked Then
                    Origen = "OPP"
                End If

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
                    Sql = "update Talonar set tal_ActualNro='" & nro & "' where tal_Cod='" & Origen & "'"
                    Using conn As New SqlConnection(CONNSTR)
                        Using cmd4 As SqlCommand = conn.CreateCommand()

                            conn.Open()

                            cmd4.CommandText = Sql
                            cmd4.ExecuteScalar()

                        End Using
                    End Using
                    MessageBox.Show("El talonario se actualizo de manera correcta", "EXITO")
                Catch ex As Exception
                    MessageBox.Show("Un error provoco que no se actualice el Talonario de ordenes de pago, se recomienda que lo haga de manera manual", "AVISO")
                End Try

                If almacenarOrden(nroCargo, Origen) Then
                    Try
                        Dim Documento As New Document(PageSize.LEGAL, 60, 5, 35, 5) 'Declaracion del documento
                        Dim parrafo As New Paragraph ' Declaracion de un parrafo
                        Dim nroPago As String
                        nroPago = dameNroPago()

                        pdf.PdfWriter.GetInstance(Documento, New FileStream("OrdenPago.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

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
                        Dim tablademo As New PdfPTable(3) 'declara la tabla con 4 Columnas
                        tablademo.SetWidthPercentage({200, 250, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

                        tablademo.AddCell(New Paragraph("NOMBRE   ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

                        tablademo.AddCell(New Paragraph("DETALLE           ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                        tablademo.AddCell(New Paragraph("IMPORTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

                        Documento.Add(tablademo) 'Agrega la tabla al documento

                        Dim tablademo2 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                        tablademo2.SetWidthPercentage({200, 250, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                        Dim counter As Integer

                        For Each row As DataGridViewRow In DataGridView3.Rows

                            tablademo2.AddCell(New Paragraph(row.Cells(0).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                            tablademo2.AddCell(New Paragraph(row.Cells(1).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

                            Dim montu As Decimal
                            Dim paraParrrafo As String
                            montu = Format(Convert.ToDecimal(row.Cells(2).Value), "standard")
                            paraParrrafo = Format(montu, "###,##0.00")
                            Dim miCelda2 As New PdfPCell
                            parrafo.Add(paraParrrafo)
                            miCelda2.AddElement(parrafo)
                            miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                            tablademo2.AddCell(miCelda2) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                            parrafo.Clear()

                        Next
                        Documento.Add(tablademo2) 'Agrega la tabla al documento

                        Dim tablademo11 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                        tablademo11.SetWidthPercentage({200, 250, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                        tablademo11.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                        tablademo11.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

                        Dim paraparrafo As String
                        Dim montito As Double
                        montito = txtTotalImp.Text
                        paraparrafo = Format(montito, "###,##0.00")
                        Dim miCelda3 As New PdfPCell
                        parrafo.Add(paraparrafo)
                        miCelda3.AddElement(parrafo)
                        miCelda3.HorizontalAlignment = Element.ALIGN_RIGHT
                        tablademo11.AddCell(miCelda3) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                        parrafo.Clear()
                        Documento.Add(tablademo11) 'Agrega la tabla al documento
                        Documento.Add(New Paragraph(" ")) 'Salto de linea
                        'Documento.Add(New Paragraph("SON PESOS: " & Numalet.ToCardinal(txtTotalFinal.Text))) 'Salto de linea
                        Documento.Add(New Paragraph("   ")) 'Salto de linea
                        Documento.Add(New Paragraph("SON:$" & txtTotalImp.Text & " (PESOS:" & Numalet.ToCardinal(txtTotalImp.Text) & ")")) 'Salto de linea
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
                        MessageBox.Show("Verifique que no tiene un documento abierto previamente", "AVISO")
                    End Try
                End If
            End If

        End If
    End Sub

    Public Sub imprimirPagoHistorial()


        Dim Documento As New Document 'Declaracion del documento
        Dim parrafo As New Paragraph ' Declaracion de un parrafo
        Dim nroPago As String
        nroPago = dameNroPago()

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
        parrafo.Add("ORDEN DE PAGO NRO:" & dameNroHistorial("Liquidaciones") & "                             ") 'Texto que se insertara
        parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("FECHA:" & Date.Now) 'Texto que se insertara

        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Dim tablademo As New PdfPTable(3) 'declara la tabla con 4 Columnas
        tablademo.SetWidthPercentage({200, 300, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

        tablademo.AddCell(New Paragraph("NOMBRE   ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

        tablademo.AddCell(New Paragraph("DETALLE           ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
        tablademo.AddCell(New Paragraph("IMPORTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        Documento.Add(tablademo) 'Agrega la tabla al documento

        Dim tablademo2 As New PdfPTable(3) 'declara la tabla con 4 Columnas
        tablademo2.SetWidthPercentage({200, 300, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        Dim counter As Integer

        For Each row As DataGridViewRow In DataGridView1.Rows
            For Each column As DataGridViewColumn In DataGridView1.Columns
                counter = 0
                If column.Index <= 3 Then
                    tablademo2.AddCell(New Paragraph(row.Cells(column.Index).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                End If


            Next
        Next
        Documento.Add(tablademo2) 'Agrega la tabla al documento

        Documento.Add(New Paragraph(" ")) 'Salto de linea

        Dim tablademo3 As New PdfPTable(3) 'declara la tabla con 4 Columnas
        tablademo3.SetWidthPercentage({200, 300, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo3.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

        tablademo3.AddCell(New Paragraph()) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        tablademo3.AddCell(New Paragraph(txtTotalFinal.Text)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        Documento.Add(tablademo3) 'Agrega la tabla al documento

        Documento.Add(New Paragraph(" ")) 'Salto de linea
        'Documento.Add(New Paragraph("SON PESOS: " & Numalet.ToCardinal(txtTotalFinal.Text))) 'Salto de linea
        Documento.Add(New Paragraph("   ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
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

    Public Sub imprimirOrdenCargoCheques()

        Sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='LiqPago'"

        Dim BANDERA As Boolean

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = Sql

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
            imprimirOrdenCargoHistorial()
        Else
            Dim origen As String

            If chkNac.Checked Then
                origen = "OPN"
            End If
            If chkProv.Checked Then
                origen = "OPP"
            End If
            Dim nroCargo As String
            Try
                Sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='Liquidaciones'"


                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()

                        conn.Open()

                        cmd4.CommandText = Sql

                        Dim reader As SqlDataReader = Nothing
                        reader = cmd4.ExecuteReader
                        reader.Read()
                        nroCargo = reader.Item(0)
                    End Using
                End Using
            Catch ex As Exception

            End Try


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

            Documento.Add(New Paragraph(" ")) 'Salto de linea

            Dim tablademo5 As New PdfPTable(5) 'declara la tabla con 4 Columnas
            tablademo5.SetWidthPercentage({100, 150, 100, 110, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tablademo5.AddCell(New Paragraph("F. EMISION        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

            tablademo5.AddCell(New Paragraph("BENEFICIARIO        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            tablademo5.AddCell(New Paragraph("NRO CTA        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            tablademo5.AddCell(New Paragraph("CH. Y/O AUT.        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

            tablademo5.AddCell(New Paragraph("IMPORTE        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            Documento.Add(tablademo5) 'Agrega la tabla al documento

            Dim tablademo9 As New PdfPTable(5) 'declara la tabla con 4 Columnas
            tablademo9.SetWidthPercentage({100, 150, 100, 110, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
            For Each row1 As DataGridViewRow In DataGridView4.Rows
                For Each column As DataGridViewColumn In DataGridView4.Columns
                    If column.Index <= 5 Then
                        Dim paraParrrafo As String
                        If IsNumeric(row1.Cells(column.Index).Value) And Not column.Name.Equals("dias") Then

                            paraParrrafo = Format(row1.Cells(column.Index).Value, "###,##0.00")
                            Dim miCelda2 As New PdfPCell
                            parrafo.Add(paraParrrafo)
                            miCelda2.AddElement(parrafo)
                            miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                            tablademo9.AddCell(miCelda2)
                            parrafo.Clear()

                        Else

                            paraParrrafo = row1.Cells(column.Index).Value
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

            Dim tablademo4 As New PdfPTable(5) 'declara la tabla con 4 Columnas
            tablademo4.SetWidthPercentage({100, 150, 100, 110, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
            For Each row1 As DataGridViewRow In DataGridView3.Rows
                For Each column As DataGridViewColumn In DataGridView3.Columns
                    If column.Index <= 5 Then
                        Dim paraParrrafo As String
                        If IsNumeric(row1.Cells(column.Index).Value) And Not column.Name.Equals("dias") Then

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
            Documento.Add(New Paragraph("Total Final:$ " & txtTotalImp.Text & "  ( Pesos: " & Numalet.ToCardinal(txtTotalImp.Text) & ")")) 'Salto de linea
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
            'almacenarPago(nroCargo, origen)
        End If

    End Sub

    Public Sub imprimirOrdenCargoHistorial()

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
        parrafo.Add("ORDEN DE PAGO NRO:" & dameNroHistorial("Liquidaciones") & "                             ") 'Texto que se insertara
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

        Documento.Add(New Paragraph(" ")) 'Salto de linea

        Dim tablademo5 As New PdfPTable(5) 'declara la tabla con 4 Columnas
        tablademo5.SetWidthPercentage({100, 150, 100, 110, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo5.AddCell(New Paragraph("F. EMISION        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

        tablademo5.AddCell(New Paragraph("BENEFICIARIO        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo5.AddCell(New Paragraph("NRO CTA        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo5.AddCell(New Paragraph("CH. Y/O AUT.        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        tablademo5.AddCell(New Paragraph("IMPORTE        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        Documento.Add(tablademo5) 'Agrega la tabla al documento

        Dim tablademo9 As New PdfPTable(5) 'declara la tabla con 4 Columnas
        tablademo9.SetWidthPercentage({100, 150, 100, 110, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        For Each row1 As DataGridViewRow In DataGridView4.Rows
            For Each column As DataGridViewColumn In DataGridView4.Columns
                If column.Index <= 5 Then
                    tablademo9.AddCell(New Paragraph(row1.Cells(column.Index).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                End If

            Next

        Next
        Documento.Add(tablademo9) 'Agrega la tabla al documento

        Dim tablademo4 As New PdfPTable(5) 'declara la tabla con 4 Columnas
        tablademo4.SetWidthPercentage({100, 150, 100, 110, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        For Each row1 As DataGridViewRow In DataGridView1.Rows
            For Each column As DataGridViewColumn In DataGridView1.Columns
                If column.Index <= 5 Then
                    tablademo4.AddCell(New Paragraph(row1.Cells(column.Index).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                End If

            Next

        Next
        Documento.Add(tablademo4) 'Agrega la tabla al documento

        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph("Total Final:$ " & txtTotalImp.Text & "  ( Pesos: " & Numalet.ToCardinal(txtTotalImp.Text) & ")")) 'Salto de linea
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

    Private Sub btnImprimir_Click(sender As System.Object, e As System.EventArgs) Handles btnImprimir.Click
        If chkAutorizado.Checked Then
            imprimirOrdenCargoCheques()
            'actualizarTalonario()
        Else
            imprimirPago()
        End If

    End Sub

    Private Function dameNroHistorial(ByRef Tipo As String) As String
        Dim Nro As String
        Sql = "Select Concepto from AAOrdenNro where Tipo='" & Tipo & "' and NroIngreso=" & adjuntarAnio()
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = Sql

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

#Region "CODIGOS AUXILIARES"

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

        Sql = "SELECT [Talonar].[tal_ActualNro] FROM  [SBDASIPT].[dbo].[Talonar] where tal_Cod='DDL'"
        Dim final As String
        Dim nroOrden As String
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = Sql

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

    Private Sub almacenarPago(ByRef nro As String, ByRef origen As String)

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
                cmd.Parameters.Add("@Tipo", SqlDbType.VarChar, 50).Value = "LiqPago"
                cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = Date.Now
                cmd.Parameters.Add("@NacPro", SqlDbType.VarChar, 3).Value = origen

                cmd.ExecuteScalar()


            End Using
        End Using
    End Sub

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
                    cmd.Parameters.Add("@Tipo", SqlDbType.VarChar, 50).Value = "Liquidaciones"
                    cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = Date.Now
                    cmd.Parameters.Add("@NacPro", SqlDbType.VarChar, 3).Value = origen

                    cmd.ExecuteScalar()


                End Using
            End Using
            bandera = True
            MessageBox.Show("Las liquidaciones se almacenaron de manera correcta", "EXITO")
        Catch ex As Exception
            bandera = False
            MessageBox.Show("Las liquidaciones no se almacenaron de manera correcta", "EXITO")
        End Try
        Return bandera
        almacenarLiquidacion(nro)

    End Function

    Private Function almacenarLiquidacion(ByRef nro As String) As Boolean
        Dim bandera As Boolean
        Try

            For Each row As DataGridViewRow In DataGridView3.Rows
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd As SqlCommand = conn.CreateCommand()

                        conn.Open()

                        cmd.CommandText = "insert into AALiquidaciones(" +
                                      "[Orden]" +
                                      ",[Causa]" +
                                      ",[Nombre]" +
                                      ",[Detalle]" +
                                      ",[Monto]" +
                                        ") VALUES (" +
                                       "@Orden" +
                                        ",@Causa" +
                                        ",@Nombre" +
                                        ",@Detalle" +
                                        ",@Monto" +
                                        ")"

                        cmd.Parameters.Add("@Orden", SqlDbType.Int).Value = nro
                        cmd.Parameters.Add("@Causa", SqlDbType.VarChar, 4).Value = txtCodigo.Text
                        cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = row.Cells(0).Value
                        cmd.Parameters.Add("@Detalle", SqlDbType.VarChar, 100).Value = row.Cells(1).Value
                        cmd.Parameters.Add("@Monto", SqlDbType.Float).Value = row.Cells(2).Value
                        cmd.ExecuteScalar()


                    End Using
                End Using
            Next
            bandera = True
            MessageBox.Show("Las liquidaciones se almacenaron de manera correcta", "EXITO")
        Catch ex As Exception
            bandera = False
            MessageBox.Show("Las liquidaciones no se almacenaron de manera correcta", "EXITO")
        End Try
        Return bandera
    End Function
#End Region

    Private Sub AgregarMonto()
        txtTotalImp.Text = 0
        Dim Total As Double
        If chkAutorizado.Checked Then
            Dim subTotal As Double
            Dim subTotal2 As Double
            For Each row As DataGridViewRow In DataGridView4.Rows
                subTotal = subTotal + row.Cells("Importe").Value
            Next
            For Each row As DataGridViewRow In DataGridView3.Rows
                subTotal2 = subTotal2 + row.Cells("Importe").Value
            Next
            txtTotalImp.Text = subTotal + subTotal2
        Else
            For Each row As DataGridViewRow In DataGridView3.Rows
                Total = Total + row.Cells("Monto").Value
            Next
            txtTotalImp.Text = Total
        End If


    End Sub

    Private Sub DataGridView1_DoubleClick(sender As Object, e As System.EventArgs) Handles DataGridView1.DoubleClick
        Dim I As Integer = DataGridView1.CurrentCell.RowIndex
        Dim Emitido As String
        If chkNuevo.Checked Then
            If chkAutorizado.Checked Then
                Dim fila(5) As Object
                fila(0) = DataGridView1.Rows(I).Cells(0).Value
                fila(1) = DataGridView1.Rows(I).Cells(1).Value
                fila(2) = DataGridView1.Rows(I).Cells(2).Value
                fila(3) = DataGridView1.Rows(I).Cells(3).Value
                fila(4) = DataGridView1.Rows(I).Cells(4).Value
                DataGridView3.Rows.Add(fila)
                AgregarMonto()
            Else
                Dim fila(3) As Object
                fila(0) = DataGridView1.Rows(I).Cells(0).Value
                fila(1) = DataGridView1.Rows(I).Cells(1).Value
                fila(2) = DataGridView1.Rows(I).Cells(2).Value

                DataGridView3.Rows.Add(fila)
                AgregarMonto()
            End If
        End If
        If chkHistorial.Checked Then
            If chkAutorizado.Checked Then
                Dim fila(5) As Object
                fila(0) = DataGridView1.Rows(I).Cells(0).Value
                fila(1) = DataGridView1.Rows(I).Cells(1).Value
                fila(2) = DataGridView1.Rows(I).Cells(2).Value
                fila(3) = DataGridView1.Rows(I).Cells(3).Value
                fila(4) = DataGridView1.Rows(I).Cells(4).Value
                DataGridView3.Rows.Add(fila)
                AgregarMonto()
            Else
                Dim fila(3) As Object
                fila(0) = DataGridView1.Rows(I).Cells(2).Value
                fila(1) = DataGridView1.Rows(I).Cells(3).Value
                fila(2) = DataGridView1.Rows(I).Cells(4).Value

                DataGridView3.Rows.Add(fila)
                AgregarMonto()
            End If
        End If
    End Sub

    Private Sub DataGridView2_DoubleClick(sender As Object, e As System.EventArgs) Handles DataGridView2.DoubleClick
        Dim I As Integer = DataGridView2.CurrentCell.RowIndex
        Dim Emitido As String
        If chkNuevo.Checked Then
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
                    AgregarMonto()
                Else
                    Dim fila(4) As Object
                    fila(0) = DataGridView2.Rows(I).Cells(0).Value
                    fila(1) = DataGridView2.Rows(I).Cells(1).Value
                    fila(2) = DataGridView2.Rows(I).Cells(2).Value
                    'fila(3) = DataGridView2.Rows(I).Cells(3).Value
                    DataGridView4.Rows.Add(fila)
                End If
            End If
        End If
        If chkHistorial.Checked Then
            If chkAutorizado.Checked Then
                Dim fila(5) As Object
                fila(0) = DataGridView2.Rows(I).Cells(0).Value
                fila(1) = DataGridView2.Rows(I).Cells(1).Value
                fila(2) = DataGridView2.Rows(I).Cells(2).Value
                fila(3) = DataGridView2.Rows(I).Cells(3).Value
                fila(4) = DataGridView2.Rows(I).Cells(4).Value
                DataGridView4.Rows.Add(fila)
                AgregarMonto()
            Else
                Dim fila(4) As Object
                fila(0) = DataGridView2.Rows(I).Cells(0).Value
                fila(1) = DataGridView2.Rows(I).Cells(1).Value
                fila(2) = DataGridView2.Rows(I).Cells(2).Value
                'fila(3) = DataGridView2.Rows(I).Cells(3).Value
                DataGridView4.Rows.Add(fila)
            End If
        End If

    End Sub

    Private Sub DataGridView3_RowsRemoved(sender As Object, e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles DataGridView3.RowsRemoved
        AgregarMonto()
    End Sub

    Private Sub chkHistorial_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkHistorial.CheckedChanged
        If chkHistorial.Checked Then
            chkNuevo.Enabled = False
        Else
            chkNuevo.Enabled = True
        End If
    End Sub

    Private Sub chkNuevo_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkNuevo.CheckedChanged
        If chkNuevo.Checked Then
            chkHistorial.Enabled = False
        Else
            chkHistorial.Enabled = True
        End If
    End Sub

    Private Sub GroupBox6_Enter(sender As System.Object, e As System.EventArgs) Handles GroupBox6.Enter

    End Sub
    Private Sub GroupBox4_Enter(sender As System.Object, e As System.EventArgs) Handles GroupBox4.Enter

    End Sub
    Private Sub GroupBox1_Enter(sender As System.Object, e As System.EventArgs) Handles GroupBox1.Enter

    End Sub
    Private Sub GroupBox2_Enter(sender As System.Object, e As System.EventArgs) Handles GroupBox2.Enter

    End Sub
    Private Sub GroupBox3_Enter(sender As System.Object, e As System.EventArgs) Handles GroupBox3.Enter

    End Sub
End Class