Imports System.Drawing.Printing

Public Class Impresora

#Region "Variables miembro"
    Private _driver As String
    Private _impresora As tipo

    Public Enum tipo
        manual
        aplicadora
    End Enum
#End Region
    
#Region "Constructor"
    ''' <summary>
    ''' Instancia un nuevo objeto impresora
    ''' </summary>
    ''' <param name="driver">Driver de Windows</param>
    ''' <param name="impresora">Impresora manual o aplicadora</param>
    ''' <remarks></remarks>
    Public Sub New(driver As String, impresora As tipo)
        _driver = driver
        _impresora = impresora
    End Sub
#End Region
    
#Region "Propertys"
    Public Property Driver() As String
        Get
            Return _driver
        End Get
        Set(ByVal value As String)
            _driver = value
        End Set
    End Property
#End Region

#Region "Métodos"

    ''' <summary>
    ''' Devuelve verdadero si existe el driver
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function existeDriver() As Boolean
        Dim existe As Boolean = False
        For Each p As String In PrinterSettings.InstalledPrinters
            If p.Equals(_driver) Then
                existe = True
            End If
        Next
        Return existe
    End Function

    Public Sub LimpiarBuffer(linea As Integer)
        Dim etiqueta As String
        etiqueta = "~JA" + vbCrLf
        RawPrinterHelper.SendStringToPrinter(_driver, etiqueta)

        'frmMain.escribirMensaje("Limpié el buffer de la impresora", linea)
    End Sub

    ''' <summary>
    ''' Envío una impresion por el puerto de red
    ''' </summary>
    ''' <param name="p">Pallet a imprimir</param>
    ''' <param name="usr">Usuario que esta enviando la impresión</param>
    ''' <remarks></remarks>
    Public Sub imprimir(p As caja)
        Dim etiqueta As String = ""
        Dim sChr = "^"
        Dim sVto As String
        Dim vto As DateTime = DateTime.Now
        Dim ultimoCampo As String
        '------------------------------------------------------------
        'vto = vto.AddDays(p.DiasVencimiento)
        sVto = vto.Day.ToString.PadLeft(2, "0") + "." + vto.Month.ToString.PadLeft(2, "0") + "." + vto.Year.ToString
        'ultimoCampo = usr.Nombre + "    " + DateTime.Now.Day.ToString.PadLeft(2, "0") + "/" + DateTime.Now.Month.ToString.PadLeft(2, "0") + "/" + DateTime.Now.Year.ToString.PadLeft(2, "0") + "    " + DateTime.Now.Hour.ToString.PadLeft(2, "0") + ":" + DateTime.Now.Minute.ToString.PadLeft(2, "0")
        '------------------------------------------------------------
        ''Guardo los datos a imprimir en una variable
        'etiqueta += "~" + "JA" + vbCrLf
        etiqueta += sChr + "XA" + vbCrLf
        etiqueta += sChr + "XF" + "E:" + p.Etiqueta.ToString + vbCrLf                                       'principal
        etiqueta += sChr + "FN1" + sChr + "FD" + p.Descripcion + sChr + "FS" + vbCrLf                       'Descripcion
        etiqueta += sChr + "FN2" + sChr + "FD" + p.Codigo + sChr + "FS" + vbCrLf                            'Codigo 
        etiqueta += sChr + "FN3" + sChr + "FD" + p.Codbar + sChr + "FS" + vbCrLf                            'Codigo datamatrix
        etiqueta += sChr + "FN4" + sChr + "FD>:" + p.Codbar + sChr + "FS" + vbCrLf                          'Codigo de barras
        etiqueta += sChr + "FN5" + sChr + "FD" + p.FechaProduccion.ToString + sChr + "FS" + vbCrLf          'fecha de produccion    
        etiqueta += sChr + "FN6" + sChr + "FD" + p.FechaVencimiento.ToString + sChr + "FS" + vbCrLf         'fecha de vencimiento
        etiqueta += sChr + "FN7" + sChr + "FD" + p.Orden.ToString + sChr + "FS" + vbCrLf                    'nro de orden
        etiqueta += sChr + "FN8" + sChr + "FD" + p.Descripcion2.ToString + sChr + "FS" + vbCrLf                    'nro de orden
        etiqueta += sChr + "FN9" + sChr + "FD" + p.Descripcion3.ToString + sChr + "FS" + vbCrLf                    'nro de orden
        etiqueta += sChr + "FN10" + sChr + "FD" + p.Descripcion4.ToString + sChr + "FS" + vbCrLf                    'nro de orden
        etiqueta += sChr + "FN11" + sChr + "FD" + p.Descripcion5.ToString + sChr + "FS" + vbCrLf
        etiqueta += sChr + "PQ1+0+1+Y" + vbCrLf
        etiqueta += sChr + "XZ" + vbCrLf
        'Mando a imprimir la etiqueta
        RawPrinterHelper.SendStringToPrinter(_driver, etiqueta)

    End Sub

    Public Sub imprimir1(p As caja, usr As Usuario)
        Dim etiqueta As String = ""
        Dim sChr = "^"
        Dim sVto As String
        Dim vto As DateTime = DateTime.Now
        Dim ultimoCampo As String
        '------------------------------------------------------------
        'vto = vto.AddDays(p.DiasVencimiento)
        sVto = vto.Day.ToString.PadLeft(2, "0") + "." + vto.Month.ToString.PadLeft(2, "0") + "." + vto.Year.ToString
        ultimoCampo = usr.Nombre + "    " + DateTime.Now.Day.ToString.PadLeft(2, "0") + "/" + DateTime.Now.Month.ToString.PadLeft(2, "0") + "/" + DateTime.Now.Year.ToString.PadLeft(2, "0") + "    " + DateTime.Now.Hour.ToString.PadLeft(2, "0") + ":" + DateTime.Now.Minute.ToString.PadLeft(2, "0")
        '------------------------------------------------------------
        ''Guardo los datos a imprimir en una variable
        'etiqueta += "~" + "JA" + vbCrLf
        etiqueta += sChr + "XA" + vbCrLf
        etiqueta += sChr + "XF" + "E:" + p.Presentacion.ToString + vbCrLf                                   'secundaria
        etiqueta += sChr + "FN1" + sChr + "FD" + p.Descripcion + sChr + "FS" + vbCrLf                       'Descripcion
        etiqueta += sChr + "FN2" + sChr + "FD" + p.Codigo + sChr + "FS" + vbCrLf                            'Codigo 
        etiqueta += sChr + "FN3" + sChr + "FD" + p.Codbar + sChr + "FS" + vbCrLf                            'Codigo datamatrix
        etiqueta += sChr + "FN4" + sChr + "FD>:" + p.Codbar + sChr + "FS" + vbCrLf                          'Codigo de barras
        etiqueta += sChr + "FN5" + sChr + "FD" + p.FechaProduccion.ToString + sChr + "FS" + vbCrLf          'fecha de produccion    
        etiqueta += sChr + "FN6" + sChr + "FD" + p.FechaVencimiento.ToString + sChr + "FS" + vbCrLf         'fecha de vencimiento
        etiqueta += sChr + "FN7" + sChr + "FD" + p.Orden.ToString + sChr + "FS" + vbCrLf                    'nro de orden
        etiqueta += sChr + "FN8" + sChr + "FD" + p.Descripcion2.ToString + sChr + "FS" + vbCrLf                    'nro de orden
        etiqueta += sChr + "FN9" + sChr + "FD" + p.Descripcion3.ToString + sChr + "FS" + vbCrLf                    'nro de orden
        etiqueta += sChr + "FN10" + sChr + "FD" + p.Descripcion4.ToString + sChr + "FS" + vbCrLf                    'nro de orden
        etiqueta += sChr + "FN11" + sChr + "FD" + p.Descripcion5.ToString + sChr + "FS" + vbCrLf
        etiqueta += sChr + "PQ1+0+1+Y" + vbCrLf
        etiqueta += sChr + "XZ" + vbCrLf
        'Mando a imprimir la etiqueta
        RawPrinterHelper.SendStringToPrinter(_driver, etiqueta)

    End Sub

    Public Sub imprimirprueba()
        Dim etiqueta As String = ""
        Dim sChr = "^"
        Dim sVto As String
        Dim vto As DateTime = DateTime.Now
        'Dim ultimoCampo As String
        '------------------------------------------------------------
        'vto = vto.AddDays(p.DiasVencimiento)
        sVto = vto.Day.ToString.PadLeft(2, "0") + "." + vto.Month.ToString.PadLeft(2, "0") + "." + vto.Year.ToString
        'ultimoCampo = usr.Nombre + "    " + DateTime.Now.Day.ToString.PadLeft(2, "0") + "/" + DateTime.Now.Month.ToString.PadLeft(2, "0") + "/" + DateTime.Now.Year.ToString.PadLeft(2, "0") + "    " + DateTime.Now.Hour.ToString.PadLeft(2, "0") + ":" + DateTime.Now.Minute.ToString.PadLeft(2, "0")
        '------------------------------------------------------------
        ''Guardo los datos a imprimir en una variable
        'etiqueta += "~" + "JA" + vbCrLf
        etiqueta += sChr + "XA" + vbCrLf
        etiqueta += sChr + "WD" + vbCrLf
        etiqueta += sChr + "XZ" + vbCrLf
        'Mando a imprimir la etiqueta
        RawPrinterHelper.SendStringToPrinter(_driver, etiqueta)

    End Sub
#End Region

End Class
