Imports System.IO
Imports System.Data.Odbc

Public Module Conexiones
    Public cnn As OdbcConnection
    Public tipoUsuario As String
    Public nombreUsuario As String
    Dim rutaFicheroINI As String

    Public FS As FileStream
    Public fLee As StreamReader

    Public servidor As String
    Public user As String
    Public base As String
    Public pass As String
    Public miOdbc As String
    Public llamaCliente As String
    Public llamaMecanico As String

    Public cnm As OdbcConnection
    Public CONNSTR As String = cargarConexion()
    Public CONNSTRM As String = cargarConexionManager()

    Public Sub main()
        cnn = New OdbcConnection(cargarConexion2)
        ' cnm.ConnectionString = "Driver={SQL Server Native Client 11.0};Server=" & servidor & ";Database=manager;Uid=" & user & ";Pwd=" & pass & ";"
        Try
            cnn.Open()
            'cnm.Open()
            MenuPrincipal.Show()

        Catch ex As Exception
            MessageBox.Show("Error de conexion en la base de datos")
            'Inicio.Close()
        End Try
    End Sub
    Private Function cargarConexion2() As String
        Dim cadena As String
        rutaFicheroINI = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "conexion.dat")
        FS = New FileStream(rutaFicheroINI, FileMode.Open, FileAccess.Read, FileShare.Read)
        fLee = New StreamReader(FS)
        servidor = fLee.ReadLine
        base = fLee.ReadLine
        user = fLee.ReadLine
        pass = fLee.ReadLine
        miOdbc = fLee.ReadLine
        cadena = "Dsn=" & miOdbc & ";uid=" & user & ";pwd=" & pass

        fLee.Close()
        FS.Close()



        Return cadena
    End Function
    Private Function cargarConexion() As String
        Dim cadena As String
        rutaFicheroINI = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "conexion.dat")
        FS = New FileStream(rutaFicheroINI, FileMode.Open, FileAccess.Read, FileShare.Read)
        fLee = New StreamReader(FS)
        servidor = fLee.ReadLine
        base = fLee.ReadLine
        user = fLee.ReadLine
        pass = fLee.ReadLine
        miOdbc = fLee.ReadLine

        cadena = "Data Source=" & servidor & ";Initial Catalog=" & base & ";Persist Security Info=True;User ID=" & user & ";Password=" & pass

        'cadena = "Data Source=" & miOdbc & ";Database=" & base & ";" _
        '& "PWD=" & pass & ";UID=" & user & ";"

        'cadena = "Persist Security Info=False;Integrated Security=true; Initial Catalog=" & base & ";Server=" & miOdbc



        fLee.Close()
        FS.Close()



        Return cadena
    End Function
    Private Function cargarConexionManager() As String
        Dim cadena As String
        rutaFicheroINI = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "conexion.dat")
        FS = New FileStream(rutaFicheroINI, FileMode.Open, FileAccess.Read, FileShare.Read)
        fLee = New StreamReader(FS)
        servidor = fLee.ReadLine
        base = fLee.ReadLine
        user = fLee.ReadLine
        pass = fLee.ReadLine

        cadena = "Data Source=" & servidor & ";Initial Catalog=manager;Persist Security Info=True;User ID=" & user & ";Password=" & pass

        fLee.Close()
        FS.Close()



        Return cadena
    End Function
End Module