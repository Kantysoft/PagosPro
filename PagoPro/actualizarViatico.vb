Imports System.Data.SqlClient
Imports System.Data.Odbc
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.Image
Imports iTextSharp.text.pdf.VerticalText
Imports System.IO
Imports NuGet

Public Class actualizarViatico
    #Region "Variables"

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
    Dim para As String
#End Region

#Region "Cargas"

    Public Sub cargarCombo()
        sql = "select Descripcion from AASituaciones where Descripcion like'%Pago%' or Descripcion like'%Rendicion%' or Descripcion like '%Viatico%' or Descripcion like '%Historial Via%' or Descripcion like '%Historial Pa%' or Descripcion like '%Historial Ren%'"

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
        'cmbTipoOrden.Items.Add("Rendicion FP")
    End Sub

    Public Sub cargarDataGrid()
        sql = " SELECT distinct  ItemComp.ico_Desc FROM CabCompra  INNER JOIN CausaEmi ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod INNER JOIN  ItemComp ON CabCompra.cco_ID=ItemComp.icocco_ID  WHERE  CausaEmi.cem_Desc='" & adjuntarAnio() & "' and [ccotco_Cod]<>'RVI' ORDER BY ItemComp.ico_Desc"

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                DataGridView1.Columns.Add("Nombre", "NOMBRE")
                DataGridView1.Columns.Add("Dias", "DIAS")
                Dim cantidadP As Integer = 0

                Dim anterior As String = ""
                Do While reader.Read()
                    'If (cantidadP > 0) Then
                    anterior = reader(0)
                    DataGridView1.Columns.Add(reader(0), reader(0))
                    'indice = DataGridView1.Columns("viatico" & count).Index.ToString()

                    'End If
                    cantidadP = cantidadP + 1
                Loop
                DataGridView1.Columns.Add("Devengado", "MONTO")
            End Using
        End Using

        cargarNombres()
        calculoTotalViatico()
        calculoTotalDias()
        calculoTotalCombustible()
        calculoTotalPeajes()
        calculoTotalViaticoMisiones()
        calculoTotalGastos()
    End Sub
    'carga el recudro concepto con todos los conceptos de la tabla origenes
    Public Sub cargarConceptos()
        'consulta sql en la que busca el atribuo ori_Cod, ori_Desc from Origenes
        sql = " SELECT ori_Cod, ori_Desc from Origenes"
        dataConceptos.Rows.Clear()

        'se conecta con la base de datos y ejecuta la consutla sql
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                'al datagrid dataConceptos se le agrega las columnas CODIGO, DESCRIPCION
                dataConceptos.Columns.Add("codigo", "CODIGO")
                dataConceptos.Columns.Add("descripcion", "DESCRIPCION")
                'recorre el vector de resultado de la consulta agregando un codigo y descripcion
                'el datagrid 
                Do While reader.Read()
                    Dim fila(2) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    dataConceptos.Rows.Add(fila)
                Loop

            End Using
        End Using
    End Sub

    Public Sub cargarConceptosPagos()
        'busca el atributo ori_Cod, ori_Desc de la tabla origenes
        sql = " SELECT ori_Cod, ori_Desc from Origenes"
        dataConceptosPago.Rows.Clear()
        'se conecta con la base de datos y ejecuta la consulta sql
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                'agrega las columnas Codigo y descripcion
                dataConceptosPago.Columns.Add("codigo", "CODIGO")
                dataConceptosPago.Columns.Add("descripcion", "DESCRIPCION")
                'recorre el vector de resultado si fueran mas de uno
                Do While reader.Read()
                    'agrega los dos atributos mensionados por cada elementos del vector resultado
                    Dim fila(2) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    dataConceptosPago.Rows.Add(fila)
                Loop

            End Using
        End Using
    End Sub

    Public Sub cargarBancos()
        'consulta sql en la que trae todos los atributos ctbbco_Cod, de la tabla CtaBan que contiene todas las cuentas
        'bancarias.
        sql = " SELECT ctbbco_Cod, ctb_Cod,ctb_Desc from CtaBan"
        dataBancos.Rows.Clear()
        'conecta con la base de datos  y ejecuto la consulta sql, 
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                'agrega las tablas Codigo,nro cta, descripcion
                dataBancos.Columns.Add("codigo", "CODIGO")
                dataBancos.Columns.Add("nroCta", "NRO CTA")
                dataBancos.Columns.Add("descripcion", "DESCRIPCION")
                'recorre el resultado de la consulta y agrega los atributos asignados en la consulta
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

    Public Sub cargarBancosPagos()
        'consulta sql que trae el codigo y descripcion de la tabla CtaBan
        sql = " SELECT ctbbco_Cod, ctb_Cod,ctb_Desc from CtaBan"
        dataBancosPago.Rows.Clear()

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader

                dataBancosPago.Columns.Add("codigo", "CODIGO")
                dataBancosPago.Columns.Add("nroCta", "NRO CTA")
                dataBancosPago.Columns.Add("descripcion", "DESCRIPCION")

                Do While reader.Read()
                    Dim fila(3) As Object
                    fila(0) = reader(0)
                    fila(1) = reader(1)
                    fila(2) = reader(2)

                    dataBancosPago.Rows.Add(fila)
                Loop

            End Using
        End Using
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

    Public Sub cargarParaPago(posicionFila As Integer)
        Dim hora As String = Now.ToString("HH:mm:ss")
        Dim I As Integer = dataConceptos.CurrentCell.RowIndex
        Dim X As Integer = dataBancos.CurrentCell.RowIndex

        'Try
        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()

                conn.Open()
                cmd.CommandText = "insert into AASobrantes(" +
                                                "[nroOrden]" +
                                                ",[sobrante]" +
                                                ",[fechaMod]" +
                                                ",[nombre]" +
                                                ",[total]" +
                                                ",[donacion]" +
                                                ") VALUES (" +
                                                "@nroOrden" +
                                                ",@sobrante" +
                                                ",@fechaMod" +
                                                ",@nombre" +
                                                ",@total" +
                                                ",@donacion" +
                                                ")"

                cmd.Parameters.Add("@nroOrden", SqlDbType.Int).Value = adjuntarAnio()
                cmd.Parameters.Add("@sobrante", SqlDbType.Float).Value = DataGridView1.Rows(posicionFila).Cells("A pagar").Value
                cmd.Parameters.Add("@fechaMod", SqlDbType.DateTime).Value = Date.Now
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 50).Value = DataGridView1.Rows(posicionFila).Cells("nombre").Value
                cmd.Parameters.Add("@total", SqlDbType.Float).Value = DataGridView1.Rows(posicionFila).Cells("DEVENGADO").Value
                Select Case MsgBox("¿El viaticado va a donar la diferencia?", MsgBoxStyle.YesNo, "DONACION??")
                    Case MsgBoxResult.Yes
                        cmd.Parameters.Add("@donacion", SqlDbType.VarChar, 2).Value = "SI"
                    Case MsgBoxResult.No
                        cmd.Parameters.Add("@donacion", SqlDbType.VarChar, 2).Value = "NO"
                End Select
                cmd.ExecuteScalar()
            End Using
        End Using
        'Catch ex As Exception

        'End Try

    End Sub

    Public Sub cargarSinSobrantes(posicionFila As Integer)
        Dim hora As String = Now.ToString("HH:mm:ss")
        Dim I As Integer = dataConceptos.CurrentCell.RowIndex
        Dim X As Integer = dataBancos.CurrentCell.RowIndex
        'Try
        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()

                conn.Open()
                cmd.CommandText = "insert into AASobrantes(" +
                                                "[nroOrden]" +
                                                ",[sobrante]" +
                                                ",[fechaMod]" +
                                                ",[nombre]" +
                                                ",[total]" +
                                                ",[donacion]" +
                                                ") VALUES (" +
                                                "@nroOrden" +
                                                ",@sobrante" +
                                                ",@fechaMod" +
                                                ",@nombre" +
                                                ",@total" +
                                                ",@donacion" +
                                                ")"

                cmd.Parameters.Add("@nroOrden", SqlDbType.Int).Value = adjuntarAnio()
                cmd.Parameters.Add("@sobrante", SqlDbType.Float).Value = 0
                cmd.Parameters.Add("@fechaMod", SqlDbType.DateTime).Value = Date.Now
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 50).Value = DataGridView1.Rows(posicionFila).Cells("nombre").Value
                cmd.Parameters.Add("@total", SqlDbType.Float).Value = DataGridView1.Rows(posicionFila).Cells("DEVENGADO").Value
                Select Case MsgBox("¿El viaticado va a donar la diferencia?", MsgBoxStyle.YesNo, "DONACION??")
                    Case MsgBoxResult.Yes
                        cmd.Parameters.Add("@donacion", SqlDbType.VarChar, 2).Value = "SI"
                    Case MsgBoxResult.No
                        cmd.Parameters.Add("@donacion", SqlDbType.VarChar, 2).Value = "NO"

                        'Select Case MsgBox("A seleccionado el modo ajuste por redondeo", MsgBoxStyle.YesNo, "REDONDEO??")
                        'Case MsgBoxResult.Yes
                        '   cmd.Parameters.Add("@donacion", SqlDbType.VarChar, 2).Value = "Y"
                        'Case MsgBoxResult.No
                        'End Select
                End Select
                cmd.ExecuteScalar()
            End Using
        End Using
        'Catch ex As Exception

        'End Try
    End Sub

    Private Sub cargarReintegro(posicionFila As Integer)
        Dim hora As String = Now.ToString("HH:mm:ss")
        Dim I As Integer = dataConceptos.CurrentCell.RowIndex
        Dim X As Integer = dataBancos.CurrentCell.RowIndex
        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()

                conn.Open()
                cmd.CommandText = "insert into CabMovF(" +
                                                "[cmfemp_Codigo]" +
                                                ",[cmfsuc_Cod]" +
                                                ",[cmf_FMov]" +
                                                ",[cmf_HMov]" +
                                                ",[cmfptr_Cod]" +
                                                ",[cmftmo_Cod]" +
                                                ",[cmf_MarcaRE]" +
                                                ",[cmf_Desc]" +
                                                ",[cmf_PasadoCG]" +
                                                ",[cmf_MarcaDepu]" +
                                                ",[cmf_CompAsoc]" +
                                                ",[cmf_CodApe]" +
                                                ",[cmf_FecMod]" +
                                                ",[cmfusu_Codigo]" +
                                                ",[cmf_Convert]" +
                                                ") VALUES (" +
                                                "@cmfemp_Codigo" +
                                               ",@cmfsuc_Cod" +
                                               ",@cmf_FMov" +
                                               ",@cmf_HMov" +
                                               ",@cmfptr_Cod" +
                                               ",@cmftmo_Cod" +
                                               ",@cmf_MarcaRE" +
                                               ",@cmf_Desc" +
                                               ",@cmf_PasadoCG" +
                                               ",@cmf_MarcaDepu" +
                                               ",@cmf_CompAsoc" +
                                               ",@cmf_CodApe" +
                                               ",@cmf_FecMod" +
                                               ",@cmfusu_Codigo" +
                                               ",@cmf_Convert" +
                                                ")"

                cmd.Parameters.Add("@cmfemp_Codigo", SqlDbType.VarChar, 4).Value = "SIPT"
                cmd.Parameters.Add("@cmfsuc_Cod", SqlDbType.VarChar, 4).Value = ""
                'cmd.Parameters.Add("@cmf_ID", SqlDbType.Int).Value = dameNroFondo()
                cmd.Parameters.Add("@cmf_FMov", SqlDbType.DateTime).Value = Date.Now
                cmd.Parameters.Add("@cmf_HMov", SqlDbType.DateTime).Value = hora
                cmd.Parameters.Add("@cmfptr_Cod", SqlDbType.VarChar, 4).Value = "001"
                cmd.Parameters.Add("@cmftmo_Cod", SqlDbType.VarChar, 6).Value = "REIVIA"
                cmd.Parameters.Add("@cmf_MarcaRE", SqlDbType.VarChar, 1).Value = "R"
                cmd.Parameters.Add("@cmf_Desc", SqlDbType.VarChar, 30).Value = "RV " & txtCodigo.Text & " " & DataGridView1.Rows(posicionFila).Cells("nombre").Value
                cmd.Parameters.Add("@cmf_PasadoCG", SqlDbType.VarChar, 1).Value = "C"
                cmd.Parameters.Add("@cmf_MarcaDepu", SqlDbType.VarChar, 1).Value = "N"
                cmd.Parameters.Add("@cmf_CompAsoc", SqlDbType.Bit).Value = 0
                cmd.Parameters.Add("@cmf_CodApe", SqlDbType.VarChar, 4).Value = ""
                cmd.Parameters.Add("@cmf_FecMod", SqlDbType.DateTime).Value = Date.Now
                cmd.Parameters.Add("@cmfusu_Codigo", SqlDbType.VarChar, 15).Value = "CNV"
                cmd.Parameters.Add("@cmf_Convert", SqlDbType.VarChar, 1).Value = ""

                cmd.ExecuteScalar()


            End Using
        End Using

        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd.CommandText = "insert into MovF(" +
                              "[mfoemp_Codigo]" +
                              ",[mfosuc_Cod]" +
                              ",[mfocmf_ID]" +
                              ",[mfocmf_FMov]" +
                              ",[mfo_MarcaOA]" +
                              ",[mfo_CodConcepto]" +
                              ",[mfoori_Cod]" +
                              ",[mfo_EmiteChq]" +
                              ",[mfomon_Cod]" +
                              ",[mfomtca_codigo]" +
                              ",[mfo_ImpMonLoc]" +
                              ",[mfo_ImpMonElem]" +
                              ",[mfo_MarcaDepu]" +
                              ",[mfo_PasadoACC]" +
                              ",[mfo_FecMod]" +
                              ",[mfousu_Codigo]" +
                              ",[mfo_ImprimeChq]" +
                                ") VALUES (" +
                                "@mfoemp_Codigo" +
                                ",@mfosuc_Cod" +
                                ",@mfocmf_ID" +
                                ",@mfocmf_FMov" +
                                ",@mfo_MarcaOA" +
                                ",@mfo_CodConcepto" +
                                ",@mfoori_Cod" +
                                ",@mfo_EmiteChq" +
                                ",@mfomon_Cod" +
                                ",@mfomtca_codigo" +
                                ",@mfo_ImpMonLoc" +
                                ",@mfo_ImpMonElem" +
                                ",@mfo_MarcaDepu" +
                                ",@mfo_PasadoACC" +
                                ",@mfo_FecMod" +
                                ",@mfousu_Codigo" +
                                ",@mfo_ImprimeChq" +
                                ")"


                cmd.Parameters.Add("@mfoemp_Codigo", SqlDbType.VarChar, 4).Value = "SIPT"
                cmd.Parameters.Add("@mfosuc_Cod", SqlDbType.VarChar, 4).Value = " "
                cmd.Parameters.Add("@mfocmf_ID", SqlDbType.Int).Value = dameNroFondo()
                'cmd.Parameters.Add("@mfo_ID", SqlDbType.Int).Value = dameNroMovFondo()
                cmd.Parameters.Add("@mfocmf_FMov", SqlDbType.DateTime).Value = Date.Now
                cmd.Parameters.Add("@mfo_MarcaOA", SqlDbType.VarChar, 1).Value = "1"
                cmd.Parameters.Add("@mfo_CodConcepto", SqlDbType.SmallInt).Value = "6"
                cmd.Parameters.Add("@mfoori_Cod", SqlDbType.VarChar, 15).Value = dataConceptos.Rows(I).Cells(0).Value
                cmd.Parameters.Add("@mfo_EmiteChq", SqlDbType.Bit).Value = 0
                cmd.Parameters.Add("@mfomon_Cod", SqlDbType.VarChar, 3).Value = "1"
                cmd.Parameters.Add("@mfomtca_codigo", SqlDbType.VarChar, 3).Value = "UNI"
                cmd.Parameters.Add("@mfo_ImpMonLoc", SqlDbType.Float).Value = DataGridView1.Rows(posicionFila).Cells("reintegrar").Value * (-1)
                cmd.Parameters.Add("@mfo_ImpMonElem", SqlDbType.Float).Value = DataGridView1.Rows(posicionFila).Cells("reintegrar").Value * (-1)
                cmd.Parameters.Add("@mfo_MarcaDepu", SqlDbType.Bit).Value = 0
                cmd.Parameters.Add("@mfo_PasadoACC", SqlDbType.Bit).Value = 0
                cmd.Parameters.Add("@mfo_FecMod", SqlDbType.DateTime).Value = Date.Now
                cmd.Parameters.Add("@mfousu_Codigo", SqlDbType.VarChar, 15).Value = "CNV"
                cmd.Parameters.Add("@mfo_ImprimeChq", SqlDbType.Bit).Value = 0

                cmd.ExecuteScalar()


            End Using
        End Using

        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd.CommandText = "insert into MovF(" +
                                "[mfoemp_Codigo]" +
                                ",[mfosuc_Cod]" +
                                ",[mfocmf_ID]" +
                                ",[mfocmf_FMov]" +
                                ",[mfo_MarcaOA]" +
                                ",[mfo_CodConcepto]" +
                                ",[mfobco_Cod]" +
                                ",[mfobco_Suc]" +
                                ",[mfoctb_Cod]" +
                                ",[mfo_EmiteChq]" +
                                ",[mfomon_Cod]" +
                                ",[mfomtca_codigo]" +
                                ",[mfo_ImpMonLoc]" +
                                ",[mfo_ImpMonElem]" +
                                ",[mfo_MarcaDepu]" +
                                ",[mfo_PasadoACC]" +
                                ",[mfo_FecMod]" +
                                ",[mfousu_Codigo]" +
                                ",[mfo_ImprimeChq]" +
                                ") VALUES (" +
                                "@mfoemp_Codigo" +
                                ",@mfosuc_Cod" +
                                ",@mfocmf_ID" +
                                ",@mfocmf_FMov" +
                                ",@mfo_MarcaOA" +
                                ",@mfo_CodConcepto" +
                                ",@mfobco_Cod" +
                                ",@mfobco_Suc" +
                                ",@mfoctb_Cod" +
                                ",@mfo_EmiteChq" +
                                ",@mfomon_Cod" +
                                ",@mfomtca_codigo" +
                                ",@mfo_ImpMonLoc" +
                                ",@mfo_ImpMonElem" +
                                ",@mfo_MarcaDepu" +
                                ",@mfo_PasadoACC" +
                                ",@mfo_FecMod" +
                                ",@mfousu_Codigo" +
                                ",@mfo_ImprimeChq" +
                                ")"


                cmd.Parameters.Add("@mfoemp_Codigo", SqlDbType.VarChar, 4).Value = "SIPT"
                cmd.Parameters.Add("@mfosuc_Cod", SqlDbType.VarChar, 4).Value = " "
                cmd.Parameters.Add("@mfocmf_ID", SqlDbType.Int).Value = dameNroFondo()
                'cmd.Parameters.Add("@mfo_ID", SqlDbType.Int).Value = dameNroMovFondo()
                cmd.Parameters.Add("@mfocmf_FMov", SqlDbType.DateTime).Value = Date.Now
                cmd.Parameters.Add("@mfo_MarcaOA", SqlDbType.VarChar, 1).Value = "2"
                cmd.Parameters.Add("@mfo_CodConcepto", SqlDbType.SmallInt).Value = "4"
                cmd.Parameters.Add("@mfobco_Cod", SqlDbType.VarChar, 3).Value = dataBancos.Rows(X).Cells(0).Value
                cmd.Parameters.Add("@mfobco_Suc", SqlDbType.VarChar, 4).Value = ""
                cmd.Parameters.Add("@mfoctb_Cod", SqlDbType.VarChar, 15).Value = dataBancos.Rows(X).Cells(1).Value
                cmd.Parameters.Add("@mfo_EmiteChq", SqlDbType.Bit).Value = 0
                cmd.Parameters.Add("@mfomon_Cod", SqlDbType.VarChar, 3).Value = "1"
                cmd.Parameters.Add("@mfomtca_codigo", SqlDbType.VarChar, 3).Value = "UNI"
                cmd.Parameters.Add("@mfo_ImpMonLoc", SqlDbType.Float).Value = DataGridView1.Rows(posicionFila).Cells("reintegrar").Value
                cmd.Parameters.Add("@mfo_ImpMonElem", SqlDbType.Float).Value = DataGridView1.Rows(posicionFila).Cells("reintegrar").Value
                cmd.Parameters.Add("@mfo_MarcaDepu", SqlDbType.Bit).Value = 0
                cmd.Parameters.Add("@mfo_PasadoACC", SqlDbType.Bit).Value = 0
                cmd.Parameters.Add("@mfo_FecMod", SqlDbType.DateTime).Value = Date.Now
                cmd.Parameters.Add("@mfousu_Codigo", SqlDbType.VarChar, 15).Value = "CNV"
                cmd.Parameters.Add("@mfo_ImprimeChq", SqlDbType.Bit).Value = 0

                cmd.ExecuteScalar()


            End Using
        End Using
        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()

                conn.Open()
                cmd.CommandText = "insert into AASobrantes(" +
                                                "[nroOrden]" +
                                                ",[sobrante]" +
                                                ",[fechaMod]" +
                                                ",[nombre]" +
                                                ",[total]" +
                                                ",[donacion]" +
                                                ") VALUES (" +
                                                "@nroOrden" +
                                                ",@sobrante" +
                                                ",@fechaMod" +
                                                ",@nombre" +
                                                ",@total" +
                                                ",@donacion" +
                                                ")"

                cmd.Parameters.Add("@nroOrden", SqlDbType.Int).Value = adjuntarAnio()
                cmd.Parameters.Add("@sobrante", SqlDbType.Float).Value = DataGridView1.Rows(posicionFila).Cells("reintegrar").Value
                cmd.Parameters.Add("@fechaMod", SqlDbType.DateTime).Value = Date.Now
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 50).Value = DataGridView1.Rows(posicionFila).Cells("nombre").Value
                cmd.Parameters.Add("@total", SqlDbType.Float).Value = DataGridView1.Rows(posicionFila).Cells("Devengado").Value
                cmd.Parameters.Add("@donacion", SqlDbType.VarChar, 2).Value = "X"
                cmd.ExecuteScalar()


            End Using
        End Using

    End Sub

    Public Sub cargarRendiciones()
        sql = "Select Fecha,Descripcion,Monto,Nombre from AARendiciones where NroInreso='" & adjuntarAnio() & "'"

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Do While reader.Read()
                    Dim fila(5) As Object
                    fila(3) = Numalet.ToCardinal(reader(2))
                    fila(0) = reader(1)
                    'fila(2) = reader(0)
                    'fila(3) = reader(3)

                    DataGridView2.Rows.Add(fila)

                Loop

            End Using
        End Using
    End Sub

    Public Sub cargarNombresRendicion()
        'consulta sql que trae el nombre de las personas que son parte de la rendicion
        sql = " SELECT  distinct CabCompra.ccopro_RazSoc FROM CabCompra  INNER JOIN CausaEmi ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod INNER JOIN  ItemComp ON CabCompra.cco_ID=ItemComp.icocco_ID  WHERE  CausaEmi.cem_Desc='" & adjuntarAnio() & "' group BY CabCompra.ccopro_RazSoc"
        'se conecta con la base de datos y ejecuta la consulta sql. 
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader

                'recorre el resultado y lo agrega a la columna 0 de cada nuevo resultado
                Do While reader.Read()
                    Dim fila(1) As Object
                    fila(0) = reader(0)
                    DataGridView1.Rows.Add(fila)
                Loop
            End Using
        End Using
        'adjuntarDatosRendicion()
    End Sub

    Public Sub cargarApertura()
        Dim codigoSituacion As String
        Dim codigoEmpresa As String
        Dim codigoSucursal As String
        Dim ID As String
        Dim nroCuota As String
        Dim estado As String = "ABIERTO"

        sql = "select Codigo from AASituaciones where Descripcion like '%" & cmbTipoOrden.Text & "%'"

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                reader.Read()

                codigoSituacion = reader(0)

            End Using
        End Using
        Try
            sql = " SELECT distinct  ccoemp_Codigo,ccosuc_Cod,cco_ID,cco_NroCuota,ccocmf_ID FROM CabCompra  INNER JOIN CausaEmi ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod INNER JOIN  ItemComp ON CabCompra.cco_ID=ItemComp.icocco_ID  WHERE  CausaEmi.cem_Desc='" & adjuntarAnio() & "'ORDER BY ccoemp_Codigo,ccosuc_Cod,cco_ID,cco_NroCuota"
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql

                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    reader.Read()
                    codigoEmpresa = reader(0)
                    codigoSucursal = reader(1)
                    ID = reader(2)
                    nroCuota = reader(3)
                    ccoMf = reader(4)
                End Using
            End Using
        Catch ex As Exception

        End Try

        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd.CommandText = "insert into AAEstadosCabCompraSituaciones(" +
                               "[ccoemp_Codigo]" +
                               ",[ccosuc_Cod]" +
                               ",[cco_ID]" +
                               ",[cco_NroCuota]" +
                               ",[Estado]" +
                               ",[Situacion]" +
                                ") VALUES (" +
                                "@ccoemp_Codigo" +
                                ",@ccosuc_Cod" +
                                ",@cco_ID" +
                                ",@cco_NroCuota" +
                                ",@Estado" +
                                ",@Situacion" +
                               ")"
                cmd.Parameters.Add("@ccoemp_Codigo", SqlDbType.VarChar, 4).Value = codigoEmpresa
                cmd.Parameters.Add("@ccosuc_Cod", SqlDbType.VarChar, 4).Value = codigoSucursal
                cmd.Parameters.Add("@cco_ID", SqlDbType.Int).Value = ID
                cmd.Parameters.Add("@cco_NroCuota", SqlDbType.VarChar, 3).Value = nroCuota
                cmd.Parameters.Add("@Estado", SqlDbType.VarChar, 10).Value = estado
                cmd.Parameters.Add("@Situacion", SqlDbType.Int).Value = codigoSituacion
                cmd.ExecuteScalar()

            End Using
        End Using


    End Sub

    Public Sub cargarCierre()
        'se declara las variables necesarias
        Dim codigoSituacion As String
        Dim codigoEmpresa As String
        Dim codigoSucursal As String
        Dim ID As String
        Dim nroCuota As String
        Dim estado As String = "CERRADO"
        'consulta sql que busca en la tabla situaciones la opcion que se eligio en el comboBox cmbTipoOrden
        sql = "select Codigo from AASituaciones where Descripcion like '%" & cmbTipoOrden.Text & "%'"
        'se conecta con la base de datos y ejecuta la sentencia y luego guarda el codigo en la variable codigoSituacional
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                reader.Read()
                codigoSituacion = reader(0)
            End Using
        End Using
        '
        sql = " SELECT distinct  ccoemp_Codigo,ccosuc_Cod,cco_ID,cco_NroCuota FROM CabCompra  INNER JOIN CausaEmi ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod INNER JOIN  ItemComp ON CabCompra.cco_ID=ItemComp.icocco_ID  WHERE  CausaEmi.cem_Desc='" & adjuntarAnio() & "'ORDER BY ccoemp_Codigo,ccosuc_Cod,cco_ID,cco_NroCuota"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                reader.Read()
                codigoEmpresa = reader(0).ToString
                codigoSucursal = reader(1)
                ID = reader(2)
                nroCuota = reader(3)

            End Using
        End Using
        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd.Parameters.Add("@ccoemp_Codigo", SqlDbType.VarChar, 4).Value = codigoEmpresa
                cmd.Parameters.Add("@ccosuc_Cod", SqlDbType.VarChar, 4).Value = codigoSucursal
                cmd.Parameters.Add("@cco_ID", SqlDbType.Int).Value = ID
                cmd.Parameters.Add("@cco_NroCuota", SqlDbType.VarChar, 3).Value = nroCuota
                cmd.Parameters.Add("@Estado", SqlDbType.VarChar, 10).Value = estado
                cmd.Parameters.Add("@Situacion", SqlDbType.Int).Value = codigoSituacion

                cmd.CommandText = "update AAEstadosCabCompraSituaciones set " +
                               "[ccoemp_Codigo]=" + "@ccoemp_Codigo" +
                               ",[ccosuc_Cod]=" + "@ccosuc_Cod" +
                               ",[cco_ID]=" + "@cco_ID" +
                               ",[cco_NroCuota]=" + "@cco_NroCuota" +
                               ",[Estado]=" + "@Estado" +
                               ",[Situacion]=" + "@Situacion" +
                                " where [cco_ID]= @cco_ID"
                cmd.ExecuteScalar()


            End Using
        End Using
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
                    fila(0) = "O.C. Libradas y Viat. y Movil."
                    fila(1) = reader(0)
                    DataGridView1.Rows.Add(fila)
                Loop

            End Using
        End Using
        adjuntarPagos()
    End Sub
    'carga a todas las personas involucradas en la rendicion en un comboBox dentro de un datagrid
    Public Sub cargarComboNombres()
        'consulta sql que trae todos los nombres de las personas involucradas en la rendicion
        sql = " SELECT  distinct CabCompra.ccopro_RazSoc FROM CabCompra  INNER JOIN CausaEmi ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod INNER JOIN  ItemComp ON CabCompra.cco_ID=ItemComp.icocco_ID  WHERE  CausaEmi.cem_Desc='" & adjuntarAnio() & "' group BY CabCompra.ccopro_RazSoc"
        'conectar con la base de datos y ejecuto la sentancia sql.
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                'recorre el resultado y lo carga en un comboBox
                Do While reader.Read()
                    Dim fila(1) As String
                    fila(0) = reader(0)
                    NOMBREGRID.Items.Add(fila(0))
                Loop

            End Using
        End Using
        adjuntarDatosRendicion()
    End Sub

#End Region

#Region "Calculos"
    'Esta funcion recorre el DataGridView1 y suma el total del contenido de la columna Devengado y lo asigna
    'en la caja de texto, txtTotalFinal
    Public Sub calculoTotalViatico()
        Dim Total As Double = 0
        For Each row As DataGridViewRow In Me.DataGridView1.Rows
            Total += row.Cells("Devengado").Value
        Next
        txtTotalFinal.Text = Total.ToString
    End Sub

    Public Sub calculoTotalGastos()
        Try
            Dim Total As Double = 0

            For Each row As DataGridViewRow In Me.DataGridView1.Rows
                Total += row.Cells(2).Value
            Next

            txtOtrosGastos.Text = Total.ToString
        Catch ex As Exception

        End Try
        
    End Sub
    'Esta funcion recorre el DataGridView1 y suma el total del contenido de la columna combustible y lo asigna
    'en la caja de texto txtCombustible. 

    Public Sub calculoTotalCombustible()
        Dim Total As Double = 0
        Try
            For Each row As DataGridViewRow In Me.DataGridView1.Rows
                Total += row.Cells("COMBUSTIBLE").Value
            Next
        Catch ex As Exception
        End Try
        txtCombustible.Text = Total.ToString
    End Sub

    'Esta funcion recorre el DataGridView1 y suma el total del contenido de la columna peajes y lo asigna
    'en la caja de texto txtPeaje. 
    Public Sub calculoTotalPeajes()
        Dim Total As Double = 0
        Try
            For Each row As DataGridViewRow In Me.DataGridView1.Rows
                Total += row.Cells("PEAJES").Value
            Next
        Catch ex As Exception
        End Try
        txtPeaje.Text = Total.ToString
    End Sub

    'Esta funcion recorre el DataGridView1 y suma el total del contenido de la columna Viaticos A rendir y lo asigna
    'en la caja de texto, txtViaticos.
    Public Sub calculoTotalViaticoMisiones()
        Dim Total As Double = 0
        Try
            For Each row As DataGridViewRow In Me.DataGridView1.Rows
                Total += row.Cells("Viaticos A rendir").Value
            Next
        Catch ex As Exception
        End Try
        txtViaticos.Text = Total.ToString
    End Sub

    'Esta funcion recorre el DataGridView1 y suma el total del contenido de la columna dias y lo asigna
    'en la caja de texto, txtDia
    Public Sub calculoTotalDias()
        Dim Columna As Integer = 1
        Dim Total As Double
        For Each row As DataGridViewRow In Me.DataGridView1.Rows
            Total = Total + row.Cells("dias").Value
        Next
        txtDia.Text = Total
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
            dia = row.Cells("DIAS").Value
            MontoFinal = monto - dia
            row.Cells("Devengado").Value = MontoFinal
        Next
    End Sub

    'Public Sub calcularCargosMora()
    '    Dim fechaInicio As Date = dtSalida.Value
    '    Dim fechaFin As Date = dtLlegada.Value
    '    Dim dif As Double
    '    Dim i As Double
    '    Dim j As Double


    '    dif = DateDiff(DateInterval.Day, fechaInicio, fechaFin) + 1
    '    If dif > 3 Then
    '        Label10.Visible = True
    '        txtMora.Visible = True
    '    Else
    '        Label10.Visible = True
    '        txtMora.Visible = True
    '    End If

    'End Sub
    Public Sub calcularCargosMora()
        'Dim fechaInicio As Date = dtSalida.Value
        'Dim fechaFin As Date = dtLlegada.Value
        'Dim dif As Double
        'Dim i As Double
        'Dim j As Double


        'dif = DateDiff(DateInterval.Day, fechaInicio, fechaFin) + 1
        'If dif > 3 Then
        '    Label10.Visible = True
        '    txtMora.Visible = True
        'Else
        '    Label10.Visible = True
        '    txtMora.Visible = True
        'End If

    End Sub

    Public Sub calcularMontoPago()
        Dim Total As Double = 0


        For Each row As DataGridViewRow In Me.DataGridView1.Rows
            Total += row.Cells("IMPORTE").Value
        Next

        txtTotalFinal.Text = Total.ToString
    End Sub

    Public Sub calcularReintegros()
        Dim Total As Double = 0

        For Each row As DataGridViewRow In Me.DataGridView2.Rows
            Total += row.Cells("MONTO").Value
        Next

        txtMontoReintegro.Text = Total.ToString
    End Sub

#End Region

#Region "Eventos del Formulario"

    Private Sub btnSalir_Click(sender As System.Object, e As System.EventArgs) Handles btnSalir.Click
        Me.Close()

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim I As Integer = DataGridView1.CurrentCell.RowIndex
        ActualizarNro.txtNroPago.Text = txtCodigo.Text
        If ((chkAutorizado.Checked) And (cmbTipoOrden.Text.Equals("Viaticos"))) Then
            ActualizarNro.txtDestinatario.Text = DataGridView1.Rows(I).Cells(0).Value.ToString
            ActualizarNro.txtNroCuenta.Text = DataGridView1.Rows(I).Cells(1).Value.ToString
            ActualizarNro.txtAnteriorNroCheque.Text = DataGridView1.Rows(I).Cells(2).Value.ToString
            ActualizarNro.Show()
        Else
            'ActualizarNro.txtNroCuenta.Text = DataGridView1.Rows(I).Cells(1).Value.ToString
            'ActualizarNro.txtAnteriorNroCheque.Text = DataGridView1.Rows(I).Cells(2).Value.ToString
            'ActualizarNro.txtDestinatario.Text = DataGridView1.Rows(I).Cells(3).Value.ToString

        End If





        'descripcion = 
        'precio = DataGridView4.Rows(I).Cells(6).Value
        'TextBox6.Text = descripcion
        'TextBox10.Text = precio.ToString
        'TextBox9.Text = codigo
        'TextBox11.Text = DataGridView4.Rows(I).Cells(1).Value.ToString
        ''Dim unActualizarNro As Form = New ActualizarNro

        'unActualizarNro.Show()
    End Sub

    Private Sub GeneradorOrdenes_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        cargarCombo()
        GroupBox4.Visible = False
        GroupBox5.Visible = False
        GroupBox7.Visible = False
        GroupBox8.Visible = False
        GroupBox9.Visible = False
        GroupBox10.Visible = False
        GroupBox11.Visible = False
        GroupBox12.Visible = False
        GroupBox13.Visible = False
        chkPago.Visible = False
        chkReintegro.Visible = False
        btnCalcular.Visible = False
        'Label10.Visible = False
        'txtMora.Visible = False
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

    Private Sub btnBuscar_Click(sender As System.Object, e As System.EventArgs) Handles btnBuscar.Click
        DataGridView1.Rows.Clear()
        DataGridView1.Columns.Clear()
        If txtCodigo.Text = Nothing Then
            MessageBox.Show("Debe ingresar un codigo valido", "AVISO")
        Else
            'Try

            If cmbTipoOrden.SelectedItem = "Viaticos" Then
                If chkAutorizado.Checked Then
                    ordenCargo()
                Else
                    cargarDataGrid()
                End If
            Else
                
                If cmbTipoOrden.SelectedItem = "Rendicion" Then
                    'lipia el DataGridView2
                    DataGridView2.Rows.Clear()
                    If (buscarRendicionesAnteriores()) Then
                        Select Case MsgBox("¿Esta orden contiene rendiciones guardadas desea eliminarlas?", MsgBoxStyle.YesNo, "AVISO")
                            'en caso de elegir si
                            Case MsgBoxResult.Yes
                                eliminarRendiciones_SobrantesAnteriores()
                            Case MsgBoxResult.No
                                MessageBox.Show("Accion cancelada por el usuario", "INFORMACION")
                        End Select

                    End If
                    'hace visible las fechas de salida y llegada de la rendicion
                    GroupBox4.Visible = True
                    'hace visible la tabla donde van los viaticos en la parte inferior
                    GroupBox5.Visible = True

                    'hace visible el boto calcular
                    btnCalcular.Visible = True

                    'hace visible el recuadro total
                    GroupBox9.Visible = True

                    'hace visible la caja de texto con la leyenda PAGOS POR MORA
                    'txtMora.Visible = True

                    'carga el recudro concepto con todos los conceptos de la tabla origenes
                    cargarConceptos()

                    'busca el atributo ori_Cod, ori_Desc de la tabla origenes
                    cargarConceptosPagos()

                    'trae todos los atributos ctbbco_Cod, de la tabla CtaBan que contiene todas las cuentas
                    'bancarias.
                    cargarBancos()
                    'carga codigo y descripcion de la tabla CtaBan a del dataGrid "dataBancosPago"
                    cargarBancosPagos()

                    'carga las descripciones de los ditintos items de el viatico mas las personas involucradas, sus importes.
                    'tambien los totales de dias,peaje,combustible, y viaticos.
                    generarRendicion()
                    ''busca la fecha de salida de la cuasa de emision y la asigna de dtSalida
                    establecerFechaInicio()
                Else
                    If cmbTipoOrden.SelectedItem = "Pago" Then
                        generarOrdenPago()
                    Else
                        If cmbTipoOrden.SelectedItem = "Historial Rendicion" Then
                            DataGridView2.Rows.Clear()

                            'cargarRendiciones()

                            'carga el recuadro Reintegrar en conceptos con todos los 
                            'conceptos de la tabla origenes
                            cargarConceptos()

                            'carga el recuadro Pagos al recuadro mas chico conceptos de la ventana los datos
                            'de la tabla origenes
                            cargarConceptosPagos()

                            'carga el recuadro reintegros en el recudro mas pequeño pagos los datos del tabla ctaBan
                            cargarBancos()
                            'cargar el recuadro Pagos en el recuadro mas pequeños los datos de la tabla ctaBan
                            cargarBancosPagos()

                            'carga las descripciones de los ditintos items de el viatico mas las personas involucradas, sus importes.
                            'tambien los totales de dias,peaje,combustible, y viaticos.
                            generarRendicion()

                            'busca las fechas,descripcion,monto,nombre,destino que se cargaron en la tabla AARendiciones los cuales pertenecen al historial de la rendiciones
                            'luego se le asigna el detino el la caja de texto txtDestino.
                            cargarHistorialReintegro()
                        Else
                            If cmbTipoOrden.SelectedItem = "Historial Viatico" Then
                                If chkAutorizado.Checked Then
                                    ordenCargo()
                                Else
                                    cargarDataGrid()
                                End If
                            Else
                                If cmbTipoOrden.SelectedItem = "Historial Pago" Then
                                    generarOrdenPago()
                                Else
                                    MessageBox.Show("Debe seleccionar un tipo de orden valido", "AVISO")
                                End If
                            End If
                        End If
                    End If
                End If
                End If
            'Catch ex As Exception
            '    MessageBox.Show("Por favor verifique el numero de entrada", "ERROR")
            'End Try

            End If
    End Sub
    Public Sub volverAGenerar()
        DataGridView1.Rows.Clear()
        DataGridView1.Columns.Clear()
        If txtCodigo.Text = Nothing Then
            MessageBox.Show("Debe ingresar un codigo valido", "AVISO")
        Else
            If cmbTipoOrden.SelectedItem = "Rendicion" Then
                'hace visible las fechas de salida y llegada de la rendicion
                GroupBox4.Visible = True
                'hace visible la tabla donde van los viaticos en la parte inferior
                GroupBox5.Visible = True

                'hace visible el boto calcular
                btnCalcular.Visible = True

                'hace visible el recuadro total
                GroupBox9.Visible = True

                'hace visible la caja de texto con la leyenda PAGOS POR MORA
                '.Visible = True

                'carga el recudro concepto con todos los conceptos de la tabla origenes
                cargarConceptos()

                'busca el atributo ori_Cod, ori_Desc de la tabla origenes
                cargarConceptosPagos()

                'trae todos los atributos ctbbco_Cod, de la tabla CtaBan que contiene todas las cuentas
                'bancarias.
                cargarBancos()
                'carga codigo y descripcion de la tabla CtaBan a del dataGrid "dataBancosPago"
                cargarBancosPagos()

                'carga las descripciones de los ditintos items de el viatico mas las personas involucradas, sus importes.
                'tambien los totales de dias,peaje,combustible, y viaticos.
                generarRendicion()
                ''busca la fecha de salida de la cuasa de emision y la asigna de dtSalida
                establecerFechaInicio()
            End If
        End If
    End Sub
    Public Sub eliminarRendiciones_SobrantesAnteriores()
        Dim sql As String
        sql = "DELETE FROM [dbo].[AARendiciones]   WHERE  [NroInreso] = '" & adjuntarAnio() & "'"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                cmd4.ExecuteScalar()
            End Using
        End Using

        sql = "DELETE FROM [dbo].[AASobrantes]   WHERE  [nroOrden] = '" & adjuntarAnio() & "'"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                cmd4.ExecuteScalar()
            End Using
        End Using
    End Sub
    Public Function buscarRendicionesAnteriores() As Boolean
        Dim ban As Boolean = False
        Dim sql As String
        sql = "SELECT [Codigo],[Fecha],[Descripcion],[Monto],[Nombre],[NroInreso],[Destino]  FROM [dbo].[AARendiciones] where [NroInreso] = '" & adjuntarAnio() & "'"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Do While reader.Read()
                    Dim fila(1) As Object
                    ban = True
                Loop
            End Using
        End Using
        Return ban
    End Function

    Private Sub cargarPosibleAjuste()
        Dim I As Integer = dataConceptos.CurrentCell.RowIndex
        Dim codigo As String
        Dim sql As String
        If chkReintegro.Checked Then
            codigo = dataConceptos.Rows(I).Cells(0).Value
            MessageBox.Show(codigo)
            If codigo.Equals("907") Then
                sql = "update AASobrantes set donacion ='Y' where nroOrden='" & adjuntarAnio() & "'and nombre='" & DataGridView2.Rows(0).Cells(3).Value & " '"
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()
                        conn.Open()
                        cmd4.CommandText = sql
                        cmd4.ExecuteScalar()
                    End Using
                End Using
            End If
        End If
    End Sub

    Private Sub btnImprimir_Click(sender As System.Object, e As System.EventArgs) Handles btnImprimir.Click
        ' pregunta si el checkBox de orden nacional o el checkBox de orden provincial esta marcado
        If chkNac.Checked Or chkProv.Checked Then
            'pregunta el valor en la caja de texto es igual a nada y despliega 
            If txtCodigo.Text = Nothing Then
                MessageBox.Show("Debee ingresar un codigo valido", "AVISO")
            Else
                'pregunta la opcion del comoboBox es Viatico
                If cmbTipoOrden.Text.Equals("Viaticos") Then
                    'pregunta si el checkbox autorizado esta seleccionado
                    If chkAutorizado.Checked Then
                        'de estar seleccionado el checkbox autorizado se invoca a imprimirOrdenCargoCheques()
                        imprimirOrdenCargoCheques()
                    Else
                        'de no estar seleccionado el checkbox autorizado se invoca a imprimirOrdenCargoPrimero()
                        impirmirOrdenCargoPrimero()
                    End If
                    ' si la opcion del comboBox no es viatico
                Else
                    'si la opcion del comboBox es Rendicion
                    If cmbTipoOrden.Text = "Rendicion" Then
                        'mustra ventana de dos opciones con el siguente mensaje
                        Select Case MsgBox("¿Esta seguro que desea generar un pago?", MsgBoxStyle.YesNo, "AVISO")
                            'en caso de seleccionar un si
                            Case MsgBoxResult.Yes
                                'pregunta si el checkbox de reintego o el checkBox  de pago estan visibles
                                If chkReintegro.Visible = True Or chkPago.Visible = True Then
                                    'pregunta si el checkbox de reintego o el checkBox  de pago no estan visibles
                                    If chkReintegro.Checked And chkPago.Visible = False Then
                                        'invoca al metodo carcarApertura()
                                        cargarApertura()
                                        'invoca al metodo imprimir rendicion
                                        imprimirRendicion()
                                        'si el checkBox reitegro y chekbox pago  no estan invisible o en estado falso
                                    Else
                                        'pregunta si el checkBox de reitegro no esta visible pero checkBox pago esta visible
                                        If chkReintegro.Visible = False And chkPago.Checked Then
                                            'invoca al metodo cargarApertura
                                            cargarApertura()
                                            'invoca al metodo imprimirRendicion()
                                            imprimirRendicion()

                                        Else ' si no
                                            'pregunta si el checkbox reitegro esta marcado y el checkBox pago esta marcado
                                            If chkReintegro.Checked And chkPago.Checked Then
                                                'invocar metodo cargarApertura
                                                cargarApertura()
                                                'invoca al metodo imprimir rendicion
                                                imprimirRendicion()
                                            Else 'si no 
                                                'invoca al metodo cargarApertura
                                                cargarApertura()
                                                'invoca al metodo imprimirRendicion()
                                                imprimirRendicion()

                                                'MessageBox.Show("Faltan Reintegros o pagos", "No se puede generar la rendicion")
                                            End If
                                        End If
                                    End If
                                Else ''pregunta si el checkbox de reintego o el checkBox  de pago estan visibles
                                    'invocar metodo cargarApertura()
                                    cargarApertura()
                                    'invoca metodo imprirRendicion
                                    imprimirRendicion()
                                End If
                                ' si la opcion de la ventana es No
                            Case MsgBoxResult.No
                                MessageBox.Show("Accion cancelada por el usuario", "INFORMACION")
                        End Select
                    Else
                        'pregunta si la opcion del comboBox es Pago
                        If cmbTipoOrden.Text = "Pago" Then
                            'pregunta si el check box de Res o el check box de disposicion estan marcados
                            If chkRes.Checked Or chkDisp.Checked Then
                                'muestra una ventana con 2 opciones.
                                Select Case MsgBox("¿Esta seguro que desea generar un pago?", MsgBoxStyle.YesNo, "AVISO")
                                    'en caso de elegir si
                                    Case MsgBoxResult.Yes
                                        '''''''''''
                                        'pregunta se la eleccion del combo box es Rendicion FP

                                        '''''''''''
                                        'pregutna si el checkBox autorizado esta marcado
                                        If chkAutorizado.Checked Then
                                            'xxxxxxxxxxxxxxxxxxxx
                                            'invoca al metodoa imprimir pago autorizado
                                            imprimirPagoAutorizado()
                                            'xxxxxxxxxxxxxxxxxxxx
                                        Else
                                            'invoca al metodo cargarCierre
                                            cargarCierre()
                                            'invoca al metodo imprimir cargo
                                            imprimirPago()
                                        End If


                                    Case MsgBoxResult.No
                                        MessageBox.Show("Accion cancelada por el usuario", "INFORMACION")
                                End Select
                            Else
                                MessageBox.Show("Debe seleccionar una opcion", "NACIONAL O PROVINCIAL??")
                            End If
                        Else
                            'pregunta si el valor del comboBox es historial rendicion
                            If cmbTipoOrden.Text = "Historial Rendicion" Then
                                imprimirHistorialRendicion()
                            Else 'si no
                                'pregunta si la eleccion del combo box  es historial viatico
                                If cmbTipoOrden.Text.Equals("Historial Viatico") Then
                                    If chkAutorizado.Checked Then
                                        imprimirOrdenCargoChequesHistorial()
                                    Else
                                        impirmirOrdenCargoPrimeroHistorial()
                                    End If
                                Else
                                    'si la opcion del combo box es historial Pago
                                    If cmbTipoOrden.Text.Equals("Historial Pago") Then
                                        'xxxxxxxxxxx
                                        If chkAutorizado.Checked Then
                                            imprimirPagoAutorizado()

                                        Else
                                            'invocar metodo imprimirPagoHistorial()
                                            imprimirPagoHistorial()
                                            'cargarAutorizadosComunes()
                                        End If

                                        'xxxxxxxxxx

                                    Else
                                        MessageBox.Show("Debe seleccionar un tipo de orden", "AVISO")
                                    End If

                                    End If
                            End If
                        End If

                    End If

                End If

            End If
        Else
            MessageBox.Show("Debe seleccionar una opcion", "NACIONAL O PROVINCIAL??")
        End If
    End Sub


    Private Sub chkAutorizado_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkAutorizado.CheckedChanged
        If chkAutorizado.Checked Then
            GroupBox3.Visible = True
            GroupBox6.Visible = False
        Else
            GroupBox3.Visible = True
            GroupBox6.Visible = True
        End If
    End Sub

    Private Sub cmbTipoOrden_TextChanged(sender As Object, e As System.EventArgs) Handles cmbTipoOrden.TextChanged
        'si en la lista desplegable se selecciona la opcion "Rendicion" activa los distintos GroupBox y las cajas de texto
        'y botones correspondientes.
        If cmbTipoOrden.SelectedItem = "Rendicion" Then
            GroupBox4.Visible = True
            GroupBox5.Visible = True
            'Label10.Visible = True
            'txtMora.Visible = True
            'btnCalcular.Visible = True
            GroupBox9.Visible = True
            txtSaliente.Text = ""
        Else
            'si no, que todos los GroupBox, cajas de texto y botones correspondientes
            GroupBox4.Visible = False
            GroupBox5.Visible = False
            chkReintegro.Visible = False
            GroupBox7.Visible = False
            GroupBox8.Visible = False
            'Label10.Visible = False
            'txtMora.Visible = False
            'btnCalcular.Visible = False
            GroupBox9.Visible = False
            GroupBox10.Visible = False
            txtSaliente.Text = ""

            'si en la lista desplegable se selecciona "Pago"
            If cmbTipoOrden.SelectedItem = "Pago" Then
                'si el checkBox Nacional esta seleccionado se busca en la base de datos el atributo
                '"tal_ActualNro y se muestra en txtSaliente y se incrementa en 1
                If para.Equals("Nacional") Then
                    sql = "Select tal_ActualNro from Talonar where tal_Cod='OPN'"
                Else
                    'si el checkBox provincial esta seleccionado se busca en la base de datos el atributo
                    '"tal_ActualNro y se muestra en txtSaliente y se incrementa en 1

                    If para.Equals("Provincial") Then
                        sql = "Select tal_ActualNro from Talonar where tal_Cod='OPP'"
                    End If
                End If
                'conecta con la base de datos y ejecuta la base de datos.
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
                'si en la lista desplegable se selecciona "Viaticos"
                If cmbTipoOrden.SelectedItem = "Viaticos" Then
                    'si el checkBox provincial esta seleccionado se busca en la base de datos el atributo
                    '"tal_ActualNro y se muestra en txtSaliente y se incrementa en 1
                    If para.Equals("Nacional") Then
                        sql = "Select tal_ActualNro from Talonar where tal_Cod='OCN'"
                    Else
                        'si el checkBox provincial esta seleccionado se busca en la base de datos el atributo
                        '"tal_ActualNro y se muestra en txtSaliente y se incrementa en 1
                        If para.Equals("Provincial") Then
                            sql = "Select tal_ActualNro from Talonar where tal_Cod='OCP'"
                        End If
                    End If
                    'conecta con la base de datos y ejecuta la base de datos.
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
                End If
            End If
        End If
        'si en la lista desplegable se selecciona la opcion "Historial Rendicion" activa los distintos GroupBox y las cajas de texto
        'y botones correspondientes.
        If cmbTipoOrden.Text = "Historial Rendicion" Then
            GroupBox4.Visible = True
            GroupBox5.Visible = True
            'Label10.Visible = True
            'txtMora.Visible = True
            'btnCalcular.Visible = True
            GroupBox9.Visible = True
            GroupBox10.Visible = True
            GroupBox11.Visible = True
            GroupBox12.Visible = True
            GroupBox13.Visible = True
            GroupBox7.Visible = True
            GroupBox8.Visible = True
            txtSaliente.Text = ""

        End If
    End Sub

    Private Sub cmbTipoOrden_TextUpdate(sender As Object, e As System.EventArgs) Handles cmbTipoOrden.TextUpdate
        If cmbTipoOrden.SelectedItem = "Rendicion" Then
            GroupBox4.Visible = True
            GroupBox5.Visible = True
        Else
            GroupBox4.Visible = False
            GroupBox5.Visible = False
         
        End If
    End Sub

    Private Sub btnCalcular_Click(sender As System.Object, e As System.EventArgs) Handles btnCalcular.Click
        volverAGenerar()
        'Realiza un recorrido anidado por un lado recorre todas las filas del datagrid 1(datos de la orden)
        'agregando 0 en las columnas "A pagar" y "Reintegrar" y por otro recorre el dataGrid2(Datos de la rendicion)
        ' calcula el total de la columna "monto".
        agregarReintegros()
        'Recorre el dataGrid2 sumando la columna monto y asigna el resultado a la caja de texto "txtMontoReintegro"
        calcularReintegros()

        GroupBox11.Visible = True
        GroupBox12.Visible = True
        GroupBox13.Visible = True
        chkPago.Visible = True
        GroupBox4.Visible = True
        GroupBox5.Visible = True
        btnCalcular.Visible = True
        GroupBox7.Visible = True
        GroupBox8.Visible = True
        GroupBox9.Visible = True
        'txtMora.Visible = True

    End Sub

    Private Sub chkReintegro_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkReintegro.CheckedChanged
        If chkReintegro.Checked Then
            GroupBox7.Visible = True
            GroupBox8.Visible = True
        Else
            GroupBox7.Visible = False
            GroupBox8.Visible = False
        End If
    End Sub

    Private Sub txtCodigo_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtCodigo.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            If txtCodigo.Text <> "" Then
                cmbTipoOrden.Focus()
            End If
        End If
    End Sub

    Private Sub chkPago_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkPago.CheckedChanged
        If chkPago.Checked Then
            GroupBox12.Visible = True
            GroupBox13.Visible = True
        Else
            GroupBox12.Visible = False
            GroupBox13.Visible = False

        End If
    End Sub

    Private Sub chkNac_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkNac.CheckedChanged
        If chkNac.Checked Then
            chkProv.Enabled = False
            para = "Nacional"
        Else
            chkProv.Enabled = True
            txtSaliente.Text = "0"
        End If
    End Sub

    Private Sub chkProv_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkProv.CheckedChanged
        If chkProv.Checked Then
            chkNac.Enabled = False
            para = "Provincial"
        Else
            chkNac.Enabled = True
            txtSaliente.Text = "0"
        End If
    End Sub

#End Region

#Region "Imprimir"

    Public Sub imprimirOrdenCargoCheques()
        Dim origen As String
        If chkNac.Checked Then
            origen = "OCN"
        End If
        If chkProv.Checked Then
            origen = "OCP"
        End If

        sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='ViaticoCargo' "
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
            'Select Case MsgBox("¿Ya existe una orden con este numero desea reemplazarla?", MsgBoxStyle.YesNo, "AVISO")
            'en caso de seleccionar un si
            '    Case MsgBoxResult.Yes
            'Elimina la tupla existente en la base de datos y despues inserta la nueva orden de pago
            'reemplazarImprimirViaticoAutorizado()
            '   Case MsgBoxResult.No
            'MessageBox.Show("Accion cancelada por el usuario", "INFORMACION")
            'End Select
            imprimirOrdenCargoChequesHistorial()

        Else
            Try
                Dim Documento As New Document(PageSize.A4, 60, 5, 35, 5) 'Declaracion del documento
                Dim parrafo As New Paragraph ' Declaracion de un parrafo
                Dim tablademo As New PdfPTable(5) 'declara la tabla con 4 Columnas

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

                sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='ViaticoCargo' or Tipo='Contratos'"
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

                parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                parrafo.Add("ORDEN NRO:" & dameNroHistorial("ViaticoCargo") & "                             ") 'Texto que se insertara
                parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                parrafo.Add("FECHA:" & Date.Now) 'Texto que se insertara

                Documento.Add(parrafo) 'Agrega el parrafo al documento
                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                Documento.Add(New Paragraph(" ")) 'Salto de linea

                tablademo.SetWidthPercentage({170, 100, 80, 80, 140}, PageSize.A4) 'Ajusta el tamaño de cada columna

                tablademo.AddCell(New Paragraph("DESTINATARIO", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                tablademo.AddCell(New Paragraph("CUENTA CORRIENTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                tablademo.AddCell(New Paragraph("NRO DE CHEQUE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                tablademo.AddCell(New Paragraph("IMPORTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                tablademo.AddCell(New Paragraph("RECIBI CONFORME", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                Documento.Add(tablademo) 'Agrega la tabla al documento

                Dim tablademo2 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                tablademo2.SetWidthPercentage({170, 100, 80, 80, 140}, PageSize.A4) 'Ajusta el tamaño de cada columna

                For Each row As DataGridViewRow In DataGridView1.Rows
                    For Each column As DataGridViewColumn In DataGridView1.Columns
                        If column.Index <= 4 Then
                            Dim paraParrrafo As String

                            If IsNumeric(row.Cells(column.Index).Value) And column.Index = 3 Then

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
                Dim GranTotal As Double

                GranTotal = Double.Parse(txtTotalFinal.Text)
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
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                parrafo.Add("DEPTO. TESORERIA") 'Texto que se insertara

                Documento.Add(parrafo) 'Agrega el parrafo al documento
                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                Documento.Close() 'Cierra el documento
                System.Diagnostics.Process.Start("OrdenCargo.pdf") 'Abre el archivo DEMO.PDF
            Catch ex As Exception
                MessageBox.Show("Verifique que no tiene el mismo documento abierto", "AVISO")
            End Try

        End If


    End Sub

    Public Sub impirmirOrdenCargoPrimero()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'se busca la cuasa de emision en la tabla AAOrden se comprueba si existe y obtiene la fecha 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim fechona As String
        'consulta sql que busca en la tabla AAOrdenes las ordenes que tengan al atributo tipo igual viaticoCarto y el numero ingreso
        'concuerde con el numero de la caja de texto txtCodgio, ayudado por el metodo adjuntarAnio
        sql = "Select Concepto,Fecha from AAOrdenNro where Tipo='ViaticoCargo' and NroIngreso=" & adjuntarAnio()
        'se conecta con la base de datos y trae el resultado de la consulta guardando el dato fecha en una variable fecha
        'de una determinada causa de emision de orden de pago
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
        ' se declara una variable origen 
        Dim origen As String

        'se pregunta si el checkBox nacional esta marcado
        'la variable adquiere un string de OCN
        If chkNac.Checked Then
            origen = "OCN"
        End If

        'se pregunta si el checkBox provincial esta marcado
        'la variable adquiere un string de OCN
        If chkProv.Checked Then
            origen = "OCP"
        End If

        'consulta sql en la cual pregunta por el concepto de una determinada cuasa de emision con atributo tipo igual a viatico Cargo
        'sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='ViaticoCargo'"
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
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'De existir una tupla en la talba AAOrdenNro significa que ya se creo una orden de pago anterior
        'asi que se invoca a la funcion impirmirOrdenCargoPrimeroHistorial(), de no ser asi comprueba si la cuasa de emision
        'se cargo correctamente realizando los ajustes necesarios.
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'pregunta si la la variable Bandera es true
        'BANDERA = True
        If BANDERA Then
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Select Case MsgBox("¿La orden que desea imprimir ya tiene una numero de orden cargado desea reemplazar el numero de orden y la fecha de esta orden?", MsgBoxStyle.YesNo, "AVISO")
            '    'en caso de seleccionar un si
            '    Case MsgBoxResult.Yes
            '        'Elimina la tupla existente en la base de datos y despues inserta la nueva orden de pago
            '        reemplazarImprimirCargoPrimero()
            '    Case MsgBoxResult.No
            '        MessageBox.Show("Accion cancelada por el usuario", "INFORMACION")
            'End Select
            ' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            MessageBox.Show("YA EXISTE UNA ORDEN CON ESTE NUMERO")
            'invoca el metodo imprimir orden cargo primero
            'impirmirOrdenCargoPrimeroHistorial()
        Else

            'se declara la variable nroCargo
            Dim nroCargo As Integer
            'asignar el valor de la caja de texto txtSaliente que es la nueva orden de cargo.
            nroCargo = txtSaliente.Text
            'se declara la varible nro
            Dim nro As String
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                'pregunta si nroCargo tiene una longitud de 1
                If nroCargo.ToString.Length = 1 Then
                    'agrega 7 ceros  y despues adjunta el nroCargo
                    nro = "0000000" & nroCargo
                End If
                'pregunta si nroCargo tiene una longitud de 2
                If nroCargo.ToString.Length = 2 Then
                    'agrega 6 ceros  y despues adjunta el nroCargo
                    nro = "000000" & nroCargo
                End If
                'pregunta si nroCargo tiene una longitud de 3
                If nroCargo.ToString.Length = 3 Then
                    'agrega 5 ceros  y despues adjunta el nroCargo
                    nro = "00000" & nroCargo
                End If
                'pregunta si nroCargo tiene una longitud de 4
                If nroCargo.ToString.Length = 4 Then
                    'agrega 4 ceros  y despues adjunta el nroCargo
                    nro = "0000" & nroCargo
                End If
                'pregunta si nroCargo tiene una longitud de 5
                If nroCargo.ToString.Length = 5 Then
                    'agrega 3 ceros  y despues adjunta el nroCargo
                    nro = "000" & nroCargo
                End If
                'pregunta si nroCargo tiene una longitud de 6
                If nroCargo.ToString.Length = 6 Then
                    'agrega 2 ceros  y despues adjunta el nroCargo
                    nro = "00" & nroCargo
                End If
                'pregunta si nroCargo tiene una longitud de 7
                If nroCargo.ToString.Length = 7 Then
                    'agrega 1 ceros  y despues adjunta el nroCargo
                    nro = "0" & nroCargo
                End If
                'pregunta si nroCargo tiene una longitud de 8
                If nroCargo.ToString.Length = 8 Then
                    'asigna el nroCargo a la variable nro
                    nro = nroCargo
                End If
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'se actualiza la tabla Talonar
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'consulta sql que actualiza la tabla Talonar el atributo tal_ActualNro con la variable nro y el atributo tal_Cod
                'con la variable origen.
                sql = "update Talonar set tal_ActualNro='" & nro & "' where tal_Cod='" & origen & "'"
                'se conecta con la base de datos  y ejecuta la consulta.
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()
                        conn.Open()
                        cmd4.CommandText = sql
                        cmd4.ExecuteScalar()
                    End Using
                End Using
                MessageBox.Show("El talonario se actualizo de manera correcta", "EXITO")
            Catch ex As Exception
                'si se genera una excepcion muestra el siguiente mensaje.
                MessageBox.Show("Un error provoco que no se actualice el Talonario de ordenes de pago, se recomienda que lo haga de manera manual", "AVISO")
            End Try

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'pregunta si la funcion almacenarOrden devuelve un valor verdadero
            'es decir si pudo insertar una tupla con exito, luego se crea el ecabezado del documento PDF
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If almacenarOrden(nroCargo, origen) Then
                Try
                    'se crea una variable de tipo document
                    Dim Documento As New Document(PageSize.LEGAL, 60, 5, 35, 5)

                    'Declaracion de un parrafo
                    Dim parrafo As New Paragraph

                    pdf.PdfWriter.GetInstance(Documento, New FileStream("OrdenCargo.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF
                    'Abre documento para su escritura
                    Documento.Open()

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
                    'NRO DE ORDEN DE CARGO

                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("ORDEN NRO:" & nroCargo & "                             ") 'Texto que se insertara
                    parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("FECHA:" & Date.Now) 'Texto que se insertara

                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Se recorre el DataGrid y se inserta los valores en la tabla y luego al pdf
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Salto de linea
                    Documento.Add(New Paragraph(" "))
                    'declara la tabla con la cantidad de columna del DataGrid de la ventana
                    Dim tablademo As New PdfPTable(DataGridView1.Columns.Count)
                    'tablademo.SetWidthPercentage({80}, PageSize.A4)

                    tablademo.TotalWidth() = 100
                    tablademo.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                    'se realiza un recorrido por columnas 
                    For Each miCol As DataGridViewColumn In DataGridView1.Columns
                        'si el idex de la columna de es menor a la cantidad de columna
                        If miCol.Index >= 0 And miCol.Index < DataGridView1.ColumnCount - 1 Then
                            'agregar una celdda 
                            tablademo.AddCell(New Paragraph(miCol.Name, FontFactory.GetFont("Arial", 10)))
                        Else
                            'si llega a la cantidad de las columnas de el datagrid se agrega una columna total
                            tablademo.AddCell(New Paragraph("Total", FontFactory.GetFont("Arial", 10)))
                        End If
                    Next
                    'agreaga una tabla al document
                    Documento.Add(tablademo) 'Agrega la tabla al documento
                    'declara la tabla con la cantidad de columnas del dataGrid
                    Dim tablademo2 As New PdfPTable(DataGridView1.Columns.Count)
                    'tablademo2.SetWidthPercentage({80}, PageSize.A4) 'Ajusta el tamaño de cada columna
                    tablademo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                    'se realiza con recordo por filas en el datagrid1
                    For Each row As DataGridViewRow In DataGridView1.Rows
                        For Each column As DataGridViewColumn In DataGridView1.Columns
                            'pregunta si el indice de la columna es menor o igual  al tamaño total de la columna
                            If column.Index <= DataGridView1.Columns.Count Then
                                'se declara una variable paraParrafo
                                Dim paraParrrafo As String
                                'pregunta si el valor de la celda es numerica y el indice no es igual a 1
                                If IsNumeric(row.Cells(column.Index).Value) And Not column.Index = 1 Then
                                    'a la variable paraParrao se le asigne el valor de la celda con un cierto formato numerico
                                    paraParrrafo = Format(row.Cells(column.Index).Value, "###,##0.00")
                                    Dim miCelda2 As New PdfPCell
                                    ' se agrega el valor a la celda.
                                    parrafo.Add(paraParrrafo)
                                    miCelda2.AddElement(parrafo)
                                    'Alinea el parrafo para que sea centrado o justificado
                                    miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                                    tablademo2.AddCell(miCelda2)
                                    parrafo.Clear()
                                Else
                                    'si el valor de la celda no es numerico o el indice es igual a 1
                                    'se asigna a la variable paraParrafo el contenido de la celda
                                    paraParrrafo = row.Cells(column.Index).Value
                                    'Alinea el parrafo para que sea centrado o justificado
                                    parrafo.Alignment = Element.ALIGN_RIGHT
                                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                    'Texto que se insertara
                                    parrafo.Add(paraParrrafo)
                                    'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo2.AddCell(New Paragraph(parrafo))
                                    'se limpia la varialbe parrafo
                                    parrafo.Clear()
                                End If
                            End If
                        Next
                    Next
                    'agrega la tabla al documento
                    Documento.Add(tablademo2)
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Dim tablademo3 As New PdfPTable(DataGridView1.Columns.Count) 'declara la tabla con 4 Columnas
                    'tablademo3.SetWidthPercentage({100, 70, 130, 100, 130, 70}, PageSize.A4) 'Ajusta el tamaño de cada columna
                    'agregar una celda Total, es la ultima fila que contendra los totales
                    tablademo3.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                    Dim paraTotal As String
                    'pregunta se la cantidad de columnas que tiene el DataGrid es igual a  4
                    If DataGridView1.Columns.Count = 4 Then
                        'agrega a la variable pdf una celda con el valor de la caja de texto txtDia 
                        tablademo3.AddCell(New Paragraph(txtDia.Text)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                        'Dim paraComb As String
                        'Dim miComb As Double
                        'miComb = Double.Parse(txtCombustible.Text)
                        'paraComb = Format(miComb, "###,##0.00")
                        'agrega una variable pdfCell
                        Dim CeldaDia As New PdfPCell
                        'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Alignment = Element.ALIGN_RIGHT
                        'Asigan fuente
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT)
                        'parrafo.Add(paraComb)
                        CeldaDia.AddElement(parrafo)
                        tablademo3.AddCell(CeldaDia) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                        parrafo.Clear()
                        'se declara la variable paraPeaje de tipo String
                        Dim paraPeaje As String
                        'declaro la variable miPeaje
                        Dim miPeajes As Double
                        'miPeajes = Double.Parse(txtPeaje.Text)
                        'paraPeaje = Format(miPeajes, "###,##0.00")
                        'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                        'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                        'parrafo.Add(paraPeaje)
                        'Dim celdaPeaje As New PdfPCell
                        'celdaPeaje.AddElement(parrafo)
                        'tablademo3.AddCell(celdaPeaje) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                        'parrafo.Clear()
                        'Dim miViaticos As Double
                        'Dim paraViatico As String
                        'miViaticos = Double.Parse(txtViaticos.Text)
                        'paraViatico = Format(miViaticos, "###,##0.00")
                        'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                        'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                        'parrafo.Add(paraViatico)
                        'Dim celdaViatico As New PdfPCell
                        'celdaViatico.AddElement(parrafo)
                        'tablademo3.AddCell(celdaViatico) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                        'parrafo.Clear()
                        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                        'se declara la variable double
                        Dim miFinal0 As Double
                        'asigna a miFinal10 el valor de txtFinal
                        miFinal0 = Double.Parse(txtTotalFinal.Text)

                        paraTotal = Format(miFinal0, "###,##0.00")
                        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                        parrafo.Add(paraTotal)
                        Dim CeldaFinal0 As New PdfPCell
                        CeldaFinal0.AddElement(parrafo)
                        tablademo3.AddCell(CeldaFinal0) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                        parrafo.Clear()
                        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                        Dim miFinal As Double
                        miFinal = Double.Parse(txtTotalFinal.Text)
                        paraTotal = Format(miFinal, "###,##0.00")
                        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                        parrafo.Add(paraTotal)
                        Dim CeldaFinal As New PdfPCell
                        CeldaFinal.AddElement(parrafo)
                        tablademo3.AddCell(CeldaFinal) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                        parrafo.Clear()
                    Else
                        'si DataGridView1 tiena mas de 4 culumnas
                        If DataGridView1.Columns.Count > 4 Then
                            'asigna a la variable tablademo3 el valor en la caja de texto dia
                            tablademo3.AddCell(New Paragraph(txtDia.Text)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                            'se declara la variable paraComb
                            Dim paraComb As String
                            'se declara la variable miComb
                            Dim miComb As Double
                            'se asigna a la variable miComb el valor de la caja de texto txtCombustible
                            miComb = Double.Parse(txtCombustible.Text)
                            'se le da formato a la celda
                            paraComb = Format(miComb, "###,##0.00")
                            'se crea la variable CeldaDia
                            Dim CeldaDia As New PdfPCell
                            'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Alignment = Element.ALIGN_RIGHT
                            'Asigan fuente
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT)
                            parrafo.Add(paraComb)
                            'agrea el parrafo a la variable CeldaDia
                            CeldaDia.AddElement(parrafo)
                            'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                            tablademo3.AddCell(CeldaDia)
                            parrafo.Clear()
                            'se declara las variables paraPeaje
                            Dim paraPeaje As String
                            'se declara la varaible miPeaje
                            Dim miPeajes As Double
                            'se asigna el valor de txtPeaje miPeajes
                            miPeajes = Double.Parse(txtPeaje.Text)
                            'se asigna el formato a la variable paraPeaje
                            paraPeaje = Format(miPeajes, "###,##0.00")
                            'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Alignment = Element.ALIGN_RIGHT
                            'Asigan fuente
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT)
                            parrafo.Add(paraPeaje)
                            Dim celdaPeaje As New PdfPCell
                            celdaPeaje.AddElement(parrafo)
                            'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                            tablademo3.AddCell(celdaPeaje)
                            parrafo.Clear()
                            'se declara
                            Dim miViaticos As Double
                            Dim paraViatico As String
                            'asigna el valor en la caja de texto txtViatico en la variable miViatico
                            miViaticos = Double.Parse(txtViaticos.Text)
                            'se le da formato
                            paraViatico = Format(miViaticos, "###,##0.00")
                            parrafo.Alignment = Element.ALIGN_RIGHT
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                            parrafo.Add(paraViatico)
                            Dim celdaViatico As New PdfPCell
                            celdaViatico.AddElement(parrafo)
                            tablademo3.AddCell(celdaViatico) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                            parrafo.Clear()

                            Dim miFinal As Double
                            miFinal = Double.Parse(txtTotalFinal.Text)
                            paraTotal = Format(miFinal, "###,##0.00")
                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                            parrafo.Add(paraTotal)
                            Dim CeldaFinal As New PdfPCell
                            CeldaFinal.AddElement(parrafo)
                            tablademo3.AddCell(CeldaFinal) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                            parrafo.Clear()
                        Else 'si no es mayor a 4 columnas ni es menor a el tamaño de la columna
                            tablademo3.AddCell(New Paragraph(txtDia.Text, FontFactory.GetFont("Arial", 9))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                            '
                            Dim misOtrosGastos As Double
                            misOtrosGastos = Double.Parse(txtOtrosGastos.Text)
                            Dim praOtros As String
                            praOtros = Format(misOtrosGastos, "###,##0.00")
                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                            parrafo.Add(praOtros)
                            Dim otrosGastos As New PdfPCell
                            otrosGastos.AddElement(parrafo)
                            tablademo3.AddCell(otrosGastos) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                            parrafo.Clear()
                            Dim miFinal As Double
                            miFinal = Double.Parse(txtTotalFinal.Text)
                            paraTotal = Format(miFinal, "###,##0.00")
                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                            parrafo.Add(paraTotal)
                            Dim CeldaFinal As New PdfPCell
                            CeldaFinal.AddElement(parrafo)
                            tablademo3.AddCell(CeldaFinal) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                            parrafo.Clear()
                        End If

                    End If

                    Documento.Add(tablademo3) 'Agrega la tabla al documento
                    'asigna la variable 
                    Documento.Add(New Paragraph("SON PESOS: " & Numalet.ToCardinal(txtTotalFinal.Text))) 'Salto de linea
                    Documento.Add(New Paragraph("OTORGADO EN CONCEPTO DE ANTICIPO DE FONDO: ")) 'Salto de linea
                    Dim misCargos As String

                    For Each column As DataGridViewColumn In DataGridView1.Columns
                        If column.Index > 1 And column.Index < DataGridView1.ColumnCount - 2 Then
                            misCargos = misCargos & column.Name & ", "
                        Else
                            If column.Index = (DataGridView1.ColumnCount - 2) Then
                                misCargos = misCargos & column.Name & " a " & txtDestino.Text
                            End If

                        End If


                    Next
                    Documento.Add(New Paragraph(misCargos)) 'Salto de linea

                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph("V'B' DIRECTOR DE SERVICIO ADMINISTRATIVO ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_JUSTIFIED) 'Asigan fuente
                    parrafo.Add("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  ") 'Salto de linea
                    Documento.Add(parrafo) 'Salto de linea
                    parrafo.Clear()

                    Documento.Close() 'Cierra el documento
                    System.Diagnostics.Process.Start("OrdenCargo.pdf") 'Abre el archivo DEMO.PDF

                Catch ex As Exception
                    MessageBox.Show("Compruebe que el documento no esta abierto previamente", "AVISO")
                End Try
            Else
            End If
        End If
    End Sub

    Public Sub reemplazarImprimirCargoPrimero()
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
        reemplazarCargo()
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
    Public Sub reemplazarCargo()
        Dim origen As String
        Dim fechona As String
        'se pregunta si el checkBox nacional esta marcado
        'la variable adquiere un string de OCN
        If chkNac.Checked Then
            origen = "OCN"
        End If

        'se pregunta si el checkBox provincial esta marcado
        'la variable adquiere un string de OCN
        If chkProv.Checked Then
            origen = "OCP"
        End If
        'se declara la variable nroCargo
        Dim nroCargo As Integer
        'asignar el valor de la caja de texto txtSaliente que es la nueva orden de cargo.
        nroCargo = txtSaliente.Text
        'se declara la varible nro
        Dim nro As String
        Try
            'pregunta si nroCargo tiene una longitud de 1
            If nroCargo.ToString.Length = 1 Then
                'agrega 7 ceros  y despues adjunta el nroCargo
                nro = "0000000" & nroCargo
            End If
            'pregunta si nroCargo tiene una longitud de 2
            If nroCargo.ToString.Length = 2 Then
                'agrega 6 ceros  y despues adjunta el nroCargo
                nro = "000000" & nroCargo
            End If
            'pregunta si nroCargo tiene una longitud de 3
            If nroCargo.ToString.Length = 3 Then
                'agrega 5 ceros  y despues adjunta el nroCargo
                nro = "00000" & nroCargo
            End If
            'pregunta si nroCargo tiene una longitud de 4
            If nroCargo.ToString.Length = 4 Then
                'agrega 4 ceros  y despues adjunta el nroCargo
                nro = "0000" & nroCargo
            End If
            'pregunta si nroCargo tiene una longitud de 5
            If nroCargo.ToString.Length = 5 Then
                'agrega 3 ceros  y despues adjunta el nroCargo
                nro = "000" & nroCargo
            End If
            'pregunta si nroCargo tiene una longitud de 6
            If nroCargo.ToString.Length = 6 Then
                'agrega 2 ceros  y despues adjunta el nroCargo
                nro = "00" & nroCargo
            End If
            'pregunta si nroCargo tiene una longitud de 7
            If nroCargo.ToString.Length = 7 Then
                'agrega 1 ceros  y despues adjunta el nroCargo
                nro = "0" & nroCargo
            End If
            'pregunta si nroCargo tiene una longitud de 8
            If nroCargo.ToString.Length = 8 Then
                'asigna el nroCargo a la variable nro
                nro = nroCargo
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'se actualiza la tabla Talonar
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'consulta sql que actualiza la tabla Talonar el atributo tal_ActualNro con la variable nro y el atributo tal_Cod
            'con la variable origen.
            sql = "update Talonar set tal_ActualNro='" & nro & "' where tal_Cod='" & origen & "'"
            'se conecta con la base de datos  y ejecuta la consulta.
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql
                    cmd4.ExecuteScalar()
                End Using
            End Using
            MessageBox.Show("El talonario se actualizo de manera correcta", "EXITO")
        Catch ex As Exception
            'si se genera una excepcion muestra el siguiente mensaje.
            MessageBox.Show("Un error provoco que no se actualice el Talonario de ordenes de pago, se recomienda que lo haga de manera manual", "AVISO")
        End Try
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'pregunta si la funcion almacenarOrden devuelve un valor verdadero
        'es decir si pudo insertar una tupla con exito, luego se crea el ecabezado del documento PDF
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If almacenarOrden(nroCargo, origen) Then
            Try
                'se crea una variable de tipo document
                Dim Documento As New Document(PageSize.LEGAL, 60, 5, 35, 5)

                'Declaracion de un parrafo
                Dim parrafo As New Paragraph

                pdf.PdfWriter.GetInstance(Documento, New FileStream("OrdenCargo.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF
                'Abre documento para su escritura
                Documento.Open()

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
                'NRO DE ORDEN DE CARGO

                parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                parrafo.Add("ORDEN NRO:" & nroCargo & "                             ") 'Texto que se insertara
                parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                parrafo.Add("FECHA:" & Date.Now) 'Texto que se insertara

                Documento.Add(parrafo) 'Agrega el parrafo al documento
                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'Se recorre el DataGrid y se inserta los valores en la tabla y luego al pdf
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'Salto de linea
                Documento.Add(New Paragraph(" "))
                'declara la tabla con la cantidad de columna del DataGrid de la ventana
                Dim tablademo As New PdfPTable(DataGridView1.Columns.Count)
                'tablademo.SetWidthPercentage({80}, PageSize.A4)

                tablademo.TotalWidth() = 100
                tablademo.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                'se realiza un recorrido por columnas 
                For Each miCol As DataGridViewColumn In DataGridView1.Columns
                    'si el idex de la columna de es menor a la cantidad de columna
                    If miCol.Index >= 0 And miCol.Index < DataGridView1.ColumnCount - 1 Then
                        'agregar una celdda 
                        tablademo.AddCell(New Paragraph(miCol.Name, FontFactory.GetFont("Arial", 10)))
                    Else
                        'si llega a la cantidad de las columnas de el datagrid se agrega una columna total
                        tablademo.AddCell(New Paragraph("Total", FontFactory.GetFont("Arial", 10)))
                    End If
                Next
                'agreaga una tabla al document
                Documento.Add(tablademo) 'Agrega la tabla al documento
                'declara la tabla con la cantidad de columnas del dataGrid
                Dim tablademo2 As New PdfPTable(DataGridView1.Columns.Count)
                'tablademo2.SetWidthPercentage({80}, PageSize.A4) 'Ajusta el tamaño de cada columna
                tablademo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                'se realiza con recordo por filas en el datagrid1
                For Each row As DataGridViewRow In DataGridView1.Rows
                    For Each column As DataGridViewColumn In DataGridView1.Columns
                        'pregunta si el indice de la columna es menor o igual  al tamaño total de la columna
                        If column.Index <= DataGridView1.Columns.Count Then
                            'se declara una variable paraParrafo
                            Dim paraParrrafo As String
                            'pregunta si el valor de la celda es numerica y el indice no es igual a 1
                            If IsNumeric(row.Cells(column.Index).Value) And Not column.Index = 1 Then
                                'a la variable paraParrao se le asigne el valor de la celda con un cierto formato numerico
                                paraParrrafo = Format(row.Cells(column.Index).Value, "###,##0.00")
                                Dim miCelda2 As New PdfPCell
                                ' se agrega el valor a la celda.
                                parrafo.Add(paraParrrafo)
                                miCelda2.AddElement(parrafo)
                                'Alinea el parrafo para que sea centrado o justificado
                                miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                                tablademo2.AddCell(miCelda2)
                                parrafo.Clear()
                            Else
                                'si el valor de la celda no es numerico o el indice es igual a 1
                                'se asigna a la variable paraParrafo el contenido de la celda
                                paraParrrafo = row.Cells(column.Index).Value
                                'Alinea el parrafo para que sea centrado o justificado
                                parrafo.Alignment = Element.ALIGN_RIGHT
                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                'Texto que se insertara
                                parrafo.Add(paraParrrafo)
                                'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo2.AddCell(New Paragraph(parrafo))
                                'se limpia la varialbe parrafo
                                parrafo.Clear()
                            End If
                        End If
                    Next
                Next
                'agrega la tabla al documento
                Documento.Add(tablademo2)
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Dim tablademo3 As New PdfPTable(DataGridView1.Columns.Count) 'declara la tabla con 4 Columnas
                'tablademo3.SetWidthPercentage({100, 70, 130, 100, 130, 70}, PageSize.A4) 'Ajusta el tamaño de cada columna
                'agregar una celda Total, es la ultima fila que contendra los totales
                tablademo3.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                Dim paraTotal As String
                'pregunta se la cantidad de columnas que tiene el DataGrid es igual a  4
                If DataGridView1.Columns.Count = 4 Then
                    'agrega a la variable pdf una celda con el valor de la caja de texto txtDia 
                    tablademo3.AddCell(New Paragraph(txtDia.Text)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                    'Dim paraComb As String
                    'Dim miComb As Double
                    'miComb = Double.Parse(txtCombustible.Text)
                    'paraComb = Format(miComb, "###,##0.00")
                    'agrega una variable pdfCell
                    Dim CeldaDia As New PdfPCell
                    'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Alignment = Element.ALIGN_RIGHT
                    'Asigan fuente
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT)
                    'parrafo.Add(paraComb)
                    CeldaDia.AddElement(parrafo)
                    tablademo3.AddCell(CeldaDia) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    parrafo.Clear()
                    'se declara la variable paraPeaje de tipo String
                    Dim paraPeaje As String
                    'declaro la variable miPeaje
                    Dim miPeajes As Double
                    'miPeajes = Double.Parse(txtPeaje.Text)
                    'paraPeaje = Format(miPeajes, "###,##0.00")
                    'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                    'parrafo.Add(paraPeaje)
                    'Dim celdaPeaje As New PdfPCell
                    'celdaPeaje.AddElement(parrafo)
                    'tablademo3.AddCell(celdaPeaje) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    'parrafo.Clear()
                    'Dim miViaticos As Double
                    'Dim paraViatico As String
                    'miViaticos = Double.Parse(txtViaticos.Text)
                    'paraViatico = Format(miViaticos, "###,##0.00")
                    'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                    'parrafo.Add(paraViatico)
                    'Dim celdaViatico As New PdfPCell
                    'celdaViatico.AddElement(parrafo)
                    'tablademo3.AddCell(celdaViatico) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    'parrafo.Clear()
                    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                    'se declara la variable double
                    Dim miFinal0 As Double
                    'asigna a miFinal10 el valor de txtFinal
                    miFinal0 = Double.Parse(txtTotalFinal.Text)

                    paraTotal = Format(miFinal0, "###,##0.00")
                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                    parrafo.Add(paraTotal)
                    Dim CeldaFinal0 As New PdfPCell
                    CeldaFinal0.AddElement(parrafo)
                    tablademo3.AddCell(CeldaFinal0) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    parrafo.Clear()
                    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                    Dim miFinal As Double
                    miFinal = Double.Parse(txtTotalFinal.Text)
                    paraTotal = Format(miFinal, "###,##0.00")
                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                    parrafo.Add(paraTotal)
                    Dim CeldaFinal As New PdfPCell
                    CeldaFinal.AddElement(parrafo)
                    tablademo3.AddCell(CeldaFinal) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    parrafo.Clear()
                Else
                    'si DataGridView1 tiena mas de 4 culumnas
                    If DataGridView1.Columns.Count > 4 Then
                        'asigna a la variable tablademo3 el valor en la caja de texto dia
                        tablademo3.AddCell(New Paragraph(txtDia.Text)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                        'se declara la variable paraComb
                        Dim paraComb As String
                        'se declara la variable miComb
                        Dim miComb As Double
                        'se asigna a la variable miComb el valor de la caja de texto txtCombustible
                        miComb = Double.Parse(txtCombustible.Text)
                        'se le da formato a la celda
                        paraComb = Format(miComb, "###,##0.00")
                        'se crea la variable CeldaDia
                        Dim CeldaDia As New PdfPCell
                        'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Alignment = Element.ALIGN_RIGHT
                        'Asigan fuente
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT)
                        parrafo.Add(paraComb)
                        'agrea el parrafo a la variable CeldaDia
                        CeldaDia.AddElement(parrafo)
                        'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                        tablademo3.AddCell(CeldaDia)
                        parrafo.Clear()
                        'se declara las variables paraPeaje
                        Dim paraPeaje As String
                        'se declara la varaible miPeaje
                        Dim miPeajes As Double
                        'se asigna el valor de txtPeaje miPeajes
                        miPeajes = Double.Parse(txtPeaje.Text)
                        'se asigna el formato a la variable paraPeaje
                        paraPeaje = Format(miPeajes, "###,##0.00")
                        'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Alignment = Element.ALIGN_RIGHT
                        'Asigan fuente
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT)
                        parrafo.Add(paraPeaje)
                        Dim celdaPeaje As New PdfPCell
                        celdaPeaje.AddElement(parrafo)
                        'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                        tablademo3.AddCell(celdaPeaje)
                        parrafo.Clear()
                        'se declara
                        Dim miViaticos As Double
                        Dim paraViatico As String
                        'asigna el valor en la caja de texto txtViatico en la variable miViatico
                        miViaticos = Double.Parse(txtViaticos.Text)
                        'se le da formato
                        paraViatico = Format(miViaticos, "###,##0.00")
                        parrafo.Alignment = Element.ALIGN_RIGHT
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                        parrafo.Add(paraViatico)
                        Dim celdaViatico As New PdfPCell
                        celdaViatico.AddElement(parrafo)
                        tablademo3.AddCell(celdaViatico) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                        parrafo.Clear()

                        Dim miFinal As Double
                        miFinal = Double.Parse(txtTotalFinal.Text)
                        paraTotal = Format(miFinal, "###,##0.00")
                        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                        parrafo.Add(paraTotal)
                        Dim CeldaFinal As New PdfPCell
                        CeldaFinal.AddElement(parrafo)
                        tablademo3.AddCell(CeldaFinal) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                        parrafo.Clear()
                    Else 'si no es mayor a 4 columnas ni es menor a el tamaño de la columna
                        tablademo3.AddCell(New Paragraph(txtDia.Text, FontFactory.GetFont("Arial", 9))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                        '
                        Dim misOtrosGastos As Double
                        misOtrosGastos = Double.Parse(txtOtrosGastos.Text)
                        Dim praOtros As String
                        praOtros = Format(misOtrosGastos, "###,##0.00")
                        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                        parrafo.Add(praOtros)
                        Dim otrosGastos As New PdfPCell
                        otrosGastos.AddElement(parrafo)
                        tablademo3.AddCell(otrosGastos) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                        parrafo.Clear()
                        Dim miFinal As Double
                        miFinal = Double.Parse(txtTotalFinal.Text)
                        paraTotal = Format(miFinal, "###,##0.00")
                        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                        parrafo.Add(paraTotal)
                        Dim CeldaFinal As New PdfPCell
                        CeldaFinal.AddElement(parrafo)
                        tablademo3.AddCell(CeldaFinal) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                        parrafo.Clear()
                    End If

                End If

                Documento.Add(tablademo3) 'Agrega la tabla al documento
                'asigna la variable 
                Documento.Add(New Paragraph("SON PESOS: " & Numalet.ToCardinal(txtTotalFinal.Text))) 'Salto de linea
                Documento.Add(New Paragraph("OTORGADO EN CONCEPTO DE ANTICIPO DE FONDO: ")) 'Salto de linea
                Dim misCargos As String

                For Each column As DataGridViewColumn In DataGridView1.Columns
                    If column.Index > 1 And column.Index < DataGridView1.ColumnCount - 2 Then
                        misCargos = misCargos & column.Name & ", "
                    Else
                        If column.Index = (DataGridView1.ColumnCount - 2) Then
                            misCargos = misCargos & column.Name & " a " & txtDestino.Text
                        End If

                    End If


                Next
                Documento.Add(New Paragraph(misCargos)) 'Salto de linea

                Documento.Add(New Paragraph(" ")) 'Salto de linea
                Documento.Add(New Paragraph("V'B' DIRECTOR DE SERVICIO ADMINISTRATIVO ")) 'Salto de linea
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_JUSTIFIED) 'Asigan fuente
                parrafo.Add("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  ") 'Salto de linea
                Documento.Add(parrafo) 'Salto de linea
                parrafo.Clear()

                Documento.Close() 'Cierra el documento
                System.Diagnostics.Process.Start("OrdenCargo.pdf") 'Abre el archivo DEMO.PDF

            Catch ex As Exception
                MessageBox.Show("Compruebe que el documento no esta abierto previamente", "AVISO")
            End Try
        Else
        End If
        'End If
    End Sub
    Public Sub imprimirRendicion()
        Dim origen As String

        If chkNac.Checked Then
            origen = "RVN"
        End If
        If chkProv.Checked Then
            origen = "RVI"
        End If
        Dim nroCargo As String
        Dim nro As String
        Dim subgasto As Double
        'Trae el ultimo numero  de la tabla Talonar  lo suma uno y lo muestra en la caja de texto n
        sql = "Select tal_ActualNro from Talonar where tal_Cod='" & origen & "'"
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
        Dim nro2 As String
        Try
            If nroCargo.ToString.Length = 1 Then
                nro2 = "0000000" & nroCargo
            End If
            If nroCargo.ToString.Length = 2 Then
                nro2 = "000000" & nroCargo
            End If
            If nroCargo.ToString.Length = 3 Then
                nro2 = "00000" & nroCargo
            End If
            If nroCargo.ToString.Length = 4 Then
                nro2 = "0000" & nroCargo
            End If
            If nroCargo.ToString.Length = 5 Then
                nro2 = "000" & nroCargo
            End If
            If nroCargo.ToString.Length = 6 Then
                nro2 = "00" & nroCargo
            End If
            If nroCargo.ToString.Length = 7 Then
                nro2 = "0" & nroCargo
            End If
            If nroCargo.ToString.Length = 8 Then
                nro2 = nroCargo
            End If
            sql = "update Talonar set tal_ActualNro='" & nro2 & "' where tal_Cod='" & origen & "'"
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
        If almacenarOrdenRendicion(nroCargo) Then 'si inserto correctamente la orden de rendicion
            If AlmacenarRendicion() Then
                Try
                    Dim Documento As New Document()
                    Dim parrafo As New Paragraph

                    Documento.SetPageSize(iTextSharp.text.PageSize.A4.Rotate())
                    pdf.PdfWriter.GetInstance(Documento, New FileStream("Rendicion.pdf", FileMode.Create))

                    Documento.Open()

                    If DataGridView1.Columns.Count = 9 Then

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
                            Documento.Add(New Paragraph("Fecha de Impresion:" & Date.Now)) 'Salto de linea
                            Documento.Add(New Paragraph(" ")) 'Salto de linea

                            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                            parrafo.Add("ORDEN NRO:" & dameNroHistorial("ViaticoCargo") & "  ")

                            parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                            parrafo.Add("NRO DE ENTRADA: " & txtCodigo.Text & "                         ") 'Texto que se insertara

                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                            parrafo.Add("FECHA SALIDA: " & dtSalida.Text) 'Texto que se insertara

                            Documento.Add(parrafo) 'Agrega el parrafo al documento
                            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                            Documento.Add(New Paragraph(" ")) 'Salto de linea

                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                            parrafo.Add("FECHA LLEGADA:" & dtLlegada.Text) 'Texto que se insertara

                            Documento.Add(parrafo) 'Agrega el parrafo al documento
                            parrafo.Clear()

                            Documento.Add(New Paragraph(" ")) 'Salto de linea

                            'FIN DEL ENCABEZADO

                            'ENCABEZADO DE LA TABLA 

                            Dim tablademo As New PdfPTable(8) 'declara la tabla con 4 Columnas
                            tablademo.SetWidthPercentage({100, 40, 70, 40, 70, 70, 70, 70}, PageSize.A4)
                            tablademo.HorizontalAlignment = Element.ALIGN_LEFT

                            tablademo.AddCell(New Paragraph("NOMBRE        ", FontFactory.GetFont("Arial", 12)))
                            tablademo.AddCell(New Paragraph("DIAS", FontFactory.GetFont("Arial", 12)))
                            tablademo.AddCell(New Paragraph("COMBUSTIBLE", FontFactory.GetFont("Arial", 12)))
                            tablademo.AddCell(New Paragraph("PEAJES", FontFactory.GetFont("Arial", 12)))
                            tablademo.AddCell(New Paragraph("VIATICOS", FontFactory.GetFont("Arial", 12)))
                            tablademo.AddCell(New Paragraph("DEVENGADO", FontFactory.GetFont("Arial", 12)))
                            tablademo.AddCell(New Paragraph("A PAGAR", FontFactory.GetFont("Arial", 12)))
                            tablademo.AddCell(New Paragraph("A REINTEGRAR", FontFactory.GetFont("Arial", 12)))


                            Documento.Add(tablademo) 'Agrega la tabla al documento

                            'FIN DEL ENCABEZADO DE LA TABLA

                            'CUERPO DE LA TABLA

                            Dim tablademo2 As New PdfPTable(8) 'declara la tabla con 4 Columnas
                            tablademo2.SetWidthPercentage({100, 40, 70, 40, 70, 70, 70, 70}, PageSize.A4) 'Ajusta el tamaño de cada columna
                            tablademo2.HorizontalAlignment = Element.ALIGN_LEFT
                            For Each column As DataGridViewColumn In DataGridView1.Columns
                                If column.Index <= 8 Then
                                    Dim paraParrrafo As String
                                    If IsNumeric(row.Cells(column.Index).Value) And Not column.Index = 1 Then
                                        paraParrrafo = Format(row.Cells(column.Index).Value, "###,##0.00")
                                        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                    Else
                                        If IsNumeric(row.Cells(column.Index).Value) And column.Index = 1 Then
                                            paraParrrafo = row.Cells(column.Index).Value
                                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                        Else
                                            paraParrrafo = row.Cells(column.Index).Value
                                            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                        End If

                                    End If
                                    parrafo.Add(paraParrrafo) 'Texto que se insertara
                                    Dim celda100 As New PdfPCell
                                    celda100.AddElement(parrafo)
                                    tablademo2.AddCell(celda100) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()
                                End If

                            Next
                            Documento.Add(tablademo2)
                            Dim tablademo3 As New PdfPTable(8) 'declara la tabla con 4 Columnas

                            tablademo3.SetWidthPercentage({100, 40, 70, 40, 70, 70, 70, 70}, PageSize.A4) 'Ajusta el tamaño de cada columna
                            tablademo3.HorizontalAlignment = Element.ALIGN_LEFT

                            tablademo3.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

                            Dim myDay As Integer
                            Dim praZo As String
                            myDay = row.Cells(1).Value.ToString
                            praZo = myDay
                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                            parrafo.Add(praZo) 'Texto que se insertara
                            Dim CELDA89 As New PdfPCell
                            CELDA89.AddElement(parrafo)
                            tablademo3.AddCell(CELDA89) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            parrafo.Clear()

                            For i As Integer = 2 To DataGridView1.Columns.Count - 1
                                If row.Cells(i).Value = Nothing Then
                                    Dim paraP As String
                                    Dim praD As Double
                                    praD = 0
                                    paraP = Format(praD, "###,##0.00")

                                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                    parrafo.Add(paraP) 'Texto que se insertara
                                    Dim CELDA As New PdfPCell
                                    CELDA.AddElement(parrafo)
                                    tablademo3.AddCell(CELDA) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()
                                Else

                                    Dim paraParrrafo As String
                                    If IsNumeric(row.Cells(i).Value) Then
                                        paraParrrafo = Format(row.Cells(i).Value, "###,##0.00")

                                    Else
                                        paraParrrafo = row.Cells(i).Value
                                    End If
                                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                    parrafo.Add(paraParrrafo) 'Texto que se insertara
                                    Dim CELDA As New PdfPCell
                                    CELDA.AddElement(parrafo)
                                    tablademo3.AddCell(CELDA) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()

                                End If
                            Next

                            Documento.Add(tablademo3) 'Agrega la tabla al documento

                            Documento.Add(New Paragraph(" ")) 'Salto de linea

                            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                            parrafo.Add("PLANILLA DE GASTOS Y VIATICOS") 'Texto que se insertara

                            Documento.Add(parrafo) 'Agrega el parrafo al documento
                            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                            Documento.Add(New Paragraph(" ")) 'Salto de linea


                            For Each row1 As DataGridViewRow In DataGridView2.Rows
                                If row.Cells(0).Value = row1.Cells(3).Value Then
                                    ' If row.Cells(2).Value = Nothing Or row.Cells(3).Value = Nothing Then
                                    'Documento.Add(New Paragraph(" NO HUBO DECLARACION DE GASTOS ")) 'Salto de linea
                                    'Else
                                    Dim tablademo4 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                                    tablademo4.HorizontalAlignment = ALIGN_LEFT
                                    tablademo4.SetWidthPercentage({100, 300, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                    For Each column As DataGridViewColumn In DataGridView2.Columns
                                        If column.Index <= 3 Then
                                            Dim paraParrrafo As String
                                            If IsNumeric(row1.Cells(column.Index).Value) Then
                                                Dim VARIABLE As Double
                                                subgasto = subgasto + row1.Cells(column.Index).Value
                                                VARIABLE = row1.Cells(column.Index).Value
                                                paraParrrafo = Format(VARIABLE, "###,##0.00")
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
                                    Documento.Add(tablademo4) 'Agrega la tabla al documento
                                End If

                                'End If
                            Next
                            Dim tablademo9 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                            tablademo9.HorizontalAlignment = ALIGN_LEFT
                            tablademo9.SetWidthPercentage({100, 300, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                            tablademo9.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            tablademo9.AddCell(New Paragraph(" ")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                            Dim miParrafo As String
                            Dim miTotal As Double
                            miTotal = Double.Parse(subgasto)
                            miParrafo = Format(miTotal, "###,##0.00")
                            Dim miCelda As New PdfPCell
                            parrafo.Add(miParrafo)
                            miCelda.AddElement(parrafo)
                            subgasto = 0
                            miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
                            tablademo9.AddCell(miCelda)
                            Documento.Add(tablademo9)
                            parrafo.Clear()
                            Documento.Add(New Paragraph(" ")) 'Salto de linea


                            parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                            'parrafo.Add(" Firma Interesado " & "          Encargado Rendicion " & "                             " & "Direccion Administracion " & vbCr & "             Ordenes de Cargo         " & "                                  " & "Finanzas") 'Texto que se insertara
                            parrafo.Add(" Firma Interesado " & "                 " & "         Encargado Rendicion " & "                             " & "Direccion Administracion " & vbCr & "  " & "                                         Ordenes de Cargo         " & "                                  " & " Finanzas") 'Texto que se insertara

                            'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                            'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                            'parrafo.Add("Direccion Administracion " & vbCr & "Finanzas") 'Texto que se insertara

                            Documento.Add(parrafo) 'Agrega el parrafo al documento
                            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                            Documento.Add(New Paragraph(" ")) 'Salto de linea
                            Documento.Add(New Paragraph(" ")) 'Salto de linea


                            Documento.NewPage()
                            If row.Cells("reintegrar").Value = 0 Or row.Cells("reintegrar").Value = Nothing Then
                                If row.Cells("A pagar").Value = 0 Or row.Cells("A pagar").Value = Nothing Then
                                    cargarSinSobrantes(row.Index)
                                Else
                                    'Esta funcion carga el sobrante en la tabla AASobrantes
                                    cargarParaPago(row.Index)
                                End If
                            Else
                                'XXXXXXXXXXXXXXXXXXXXXXXXXX
                                'If (chkReintegro.Checked) Then

                                'Else

                                cargarReintegro(row.Index)
                                'End If
                                'XXXXXXXXXXXXXXXXXXX
                            End If

                        Next

                        'Agrega la tabla al documento

                        'FIN DEL CUERPO DE LA TABLA
                    Else
                        If DataGridView1.Columns.Count >= 10 Then
                            For Each row As DataGridViewRow In DataGridView1.Rows
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
                                parrafo.Add("ORDEN NRO:" & dameNroHistorial("ViaticoCargo") & "  ")
                                parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                                parrafo.Add("NRO DE ENTRADA: " & txtCodigo.Text & "                         ") 'Texto que se insertara

                                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                                parrafo.Add("FECHA SALIDA: " & dtSalida.Text) 'Texto que se insertara

                                Documento.Add(parrafo) 'Agrega el parrafo al documento
                                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                                Documento.Add(New Paragraph(" ")) 'Salto de linea

                                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                                parrafo.Add("FECHA LLEGADA:" & dtLlegada.Text) 'Texto que se insertara

                                Documento.Add(parrafo) 'Agrega el parrafo al documento
                                parrafo.Clear()

                                Documento.Add(New Paragraph(" ")) 'Salto de linea
                                Dim tablademo As New PdfPTable(DataGridView1.Columns.Count) 'declara la tabla con 4 Columnas
                                tablademo.TotalWidth = 90
                                tablademo.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                                For Each miCol As DataGridViewColumn In DataGridView1.Columns
                                    tablademo.AddCell(New Paragraph(miCol.Name, FontFactory.GetFont("Arial", 12)))
                                Next
                                Documento.Add(tablademo) 'Agrega la tabla al documento

                                Dim tablademo2 As New PdfPTable(DataGridView1.Columns.Count) 'declara la tabla con 4 Columnas
                                tablademo2.TotalWidth = 90 'Ajusta el tamaño de cada columna
                                tablademo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED

                                For Each column As DataGridViewColumn In DataGridView1.Columns
                                    If column.Index <= DataGridView1.Columns.Count Then
                                        Dim paraParrrafo As String
                                        If IsNumeric(row.Cells(column.Index).Value) And Not column.Index = 1 Then
                                            paraParrrafo = Format(row.Cells(column.Index).Value, "###,##0.00")
                                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                        Else
                                            If IsNumeric(row.Cells(column.Index).Value) And column.Index = 1 Then
                                                paraParrrafo = row.Cells(column.Index).Value
                                                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                            Else
                                                paraParrrafo = row.Cells(column.Index).Value
                                                parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                            End If

                                        End If
                                        parrafo.Add(paraParrrafo) 'Texto que se insertara
                                        Dim celda100 As New PdfPCell
                                        celda100.AddElement(parrafo)
                                        tablademo2.AddCell(celda100) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        parrafo.Clear()
                                    End If

                                Next
                                Documento.Add(tablademo2)
                                Dim tablademo3 As New PdfPTable(DataGridView1.Columns.Count) 'declara la tabla con 4 Columnas

                                tablademo3.TotalWidth = 90 'Ajusta el tamaño de cada columna
                                tablademo3.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo3.AddCell(New Paragraph(row.Cells(1).Value.ToString)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                                For i As Integer = 2 To DataGridView1.Columns.Count - 1
                                    If row.Cells(i).Value = Nothing Then
                                        tablademo3.AddCell(New Paragraph("0,00"))
                                    Else

                                        Dim paraParrrafo As String
                                        If IsNumeric(row.Cells(i).Value) Then
                                            paraParrrafo = Format(row.Cells(i).Value, "###,##0.00")

                                        Else
                                            paraParrrafo = row.Cells(i).Value
                                        End If
                                        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                        parrafo.Add(paraParrrafo) 'Texto que se insertara
                                        tablademo3.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        parrafo.Clear()

                                    End If
                                Next

                                Documento.Add(tablademo3) 'Agrega la tabla al documento

                                Documento.Add(New Paragraph(" ")) 'Salto de linea


                                parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado

                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                                parrafo.Add("PLANILLA DE GASTOS Y VIATICOS") 'Texto que se insertara

                                Documento.Add(parrafo) 'Agrega el parrafo al documento
                                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                                Documento.Add(New Paragraph(" ")) 'Salto de linea


                                For Each row1 As DataGridViewRow In DataGridView2.Rows
                                    If row.Cells(0).Value = row1.Cells(3).Value Then
                                        ' If row.Cells(2).Value = Nothing Or row.Cells(3).Value = Nothing Then
                                        'Documento.Add(New Paragraph(" NO HUBO DECLARACION DE GASTOS ")) 'Salto de linea
                                        'Else
                                        Dim tablademo4 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                                        tablademo4.HorizontalAlignment = ALIGN_LEFT
                                        tablademo4.SetWidthPercentage({100, 300, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna



                                        For Each column As DataGridViewColumn In DataGridView2.Columns
                                            If column.Index <= 3 Then
                                                Dim paraParrrafo As String
                                                If IsNumeric(row1.Cells(column.Index).Value) Then
                                                    subgasto = subgasto + row1.Cells(column.Index).Value
                                                    Dim VARIABLE As Double
                                                    VARIABLE = row1.Cells(column.Index).Value
                                                    paraParrrafo = Format(VARIABLE, "###,##0.00")
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
                                        Documento.Add(tablademo4) 'Agrega la tabla al documento
                                    End If

                                    'End If
                                Next
                                Dim tablademo9 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                                tablademo9.HorizontalAlignment = ALIGN_LEFT
                                tablademo9.SetWidthPercentage({100, 300, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                tablademo9.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo9.AddCell(New Paragraph(" ")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                                Dim miParrafo As String
                                Dim miTotal As Double
                                miTotal = Double.Parse(subgasto)
                                miParrafo = Format(miTotal, "###,##0.00")
                                Dim miCelda As New PdfPCell
                                parrafo.Add(miParrafo)
                                miCelda.AddElement(parrafo)
                                subgasto = 0
                                miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
                                tablademo9.AddCell(miCelda)
                                Documento.Add(tablademo9)
                                parrafo.Clear()
                                Documento.Add(New Paragraph(" ")) 'Salto de linea
                                Documento.Add(New Paragraph(" ")) 'Salto de linea

                                parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                                parrafo.Add("            " & "          Encargado Rendicion " & "                             " & "Direccion Administracion " & vbCr & "             Ordenes de Cargo         " & "                                  " & "Finanzas") 'Texto que se insertara

                                'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                                'parrafo.Add("Direccion Administracion " & vbCr & "Finanzas") 'Texto que se insertara

                                Documento.Add(parrafo) 'Agrega el parrafo al documento
                                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                                Documento.Add(New Paragraph(" ")) 'Salto de linea
                                Documento.Add(New Paragraph(" ")) 'Salto de linea
                                Documento.Add(New Paragraph("Fecha de Impresion:" & Date.Now)) 'Salto de linea
                                Documento.NewPage()
                                If row.Cells("reintegrar").Value = 0 Or row.Cells("reintegrar").Value = Nothing Then
                                    If row.Cells("A pagar").Value = 0 Or row.Cells("A pagar").Value = Nothing Then
                                        cargarSinSobrantes(row.Index)
                                    Else
                                        'Esta funcion carga el sobrante en la tabla AASobrantes
                                        cargarParaPago(row.Index)
                                    End If
                                Else
                                    cargarReintegro(row.Index)
                                End If

                            Next
                        Else
                            For Each row As DataGridViewRow In DataGridView1.Rows
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
                                parrafo.Add("ORDEN NRO:" & dameNroHistorial("ViaticoCargo") & "  ")
                                parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                                parrafo.Add("NRO DE ENTRADA: " & txtCodigo.Text & "                         ") 'Texto que se insertara

                                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                                parrafo.Add("FECHA SALIDA: " & dtSalida.Text) 'Texto que se insertara

                                Documento.Add(parrafo) 'Agrega el parrafo al documento
                                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                                Documento.Add(New Paragraph(" ")) 'Salto de linea

                                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                                parrafo.Add("FECHA LLEGADA:" & dtLlegada.Text) 'Texto que se insertara

                                Documento.Add(parrafo) 'Agrega el parrafo al documento
                                parrafo.Clear()

                                Documento.Add(New Paragraph(" ")) 'Salto de linea
                                Dim tablademo As New PdfPTable(DataGridView1.Columns.Count) 'declara la tabla con 4 Columnas
                                tablademo.TotalWidth = 90
                                tablademo.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                                For Each miCol As DataGridViewColumn In DataGridView1.Columns
                                    tablademo.AddCell(New Paragraph(miCol.Name, FontFactory.GetFont("Arial", 12)))
                                Next
                                Documento.Add(tablademo) 'Agrega la tabla al documento

                                Dim tablademo2 As New PdfPTable(DataGridView1.Columns.Count) 'declara la tabla con 4 Columnas
                                tablademo2.TotalWidth = 90 'Ajusta el tamaño de cada columna
                                tablademo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED

                                For Each column As DataGridViewColumn In DataGridView1.Columns
                                    If column.Index <= DataGridView1.Columns.Count Then
                                        Dim paraParrrafo As String
                                        If IsNumeric(row.Cells(column.Index).Value) And Not column.Index = 1 Then
                                            paraParrrafo = Format(row.Cells(column.Index).Value, "###,##0.00")
                                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                        Else
                                            If IsNumeric(row.Cells(column.Index).Value) And column.Index = 1 Then
                                                paraParrrafo = row.Cells(column.Index).Value
                                                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                            Else
                                                paraParrrafo = row.Cells(column.Index).Value
                                                parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                            End If

                                        End If
                                        parrafo.Add(paraParrrafo) 'Texto que se insertara
                                        Dim celda100 As New PdfPCell
                                        celda100.AddElement(parrafo)
                                        tablademo2.AddCell(celda100) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        parrafo.Clear()
                                    End If

                                Next
                                Documento.Add(tablademo2)
                                Dim tablademo3 As New PdfPTable(DataGridView1.Columns.Count) 'declara la tabla con 4 Columnas

                                tablademo3.TotalWidth = 90 'Ajusta el tamaño de cada columna
                                tablademo3.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo3.AddCell(New Paragraph(row.Cells(1).Value.ToString)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                                For i As Integer = 2 To DataGridView1.Columns.Count - 1
                                    If row.Cells(i).Value = Nothing Then
                                        tablademo3.AddCell(New Paragraph("0,00"))
                                    Else

                                        Dim paraParrrafo As String
                                        If IsNumeric(row.Cells(i).Value) Then
                                            paraParrrafo = Format(row.Cells(i).Value, "###,##0.00")

                                        Else
                                            paraParrrafo = row.Cells(i).Value
                                        End If
                                        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                        parrafo.Add(paraParrrafo) 'Texto que se insertara
                                        tablademo3.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        parrafo.Clear()

                                    End If
                                Next

                                Documento.Add(New Paragraph(" ")) 'Salto de linea
                                Documento.Add(New Paragraph(" ")) 'Salto de linea

                                parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado

                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                                parrafo.Add("PLANILLA DE GASTOS Y VIATICOS") 'Texto que se insertara

                                Documento.Add(parrafo) 'Agrega el parrafo al documento
                                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                                Documento.Add(New Paragraph(" ")) 'Salto de linea
                                Documento.Add(New Paragraph(" ")) 'Salto de linea

                                For Each row1 As DataGridViewRow In DataGridView2.Rows
                                    If row.Cells(0).Value = row1.Cells(3).Value Then
                                        ' If row.Cells(2).Value = Nothing Or row.Cells(3).Value = Nothing Then
                                        'Documento.Add(New Paragraph(" NO HUBO DECLARACION DE GASTOS ")) 'Salto de linea
                                        'Else
                                        Dim tablademo4 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                                        tablademo4.HorizontalAlignment = ALIGN_LEFT
                                        tablademo4.SetWidthPercentage({100, 300, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna



                                        For Each column As DataGridViewColumn In DataGridView2.Columns
                                            If column.Index <= 3 Then
                                                Dim paraParrrafo As String
                                                If IsNumeric(row1.Cells(column.Index).Value) Then
                                                    subgasto = subgasto + row1.Cells(column.Index).Value
                                                    Dim VARIABLE As Double
                                                    VARIABLE = row1.Cells(column.Index).Value
                                                    paraParrrafo = Format(VARIABLE, "###,##0.00")
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
                                        Documento.Add(tablademo4) 'Agrega la tabla al documento
                                    End If

                                    'End If
                                Next
                                Dim tablademo9 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                                tablademo9.HorizontalAlignment = ALIGN_LEFT
                                tablademo9.SetWidthPercentage({100, 300, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                tablademo9.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo9.AddCell(New Paragraph(" ")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                                Dim miParrafo As String
                                Dim miTotal As Double
                                miTotal = Double.Parse(subgasto)
                                miParrafo = Format(miTotal, "###,##0.00")
                                Dim miCelda As New PdfPCell
                                parrafo.Add(miParrafo)
                                miCelda.AddElement(parrafo)
                                subgasto = 0
                                miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
                                tablademo9.AddCell(miCelda)
                                Documento.Add(tablademo9)
                                parrafo.Clear()
                                Documento.Add(New Paragraph(" ")) 'Salto de linea
                                Documento.Add(New Paragraph(" ")) 'Salto de linea

                                parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                                parrafo.Add("            " & "          Encargado Rendicion " & "                             " & "Direccion Administracion " & vbCr & "             Ordenes de Cargo         " & "                                  " & "Finanzas") 'Texto que se insertara

                                'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                                'parrafo.Add("Direccion Administracion " & vbCr & "Finanzas") 'Texto que se insertara

                                Documento.Add(parrafo) 'Agrega el parrafo al documento
                                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                                Documento.Add(New Paragraph("Fecha de Impresion:" & Date.Now)) 'Salto de linea
                                Documento.NewPage()
                                If row.Cells("reintegrar").Value = 0 Or row.Cells("reintegrar").Value = Nothing Then
                                    If row.Cells("A pagar").Value = 0 Or row.Cells("A pagar").Value = Nothing Then

                                        cargarSinSobrantes(row.Index)
                                    Else
                                        'Esta funcion carga los sobrantes en la tabla AASobrantes
                                        cargarParaPago(row.Index)
                                    End If
                                Else
                                    cargarReintegro(row.Index)
                                End If
                            Next
                        End If
                    End If

                    Documento.Close() 'Cierra el documento
                    System.Diagnostics.Process.Start("Rendicion.pdf")
                Catch ex As Exception
                    MessageBox.Show("Compruebe que no exista un documento abierto previamente", "AVISO")
                End Try
            Else

            End If
        Else

        End If

    End Sub

    Private Sub almacenarDisp()
        If chkDisp.Checked Then
            sql = "update AAOrdenNro set Disposicion='" & txtNro.Text & "' where Tipo='ViaticoPago' and NroIngreso='" & adjuntarAnio() & "'"
        End If

        If chkRes.Checked Then
            sql = "update AAOrdenNro set Resolucion='" & txtNro.Text & "' where Tipo='ViaticoPago' and NroIngreso='" & adjuntarAnio() & "'"
        End If

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql
                cmd4.ExecuteScalar()

            End Using
        End Using
    End Sub
    Public Sub imprimirPago2()
        Dim origen As String

        If chkNac.Checked Then
            origen = "OPN"
        End If
        If chkProv.Checked Then
            origen = "OPP"
        End If

        sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='ViaticoPago'"
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
        'If BANDERA Then
        'MessageBox.Show("este es la entrada")
        'imprimirPagoHistorial()
        ' Else

        Dim Nro As String
        sql = "Select Concepto from AAOrdenNro where Tipo='ViaticoCargo' and NroIngreso=" & adjuntarAnio()
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
            sql = "update Talonar set tal_ActualNro='" & nroOrden & "' where tal_Cod='" & origen & "'"
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

            ' MessageBox.Show(nroCargo)
            'Try
            Dim Documento As New Document(PageSize.A4, 60, 5, 35, 5)
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
            parrafo.Add("ORDEN PAGO NRO:" & nroCargo & "  ")
            parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("FECHA:" & Date.Now) 'Texto que se insertara

            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph("ORDEN CARGO ASOCIADA: " & Nro, FontFactory.GetFont("Arial", 9))) 'Salto de linea

            Documento.Add(New Paragraph(" ")) 'Salto de linea
            If chkDisp.Checked Then
                Documento.Add(New Paragraph("Disposición Nro:" & txtNro.Text)) 'Salto de linea
            End If
            If chkRes.Checked Then
                Documento.Add(New Paragraph("Resolución Nro:" & txtNro.Text)) 'Salto de linea
            End If
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


            'sql2 = "SELECT Distinct [chp_FVto],[pro_RazSoc], [chpctb_Cod],[chp_NroCheq],min[chp_Importe]  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
            '      " inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
            '        " inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]inner join [SBDASIPT].dbo.[CabMovF] on [CabCompra].ccocmf_ID=[CabMovF].[cmf_ID] " &
            '        " inner join [SBDASIPT].[dbo].[MovF] on  [CabMovF].cmf_ID=[MovF].mfocmf_ID " &
            '        " where CausaEmi.cem_Desc='" & adjuntarAnio() & "' GROUP BY [chp_FVto],[pro_RazSoc],[chpctb_Cod],[chp_NroCheq],min[chp_Importe]"

            sql2 = "SELECT Distinct [chp_FVto],[pro_RazSoc], [chpctb_Cod],[chp_NroCheq],[chp_Importe]  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
           " inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
            " inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]inner join [SBDASIPT].dbo.[CabMovF] on [CabCompra].ccocmf_ID=[CabMovF].[cmf_ID] " &
            " inner join [SBDASIPT].[dbo].[MovF] on  [CabMovF].cmf_ID=[MovF].mfocmf_ID " &
            " where CausaEmi.cem_Desc='" & adjuntarAnio() & "' GROUP BY [chp_FVto],[pro_RazSoc],[chpctb_Cod],[chp_NroCheq],[chp_Importe]"

            Dim codigo As String
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql2
                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    'reader.Read()
                    'codigo = reader(0)
                    While reader.Read()

                        For i = 0 To tablademo4.NumberOfColumns
                            If i >= 5 Then

                            Else
                                Dim paraParrrafo As String
                                If IsNumeric(reader(i)) And i <> 3 Then
                                    'MessageBox.Show("hasta aca anda")
                                    Dim resultado As Double = 0
                                    If (i = 3) Then

                                        paraParrrafo = Format(resultado, "###,##0.00")
                                        Dim miCelda2 As New PdfPCell
                                        parrafo.Add(paraParrrafo)
                                        miCelda2.AddElement(parrafo)
                                        miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                                        tablademo4.AddCell(miCelda2)
                                        parrafo.Clear()
                                    Else
                                        paraParrrafo = Format(resultado, "###,##0.00")
                                        Dim miCelda2 As New PdfPCell
                                        parrafo.Add(paraParrrafo)
                                        miCelda2.AddElement(parrafo)
                                        miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                                        tablademo4.AddCell(miCelda2)
                                        parrafo.Clear()
                                    End If

                                Else

                                    paraParrrafo = reader(i)
                                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                    parrafo.Add(paraParrrafo) 'Texto que se insertara
                                    tablademo4.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()

                                End If
                            End If

                        Next


                    End While


                End Using
            End Using

            Documento.Add(tablademo4) 'Agrega la tabla al documento
            Dim totalDon As Double
            totalDon = 0
            Dim SUBTOTAL As Double
            sql = "Select sobrante,donacion,fechaMod,total,nombre  from AASobrantes where NroOrden='" & adjuntarAnio() & "'"

            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()

                    conn.Open()

                    cmd4.CommandText = sql

                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    While reader.Read()

                        If Not reader.Item(0).Equals(Nothing) Or Not reader.Item(1).ToString.Equals("X") Then

                            totalDon = totalDon + reader.Item(3)
                            Dim Donacion As String
                            Donacion = reader.Item(1)
                            If Donacion = "SI" Then
                                If reader(0) = 0 Then

                                Else
                                    Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                    tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

                                    tablademo0.AddCell(reader.Item(2)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell("DONACION") 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph("")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph("")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    Dim importe As String
                                    Dim miImporte As Double
                                    If reader.Item(0) > 0 Then
                                        miImporte = reader.Item(0)
                                    Else
                                        miImporte = reader.Item(0) * -1
                                    End If
                                    SUBTOTAL = SUBTOTAL + reader.Item(0)
                                    importe = Format(miImporte, "###,##0.00")
                                    parrafo.Add(importe)
                                    Dim CeldaImp As New PdfPCell
                                    CeldaImp.AddElement(parrafo)
                                    tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()

                                    Documento.Add(tablademo0) 'Agrega la tabla al documento
                                End If
                            Else
                                If reader.Item(0) = 0 Then

                                Else
                                    If reader.Item(0) > 0 Then
                                        Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                        tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

                                        tablademo0.AddCell("DIFERENCIA A REINTEGRAR") 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo0.AddCell(reader.Item(4).ToString) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo0.AddCell(New Paragraph(" ")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo0.AddCell(New Paragraph("")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        Dim importe As String
                                        Dim miImporte As Double
                                        If reader.Item(0) > 0 Then
                                            miImporte = reader.Item(0) * -1
                                            'totalDon = totalDon - reader.Item(0)
                                        Else
                                            miImporte = reader.Item(0) * -1
                                            'totalDon = totalDon + (reader.Item(0))
                                        End If
                                        SUBTOTAL = SUBTOTAL + miImporte
                                        importe = Format(miImporte, "###,##0.00")
                                        parrafo.Add(importe)
                                        Dim CeldaImp As New PdfPCell
                                        CeldaImp.AddElement(parrafo)
                                        tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        parrafo.Clear()
                                        Documento.Add(tablademo0) 'Agrega la tabla al documento
                                    Else
                                        Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                        tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

                                        tablademo0.AddCell("DIFERENCIA A PAGAR") 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

                                        tablademo0.AddCell(reader.Item(4).ToString) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo0.AddCell(New Paragraph(" ")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo0.AddCell(New Paragraph("")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        Dim importe As String
                                        Dim miImporte As Double
                                        If reader.Item(0) > 0 Then
                                            miImporte = reader.Item(0) * -1
                                            'totalDon = totalDon - reader.Item(0)
                                        Else
                                            miImporte = reader.Item(0) * -1
                                            'totalDon = totalDon + (reader.Item(0) * -1)
                                        End If
                                        SUBTOTAL = SUBTOTAL + miImporte
                                        importe = Format(miImporte, "###,##0.00")
                                        parrafo.Add(importe)
                                        Dim CeldaImp As New PdfPCell
                                        CeldaImp.AddElement(parrafo)
                                        tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        parrafo.Clear()
                                        Documento.Add(tablademo0) 'Agrega la tabla al documento
                                    End If

                                End If

                            End If
                        Else
                            sql3 = "select cmf_FMov,cmf_Desc,mfoctb_Cod,mfo_ImpMonElem from [SBDASIPT].dbo.CabMovF " +
        "inner join [SBDASIPT].dbo.MovF on CabMovF.cmf_ID=MovF.mfocmf_ID " +
        "where cmftmo_Cod='REIVIA'  and mfo_ImpMonElem>0 and cmf_Desc like '%" & txtCodigo.Text & "%'"
                            Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                            tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

                            Using conn2 As New SqlConnection(CONNSTR)
                                Using cmd42 As SqlCommand = conn2.CreateCommand()

                                    conn2.Open()

                                    cmd42.CommandText = sql3

                                    Dim reader2 As SqlDataReader = Nothing
                                    reader2 = cmd42.ExecuteReader
                                    'reader.Read()
                                    'codigo = reader(0)
                                    While reader2.Read()
                                        tablademo0.AddCell(reader2.Item(0)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo0.AddCell(reader2.Item(1)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        Dim cta As String
                                        cta = reader2(2)
                                        tablademo0.AddCell(New Paragraph(cta)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo0.AddCell(New Paragraph("Deposito")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        Dim importe As String
                                        Dim miImporte As Double
                                        If reader2.Item(3) > 0 Then
                                            miImporte = reader2.Item(3)
                                        Else
                                            miImporte = reader2.Item(3) * -1
                                        End If

                                        importe = Format(miImporte, "###,##0.00")
                                        parrafo.Add(importe)
                                        Dim CeldaImp As New PdfPCell
                                        CeldaImp.AddElement(parrafo)
                                        tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        parrafo.Clear()
                                    End While


                                End Using
                            End Using
                            Documento.Add(tablademo0) 'Agrega la tabla al documento
                        End If
                    End While

                End Using
            End Using
            '''''''''''''''''''''''''
            Dim tabla9 As New PdfPTable(5)
            'tabla9.HorizontalAlignment = ALIGN_LEFT
            tabla9.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tabla9.AddCell("TOTAL")
            tabla9.AddCell(" ")
            tabla9.AddCell(" ")
            tabla9.AddCell(" ")
            Dim miParrafo As String
            Dim miTotal As Double
            miTotal = 0 'Double.Parse(totalDon)
            'MessageBox.Show("hasta aca estoy ahora")
            miParrafo = Format(0, "###,##0.00")
            Dim miCelda As New PdfPCell
            parrafo.Add(miParrafo)
            miCelda.AddElement(parrafo)

            miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
            tabla9.AddCell(miCelda)
            Documento.Add(tabla9)
            parrafo.Clear()

            miTotal = Double.Parse(miTotal)
            Documento.Add(New Paragraph("TOTAL:$" & 0 & "(PESOS " & Numalet.ToCardinal(0) & ")")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL") 'Salto de linea

            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente


            Documento.Close() 'Cierra el documento
            System.Diagnostics.Process.Start("OrdenPago.pdf") 'Abre el archivo DEMO.PDF
            almacenarDisp()
            'Catch ex As Exception
            '    MessageBox.Show("Compruebe que no tiene un documento abierto previamente", "Aviso")
            'End Try

        Else

        End If

        'End If

    End Sub
    Public Function verificar() As String
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
        If (codigo.Substring(0, 3).Equals(fecha)) Then

        End If

        Return codigo

    End Function


    Public Sub imprimirPago()
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Se busca en la tabla AAOrdenNro la causa de emision de la ventana de encontarla se asigna true o false a la variable booleana

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' se declara la variable origen
        Dim origen As String
        'pregunta si el checkBox nacional esta marcado asigna a origen OPN
        If chkNac.Checked Then
            origen = "OPN"
        End If
        'pregunta si el checkBox provincial esta marcado asigna a origen OPP
        If chkProv.Checked Then
            origen = "OPP"
        End If
        'consulta sql que busca en la tabla AAOrdenNro la cuasa de emision que esta en la ventana con el atributo tipo de valor ViaticoPago
        'sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='ViaticoPago'"
        sql = "Select Concepto from AAOrdenNro where NroIngreso like '%" & cmbAnio.Text & "%' and NacPro='" & origen & "'"
        'declara una variable booleana
        Dim BANDERA As Boolean
        'se conecta con la base de datos y ejecuta la consulta
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
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
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'si la variable BANDERA es true se invoca a la funcion imprimirPagoHistorial(), y se actualiza el Talonar realizando 
        'todos los ajustes necesarios
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If BANDERA Then
            'Select Case MsgBox("¿YA EXISTE UNA ORDEN DE PAGO CON ESTE NUMERO?", MsgBoxStyle.YesNo, "AVISO")
            'en caso de seleccionar un si
            '    Case MsgBoxResult.Yes
            'Elimina la tupla existente en la base de datos y despues inserta la nueva orden de pago
            'reemplazarImprimirPago()
            '   Case MsgBoxResult.No
            ' MessageBox.Show("Accion cancelada por el usuario", "INFORMACION")
            'End Select

            'MessageBox.Show("este es la entrada")
            'imprimirPagoHistorial()
            MessageBox.Show("¿YA EXISTE UNA ORDEN DE PAGO CON ESTE NUMERO?", "AVISO")
        Else
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'se declara la variable nro
            Dim Nro As String
            'consulta sql en donde se busca en la tabla AAOrdenNro con el tipo ViaticoCargo la cuasa de emison ingresada en la ventana
            sql = "Select Concepto from AAOrdenNro where Tipo='ViaticoCargo' and NroIngreso=" & adjuntarAnio()
            'se conecta con la base de datos y se guarda el concepto  en la variable Nro
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
            'declara las variables 
            Dim nroCargo As String
            Dim nroOrden As String
            'asigna a nroCargo el valor de la caja de texto txtSaliente
            nroCargo = txtSaliente.Text
            Try
                'consulta y ajusta si se ingreso mal en la ventana la causa de emision
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
                'actualiza el talonario.
                sql = "update Talonar set tal_ActualNro='" & nroOrden & "' where tal_Cod='" & origen & "'"
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
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Si se ingresa correctamenta la tupla en la tabla AAOrdenNro con la funcion almacenarOrdenPago,
            'se declara la cabecera del pdf
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If almacenarOrdenPago(nroCargo, origen) Then
                'Try
                Dim Documento As New Document(PageSize.A4, 60, 5, 35, 5)
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
                parrafo.Add("ORDEN PAGO NRO:" & nroCargo & "  ")
                parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                parrafo.Add("FECHA:" & Date.Now) 'Texto que se insertara

                Documento.Add(parrafo) 'Agrega el parrafo al documento
                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                Documento.Add(New Paragraph("ORDEN CARGO ASOCIADA: " & Nro, FontFactory.GetFont("Arial", 9))) 'Salto de linea

                Documento.Add(New Paragraph(" ")) 'Salto de linea
                If chkDisp.Checked Then
                    Documento.Add(New Paragraph("Disposición Nro:" & txtNro.Text)) 'Salto de linea
                End If
                If chkRes.Checked Then
                    Documento.Add(New Paragraph("Resolución Nro:" & txtNro.Text)) 'Salto de linea
                End If
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


                'sql2 = "SELECT Distinct [chp_FVto],[pro_RazSoc], [chpctb_Cod],[chp_NroCheq],min[chp_Importe]  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
                '      " inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
                '        " inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]inner join [SBDASIPT].dbo.[CabMovF] on [CabCompra].ccocmf_ID=[CabMovF].[cmf_ID] " &
                '        " inner join [SBDASIPT].[dbo].[MovF] on  [CabMovF].cmf_ID=[MovF].mfocmf_ID " &
                '        " where CausaEmi.cem_Desc='" & adjuntarAnio() & "' GROUP BY [chp_FVto],[pro_RazSoc],[chpctb_Cod],[chp_NroCheq],min[chp_Importe]"
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                sql2 = "SELECT Distinct [chp_FVto],[pro_RazSoc], [chpctb_Cod],[chp_NroCheq],[chp_Importe]  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
               " inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
                " inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]inner join [SBDASIPT].dbo.[CabMovF] on [CabCompra].ccocmf_ID=[CabMovF].[cmf_ID] " &
                " inner join [SBDASIPT].[dbo].[MovF] on  [CabMovF].cmf_ID=[MovF].mfocmf_ID " &
                " where CausaEmi.cem_Desc='" & adjuntarAnio() & "' GROUP BY [chp_FVto],[pro_RazSoc],[chpctb_Cod],[chp_NroCheq],[chp_Importe]"
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Dim subtotal As Double
                subtotal = 0
                Dim cuentaYcheque(2) As Object
                Dim codigo As String
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()
                        conn.Open()
                        cmd4.CommandText = sql2
                        Dim reader As SqlDataReader = Nothing
                        reader = cmd4.ExecuteReader
                        'reader.Read()
                        'codigo = reader(0)
                        While reader.Read()
                            For i = 0 To tablademo4.NumberOfColumns
                                If i >= 5 Then
                                Else
                                    Dim paraParrrafo As String
                                    If IsNumeric(reader(i)) And i <> 3 Then
                                        paraParrrafo = Format(reader(i), "###,##0.00")
                                        subtotal = subtotal + reader(i)
                                        Dim miCelda2 As New PdfPCell
                                        parrafo.Add(paraParrrafo)
                                        miCelda2.AddElement(parrafo)
                                        miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                                        tablademo4.AddCell(miCelda2)
                                        parrafo.Clear()
                                    Else
                                        paraParrrafo = reader(i)
                                        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                        parrafo.Add(paraParrrafo) 'Texto que se insertara
                                        tablademo4.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        parrafo.Clear()
                                    End If
                                End If
                            Next
                        End While
                    End Using
                End Using
                Documento.Add(tablademo4) 'Agrega la tabla al documento
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'se declaran las variables necesarias
                sql = "Select sobrante,donacion,fechaMod,total,nombre,nroCuenta,nroCheque  from AASobrantes where NroOrden='" & adjuntarAnio() & "'"
                Dim totalDon As Double
                Dim totalAdisional As Double
                totalAdisional = 0

                Using conn As New SqlConnection(CONNSTR)
                    Using cmd4 As SqlCommand = conn.CreateCommand()
                        conn.Open()
                        cmd4.CommandText = sql
                        Dim reader As SqlDataReader = Nothing
                        reader = cmd4.ExecuteReader
                        While reader.Read()
                            If Not reader.Item(0).Equals(Nothing) Or Not reader.Item(1).ToString.Equals("X") Then
                                totalDon = totalDon + reader.Item(3)
                                Dim Donacion As String
                                Donacion = reader.Item(1).ToString
                                If Donacion = "SI" Then
                                    If reader(0) = 0 Then
                                    Else
                                        Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                        tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                        Dim nroCuenta As String
                                        Dim nroCheque As String
                                        If (IsDBNull(reader(5))) Then
                                            nroCuenta = ""
                                        Else
                                            nroCuenta = reader(5)
                                        End If
                                        If (IsDBNull(reader(6))) Then
                                            nroCheque = ""
                                        Else
                                            nroCheque = reader(6)
                                        End If

                                        tablademo0.AddCell(reader.Item(2)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo0.AddCell("DONACION") 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

                                        tablademo0.AddCell(New Paragraph(nroCuenta)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo0.AddCell(New Paragraph(nroCheque)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        Dim importe As String
                                        Dim miImporte As Double
                                        If reader.Item(0) > 0 Then
                                            miImporte = reader.Item(0)
                                            totalAdisional = totalAdisional + miImporte
                                        Else
                                            miImporte = reader.Item(0)
                                            totalAdisional = totalAdisional + (miImporte * -1)
                                            miImporte = reader.Item(0) * -1
                                        End If
                                        ' subtotal = subtotal + reader.Item(0)
                                        importe = Format(miImporte, "###,##0.00")
                                        parrafo.Add(importe)
                                        Dim CeldaImp As New PdfPCell
                                        CeldaImp.AddElement(parrafo)
                                        tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        parrafo.Clear()

                                        Documento.Add(tablademo0) 'Agrega la tabla al documento
                                    End If
                                Else
                                    If reader.Item(0) = 0 Then

                                    Else
                                        If reader.Item(0) > 0 Then
                                            Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                            tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                            Dim nroCuenta As String
                                            Dim nroCheque As String
                                            If (IsDBNull(reader(5))) Then
                                                nroCuenta = ""
                                            Else
                                                nroCuenta = reader(5)
                                            End If
                                            If (IsDBNull(reader(6))) Then
                                                nroCheque = ""
                                            Else
                                                nroCheque = reader(6)
                                            End If

                                            tablademo0.AddCell("DIFERENCIA A REINTEGRAR") 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                            tablademo0.AddCell(reader.Item(4).ToString) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                            tablademo0.AddCell(New Paragraph(nroCuenta)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                            tablademo0.AddCell(New Paragraph(nroCheque)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                            Dim importe As String
                                            Dim miImporte As Double
                                            If reader.Item(0) > 0 Then
                                                miImporte = reader.Item(0) * 1 'modificacion reciente
                                                totalAdisional = totalAdisional - miImporte
                                                'totalDon = totalDon - reader.Item(0)
                                                miImporte = miImporte * -1
                                            Else

                                                miImporte = reader.Item(0) * 1
                                                totalAdisional = totalAdisional + miImporte
                                            End If

                                            importe = Format(miImporte, "###,##0.00")
                                            parrafo.Add(importe)
                                            Dim CeldaImp As New PdfPCell
                                            CeldaImp.AddElement(parrafo)
                                            tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                            parrafo.Clear()

                                            Documento.Add(tablademo0) 'Agrega la tabla al documento
                                        Else
                                            Dim nroCuenta As String
                                            Dim nroCheque As String
                                            If (IsDBNull(reader(5))) Then
                                                nroCuenta = ""
                                            Else
                                                nroCuenta = reader(5)
                                            End If
                                            If (IsDBNull(reader(6))) Then
                                                nroCheque = ""
                                            Else
                                                nroCheque = reader(6)
                                            End If

                                            Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                            tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                            tablademo0.AddCell("DIFERENCIA A PAGAR") 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

                                            tablademo0.AddCell(reader.Item(4).ToString) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                            tablademo0.AddCell(New Paragraph(nroCuenta)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                            tablademo0.AddCell(New Paragraph(nroCheque)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                            Dim importe As String
                                            Dim miImporte As Double
                                            If reader.Item(0) > 0 Then
                                                miImporte = reader.Item(0)
                                                totalAdisional = totalAdisional + miImporte
                                                miImporte = reader.Item(0) * 1
                                            Else
                                                miImporte = reader.Item(0) * 1
                                                totalAdisional = totalAdisional + miImporte

                                            End If
                                            'subtotal = subtotal + miImporte
                                            importe = Format(miImporte, "###,##0.00")
                                            parrafo.Add(importe)
                                            Dim CeldaImp As New PdfPCell
                                            CeldaImp.AddElement(parrafo)
                                            tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                            parrafo.Clear()
                                            Documento.Add(tablademo0) 'Agrega la tabla al documento
                                        End If

                                    End If

                                End If
                            Else
                                sql3 = "select Distinct cmf_FMov,cmf_Desc,mfoctb_Cod,mfo_ImpMonElem from [SBDASIPT].dbo.CabMovF " +
                                "inner join [SBDASIPT].dbo.MovF on CabMovF.cmf_ID=MovF.mfocmf_ID " +
                                "where cmftmo_Cod='REIVIA'  and mfo_ImpMonElem>0 and cmf_Desc like '%" & txtCodigo.Text & "%'"
                                Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

                                Using conn2 As New SqlConnection(CONNSTR)
                                    Using cmd42 As SqlCommand = conn2.CreateCommand()

                                        conn2.Open()

                                        cmd42.CommandText = sql3

                                        Dim reader2 As SqlDataReader = Nothing
                                        reader2 = cmd42.ExecuteReader
                                        'reader.Read()
                                        'codigo = reader(0)
                                        While reader2.Read()
                                            tablademo0.AddCell(reader2.Item(0)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                            tablademo0.AddCell(reader2.Item(1)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                            Dim cta As String
                                            cta = reader2(2)
                                            tablademo0.AddCell(New Paragraph(cta)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                            tablademo0.AddCell(New Paragraph("Deposito")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                            Dim importe As String
                                            Dim miImporte As Double
                                            If reader2.Item(3) > 0 Then
                                                miImporte = reader2.Item(3)
                                                totalAdisional = totalAdisional - miImporte
                                            Else
                                                miImporte = reader2.Item(3)
                                                totalAdisional = totalAdisional + miImporte
                                                miImporte = reader2.Item(3)
                                            End If

                                            importe = Format(miImporte, "###,##0.00")
                                            parrafo.Add(importe)
                                            Dim CeldaImp As New PdfPCell
                                            CeldaImp.AddElement(parrafo)
                                            tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                            parrafo.Clear()
                                        End While


                                    End Using
                                End Using
                                Documento.Add(tablademo0) 'Agrega la tabla al documento
                            End If
                        End While



                    End Using
                End Using

                Dim tabla9 As New PdfPTable(5)
                'tabla9.HorizontalAlignment = ALIGN_LEFT
                tabla9.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                tabla9.AddCell("TOTAL")
                tabla9.AddCell(" ")
                tabla9.AddCell(" ")
                tabla9.AddCell(" ")
                Dim miParrafo As String
                Dim miTotal As Double

                subtotal = subtotal + totalAdisional
                miTotal = Double.Parse(subtotal)

                miParrafo = Format(miTotal, "###,##0.00")
                Dim miCelda As New PdfPCell
                parrafo.Add(miParrafo)
                miCelda.AddElement(parrafo)

                miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
                tabla9.AddCell(miCelda)
                Documento.Add(tabla9)
                parrafo.Clear()



                'miTotal = Double.Parse(txtTotalFinal.Text)
                Documento.Add(New Paragraph("TOTAL:$" & miTotal & "(PESOS " & Numalet.ToCardinal(miTotal) & ")")) 'Salto de linea
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                parrafo.Add("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL") 'Salto de linea

                Documento.Add(parrafo) 'Agrega el parrafo al documento
                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente


                Documento.Close() 'Cierra el documento
                System.Diagnostics.Process.Start("OrdenPago.pdf") 'Abre el archivo DEMO.PDF
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Else

            End If

        End If

    End Sub
    Public Sub reemplazarImprimirViaticoAutorizado()
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
        reemplazarViaticoAutorizado()
        MessageBox.Show("El remplazo se logro de manera correcta")
    End Sub
    Public Sub reemplazarViaticoAutorizado()
        Dim origen As String
        If chkNac.Checked Then
            origen = "OCN"
        End If
        If chkProv.Checked Then
            origen = "OCP"
        End If
        Try
            Dim Documento As New Document(PageSize.A4, 60, 5, 35, 5) 'Declaracion del documento
            Dim parrafo As New Paragraph ' Declaracion de un parrafo
            Dim tablademo As New PdfPTable(5) 'declara la tabla con 4 Columnas

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

            sql = "Select Concepto from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='ViaticoCargo' or Tipo='Contratos'"
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

            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("ORDEN NRO:" & dameNroHistorial("ViaticoCargo") & "                             ") 'Texto que se insertara
            parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("FECHA:" & Date.Now) 'Texto que se insertara

            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

            Documento.Add(New Paragraph(" ")) 'Salto de linea

            tablademo.SetWidthPercentage({170, 100, 80, 80, 140}, PageSize.A4) 'Ajusta el tamaño de cada columna

            tablademo.AddCell(New Paragraph("DESTINATARIO", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            tablademo.AddCell(New Paragraph("CUENTA CORRIENTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
            tablademo.AddCell(New Paragraph("NRO DE CHEQUE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
            tablademo.AddCell(New Paragraph("IMPORTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo.AddCell(New Paragraph("RECIBI CONFORME", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            Documento.Add(tablademo) 'Agrega la tabla al documento

            Dim tablademo2 As New PdfPTable(5) 'declara la tabla con 4 Columnas
            tablademo2.SetWidthPercentage({170, 100, 80, 80, 140}, PageSize.A4) 'Ajusta el tamaño de cada columna

            For Each row As DataGridViewRow In DataGridView1.Rows
                For Each column As DataGridViewColumn In DataGridView1.Columns
                    If column.Index <= 4 Then
                        Dim paraParrrafo As String

                        If IsNumeric(row.Cells(column.Index).Value) And column.Index = 3 Then

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
            Dim GranTotal As Double

            GranTotal = Double.Parse(txtTotalFinal.Text)
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
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("DEPTO. TESORERIA") 'Texto que se insertara

            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

            Documento.Close() 'Cierra el documento
            System.Diagnostics.Process.Start("OrdenCargo.pdf") 'Abre el archivo DEMO.PDF
        Catch ex As Exception
            MessageBox.Show("Verifique que no tiene el mismo documento abierto", "AVISO")
        End Try


    End Sub
    Public Sub reemplazarImprimirPago()
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
        ' se declara la variable origen
        Dim origen As String
        'pregunta si el checkBox nacional esta marcado asigna a origen OPN
        If chkNac.Checked Then
            origen = "OPN"
        End If
        'pregunta si el checkBox provincial esta marcado asigna a origen OPP
        If chkProv.Checked Then
            origen = "OPP"
        End If

        'se declara la variable nro
            Dim Nro As String
            'consulta sql en donde se busca en la tabla AAOrdenNro con el tipo ViaticoCargo la cuasa de emison ingresada en la ventana
            sql = "Select Concepto from AAOrdenNro where Tipo='ViaticoCargo' and NroIngreso=" & adjuntarAnio()
            'se conecta con la base de datos y se guarda el concepto  en la variable Nro
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
            'declara las variables 
            Dim nroCargo As String
            Dim nroOrden As String
            'asigna a nroCargo el valor de la caja de texto txtSaliente
            nroCargo = txtSaliente.Text
            Try
                'consulta y ajusta si se ingreso mal en la ventana la causa de emision
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
                'actualiza el talonario.
                sql = "update Talonar set tal_ActualNro='" & nroOrden & "' where tal_Cod='" & origen & "'"
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
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Si se ingresa correctamenta la tupla en la tabla AAOrdenNro con la funcion almacenarOrdenPago,
            'se declara la cabecera del pdf
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If almacenarOrdenPago(nroCargo, origen) Then
            'Try
            Dim Documento As New Document(PageSize.A4, 60, 5, 35, 5)
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
            parrafo.Add("ORDEN PAGO NRO:" & nroCargo & "  ")
            parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("FECHA:" & Date.Now) 'Texto que se insertara

            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph("ORDEN CARGO ASOCIADA: " & Nro, FontFactory.GetFont("Arial", 9))) 'Salto de linea

            Documento.Add(New Paragraph(" ")) 'Salto de linea
            If chkDisp.Checked Then
                Documento.Add(New Paragraph("Disposición Nro:" & txtNro.Text)) 'Salto de linea
            End If
            If chkRes.Checked Then
                Documento.Add(New Paragraph("Resolución Nro:" & txtNro.Text)) 'Salto de linea
            End If
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


            'sql2 = "SELECT Distinct [chp_FVto],[pro_RazSoc], [chpctb_Cod],[chp_NroCheq],min[chp_Importe]  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
            '      " inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
            '        " inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]inner join [SBDASIPT].dbo.[CabMovF] on [CabCompra].ccocmf_ID=[CabMovF].[cmf_ID] " &
            '        " inner join [SBDASIPT].[dbo].[MovF] on  [CabMovF].cmf_ID=[MovF].mfocmf_ID " &
            '        " where CausaEmi.cem_Desc='" & adjuntarAnio() & "' GROUP BY [chp_FVto],[pro_RazSoc],[chpctb_Cod],[chp_NroCheq],min[chp_Importe]"
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            sql2 = "SELECT Distinct [chp_FVto],[pro_RazSoc], [chpctb_Cod],[chp_NroCheq],[chp_Importe]  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
           " inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
            " inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]inner join [SBDASIPT].dbo.[CabMovF] on [CabCompra].ccocmf_ID=[CabMovF].[cmf_ID] " &
            " inner join [SBDASIPT].[dbo].[MovF] on  [CabMovF].cmf_ID=[MovF].mfocmf_ID " &
            " where CausaEmi.cem_Desc='" & adjuntarAnio() & "' GROUP BY [chp_FVto],[pro_RazSoc],[chpctb_Cod],[chp_NroCheq],[chp_Importe]"
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim subtotal As Double
            subtotal = 0
            Dim cuentaYcheque(2) As Object
            Dim codigo As String
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql2
                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    'reader.Read()
                    'codigo = reader(0)
                    While reader.Read()
                        For i = 0 To tablademo4.NumberOfColumns
                            If i >= 5 Then
                            Else
                                Dim paraParrrafo As String
                                If IsNumeric(reader(i)) And i <> 3 Then
                                    paraParrrafo = Format(reader(i), "###,##0.00")
                                    subtotal = subtotal + reader(i)
                                    Dim miCelda2 As New PdfPCell
                                    parrafo.Add(paraParrrafo)
                                    miCelda2.AddElement(parrafo)
                                    miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                                    tablademo4.AddCell(miCelda2)
                                    parrafo.Clear()
                                Else
                                    paraParrrafo = reader(i)
                                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                    parrafo.Add(paraParrrafo) 'Texto que se insertara
                                    tablademo4.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()
                                End If
                            End If
                        Next
                    End While
                End Using
            End Using
            Documento.Add(tablademo4) 'Agrega la tabla al documento
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'se declaran las variables necesarias
            sql = "Select sobrante,donacion,fechaMod,total,nombre,nroCuenta,nroCheque  from AASobrantes where NroOrden='" & adjuntarAnio() & "'"
            Dim totalDon As Double
            Dim totalAdisional As Double
            totalAdisional = 0

            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql
                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    While reader.Read()
                        If Not reader.Item(0).Equals(Nothing) Or Not reader.Item(1).ToString.Equals("X") Then
                            totalDon = totalDon + reader.Item(3)
                            Dim Donacion As String
                            Donacion = reader.Item(1).ToString
                            If Donacion = "SI" Then
                                If reader(0) = 0 Then
                                Else
                                    Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                    tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                    Dim nroCuenta As String
                                    Dim nroCheque As String
                                    If (IsDBNull(reader(5))) Then
                                        nroCuenta = ""
                                    Else
                                        nroCuenta = reader(5)
                                    End If
                                    If (IsDBNull(reader(6))) Then
                                        nroCheque = ""
                                    Else
                                        nroCheque = reader(6)
                                    End If

                                    tablademo0.AddCell(reader.Item(2)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell("DONACION") 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

                                    tablademo0.AddCell(New Paragraph(nroCuenta)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph(nroCheque)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    Dim importe As String
                                    Dim miImporte As Double
                                    If reader.Item(0) > 0 Then
                                        miImporte = reader.Item(0)
                                        totalAdisional = totalAdisional + miImporte
                                    Else
                                        miImporte = reader.Item(0)
                                        totalAdisional = totalAdisional + (miImporte * -1)
                                        miImporte = reader.Item(0) * -1
                                    End If
                                    ' subtotal = subtotal + reader.Item(0)
                                    importe = Format(miImporte, "###,##0.00")
                                    parrafo.Add(importe)
                                    Dim CeldaImp As New PdfPCell
                                    CeldaImp.AddElement(parrafo)
                                    tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()

                                    Documento.Add(tablademo0) 'Agrega la tabla al documento
                                End If
                            Else
                                If reader.Item(0) = 0 Then

                                Else
                                    If reader.Item(0) > 0 Then
                                        Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                        tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                        Dim nroCuenta As String
                                        Dim nroCheque As String
                                        If (IsDBNull(reader(5))) Then
                                            nroCuenta = ""
                                        Else
                                            nroCuenta = reader(5)
                                        End If
                                        If (IsDBNull(reader(6))) Then
                                            nroCheque = ""
                                        Else
                                            nroCheque = reader(6)
                                        End If

                                        tablademo0.AddCell("DIFERENCIA A REINTEGRAR") 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo0.AddCell(reader.Item(4).ToString) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo0.AddCell(New Paragraph(nroCuenta)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo0.AddCell(New Paragraph(nroCheque)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        Dim importe As String
                                        Dim miImporte As Double
                                        If reader.Item(0) > 0 Then
                                            miImporte = reader.Item(0) * 1 'modificacion reciente
                                            totalAdisional = totalAdisional - miImporte
                                            'totalDon = totalDon - reader.Item(0)
                                        Else

                                            miImporte = reader.Item(0) * 1
                                            totalAdisional = totalAdisional + miImporte
                                        End If

                                        importe = Format(miImporte, "###,##0.00")
                                        parrafo.Add(importe)
                                        Dim CeldaImp As New PdfPCell
                                        CeldaImp.AddElement(parrafo)
                                        tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        parrafo.Clear()

                                        Documento.Add(tablademo0) 'Agrega la tabla al documento
                                    Else
                                        Dim nroCuenta As String
                                        Dim nroCheque As String
                                        If (IsDBNull(reader(5))) Then
                                            nroCuenta = ""
                                        Else
                                            nroCuenta = reader(5)
                                        End If
                                        If (IsDBNull(reader(6))) Then
                                            nroCheque = ""
                                        Else
                                            nroCheque = reader(6)
                                        End If

                                        Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                        tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                        tablademo0.AddCell("DIFERENCIA A PAGAR") 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

                                        tablademo0.AddCell(reader.Item(4).ToString) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo0.AddCell(New Paragraph(nroCuenta)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo0.AddCell(New Paragraph(nroCheque)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        Dim importe As String
                                        Dim miImporte As Double
                                        If reader.Item(0) > 0 Then
                                            miImporte = reader.Item(0)
                                            totalAdisional = totalAdisional + miImporte
                                            miImporte = reader.Item(0) * -1
                                        Else
                                            miImporte = reader.Item(0) * -1
                                            totalAdisional = totalAdisional + miImporte

                                        End If
                                        'subtotal = subtotal + miImporte
                                        importe = Format(miImporte, "###,##0.00")
                                        parrafo.Add(importe)
                                        Dim CeldaImp As New PdfPCell
                                        CeldaImp.AddElement(parrafo)
                                        tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        parrafo.Clear()
                                        Documento.Add(tablademo0) 'Agrega la tabla al documento
                                    End If

                                End If

                            End If
                        Else
                            sql3 = "select Distinct cmf_FMov,cmf_Desc,mfoctb_Cod,mfo_ImpMonElem from [SBDASIPT].dbo.CabMovF " +
                            "inner join [SBDASIPT].dbo.MovF on CabMovF.cmf_ID=MovF.mfocmf_ID " +
                            "where cmftmo_Cod='REIVIA'  and mfo_ImpMonElem>0 and cmf_Desc like '%" & txtCodigo.Text & "%'"
                            Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                            tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

                            Using conn2 As New SqlConnection(CONNSTR)
                                Using cmd42 As SqlCommand = conn2.CreateCommand()

                                    conn2.Open()

                                    cmd42.CommandText = sql3

                                    Dim reader2 As SqlDataReader = Nothing
                                    reader2 = cmd42.ExecuteReader
                                    'reader.Read()
                                    'codigo = reader(0)
                                    While reader2.Read()
                                        tablademo0.AddCell(reader2.Item(0)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo0.AddCell(reader2.Item(1)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        Dim cta As String
                                        cta = reader2(2)
                                        tablademo0.AddCell(New Paragraph(cta)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        tablademo0.AddCell(New Paragraph("Deposito")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        Dim importe As String
                                        Dim miImporte As Double
                                        If reader2.Item(3) > 0 Then
                                            miImporte = reader2.Item(3)
                                            totalAdisional = totalAdisional - miImporte
                                        Else
                                            miImporte = reader2.Item(3)
                                            totalAdisional = totalAdisional + miImporte
                                            miImporte = reader2.Item(3)
                                        End If

                                        importe = Format(miImporte, "###,##0.00")
                                        parrafo.Add(importe)
                                        Dim CeldaImp As New PdfPCell
                                        CeldaImp.AddElement(parrafo)
                                        tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                        parrafo.Clear()
                                    End While


                                End Using
                            End Using
                            Documento.Add(tablademo0) 'Agrega la tabla al documento
                        End If
                    End While



                End Using
            End Using

            Dim tabla9 As New PdfPTable(5)
            'tabla9.HorizontalAlignment = ALIGN_LEFT
            tabla9.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tabla9.AddCell("TOTAL")
            tabla9.AddCell(" ")
            tabla9.AddCell(" ")
            tabla9.AddCell(" ")
            Dim miParrafo As String
            Dim miTotal As Double

            subtotal = subtotal + totalAdisional
            miTotal = Double.Parse(subtotal)

            miParrafo = Format(miTotal, "###,##0.00")
            Dim miCelda As New PdfPCell
            parrafo.Add(miParrafo)
            miCelda.AddElement(parrafo)

            miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
            tabla9.AddCell(miCelda)
            Documento.Add(tabla9)
            parrafo.Clear()



            'miTotal = Double.Parse(txtTotalFinal.Text)
            Documento.Add(New Paragraph("TOTAL:$" & miTotal & "(PESOS " & Numalet.ToCardinal(miTotal) & ")")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL") 'Salto de linea

            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente


            Documento.Close() 'Cierra el documento
            System.Diagnostics.Process.Start("OrdenPago.pdf") 'Abre el archivo DEMO.PDF
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Else

        End If
    End Sub
    Public Function buscarSobrantes() As Boolean
        Dim bandera As Boolean = False
        Dim Sql As String
        Dim totalDon As Double
        totalDon = 0
        Dim totalAdisional As Double
        totalAdisional = 0
        Sql = "Select sobrante,donacion,fechaMod,total,nombre,nroCuenta,nroCheque  from AASobrantes where NroOrden='" & adjuntarAnio() & "'"
        Dim experimento As Double
        experimento = 0
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = Sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    If (Not IsDBNull(reader(0)) And reader(0) <> 0) Then
                        bandera = True
                    End If

                End While

            End Using
        End Using

        Return bandera
    End Function
    Private Sub cargarAutorizadosComunes()
        Dim fechona As String
        Dim firmas As Integer
        firmas = 0
        sql = "Select Concepto,Fecha from AAOrdenNro where Tipo='ViaticoCargo' and NroIngreso=" & adjuntarAnio() & ""
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
        'se declara la variable Nro como String 
        Dim Nro As String
        'asigna la sentancia sql
        sql = "Select Concepto from AAOrdenNro where Tipo='ViaticoCargo' and NroIngreso=" & adjuntarAnio()
        'establece una conexiona y recorrido a la base de datos
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                'asigna a la variable Nro lo que se obtuvo en reader(0)
                If reader.Read() Then
                    Nro = reader(0)
                End If
            End Using
        End Using
        Dim nroPago As String
        sql = "Select Concepto from AAOrdenNro where Tipo='ViaticoPago' and NroIngreso=" & adjuntarAnio()
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                'mismo recorrido que el anterior sol que para obtener el numero de pago
                If reader.Read() Then
                    nroPago = reader(0)
                End If
            End Using
        End Using

        Dim Documento As New Document(PageSize.A4, 60, 5, 35, 5)
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
        parrafo.Add("ORDEN PAGO NRO:" & nroPago & "  ")
        parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("FECHA:" & fechona) 'Texto que se insertara

        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph("ORDEN CARGO ASOCIADA: " & Nro, FontFactory.GetFont("Arial", 9))) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea

        'parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
        'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        'Documento.Add(New Paragraph("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL")) 'Salto de linea

        'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        'parrafo.Add("Direccion Administracion " & vbCr & "Finanzas") 'Texto que se insertara

        'Documento.Add(parrafo) 'Agrega el parrafo al documento
        'parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        'se declara la variable res
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
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        sql2 = "SELECT Distinct [chp_FVto],[pro_RazSoc], [chpctb_Cod],[chp_NroCheq],[chp_Importe]  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
               " inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
                " inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]inner join [SBDASIPT].dbo.[CabMovF] on [CabCompra].ccocmf_ID=[CabMovF].[cmf_ID] " &
                " inner join [SBDASIPT].[dbo].[MovF] on  [CabMovF].cmf_ID=[MovF].mfocmf_ID " &
                " where CausaEmi.cem_Desc='" & adjuntarAnio() & "' GROUP BY [chp_FVto],[pro_RazSoc],[chpctb_Cod],[chp_NroCheq],[chp_Importe]"
        Dim subtotal As Double
        subtotal = 0
        Dim codigo As String
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql2
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    For i = 0 To tablademo4.NumberOfColumns
                        If i >= 5 Then
                        Else
                            Dim paraParrrafo As String
                            If IsNumeric(reader(i)) And i <> 3 Then
                                firmas = firmas + 1
                                subtotal = subtotal + reader(i)
                                Dim prob As Double
                                prob = reader(i)
                                paraParrrafo = Format(reader(i), "###,##0.00")
                                Dim miCelda2 As New PdfPCell
                                parrafo.Add(paraParrrafo)
                                miCelda2.AddElement(parrafo)
                                miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                                tablademo4.AddCell(miCelda2)
                                parrafo.Clear()
                            Else
                                'subtotal = subtotal + reader(i)
                                paraParrrafo = reader(i)
                                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                parrafo.Add(paraParrrafo) 'Texto que se insertara
                                tablademo4.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                parrafo.Clear()

                            End If
                        End If
                    Next
                End While


            End Using
        End Using
        Documento.Add(tablademo4) 'Agrega la tabla al documento
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim totalDon As Double
        totalDon = 0
        Dim totalAdisional As Double
        totalAdisional = 0
        sql = "Select sobrante,donacion,fechaMod,total,nombre,nroCuenta,nroCheque  from AASobrantes where NroOrden='" & adjuntarAnio() & "'"
        Dim experimento As Double
        experimento = 0

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    If Not reader.Item(0).Equals(Nothing) Or Not reader.Item(1).ToString.Equals("X") Or Not reader.Item(1).ToString.Equals("Y") Then
                        totalDon = totalDon + reader.Item(3)
                        Dim Donacion As String
                        Donacion = reader.Item(1).ToString

                        If Donacion = "SI" Then
                            If reader(0) = 0 Then
                            Else
                                Dim nroCuenta As String
                                Dim nroCheque As String
                                If (Not IsDBNull(reader(5)) Or Not reader(5).Equals(Nothing)) Then
                                    nroCuenta = reader(5)
                                Else
                                    nroCuenta = ""
                                End If

                                If (Not IsDBNull(reader(6)) Or Not reader(6).Equals(Nothing)) Then
                                    nroCheque = reader(6)
                                Else
                                    nroCheque = ""
                                End If

                                Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                tablademo0.AddCell(reader.Item(2)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo0.AddCell("DONACION") 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo0.AddCell(New Paragraph(nroCuenta)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo0.AddCell(New Paragraph(nroCheque)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                Dim importe As String
                                Dim miImporte As Double
                                If reader.Item(0) > 0 Then
                                    miImporte = reader.Item(0)
                                    totalAdisional = totalAdisional - miImporte
                                Else
                                    miImporte = reader.Item(0)
                                    totalAdisional = totalAdisional + miImporte
                                End If
                                'subtotal = subtotal + reader.Item(0)
                                importe = Format(miImporte, "###,##0.00") 'XXX
                                parrafo.Add(importe)
                                Dim CeldaImp As New PdfPCell
                                CeldaImp.AddElement(parrafo)
                                tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                parrafo.Clear()

                                Documento.Add(tablademo0) 'Agrega la tabla al documento
                            End If
                        Else
                            If reader.Item(0) = 0 Then

                            Else
                                If reader.Item(0) > 0 Then
                                    Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                    tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

                                    Dim nroCuenta As String
                                    Dim nroCheque As String
                                    If (Not reader(5).Equals(Nothing)) Then
                                        nroCuenta = reader(5)
                                    Else
                                        nroCuenta = ""
                                    End If

                                    If (Not IsDBNull(reader(6)) Or Not reader(6).Equals(Nothing)) Then
                                        nroCheque = reader(6)
                                    Else
                                        nroCheque = ""
                                    End If

                                    tablademo0.AddCell("DIFERENCIA REITEGRADA") 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(reader.Item(4).ToString) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph(nroCuenta)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph(nroCheque)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    Dim importe As String
                                    Dim miImporte As Double

                                    If reader.Item(0) > 0 Then
                                        miImporte = reader.Item(0) * 1 'modificacion reciente
                                        totalAdisional = totalAdisional - miImporte
                                        'totalDon = totalDon - reader.Item(0)
                                    Else

                                        miImporte = reader.Item(0) * 1
                                        totalAdisional = totalAdisional + miImporte
                                    End If

                                    importe = Format(miImporte, "###,##0.00")
                                    parrafo.Add(importe)
                                    Dim CeldaImp As New PdfPCell
                                    CeldaImp.AddElement(parrafo)
                                    tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()
                                    Documento.Add(tablademo0) 'Agrega la tabla al documento
                                Else
                                    Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                    tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                    Dim nroCuenta As String
                                    Dim nroCheque As String
                                    If (Not IsDBNull(reader(5)) Or Not reader(5).Equals(Nothing)) Then
                                        nroCuenta = reader(5)
                                    Else
                                        nroCuenta = ""
                                    End If

                                    If (Not IsDBNull(reader(6)) Or Not reader(6).Equals(Nothing)) Then
                                        nroCheque = reader(6)
                                    Else
                                        nroCheque = ""
                                    End If

                                    tablademo0.AddCell("DIFERENCIA PAGADA") 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(reader.Item(4).ToString) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph(nroCuenta)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph(nroCheque)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    Dim importe As String
                                    Dim miImporte As Double
                                    If reader.Item(0) > 0 Then
                                        miImporte = reader.Item(0) * 1 'modificacion reciente
                                        'totalDon = totalDon - reader.Item(0)
                                        totalAdisional = totalAdisional + miImporte
                                        miImporte = reader.Item(0) * -1 'modificacion reciente
                                    Else
                                        miImporte = reader.Item(0) * -1
                                        totalAdisional = totalAdisional + miImporte
                                        miImporte = reader.Item(0) * -1 'modificacion reciente
                                        'totalDon = totalDon + (reader.Item(0) * -1)
                                    End If
                                    'subtotal = subtotal + miImporte
                                    importe = Format(miImporte, "###,##0.00")
                                    parrafo.Add(importe)
                                    Dim CeldaImp As New PdfPCell
                                    CeldaImp.AddElement(parrafo)
                                    tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()
                                    Documento.Add(tablademo0) 'Agrega la tabla al documento

                                End If

                            End If

                        End If
                    Else

                        ''''''''''Codigo agregado 22/12/2014''''''''''
                        If ((reader.Item(0) > 0)) Then

                        End If
                        sql3 = "select cmf_FMov,cmf_Desc,mfoctb_Cod,mfo_ImpMonElem from [SBDASIPT].dbo.CabMovF " +
                       "inner join [SBDASIPT].dbo.MovF on CabMovF.cmf_ID=MovF.mfocmf_ID " +
                       "where cmftmo_Cod='REIVIA'  and mfo_ImpMonElem>0 and cmf_Desc like '%" & txtCodigo.Text & "%'"
                        Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                        tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

                        Using conn2 As New SqlConnection(CONNSTR)
                            Using cmd42 As SqlCommand = conn2.CreateCommand()
                                conn2.Open()
                                cmd42.CommandText = sql3

                                Dim reader2 As SqlDataReader = Nothing
                                reader2 = cmd42.ExecuteReader
                                'reader.Read()
                                'codigo = reader(0)
                                While reader2.Read()
                                    tablademo0.AddCell(reader2.Item(0)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(reader2.Item(1)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    Dim cta As String
                                    cta = reader2(2)
                                    tablademo0.AddCell(New Paragraph(cta)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph("Deposito")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    Dim importe As String
                                    Dim miImporte As Double
                                    If reader2.Item(3) > 0 Then
                                        miImporte = reader2.Item(3)
                                        totalAdisional = totalAdisional + miImporte
                                    Else
                                        miImporte = reader2.Item(3) * -1
                                        totalAdisional = totalAdisional + miImporte
                                    End If

                                    importe = Format(miImporte, "###,##0.00")
                                    parrafo.Add(importe)
                                    Dim CeldaImp As New PdfPCell
                                    CeldaImp.AddElement(parrafo)
                                    tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()
                                End While


                            End Using
                        End Using
                        Documento.Add(tablademo0) 'Agrega la tabla al documento


                    End If


                End While

            End Using
        End Using
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim tabla9 As New PdfPTable(5)
        'tabla9.HorizontalAlignment = ALIGN_LEFT
        tabla9.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tabla9.AddCell("TOTAL")
        tabla9.AddCell(" ")
        tabla9.AddCell(" ")
        tabla9.AddCell(" ")
        Dim miParrafo As String
        Dim miTotal As Double

        'miTotal = Double.Parse(totalDon)
        subtotal = subtotal + totalAdisional

        miTotal = Double.Parse(subtotal)
        '''''

        ''''''
        miParrafo = Format(miTotal, "###,##0.00")
        Dim miCelda As New PdfPCell
        parrafo.Add(miParrafo)
        miCelda.AddElement(parrafo)

        miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
        tabla9.AddCell(miCelda)
        Documento.Add(tabla9)
        parrafo.Clear()


        miTotal = Double.Parse(miTotal)
        Documento.Add(New Paragraph("TOTAL:$" & miTotal & "(PESOS " & Numalet.ToCardinal(miTotal) & ")")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        ' parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        ' parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        ' parrafo.Add("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL") 'Salto de linea

        ' Documento.Add(parrafo) 'Agrega el parrafo al documento
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim tablademo6 As New PdfPTable(2) 'declara la tabla con 4 Columnas
        tablademo6.SetWidthPercentage({150, 400}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo6.AddCell(New Paragraph("        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        tablademo6.AddCell(New Paragraph("NOTA:" & vbCr & "        " & vbCr & "        " & vbCr & "        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        Documento.Add(tablademo6)
        Dim AUX As Integer
        AUX = firmas
        While (AUX > 0)
            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("RECIBI CONFORME EL IMPORTE:________________ ")
            parrafo.Add(vbCr)
            parrafo.Add("ACLARACION DE FIRMA:________________________") 'Texto que se insertara
            parrafo.Add(vbCr)
            parrafo.Add(vbCr)
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            AUX = AUX - 1
        End While

        Documento.Add(New Paragraph(" "))
        Documento.Add(New Paragraph(" "))
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
        parrafo.Add("DEPTO. TESORERIA") 'Texto que se insertara


        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

        Documento.Close() 'Cierra el documento
        System.Diagnostics.Process.Start("OrdenPago.pdf") 'Abre el archivo DEMO.PDF
    End Sub
    Private Sub cargarAutorizadosComunes2()

        Dim fechona As String
        Dim firmas As Integer
        firmas = 0
        sql = "Select Concepto,Fecha from AAOrdenNro where Tipo='ViaticoCargo' and NroIngreso=" & adjuntarAnio() & ""
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


        'se declara la variable Nro como String 
        Dim Nro As String
        'asigna la sentancia sql
        sql = "Select Concepto from AAOrdenNro where Tipo='ViaticoCargo' and NroIngreso=" & adjuntarAnio()
        'establece una conexiona y recorrido a la base de datos
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                'asigna a la variable Nro lo que se obtuvo en reader(0)
                If reader.Read() Then
                    Nro = reader(0)
                End If
            End Using
        End Using
        Dim nroPago As String
        sql = "Select Concepto,Fecha from AAOrdenNro where Tipo='ViaticoPago' and NroIngreso=" & adjuntarAnio()
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                'mismo recorrido que el anterior sol que para obtener el numero de pago
                If reader.Read() Then
                    nroPago = reader(0)
                    fechona = reader(1)

                End If
            End Using
        End Using

        Dim Documento As New Document(PageSize.A4, 60, 5, 35, 5)
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
        parrafo.Add("ORDEN PAGO NRO:" & nroPago & "  ")
        parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("FECHA:" & fechona) 'Texto que se insertara

        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph("ORDEN CARGO ASOCIADA: " & Nro, FontFactory.GetFont("Arial", 9))) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea

        'parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
        'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        'Documento.Add(New Paragraph("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL")) 'Salto de linea

        'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        'parrafo.Add("Direccion Administracion " & vbCr & "Finanzas") 'Texto que se insertara

        'Documento.Add(parrafo) 'Agrega el parrafo al documento
        'parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        'se declara la variable res
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
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        sql2 = "SELECT Distinct [chp_FVto],[pro_RazSoc], [chpctb_Cod],[chp_NroCheq],[chp_Importe]  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
               " inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
                " inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]inner join [SBDASIPT].dbo.[CabMovF] on [CabCompra].ccocmf_ID=[CabMovF].[cmf_ID] " &
                " inner join [SBDASIPT].[dbo].[MovF] on  [CabMovF].cmf_ID=[MovF].mfocmf_ID " &
                " where CausaEmi.cem_Desc='" & adjuntarAnio() & "' GROUP BY [chp_FVto],[pro_RazSoc],[chpctb_Cod],[chp_NroCheq],[chp_Importe]"
        Dim subtotal As Double
        subtotal = 0
        Dim codigo As String
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql2
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    For i = 0 To tablademo4.NumberOfColumns
                        If i >= 5 Then
                        Else
                            Dim paraParrrafo As String
                            If IsNumeric(reader(i)) And i <> 3 Then
                                firmas = firmas + 1
                                subtotal = subtotal + reader(i)
                                Dim prob As Double
                                prob = reader(i)
                                paraParrrafo = Format(reader(i), "###,##0.00")
                                Dim miCelda2 As New PdfPCell
                                parrafo.Add(paraParrrafo)
                                miCelda2.AddElement(parrafo)
                                miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                                tablademo4.AddCell(miCelda2)
                                parrafo.Clear()
                            Else
                                'subtotal = subtotal + reader(i)
                                paraParrrafo = reader(i)
                                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                parrafo.Add(paraParrrafo) 'Texto que se insertara
                                tablademo4.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                parrafo.Clear()

                            End If
                        End If
                    Next
                End While


            End Using
        End Using
        Documento.Add(tablademo4) 'Agrega la tabla al documento
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim totalDon As Double
        totalDon = 0
        Dim totalAdisional As Double
        totalAdisional = 0
        sql = "Select sobrante,donacion,fechaMod,total,nombre,nroCuenta,nroCheque  from AASobrantes where NroOrden='" & adjuntarAnio() & "'"
        Dim experimento As Double
        experimento = 0

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    If Not reader.Item(0).Equals(Nothing) Or Not reader.Item(1).ToString.Equals("X") Or Not reader.Item(1).ToString.Equals("Y") Then
                        totalDon = totalDon + reader.Item(3)
                        Dim Donacion As String
                        Donacion = reader.Item(1).ToString

                        If Donacion = "SI" Then
                            If reader(0) = 0 Then
                            Else
                                Dim nroCuenta As String
                                Dim nroCheque As String
                                'If (Not IsDBNull(reader(5)) Or Not reader(5).Equals(Nothing)) Then
                                'nroCuenta = reader(5)
                                'Else
                                'nroCuenta = ""
                                'End If

                                'If (Not IsDBNull(reader(6)) Or Not reader(6).Equals(Nothing)) Then
                                'nroCheque = reader(6)
                                'Else
                                'nroCheque = ""
                                'End If

                                Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                tablademo0.AddCell(reader.Item(2)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo0.AddCell("DONACION") 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo0.AddCell(New Paragraph("")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo0.AddCell(New Paragraph("")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                Dim importe As String
                                Dim miImporte As Double
                                If reader.Item(0) > 0 Then
                                    miImporte = reader.Item(0)
                                    totalAdisional = totalAdisional - miImporte
                                Else
                                    miImporte = reader.Item(0)
                                    totalAdisional = totalAdisional + miImporte
                                End If
                                'subtotal = subtotal + reader.Item(0)
                                importe = Format(miImporte, "###,##0.00") 'XXX
                                parrafo.Add(importe)
                                Dim CeldaImp As New PdfPCell
                                CeldaImp.AddElement(parrafo)
                                tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                parrafo.Clear()

                                Documento.Add(tablademo0) 'Agrega la tabla al documento
                            End If
                    Else
                        If reader.Item(0) = 0 Then

                        Else
                            If reader.Item(0) > 0 Then
                                Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

                                Dim nroCuenta As String
                                Dim nroCheque As String
                                    'If (Not reader(5).Equals(Nothing)) Then
                                    '    nroCuenta = reader(5)
                                    'Else
                                    '    nroCuenta = ""
                                    'End If

                                    'If (Not IsDBNull(reader(6)) Or Not reader(6).Equals(Nothing)) Then
                                    '    nroCheque = reader(6)
                                    'Else
                                    '    nroCheque = ""
                                    'End If

                                tablademo0.AddCell("DIFERENCIA REITEGRADA") 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo0.AddCell(reader.Item(4).ToString) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph("")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph("")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                Dim importe As String
                                Dim miImporte As Double

                                If reader.Item(0) > 0 Then
                                    miImporte = reader.Item(0) * 1 'modificacion reciente
                                    totalAdisional = totalAdisional - miImporte
                                    'totalDon = totalDon - reader.Item(0)
                                Else

                                    miImporte = reader.Item(0) * 1
                                    totalAdisional = totalAdisional + miImporte
                                End If

                                importe = Format(miImporte, "###,##0.00")
                                parrafo.Add(importe)
                                Dim CeldaImp As New PdfPCell
                                CeldaImp.AddElement(parrafo)
                                tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                parrafo.Clear()
                                Documento.Add(tablademo0) 'Agrega la tabla al documento
                            Else
                                Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                Dim nroCuenta As String
                                Dim nroCheque As String
                                    'If (Not IsDBNull(reader(5)) Or Not reader(5).Equals(Nothing)) Then
                                    '    nroCuenta = reader(5)
                                    'Else
                                    '    nroCuenta = ""
                                    'End If

                                    'If (Not IsDBNull(reader(6)) Or Not reader(6).Equals(Nothing)) Then
                                    '    nroCheque = reader(6)
                                    'Else
                                    '    nroCheque = ""
                                    'End If

                                tablademo0.AddCell("DIFERENCIA PAGADA") 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo0.AddCell(reader.Item(4).ToString) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph("")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph("")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                Dim importe As String
                                Dim miImporte As Double
                                If reader.Item(0) > 0 Then
                                    miImporte = reader.Item(0) * 1 'modificacion reciente
                                    'totalDon = totalDon - reader.Item(0)
                                    totalAdisional = totalAdisional + miImporte
                                    miImporte = reader.Item(0) * -1 'modificacion reciente
                                Else
                                    miImporte = reader.Item(0) * -1
                                    totalAdisional = totalAdisional + miImporte
                                    miImporte = reader.Item(0) * -1 'modificacion reciente
                                    'totalDon = totalDon + (reader.Item(0) * -1)
                                End If
                                'subtotal = subtotal + miImporte
                                importe = Format(miImporte, "###,##0.00")
                                parrafo.Add(importe)
                                Dim CeldaImp As New PdfPCell
                                CeldaImp.AddElement(parrafo)
                                tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                parrafo.Clear()
                                Documento.Add(tablademo0) 'Agrega la tabla al documento

                            End If

                        End If

                    End If
                    Else

                        ''''''''''Codigo agregado 22/12/2014''''''''''
                        If ((reader.Item(0) > 0)) Then

                        End If
                        sql3 = "select cmf_FMov,cmf_Desc,mfoctb_Cod,mfo_ImpMonElem from [SBDASIPT].dbo.CabMovF " +
                       "inner join [SBDASIPT].dbo.MovF on CabMovF.cmf_ID=MovF.mfocmf_ID " +
                       "where cmftmo_Cod='REIVIA'  and mfo_ImpMonElem>0 and cmf_Desc like '%" & txtCodigo.Text & "%'"
                        Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                        tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

                        Using conn2 As New SqlConnection(CONNSTR)
                            Using cmd42 As SqlCommand = conn2.CreateCommand()
                                conn2.Open()
                                cmd42.CommandText = sql3

                                Dim reader2 As SqlDataReader = Nothing
                                reader2 = cmd42.ExecuteReader
                                'reader.Read()
                                'codigo = reader(0)
                                While reader2.Read()
                                    tablademo0.AddCell(reader2.Item(0)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(reader2.Item(1)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    Dim cta As String
                                    cta = reader2(2)
                                    tablademo0.AddCell(New Paragraph(cta)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph("Deposito")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    Dim importe As String
                                    Dim miImporte As Double
                                    If reader2.Item(3) > 0 Then
                                        miImporte = reader2.Item(3)
                                        totalAdisional = totalAdisional + miImporte
                                    Else
                                        miImporte = reader2.Item(3) * -1
                                        totalAdisional = totalAdisional + miImporte
                                    End If

                                    importe = Format(miImporte, "###,##0.00")
                                    parrafo.Add(importe)
                                    Dim CeldaImp As New PdfPCell
                                    CeldaImp.AddElement(parrafo)
                                    tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()
                                End While


                            End Using
                        End Using
                        Documento.Add(tablademo0) 'Agrega la tabla al documento


                    End If


                End While

            End Using
        End Using
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim tabla9 As New PdfPTable(5)
        'tabla9.HorizontalAlignment = ALIGN_LEFT
        tabla9.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tabla9.AddCell("TOTAL")
        tabla9.AddCell(" ")
        tabla9.AddCell(" ")
        tabla9.AddCell(" ")
        Dim miParrafo As String
        Dim miTotal As Double

        'miTotal = Double.Parse(totalDon)
        subtotal = subtotal + totalAdisional

        miTotal = Double.Parse(subtotal)
        '''''

        ''''''
        miParrafo = Format(miTotal, "###,##0.00")
        Dim miCelda As New PdfPCell
        parrafo.Add(miParrafo)
        miCelda.AddElement(parrafo)

        miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
        tabla9.AddCell(miCelda)
        Documento.Add(tabla9)
        parrafo.Clear()


        miTotal = Double.Parse(miTotal)
        Documento.Add(New Paragraph("TOTAL:$" & miTotal & "(PESOS " & Numalet.ToCardinal(miTotal) & ")")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        ' parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        ' parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        ' parrafo.Add("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL") 'Salto de linea

        ' Documento.Add(parrafo) 'Agrega el parrafo al documento
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim tablademo6 As New PdfPTable(2) 'declara la tabla con 4 Columnas
        tablademo6.SetWidthPercentage({150, 400}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo6.AddCell(New Paragraph("        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        tablademo6.AddCell(New Paragraph("NOTA:" & vbCr & "        " & vbCr & "        " & vbCr & "        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        Documento.Add(tablademo6)
        Dim AUX As Integer
        AUX = firmas
        While (AUX > 0)
            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("RECIBI CONFORME EL IMPORTE:________________ ")
            parrafo.Add(vbCr)
            parrafo.Add("ACLARACION DE FIRMA:________________________") 'Texto que se insertara
            parrafo.Add(vbCr)
            parrafo.Add(vbCr)
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            AUX = AUX - 1
        End While

        Documento.Add(New Paragraph(" "))
        Documento.Add(New Paragraph(" "))
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
        parrafo.Add("DEPTO. TESORERIA") 'Texto que se insertara


        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

        Documento.Close() 'Cierra el documento
        System.Diagnostics.Process.Start("OrdenPago.pdf") 'Abre el archivo DEMO.PDF
    End Sub
    Private Sub imprimirPagoAutorizado()
        If (buscarSobrantes()) Then
            Select Case MsgBox("¿Existen sobrantes relacionados con la orden, desea asignar/modificar algun cuenta/cheque ?", MsgBoxStyle.YesNo, "AVISO")
                Case MsgBoxResult.Yes
                    'xxxxxxxxxxxxxxxxxxxx
                    'Sobrantes.txtCodigo.Text = txtCodigo.Text
                    'Sobrantes.cmbAnio.Text = cmbAnio.SelectedItem.ToString
                    'Sobrantes.Show()
                    ''xxxxxxxxxxxxxxxxxxxx
                Case MsgBoxResult.No
                    'cargarAutorizadosComunes()
                    cargarAutorizadosComunes2()

                    'MessageBox.Show("Accion cancelada por el usuario", "INFORMACION")
            End Select
        Else
            cargarAutorizadosComunes()
        End If


    End Sub

#End Region

#Region "Codigos Auxiliares"

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
                                    day += reader(4)
                                End If
                                row.Cells("DIAS").Value = day
                            End If
                        Next

                    Next


                Loop
            End Using
        End Using
        calculoMonto()
    End Sub

    Public Sub ordenCargo()
        sql = "SELECT [chp_NroCheq],[chpctb_Cod],[chp_Importe],[pro_RazSoc]  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
              "inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
              "inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]" &
              "where CausaEmi.cem_Desc='" & adjuntarAnio() & "'"

        DataGridView1.Columns.Add("destinatario", "DESTINATARIO")
        DataGridView1.Columns.Add("ctaCte", "CUENTA CORRIENTE")
        DataGridView1.Columns.Add("cheques", "CHEQUES")

        DataGridView1.Columns.Add("nombre", "IMPORTE")
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
                    fila(0) = reader(3)
                    fila(1) = reader(1)
                    fila(2) = reader(0)
                    fila(3) = reader(2)
                    fila(4) = ""
                    DataGridView1.Rows.Add(fila)

                Loop

            End Using
        End Using
        calculoTotal()
    End Sub

    Private Sub calculoTotal()

        Dim Total As Double = 0


        For Each row As DataGridViewRow In Me.DataGridView1.Rows
            Total += row.Cells("nombre").Value
        Next

        txtTotalFinal.Text = Total.ToString

    End Sub

    Public Sub generarRendicion()
        'consulta sql  que trae la descripcion de todos los items de compra  
        'del viatico de una determinada causa de emsion
        sql = "SELECT distinct  ItemComp.ico_Desc FROM CabCompra  INNER JOIN CausaEmi ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod INNER JOIN  ItemComp ON CabCompra.cco_ID=ItemComp.icocco_ID  WHERE  CausaEmi.cem_Desc='" & adjuntarAnio() & "' and [ccotco_Cod]<>'RVI' ORDER BY ItemComp.ico_Desc"

        'conecta con la base de datos y ejectua la consulta sql
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                'agrega las columnas Nombre y dias a el datagrid
                DataGridView1.Columns.Add("Nombre", "NOMBRE")
                DataGridView1.Columns.Add("Dias", "DIAS")

                Dim count As Integer
                count = 0
                Do While reader.Read()
                    DataGridView1.Columns.Add(reader(0), reader(0))
                    'indice = DataGridView1.Columns("viatico" & count).Index.ToString()
                    count = count + 1
                Loop
                'agraga al datagrid las columnas devengados, a pagar, a reintegrar ,firma
                DataGridView1.Columns.Add("Devengado", "DEVENGADO")
                DataGridView1.Columns.Add("A pagar", "A PAGAR")
                DataGridView1.Columns.Add("Reintegrar", "A REINTEGRAR")
                DataGridView1.Columns.Add("Firma", "FIRMA")
            End Using
        End Using
        'carga todos los nombres de las personas involucradas en la rendicion
        cargarNombresRendicion()
        'carga todas las personas involucradas pero los que estan dentro del datagrid en el combo box
        cargarComboNombres()

        'calculoMonto()
        'Esta funcion recorre el DataGridView1 y suma el total del contenido de la columna Devengado.
        calculoTotalViatico()

        'Esta funcion recorre el DataGridView1 y suma el total del contenido de la columna dias y lo asigna
        'en la caja de texto, txtDia
        calculoTotalDias()

        'Esta funcion recorre el DataGridView1 y suma el total del contenido de la columna Combustible y lo asigna
        'en la caja de texto txtPeaje. 
        calculoTotalCombustible()

        'Esta funcion recorre el DataGridView1 y suma el total del contenido de la columna peajes y lo asigna
        'en la caja de texto txtPeaje. 
        calculoTotalPeajes()

        'Esta funcion recorre el DataGridView1 y suma el total del contenido de la columna Viaticos A rendir y lo asigna
        'en la caja de texto, txtViaticos.
        calculoTotalViaticoMisiones()

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
                                    day += reader(4)

                                End If
                                row.Cells("DIAS").Value = day
                            End If

                        Next

                    Next


                Loop
            End Using
        End Using
        calculoMonto()
    End Sub

    Private Function dameNroFondo() As String
        sql = "select max (cmf_ID) from CabMovF"
        Dim nroOrden As String
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                reader.Read()

                If reader.HasRows Then
                    nroOrden = reader(0)
                Else

                End If

            End Using
        End Using

        Return nroOrden
    End Function

    Private Function dameNroMovFondo() As String
        sql = "select max (mfocmf_ID) from MovF"
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
                    nroOrden = 1
                End If

            End Using
        End Using

        Return nroOrden
    End Function
    Dim montoCheque As Double
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

        'sql = "SELECT [chp_NroCheq],[chpctb_Cod],[chp_Importe],[pro_RazSoc],[chp_FVto]  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
        '      "inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
        '      "inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]" &
        '      "where CausaEmi.cem_Desc='" & adjuntarAnio() & "'"

        'DataGridView1.Columns.Add("fecha", "FECHA EMISION")
        'DataGridView1.Columns.Add("destinatario", "BENEFICIARIO")
        'DataGridView1.Columns.Add("ctaCte", "CUENTA CORRIENTE")
        'DataGridView1.Columns.Add("cheques", "NRO CHEQUE")

        'DataGridView1.Columns.Add("importe", "IMPORTE")
        'DataGridView1.Columns.Item("importe").DefaultCellStyle.Format = "###,##0.00"

        'montoCheque = 0
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
        '            fila(0) = reader(4)
        '            fila(1) = reader(3)
        '            fila(2) = reader(1)
        '            fila(3) = reader(0)
        '            fila(4) = reader(2)

        '            DataGridView1.Rows.Add(fila)
        '            montoCheque = montoCheque + reader(2)
        '        Loop
        '    End Using
        'End Using
        'txtTotalFinal.Text = montoCheque
        adjuntarPagos()
    End Sub

    Public Sub adjuntarPagos()
        sql = "SELECT nombre,total,sobrante FROM [dbo].[AASobrantes] where nroOrden=" & adjuntarAnio()
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
                                row.Cells("IMPORTE").Value = reader(1) + reader(2)

                            End If

                        Next

                    Next

                Loop
            End Using
        End Using
        calcularMontoPago()
    End Sub
    'recorre el datagrid1 y asigna 0 a la columnas A pagar y Reintegar
    'luego recorre el datagrid2 y suma el total de la columna Monto por cada proveedor.

    Public Sub agregarReintegros()
        Dim total As Double = 0
        Dim tipito As String
        'recorre el DataGrid1 y coloca 0  en las columnas "A pagar" y "Reintegrar"
        For Each row2 As DataGridViewRow In Me.DataGridView1.Rows
            total = 0
            row2.Cells("A pagar").Value = 0
            row2.Cells("Reintegrar").Value = 0

            'por cada ves que se avance el DataGrid1 se recorre toda la columna DataGrid2
            'si el nombre en el datagrid1 es igual al nombre en el datagrid2 se suma la columna monto del dataGrid2
            For Each row As DataGridViewRow In Me.DataGridView2.Rows
                If row.Cells(3).Value = row2.Cells(0).Value Then
                    total += row.Cells("MONTO").Value
                End If
            Next
            'asigna el valor en la celda 0 en la variable tipito
            tipito = row2.Cells(0).Value
            'si el total del viatico que se le dio es mayor a lo que gasto
            'si lo que gasto es mas de lo que le dimos hay que pagarle
            If total > row2.Cells("Devengado").Value Then
                Dim Pago As Double = (total - row2.Cells("Devengado").Value) * -1

                'a devengado se le asigna a la coumna "Devengado" del dataGrid1 el total
                'y la columna a pagar se le asigna pago.
                row2.Cells("Devengado").Value = total
                row2.Cells("A pagar").Value = Math.Round(Pago, 2)
                row2.Cells("Reintegrar").Value = 0
                GroupBox11.Visible = True
                chkPago.Visible = True
            Else
                'Si lo que gasto es menor a lo que le dimos entonces nos tiene que devolver.
                If total < row2.Cells("Devengado").Value Then
                    Dim reintegro As Double = (total - row2.Cells("Devengado").Value) * -1
                    row2.Cells("Reintegrar").Value = Math.Round(reintegro, 2)
                    row2.Cells("A pagar").Value = 0
                    GroupBox10.Visible = True
                    chkReintegro.Visible = True
                    row2.Cells("Devengado").Value = (row2.Cells("Devengado").Value - reintegro)
                Else
                    If total = row2.Cells("Devengado").Value Then
                        row2.Cells("Reintegrar").Value = 0
                        row2.Cells("A pagar").Value = 0
                    End If
                End If
            End If
        Next

    End Sub
    'busca la fecha de salida de la cuasa de emision y la asigna de dtSalida
    Public Sub establecerFechaInicio()
        sql = " SELECT  distinct cco_FEmision FROM CabCompra  INNER JOIN CausaEmi ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod INNER JOIN  ItemComp ON CabCompra.cco_ID=ItemComp.icocco_ID  WHERE  CausaEmi.cem_Desc='" & adjuntarAnio() & "' group BY CabCompra.cco_FEmision"
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                Do While reader.Read()
                    'Dim fila(1) As Object
                    'fila(0) = reader(0)
                    dtSalida.Value = reader(0)
                Loop

            End Using
        End Using
        calcularCargosMora()
    End Sub

    Public Function ControlCierre() As Boolean

    End Function

    Private Function dameCMF() As ArrayList

        Dim lista As New ArrayList

        sql = "SELECT cmf_ID   FROM [SBDASIPT].[dbo].[CabMovF] inner join SBDASIPT.dbo.CabCompra on CabMovF.cmf_ID=CabCompra.ccocmf_ID " &
  "inner join [SBDASIPT].dbo.CausaEmi on CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
  "inner join [SBDASIPT].[dbo].[MovF] on  [CabMovF].cmf_ID=[MovF].mfocmf_ID " &
  "where cem_Desc=" & adjuntarAnio() & " and mfobco_Cod not like 'null'  and cmf_FMov=(select min(cmf_FMov) from [SBDASIPT].dbo.[CabMovF])"

        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()

                conn.Open()

                cmd4.CommandText = sql

                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader

                Do While reader.Read()
                    lista.Add(reader(0))
                Loop

            End Using
        End Using
        Return lista
    End Function

    Private Function AlmacenarRendicion() As Boolean
        Dim bandera As String

        'Try
        For Each Row As DataGridViewRow In DataGridView2.Rows ' por cada uno de los elementos del DataGridView2
            Dim value As Object
            value = Row.Cells(0).Value
            If ((value Is Nothing) OrElse (value Is DBNull.Value)) Then

            Else
                Using conn As New SqlConnection(CONNSTR)
                    Using cmd As SqlCommand = conn.CreateCommand()

                        conn.Open()

                        cmd.CommandText = "insert into AARendiciones(" +
                                       "[Fecha]" +
                                       ",[Descripcion]" +
                                       ",[Monto]" +
                                       ",[Nombre]" +
                                       ",[NroInreso]" +
                                       ",[Destino]" +
                                        ") VALUES (" +
                                        "@Fecha" +
                                        ",@Descripcion" +
                                        ",@Monto" +
                                        ",@Nombre" +
                                        ",@NroInreso" +
                                        ",@Destino" +
                                        ")"


                        cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = Row.Cells(0).Value
                        cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 50).Value = Row.Cells(1).Value
                        cmd.Parameters.Add("@Monto", SqlDbType.Decimal).Value = Row.Cells(2).Value
                        cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 30).Value = Row.Cells(3).Value
                        cmd.Parameters.Add("@NroInreso", SqlDbType.VarChar, 10).Value = adjuntarAnio()
                        cmd.Parameters.Add("@Destino", SqlDbType.VarChar, 50).Value = txtDestino.Text

                        cmd.ExecuteScalar()


                    End Using
                End Using
            End If

        Next
        bandera = True
        MessageBox.Show("Se almacenaron de manera correcta los gastos", "EXITO")
        'Catch ex As Exception
        '    bandera = False
        '    MessageBox.Show("No se almacenaron de manera correcta los gastos", "AVISO")
        'End Try
        Return bandera
    End Function
    'Esta funcion inserta una nueva tupla en la tabla AAOrdenNro con los parametros, nro,origen,fecha actual, y causa
    'de emision de ingresado de la ventana.
    Private Function almacenarOrden(ByRef nro As String, ByRef Origen As String) As Boolean
        'se declaro la variable booleana bandera
        Dim bandera As Boolean

        Try
            'se conecta con la base de datos y se ejectua la consulta
        Using conn As New SqlConnection(CONNSTR)
            Using cmd As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    'consulta sql que inserta una nueva tupla en la tabla AAOrdenNro
                    'con los nuevos datos nro,causa de emision en la ventana, un string viatico Cargo
                    'origen y la fecha actual.
                    cmd.CommandText = "insert into AAOrdenNro(" +
                               "[NroIngreso]" +
                               ",[Concepto]" +
                               ",[Tipo]" +
                               ",[NacPro]" +
                               ",[Fecha]" +
                                ") VALUES (" +
                                "@NroIngreso" +
                                ",@Concepto" +
                                ",@Tipo" +
                                ",@NacPro" +
                                ",@Fecha" +
                                ")"
                    cmd.Parameters.Add("@Concepto", SqlDbType.Int).Value = nro
                    cmd.Parameters.Add("@NroIngreso", SqlDbType.Int).Value = adjuntarAnio()
                cmd.Parameters.Add("@Tipo", SqlDbType.VarChar, 50).Value = "ViaticoCargo"
                cmd.Parameters.Add("@NacPro", SqlDbType.VarChar, 3).Value = Origen
                cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = Date.Now
                cmd.ExecuteScalar()
                End Using
            End Using
            bandera = True
            MessageBox.Show("Se almaceno de manera correcta la orden", "EXITO")
        Catch ex As Exception
            MessageBox.Show("No se almaceno de manera correcta la orden", "AVISO")
            bandera = False
        End Try
        Return bandera
    End Function

    Private Function almacenarOrdenRendicion(ByRef nro As String) As Boolean

        Dim bandera As Boolean
        Try
            Using conn As New SqlConnection(CONNSTR)
                Using cmd As SqlCommand = conn.CreateCommand()

                    conn.Open()

                    cmd.CommandText = "insert into AAOrdenNro(" +
                                   "[NroIngreso]" +
                                   ",[Concepto]" +
                                   ",[Tipo]" +
                                   ",[NacPro]" +
                                   ",[Fecha]" +
                                    ") VALUES (" +
                                    "@NroIngreso" +
                                    ",@Concepto" +
                                    ",@Tipo" +
                                    ",@NacPro" +
                                    ",@Fecha" +
                                    ")"


                    cmd.Parameters.Add("@Concepto", SqlDbType.Int).Value = nro
                    cmd.Parameters.Add("@NroIngreso", SqlDbType.Int).Value = adjuntarAnio()
                    cmd.Parameters.Add("@Tipo", SqlDbType.VarChar, 50).Value = "ViaticoRendicion"
                    cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = Date.Now
                    cmd.Parameters.Add("@NacPro", SqlDbType.VarChar, 3).Value = "RVI"
                    cmd.ExecuteScalar()


                End Using
            End Using
            bandera = True
            MessageBox.Show("Se almaceno de manera correcta la orden de rendición", "EXITO")
        Catch ex As Exception
            bandera = False
            MessageBox.Show("no se almaceno de manera correcta la orden de rendición", "AVISO")
        End Try
        Return bandera
    End Function

    Private Function almacenarOrdenPago(ByRef nro As String, ByRef origen As String) As Boolean
        Dim bandera As Boolean = False
        Try

            Using conn As New SqlConnection(CONNSTR)
                Using cmd As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd.CommandText = "insert into AAOrdenNro(" +
                              "[NroIngreso]" +
                              ",[Concepto]" +
                              ",[Tipo]" +
                              ",[NacPro]" +
                              ",[Fecha]" +
                               ") VALUES (" +
                               "@NroIngreso" +
                               ",@Concepto" +
                               ",@Tipo" +
                               ",@NacPro" +
                               ",@Fecha" +
                               ")"


                    cmd.Parameters.Add("@Concepto", SqlDbType.Int).Value = nro

                    cmd.Parameters.Add("@NroIngreso", SqlDbType.Int).Value = adjuntarAnio()
                    cmd.Parameters.Add("@Tipo", SqlDbType.VarChar, 50).Value = "ViaticoPago"
                    cmd.Parameters.Add("@NacPro", SqlDbType.VarChar, 3).Value = origen
                    cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = Date.Now
                    cmd.ExecuteScalar()


                End Using
            End Using
            bandera = True
            MessageBox.Show("Se almaceno de manera correcta la orden de pago", "EXITO")
        Catch ex As Exception
            bandera = False
            MessageBox.Show("no se almaceno de manera correcta la orden de pago", "AVISO")
        End Try
        Return bandera
    End Function

#End Region

#Region "HISTORIALES"

    Public Sub imprimirHistorialRendicion()
        Dim fechona As String
        sql = "Select Concepto,Fecha from AAOrdenNro where Tipo='ViaticoRendicion' and NroIngreso=" & adjuntarAnio()
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

        Dim Documento As New Document()
        Dim parrafo As New Paragraph
        Dim subGastos As Double
        Documento.SetPageSize(iTextSharp.text.PageSize.A4.Rotate())
        pdf.PdfWriter.GetInstance(Documento, New FileStream("Rendicion.pdf", FileMode.Create))

        Documento.Open()


        If DataGridView1.Columns.Count = 9 Then
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

                parrafo.Add("Fecha de Impresion:" & fechona & "  ")
                parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                parrafo.Add("ORDEN NRO: " & dameNroHistorial("ViaticoRendicion"))

                parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                parrafo.Add("NRO DE ENTRADA: " & txtCodigo.Text & "                         ") 'Texto que se insertara

                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                parrafo.Add("FECHA SALIDA: " & dtSalida.Text) 'Texto que se insertara

                Documento.Add(parrafo) 'Agrega el parrafo al documento
                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                Documento.Add(New Paragraph(" ")) 'Salto de linea

                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                parrafo.Add("FECHA LLEGADA:" & dtLlegada.Text) 'Texto que se insertara

                Documento.Add(parrafo) 'Agrega el parrafo al documento
                parrafo.Clear()

                Documento.Add(New Paragraph(" ")) 'Salto de linea

                'FIN DEL ENCABEZADO

                'ENCABEZADO DE LA TABLA 

                Dim tablademo As New PdfPTable(8) 'declara la tabla con 4 Columnas
                tablademo.SetWidthPercentage({100, 40, 70, 40, 70, 70, 70, 70}, PageSize.A4)
                tablademo.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                tablademo.AddCell(New Paragraph("NOMBRE        ", FontFactory.GetFont("Arial", 12)))
                tablademo.AddCell(New Paragraph("DIAS", FontFactory.GetFont("Arial", 12)))
                tablademo.AddCell(New Paragraph("COMBUSTIBLE", FontFactory.GetFont("Arial", 12)))
                tablademo.AddCell(New Paragraph("PEAJES", FontFactory.GetFont("Arial", 12)))
                tablademo.AddCell(New Paragraph("VIATICOS MISIONES", FontFactory.GetFont("Arial", 12)))
                tablademo.AddCell(New Paragraph("DEVENGADO", FontFactory.GetFont("Arial", 12)))
                tablademo.AddCell(New Paragraph("A PAGAR", FontFactory.GetFont("Arial", 12)))
                tablademo.AddCell(New Paragraph("A REINTEGRAR", FontFactory.GetFont("Arial", 12)))


                Documento.Add(tablademo) 'Agrega la tabla al documento

                'FIN DEL ENCABEZADO DE LA TABLA

                'CUERPO DE LA TABLA

                Dim tablademo2 As New PdfPTable(8) 'declara la tabla con 4 Columnas
                tablademo2.SetWidthPercentage({100, 40, 70, 40, 70, 70, 70, 70}, PageSize.A4) 'Ajusta el tamaño de cada columna
                tablademo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED

                For Each column As DataGridViewColumn In DataGridView1.Columns
                    If column.Index <= 8 Then
                        Dim paraParrrafo As String
                        If IsNumeric(row.Cells(column.Index).Value) And Not column.Index = 1 Then
                            paraParrrafo = Format(row.Cells(column.Index).Value, "###,##0.00")
                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                        Else
                            If IsNumeric(row.Cells(column.Index).Value) And column.Index = 1 Then
                                paraParrrafo = row.Cells(column.Index).Value
                                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                            Else
                                paraParrrafo = row.Cells(column.Index).Value
                                parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                            End If

                        End If
                        parrafo.Add(paraParrrafo) 'Texto que se insertara
                        Dim celda100 As New PdfPCell
                        celda100.AddElement(parrafo)
                        tablademo2.AddCell(celda100) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                        parrafo.Clear()
                    End If

                Next
                Documento.Add(tablademo2)

                Dim tablademo3 As New PdfPTable(8) 'declara la tabla con 4 Columnas

                tablademo3.SetWidthPercentage({100, 40, 70, 40, 70, 70, 70, 70}, PageSize.A4) 'Ajusta el tamaño de cada columna
                'tablademo3.HorizontalAlignment = Element.ALIGN_LEFT

                tablademo3.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

                Dim myDay As Integer
                Dim praZo As String
                myDay = row.Cells(1).Value.ToString
                praZo = myDay
                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                parrafo.Add(praZo) 'Texto que se insertara
                Dim CELDA89 As New PdfPCell
                CELDA89.AddElement(parrafo)
                tablademo3.AddCell(CELDA89) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                parrafo.Clear()


                For i As Integer = 2 To DataGridView1.Columns.Count - 1
                    If row.Cells(i).Value = Nothing Then
                        Dim paraP As String
                        Dim praD As Double
                        praD = 0
                        paraP = Format(praD, "###,##0.00")

                        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                        parrafo.Add(paraP) 'Texto que se insertara
                        Dim CELDA As New PdfPCell
                        CELDA.AddElement(parrafo)
                        tablademo3.AddCell(CELDA) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                        parrafo.Clear()
                    Else

                        Dim paraParrrafo As String
                        If IsNumeric(row.Cells(i).Value) Then
                            paraParrrafo = Format(row.Cells(i).Value, "###,##0.00")

                        Else
                            paraParrrafo = row.Cells(i).Value
                        End If
                        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                        parrafo.Add(paraParrrafo) 'Texto que se insertara
                        Dim CELDA As New PdfPCell
                        CELDA.AddElement(parrafo)
                        tablademo3.AddCell(CELDA) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                        parrafo.Clear()

                    End If
                Next

                Documento.Add(tablademo3) 'Agrega la tabla al documento

                Documento.Add(New Paragraph(" ")) 'Salto de linea
                Documento.Add(New Paragraph(" ")) 'Salto de linea

                parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado

                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                parrafo.Add("PLANILLA DE GASTOS Y VIATICOS") 'Texto que se insertara

                Documento.Add(parrafo) 'Agrega el parrafo al documento
                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                Documento.Add(New Paragraph(" ")) 'Salto de linea

                For Each row1 As DataGridViewRow In DataGridView2.Rows
                    If row.Cells(0).Value = row1.Cells(3).Value Then
                        Dim tablademo4 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                        'tablademo4.HorizontalAlignment = ALIGN_LEFT
                        tablademo4.SetWidthPercentage({100, 300, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                        For Each column As DataGridViewColumn In DataGridView2.Columns
                            If column.Index <= 3 Then
                                Dim paraParrrafo As String
                                If IsNumeric(row1.Cells(column.Index).Value) Then
                                    Dim VARIABLE As Double
                                    VARIABLE = row1.Cells(column.Index).Value
                                    subGastos = subGastos + row1.Cells(column.Index).Value
                                    paraParrrafo = Format(VARIABLE, "###,##0.00")
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
                        Documento.Add(tablademo4) 'Agrega la tabla al documento
                    End If

                    'End If
                Next
                Dim tablademo9 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                'tablademo9.HorizontalAlignment = ALIGN_LEFT
                tablademo9.SetWidthPercentage({100, 300, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                tablademo9.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                tablademo9.AddCell(New Paragraph(" ")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                Dim miParrafo As String
                Dim miTotal As Double
                miTotal = Double.Parse(subGastos)
                miParrafo = Format(miTotal, "###,##0.00")
                Dim miCelda As New PdfPCell
                parrafo.Add(miParrafo)
                miCelda.AddElement(parrafo)
                subGastos = 0
                miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
                tablademo9.AddCell(miCelda)
                Documento.Add(tablademo9)
                parrafo.Clear()
                Documento.Add(New Paragraph(" ")) 'Salto de linea


                parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                parrafo.Add(" Firma Interesado " & "                 " & "         Encargado Rendicion " & "                             " & "Direccion Administracion " & vbCr & "  " & "                                         Ordenes de Cargo         " & "                                  " & " Finanzas") 'Texto que se insertara


                'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                'parrafo.Add("Direccion Administracion " & vbCr & "Finanzas") 'Texto que se insertara

                Documento.Add(parrafo) 'Agrega el parrafo al documento
                parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                Documento.Add(New Paragraph(" ")) 'Salto de linea
                Documento.Add(New Paragraph(" ")) 'Salto de linea

                Documento.NewPage()

            Next

            'Agrega la tabla al documento

            'FIN DEL CUERPO DE LA TABLA

        Else
            If DataGridView1.Columns.Count >= 10 Then
                For Each row As DataGridViewRow In DataGridView1.Rows
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

                    parrafo.Add("Fecha de Impresion:" & fechona & "  ")
                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("ORDEN NRO: " & dameNroHistorial("ViaticoRendicion"))
                    parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("NRO DE ENTRADA: " & txtCodigo.Text & "                         ") 'Texto que se insertara

                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("FECHA SALIDA: " & dtSalida.Text) 'Texto que se insertara

                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                    Documento.Add(New Paragraph(" ")) 'Salto de linea

                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("FECHA LLEGADA:" & dtLlegada.Text) 'Texto que se insertara

                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear()

                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Dim tablademo As New PdfPTable(DataGridView1.Columns.Count) 'declara la tabla con 4 Columnas
                    tablademo.TotalWidth = 80
                    tablademo.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                    For Each miCol As DataGridViewColumn In DataGridView1.Columns
                        tablademo.AddCell(New Paragraph(miCol.Name, FontFactory.GetFont("Arial", 12)))
                    Next
                    Documento.Add(tablademo) 'Agrega la tabla al documento

                    Dim tablademo2 As New PdfPTable(DataGridView1.Columns.Count) 'declara la tabla con 4 Columnas
                    tablademo2.TotalWidth = 90 'Ajusta el tamaño de cada columna
                    tablademo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED

                    For Each column As DataGridViewColumn In DataGridView1.Columns
                        If column.Index <= DataGridView1.Columns.Count Then
                            Dim paraParrrafo As String
                            If IsNumeric(row.Cells(column.Index).Value) And Not column.Index = 1 Then
                                paraParrrafo = Format(row.Cells(column.Index).Value, "###,##0.00")
                                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                            Else
                                If IsNumeric(row.Cells(column.Index).Value) And column.Index = 1 Then
                                    paraParrrafo = row.Cells(column.Index).Value
                                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                Else
                                    paraParrrafo = row.Cells(column.Index).Value
                                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                End If

                            End If
                            parrafo.Add(paraParrrafo) 'Texto que se insertara
                            Dim celda100 As New PdfPCell
                            celda100.AddElement(parrafo)
                            tablademo2.AddCell(celda100) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            parrafo.Clear()
                        End If

                    Next
                    Documento.Add(tablademo2)
                    Dim tablademo3 As New PdfPTable(DataGridView1.Columns.Count) 'declara la tabla con 4 Columnas

                    tablademo3.TotalWidth = 90 'Ajusta el tamaño de cada columna
                    tablademo3.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                    tablademo3.AddCell(New Paragraph(row.Cells(1).Value.ToString)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                    For i As Integer = 2 To DataGridView1.Columns.Count - 1
                        If row.Cells(i).Value = Nothing Then
                            tablademo3.AddCell(New Paragraph("0,00"))
                        Else

                            Dim paraParrrafo As String
                            If IsNumeric(row.Cells(i).Value) Then
                                paraParrrafo = Format(row.Cells(i).Value, "###,##0.00")

                            Else
                                paraParrrafo = row.Cells(i).Value
                            End If
                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                            parrafo.Add(paraParrrafo) 'Texto que se insertara
                            tablademo3.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            parrafo.Clear()

                        End If
                    Next

                    Documento.Add(tablademo3) 'Agrega la tabla al documento

                    Documento.Add(New Paragraph(" ")) 'Salto de linea


                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado

                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("PLANILLA DE GASTOS Y VIATICOS") 'Texto que se insertara

                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea

                    For Each row1 As DataGridViewRow In DataGridView2.Rows

                        If row.Cells(0).Value = row1.Cells(3).Value Then
                            ' If row.Cells(2).Value = Nothing Or row.Cells(3).Value = Nothing Then
                            'Documento.Add(New Paragraph(" NO HUBO DECLARACION DE GASTOS ")) 'Salto de linea
                            'Else
                            Dim tablademo4 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                            'tablademo4.HorizontalAlignment = ALIGN_LEFT
                            tablademo4.SetWidthPercentage({100, 300, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                            For Each column As DataGridViewColumn In DataGridView2.Columns
                                If column.Index <= 3 Then
                                    Dim paraParrrafo As String
                                    If IsNumeric(row1.Cells(column.Index).Value) Then
                                        subGastos = subGastos + row1.Cells(column.Index).Value
                                        Dim VARIABLE As Double
                                        VARIABLE = row1.Cells(column.Index).Value
                                        paraParrrafo = Format(VARIABLE, "###,##0.00")
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
                            Documento.Add(tablademo4) 'Agrega la tabla al documento
                        End If

                        'End If
                    Next
                    Dim tablademo9 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                    'tablademo9.HorizontalAlignment = ALIGN_LEFT
                    tablademo9.SetWidthPercentage({100, 300, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                    tablademo9.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                    tablademo9.AddCell(New Paragraph(" ")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                    Dim miParrafo As String
                    Dim miTotal As Double
                    miTotal = Double.Parse(subGastos)
                    miParrafo = Format(miTotal, "###,##0.00")
                    Dim miCelda As New PdfPCell
                    parrafo.Add(miParrafo)
                    miCelda.AddElement(parrafo)
                    subGastos = 0
                    miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
                    tablademo9.AddCell(miCelda)
                    Documento.Add(tablademo9)
                    parrafo.Clear()
                    Documento.Add(New Paragraph(" ")) 'Salto de linea


                    parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("            " & "          Encargado Rendicion " & "                             " & "Direccion Administracion " & vbCr & "" & "             Ordenes de Cargo         " & "                                  " & "Finanzas") 'Texto que se insertara

                    'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    'parrafo.Add("Direccion Administracion " & vbCr & "Finanzas") 'Texto que se insertara

                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.NewPage()
                Next
            Else
                For Each row As DataGridViewRow In DataGridView1.Rows
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

                    parrafo.Add("Fecha de Impresion:" & fechona & "  ")
                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("ORDEN NRO:" & dameNroHistorial("ViaticoCargo") & "  ")
                    parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_MIDDLE) 'Asigan fuente
                    parrafo.Add("NRO DE ENTRADA: " & txtCodigo.Text & "                         ") 'Texto que se insertara
                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("FECHA SALIDA: " & dtSalida.Text) 'Texto que se insertara

                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

                    Documento.Add(New Paragraph(" ")) 'Salto de linea

                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("FECHA LLEGADA:" & dtLlegada.Text) 'Texto que se insertara

                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear()

                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Dim tablademo As New PdfPTable(DataGridView1.Columns.Count) 'declara la tabla con 4 Columnas
                    tablademo.TotalWidth = 90
                    tablademo.HorizontalAlignment = Element.ALIGN_JUSTIFIED
                    For Each miCol As DataGridViewColumn In DataGridView1.Columns
                        tablademo.AddCell(New Paragraph(miCol.Name, FontFactory.GetFont("Arial", 12)))
                    Next
                    Documento.Add(tablademo) 'Agrega la tabla al documento

                    Dim tablademo2 As New PdfPTable(DataGridView1.Columns.Count) 'declara la tabla con 4 Columnas
                    tablademo2.TotalWidth = 90 'Ajusta el tamaño de cada columna
                    tablademo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED

                    For Each column As DataGridViewColumn In DataGridView1.Columns
                        If column.Index <= DataGridView1.Columns.Count Then
                            Dim paraParrrafo As String
                            If IsNumeric(row.Cells(column.Index).Value) And Not column.Index = 1 Then
                                paraParrrafo = Format(row.Cells(column.Index).Value, "###,##0.00")
                                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                            Else
                                If IsNumeric(row.Cells(column.Index).Value) And column.Index = 1 Then
                                    paraParrrafo = row.Cells(column.Index).Value
                                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                Else
                                    paraParrrafo = row.Cells(column.Index).Value
                                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
                                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                End If

                            End If
                            parrafo.Add(paraParrrafo) 'Texto que se insertara
                            Dim celda100 As New PdfPCell
                            celda100.AddElement(parrafo)
                            tablademo2.AddCell(celda100) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            parrafo.Clear()
                        End If

                    Next
                    Documento.Add(tablademo2)
                    Dim tablademo3 As New PdfPTable(DataGridView1.Columns.Count) 'declara la tabla con 4 Columnas

                    tablademo3.TotalWidth = 90 'Ajusta el tamaño de cada columna
                    tablademo3.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                    tablademo3.AddCell(New Paragraph(row.Cells(1).Value.ToString)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                    For i As Integer = 2 To DataGridView1.Columns.Count - 1
                        If row.Cells(i).Value = Nothing Then
                            tablademo3.AddCell(New Paragraph("0,00"))
                        Else

                            Dim paraParrrafo As String
                            If IsNumeric(row.Cells(i).Value) Then
                                paraParrrafo = Format(row.Cells(i).Value, "###,##0.00")

                            Else
                                paraParrrafo = row.Cells(i).Value
                            End If
                            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                            parrafo.Add(paraParrrafo) 'Texto que se insertara
                            tablademo3.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                            parrafo.Clear()

                        End If
                    Next

                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea

                    parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado

                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("PLANILLA DE GASTOS Y VIATICOS") 'Texto que se insertara

                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea

                    For Each row1 As DataGridViewRow In DataGridView2.Rows
                        If row.Cells(0).Value = row1.Cells(3).Value Then
                            ' If row.Cells(2).Value = Nothing Or row.Cells(3).Value = Nothing Then
                            'Documento.Add(New Paragraph(" NO HUBO DECLARACION DE GASTOS ")) 'Salto de linea
                            'Else
                            Dim tablademo4 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                            'tablademo4.HorizontalAlignment = ALIGN_LEFT
                            tablademo4.SetWidthPercentage({100, 300, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna



                            For Each column As DataGridViewColumn In DataGridView2.Columns
                                If column.Index <= 3 Then
                                    Dim paraParrrafo As String
                                    If IsNumeric(row1.Cells(column.Index).Value) Then
                                        subGastos = subGastos + row1.Cells(column.Index).Value
                                        Dim VARIABLE As Double
                                        VARIABLE = row1.Cells(column.Index).Value
                                        paraParrrafo = Format(VARIABLE, "###,##0.00")
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
                            Documento.Add(tablademo4) 'Agrega la tabla al documento
                        End If

                        'End If
                    Next
                    Dim tablademo9 As New PdfPTable(3) 'declara la tabla con 4 Columnas
                    'tablademo9.HorizontalAlignment = ALIGN_LEFT
                    tablademo9.SetWidthPercentage({100, 300, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                    tablademo9.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                    tablademo9.AddCell(New Paragraph(" ")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                    Dim miParrafo As String
                    Dim miTotal As Double
                    miTotal = Double.Parse(subGastos)
                    miParrafo = Format(miTotal, "###,##0.00")
                    Dim miCelda As New PdfPCell
                    parrafo.Add(miParrafo)
                    miCelda.AddElement(parrafo)
                    subGastos = 0
                    miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
                    tablademo9.AddCell(miCelda)
                    Documento.Add(tablademo9)
                    parrafo.Clear()
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea

                    parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    parrafo.Add("            " & "          Encargado Rendicion " & "                             " & "Direccion Administracion " & vbCr & "             Ordenes de Cargo         " & "                                  " & "Finanzas") 'Texto que se insertara

                    'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
                    'parrafo.Add("Direccion Administracion " & vbCr & "Finanzas") 'Texto que se insertara

                    Documento.Add(parrafo) 'Agrega el parrafo al documento
                    parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.Add(New Paragraph(" ")) 'Salto de linea
                    Documento.NewPage()


                Next
            End If

        End If



        Documento.Close() 'Cierra el documento
        System.Diagnostics.Process.Start("Rendicion.pdf")

    End Sub

    Private Function dameNroHistorial(ByRef Tipo As String) As String
        Dim Nro As String
        sql = "Select Concepto from AAOrdenNro where Tipo='" & Tipo & "'  and NroIngreso=" & adjuntarAnio()
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
    'busca las fechas,descripcion,monto,nombre,destino que se cargaron en la tabla AARendiciones los cuales pertenecen al historial de la rendiciones
    'luego se le asigna el detino el la caja de texto txtDestino.
    Private Sub cargarHistorialReintegro()
        Try
            sql = "SELECT [Fecha],[Descripcion],[Monto],[Nombre],[Destino] FROM [SBDASIPT].[dbo].[AARendiciones] where [NroInreso]='" & adjuntarAnio() & "'"
            Using conn As New SqlConnection(CONNSTR)
                Using cmd4 As SqlCommand = conn.CreateCommand()
                    conn.Open()
                    cmd4.CommandText = sql
                    Dim reader As SqlDataReader = Nothing
                    reader = cmd4.ExecuteReader
                    While reader.Read()
                        Dim fila(5) As Object
                        fila(0) = reader(0)
                        fila(1) = reader(1)
                        fila(2) = reader(2)
                        fila(3) = reader(3)
                        DataGridView2.Rows.Add(fila)
                        txtDestino.Text = reader(4)
                    End While
                End Using
            End Using
        Catch ex As Exception

        End Try
        
    End Sub

    Public Sub imprimirPagoHistorial()
        Dim fechona As String
        Dim Nro As String
        Try
            sql = "Select Concepto,Fecha from AAOrdenNro where Tipo='ViaticoCargo' and NroIngreso=" & adjuntarAnio()
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
            sql = "Select Concepto,Fecha from AAOrdenNro where Tipo='ViaticoPago' and NroIngreso=" & adjuntarAnio()
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
        Catch ex As Exception
        End Try
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
        parrafo.Add("ORDEN PAGO NRO:" & dameNroHistorial("ViaticoPago") & "  ")
        parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("FECHA:" & fechona) 'Texto que se insertara

        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        'Documento.Add(New Paragraph("FECHA DE IMPRESION:" & Date.Now, FontFactory.GetFont("Arial", 8))) 'Salto de linea
        Documento.Add(New Paragraph("ORDEN CARGO ASOCIADA: " & Nro, FontFactory.GetFont("Arial", 9))) 'Salto de linea

        Dim res As String
        sql = "Select Disposicion from AAOrdenNro where Tipo='Contratos' and NroIngreso=" & adjuntarAnio()
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                If reader.Read() Then
                    If (IsDBNull(reader(0))) Then
                        res = "" 'reader(0)
                    Else
                        res = reader(0)
                    End If

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
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Dim tablademo5 As New PdfPTable(5) 'declara la tabla con 4 Columnas
        tablademo5.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tablademo5.AddCell(New Paragraph("F. EMISION        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
        tablademo5.AddCell(New Paragraph("BENEFICIARIO        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo5.AddCell(New Paragraph("CONCEPTO     ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        tablademo5.AddCell(New Paragraph("CH. Y/O AUT.        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10

        tablademo5.AddCell(New Paragraph("IMPORTE        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
        Documento.Add(tablademo5) 'Agrega la tabla al documento
        Dim tablademo4 As New PdfPTable(5) 'declara la tabla con 4 Columnas
        tablademo4.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        Dim cuenta As String
        Dim cheque As String

        'sql2 = "SELECT Distinct [chp_FVto],[pro_RazSoc], [chpctb_Cod],MAX([chp_NroCheq]),min([chp_Importe])  FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
        '      " inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
        '      " inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]inner join [SBDASIPT].dbo.[CabMovF] on [CabCompra].ccocmf_ID=[CabMovF].[cmf_ID] " &
        '     " inner join [SBDASIPT].[dbo].[MovF] on  [CabMovF].cmf_ID=[MovF].mfocmf_ID " &
        '    " where CausaEmi.cem_Desc='" & adjuntarAnio() & "' GROUP BY [chp_FVto],[pro_RazSoc],[chpctb_Cod]"
        sql2 = "SELECT Distinct [chp_FVto],[pro_RazSoc], [chpctb_Cod],[chp_NroCheq],[chp_Importe] FROM [SBDASIPT].[dbo].[ChequesP] inner join [SBDASIPT].[dbo].[RelaChqP] on [ChequesP].chp_ID= [RelaChqP].[rcpchp_ID] " &
               " inner join [SBDASIPT].[dbo].[CabCompra] on [RelaChqP].[rcpcmf_ID]=[CabCompra].[ccocmf_ID] INNER JOIN [SBDASIPT].[dbo].[CausaEmi] ON CabCompra.ccocem_Cod=CausaEmi.cem_Cod " &
               " inner join [SBDASIPT].[dbo].[Proveed] on [ChequesP].[chppro_Cod]=[Proveed].[pro_Cod]inner join [SBDASIPT].dbo.[CabMovF] on [CabCompra].ccocmf_ID=[CabMovF].[cmf_ID] " &
               " inner join [SBDASIPT].[dbo].[MovF] on  [CabMovF].cmf_ID=[MovF].mfocmf_ID " &
               " where CausaEmi.cem_Desc='" & adjuntarAnio() & "' GROUP BY [chp_FVto],[pro_RazSoc],[chpctb_Cod],[chp_NroCheq],[chp_Importe]"
        Dim subtotal As Double
        subtotal = 0
        Dim cuentaYcheque(2) As Object
        Dim codigo As String
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql2
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                'reader.Read()
                'codigo = reader(0)
                While reader.Read()
                    cuenta = adjuntarAnio()
                    cheque = reader(1)
                    For i = 0 To tablademo4.NumberOfColumns
                        If i >= 5 Then
                        Else
                            Dim paraParrrafo As String
                            If IsNumeric(reader(i)) And i <> 3 Then
                                paraParrrafo = Format(reader(i), "###,##0.00")
                                subtotal = subtotal + reader(i)
                                Dim miCelda2 As New PdfPCell
                                parrafo.Add(paraParrrafo)
                                miCelda2.AddElement(parrafo)
                                miCelda2.HorizontalAlignment = Element.ALIGN_RIGHT
                                tablademo4.AddCell(miCelda2)
                                parrafo.Clear()
                            Else
                                paraParrrafo = reader(i)
                                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                                parrafo.Add(paraParrrafo) 'Texto que se insertara
                                tablademo4.AddCell(New Paragraph(parrafo)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                parrafo.Clear()
                        End If
                        End If
                    Next
                End While
            End Using
        End Using
        Documento.Add(tablademo4) 'Agrega la tabla al documento
        sql = "Select sobrante,donacion,fechaMod,total,nombre,nroCuenta,nroCheque  from AASobrantes where NroOrden='" & adjuntarAnio() & "'"
        Dim totalDon As Double
        Dim totalAdisional As Double
        totalAdisional = 0
        Using conn As New SqlConnection(CONNSTR)
            Using cmd4 As SqlCommand = conn.CreateCommand()
                conn.Open()
                cmd4.CommandText = sql
                Dim reader As SqlDataReader = Nothing
                reader = cmd4.ExecuteReader
                While reader.Read()
                    If Not reader.Item(0).Equals(Nothing) Or Not reader.Item(1).ToString.Equals("X") Then
                        totalDon = totalDon + reader.Item(3)
                        Dim Donacion As String
                        Donacion = reader.Item(1).ToString
                        If Donacion = "SI" Then
                            If reader(0) = 0 Then
                            Else
                                Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                Dim nroCuenta As String
                                Dim nroCheque As String
                                If (IsDBNull(reader(5))) Then
                                    nroCuenta = ""
                                Else
                                    nroCuenta = reader(5)
                                End If
                                If (IsDBNull(reader(6))) Then
                                    nroCheque = ""
                                Else
                                    nroCheque = reader(6)
                                End If

                                tablademo0.AddCell(reader.Item(2)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo0.AddCell("DONACION") 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

                                tablademo0.AddCell(New Paragraph(nroCuenta)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                tablademo0.AddCell(New Paragraph(nroCheque)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                Dim importe As String
                                Dim miImporte As Double
                                If reader.Item(0) > 0 Then
                                    miImporte = reader.Item(0)
                                    totalAdisional = totalAdisional + miImporte
                                Else
                                    miImporte = reader.Item(0)
                                    totalAdisional = totalAdisional + (miImporte * -1)
                                    miImporte = reader.Item(0) * -1
                                End If
                                ' subtotal = subtotal + reader.Item(0)
                                importe = Format(miImporte, "###,##0.00")
                                parrafo.Add(importe)
                                Dim CeldaImp As New PdfPCell
                                CeldaImp.AddElement(parrafo)
                                tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                parrafo.Clear()

                                Documento.Add(tablademo0) 'Agrega la tabla al documento
                            End If
                        Else
                            If reader.Item(0) = 0 Then

                            Else
                                If reader.Item(0) > 0 Then
                                    Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                    tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                    Dim nroCuenta As String
                                    Dim nroCheque As String
                                    If (IsDBNull(reader(5))) Then
                                        nroCuenta = ""
                                    Else
                                        nroCuenta = reader(5)
                                    End If
                                    If (IsDBNull(reader(6))) Then
                                        nroCheque = ""
                                    Else
                                        nroCheque = reader(6)
                                    End If

                                    tablademo0.AddCell("DIFERENCIA A REINTEGRAR") 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(reader.Item(4).ToString) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph(nroCuenta)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph(nroCheque)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    Dim importe As String
                                    Dim miImporte As Double
                                    If reader.Item(0) > 0 Then
                                        miImporte = reader.Item(0) * 1 'modificacion reciente
                                        totalAdisional = totalAdisional - miImporte
                                        miImporte = miImporte * -1
                                        'totalDon = totalDon - reader.Item(0)
                                    Else

                                        miImporte = reader.Item(0) * 1
                                        totalAdisional = totalAdisional + miImporte

                                    End If

                                    importe = Format(miImporte, "###,##0.00")
                                    parrafo.Add(importe)
                                    Dim CeldaImp As New PdfPCell
                                    CeldaImp.AddElement(parrafo)
                                    tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()

                                    Documento.Add(tablademo0) 'Agrega la tabla al documento
                                Else
                                    Dim nroCuenta As String
                                    Dim nroCheque As String
                                    If (IsDBNull(reader(5))) Then
                                        nroCuenta = ""
                                    Else
                                        nroCuenta = reader(5)
                                    End If
                                    If (IsDBNull(reader(6))) Then
                                        nroCheque = ""
                                    Else
                                        nroCheque = reader(6)
                                    End If

                                    Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                                    tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
                                    tablademo0.AddCell("DIFERENCIA A PAGAR") 'Agrega COLUMNA1 con fuente ARIAL tamaño 5

                                    tablademo0.AddCell(reader.Item(4).ToString) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph(nroCuenta)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph(nroCheque)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    Dim importe As String
                                    Dim miImporte As Double
                                    If reader.Item(0) > 0 Then
                                        miImporte = reader.Item(0)
                                        totalAdisional = totalAdisional + miImporte
                                        miImporte = reader.Item(0) * -1
                                    Else
                                        miImporte = reader.Item(0) * -1
                                        totalAdisional = totalAdisional + miImporte

                                    End If
                                    '  subtotal = subtotal + miImporte
                                    importe = Format(miImporte, "###,##0.00")
                                    parrafo.Add(importe)
                                    Dim CeldaImp As New PdfPCell
                                    CeldaImp.AddElement(parrafo)
                                    tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()
                                    Documento.Add(tablademo0) 'Agrega la tabla al documento
                                End If

                            End If

                        End If
                    Else
                        sql3 = "select Distinct cmf_FMov,cmf_Desc,mfoctb_Cod,mfo_ImpMonElem from [SBDASIPT].dbo.CabMovF " +
                        "inner join [SBDASIPT].dbo.MovF on CabMovF.cmf_ID=MovF.mfocmf_ID " +
                        "where cmftmo_Cod='REIVIA'  and mfo_ImpMonElem>0 and cmf_Desc like '%" & txtCodigo.Text & "%'"
                        Dim tablademo0 As New PdfPTable(5) 'declara la tabla con 4 Columnas
                        tablademo0.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna

                        Using conn2 As New SqlConnection(CONNSTR)
                            Using cmd42 As SqlCommand = conn2.CreateCommand()

                                conn2.Open()

                                cmd42.CommandText = sql3

                                Dim reader2 As SqlDataReader = Nothing
                                reader2 = cmd42.ExecuteReader
                                'reader.Read()
                                'codigo = reader(0)
                                While reader2.Read()
                                    tablademo0.AddCell(reader2.Item(0)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(reader2.Item(1)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    Dim cta As String
                                    cta = reader2(2)
                                    tablademo0.AddCell(New Paragraph(cta)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    tablademo0.AddCell(New Paragraph("Deposito")) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    Dim importe As String
                                    Dim miImporte As Double
                                    If reader2.Item(3) > 0 Then
                                        miImporte = reader2.Item(3)
                                        totalAdisional = totalAdisional - miImporte
                                    Else
                                        miImporte = reader2.Item(3)
                                        totalAdisional = totalAdisional + miImporte
                                        miImporte = reader2.Item(3)
                                    End If

                                    importe = Format(miImporte, "###,##0.00")
                                    parrafo.Add(importe)
                                    Dim CeldaImp As New PdfPCell
                                    CeldaImp.AddElement(parrafo)
                                    tablademo0.AddCell(CeldaImp) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
                                    parrafo.Clear()
                                End While


                            End Using
                        End Using
                        Documento.Add(tablademo0) 'Agrega la tabla al documento
                    End If
                End While



            End Using
        End Using
   
        Dim tabla9 As New PdfPTable(5)
        'tabla9.HorizontalAlignment = ALIGN_LEFT
        tabla9.SetWidthPercentage({100, 150, 100, 100, 100}, PageSize.A4) 'Ajusta el tamaño de cada columna
        tabla9.AddCell("TOTAL")
        tabla9.AddCell(" ")
        tabla9.AddCell(" ")
        tabla9.AddCell(" ")
        Dim miParrafo As String
        Dim miTotal As Double

        subtotal = subtotal + totalAdisional
        miTotal = Double.Parse(subtotal)

        miParrafo = Format(miTotal, "###,##0.00")
        Dim miCelda As New PdfPCell
        parrafo.Add(miParrafo)
        miCelda.AddElement(parrafo)

        miCelda.HorizontalAlignment = Element.ALIGN_RIGHT
        tabla9.AddCell(miCelda)
        Documento.Add(tabla9)
        parrafo.Clear()



        'miTotal = Double.Parse(txtTotalFinal.Text)
        Documento.Add(New Paragraph("TOTAL:$" & miTotal & "(PESOS " & Numalet.ToCardinal(miTotal) & ")")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        Documento.Add(New Paragraph(" ")) 'Salto de linea
        parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
        parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
        parrafo.Add("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  " & "DIRECTOR GENERAL") 'Salto de linea

        Documento.Add(parrafo) 'Agrega el parrafo al documento
        parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente


        Documento.Close() 'Cierra el documento
        System.Diagnostics.Process.Start("OrdenPago.pdf") 'Abre el archivo DEMO.PDF

    End Sub

    ' Private Function (ByRef sql As String,ByRef cuenta As String, ByRef cheque As String )As String 
    '    Using conn As New SqlConnection(CONNSTR)
     '       Using cmd4 As SqlCommand = conn.CreateCommand()
    '           conn.Open()
    '          cmd4.CommandText = sql
    '    Dim reader As SqlDataReader = Nothing
    '               reader = cmd4.ExecuteReader
    '              If reader.Read() Then
    '                 Nro = reader(0)
    '                fechona = reader(1)
    '           End If
    '      End Using
    ' End Using
    '
    'End Function


    Public Sub imprimirOrdenCargoChequesHistorial()
        Try
            Dim fechona As String
            Dim Nro As String
            Try
                sql = "Select Concepto,Fecha from AAOrdenNro where Tipo='ViaticoCargo' and NroIngreso=" & adjuntarAnio()
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
            Catch ex As Exception

            End Try

            Dim Documento As New Document(PageSize.A4, 60, 5, 35, 5) 'Declaracion del documento
            Dim parrafo As New Paragraph ' Declaracion de un parrafo
            Dim tablademo As New PdfPTable(5) 'declara la tabla con 4 Columnas

            pdf.PdfWriter.GetInstance(Documento, New FileStream("OrdenCargo.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

            Documento.Open() 'Abre documento para su escritura

            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("PROVINCIAL DE MISIONES" & vbCr & "SISTEMA PROVINCIAL" & vbCr & "DE TELEDUCACION Y DESARROLLO   ") 'Texto que se insertara
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Alignment = Element.ALIGN_MIDDLE 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("FECHA DE IMPRESIÓN:" & fechona)
            'parrafo.Add("FECHA DE IMPRESIÓN:" & Date.Now) 'Texto que se insertara
            'Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

            Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

            imagendemo = iTextSharp.text.Image.GetInstance("descarga.jpg") 'Dirreccion a la imagen que se hace referencia
            imagendemo.SetAbsolutePosition(450, 750) 'Posicion en el eje cartesiano
            imagendemo.ScaleAbsoluteWidth(100) 'Ancho de la imagen
            imagendemo.ScaleAbsoluteHeight(100) 'Altura de la imagen
            Documento.Add(imagendemo) ' Agrega la imagen al documento


            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("ORDEN NRO:" & dameNroHistorial("ViaticoCargo") & "                             ") 'Texto que se insertara
            parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("FECHA EMISIÓN:" & fechona) 'Texto que se insertara

            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

            Documento.Add(New Paragraph(" ")) 'Salto de linea

            tablademo.SetWidthPercentage({170, 100, 80, 80, 140}, PageSize.A4) 'Ajusta el tamaño de cada columna

            tablademo.AddCell(New Paragraph("DESTINATARIO", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            tablademo.AddCell(New Paragraph("CUENTA CORRIENTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
            tablademo.AddCell(New Paragraph("NRO DE CHEQUE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
            tablademo.AddCell(New Paragraph("IMPORTE", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            tablademo.AddCell(New Paragraph("RECIBI CONFORME", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
            Documento.Add(tablademo) 'Agrega la tabla al documento

            Dim tablademo2 As New PdfPTable(5) 'declara la tabla con 4 Columnas
            tablademo2.SetWidthPercentage({170, 100, 80, 80, 140}, PageSize.A4) 'Ajusta el tamaño de cada columna

            For Each row As DataGridViewRow In DataGridView1.Rows
                For Each column As DataGridViewColumn In DataGridView1.Columns
                    If column.Index <= 4 Then
                        Dim paraParrrafo As String

                        If IsNumeric(row.Cells(column.Index).Value) And column.Index = 3 Then

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
            Dim GranTotal As Double

            GranTotal = Double.Parse(txtTotalFinal.Text)
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
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente
            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("DEPTO. TESORERIA") 'Texto que se insertara

            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

            Documento.Close() 'Cierra el documento
            System.Diagnostics.Process.Start("OrdenCargo.pdf") 'Abre el archivo DEMO.PDF
        Catch ex As Exception
            MessageBox.Show("Verifique que no tiene el mismo documento abierto", "AVISO")
        End Try


    End Sub

    Public Sub impirmirOrdenCargoPrimeroHistorial()
        Try
            Dim fechona As String
            Dim Nro As String
            Try
                sql = "Select Concepto,Fecha from AAOrdenNro where NroIngreso='" & adjuntarAnio() & "' and Tipo='ViaticoCargo' "
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
            Catch ex As Exception

            End Try

            Dim Documento As New Document(PageSize.LEGAL, 60, 5, 35, 5)
            Dim parrafo As New Paragraph ' Declaracion de un parrafo

            pdf.PdfWriter.GetInstance(Documento, New FileStream("OrdenCargo.pdf", FileMode.Create)) 'Crea el archivo "DEMO.PDF

            Documento.Open() 'Abre documento para su escritura

            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("PROVINCIAL DE MISIONES" & vbCr & "SISTEMA PROVINCIAL" & vbCr & "DE TELEDUCACION Y DESARROLLO   ") 'Texto que se insertara
            'Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Alignment = Element.ALIGN_MIDDLE 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            'parrafo.Add("FECHA DE IMPRESIÓN:" & Date.Now) 'Texto que se insertara
            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

            Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

            imagendemo = iTextSharp.text.Image.GetInstance("descarga.jpg") 'Dirreccion a la imagen que se hace referencia
            imagendemo.SetAbsolutePosition(490, 920) 'Posicion en el eje cartesiano
            imagendemo.ScaleAbsoluteWidth(100) 'Ancho de la imagen
            imagendemo.ScaleAbsoluteHeight(100) 'Altura de la imagen
            Documento.Add(imagendemo) ' Agrega la imagen al documento
            'NRO DE ORDEN DE CARGO


            parrafo.Alignment = Element.ALIGN_LEFT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("ORDEN NRO:" & dameNroHistorial("ViaticoCargo") & "                             ") 'Texto que se insertara
            parrafo.Alignment = Element.ALIGN_CENTER 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("NRO DE ENTRADA:" & txtCodigo.Text & "                             ") 'Texto que se insertara
            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_LEFT) 'Asigan fuente
            parrafo.Add("FECHA:" & fechona) 'Texto que se insertara

            Documento.Add(parrafo) 'Agrega el parrafo al documento
            parrafo.Clear() 'Limpia el parrafo para que despues pueda ser utilizado nuevamente

            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Dim tablademo As New PdfPTable(DataGridView1.Columns.Count) 'declara la tabla con 4 Columnas
            'tablademo.SetWidthPercentage({80}, PageSize.A4)
            tablademo.TotalWidth() = 100
            tablademo.HorizontalAlignment = Element.ALIGN_JUSTIFIED
            For Each miCol As DataGridViewColumn In DataGridView1.Columns
                If miCol.Index >= 0 And miCol.Index < DataGridView1.ColumnCount - 1 Then
                    tablademo.AddCell(New Paragraph(miCol.Name, FontFactory.GetFont("Arial", 10)))
                Else
                    tablademo.AddCell(New Paragraph("Total", FontFactory.GetFont("Arial", 10)))
                End If

            Next
            Documento.Add(tablademo) 'Agrega la tabla al documento

            Dim tablademo2 As New PdfPTable(DataGridView1.Columns.Count) 'declara la tabla con 4 Columnas
            'tablademo2.SetWidthPercentage({80}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tablademo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED
            For Each row As DataGridViewRow In DataGridView1.Rows
                For Each column As DataGridViewColumn In DataGridView1.Columns
                    If column.Index <= DataGridView1.Columns.Count Then
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
            Documento.Add(tablademo2)

            'Documento.Add(New Paragraph(" ")) 'Salto de linea

            Dim tablademo3 As New PdfPTable(DataGridView1.Columns.Count) 'declara la tabla con 4 Columnas
            'tablademo3.SetWidthPercentage({100, 70, 130, 100, 130, 70}, PageSize.A4) 'Ajusta el tamaño de cada columna
            tablademo3.AddCell(New Paragraph("TOTAL        ", FontFactory.GetFont("Arial", 12))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 5
            Dim paraTotal As String
            If DataGridView1.Columns.Count = 4 Then
                tablademo3.AddCell(New Paragraph(txtDia.Text)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                'Dim paraComb As String
                'Dim miComb As Double
                'miComb = Double.Parse(txtCombustible.Text)
                'paraComb = Format(miComb, "###,##0.00")
                'Dim CeldaDia As New PdfPCell
                'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                'parrafo.Add(paraComb)
                'CeldaDia.AddElement(parrafo)
                'tablademo3.AddCell(CeldaDia) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                'parrafo.Clear()
                'Dim paraPeaje As String
                'Dim miPeajes As Double
                'miPeajes = Double.Parse(txtPeaje.Text)
                'paraPeaje = Format(miPeajes, "###,##0.00")
                'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                'parrafo.Add(paraPeaje)
                'Dim celdaPeaje As New PdfPCell
                'celdaPeaje.AddElement(parrafo)
                'tablademo3.AddCell(celdaPeaje) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                'parrafo.Clear()
                'Dim miViaticos As Double
                'Dim paraViatico As String
                'miViaticos = Double.Parse(txtViaticos.Text)
                'paraViatico = Format(miViaticos, "###,##0.00")
                'parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                'parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                'parrafo.Add(paraViatico)
                'Dim celdaViatico As New PdfPCell
                'celdaViatico.AddElement(parrafo)
                'tablademo3.AddCell(celdaViatico) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                'parrafo.Clear()
                Dim miFinal0 As Double
                miFinal0 = Double.Parse(txtTotalFinal.Text)
                paraTotal = Format(miFinal0, "###,##0.00")
                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                parrafo.Add(paraTotal)
                Dim CeldaFinal As New PdfPCell
                CeldaFinal.AddElement(parrafo)
                tablademo3.AddCell(CeldaFinal) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                parrafo.Clear()
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim miFinal As Double
                miFinal = Double.Parse(txtTotalFinal.Text)
                paraTotal = Format(miFinal, "###,##0.00")
                parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                parrafo.Add(paraTotal)
                Dim CeldaFinal0 As New PdfPCell
                CeldaFinal0.AddElement(parrafo)
                tablademo3.AddCell(CeldaFinal0) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                parrafo.Clear()
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            Else
                If DataGridView1.Columns.Count > 4 Then
                    tablademo3.AddCell(New Paragraph(txtDia.Text)) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8
                    Dim paraComb As String
                    Dim miComb As Double
                    miComb = Double.Parse(txtCombustible.Text)
                    paraComb = Format(miComb, "###,##0.00")
                    Dim CeldaDia As New PdfPCell
                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                    parrafo.Add(paraComb)
                    CeldaDia.AddElement(parrafo)
                    tablademo3.AddCell(CeldaDia) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    parrafo.Clear()
                    Dim paraPeaje As String
                    Dim miPeajes As Double
                    miPeajes = Double.Parse(txtPeaje.Text)
                    paraPeaje = Format(miPeajes, "###,##0.00")
                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                    parrafo.Add(paraPeaje)
                    Dim celdaPeaje As New PdfPCell
                    celdaPeaje.AddElement(parrafo)
                    tablademo3.AddCell(celdaPeaje) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    parrafo.Clear()
                    Dim miViaticos As Double
                    Dim paraViatico As String
                    miViaticos = Double.Parse(txtViaticos.Text)
                    paraViatico = Format(miViaticos, "###,##0.00")
                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                    parrafo.Add(paraViatico)
                    Dim celdaViatico As New PdfPCell
                    celdaViatico.AddElement(parrafo)
                    tablademo3.AddCell(celdaViatico) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    parrafo.Clear()
                    Dim miFinal As Double
                    miFinal = Double.Parse(txtTotalFinal.Text)
                    paraTotal = Format(miFinal, "###,##0.00")
                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                    parrafo.Add(paraTotal)
                    Dim CeldaFinal As New PdfPCell
                    CeldaFinal.AddElement(parrafo)
                    tablademo3.AddCell(CeldaFinal) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    parrafo.Clear()
                Else
                    tablademo3.AddCell(New Paragraph(txtDia.Text, FontFactory.GetFont("Arial", 9))) 'Agrega COLUMNA1 con fuente ARIAL tamaño 8

                    Dim misOtrosGastos As Double
                    misOtrosGastos = Double.Parse(txtOtrosGastos.Text)
                    Dim praOtros As String
                    praOtros = Format(misOtrosGastos, "###,##0.00")
                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                    parrafo.Add(praOtros)
                    Dim otrosGastos As New PdfPCell
                    otrosGastos.AddElement(parrafo)
                    tablademo3.AddCell(otrosGastos) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    parrafo.Clear()
                    Dim miFinal As Double
                    miFinal = Double.Parse(txtTotalFinal.Text)
                    paraTotal = Format(miFinal, "###,##0.00")
                    parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
                    parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_RIGHT) 'Asigan fuente
                    parrafo.Add(paraTotal)
                    Dim CeldaFinal As New PdfPCell
                    CeldaFinal.AddElement(parrafo)
                    tablademo3.AddCell(CeldaFinal) 'Agrega COLUMNA1 con fuente ARIAL tamaño 10
                    parrafo.Clear()
                End If

            End If

            Documento.Add(tablademo3) 'Agrega la tabla al documento


            Documento.Add(New Paragraph("SON:$" & paraTotal & " (Pesos " & Numalet.ToCardinal(txtTotalFinal.Text) & ")")) 'Salto de linea
            Documento.Add(New Paragraph("OTORGADO EN CONCEPTO DE ANTICIPO DE FONDO: ")) 'Salto de linea
            Dim misCargos As String

            For Each column As DataGridViewColumn In DataGridView1.Columns
                If column.Index = 1 And column.Index < DataGridView1.ColumnCount - 2 Then
                    misCargos = misCargos & column.Name & ", "
                Else
                    If column.Index = (DataGridView1.ColumnCount - 2) Then
                        misCargos = misCargos & column.Name & " a " & txtDestino.Text
                    End If

                End If
            Next

            Documento.Add(New Paragraph(misCargos)) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph("V'B' DIRECTOR DE SERVICIO ADMINISTRATIVO ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            Documento.Add(New Paragraph(" ")) 'Salto de linea
            parrafo.Alignment = Element.ALIGN_RIGHT 'Alinea el parrafo para que sea centrado o justificado
            parrafo.Font = FontFactory.GetFont("Arial", 10, ALIGN_JUSTIFIED) 'Asigan fuente
            parrafo.Add("DPTO CONTAB. Y LIQ " & "                  " & "DIRECTOR SERV. ADMIN." & "                  ") 'Salto de linea
            Documento.Add(parrafo) 'Salto de linea
            parrafo.Clear()
            Documento.Close() 'Cierra el documento
            System.Diagnostics.Process.Start("OrdenCargo.pdf") 'Abre el archivo DEMO.PDF

        Catch ex As Exception
            MessageBox.Show("Verifique que no tiene el mismo documento abierto", "AVISO")
        End Try

    End Sub

#End Region

    Private Sub txtTotalFinal_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtTotalFinal.TextChanged

    End Sub

    Private Sub dataConceptosPago_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dataConceptosPago.CellContentClick

    End Sub

    Private Sub DataGridView2_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick

    End Sub

    Private Sub cmbTipoOrden_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbTipoOrden.SelectedIndexChanged

    End Sub

    Private Sub txtSaliente_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtSaliente.TextChanged

    End Sub

    Private Sub GroupBox11_Enter(sender As System.Object, e As System.EventArgs) Handles GroupBox11.Enter

    End Sub

    Private Sub DataGridView1_DoubleClick(sender As Object, e As System.EventArgs) Handles DataGridView1.DoubleClick
        Dim I As Integer = DataGridView1.CurrentCell.RowIndex
        ActualizarNro.txtNroPago.Text = txtCodigo.Text
        If ((chkAutorizado.Checked) And (cmbTipoOrden.Text.Equals("Viaticos"))) Then
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