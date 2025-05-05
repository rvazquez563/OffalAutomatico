Imports System.Security.Cryptography

Public Class SHA1

    Public Shared Function encriptar(texto As String) As String
        Dim resultado As String = Nothing
        Dim Molecul3 As New SHA1CryptoServiceProvider
        Dim byteString() As Byte = System.Text.Encoding.ASCII.GetBytes(texto)

        byteString = Molecul3.ComputeHash(byteString)

        For Each b As Byte In byteString
            resultado &= b.ToString("x2")
        Next
        Return resultado
    End Function
End Class
