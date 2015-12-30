
Public Class MenuPrincipal

    Private Sub InicioToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles InicioToolStripMenuItem.Click

    End Sub

    Private Sub ToolStripTextBox1_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub TablaCondicionToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TablaCondicionToolStripMenuItem.Click
        Dim unaCondicionIva As Form = New condicionIva()
        unaCondicionIva.Show()
    End Sub

    Private Sub TablaGananciaToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TablaGananciaToolStripMenuItem.Click
        Dim unaSituGan As Form = New SituacionGanancia
        unaSituGan.Show()
    End Sub

    Private Sub TablaIngresoBrutoToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TablaIngresoBrutoToolStripMenuItem.Click
        Dim unaSitIB As Form = New SituacionIngresoBruto
        unaSitIB.Show()
    End Sub

    Private Sub TablaModoDePagoToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles TablaModoDePagoToolStripMenuItem1.Click
        Dim unModoPago As Form = New ModoPago
        unModoPago.Show()
    End Sub

    Private Sub TablaBeneficiarioToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub GenerarOPGToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub TipoDocumentoToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TipoDocumentoToolStripMenuItem.Click
        Dim unTipoDocum As Form = New TipoDoc
        unTipoDocum.Show()
    End Sub

    Private Sub CargarToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        Dim unOPG As Form = New OPG
        unOPG.Show()
    End Sub

    Private Sub EliminarToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        Dim eliminar As Form = New EliminarOPG
        eliminar.Show()
    End Sub

    Private Sub SalirToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SalirToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub CargarToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles CargarToolStripMenuItem1.Click
        OPG.laberTIpo.Text = LabelTipo.Text
        OPG.labelUsuario.Text = labelUsuario.Text
        OPG.Show()
    End Sub

    Private Sub BeneficiarioToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles BeneficiarioToolStripMenuItem.Click
        cargaPersonalizadaBeneficiario.labelUsuario.Text = labelUsuario.Text
        cargaPersonalizadaBeneficiario.laberTIpo.Text = LabelTipo.Text
        cargaPersonalizadaBeneficiario.Show()

    End Sub

    Private Sub ActualizarPagoToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ActualizarPagoToolStripMenuItem.Click

    End Sub

    Private Sub ConsultarToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ConsultarToolStripMenuItem.Click
        ConsultarOPG.labelUsuario.Text = labelUsuario.Text
        ConsultarOPG.labelTipo.Text = LabelTipo.Text
        ConsultarOPG.Show()

    End Sub

    Private Sub ComprasToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ComprasToolStripMenuItem.Click
        Dim unActualizarPago As Form = New ActualzarPago
        unActualizarPago.Show()
    End Sub

    Private Sub ViaticosToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ViaticosToolStripMenuItem.Click
        actualizarViatico.Show()
    End Sub

    Private Sub FondosFijosToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FondosFijosToolStripMenuItem.Click
        ActualizarFondosFijos.Show()
    End Sub

    Private Sub ContratosToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ContratosToolStripMenuItem.Click
        ActualizarContrato.Show()
    End Sub

    Private Sub AlquilerToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AlquilerToolStripMenuItem.Click
        ActualizarAlquiler.Show()
    End Sub

    Private Sub CargarToolStripMenuItem_Click_1(sender As System.Object, e As System.EventArgs) Handles CargarToolStripMenuItem.Click
        cambiarContraseña.txtUsuario.Text = labelUsuario.Text
        cambiarContraseña.Show()
    End Sub

    Private Sub ElminarToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        EliminarCuenta.Show()
    End Sub

    Private Sub MenuPrincipal_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Loginvb.Close()
    End Sub

    Private Sub OPGToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles OPGToolStripMenuItem.Click

    End Sub

    Private Sub EliminiarToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        ExportarOPGpersonalisado.Show()
    End Sub

    Private Sub PersonalizadaToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub CuentasToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CuentasToolStripMenuItem.Click

    End Sub

    Private Sub PorFechaToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        Beneficiario.labelUsuario.Text = labelUsuario.Text
        Beneficiario.laberTIpo.Text = LabelTipo.Text
        Beneficiario.Show()

    End Sub

    Private Sub PersonalizadoToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        cargaPersonalizadaBeneficiario.labelUsuario.Text = labelUsuario.Text
        cargaPersonalizadaBeneficiario.laberTIpo.Text = LabelTipo.Text
        cargaPersonalizadaBeneficiario.Show()

    End Sub

    Private Sub CargarToolStripMenuItem2_Click(sender As System.Object, e As System.EventArgs) Handles CargarToolStripMenuItem2.Click
        CargarCuentasvb.Show()

    End Sub

    Private Sub EliminarToolStripMenuItem_Click_1(sender As System.Object, e As System.EventArgs) Handles EliminarToolStripMenuItem.Click
        EliminarCuenta.Show()
    End Sub

    Private Sub CBUToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CBUToolStripMenuItem.Click
        ModificarCBU.labelUsuario.Text = Me.labelUsuario.Text
        ModificarCBU.laberTIpo.Text = Me.LabelTipo.Text
        ModificarCBU.Show()
    End Sub
End Class
