Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.Text
Imports System.IO

Public Class Cliente

#Region "VARIABLES"
    Dim cnt As Boolean
    Private Stm As Stream 'Utilizado para enviar datos al Servidor y recibir datos del mismo
    Private m_IPDelHost As String 'Direccion del objeto de la clase Servidor
    Private m_PuertoDelHost As String 'Puerto donde escucha el objeto de la clase Servidor
    Private tcpClnt As TcpClient
    Private tcpThd As Thread 'Se encarga de escuchar mensajes enviados por el Servidor
    Private conectado As Boolean = False
#End Region

#Region "EVENTOS"
    Public Event ConexionTerminada()
    Public Event DatosRecibidos(ByVal datos As String)
#End Region

#Region "PROPIEDADES"
    Public Property IPDelHost() As String
        Get
            IPDelHost = m_IPDelHost
        End Get

        Set(ByVal Value As String)
            m_IPDelHost = Value
        End Set
    End Property

    Public Property PuertoDelHost() As String
        Get
            PuertoDelHost = m_PuertoDelHost
        End Get
        Set(ByVal Value As String)
            m_PuertoDelHost = Value
        End Set
    End Property
#End Region

    'Public Sub New(cnt1)
    '   cnt = cnt1
    'End Sub
#Region "METODOS"
    Public Sub Conectar()
        If (gsParam.ModoDebug And cnt) Then
            Exit Sub
        End If


        If conectado = False Then
            tcpClnt = New TcpClient()
            'Me conecto al objeto de la clase Servidor,
            '  determinado por las propiedades IPDelHost y PuertoDelHost
            tcpClnt.Connect(IPDelHost, PuertoDelHost)
            Stm = tcpClnt.GetStream()

            'Creo e inicio un thread para que escuche los mensajes enviados por el Servidor
            tcpThd = New Thread(AddressOf LeerSocket)
            tcpThd.Start()
            conectado = True

        End If
    End Sub

    Public Sub EnviarDatos(ByVal Datos As String)
        Dim BufferDeEscritura() As Byte

        BufferDeEscritura = Encoding.ASCII.GetBytes(Datos)

        If Not (Stm Is Nothing) Then
            'Envio los datos al Servidor
            Try
                Stm.Write(BufferDeEscritura, 0, BufferDeEscritura.Length)
            Catch ex As Exception
                Call MessageBox.Show("Error enviando datos: " + ex.Message)
                Throw New Exception(ex.Message)
            End Try
        Else
            Throw New Exception("La impresora no esta conectada")
        End If
    End Sub

    Public Sub Desconectar()
        If conectado = True Then
            tcpThd.Abort()
            tcpClnt.Close()
        End If
    End Sub

    Public Function isConnected() As Boolean
        Return tcpClnt.Connected
    End Function
#End Region

#Region "FUNCIONES PRIVADAS"
    Private Sub LeerSocket()
        Dim BufferDeLectura() As Byte

        While True
            Try
                BufferDeLectura = New Byte(100) {}
                'Me quedo esperando a que llegue algun mensaje
                Stm.Read(BufferDeLectura, 0, BufferDeLectura.Length)
                'Genero el evento DatosRecibidos, ya que se han recibido datos desde el Servidor
                RaiseEvent DatosRecibidos(Encoding.ASCII.GetString(BufferDeLectura))

            Catch e As Exception
                Logs.nuevo(e.Message)
                ''frmMain.escribirMensaje(e.Message)
                Exit While
            End Try
        End While
    End Sub
#End Region

End Class