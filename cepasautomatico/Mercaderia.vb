Public Class Mercaderia
#Region "Variables miembro"
    Private _mercaderia As Integer
    Private _descripcion As String
    Private _codigo As String
    Private _icantfajas As Integer
    Private _presentacion As String
    Private _etiqueta As String
    Private _fechaProduccion As String
    Private _fechaVencimiento As String
    Private _orden As String
    Private _descripcion2 As String
    Private _descripcion3 As String
    Private _descripcion4 As String
    Private _descripcion5 As String
    Private _codCaja As Integer






#End Region

#Region "Propertys"
    Public Property CodCaja() As Integer
        Get
            Return _codCaja
        End Get
        Set(ByVal value As Integer)
            _codCaja = value
        End Set
    End Property
    Public Property Descripcion5() As String
        Get
            Return _descripcion5
        End Get
        Set(ByVal value As String)
            _descripcion5 = value
        End Set
    End Property

    Public Property Descripcion4() As String
        Get
            Return _descripcion4
        End Get
        Set(ByVal value As String)
            _descripcion4 = value
        End Set
    End Property

    Public Property Descripcion3() As String
        Get
            Return _descripcion3
        End Get
        Set(ByVal value As String)
            _descripcion3 = value
        End Set
    End Property

    Public Property Descripcion2() As String
        Get
            Return _descripcion2
        End Get
        Set(ByVal value As String)
            _descripcion2 = value
        End Set
    End Property
    Public Property Orden() As String
        Get
            Return _orden
        End Get
        Set(ByVal value As String)
            _orden = value
        End Set
    End Property

    Public Property FechaVencimiento() As String
        Get
            Return _fechaVencimiento
        End Get
        Set(ByVal value As String)
            _fechaVencimiento = value
        End Set
    End Property
    Public Property FechaProduccion() As String
        Get
            Return _fechaProduccion
        End Get
        Set(ByVal value As String)
            _fechaProduccion = value
        End Set
    End Property
    Public Property Etiqueta() As String
        Get
            Return _etiqueta
        End Get
        Set(ByVal value As String)
            _etiqueta = value
        End Set
    End Property


    Public Property Presentacion() As String
        Get
            Return _presentacion
        End Get
        Set(ByVal value As String)
            _presentacion = value
        End Set
    End Property

    Public Property ICantFajas() As Integer
        Get
            Return _icantfajas
        End Get
        Set(ByVal value As Integer)
            _icantfajas = value
        End Set
    End Property

    Public Property Codigo() As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Public Property Descripcion() As String
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            _descripcion = value
        End Set
    End Property

    Public Property Mercaderia() As Integer
        Get
            Return _mercaderia
        End Get
        Set(ByVal value As Integer)
            _mercaderia = value
        End Set
    End Property
#End Region
End Class
