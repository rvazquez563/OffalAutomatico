Public Class Usuario

#Region "Variables Miembro"
    Private _nombre As String
    Private _acceso As String
    Private _logueado As Boolean
    Private _password As String
#End Region

#Region "Constructor"
    Public Sub New()
        _nombre = ""
        _acceso = 0
        _logueado = False
    End Sub
#End Region

#Region "Propertys"

    Public Property Contraseña() As String
        Get
            Return _password
        End Get
        Set(value As String)
            _password = value
        End Set
    End Property

    Public Property Logueado() As Boolean
        Get
            Return _logueado
        End Get
        Set(value As Boolean)
            _logueado = value
        End Set
    End Property

    Public Property Acceso() As String
        Get
            Return _acceso
        End Get
        Set(value As String)
            _acceso = value
        End Set
    End Property

    Public Property Nombre() As String
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property
#End Region

#Region "Métodos"
    Public Function loguearUsuario(nombre As String, password As String) As Boolean
        'Busco el usuario en la BD
        'El usuario KETAN esta hardcodeado
        If nombre.ToUpper = "KETAN" Then
            If password = obtenerFechaAlReves() Then
                _logueado = True
                _nombre = "Ketan"
                _acceso = "Administrador"
                Return True
            Else
                Call MessageBox.Show("El usuario o la contraseña son incorrectos")
                desloguearUsuario()
                Return False
            End If
        Else
            Me.Contraseña = SHA1.encriptar(password)
            Me.Nombre = nombre
            If Repositorio.validarUsuario(Me) = True Then
                _logueado = True
                Return True
            Else
                Call MessageBox.Show("El usuario o la contraseña son incorrectos")
                desloguearUsuario()
                Return False
            End If
        End If
    End Function

    Public Function guardarUsuario() As Boolean
        If Repositorio.guardarUsuario(Me) = True Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function eliminarUsuario() As Boolean
        If Repositorio.eliminarUsuario(Me) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function editarUsuario(usuarioAnt As String) As Boolean
        If Repositorio.editarUsuario(Me, usuarioAnt) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function obtenerUsuario(nombre As String) As Usuario
        Return Nothing
    End Function


    Public Sub desloguearUsuario()
        _nombre = ""
        _acceso = 0
        _logueado = False
    End Sub

    Public Function obtenerFechaAlReves() As String
        Dim resultado As String = ""
        Dim aux As String
        aux = String.Format("{0:ddMMyyyy}", DateTime.Now)
        For i = aux.Length - 1 To 0 Step -1
            resultado &= aux.Substring(i, 1)
        Next
        Return resultado
    End Function
#End Region
End Class
