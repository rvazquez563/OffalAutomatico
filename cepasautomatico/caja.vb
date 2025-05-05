Public Class caja : Inherits Mercaderia


#Region "Varialbes miembro"
    Private _mercaderia As Integer
    Private _descripcion As String
    Private _codigo As String
    Private _icantfajas As Integer
    Private _presentacion As String
    Private _etiqueta As String 'el codebar
    Private _procesado As Boolean
    Private _linea As Integer
    Private _horayfecha As Date
    Private _numero As Integer
    Private _codbar As String
    Private _fechaproduccion As String
    Private _fechavencimiento As String
    Private _orden As String
    Private _descripcion2 As String
    Private _descripcion3 As String
    Private _descripcion4 As String
    Private _descripcion5 As String




#End Region

#Region "Constructor"
    Sub New()

    End Sub

    Public Sub New(orden As Mercaderia, linea As Integer, Optional fechaprod As Date = Nothing)
        Try
            Dim i As Integer
            For i = 0 To 10
                _numero = Repositorio.obtenernumplmax
                If _numero >= 0 Then
                    Exit For

                End If
            Next


            '_numero = Repositorio.obtenernumplmax + 1
            _mercaderia = orden.Mercaderia
            _descripcion = orden.Descripcion
            '_codigo = orden.Codigo
            _icantfajas = orden.ICantFajas
            _presentacion = orden.Presentacion
            _etiqueta = orden.Etiqueta
            _procesado = False
            _horayfecha = Date.Now
            _linea = linea + 1

            '_codbar()
            For Each a As Char In orden.Codigo
                If Asc(a) <= 57 Then
                    _codigo += a
                End If
            Next

            If _icantfajas <= 1 Or _icantfajas = 4 Then
                _codbar = "X" + Date.Now.Day.ToString.PadLeft(2, "0") + Date.Now.Month.ToString.PadLeft(2, "0") + gsParam.LetraCodBar + Date.Now.Year.ToString.Substring(2, 2) + _codigo.PadLeft(5, "0") + _numero.ToString.PadLeft(5, "0")
            Else
                _codbar = "F" + Date.Now.Day.ToString.PadLeft(2, "0") + Date.Now.Month.ToString.PadLeft(2, "0") + gsParam.LetraCodBar + Date.Now.Year.ToString.Substring(2, 2) + _codigo.PadLeft(5, "0") + _numero.ToString.PadLeft(5, "0")
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'modificacion del constructor
            If fechaprod = Nothing Then
                _fechaproduccion = orden.FechaProduccion
                _fechavencimiento = ((Convert.ToDateTime(orden.FechaProduccion)).AddMonths(Convert.ToInt32(orden.FechaVencimiento))).Day.ToString.PadLeft(2, "0") + "-" + ((Convert.ToDateTime(orden.FechaProduccion)).AddMonths(Convert.ToInt32(orden.FechaVencimiento))).Month.ToString.PadLeft(2, "0") + "-" + ((Convert.ToDateTime(orden.FechaProduccion)).AddMonths(Convert.ToInt32(orden.FechaVencimiento))).Year.ToString.PadLeft(4, "0")
                _orden = (Convert.ToDateTime(orden.FechaProduccion)).Year.ToString.PadLeft(4, "0") + (Convert.ToDateTime(orden.FechaProduccion)).Month.ToString.PadLeft(2, "0") + (Convert.ToDateTime(orden.FechaProduccion)).Day.ToString.PadLeft(2, "0")
            Else
                _fechaproduccion = fechaprod.Day.ToString.PadLeft(2, "0") + "-" + fechaprod.Month.ToString.PadLeft(2, "0") + "-" + fechaprod.Year.ToString.PadLeft(4, "0")
                _fechavencimiento = (fechaprod.AddMonths(Convert.ToInt32(orden.FechaVencimiento))).Day.ToString.PadLeft(2, "0") + "-" + (fechaprod.AddMonths(Convert.ToInt32(orden.FechaVencimiento))).Month.ToString.PadLeft(2, "0") + "-" + (fechaprod.AddMonths(Convert.ToInt32(orden.FechaVencimiento))).Year.ToString.PadLeft(4, "0")
                _orden = fechaprod.Year.ToString.PadLeft(4, "0") + fechaprod.Month.ToString.PadLeft(2, "0") + fechaprod.Day.ToString.PadLeft(2, "0")
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            _codigo = orden.Codigo
            _descripcion2 = orden.Descripcion2
            _descripcion3 = orden.Descripcion3
            _descripcion4 = orden.Descripcion4
            _descripcion5 = orden.Descripcion5
        Catch ex As Exception
            Logs.nuevo("Error creando caja" + ex.Message)
            MsgBox("Error creando caja" + ex.Message)

        End Try
    End Sub
#End Region



#Region "Propertys"
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
            Return _fechavencimiento
        End Get
        Set(ByVal value As String)
            _fechavencimiento = value
        End Set
    End Property
    Public Property FechaProduccion() As String
        Get
            Return _fechaproduccion
        End Get
        Set(ByVal value As String)
            _fechaproduccion = value
        End Set
    End Property
    Public Property Codbar() As String
        Get
            Return _codbar
        End Get
        Set(ByVal value As String)
            _codbar = value
        End Set
    End Property
    Public Property Numero() As Integer
        Get
            Return _numero
        End Get
        Set(ByVal value As Integer)
            _numero = value
        End Set
    End Property

    Public Property HoraFecha() As Date
        Get
            Return _horayfecha
        End Get
        Set(ByVal value As Date)
            _horayfecha = value
        End Set
    End Property
    Public Property Linea() As String
        Get
            Return _linea
        End Get
        Set(ByVal value As String)
            _linea = value
        End Set
    End Property

    Public Property Procesado() As Boolean
        Get
            Return _procesado
        End Get
        Set(ByVal value As Boolean)
            _procesado = value
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

    Public Sub guardarCaja()
        Repositorio.guardarcaja(Me)
    End Sub

End Class
