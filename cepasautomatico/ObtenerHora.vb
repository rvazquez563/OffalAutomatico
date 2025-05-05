Imports System
Imports System.Collections.Generic
Imports System.Text

Public Class ObtenerHora

    Public Shared Function obtenerHora() As String
        Dim fecha As String = (DateTime.Today.Day.ToString + ("/" _
                    + (DateTime.Today.Month.ToString + ("/" _
                    + (DateTime.Today.Year.ToString + (" " _
                    + (DateTime.Now.Hour.ToString + (":" _
                    + (DateTime.Now.Minute.ToString + (":" + DateTime.Now.Second.ToString))))))))))
        Return fecha
    End Function
End Class