Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Windows.Forms
Imports System.Threading


Public Class Logs
    Public Shared Property objec As Object = New Object()


    ''' <summary>
    ''' Crea un log del programa en la carpeta Logs que esta en la ruta de instalacion del programa
    ''' </summary>
    ''' <param name="texto">Texto que se va a guardar</param>

    Public Shared Sub nuevo(ByVal texto As String)
        SyncLock objec
            Dim direccion As String = gsParam.UbicacionLogs + "\Logs"
            'el @ va por que si no trata de buscar el comando /logs. Con el @ toma lo que esta entre comillas como texto puro
            Dim directorioCreado As Boolean = False
            'Variable para comprobar si el directiorio /Logs fue creado
            ''''''''''''''''''''''''''''''''''''''''''''''CREACIÓN DEL DIRECTORIO /Logs'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Reviso si existe el directorio
            Try
                If Directory.Exists(direccion) Then
                    'La carpeta LOGS existe, asi que trabajo con el archivo de texto
                    directorioCreado = True
                Else
                    'La carpeta LOGS no existe asi que la creo
                    Dim di As DirectoryInfo = Directory.CreateDirectory(direccion)
                    'MessageBox.Show("Se creo la carpeta LOGS: " + di.ToString());
                    directorioCreado = True
                End If
            Catch e As Exception
                Dim err As String = ("Error creando el directorio LOGS en la dirección: " + direccion + " " + e.ToString)
                MessageBox.Show(("Error creando el Directorio: " _
                            + (direccion + (" " + e.ToString))))
                directorioCreado = False
            End Try
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''MANEJO DEL ARCHIVO /dia.txt''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Si se creo el directorio empiezo con el archivo
            If directorioCreado Then
                Dim pathDelLog As String = (direccion + ("\\Log" _
                            + (DateTime.Now.Day.ToString + ".txt")))
                Dim fechaDelLog As DateTime
                Dim sw As StreamWriter = Nothing
                'objeto que escribe en el archivo
                'Veo si existe el archivo del d�a
                If File.Exists(pathDelLog) Then
                    'El archivo existe, veo de que fecha es
                    fechaDelLog = Directory.GetLastWriteTime(pathDelLog)
                    If (fechaDelLog.Date.CompareTo(DateTime.Now.Date) < 0) Then
                        'La fecha de creaci�n es anterior a la de hoy, lo borro, luego el append crea uno nuevo
                        Try
                            File.Delete(pathDelLog)
                        Catch e As Exception
                            MessageBox.Show(("Error eliminando el log viejo " + e.Message))
                        End Try
                    Else
                        'El archivo existe pero la fecha es de hoy, asi que agrego lineas
                    End If
                End If
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    Dim log As String = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + ": " + texto.Trim()
                    'El log queda como hh:mm:ss: Texto
                    sw = File.AppendText(pathDelLog)
                    'Abro el achivo para agregar texto al final (En el caso de que no exista lo crea)
                    sw.WriteLine(log)
                    'Escribo una nueva linea
                Catch e As Exception
                    Dim mensaje As String = e.Message
                    MessageBox.Show(("Error creando log " + mensaje))
                Finally
                    Try
                        If (Not (sw) Is Nothing) Then
                            sw.Close()
                            sw.Dispose()

                            'Cierro el streaming de datos. IMPORTANTE hasta que no se cierra el straming en el archivo no se escribe NADA
                        End If
                    Catch e2 As Exception
                        MessageBox.Show(e2.Message)
                    End Try
                End Try
            Else
                'Hubo un error creando el directorio logs asi que no grabo ningun archivo
            End If
        End SyncLock

    End Sub

End Class
