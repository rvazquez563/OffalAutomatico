Public Class gsParam

#Region "Variables miembro"
    Private Shared _connSQL As String                   'Guarda el string de conexión del SQL local
    Private Shared _driverimpresoraPrincipar(9) As String

    Private Shared _driverimpresoraSecundaria(9) As String

    Private Shared _ubicacionLogs As String             'Guarda la direccion de la carpeta de Logs
    Private Shared _ipPLC As String
    Private Shared _hayConfigPrevia As Boolean = False
    Private Shared _modoDebug As Boolean = False
    Private Shared _letracodbar As String
    Private Shared _cantidadDespuesBajoInsumo As Integer
    Private Shared _tandem As Integer
    Private Shared _deshablin(9) As Boolean
    Private Shared _deshabImp(9, 1) As Boolean


#End Region

#Region "Propertys"

    Public Shared Property DeshabImp(i As Integer,j As Integer) As Boolean
        Get
            Return _deshabImp(i, j)
        End Get
        Set(ByVal value As Boolean)
            _deshabImp(i, j) = value
        End Set
    End Property
    Public Shared Property Deshablin1(i As Integer) As Boolean
        Get
            Return _deshablin(i)
        End Get
        Set(ByVal value As Boolean)
            _deshablin(i) = value
        End Set
    End Property

    Public Shared Property Tandem() As Integer
        Get
            Return _tandem
        End Get
        Set(ByVal value As Integer)
            _tandem = value
        End Set
    End Property
    Public Shared Property CantidadDespuesBajoInsumo() As Integer
        Get
            Return _cantidadDespuesBajoInsumo
        End Get
        Set(ByVal value As Integer)
            _cantidadDespuesBajoInsumo = value
        End Set
    End Property

    Public Shared Property LetraCodBar() As String
        Get
            Return _letracodbar
        End Get
        Set(ByVal value As String)
            _letracodbar = value
        End Set
    End Property
    Public Shared Property ModoDebug() As Boolean
        Get
            Return _modoDebug
        End Get
        Set(ByVal value As Boolean)
            _modoDebug = value
        End Set
    End Property
    Public Shared Property DriverImpresoraSecundaria(i As Integer) As String
        Get
            Return _driverimpresoraSecundaria(i)
        End Get
        Set(ByVal value As String)
            _driverimpresoraSecundaria(i) = value
        End Set
    End Property
    Public Shared Property DriverImpresoraPrincipal(i As Integer) As String
        Get
            Return _driverimpresoraPrincipar(i)
        End Get
        Set(ByVal value As String)
            _driverimpresoraPrincipar(i) = value
        End Set
    End Property

    Public Shared Property IPPLC() As String
        Get
            Return _ipPLC
        End Get
        Set(ByVal value As String)
            _ipPLC = value
        End Set
    End Property

    Public Shared Property HayConfigPrevia() As Boolean
        Get
            Return _hayConfigPrevia
        End Get
        Set(ByVal value As Boolean)
            _hayConfigPrevia = value
        End Set
    End Property

    Public Shared Property ConnSQL() As String
        Get
            Return _connSQL
        End Get
        Set(ByVal value As String)
            _connSQL = value
        End Set
    End Property

    Public Shared Property UbicacionLogs() As String
        Get
            Return _ubicacionLogs
        End Get
        Set(ByVal value As String)
            _ubicacionLogs = value
        End Set
    End Property

    Public Shared Property StringConSQL() As String
        Get
            Return _connSQL
        End Get
        Set(ByVal value As String)
            _connSQL = value
        End Set
    End Property


#End Region

End Class
