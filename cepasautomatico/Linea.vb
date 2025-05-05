Public Class Linea
    'Cada línea contiene una orden (La que le asigna el operador) y su contador propio
#Region "Variables miembro"
    Private _orden As Mercaderia
    Private _contador As Long
    Private _id As String
    Private _numero As String
    Private _tieneOrden As Boolean = False
    Private _esNOREAD As Boolean = False     'Este parámetro define si la línea responde al código NOREAD
#End Region

#Region "Constructor"
    Public Sub New(numero As String)
        _numero = numero
    End Sub

    Public Sub New()

    End Sub
#End Region

#Region "Propertys"
    Public Property Orden() As Mercaderia
        Get
            Return _orden
        End Get
        Set(ByVal value As Mercaderia)
            _orden = value
        End Set
    End Property

    Public Property EsNOREAD() As Boolean
        Get
            Return _esNOREAD
        End Get
        Set(ByVal value As Boolean)
            _esNOREAD = value
        End Set
    End Property

    Public Property Contador() As Long
        Get
            Return _contador
        End Get
        Set(ByVal value As Long)
            _contador = value
        End Set
    End Property

    Public Property ID() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Public Property Numero() As String
        Get
            Return _numero
        End Get
        Set(ByVal value As String)
            _numero = value
        End Set
    End Property

    Public Property TieneOrden() As Boolean
        Get
            Return _tieneOrden
        End Get
        Set(ByVal value As Boolean)
            _tieneOrden = value
        End Set
    End Property
#End Region

#Region "Métodos"
    ''' <summary>
    ''' Este método borra todos los datos de la línea EXCEPTO el número
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub limpiarLinea()
        _orden = Nothing
        _contador = 0
        _id = ""
        _tieneOrden = False
    End Sub
#End Region

End Class
