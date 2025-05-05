Imports System.Data.SqlClient

Public Class Repositorio

#Region "Usuarios"

    ''' <summary>
    ''' Devuelve una lista de usuarios
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function obtenerTodosLosUsuarios() As List(Of Usuario)
        Dim lista As New List(Of Usuario)
        '1) Armado del Connection String
        Dim connString As String = gsParam.ConnSQL
        '2) Creacion del objeto SqlConnection
        'using utilizado de esta manera, permite liberar los recursos utilizados
        Using conn As New SqlConnection(connString)
            '3) Creacion del QueryString
            Const queryString As String = "ObtenerTodosLosUsuarios"
            '4) Creacion del objeto SqlCommand
            Using cmd As New SqlCommand(queryString, conn)
                cmd.CommandType = CommandType.StoredProcedure
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                If reader.HasRows = True Then
                    Try
                        While reader.Read
                            Dim usuario As New Usuario()
                            usuario.Nombre = reader.GetString(reader.GetOrdinal("NOMBRE"))
                            usuario.Acceso = reader.GetString(reader.GetOrdinal("PRIVILEGIO"))
                            lista.Add(usuario)
                        End While
                    Catch ex As Exception
                        Logs.nuevo("Error buscando usuarios " + ex.Message)
                        Call MessageBox.Show("Error buscando usuarios en la BD " + ex.Message)
                    Finally
                        reader.Close()
                        conn.Close()
                    End Try
                End If
            End Using
        End Using
        Return lista
    End Function

    ''' <summary>
    ''' Verifica que el usuario no exista en la BD y lo guarda
    ''' </summary>
    ''' <param name="nuevoUsuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function guardarUsuario(nuevoUsuario As Usuario) As Boolean
        Dim resultado As Boolean
        '1) Armado del Connection String
        Dim connString As String = gsParam.ConnSQL
        '2) Creacion del objeto SqlConnection
        'using utilizado de esta manera, permite liberar los recursos utilizados
        Using conn As New SqlConnection(connString)
            '3) Creacion del QueryString
            Dim queryString As String = "GuardarUsuario"
            '4) Creacion del objeto SqlCommand
            Using cmd As New SqlCommand(queryString, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Nombre", nuevoUsuario.Nombre)
                cmd.Parameters.AddWithValue("@Clave", nuevoUsuario.Contraseña)
                cmd.Parameters.AddWithValue("@Data", nuevoUsuario.Acceso)
                conn.Open()
                Try
                    cmd.ExecuteNonQuery()
                    resultado = True
                Catch ex As Exception
                    Logs.nuevo("Error creando nuevo usuario " + ex.Message)
                    resultado = False
                End Try
                conn.Close()
            End Using
        End Using
        Return resultado
    End Function

    ''' <summary>
    ''' Valida que el usuario y la contraseña se correspondan y devuelve el nivel de acceso
    ''' </summary>
    ''' <param name="usuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function validarUsuario(ByRef usuario As Usuario) As Boolean
        Dim resultado As Boolean
        '1) Armado del Connection String
        Dim connString As String = gsParam.ConnSQL
        '2) Creacion del objeto SqlConnection
        'using utilizado de esta manera, permite liberar los recursos utilizados
        Using conn As New SqlConnection(connString)
            '3) Creacion del QueryString
            Dim queryString As String = "ValidarUsuario"
            '4) Creacion del objeto SqlCommand
            Using cmd As New SqlCommand(queryString, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre)
                cmd.Parameters.AddWithValue("@Clave", usuario.Contraseña)
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                If reader.HasRows = True Then
                    reader.Read()
                    resultado = True
                    usuario.Acceso = reader.GetString(reader.GetOrdinal("PRIVILEGIO"))
                Else
                    resultado = False
                End If
                reader.Close()
                conn.Close()
            End Using
        End Using
        Return resultado
    End Function

    Shared Function editarUsuario(usuario As Usuario, usuarioAnt As String) As Boolean
        Dim resultado As Boolean
        '1) Armado del Connection String
        Dim connString As String = gsParam.ConnSQL
        '2) Creacion del objeto SqlConnection
        'using utilizado de esta manera, permite liberar los recursos utilizados
        Using conn As New SqlConnection(connString)
            '3) Creacion del QueryString
            Dim queryString As String = "EditarUsuario"
            '4) Creacion del objeto SqlCommand
            Using cmd As New SqlCommand(queryString, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@NuevoNombre", usuario.Nombre)
                cmd.Parameters.AddWithValue("@Acceso", usuario.Acceso)
                cmd.Parameters.AddWithValue("@AntiguoNombre", usuarioAnt)
                conn.Open()
                Try
                    cmd.ExecuteNonQuery()
                    resultado = True
                Catch ex As Exception
                    Logs.nuevo("Error editando el usuario " + ex.Message)
                    resultado = False
                End Try
                conn.Close()
            End Using
        End Using
        Return resultado
    End Function

    Shared Function eliminarUsuario(usuario As Usuario) As Boolean
        Dim resultado As Boolean
        '1) Armado del Connection String
        Dim connString As String = gsParam.ConnSQL
        '2) Creacion del objeto SqlConnection
        'using utilizado de esta manera, permite liberar los recursos utilizados
        Using conn As New SqlConnection(connString)
            '3) Creacion del QueryString
            Dim queryString As String = "EliminarUsuario"
            '4) Creacion del objeto SqlCommand
            Using cmd As New SqlCommand(queryString, conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre)
                conn.Open()
                Try
                    cmd.ExecuteNonQuery()
                    resultado = True
                Catch ex As Exception
                    Logs.nuevo("Error borrando el usuario " + ex.Message)
                    resultado = False
                End Try
                conn.Close()
            End Using
        End Using
        Return resultado
    End Function

#End Region

    Public Shared Function test(connString As String) As Boolean
        Try
            Using conn As New SqlConnection(connString)
                conn.Open()
                conn.Close()
            End Using
            Return True
        Catch ex As Exception
            Call MessageBox.Show(ex.Message)
            Return False
        End Try
    End Function

    Public Shared Function obtenertodaslasordenes() As List(Of Mercaderia)
        Dim lista As New List(Of Mercaderia)
        '1) Armado del Connection String
        Dim connString As String = gsParam.ConnSQL
        '2) Creacion del objeto SqlConnection
        'using utilizado de esta manera, permite liberar los recursos utilizados
        Using conn As New SqlConnection(connString)
            '3) Creacion del QueryString
            Const queryString As String = "dbo.ObtenerOrdenes "
            Logs.nuevo(queryString)
            Try


                '4) Creacion del objeto SqlCommand

                Using cmd As New SqlCommand(queryString, conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.HasRows = True Then
                        Try
                            While reader.Read
                                Dim orden As New Mercaderia()
                                orden.Mercaderia = reader.GetInt32(reader.GetOrdinal("nc_mercaderia"))
                                orden.Descripcion = reader.GetString(reader.GetOrdinal("descripcion"))
                                orden.Codigo = reader.GetString(reader.GetOrdinal("codigo"))
                                orden.ICantFajas = reader.GetInt32(reader.GetOrdinal("icantfajas"))
                                orden.Presentacion = reader.GetString(reader.GetOrdinal("presentacion")).ToString
                                orden.Etiqueta = reader.GetString(reader.GetOrdinal("ETIQUETA"))
                                orden.FechaProduccion = reader.GetString(reader.GetOrdinal("fechaProd"))
                                orden.FechaVencimiento = reader.GetString(reader.GetOrdinal("fechaVenc"))
                                orden.Orden = reader.GetString(reader.GetOrdinal("lote"))
                                orden.Descripcion2 = reader.GetString(reader.GetOrdinal("descripcion2"))
                                orden.Descripcion3 = reader.GetString(reader.GetOrdinal("descripcion3"))
                                orden.Descripcion4 = reader.GetString(reader.GetOrdinal("descripcion4"))
                                orden.Descripcion5 = reader.GetString(reader.GetOrdinal("descripcion5"))
                                If Not reader.IsDBNull(reader.GetOrdinal("Cod_Caja")) Then
                                    orden.CodCaja = reader.GetInt32(reader.GetOrdinal("Cod_Caja"))
                                Else
                                    orden.CodCaja = 0 ' O cualquier valor predeterminado que desees
                                End If
                                lista.Add(orden)
                            End While
                            Logs.nuevo("Ordenes obtenidas correctamente")
                        Catch ex As Exception
                            Logs.nuevo("Error buscando ordenes " + ex.Message)
                            Call MessageBox.Show("Error buscando ordenes en la BD " + ex.Message)
                        Finally
                            reader.Close()
                            conn.Close()
                        End Try
                    End If
                End Using
            Catch ex As Exception
                Logs.nuevo("Error buscando ordenes " + ex.Message)
            End Try
        End Using
        Return lista
    End Function

    Public Shared Function obtenertodalascajas(linea As Integer) As List(Of caja)
        Dim lista As New List(Of caja)
        '1) Armado del Connection String
        Dim connString As String = gsParam.ConnSQL
        '2) Creacion del objeto SqlConnection
        'using utilizado de esta manera, permite liberar los recursos utilizados
        Using conn As New SqlConnection(connString)
            '3) Creacion del QueryString
            Const queryString As String = "dbo.ObtenerCajas "
            Logs.nuevo(queryString)
            Try
                '4) Creacion del objeto SqlCommand
                Using cmd As New SqlCommand(queryString, conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@linea", linea)
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.HasRows = True Then
                        Try
                            While reader.Read
                                Dim caja As New caja()
                                caja.Mercaderia = reader.GetInt32(reader.GetOrdinal("nc_mercaderia"))
                                caja.Descripcion = reader.GetString(reader.GetOrdinal("descripcion"))
                                caja.Codigo = reader.GetString(reader.GetOrdinal("codigo"))
                                caja.ICantFajas = reader.GetInt32(reader.GetOrdinal("icantfajas"))
                                'caja.Presentacion = reader.GetInt32(reader.GetOrdinal("presentacion")).ToString
                                caja.Etiqueta = reader.GetString(reader.GetOrdinal("ETIQUETA"))
                                caja.HoraFecha = reader.GetDateTime(reader.GetOrdinal("FechaHora"))
                                caja.Linea = reader.GetInt32(reader.GetOrdinal("Linea"))
                                caja.Codbar = reader.GetString(reader.GetOrdinal("codbar"))
                                lista.Add(caja)
                            End While
                            Logs.nuevo("Ordenes obtenidas correctamente")
                        Catch ex As Exception
                            Logs.nuevo("Error buscando ordenes " + ex.Message)
                            Call MessageBox.Show("Error buscando ordenes en la BD " + ex.Message)
                        Finally
                            reader.Close()
                            conn.Close()
                        End Try
                    End If
                End Using

            Catch ex As Exception
                Logs.nuevo("Error buscando ordenes " + ex.Message)
            End Try
        End Using
        Return lista
    End Function
    Shared Function guardarcaja(caja As caja) As Boolean
        Dim resultado As Boolean
        '1) Armado del Connection String
        Dim connString As String = gsParam.ConnSQL
        '2) Creacion del objeto SqlConnection
        'using utilizado de esta manera, permite liberar los recursos utilizados
        Using conn As New SqlConnection(connString)
            '3) Creacion del QueryString
            Dim queryString As String = "dbo.GuardarCaja"
            '4) Creacion del objeto SqlCommand
            Try
                Using cmd As New SqlCommand(queryString, conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@mercaderia", Convert.ToInt32(caja.Mercaderia))
                    cmd.Parameters.AddWithValue("@descripcion", caja.Descripcion)
                    cmd.Parameters.AddWithValue("@codigo", caja.Codigo)
                    cmd.Parameters.AddWithValue("@codbar", caja.Codbar)
                    cmd.Parameters.AddWithValue("@icantfajas", caja.ICantFajas)
                    cmd.Parameters.AddWithValue("@presentacion", 1)
                    cmd.Parameters.AddWithValue("@etiqueta", caja.Etiqueta)
                    If caja.Procesado Then
                        cmd.Parameters.AddWithValue("@procesado", 1)
                    Else
                        cmd.Parameters.AddWithValue("@procesado", 0)
                    End If
                    cmd.Parameters.AddWithValue("@linea", caja.Linea)
                    cmd.Parameters.AddWithValue("@fechahora", Date.Today)
                    cmd.Parameters.AddWithValue("@numero", caja.Numero)
                    ' cmd.Parameters.AddWithValue("@fechahora", DateTime.Today)
                    cmd.Parameters.AddWithValue("@fechaprod", caja.FechaProduccion)
                    cmd.Parameters.AddWithValue("@fechavenc", caja.FechaVencimiento)
                    cmd.Parameters.AddWithValue("@lote", caja.Orden)
                    cmd.Parameters.AddWithValue("@descripcion2", caja.Descripcion2)
                    cmd.Parameters.AddWithValue("@descripcion3", caja.Descripcion3)

                    cmd.Parameters.AddWithValue("@descripcion4", caja.Descripcion4)
                    cmd.Parameters.AddWithValue("@descripcion5", caja.Descripcion5)
                    'cmd.Parameters.AddWithValue("@codigo1", caja.Codigo)

                    conn.Open()
                        Try
                            cmd.ExecuteNonQuery()
                            resultado = True
                        Catch ex As Exception
                            Logs.nuevo("Error guardando caja" + ex.Message)
                            resultado = False
                        End Try
                        conn.Close()

                End Using
            Catch ex As Exception
                Logs.nuevo("Error guardando caja" + ex.Message)
                resultado = False
            End Try
        End Using
        Return resultado
    End Function
    Public Shared Function obtenernumplmax() As Integer
        Dim resultado As Integer
        '1) Armado del Connection String
        Dim connString As String = gsParam.ConnSQL
        '2) Creacion del objeto SqlConnection
        'using utilizado de esta manera, permite liberar los recursos utilizados
        Using conn As New SqlConnection(connString)
            '3) Creacion del QueryString
            Dim queryString As String = "ObtenerCajaMaxima1"
            '4) Creacion del objeto SqlCommand
            Try
                Using cmd As New SqlCommand(queryString, conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    ''cmd.Parameters.AddWithValue("@date", Date.Today)


                    conn.Open()
                        Dim reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.HasRows = True Then

                        reader.Read()
                        Try
                            resultado = reader.GetInt32(0)
                        Catch ex As Exception
                            Logs.nuevo("Error buscando max pl  " + ex.Message)
                            resultado = -1
                        End Try
                    Else
                        resultado = 0

                    End If
                    reader.Close()
                        conn.Close()


                End Using
            Catch ex As Exception
                Logs.nuevo("Error buscando max pl  " + ex.Message)
                resultado = -1
            End Try
        End Using
        Return resultado
    End Function
    Shared Function Actulizarpedido() As Boolean
        Dim resultado As Boolean
        '1) Armado del Connection String
        Dim connString As String = gsParam.ConnSQL
        '2) Creacion del objeto SqlConnection
        'using utilizado de esta manera, permite liberar los recursos utilizados
        Using conn As New SqlConnection(connString)
            '3) Creacion del QueryString
            Dim queryString As String = "ActualizarMercaderia"
            '4) Creacion del objeto SqlCommand
            Try

           
            Using cmd As New SqlCommand(queryString, conn)
                cmd.CommandType = CommandType.StoredProcedure

                conn.Open()
                Try
                    cmd.ExecuteNonQuery()
                    resultado = True
                Catch ex As Exception
                    Logs.nuevo("Error actualizando a pedido" + ex.Message)
                    resultado = False
                End Try
                conn.Close()
                End Using
            Catch ex As Exception
                Logs.nuevo("Error actualizando a pedido" + ex.Message)
                resultado = False
            End Try
        End Using
        Return resultado
    End Function
End Class
