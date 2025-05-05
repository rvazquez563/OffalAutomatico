#Region "COPYRIGHT"
' Part of Libnodave, a free communication libray for Siemens S7 200/300/400
'  
' (C) Thomas Hergenhahn (thomas.hergenhahn@web.de) 2005
'
' Libnodave is free software; you can redistribute it and/or modify
' it under the terms of the GNU Library General Public License as published by
' the Free Software Foundation; either version 2, or (at your option)
' any later version.
'
' Libnodave is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU Library General Public License
' along with Libnodave; see the file COPYING.  If not, write to
' the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.  
'
#End Region
Imports System.Threading
Imports libnodave

Public Class Libnodave_WinAC
#Region "Variables miembro"
    'Dim fds As libnodave.daveOSserialType
    'Dim di As libnodave.daveInterface
    'Dim dc As libnodave.daveConnection
    Dim daveSerie As libnodave.daveOSserialType
    Dim daveInter As libnodave.daveInterface
    Dim daveConex As libnodave.daveConnection
    Dim estadoAnterior(27, 9) As Integer
    Dim estadoPLC As Integer = 0
    Dim errorenlectura As Integer = 0

    Public BufferLectura(9)() As Byte

    Public BufferEscritura(1000) As Byte
    Public res As Integer

    Private Conectado As Boolean = False
    Private Mensaje As String
    Public Shared Property objec As Object = New Object()
#End Region

#Region "Eventos"
    Public Event InicioCiclo()
    Public Event CambioElPaso(val As Integer)
    Public Event CambioCodigoDeError(val As Integer)
    Public Event FinDeCicloOK()
    Public Event FinDeCicloFallido()
    Public Event ResultadoAplicacion1(val As Integer)
    Public Event ResultadoLectura1(val As Integer)
    Public Event ResultadoAplicacion2(val As Integer)
    Public Event ResultadoLectura2(val As Integer)
#End Region

#Region "Constructores"
    Public Sub New()
        'Inicializo el vector de estado anterior
        For Each i As Integer In estadoAnterior
            i = 0
        Next

        'For k As Int16 = 0 To 100
        '    For l As Int16 = 0 To 9
        '        BufferLectura(l)(k) = 0
        '    Next
        'Next
    End Sub
#End Region

#Region "Metodos"
    Public Function Conectar(Optional ByVal Puerto As Integer = 102,
                             Optional ByVal IP As String = "192.168.001.101",
                             Optional ByVal Rack As Integer = 0,
                             Optional ByVal Slot As Integer = 2) As Boolean
        If Conectado Then
            Mensaje = "Conexión abortada, ya existe una conexión."
            Conectar = False
            Exit Function
        End If

        If gsParam.ModoDebug Then
            Return True
        End If

        Dim hayDLL As Boolean = True
        Dim Respuesta As Integer

        Mensaje = "Abriendo una conexión serie..."
        Try
            daveSerie.rfd = libnodave.openSocket(Puerto, IP)
            daveSerie.wfd = daveSerie.rfd
        Catch ex As Exception
            MessageBox.Show("No se encontro la DLL de Libnodave " + ex.Message)
            hayDLL = False
            Mensaje = "No se encontro la DLL"
        End Try

        If hayDLL = False Then
            Conectar = False
            Exit Function
        End If

        If daveSerie.rfd > 0 Then
            Mensaje = "Conexión OK, creando interface..."

            daveInter = New libnodave.daveInterface(daveSerie, "",
                                                    0,
                                                    libnodave.daveProtoISOTCP,
                                                    libnodave.daveSpeed187k)

            daveInter.setTimeout(1000000)  'Make this longer if you have a very long response time


            Mensaje = "Inicialización del adaptador OK, creando conexión..."
            daveConex = New libnodave.daveConnection(daveInter, 0, Rack, Slot)
            Respuesta = daveConex.connectPLC()

            If Respuesta = 0 Then
                Mensaje = "Conexión correcta, lista para operar."
                Conectado = True
            Else
                libnodave.closeSocket(daveSerie.rfd)
                Mensaje = "Error al abrir la conexión [" &
                          libnodave.daveStrerror(Respuesta) & "]"
                Conectado = False
            End If

        Else
            Mensaje = "Error al abrir la conexión"
            Conectado = False
        End If

        Conectar = Conectado

    End Function

    Public Function Desconectar() As Boolean
        If Conectado Then
            daveConex.disconnectPLC()
            libnodave.closeSocket(daveSerie.rfd)
            Conectado = False
            Mensaje = "Conexión correctamente terminada."
            Desconectar = True
        Else
            Mensaje = "No existe conexión activa."
            Desconectar = False
        End If
    End Function

    Public Function LeerBytesDB(linea As Integer) As Boolean 'ByVal NumDB As Integer,As Boolean
        'ByVal Dir As Integer,
        'ByVal NumBytes As Integer, linea As Integer)









        Dim Respuesta As Integer
        SyncLock objec
            'Respuesta = daveConex.readBytes(libnodave.daveDB, NumDB, Dir, NumBytes, BufferLectura[, linea])
            If linea = 0 Then
                Dim BufferLectura0(100) As Byte
                Thread.Sleep(300)
                Respuesta = daveConex.readBytes(libnodave.daveDB, 501, 0, 19, BufferLectura0)
                BufferLectura(linea) = BufferLectura0 '(i)
                'Next
                'Dim db1 As String = ""
                'For Each a As Byte In BufferLectura0
                '    db1 += a.ToString()
                'Next
                'Logs.nuevo(linea.ToString + " bf: " + db1)
                BufferLectura0 = Nothing
            ElseIf linea = 1 Then
                Dim BufferLectura1(100) As Byte
                Thread.Sleep(300)
                Respuesta = daveConex.readBytes(libnodave.daveDB, 502, 0, 19, BufferLectura1)
                BufferLectura(linea) = BufferLectura1 '(i)
                'Next
                'Dim db1 As String = ""
                'For Each a As Byte In BufferLectura1
                '    db1 += a.ToString()
                'Next
                'Logs.nuevo(linea.ToString + " bf: " + db1)
                BufferLectura1 = Nothing
            ElseIf linea = 2 Then
                Dim BufferLectura2(100) As Byte
                Thread.Sleep(300)
                Respuesta = daveConex.readBytes(libnodave.daveDB, 503, 0, 19, BufferLectura2)
                BufferLectura(linea) = BufferLectura2 '(i)
                'Next
                'Dim db1 As String = ""
                'For Each a As Byte In BufferLectura2
                '    db1 += a.ToString()
                'Next
                'Logs.nuevo(linea.ToString + " bf: " + db1)
                BufferLectura2 = Nothing
            ElseIf linea = 4 Then
                Dim BufferLectura4(100) As Byte
                Thread.Sleep(300)
                Respuesta = daveConex.readBytes(libnodave.daveDB, 504, 0, 19, BufferLectura4)
                BufferLectura(linea) = BufferLectura4 '(i)
                'Next
                'Dim db1 As String = ""
                'For Each a As Byte In BufferLectura4
                '    db1 += a.ToString()
                'Next
                'Logs.nuevo(linea.ToString + " bf: " + db1)
                BufferLectura4 = Nothing
            ElseIf linea = 5 Then
                Dim BufferLectura5(100) As Byte
                Thread.Sleep(300)
                Respuesta = daveConex.readBytes(libnodave.daveDB, 505, 0, 19, BufferLectura5)
                BufferLectura(linea) = BufferLectura5
                BufferLectura5 = Nothing
            ElseIf linea = 6 Then
                Dim BufferLectura6(100) As Byte
                Respuesta = daveConex.readBytes(libnodave.daveDB, 506, 0, 19, BufferLectura6)
                BufferLectura(linea) = BufferLectura6
                BufferLectura6 = Nothing
            ElseIf linea = 7 Then
                Dim BufferLectura7(100) As Byte
                Respuesta = daveConex.readBytes(libnodave.daveDB, 507, 0, 19, BufferLectura7)
                BufferLectura(linea) = BufferLectura7
                BufferLectura7 = Nothing
            ElseIf linea = 8 Then
                Dim BufferLectura8(100) As Byte
                Respuesta = daveConex.readBytes(libnodave.daveDB, 508, 0, 19, BufferLectura8)
                BufferLectura(linea) = BufferLectura8
                BufferLectura8 = Nothing
            ElseIf linea = 9 Then
                Dim BufferLectura9(100) As Byte
                Respuesta = daveConex.readBytes(libnodave.daveDB, 509, 0, 19, BufferLectura9)
                BufferLectura(linea) = BufferLectura9
                BufferLectura9 = Nothing
            End If
        End SyncLock


        'Dim db As String = ""
        'For Each a As Byte In BufferLectura(Linea)
        '    db += a.ToString()
        'Next
        'Logs.nuevo(Linea.ToString + " : " + db)

        If Respuesta = 0 Then
            Mensaje = "Linea " & (linea + 1).ToString & ": Leídos 19" & " bytes a partir de la dirección 0" &
                       " en el DB de la linea" & (linea + 1).ToString
            LeerBytesDB = True
            ' Logs.nuevo(Mensaje)
            'Dim a As Integer = (256 * 256 * 256 * BufferLectura(32)) + (256 * 256 * BufferLectura(33)) + (256 * BufferLectura(34)) + BufferLectura(35)
            'MsgBox(a.ToString)
            If errorenlectura <> 0 Then
                errorenlectura += 1
                Logs.nuevo(Mensaje)
                If errorenlectura > 7 Then
                    errorenlectura = 0
                End If
            End If

        Else
            Mensaje = "Linea " & (linea + 1).ToString & ": Error al leer 19" & " bytes a partir de la dirección 0" &
                       " en el DB de la linea " & (linea + 1).ToString & "con numero de respuesta " & Respuesta.ToString
            LeerBytesDB = False
            Logs.nuevo(Mensaje)
            errorenlectura = 1


        End If

    End Function

    Public Function LeerBytesOutput(ByVal NumDB As Integer,
                                ByVal Dir As Integer,
                                ByVal NumBytes As Integer) As Boolean

        Dim Respuesta As Integer

        ' Respuesta = daveConex.readBytes(libnodave.daveDB, NumDB, Dir, NumBytes, BufferLectura)

        If Respuesta = 0 Then
            Mensaje = "Leídos " & NumBytes & " bytes a partir de la dirección " &
                      Dir & " en el DB " & NumDB
            LeerBytesOutput = True
        Else
            Mensaje = "Error al leer " & NumBytes & " bytes a partir de la dirección " &
                      Dir & " en el DB " & NumDB
            LeerBytesOutput = False
        End If

    End Function

    Public Function EscribirBytesDB(ByVal NumDB As Integer,
                                    ByVal Dir As Integer,
                                    ByVal NumBytes As Integer) As Boolean
        Dim Respuesta As Integer
        If gsParam.ModoDebug Then
            Return True
        End If

        Respuesta = daveConex.writeBytes(libnodave.daveDB, NumDB, Dir, NumBytes, BufferEscritura)

        If Respuesta = 0 Then
            Mensaje = "Escritos " & NumBytes & " bytes a partir de la dirección " &
                      Dir & " en el DB " & NumDB
            EscribirBytesDB = True
            Logs.nuevo(Mensaje)
            Return True

        Else
            Mensaje = "Error al escribir " & NumBytes & " bytes a partir de la dirección " &
                      Dir & " en el DB " & NumDB
            EscribirBytesDB = False
            Logs.nuevo(Mensaje)
            Return False
        End If

    End Function

    Public Sub procesarEstadoPLC(linea As Integer)

        'En esta funcion uso el buffer de lectura para saber el estado del sistema (Variables de un byte)
        'haycaja0.0
        'codigocaja dint 0.1-0.2-0.3-0.4
        'zebraonline0.5
        'bajoinsumo0.6
        'zebraerror0.7
        'enprduccion0.8
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'If estadoAnterior(0, linea) <> BufferLectura(0) Then   'Hay Caja
        ' estadoAnterior(0, linea) = BufferLectura(0)

        If BufferLectura(linea)(0) = 1 Then
            Try
                frmMain.HAY_CAJA(linea) = True
                frmMain.codigo_caja(linea) = (256 * BufferLectura(linea)(4)) + BufferLectura(linea)(5)
                BufferEscritura(0) = 0

                'Reset(linea)

                'If linea = 0 Then
                'If EscribirBytesDB(501, 0, 1) Then
                'estadoAnterior(0, linea) = 0
                'End If
                'ElseIf linea = 1 Then
                ' If EscribirBytesDB(502, 0, 1) Then
                'estadoAnterior(0, linea) = 0
                ' End If
                ' Else
                ' If EscribirBytesDB(503, 0, 1) Then
                'estadoAnterior(0, linea) = 0
                'End If
                'End If

            Catch ex As Exception
                MsgBox(ex.Message.ToString)
            End Try
        End If
        ' End If



    End Sub

#End Region

#Region "Propertys"

    Public ReadOnly Property Msj() As String
        Get
            Return Mensaje
        End Get
    End Property

    Public ReadOnly Property Conn() As Boolean
        Get
            Return Conectado
        End Get
    End Property

#End Region

    Sub Etiquetar()
        BufferEscritura(0) = 1
        EscribirBytesDB(1, 11, 1)
    End Sub

    Sub Evacuar(i As Integer)
        BufferEscritura(0) = i
        'EscribirBytesDB(1, 12, 1)
    End Sub

    Sub Reset(linea As Integer) 'NUMDB
        BufferEscritura(0) = 0
        If linea = 0 Then
            EscribirBytesDB(501, 0, 1)
        ElseIf linea = 1 Then
            EscribirBytesDB(502, 0, 1)
        ElseIf linea = 2 Then
            EscribirBytesDB(503, 0, 1)
        ElseIf linea = 4 Then
            EscribirBytesDB(504, 0, 1)
        ElseIf linea = 5 Then
            EscribirBytesDB(505, 0, 1)
        ElseIf linea = 6 Then
            EscribirBytesDB(506, 0, 1)
        ElseIf linea = 7 Then
            EscribirBytesDB(507, 0, 1)
        ElseIf linea = 8 Then
            EscribirBytesDB(508, 0, 1)
        ElseIf linea = 9 Then
            EscribirBytesDB(509, 0, 1)
        End If
    End Sub

    Sub resetFinDeCiclo()
        BufferEscritura(0) = 0
        BufferEscritura(1) = 0
        EscribirBytesDB(1, 14, 2)

    End Sub

    Sub resetFinDeCicloERR()
        BufferEscritura(0) = 0
        BufferEscritura(1) = 0
        EscribirBytesDB(1, 15, 2)

    End Sub







End Class
