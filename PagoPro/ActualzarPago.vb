Imports System.Data.SqlClient
Imports System.Data.Odbc
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.Image
Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet
Public Class ActualzarPago
    Dim sql As String
    Dim sql2 As String
    Dim sql3 As String
    Dim sql4 As String

    Dim montoCheque As Double
    Dim da As New OdbcDataAdapter
    Dim da2 As New OdbcDataAdapter
    Dim da3 As New OdbcDataAdapter
    Dim anio As String
    Dim indiceRow As String
    Dim indiceColumn As String
    Private Sub btnBuscar_Click(sender As System.Object, e As System.EventArgs) Handles btnBuscar.Click
        DataGridView1.Rows.Clear()
        DataGridView1.Columns.Clear()
        DataGridView2.Rows.Clear()
        DataGridView2.Columns.Clear()
        If txtCodigo.Text = Nothing Then
            MessageBox.Show("Debe ingresar un codigo valido", "AVISO")
        Else
            'Try
            If cmbTipoOrden.SelectedItem = "Compras" Then
                If chkAutorizado.Checked Then
                    'ordenCargo()
                    GroupBox5.Visible = True
                    cargarDataRendicion()
                    CargarRetencion()
                    cargardataTransferencias()
                Else
                    adjuntarDatos()
                    GroupBox5.Visible = True
                    CargarRetencion()
                    adjuntarDatosRendicionAlquiler()
                End If
            Else
                If cmbTipoOrden.SelectedItem = "Historial Compras" Then
                    If chkAutorizado.Checked Then
                        ordenCargo()
                        GroupBox5.Visible = True
                        cargarDataRendicion()
                        CargarRetencion()
                    Else
                        adjuntarDatos()
                        GroupBox5.Visible = True
                        CargarRetencion()
                        adjuntarDatosRendicionAlquiler()
                    End If
                Else
                    If cmbTipoOrden.SelectedItem = "Fondos Fijos" Then
                        adjuntarDatosFondos()
                        GroupBox5.Visible = True
                    Else
                        MessageBox.Show("Debe seleccionar un tipo de orden valido", "AVISO")
                    End If


                End If

            End If
            'Catch ex As Exception
            '    MessageBox.Show("Por favor verifique el número de entrada", "ERROR")
            'End Try
        End If
    End Sub
    Public Sub adjuntarDatosFondos()

        DataGridView2.Columns.Add("fecha", "FECHA")
        DataGridView2.Columns.Add("comprobante", "COMPROBANTE")
        DataGridView2.Columns.Add("detalle", "DETALLE")
        DataGridView2.Columns.Add("monto", "MONTO")
        DataGridView2.Rows.Clear()
        sql = "SELECT [cco_FEmision],[cco_Nro],[ccopro_RazSoc],[cco_ImpMonLoc],[ccopro_CUIT],[cco_CodPvt],[ccotco_Cod],[cco_Letra] FROM [SBDASIPT].[dbo].[CabCompra] inner join [SBDASIPT].[dbo].[ItemComp]on [CabCompra].cco_ID=[ItemComp].icocco_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=[CabCompra].ccocem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " and [CabCompra].cco_Nro='" & txtFact.Text & "' GROUP BY [cco_FEmision],[cco_Nro],[ccopro_RazSoc],[cco_ImpMonLoc],[ccopro_CUIT],[cco_CodPvt],[ccotco_Cod],[cco_Letra]"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                'If reader.Read() Then
                Do While reader.Read()
                    Dim fila(4) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(6) & "." & reader(7) & "." & reader(5) & "." & reader(1)
                    fila(2) = reader(2) & "  CUIT:" & reader(4)
                    fila(3) = reader(3) * (-1)
                    DataGridView2.Rows.Add(fila)
                Loop
                'Else
                'MessageBox.Show("Verifique que los datos cargados sean los correctos", "AVISO")
                'End If
            End Using
        End Using
        calculoTotalAlquilerRendicion()
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
        DataGridView1.Columns.Add("recibo", "RECIBI CONFORME")

        montoCheque = 0

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
                    fila(0) = reader(2) 'Numalet.ToCardinal(reader(2))
                    fila(1) = reader(1)
                    fila(2) = reader(0)
                    fila(3) = reader(3)
                    fila(4) = ""
                    DataGridView1.Rows.Add(fila)
                    montoCheque = montoCheque + reader(2)
                Loop
            End Using
        End Using
        txtTotalFinal.Text = montoCheque
    End Sub
    Public Sub adjuntarDatosRendicionAlquiler()
        DataGridView2.Columns.Add("fecha", "FECHA")
        DataGridView2.Columns.Add("comprobante", "COMPROBANTE")
        DataGridView2.Columns.Add("detalle", "DETALLE")
        DataGridView2.Columns.Add("monto", "MONTO")
        DataGridView2.Columns.Item("monto").DefaultCellStyle.Format = "###,##0.00"
        Dim tipo As String
        tipo = ""
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

                        DataGridView2.Rows.Add(fila)

                    Loop
                End Using
            End Using
        End If

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
                    fila(3) = reader(3)
                    DataGridView1.Rows.Add(fila)
                Loop
            End Using
        End Using
        'calculoMonto()
        'calculoTotalAlquiler()
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
    Public Sub cargardataTransferencias()
        DataGridView2.Rows.Clear()
        DataGridView2.Columns.Clear()
        sql = "SELECT mfocmf_FMov,mfoctb_Cod,pro_RazSoc,mfo_ImpMonElem   FROM [SBDASIPT].[dbo].[CabMovF] inner join SBDASIPT.dbo.CabCompra on CabMovF.cmf_ID=CabCompra.ccocmf_ID " &
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
                    DataGridView2.Rows.Add(fila)
                    montoFinal = montoFinal + fila(4)
                Loop
            End Using
        End Using
        txtTotalGastos.Text = montoFinal
    End Sub
    Public Sub CargarRetencion()
        sql = "SELECT cco_Nro,dicres_Art,dicres_Cod,dic_Imp1 " &
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



                DataGridView3.Rows.Clear()
                DataGridView3.Columns.Clear()

                DataGridView3.Columns.Add("nro", "NUMERO")
                DataGridView3.Columns.Add("art", "ARTICULO")
                DataGridView3.Columns.Add("cod", "CODIGO")
                DataGridView3.Columns.Add("imp", "IMPORTE")

                While reader.Read()
                    GroupBox7.Visible = True
                    Dim fila(4) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    fila(2) = reader(2)
                    fila(3) = reader(3)
                    DataGridView3.Rows.Add(fila)
                End While

            End Using
        End Using

    End Sub
    Public Sub cargarDataRendicion()
        sql = "SELECT [chp_NroCheq],[chpctb_Cod],[chp_Importe],[pro_RazSoc],[chp_FVto]  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
              "inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
              "inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]" &
              "where CausaEmi.cem_Desc='" & adjuntarAnio() & "'"

        DataGridView1.Columns.Add("fecha", "FECHA EMISION")
        DataGridView1.Columns.Add("destinatario", "BENEFICIARIO")
        DataGridView1.Columns.Add("ctaCte", "CUENTA CORRIENTE")
        DataGridView1.Columns.Add("cheques", "NRO CHEQUE")

        DataGridView1.Columns.Add("importe", "IMPORTE")
        DataGridView1.Columns.Item("importe").DefaultCellStyle.Format = "###,##0.00"

        montoCheque = 0
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

                    DataGridView1.Rows.Add(fila)
                    montoCheque = montoCheque + reader(2)
                Loop
            End Using
        End Using
        txtTotalFinal.Text = montoCheque
    End Sub

    Private Sub ActualzarPago_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        cargarCombo()
        GroupBox5.Visible = False
        GroupBox7.Visible = False
        GroupBox8.Visible = False
        GroupBox9.Visible = False
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
                    cmbA.Items.Add(fila(0))
                Loop
            End Using
        End Using

    End Sub
    Public Sub cargarCombo()
        sql = "select Descripcion from AASituaciones where  Descripcion like '%Compras%' or Descripcion like '%Fondos Fijos%'"

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
    Private Sub btnSalir_Click(sender As System.Object, e As System.EventArgs) Handles btnSalir.Click
        Me.Dispose()

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs)
        'Dim unActualizarNro As Form = New ActualizarNro
        'ActualizarNro.T 
        'unActualizarNro.Show()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim I As Integer = DataGridView1.CurrentCell.RowIndex
        ActualizarNro.txtNroPago.Text = txtCodigo.Text
        If (chkAutorizado.Checked) Then
            ActualizarNro.txtDestinatario.Text = DataGridView1.Rows(I).Cells(1).Value.ToString
            ActualizarNro.txtNroCuenta.Text = DataGridView1.Rows(I).Cells(2).Value.ToString
            ActualizarNro.txtAnteriorNroCheque.Text = DataGridView1.Rows(I).Cells(3).Value.ToString

        Else
            ActualizarNro.txtNroCuenta.Text = DataGridView1.Rows(I).Cells(1).Value.ToString
            ActualizarNro.txtAnteriorNroCheque.Text = DataGridView1.Rows(I).Cells(2).Value.ToString
            ActualizarNro.txtDestinatario.Text = DataGridView1.Rows(I).Cells(3).Value.ToString

        End If

        ActualizarNro.Show()



        'descripcion = 
        'precio = DataGridView4.Rows(I).Cells(6).Value
        'TextBox6.Text = descripcion
        'TextBox10.Text = precio.ToString
        'TextBox9.Text = codigo
        'TextBox11.Text = DataGridView4.Rows(I).Cells(1).Value.ToString
        ''Dim unActualizarNro As Form = New ActualizarNro

        'unActualizarNro.Show()
    End Sub

    Private Sub btnImprimir_Click(sender As System.Object, e As System.EventArgs) Handles btnImprimir.Click
        Dim total As Double
        'total = txtTotalGastos.Text
        If chkNac.Checked Or chkProv.Checked Then
            If chkDisp.Checked Or chkRes.Checked Then
                If txtCodigo.Text = Nothing Then
                    MessageBox.Show("Debe ingresar un codigo valido", "AVISO")
                Else
                    If cmbTipoOrden.Text.Equals("Compras") Then
                        If chkActa.Checked Then
                            Select Case MsgBox("¿Desea Generar un nuevo Acta?", MsgBoxStyle.YesNo, "AVISO")
                                Case MsgBoxResult.Yes
                                    'cargarApertura()
                                    imprimirActa()
                                    'imprimirPago()
                                Case MsgBoxResult.No
                                    Dim myValue As String = InputBox("Ingrese el Nro de Acta requerido", "HISTORIAL DE ACTAS")
                                    If String.IsNullOrEmpty(myValue) Then
                                        MessageBox.Show("Se cancelo la Accion")
                                        Return
                                    Else
                                        imprimirActaHistorial(myValue)
                                    End If
                                    'InputBox("Enter Student's Date of Birth", _
                                    '"Red Oak High School - Student Registration")
                                    'imprimirActaFondosHistorial()
                            End Select

                        Else
                            If chkAutorizado.Checked Then
                                imprimirOrdenCargoCheques()
                            Else
                                'Select Case MsgBox("¿Esta seguro que desea generar un pago?", MsgBoxStyle.YesNo, "AVISO")
                                '   Case MsgBoxResult.Yes
                                'cargarApertura()
                                impirmirOrdenCargoPrimeroHistorial()
                                'impirmirOrdenCargoPrimero()
                                'imprimirPago()
                                '  Case MsgBoxResult.No
                                'MessageBox.Show("Accion cancelada por el usuario", "INFORMACION")
                                'End Select

                            End If
                        End If
                    Else
                        If cmbTipoOrden.Text.Equals("Historial Compras") Then
                            If chkActa.Checked Then
                                Select Case MsgBox("¿Desea Generar un nuevo Acta?", MsgBoxStyle.YesNo, "AVISO")
                                    Case MsgBoxResult.Yes
                                        'cargarApertura()
                                        imprimirActa()
                                        'imprimirPago()
                                    Case MsgBoxResult.No
                                        Dim myValue As String = InputBox("Ingrese el Nro de Acta requerido", "HISTORIAL DE ACTAS")
                                        If String.IsNullOrEmpty(myValue) Then
                                            MessageBox.Show("Se cancelo la Accion")
                                            Return
                                        Else
                                            imprimirActaHistorial(myValue)
                                        End If
                                        '             InputBox("Enter Student's Date of Birth", _
                                        '"Red Oak High School - Student Registration")
                                        'imprimirActaFondosHistorial()
                                End Select
                            Else
                                If chkAutorizado.Checked Then
                                    imprimirOrdenCargoChequesHistorial()
                                Else
                                    impirmirOrdenCargoPrimeroHistorial()
                                End If
                            End If
                        Else
                            If cmbTipoOrden.Text.Equals("Fondos Fijos") Then
                                Select Case MsgBox("¿Desea Generar un nuevo Acta?", MsgBoxStyle.YesNo, "AVISO")
                                    Case MsgBoxResult.Yes
                                        'cargarApertura()
                                        imprimirActaFondos()
                                        'imprimirPago()
                                    Case MsgBoxResult.No
                                        Dim myValue As String = InputBox("Ingrese el Nro de Acta requerido", "HISTORIAL DE ACTAS")
                                        If String.IsNullOrEmpty(myValue) Then
                                            MessageBox.Show("Se cancelo la Accion")
                                            Return
                                        Else
                                            imprimirActaFondosHistorial(myValue)
                                        End If
                                        '             InputBox("Enter Student's Date of Birth", _
                                        '"Red Oak High School - Student Registration")
                                        'imprimirActaFondosHistorial()
                                End Select

                            Else
                                MessageBox.Show("Debe seleccionar un tipo de orden", "AVISO")
                            End If

                        End If
                    End If
                End If
            Else
                MessageBox.Show("Debe seleccionar una opcion", " RESOLUCION O DISPOSICION??")
            End If

        Else
            MessageBox.Show("Debe seleccionar una opcion", " NACIONAL O PROVINCIAL??")
        End If

    End Sub
    Private Function dameDomicilio(ByRef nombre As String) As String
        Dim dom As String
        sql = " SELECT  pro_Direc FROM [SBDASIPT].dbo.Proveed where pro_RazSoc like '%" & nombre & "%'"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                If reader.Read() Then
                    dom = reader(0)
                End If

            End Using
        End Using
        Return dom
    End Function
    Public Sub imprimirActaFondosHistorial(ByVal Nro As String)
        Dim Documento As New Document 'Declaracion del documento
        Dim parrafo As New Paragraph ' Declaracion de un parrafo

        pdf.PdfWriter.GetInstance(Documento, New FileStream("ActaCompra.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

        Documento.Open() 'Abre documento para su escritura
        For Each row As DataGridViewRow In DataGridView2.Rows
            Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

            imagendemo = iTextSharp.text.Image.GetInstance("MembreteActa.png") 'Dirreccion a la imagen que se hace referencia
            imagendemo.SetAbsolutePosition(5, 750) 'Posicion en el eje cartesiano
            imagendemo.ScaleAbsoluteWidth(550) 'Ancho de la imagen
            imagendemo.ScaleAbsoluteHeight(95) 'Altura de la imagen
            Documento.Add(imagendemo) ' Agrega la imagen al documento
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("Posadas:" & Date.Now) 'Texto que se insertara
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

            sql = "Select Concepto,Fecha from AAOrdenNro where NroIngreso=" & adjuntarAnio() & " and Tipo='ComprasHistorial' and fecha='" & Format(dtFechaIngreso.Value, "yyyy/MM/dd") & "'"
            Dim nroCargo As String
            Dim fecha As String
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()

                    conn.Open()

                    cmd4.CommandText = sql

                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    reader.Read()
                    nroCargo = reader.Item(0)
                    fecha = reader(1)
                End Using
            End Using
            Format(nroCargo, "00000000")
            parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 14, ALIGN_JUSTIFIED) 'Asigan fuente
            parrafo.Add("Acta de Recepción Nro:" & Nro & "/" & Date.Now.Year) 'Texto que se insertara
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("Expediente Nro:") 'Texto que se insertara
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("Resolucion Nro:") 'Texto que se insertara
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("Orden Pago Nro:") 'Texto que se insertara
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("Proveedor :" & row.Cells(2).Value) 'Texto que se insertara
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("Direccion :" & dameDomicilio(row.Cells(2).Value.ToString.Substring(0, 11))) 'Texto que se insertara
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("En la ciudad de Posadas, Capital de la provinica de Misiones en el asiento del Sistema Provincial de " & "Teleducación y Desarrollo (SIPTeD), Departamento de Patrimonio, a los  " & Numalet.ToCardinal(Integer.Parse(fecha.Substring(0, 2))) & " días del mes de " & dameMes(fecha.Substring(3, 2)) & " del año " & Numalet.ToCardinal(Integer.Parse(fecha.Substring(6, 4))) & "  se hace constar por la presente que se han recepcionado del proveedor arriba indicado, según " & "factura/s " & DataGridView2.Rows(row.Index).Cells(1).Value & " los bienes/servicios que se detallan a continuación, los que habiendo sido" & " examinados, han sido encontrados de conformidad tanto en calidad como en cantidad:") 'Texto que se insertara
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Dim tablademo As New PdfPTable(7) 'declara la tabla con 4 Columnas
            tablademo.SetWidthPercentage({70, 70, 140, 90, 70, 70, 70}, PageSize.A4)
            tablademo.HorizontalAlignment = Element.ALIGN_JUSTIFIED
            tablademo.AddCell(New Paragraph("Renglon       ", FontFactory.GetFont("Arial", 12)))
            tablademo.AddCell(New Paragraph("Cant", FontFactory.GetFont("Arial", 12)))
            tablademo.AddCell(New Paragraph("Descripcion", FontFactory.GetFont("Arial", 12)))
            tablademo.AddCell(New Paragraph("Codigo", FontFactory.GetFont("Arial", 12)))
            tablademo.AddCell(New Paragraph("Nro Inventario", FontFactory.GetFont("Arial", 12)))
            tablademo.AddCell(New Paragraph("Pr Unit", FontFactory.GetFont("Arial", 12)))
            tablademo.AddCell(New Paragraph("Pr Total", FontFactory.GetFont("Arial", 12)))

            Documento.Add(tablademo) 'Agrega la tabla al documento
            Dim BANDERITA As Boolean = False
            sql = "SELECT [sdc_NReng],[sdc_CantUM1],[sdc_Desc],[sdcart_CodGen],[DtsArticulos].Dart_2,[sdc_PrecioUn],[sdc_ImpTot],[DtsArticulos].Dart_1  FROM [SBDASIPT].[dbo].[SegDetC] " &
                      "inner join [SBDASIPT].[dbo].[SegCabC] on [SBDASIPT].[dbo].[SegCabC].[scc_ID]=[SBDASIPT].[dbo].[SegDetC].[sdcscc_ID] " &
                      "INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON SegCabC.[scccem_Cod]=CausaEmi.cem_Cod " &
                      "INNER JOIN [SBDASIPT].[dbo].[SegTiposC] on SegCabC.[scc_ID]=[SegTiposC].[spcscc_ID] " &
                      "inner join [SBDASIPT].[dbo].[Articulos] on [SegDetC].sdcart_CodGen=Articulos.art_CodGen " &
                      "inner Join [SBDASIPT].[dbo].[DtsArticulos] on Articulos.art_CodGen=DtsArticulos.art_CodGen " &
                      "where CausaEmi.cem_Desc='" & adjuntarAnio() & "' and spc_Nro=" & txtFact.Text
            Dim tablademo2 As New PdfPTable(7) 'declara la tabla con 4 Columnas
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql
                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    If reader.Read() Then

                        tablademo2.SetWidthPercentage({70, 70, 140, 90, 70, 70, 70}, PageSize.A4)
                        tablademo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                        Do
                            tablademo2.AddCell(New Paragraph(reader(0), FontFactory.GetFont("Arial", 12)))
                            tablademo2.AddCell(New Paragraph(reader(1), FontFactory.GetFont("Arial", 12)))
                            tablademo2.AddCell(New Paragraph(reader(2), FontFactory.GetFont("Arial", 12)))
                            tablademo2.AddCell(New Paragraph(reader(7), FontFactory.GetFont("Arial", 12)))
                            tablademo2.AddCell(New Paragraph(reader(4), FontFactory.GetFont("Arial", 12)))
                            tablademo2.AddCell(New Paragraph(reader(5), FontFactory.GetFont("Arial", 12)))
                            tablademo2.AddCell(New Paragraph(reader(6), FontFactory.GetFont("Arial", 12)))
                        Loop While reader.Read()
                    Else
                        BANDERITA = True
                    End If

                End Using
            End Using
            If BANDERITA = True Then
                sql = "SELECT [sdc_NReng],[sdc_CantUM1],[sdc_Desc],[sdcart_CodGen],[sdc_PrecioUn],[sdc_ImpTot]  FROM [SBDASIPT].[dbo].[SegDetC] " &
                                         "inner join [SBDASIPT].[dbo].[SegCabC] on [SBDASIPT].[dbo].[SegCabC].[scc_ID]=[SBDASIPT].[dbo].[SegDetC].[sdcscc_ID] " &
                                          "INNER JOIN [SBDASIPT].[dbo].[SegTiposC] on SegCabC.[scc_ID]=[SegTiposC].[spcscc_ID] " &
                                         "INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON SegCabC.[scccem_Cod]=CausaEmi.cem_Cod " &
                                         "where CausaEmi.cem_Desc='" & adjuntarAnio() & "' and spc_Nro=" & txtFact.Text

                tablademo2.SetWidthPercentage({70, 70, 140, 90, 70, 70, 70}, PageSize.A4)
                tablademo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()
                        conn.Open()
                        cmd4.CommandText = sql
                        Dim reader As SqlDataReader = Nothing
                        reader = cmd4.ExecuteReader

                        While reader.Read()
                            tablademo2.AddCell(New Paragraph(reader(0), FontFactory.GetFont("Arial", 12)))
                            tablademo2.AddCell(New Paragraph(reader(1), FontFactory.GetFont("Arial", 12)))
                            tablademo2.AddCell(New Paragraph(reader(2), FontFactory.GetFont("Arial", 12)))
                            tablademo2.AddCell(New Paragraph("0", FontFactory.GetFont("Arial", 12)))
                            tablademo2.AddCell(New Paragraph("0", FontFactory.GetFont("Arial", 12)))
                            tablademo2.AddCell(New Paragraph(reader(4), FontFactory.GetFont("Arial", 12)))
                            tablademo2.AddCell(New Paragraph(reader(5), FontFactory.GetFont("Arial", 12)))
                        End While

                    End Using
                End Using
            End If
            Documento.Add(tablademo2) 'Agrega la tabla al documento
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Dim banMonto As Boolean
            sql = "SELECT sum([sdc_ImpTot])  FROM [SBDASIPT].[dbo].[SegDetC] " &
                    "inner join [SBDASIPT].[dbo].[SegCabC] on [SBDASIPT].[dbo].[SegCabC].[scc_ID]=[SBDASIPT].[dbo].[SegDetC].[sdcscc_ID] " &
                    "INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON SegCabC.[scccem_Cod]=CausaEmi.cem_Cod " &
                    "INNER JOIN [SBDASIPT].[dbo].[SegTiposC] on SegCabC.[scc_ID]=[SegTiposC].[spcscc_ID] " &
                    "inner join [SBDASIPT].[dbo].[Articulos] on [SegDetC].sdcart_CodGen=Articulos.art_CodGen " &
                    "inner Join [SBDASIPT].[dbo].[DtsArticulos] on Articulos.art_CodGen=DtsArticulos.art_CodGen " &
                    "where CausaEmi.cem_Desc='" & adjuntarAnio() & "' and spc_Nro=" & txtFact.Text
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql
                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    If reader.Read() Then
                        parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                        parrafo.Add("Son:$" & reader(0) & "    Pesos:" & Numalet.ToCardinal(reader(0).ToString))
                        Documento.Add(parrafo) 'Agrega el parrafo al documento
                        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    Else
                        banMonto = True
                    End If
                End Using
            End Using
            If banMonto = True Then
                sql = "SELECT sum([sdc_ImpTot])  FROM [SBDASIPT].[dbo].[SegDetC] " &
                    "inner join [SBDASIPT].[dbo].[SegCabC] on [SBDASIPT].[dbo].[SegCabC].[scc_ID]=[SBDASIPT].[dbo].[SegDetC].[sdcscc_ID] " &
                    "INNER JOIN [SBDASIPT].[dbo].[SegTiposC] on SegCabC.[scc_ID]=[SegTiposC].[spcscc_ID] " &
                    "INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON SegCabC.[scccem_Cod]=CausaEmi.cem_Cod " &
                    "where CausaEmi.cem_Desc='" & adjuntarAnio() & "' and spc_Nro=" & txtFact.Text
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()
                        conn.Open()
                        cmd4.CommandText = sql
                        Dim reader As SqlDataReader = Nothing
                        reader = cmd4.ExecuteReader
                        If reader.Read() Then
                            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                            parrafo.Add("Son:$" & reader(0) & "    Pesos:" & Numalet.ToCardinal(reader(0).ToString))
                            Documento.Add(parrafo) 'Agrega el parrafo al documento
                            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                        Else
                            banMonto = True
                        End If
                    End Using
                End Using
            End If
        Next
        Documento.Close() 'Cierra el documento
        System.Diagnostics.Process.Start("ActaCompra.pdf") 'Abre el archivo DEMO.PDF
    End Sub
    Public Sub imprimirActaFondos()
        Dim nroCargo As String
        nroCargo = txtSaliente.Text
        Dim nro As String
        Format(nroCargo, "00000000")
        Dim fecha As String
        fecha = dtFechaIngreso.Text
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
            sql = "update Talonar set tal_ActualNro='" & nroCargo & "' where tal_Cod='ACT'"
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

        If almacenarActa(nroCargo, fecha) Then
            Try

                Dim Documento As New Document 'Declaracion del documento
                Dim parrafo As New Paragraph ' Declaracion de un parrafo

                pdf.PdfWriter.GetInstance(Documento, New FileStream("ActaCompra.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

                Documento.Open() 'Abre documento para su escritura
                For Each row As DataGridViewRow In DataGridView2.Rows
                    Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

                    imagendemo = iTextSharp.text.Image.GetInstance("MembreteActa.png") 'Dirreccion a la imagen que se hace referencia
                    imagendemo.SetAbsolutePosition(5, 750) 'Posicion en el eje cartesiano
                    imagendemo.ScaleAbsoluteWidth(550) 'Ancho de la imagen
                    imagendemo.ScaleAbsoluteHeight(95) 'Altura de la imagen
                    Documento.Add(imagendemo) ' Agrega la imagen al documento
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("Posadas:" & Date.Now) 'Texto que se insertara
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente


                    parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 14, ALIGN_JUSTIFIED) 'Asigan fuente
                    parrafo.Add("Acta de Recepción Nro:" & nroCargo & "/" & Date.Now.Year) 'Texto que se insertara
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("Expediente Nro:") 'Texto que se insertara
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("Resolucion Nro:") 'Texto que se insertara
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("Orden Pago Nro:") 'Texto que se insertara
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("Proveedor :" & row.Cells(2).Value) 'Texto que se insertara
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("Direccion :" & dameDomicilio(row.Cells(2).Value.ToString.Substring(0, 11))) 'Texto que se insertara
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("En la ciudad de Posadas, Capital de la provinica de Misiones en el asiento del Sistema Provincial de " & "Teleducación y Desarrollo (SIPTeD), Departamento de Patrimonio, a los  " & Numalet.ToCardinal(Integer.Parse(fecha.Substring(0, 2))) & " días del mes de " & dameMes(fecha.Substring(3, 2)) & " del año " & Numalet.ToCardinal(Integer.Parse(fecha.Substring(6, 4))) & "  se hace constar por la presente que se han recepcionado del proveedor arriba indicado, según " & "factura/s " & DataGridView2.Rows(row.Index).Cells(1).Value & " los bienes/servicios que se detallan a continuación, los que habiendo sido" & " examinados, han sido encontrados de conformidad tanto en calidad como en cantidad:") 'Texto que se insertara
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Dim tablademo As New PdfPTable(7) 'declara la tabla con 4 Columnas
                    tablademo.SetWidthPercentage({70, 70, 140, 90, 70, 70, 70}, PageSize.A4)
                    tablademo.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                    tablademo.AddCell(New Paragraph("Renglon       ", FontFactory.GetFont("Arial", 12)))
                    tablademo.AddCell(New Paragraph("Cant", FontFactory.GetFont("Arial", 12)))
                    tablademo.AddCell(New Paragraph("Descripcion", FontFactory.GetFont("Arial", 12)))
                    tablademo.AddCell(New Paragraph("Codigo", FontFactory.GetFont("Arial", 12)))
                    tablademo.AddCell(New Paragraph("Nro Inventario", FontFactory.GetFont("Arial", 12)))
                    tablademo.AddCell(New Paragraph("Pr Unit", FontFactory.GetFont("Arial", 12)))
                    tablademo.AddCell(New Paragraph("Pr Total", FontFactory.GetFont("Arial", 12)))

                    Documento.Add(tablademo) 'Agrega la tabla al documento
                    Dim BANDERITA As Boolean = False
                    sql = "SELECT [sdc_NReng],[sdc_CantUM1],[sdc_Desc],[sdcart_CodGen],[DtsArticulos].Dart_2,[sdc_PrecioUn],[sdc_ImpTot],[DtsArticulos].Dart_1  FROM [SBDASIPT].[dbo].[SegDetC] " &
                          "inner join [SBDASIPT].[dbo].[SegCabC] on [SBDASIPT].[dbo].[SegCabC].[scc_ID]=[SBDASIPT].[dbo].[SegDetC].[sdcscc_ID] " &
                          "INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON SegCabC.[scccem_Cod]=CausaEmi.cem_Cod " &
                          "INNER JOIN [SBDASIPT].[dbo].[SegTiposC] on SegCabC.[scc_ID]=[SegTiposC].[spcscc_ID] " &
                          "inner join [SBDASIPT].[dbo].[Articulos] on [SegDetC].sdcart_CodGen=Articulos.art_CodGen " &
                          "inner Join [SBDASIPT].[dbo].[DtsArticulos] on Articulos.art_CodGen=DtsArticulos.art_CodGen " &
                          "where CausaEmi.cem_Desc='" & adjuntarAnio() & "' and spc_Nro=" & txtFact.Text
                    Dim tablademo2 As New PdfPTable(7) 'declara la tabla con 4 Columnas
                    Using conn As New SqlConnection(CONNSTR)
                        Using cmd4 As SqlCommand = conn.CreateCommand()
                            conn.Open()
                            cmd4.CommandText = sql
                            Dim reader As SqlDataReader = Nothing
                            reader = cmd4.ExecuteReader
                            If reader.Read() Then

                                tablademo2.SetWidthPercentage({70, 70, 140, 90, 70, 70, 70}, PageSize.A4)
                                tablademo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                                Do
                                    tablademo2.AddCell(New Paragraph(reader(0), FontFactory.GetFont("Arial", 12)))
                                    tablademo2.AddCell(New Paragraph(reader(1), FontFactory.GetFont("Arial", 12)))
                                    tablademo2.AddCell(New Paragraph(reader(2), FontFactory.GetFont("Arial", 12)))
                                    tablademo2.AddCell(New Paragraph(reader(7), FontFactory.GetFont("Arial", 12)))
                                    tablademo2.AddCell(New Paragraph(reader(4), FontFactory.GetFont("Arial", 12)))
                                    tablademo2.AddCell(New Paragraph(reader(5), FontFactory.GetFont("Arial", 12)))
                                    tablademo2.AddCell(New Paragraph(reader(6), FontFactory.GetFont("Arial", 12)))
                                Loop While reader.Read()
                            Else
                                BANDERITA = True
                            End If

                        End Using
                    End Using
                    If BANDERITA = True Then
                        sql = "SELECT [sdc_NReng],[sdc_CantUM1],[sdc_Desc],[sdcart_CodGen],[sdc_PrecioUn],[sdc_ImpTot]  FROM [SBDASIPT].[dbo].[SegDetC] " &
                                             "inner join [SBDASIPT].[dbo].[SegCabC] on [SBDASIPT].[dbo].[SegCabC].[scc_ID]=[SBDASIPT].[dbo].[SegDetC].[sdcscc_ID] " &
                                              "INNER JOIN [SBDASIPT].[dbo].[SegTiposC] on SegCabC.[scc_ID]=[SegTiposC].[spcscc_ID] " &
                                             "INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON SegCabC.[scccem_Cod]=CausaEmi.cem_Cod " &
                                             "where CausaEmi.cem_Desc='" & adjuntarAnio() & "' and spc_Nro=" & txtFact.Text

                        tablademo2.SetWidthPercentage({70, 70, 140, 90, 70, 70, 70}, PageSize.A4)
                        tablademo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                        Using conn As New SqlConnection(CONNSTR)
                            Using cmd4 As SqlCommand = conn.CreateCommand()
                                conn.Open()
                                cmd4.CommandText = sql
                                Dim reader As SqlDataReader = Nothing
                                reader = cmd4.ExecuteReader

                                While reader.Read()
                                    tablademo2.AddCell(New Paragraph(reader(0), FontFactory.GetFont("Arial", 12)))
                                    tablademo2.AddCell(New Paragraph(reader(1), FontFactory.GetFont("Arial", 12)))
                                    tablademo2.AddCell(New Paragraph(reader(2), FontFactory.GetFont("Arial", 12)))
                                    tablademo2.AddCell(New Paragraph("0", FontFactory.GetFont("Arial", 12)))
                                    tablademo2.AddCell(New Paragraph("0", FontFactory.GetFont("Arial", 12)))
                                    tablademo2.AddCell(New Paragraph(reader(4), FontFactory.GetFont("Arial", 12)))
                                    tablademo2.AddCell(New Paragraph(reader(5), FontFactory.GetFont("Arial", 12)))
                                End While

                            End Using
                        End Using
                    End If
                    Documento.Add(tablademo2) 'Agrega la tabla al documento
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Dim banMonto As Boolean
                    sql = "SELECT sum([sdc_ImpTot])  FROM [SBDASIPT].[dbo].[SegDetC] " &
                        "inner join [SBDASIPT].[dbo].[SegCabC] on [SBDASIPT].[dbo].[SegCabC].[scc_ID]=[SBDASIPT].[dbo].[SegDetC].[sdcscc_ID] " &
                        "INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON SegCabC.[scccem_Cod]=CausaEmi.cem_Cod " &
                        "INNER JOIN [SBDASIPT].[dbo].[SegTiposC] on SegCabC.[scc_ID]=[SegTiposC].[spcscc_ID] " &
                        "inner join [SBDASIPT].[dbo].[Articulos] on [SegDetC].sdcart_CodGen=Articulos.art_CodGen " &
                        "inner Join [SBDASIPT].[dbo].[DtsArticulos] on Articulos.art_CodGen=DtsArticulos.art_CodGen " &
                        "where CausaEmi.cem_Desc='" & adjuntarAnio() & "' and spc_Nro=" & txtFact.Text
                    Using conn As New SqlConnection(CONNSTR)
                        Using cmd4 As SqlCommand = conn.CreateCommand()
                            conn.Open()
                            cmd4.CommandText = sql
                            Dim reader As SqlDataReader = Nothing
                            reader = cmd4.ExecuteReader
                            If reader.Read() Then
                                parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                                parrafo.Add("Son:$" & txtTotalGastos.Text & "    Pesos:" & Numalet.ToCardinal(txtTotalGastos.Text))
                                Documento.Add(parrafo) 'Agrega el parrafo al documento
                                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                            Else
                                banMonto = True
                            End If
                        End Using
                    End Using
                    If banMonto = True Then
                        sql = "SELECT sum([sdc_ImpTot])  FROM [SBDASIPT].[dbo].[SegDetC] " &
                        "inner join [SBDASIPT].[dbo].[SegCabC] on [SBDASIPT].[dbo].[SegCabC].[scc_ID]=[SBDASIPT].[dbo].[SegDetC].[sdcscc_ID] " &
                        "INNER JOIN [SBDASIPT].[dbo].[SegTiposC] on SegCabC.[scc_ID]=[SegTiposC].[spcscc_ID] " &
                        "INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON SegCabC.[scccem_Cod]=CausaEmi.cem_Cod " &
                        "where CausaEmi.cem_Desc='" & adjuntarAnio() & "' and spc_Nro=" & txtFact.Text
                        Using conn As New SqlConnection(CONNSTR)
                            Using cmd4 As SqlCommand = conn.CreateCommand()
                                conn.Open()
                                cmd4.CommandText = sql
                                Dim reader As SqlDataReader = Nothing
                                reader = cmd4.ExecuteReader
                                If reader.Read() Then
                                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                                    parrafo.Add("Son:$" & txtTotalGastos.Text & "    Pesos:" & Numalet.ToCardinal(txtTotalGastos.Text))
                                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                                Else
                                    banMonto = True
                                End If
                            End Using
                        End Using
                    End If
                    Documento.NewPage()

                Next

                Documento.Close() 'Cierra el documento
                System.Diagnostics.Process.Start("ActaCompra.pdf") 'Abre el archivo DEMO.PDF

            Catch ex As Exception
                MessageBox.Show("Compruebe que no tiene el documento abierto previamente", "AVISO")
            End Try
        Else

        End If

    End Sub
    Public Sub impirmirOrdenCargoPrimeroHistorial()
        Dim Documento As New Document(PageSize.LEGAL, 60, 5, 35, 5) 'Declaracion del documento
        Dim parrafo As New Paragraph ' Declaracion de un parrafo
        Dim tablademo As New PdfPTable(4) 'declara la tabla con 4 Columnas

        pdf.PdfWriter.GetInstance(Documento, New FileStream("Compra.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

        Documento.Open() 'Abre documento para su escritura

        parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("PROVINCIAL DE MISIONES" & vbCr & "SISTEMA PROVINCIAL" & vbCr & "DE TELEDUCACION Y DESARROLLO") 'Texto que se insertara
        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

        Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

        imagendemo = iTextSharp.text.Image.GetInstance("descarga.jpg") 'Dirreccion a la imagen que se hace referencia
        imagendemo.SetAbsolutePosition(490, 920)  'Posicion en el eje cartesiano
        imagendemo.ScaleAbsoluteWidth(100) 'Ancho de la imagen
        imagendemo.ScaleAbsoluteHeight(100) 'Altura de la imagen
        Documento.Add(imagendemo) ' Agrega la imagen al documento

        parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("ORDEN DE PAGO NRO:" & dameNroHistorial("ComprasPago") & "                             ") 'Texto que se insertara
        parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("FECHA:" & Date.Now) 'Texto que se insertara
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Dim res As String
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
        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" DETALLES DE LAS COMPRAS: ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Dim tablademo6 As New PdfPTable(4) 'declara la tabla con 4 Columnas

        tablademo6.SetWidthPercentage({100, 150, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo6.AddCell(New Paragraph("FECHA        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        tablademo6.AddCell(New Paragraph("COMPROBANTE")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
        tablademo6.AddCell(New Paragraph("DETALLE       ")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo6.AddCell(New Paragraph("IMPORTE")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        Documento.Add(tablademo6) 'Agrega la tabla al documento
        Dim tablademo4 As New PdfPTable(4) 'declara la tabla con 4 Columnas
        'tablademo4.HorizontalAlignment = ALIGN_LEFT

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
        Dim tabla9 As New PdfPTable(4)
        'tabla9.HorizontalAlignment = ALIGN_LEFT
        tabla9.SetWidthPercentage({100, 150, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tabla9.AddCell("TOTAL")
        tabla9.AddCell(" ")
        tabla9.AddCell(" ")

        Dim miParrafo As String
        Dim miTotal As Double
        miTotal = Double.Parse(txtTotalGastos.Text)
        miParrafo = Format(miTotal, "###,##0.00")
        Dim miCelda As New PdfPCell
        parrafo.Add(miParrafo)
        miCelda.AddElement(parrafo)

        miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
        tabla9.AddCell(miCelda)
        Documento.Add(tabla9)
        parrafo.Clear()
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph("TOTAL:$" & txtTotalGastos.Text & "(PESOS " & Numalet.ToCardinal(miTotal) & ")")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Dim tabla93 As New PdfPTable(3)
        tabla93.SetWidthPercentage({150, 200, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tabla93.AddCell("Detalle")
        tabla93.AddCell("Imputación")
        tabla93.AddCell("Situación Fiscal")
        Documento.Add(tabla93) 'Salto de linea
        sql = "SELECT [sdc_Desc],SegCabC.sccpro_RazSoc FROM [SBDASIPT].[dbo].[SegCabC] inner join [SBDASIPT].dbo.[SegDetC] ON SegCabC.scc_ID=SegDetC.sdcscc_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=SegCabC.scccem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " order by SegCabC.sccpro_RazSoc"
        Dim tabla92 As New PdfPTable(3)
        tabla92.SetWidthPercentage({150, 200, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Do While reader.Read()
                    'Dim Imp As String
                    'Imp = " IMPUTACION: " & reader(0)
                    tabla92.AddCell(reader(1))
                    tabla92.AddCell(reader(0))
                    tabla92.AddCell(" ")
                Loop

            End Using
        End Using
        Documento.Add(tabla92) 'Salto de linea

        Documento.Add(New Paragraph(" ")) 'Salto de linea

        If GroupBox7.Visible = True Then

            Documento.Add(New Paragraph("DATOS DE RETENCIONES: ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea

            Dim tablademo8 As New PdfPTable(4) 'declara la tabla con 4 Columnas
            tablademo8.SetWidthPercentage({100, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tablademo8.AddCell(New Paragraph("NUMERO", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo8.AddCell(New Paragraph("ARTICULO", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo8.AddCell(New Paragraph("CODIGO", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo8.AddCell(New Paragraph("IMPORTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            Documento.Add(tablademo8) 'Agrega la tabla al documento
            Dim tablademo9 As New PdfPTable(4) 'declara la tabla con 4 Columnas
            tablademo9.SetWidthPercentage({100, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
            For Each row As DataGridViewRow In DataGridView3.Rows
                For Each column As DataGridViewColumn In DataGridView3.Columns
                    If column.Index <= 4 Then
                        tablademo9.AddCell(New Paragraph(row.Cells(column.Index).Value, FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                    End If

                Next
            Next
            Documento.Add(tablademo9) 'Agrega la tabla al documento
        End If

        Documento.Add(New Paragraph(" ")) 'Salto de linea

        Documento.Add(New Paragraph("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL")) 'Salto de linea

        Documento.Close() 'Cierra el documento
        System.Diagnostics.Process.Start("Compra.pdf") 'Abre el archivo DEMO.PDF

    End Sub
    Private Function almacenarActa(ByRef nro As String, ByRef fecha As String) As Boolean
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
                    cmd.Parameters.Add("@Tipo", SqlDbType.VarChar, 50).Value = "ComprasHistorial"
                    cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = fecha
                    cmd.Parameters.Add("@NacPro", SqlDbType.VarChar, 3).Value = "ACT"
                    cmd.ExecuteScalar()
                End Using
            End Using
            bandera = True
            MessageBox.Show("Se cargo correctamente el acta", "EXITO")
        Catch ex As Exception
            bandera = False
            MessageBox.Show("No se pudo cargar el acta", "AVISO")
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
    Public Sub imprimirOrdenCargoChequesHistorial()
        Dim Documento As New Document(PageSize.A4, 60, 5, 35, 5) 'Declaracion del documento
        Dim parrafo As New Paragraph ' Declaracion de un parrafo
        Dim tablademo As New PdfPTable(5) 'declara la tabla con 4 Columnas
        Dim subtotal3 As Double
        pdf.PdfWriter.GetInstance(Documento, New FileStream("CompraPago.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

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

        Documento.Add(New Paragraph(" ")) 'Salto de linea

        parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("ORDEN PAGO NRO:" & dameNroHistorial("ComprasPago") & "  ")
        parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("FECHA:" & Date.Now) 'Texto que se insertara

        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        'Dim Nro As String
        'sql = "Select Concepto from AAOrdenNro where Tipo='ComprasPago' and NroIngreso=" & adjuntarAnio()
        'Using conn As New SqlConnection(CONNSTR)
        '    Using cmd4 As SqlCommand = conn.CreateCommand()

        '        conn.Open()

        '        cmd4.CommandText = sql

        '        Dim reader As SqlDataReader = Nothing
        '        reader = cmd4.ExecuteReader
        '        If reader.Read() Then
        '            Nro = reader(0)
        '        End If
        '    End Using
        'End Using

        'Documento.Add(New Paragraph("ORDEN CARGO ASOCIADA: " & Nro, FontFactory.GetFont("Arial", 9))) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea

        Dim tablademo5 As New PdfPTable(5) 'declara la tabla con 4 Columnas
        tablademo5.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT
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
                    If IsNumeric(row1.Cells(column.Index).Value) And column.Index = 4 Then
                        'subtotal3 = subtotal3 + row1.Cells(column.Index).Value
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
        tabla9.AddCell("SUBTOTAL")
        tabla9.AddCell(" ")
        tabla9.AddCell(" ")
        tabla9.AddCell(" ")
        Dim miParrafo As String
        Dim miTotal As Double
        miTotal = Double.Parse(txtTotalGastos.Text)
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
        tablademo.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

        tablademo.AddCell(New Paragraph("F. EMISION        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        tablademo.AddCell(New Paragraph("BENEFICIARIO        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo.AddCell(New Paragraph("NRO CTA        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo.AddCell(New Paragraph("CH. Y/O AUT.        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo.AddCell(New Paragraph("IMPORTE        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        Documento.Add(tablademo) 'Agrega la tabla al documento

        Dim tablademo2 As New PdfPTable(5) 'declara la tabla con 4 Columnas
        tablademo2.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

        For Each row1 As DataGridViewRow In DataGridView1.Rows
            For Each column As DataGridViewColumn In DataGridView1.Columns
                If column.Index <= 5 Then
                    Dim paraParrrafo As String
                    If IsNumeric(row1.Cells(column.Index).Value) And column.Index = 4 Then

                        paraParrrafo = Format(row1.Cells(column.Index).Value, "###,##0.00")
                        Dim miCelda2 As New PdfPCell
                        parrafo.Add(paraParrrafo)
                        miCelda2.AddElement(parrafo)
                        miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                        tablademo2.AddCell(miCelda2)
                        parrafo.Clear()
                    Else
                        paraParrrafo = row1.Cells(column.Index).Value
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
        Dim tabla10 As New PdfPTable(5)
        'tabla9.HorizontalAlignment = ALIGN_LEFT
        tabla10.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tabla10.AddCell("SUBTOTAL")
        tabla10.AddCell(" ")
        tabla10.AddCell(" ")
        tabla10.AddCell(" ")
        Dim miParrafo2 As String
        Dim miTotal2 As Double
        miTotal2 = Double.Parse(txtTotalFinal.Text)
        miParrafo2 = Format(miTotal2, "###,##0.00")
        Dim miCelda3 As New PdfPCell
        parrafo.Add(miParrafo2)
        miCelda3.AddElement(parrafo)

        miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
        tabla10.AddCell(miCelda3)
        Documento.Add(tabla10)
        parrafo.Clear()
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        If GroupBox7.Visible = True Then

            Documento.Add(New Paragraph("DATOS DE RETENCIONES: ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea

            Dim tablademo8 As New PdfPTable(4) 'declara la tabla con 4 Columnas
            tablademo8.SetWidthPercentage({100, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tablademo8.AddCell(New Paragraph("NUMERO", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo8.AddCell(New Paragraph("ARTICULO", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo8.AddCell(New Paragraph("CODIGO", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo8.AddCell(New Paragraph("IMPORTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            Documento.Add(tablademo8) 'Agrega la tabla al documento
            Dim tablademo9 As New PdfPTable(4) 'declara la tabla con 4 Columnas
            tablademo9.SetWidthPercentage({100, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
            For Each row As DataGridViewRow In DataGridView3.Rows
                For Each column As DataGridViewColumn In DataGridView3.Columns
                    If column.Index <= 4 Then
                        Dim paraParrrafo As String
                        If IsNumeric(row.Cells(column.Index).Value) And column.Index = 3 Then
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
            subtotal3 = 0
            For Each row3 As DataGridViewRow In DataGridView3.Rows
                subtotal3 = subtotal3 + row3.Cells(3).Value
            Next
            Documento.Add(tablademo9) 'Agrega la tabla al documento
        End If
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea

        Dim GranTotal As Double
        Dim subTotal As Double
        Dim subTotal2 As Double

        subTotal = Double.Parse(txtTotalFinal.Text)
        subTotal2 = Double.Parse(txtTotalGastos.Text)
        GranTotal = subTotal + subTotal2 + subtotal3
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
        System.Diagnostics.Process.Start("CompraPago.pdf") 'Abre el archivo DEMO.PDF

    End Sub
    Private Function dameMes(ByRef mes As Integer) As String
        Dim miMes As String
        If mes = 1 Then
            miMes = "enero"
        End If
        If mes = 2 Then
            miMes = "febrero"
        End If
        If mes = 3 Then
            miMes = "marzo"
        End If
        If mes = 4 Then
            miMes = "abril"
        End If
        If mes = 5 Then
            miMes = "mayo"
        End If
        If mes = 6 Then
            miMes = "junio"
        End If
        If mes = 7 Then
            miMes = "julio"
        End If
        If mes = 8 Then
            miMes = "agosto"
        End If
        If mes = 9 Then
            miMes = "septiembre"
        End If
        If mes = 10 Then
            miMes = "octubre"
        End If
        If mes = 11 Then
            miMes = "noviembre"
        End If
        If mes = 12 Then
            miMes = "diciembre"
        End If
        Return miMes
    End Function
    Public Sub impirmirOrdenCargoPrimero()
        Dim Origen As String
        If chkNac.Checked Then
            Origen = "OPN"
        End If
        If chkProv.Checked Then
            Origen = "OPP"
        End If

        sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='ComprasPago' "

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

        If BANDERA = True Then
            Select Case MsgBox("¿Ya existe una orden con este numero desea reemplazarla?", MsgBoxStyle.YesNo, "AVISO")
                'en caso de seleccionar un si
                Case MsgBoxResult.Yes
                    'Elimina la tupla existente en la base de datos y despues inserta la nueva orden de pago
                    reemplazarImprimirCompras()
                Case MsgBoxResult.No
                    MessageBox.Show("Accion cancelada por el usuario", "INFORMACION")
            End Select

            'impirmirOrdenCargoPrimeroHistorial()
        Else

            Dim Documento As New Document(PageSize.LEGAL, 60, 5, 35, 5) 'Declaracion del documento
            Dim parrafo As New Paragraph ' Declaracion de un parrafo
            Dim tablademo As New PdfPTable(4) 'declara la tabla con 4 Columnas
            Dim nro As String
            Dim nroCargo As String


            nroCargo = txtSaliente.Text
            If almacenarOrden(nroCargo, Origen) Then
                If almacenarDisp() Then
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

                    pdf.PdfWriter.GetInstance(Documento, New FileStream("Compra.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

                    Documento.Open() 'Abre documento para su escritura

                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("PROVINCIAL DE MISIONES" & vbCr & "SISTEMA PROVINCIAL" & vbCr & "DE TELEDUCACION Y DESARROLLO") 'Texto que se insertara
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                    Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

                    imagendemo = iTextSharp.text.Image.GetInstance("descarga.jpg") 'Dirreccion a la imagen que se hace referencia
                    imagendemo.SetAbsolutePosition(490, 920)  'Posicion en el eje cartesiano
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
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    If chkDisp.Checked Then
                        Documento.Add(New Paragraph("Disposición Nro:" & txtNro.Text)) 'Salto de linea
                    End If
                    If chkRes.Checked Then
                        Documento.Add(New Paragraph("Resolución Nro:" & txtNro.Text)) 'Salto de linea
                    End If

                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" DETALLES DE LAS COMPRAS: ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Dim tablademo6 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                    tablademo6.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT
                    'tablademo6.HorizontalAlignment = Element.ALIGN_LEFT
                    tablademo6.SetWidthPercentage({100, 150, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                    tablademo6.AddCell(New Paragraph("FECHA        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                    tablademo6.AddCell(New Paragraph("COMPROBANTE")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                    tablademo6.AddCell(New Paragraph("DETALLE       ")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10


                    tablademo6.AddCell(New Paragraph("IMPORTE")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

                    Documento.Add(tablademo6) 'Agrega la tabla al documento
                    Dim tablademo4 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                    'tablademo4.HorizontalAlignment = ALIGN_LEFT
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

                    Dim tabla9 As New PdfPTable(4)
                    'tabla9.HorizontalAlignment = ALIGN_LEFT
                    tabla9.SetWidthPercentage({100, 150, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                    tabla9.AddCell("TOTAL")
                    tabla9.AddCell(" ")
                    tabla9.AddCell(" ")
                    Dim miParrafo As String
                    Dim miTotal As Double
                    miTotal = Double.Parse(txtTotalGastos.Text)
                    miParrafo = Format(miTotal, "###,##0.00")
                    Dim miCelda As New PdfPCell
                    parrafo.Add(miParrafo)
                    miCelda.AddElement(parrafo)

                    miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
                    tabla9.AddCell(miCelda)
                    Documento.Add(tabla9)
                    parrafo.Clear()
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph("TOTAL:$" & txtTotalGastos.Text & "(PESOS " & Numalet.ToCardinal(miTotal) & ")")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea

                    Dim tabla93 As New PdfPTable(3)
                    tabla93.SetWidthPercentage({150, 200, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
                    tabla93.AddCell("Detalle")
                    tabla93.AddCell("Imputación")
                    tabla93.AddCell("Situación Fiscal")
                    Documento.Add(tabla93) 'Salto de linea
                    sql = "SELECT [sdc_Desc],SegCabC.sccpro_RazSoc FROM [SBDASIPT].[dbo].[SegCabC] inner join [SBDASIPT].dbo.[SegDetC] ON SegCabC.scc_ID=SegDetC.sdcscc_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=SegCabC.scccem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " order by SegCabC.sccpro_RazSoc"
                    Dim tabla92 As New PdfPTable(3)
                    tabla92.SetWidthPercentage({150, 200, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
                    Using conn As New SqlConnection(CONNSTR)
                        Using cmd4 As SqlCommand = conn.CreateCommand()
                            conn.Open()
                            cmd4.CommandText = sql
                            Dim reader As SqlDataReader = Nothing
                            reader = cmd4.ExecuteReader
                            Do While reader.Read()
                                'Dim Imp As String
                                'Imp = " IMPUTACION: " & reader(0)
                                tabla92.AddCell(reader(1))
                                tabla92.AddCell(reader(0))
                                tabla92.AddCell(" ")
                            Loop

                        End Using
                    End Using
                    Documento.Add(tabla92) 'Salto de linea


                    Documento.Add(New Paragraph(" ")) 'Salto de linea



                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL")) 'Salto de linea

                    Documento.Close() 'Cierra el documento
                    System.Diagnostics.Process.Start("Compra.pdf") 'Abre el archivo DEMO.PDF

                Else

                End If
            Else

            End If

        End If
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
    Public Sub reemplazarImprimirCompras()
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
        reemplazarCompra()
        MessageBox.Show("El remplazo se logro de manera correcta")
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
    Public Sub reemplazarCompra()

        Dim Origen As String
        If chkNac.Checked Then
            Origen = "OPN"
        End If
        If chkProv.Checked Then
            Origen = "OPP"
        End If

        sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='ComprasPago' "

        Dim BANDERA As Boolean

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                'If reader.Read() Then
                'If reader(0).ToString.Equals("") Or reader(0).ToString.Equals(Nothing) Then
                'BANDERA = False
                'Else
                'If IsDBNull(reader(0)) Then
                'BANDERA = False
                'Else
                'BANDERA = True
                'End If
                'End If
                'Else
                'BANDERA = False
                'End If

            End Using
        End Using



        Dim Documento As New Document(PageSize.LEGAL, 60, 5, 35, 5) 'Declaracion del documento
        Dim parrafo As New Paragraph ' Declaracion de un parrafo
        Dim tablademo As New PdfPTable(4) 'declara la tabla con 4 Columnas
        Dim nro As String
        Dim nroCargo As String


        nroCargo = txtSaliente.Text
        If almacenarOrden(nroCargo, Origen) Then
            If almacenarDisp() Then
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

                pdf.PdfWriter.GetInstance(Documento, New FileStream("Compra.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

                Documento.Open() 'Abre documento para su escritura

                parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                parrafo.Add("PROVINCIAL DE MISIONES" & vbCr & "SISTEMA PROVINCIAL" & vbCr & "DE TELEDUCACION Y DESARROLLO") 'Texto que se insertara
                Documento.Add(parrafo) 'Agrega el parrafo al documento
                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

                imagendemo = iTextSharp.text.Image.GetInstance("descarga.jpg") 'Dirreccion a la imagen que se hace referencia
                imagendemo.SetAbsolutePosition(490, 920)  'Posicion en el eje cartesiano
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
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                If chkDisp.Checked Then
                    Documento.Add(New Paragraph("Disposición Nro:" & txtNro.Text)) 'Salto de linea
                End If
                If chkRes.Checked Then
                    Documento.Add(New Paragraph("Resolución Nro:" & txtNro.Text)) 'Salto de linea
                End If

                Documento.Add(parrafo) 'Agrega el parrafo al documento
                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                Documento.Add(New Paragraph(" DETALLES DE LAS COMPRAS: ")) 'Salto de linea
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                Dim tablademo6 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                tablademo6.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT
                'tablademo6.HorizontalAlignment = Element.ALIGN_LEFT
                tablademo6.SetWidthPercentage({100, 150, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                tablademo6.AddCell(New Paragraph("FECHA        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                tablademo6.AddCell(New Paragraph("COMPROBANTE")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                tablademo6.AddCell(New Paragraph("DETALLE       ")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10


                tablademo6.AddCell(New Paragraph("IMPORTE")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

                Documento.Add(tablademo6) 'Agrega la tabla al documento
                Dim tablademo4 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                'tablademo4.HorizontalAlignment = ALIGN_LEFT
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

                Dim tabla9 As New PdfPTable(4)
                'tabla9.HorizontalAlignment = ALIGN_LEFT
                tabla9.SetWidthPercentage({100, 150, 200, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                tabla9.AddCell("TOTAL")
                tabla9.AddCell(" ")
                tabla9.AddCell(" ")
                Dim miParrafo As String
                Dim miTotal As Double
                miTotal = Double.Parse(txtTotalGastos.Text)
                miParrafo = Format(miTotal, "###,##0.00")
                Dim miCelda As New PdfPCell
                parrafo.Add(miParrafo)
                miCelda.AddElement(parrafo)

                miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
                tabla9.AddCell(miCelda)
                Documento.Add(tabla9)
                parrafo.Clear()
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                Documento.Add(New Paragraph("TOTAL:$" & txtTotalGastos.Text & "(PESOS " & Numalet.ToCardinal(miTotal) & ")")) 'Salto de linea
                Documento.Add(New Paragraph(" ")) 'Salto de linea

                Dim tabla93 As New PdfPTable(3)
                tabla93.SetWidthPercentage({150, 200, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
                tabla93.AddCell("Detalle")
                tabla93.AddCell("Imputación")
                tabla93.AddCell("Situación Fiscal")
                Documento.Add(tabla93) 'Salto de linea
                sql = "SELECT [sdc_Desc],SegCabC.sccpro_RazSoc FROM [SBDASIPT].[dbo].[SegCabC] inner join [SBDASIPT].dbo.[SegDetC] ON SegCabC.scc_ID=SegDetC.sdcscc_ID inner join [SBDASIPT].[dbo].[CausaEmi] on [CausaEmi].[cem_Cod]=SegCabC.scccem_Cod where [CausaEmi].cem_Desc=" & adjuntarAnio() & " order by SegCabC.sccpro_RazSoc"
                Dim tabla92 As New PdfPTable(3)
                tabla92.SetWidthPercentage({150, 200, 150}, PageSize.A4) 'Ajusta el tamaño de cada columna
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()
                        conn.Open()
                        cmd4.CommandText = sql
                        Dim reader As SqlDataReader = Nothing
                        reader = cmd4.ExecuteReader
                        Do While reader.Read()
                            'Dim Imp As String
                            'Imp = " IMPUTACION: " & reader(0)
                            tabla92.AddCell(reader(1))
                            tabla92.AddCell(reader(0))
                            tabla92.AddCell(" ")
                        Loop

                    End Using
                End Using
                Documento.Add(tabla92) 'Salto de linea


                Documento.Add(New Paragraph(" ")) 'Salto de linea



                Documento.Add(New Paragraph(" ")) 'Salto de linea
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                Documento.Add(New Paragraph("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL")) 'Salto de linea

                Documento.Close() 'Cierra el documento
                System.Diagnostics.Process.Start("Compra.pdf") 'Abre el archivo DEMO.PDF

            Else

            End If
        Else

        End If


    End Sub
    Public Sub imprimirOrdenCargoCheques()
        Dim Origen As String
        If chkNac.Checked Then
            Origen = "OPN"
        End If
        If chkProv.Checked Then
            Origen = "OPP"
        End If

        sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='ComprasPago'"

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
            ' MessageBox.Show("entra por historial")
            imprimirOrdenCargoChequesHistorial()
        Else

            Dim Nro As String
            sql = "Select Concepto from AAOrdenNro where Tipo='ComprasPago' and NroIngreso=" & adjuntarAnio()
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

            Dim Documento As New Document(PageSize.A4, 60, 5, 35, 5) 'Declaracion del documento
            Dim parrafo As New Paragraph ' Declaracion de un parrafo
            Dim tablademo As New PdfPTable(5) 'declara la tabla con 4 Columnas
            Dim subTotal3 As Double
            pdf.PdfWriter.GetInstance(Documento, New FileStream("CompraPago.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

            Documento.Open() 'Abre documento para su escritura

            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("PROVINCIA DE MISIONES" & vbCr & "SISTEMA PROVINCIAL" & vbCr & "DE TELEDUCACION Y DESARROLLO") 'Texto que se insertara
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

            Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

            imagendemo = iTextSharp.text.Image.GetInstance("descarga.jpg") 'Dirreccion a la imagen que se hace referencia
            imagendemo.SetAbsolutePosition(450, 750) 'Posicion en el eje cartesiano
            imagendemo.ScaleAbsoluteWidth(100) 'Ancho de la imagen
            imagendemo.ScaleAbsoluteHeight(100) 'Altura de la imagen
            Documento.Add(imagendemo) ' Agrega la imagen al documento

            Documento.Add(New Paragraph(" ")) 'Salto de linea
            sql = "Select tal_ActualNro from Talonar where tal_Cod='" & Origen & "'"
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

            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("ORDEN PAGO NRO:" & Nro & "  ")
            parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("FECHA:" & Date.Now) 'Texto que se insertara

            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente


            Documento.Add(New Paragraph(" ")) 'Salto de linea

            Dim tablademo5 As New PdfPTable(5) 'declara la tabla con 4 Columnas
            tablademo5.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT
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
            'subTotal3 = 0
            'For Each row3 As DataGridViewRow In DataGridView3.Rows
            '    subTotal3 = subTotal3 + row3.Cells(3).Value
            'Next
            Documento.Add(tablademo4) 'Agrega la tabla al documento
            Dim tabla9 As New PdfPTable(5)
            'tabla9.HorizontalAlignment = ALIGN_LEFT
            tabla9.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tabla9.AddCell("SUBTOTAL")
            tabla9.AddCell(" ")
            tabla9.AddCell(" ")
            tabla9.AddCell(" ")
            Dim miParrafo As String
            Dim miTotal As Double
            miTotal = Double.Parse(txtTotalGastos.Text)
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
            tablademo.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

            tablademo.AddCell(New Paragraph("F. EMISION        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo.AddCell(New Paragraph("BENEFICIARIO        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            tablademo.AddCell(New Paragraph("NRO CTA        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            tablademo.AddCell(New Paragraph("CH. Y/O AUT.        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            tablademo.AddCell(New Paragraph("IMPORTE        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

            Documento.Add(tablademo) 'Agrega la tabla al documento

            Dim tablademo2 As New PdfPTable(5) 'declara la tabla con 4 Columnas
            tablademo2.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

            For Each row1 As DataGridViewRow In DataGridView1.Rows
                For Each column As DataGridViewColumn In DataGridView1.Columns
                    If column.Index <= 5 Then
                        Dim paraParrrafo As String
                        If IsNumeric(row1.Cells(column.Index).Value) And column.Index = 4 Then

                            paraParrrafo = Format(row1.Cells(column.Index).Value, "###,##0.00")
                            Dim miCelda2 As New PdfPCell
                            parrafo.Add(paraParrrafo)
                            miCelda2.AddElement(parrafo)

                            miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                            tablademo2.AddCell(miCelda2)
                            parrafo.Clear()
                        Else
                            paraParrrafo = row1.Cells(column.Index).Value
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
            Dim tabla10 As New PdfPTable(5)
            'tabla9.HorizontalAlignment = ALIGN_LEFT
            tabla10.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tabla10.AddCell("SUBTOTAL")
            tabla10.AddCell(" ")
            tabla10.AddCell(" ")
            tabla10.AddCell(" ")
            Dim miParrafo2 As String
            Dim miTotal2 As Double
            miTotal2 = Double.Parse(txtTotalFinal.Text)
            miParrafo2 = Format(miTotal2, "###,##0.00")
            Dim miCelda3 As New PdfPCell
            parrafo.Add(miParrafo2)
            miCelda3.AddElement(parrafo)

            miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
            tabla10.AddCell(miCelda3)
            Documento.Add(tabla10)
            parrafo.Clear()
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            If GroupBox7.Visible = True Then

                Documento.Add(New Paragraph("DATOS DE RETENCIONES: ")) 'Salto de linea
                Documento.Add(New Paragraph(" ")) 'Salto de linea

                Dim tablademo8 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                tablademo8.SetWidthPercentage({100, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                tablademo8.AddCell(New Paragraph("NUMERO", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                tablademo8.AddCell(New Paragraph("ARTICULO", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                tablademo8.AddCell(New Paragraph("CODIGO", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                tablademo8.AddCell(New Paragraph("IMPORTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                Documento.Add(tablademo8) 'Agrega la tabla al documento
                Dim tablademo9 As New PdfPTable(4) 'declara la tabla con 4 Columnas
                tablademo9.SetWidthPercentage({100, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                For Each row As DataGridViewRow In DataGridView3.Rows
                    For Each column As DataGridViewColumn In DataGridView3.Columns
                        If column.Index <= 4 Then
                            Dim paraParrrafo As String
                            If IsNumeric(row.Cells(column.Index).Value) And column.Index = 3 Then

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
            Documento.Add(New Paragraph(" ")) 'Salto de linea

            Dim GranTotal As Double
            Dim subTotal As Double
            Dim subTotal2 As Double

            subTotal = Double.Parse(txtTotalFinal.Text)
            subTotal2 = Double.Parse(txtTotalGastos.Text)
            GranTotal = subTotal + subTotal2
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
            'sql = "update Talonar set tal_ActualNro='" & nroCargo & "' where tal_Cod='" & Origen & "'"
            'Using conn As New SqlConnection(CONNSTR)
            '    Using cmd4 As SqlCommand = conn.CreateCommand()

            '        conn.Open()

            '        cmd4.CommandText = sql
            '        cmd4.ExecuteScalar()

            '    End Using
            'End Using

            Documento.Close() 'Cierra el documento
            System.Diagnostics.Process.Start("CompraPago.pdf") 'Abre el archivo DEMO.PDF
            'almacenarPago(nroCargo)
        End If
    End Sub
    Public Sub imprimirActaHistorial(ByRef NroActa As String)
        Dim Documento As New Document(PageSize.A4, 60, 5, 35, 5) 'Declaracion del documento
        Dim parrafo As New Paragraph ' Declaracion de un parrafo

        pdf.PdfWriter.GetInstance(Documento, New FileStream("ActaCompra.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

        Documento.Open() 'Abre documento para su escritura
        For Each row As DataGridViewRow In DataGridView2.Rows
            Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

            imagendemo = iTextSharp.text.Image.GetInstance("MembreteActa.png") 'Dirreccion a la imagen que se hace referencia
            imagendemo.SetAbsolutePosition(5, 750) 'Posicion en el eje cartesiano
            imagendemo.ScaleAbsoluteWidth(550) 'Ancho de la imagen
            imagendemo.ScaleAbsoluteHeight(95) 'Altura de la imagen
            Documento.Add(imagendemo) ' Agrega la imagen al documento
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("Posadas:" & Date.Now) 'Texto que se insertara
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

            sql = "Select Concepto from AAOrdenNro where NroIngreso=" & adjuntarAnio() & " and Tipo='ComprasHistorial' and Fecha='" & dtFechaIngreso.Text & "' and Concepto='" & NroActa & "'"
            Dim nroCargo As String

            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()

                    conn.Open()

                    cmd4.CommandText = sql

                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    reader.Read()
                    nroCargo = reader.Item(0)
                End Using
            End Using

            Format(nroCargo, "00000000")
            Dim fecha As String
            fecha = dtFechaIngreso.Text
            parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 14, ALIGN_JUSTIFIED) 'Asigan fuente
            parrafo.Add("Acta de Recepción Nro:" & nroCargo & "/" & Date.Now.Year) 'Texto que se insertara
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("Expediente Nro:") 'Texto que se insertara
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("Resolucion Nro:") 'Texto que se insertara
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("Orden Pago Nro:") 'Texto que se insertara
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("Proveedor :" & row.Cells(2).Value) 'Texto que se insertara
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("Direccion :" & dameDomicilio(row.Cells(2).Value.ToString.Substring(0, 11))) 'Texto que se insertara
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("En la ciudad de Posadas, Capital de la provinica de Misiones en el asiento del Sistema Provincial de " & "Teleducación y Desarrollo (SIPTeD), Departamento de Patrimonio, a los  " & Numalet.ToCardinal(Integer.Parse(fecha.Substring(0, 2))) & " días del mes de " & dameMes(fecha.Substring(3, 2)) & " del año " & Numalet.ToCardinal(Integer.Parse(fecha.Substring(6, 4))) & "  se hace constar por la presente que se han recepcionado del proveedor arriba indicado, según " & "factura/s " & DataGridView2.Rows(row.Index).Cells(1).Value & " los bienes/servicios que se detallan a continuación, los que habiendo sido" & " examinados, han sido encontrados de conformidad tanto en calidad como en cantidad:") 'Texto que se insertara
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Dim tablademo As New PdfPTable(7) 'declara la tabla con 4 Columnas
            tablademo.SetWidthPercentage({70, 70, 140, 90, 70, 70, 70}, PageSize.A4)
            tablademo.HorizontalAlignment = Element.ALIGN_JUSTIFIED
            tablademo.AddCell(New Paragraph("Renglon       ", FontFactory.GetFont("Arial", 9)))
            tablademo.AddCell(New Paragraph("Cant", FontFactory.GetFont("Arial", 9)))
            tablademo.AddCell(New Paragraph("Descripcion", FontFactory.GetFont("Arial", 9)))
            tablademo.AddCell(New Paragraph("Codigo", FontFactory.GetFont("Arial", 9)))
            tablademo.AddCell(New Paragraph("Nro Inventario", FontFactory.GetFont("Arial", 9)))
            tablademo.AddCell(New Paragraph("Pr Unit", FontFactory.GetFont("Arial", 9)))
            tablademo.AddCell(New Paragraph("Pr Total", FontFactory.GetFont("Arial", 9)))

            Documento.Add(tablademo) 'Agrega la tabla al documento

            Dim BANDERITA As Boolean = False

            sql = "SELECT [sdc_NReng],[sdc_CantUM1],[sdc_Desc],[sdcart_CodGen],[DtsArticulos].Dart_2,[sdc_PrecioUn],[sdc_ImpTot],[DtsArticulos].Dart_1  FROM [SBDASIPT].[dbo].[SegDetC] " &
                  "inner join [SBDASIPT].[dbo].[SegCabC] on [SBDASIPT].[dbo].[SegCabC].[scc_ID]=[SBDASIPT].[dbo].[SegDetC].[sdcscc_ID] " &
                  "INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON SegCabC.[scccem_Cod]=CausaEmi.cem_Cod " &
                  "inner join [SBDASIPT].[dbo].[Articulos] on [SegDetC].sdcart_CodGen=Articulos.art_CodGen " &
                  "inner Join [SBDASIPT].[dbo].[DtsArticulos] on Articulos.art_CodGen=DtsArticulos.art_CodGen " &
                  "where CausaEmi.cem_Desc='" & adjuntarAnio() & "'"
            Dim tablademo2 As New PdfPTable(7) 'declara la tabla con 4 Columnas
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql
                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    'IsDBNull(reader.Read()) Or
                    If reader.Read().Equals(Nothing) Then
                        BANDERITA = True
                    Else

                        BANDERITA = False

                    End If
                End Using
            End Using

            If BANDERITA = True Then
                sql = "SELECT [sdc_NReng],[sdc_CantUM1],[sdc_Desc],[sdcart_CodGen],[sdc_PrecioUn],[sdc_ImpTot]  FROM [SBDASIPT].[dbo].[SegDetC] " &
                                     "inner join [SBDASIPT].[dbo].[SegCabC] on [SBDASIPT].[dbo].[SegCabC].[scc_ID]=[SBDASIPT].[dbo].[SegDetC].[sdcscc_ID] " &
                                     "INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON SegCabC.[scccem_Cod]=CausaEmi.cem_Cod " &
                                     "where CausaEmi.cem_Desc='" & adjuntarAnio() & "'"

                tablademo2.SetWidthPercentage({70, 70, 140, 90, 70, 70, 70}, PageSize.A4)
                tablademo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()
                        conn.Open()
                        cmd4.CommandText = sql
                        Dim reader As SqlDataReader = Nothing
                        reader = cmd4.ExecuteReader

                        While reader.Read()
                            tablademo2.AddCell(New Paragraph(reader(0), FontFactory.GetFont("Arial", 9)))
                            tablademo2.AddCell(New Paragraph(reader(1), FontFactory.GetFont("Arial", 9)))
                            tablademo2.AddCell(New Paragraph(reader(2), FontFactory.GetFont("Arial", 9)))
                            tablademo2.AddCell(New Paragraph("0", FontFactory.GetFont("Arial", 9)))
                            tablademo2.AddCell(New Paragraph("0", FontFactory.GetFont("Arial", 9)))
                            Dim paraParrrafo As String

                            paraParrrafo = Format(reader(4), "###,##0.00")

                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 9, ALIGN_RIGHT) 'Asigan fuente
                            parrafo.Add(paraParrrafo) 'Texto que se insertara
                            Dim cell As New PdfPCell
                            cell.HorizontalAlignment = 1
                            cell.AddElement(parrafo)


                            tablademo2.AddCell(cell) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            parrafo.Clear()

                            paraParrrafo = Format(reader(5), "###,##0.00")

                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 9, ALIGN_RIGHT) 'Asigan fuente
                            parrafo.Add(paraParrrafo) 'Texto que se insertara
                            tablademo2.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            parrafo.Clear()
                        End While
                    End Using
                End Using
            Else
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()
                        conn.Open()
                        cmd4.CommandText = sql
                        Dim reader As SqlDataReader = Nothing
                        reader = cmd4.ExecuteReader

                        tablademo2.SetWidthPercentage({70, 70, 140, 90, 70, 70, 70}, PageSize.A4)
                        tablademo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                        Do While reader.Read()
                            tablademo2.AddCell(New Paragraph(reader(0), FontFactory.GetFont("Arial", 9)))
                            tablademo2.AddCell(New Paragraph(reader(1), FontFactory.GetFont("Arial", 9)))
                            tablademo2.AddCell(New Paragraph(reader(2), FontFactory.GetFont("Arial", 9)))
                            tablademo2.AddCell(New Paragraph(reader(7), FontFactory.GetFont("Arial", 9)))
                            tablademo2.AddCell(New Paragraph(reader(4), FontFactory.GetFont("Arial", 9)))
                            Dim paraParrrafo As String

                            paraParrrafo = Format(reader(5), "###,##0.00")

                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 9, ALIGN_RIGHT) 'Asigan fuente
                            parrafo.Add(paraParrrafo) 'Texto que se insertara
                            tablademo2.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            parrafo.Clear()

                            paraParrrafo = Format(reader(6), "###,##0.00")

                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 9, ALIGN_RIGHT) 'Asigan fuente
                            parrafo.Add(paraParrrafo) 'Texto que se insertara
                            tablademo2.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            parrafo.Clear()
                        Loop

                    End Using
                End Using
            End If

            Documento.Add(tablademo2) 'Agrega la tabla al documento
            Documento.Add(New Paragraph(" ")) 'Salto de linea

            Dim banMonto As Boolean
            sql = "SELECT sum([sdc_ImpTot])  FROM [SBDASIPT].[dbo].[SegDetC] " &
                "inner join [SBDASIPT].[dbo].[SegCabC] on [SBDASIPT].[dbo].[SegCabC].[scc_ID]=[SBDASIPT].[dbo].[SegDetC].[sdcscc_ID] " &
                "INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON SegCabC.[scccem_Cod]=CausaEmi.cem_Cod " &
                "inner join [SBDASIPT].[dbo].[Articulos] on [SegDetC].sdcart_CodGen=Articulos.art_CodGen " &
                "inner Join [SBDASIPT].[dbo].[DtsArticulos] on Articulos.art_CodGen=DtsArticulos.art_CodGen " &
                "where CausaEmi.cem_Desc='" & adjuntarAnio() & "'"
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql
                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    If reader.Read() And Not IsDBNull(reader(0)) Then
                        parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                        parrafo.Add("Son:$" & reader(0) & "    Pesos:" & Numalet.ToCardinal(reader(0).ToString))


                    Else
                        banMonto = True
                    End If
                End Using
            End Using
            If banMonto = True Then
                sql = "SELECT sum([sdc_ImpTot])  FROM [SBDASIPT].[dbo].[SegDetC] " &
                "inner join [SBDASIPT].[dbo].[SegCabC] on [SBDASIPT].[dbo].[SegCabC].[scc_ID]=[SBDASIPT].[dbo].[SegDetC].[sdcscc_ID] " &
                "INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON SegCabC.[scccem_Cod]=CausaEmi.cem_Cod " &
                "where CausaEmi.cem_Desc='" & adjuntarAnio() & "'"
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()
                        conn.Open()
                        cmd4.CommandText = sql
                        Dim reader As SqlDataReader = Nothing
                        reader = cmd4.ExecuteReader
                        If reader.Read() Then
                            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                            parrafo.Add("Son:$" & reader(0) & "    Pesos:" & reader(0)) 'Numalet.ToCardinal(reader(0)))


                        End If
                    End Using
                End Using
            End If

            Documento.Add(parrafo) 'Agrega el parrafo al documento
            Documento.NewPage()
        Next

        Documento.Close() 'Cierra el documento
        System.Diagnostics.Process.Start("ActaCompra.pdf") 'Abre el archivo DEMO.PDF

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
                    cmd.Parameters.Add("@Tipo", SqlDbType.VarChar, 50).Value = "ComprasPago"
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
    Public Sub imprimirActa()

        Dim fecha As String
        fecha = dtFechaIngreso.Text
        Dim nroCargo As String
        Dim nro As String
        nroCargo = txtSaliente.Text
        Try
            Format(nroCargo, "00000000")
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
            sql = "update Talonar set tal_ActualNro='" & nro & "' where tal_Cod='ACT'"
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


        If almacenarActa(nroCargo, fecha) Then
            Try

                Dim Documento As New Document(PageSize.A4, 60, 5, 35, 5) 'Declaracion del documento
                Dim parrafo As New Paragraph ' Declaracion de un parrafo

                pdf.PdfWriter.GetInstance(Documento, New FileStream("ActaCompra.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

                Documento.Open() 'Abre documento para su escritura
                For Each row As DataGridViewRow In DataGridView2.Rows
                    Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

                    imagendemo = iTextSharp.text.Image.GetInstance("MembreteActa.png") 'Dirreccion a la imagen que se hace referencia
                    imagendemo.SetAbsolutePosition(5, 750) 'Posicion en el eje cartesiano
                    imagendemo.ScaleAbsoluteWidth(550) 'Ancho de la imagen
                    imagendemo.ScaleAbsoluteHeight(95) 'Altura de la imagen
                    Documento.Add(imagendemo) ' Agrega la imagen al documento
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("Posadas:" & Date.Now) 'Texto que se insertara
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente


                    parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 14, ALIGN_JUSTIFIED) 'Asigan fuente
                    parrafo.Add("Acta de Recepción Nro:" & nroCargo & "/" & Date.Now.Year) 'Texto que se insertara
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("Expediente Nro:") 'Texto que se insertara
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("Resolucion Nro:") 'Texto que se insertara
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("Orden Pago Nro:") 'Texto que se insertara
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("Proveedor :" & row.Cells(2).Value) 'Texto que se insertara
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("Direccion :" & dameDomicilio(row.Cells(2).Value.ToString.Substring(0, 11))) 'Texto que se insertara
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("En la ciudad de Posadas, Capital de la provinica de Misiones en el asiento del Sistema Provincial de " & "Teleducación y Desarrollo (SIPTeD), Departamento de Patrimonio, a los  " & Numalet.ToCardinal(Integer.Parse(fecha.Substring(0, 2))) & " días del mes de " & dameMes(fecha.Substring(3, 2)) & " del año " & Numalet.ToCardinal(Integer.Parse(fecha.Substring(6, 4))) & "  se hace constar por la presente que se han recepcionado del proveedor arriba indicado, según " & "factura/s " & DataGridView2.Rows(row.Index).Cells(1).Value & " los bienes/servicios que se detallan a continuación, los que habiendo sido" & " examinados, han sido encontrados de conformidad tanto en calidad como en cantidad:") 'Texto que se insertara
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Dim tablademo As New PdfPTable(7) 'declara la tabla con 4 Columnas
                    tablademo.SetWidthPercentage({70, 70, 140, 90, 70, 70, 70}, PageSize.A4)
                    tablademo.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                    tablademo.AddCell(New Paragraph("Renglon       ", FontFactory.GetFont("Arial", 12)))
                    tablademo.AddCell(New Paragraph("Cant", FontFactory.GetFont("Arial", 12)))
                    tablademo.AddCell(New Paragraph("Descripcion", FontFactory.GetFont("Arial", 12)))
                    tablademo.AddCell(New Paragraph("Codigo", FontFactory.GetFont("Arial", 12)))
                    tablademo.AddCell(New Paragraph("Nro Inventario", FontFactory.GetFont("Arial", 12)))
                    tablademo.AddCell(New Paragraph("Pr Unit", FontFactory.GetFont("Arial", 12)))
                    tablademo.AddCell(New Paragraph("Pr Total", FontFactory.GetFont("Arial", 12)))

                    Documento.Add(tablademo) 'Agrega la tabla al documento
                    Dim BANDERITA As Boolean = False

                    sql = "SELECT [sdc_NReng],[sdc_CantUM1],[sdc_Desc],[sdcart_CodGen],[DtsArticulos].Dart_2,[sdc_PrecioUn],[sdc_ImpTot],[DtsArticulos].Dart_1  FROM [SBDASIPT].[dbo].[SegDetC] " &
                          "inner join [SBDASIPT].[dbo].[SegCabC] on [SBDASIPT].[dbo].[SegCabC].[scc_ID]=[SBDASIPT].[dbo].[SegDetC].[sdcscc_ID] " &
                          "INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON SegCabC.[scccem_Cod]=CausaEmi.cem_Cod " &
                          "inner join [SBDASIPT].[dbo].[Articulos] on [SegDetC].sdcart_CodGen=Articulos.art_CodGen " &
                          "inner Join [SBDASIPT].[dbo].[DtsArticulos] on Articulos.art_CodGen=DtsArticulos.art_CodGen " &
                          "where CausaEmi.cem_Desc='" & adjuntarAnio() & "'"
                    Dim tablademo2 As New PdfPTable(7) 'declara la tabla con 4 Columnas
                    Using conn As New SqlConnection(CONNSTR)
                        Using cmd4 As SqlCommand = conn.CreateCommand()
                            conn.Open()
                            cmd4.CommandText = sql
                            Dim reader As SqlDataReader = Nothing
                            reader = cmd4.ExecuteReader
                            If IsDBNull(reader.Read) Or reader.Read().Equals(Nothing) Then
                                BANDERITA = True
                            Else
                                BANDERITA = False
                            End If
                        End Using
                    End Using

                    If BANDERITA = True Then
                        sql = "SELECT [sdc_NReng],[sdc_CantUM1],[sdc_Desc],[sdcart_CodGen],[sdc_PrecioUn],[sdc_ImpTot]  FROM [SBDASIPT].[dbo].[SegDetC] " &
                                             "inner join [SBDASIPT].[dbo].[SegCabC] on [SBDASIPT].[dbo].[SegCabC].[scc_ID]=[SBDASIPT].[dbo].[SegDetC].[sdcscc_ID] " &
                                             "INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON SegCabC.[scccem_Cod]=CausaEmi.cem_Cod " &
                                             "where CausaEmi.cem_Desc='" & adjuntarAnio() & "'"

                        tablademo2.SetWidthPercentage({70, 70, 140, 90, 70, 70, 70}, PageSize.A4)
                        tablademo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                        Using conn As New SqlConnection(CONNSTR)
                            Using cmd4 As SqlCommand = conn.CreateCommand()
                                conn.Open()
                                cmd4.CommandText = sql
                                Dim reader As SqlDataReader = Nothing
                                reader = cmd4.ExecuteReader

                                While reader.Read()
                                    tablademo2.AddCell(New Paragraph(reader(0), FontFactory.GetFont("Arial", 12)))
                                    tablademo2.AddCell(New Paragraph(reader(1), FontFactory.GetFont("Arial", 12)))
                                    tablademo2.AddCell(New Paragraph(reader(2), FontFactory.GetFont("Arial", 9)))
                                    tablademo2.AddCell(New Paragraph("0", FontFactory.GetFont("Arial", 12)))
                                    tablademo2.AddCell(New Paragraph("0", FontFactory.GetFont("Arial", 12)))
                                    Dim paraParrrafo As String

                                    paraParrrafo = Format(reader(4), "###,##0.00")

                                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                    parrafo.Add(paraParrrafo) 'Texto que se insertara
                                    tablademo2.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()

                                    paraParrrafo = Format(reader(5), "###,##0.00")

                                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                    parrafo.Add(paraParrrafo) 'Texto que se insertara
                                    tablademo2.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()

                                End While
                            End Using
                        End Using
                    Else
                        Using conn As New SqlConnection(CONNSTR)
                            Using cmd4 As SqlCommand = conn.CreateCommand()
                                conn.Open()
                                cmd4.CommandText = sql
                                Dim reader As SqlDataReader = Nothing
                                reader = cmd4.ExecuteReader
                                If IsDBNull(reader.Read) Or reader(0).Equals(Nothing) Then
                                    BANDERITA = True
                                Else
                                    tablademo2.SetWidthPercentage({70, 70, 140, 90, 70, 70, 70}, PageSize.A4)
                                    tablademo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                                    Do While reader.Read()
                                        tablademo2.AddCell(New Paragraph(reader(0), FontFactory.GetFont("Arial", 12)))
                                        tablademo2.AddCell(New Paragraph(reader(1), FontFactory.GetFont("Arial", 12)))
                                        tablademo2.AddCell(New Paragraph(reader(2), FontFactory.GetFont("Arial", 9)))
                                        tablademo2.AddCell(New Paragraph(reader(7), FontFactory.GetFont("Arial", 12)))
                                        tablademo2.AddCell(New Paragraph(reader(4), FontFactory.GetFont("Arial", 12)))
                                        Dim paraParrrafo As String

                                        paraParrrafo = Format(reader(5), "###,##0.00")

                                        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                        parrafo.Add(paraParrrafo) 'Texto que se insertara
                                        tablademo2.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        parrafo.Clear()

                                        paraParrrafo = Format(reader(6), "###,##0.00")

                                        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                        parrafo.Add(paraParrrafo) 'Texto que se insertara
                                        tablademo2.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        parrafo.Clear()

                                    Loop
                                End If
                            End Using
                        End Using
                    End If

                    Documento.Add(tablademo2) 'Agrega la tabla al documento

                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Dim banMonto As Boolean
                    sql = "SELECT sum([sdc_ImpTot])  FROM [SBDASIPT].[dbo].[SegDetC] " &
                        "inner join [SBDASIPT].[dbo].[SegCabC] on [SBDASIPT].[dbo].[SegCabC].[scc_ID]=[SBDASIPT].[dbo].[SegDetC].[sdcscc_ID] " &
                        "INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON SegCabC.[scccem_Cod]=CausaEmi.cem_Cod " &
                        "inner join [SBDASIPT].[dbo].[Articulos] on [SegDetC].sdcart_CodGen=Articulos.art_CodGen " &
                        "inner Join [SBDASIPT].[dbo].[DtsArticulos] on Articulos.art_CodGen=DtsArticulos.art_CodGen " &
                        "where CausaEmi.cem_Desc='" & adjuntarAnio() & "'"
                    Using conn As New SqlConnection(CONNSTR)
                        Using cmd4 As SqlCommand = conn.CreateCommand()
                            conn.Open()
                            cmd4.CommandText = sql
                            Dim reader As SqlDataReader = Nothing
                            reader = cmd4.ExecuteReader
                            reader.Read()
                            If Not IsDBNull(reader(0)) Then
                                parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                                parrafo.Add("Son:$" & reader(0) & "    Pesos:" & Numalet.ToCardinal(reader(0)))


                            Else
                                banMonto = True
                            End If
                        End Using
                    End Using
                    If banMonto = True Then
                        sql = "SELECT sum([sdc_ImpTot])  FROM [SBDASIPT].[dbo].[SegDetC] " &
                        "inner join [SBDASIPT].[dbo].[SegCabC] on [SBDASIPT].[dbo].[SegCabC].[scc_ID]=[SBDASIPT].[dbo].[SegDetC].[sdcscc_ID] " &
                        "INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON SegCabC.[scccem_Cod]=CausaEmi.cem_Cod " &
                        "where CausaEmi.cem_Desc='" & adjuntarAnio() & "'"
                        Using conn As New SqlConnection(CONNSTR)
                            Using cmd4 As SqlCommand = conn.CreateCommand()
                                conn.Open()
                                cmd4.CommandText = sql
                                Dim reader As SqlDataReader = Nothing
                                reader = cmd4.ExecuteReader
                                If reader.Read() Then
                                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                                    parrafo.Add("Son:$" & reader(0) & "    Pesos:" & Numalet.ToCardinal(reader(0)))

                                End If
                            End Using
                        End Using
                    End If
                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    Documento.NewPage()

                Next

                Documento.Close() 'Cierra el documento
                System.Diagnostics.Process.Start("ActaCompra.pdf") 'Abre el archivo DEMO.PDF
            Catch ex As Exception
                MessageBox.Show("Compruebe que el documento ya no se encuentre abierto", "AVISO")
            End Try
        Else
        End If
    End Sub
End Class