Imports System.Runtime.InteropServices


Public Class WinApi

    <DllImport("user32.dll", EntryPoint:="GetSystemMetrics")> _
    Public Shared Function GetSystemMetrics(which As Integer) As Integer
    End Function

    <DllImport("user32.dll")> _
    Public Shared Sub SetWindowPos(hwnd As IntPtr, hwndInsertAfter As IntPtr, X As Integer, Y As Integer, width As Integer, height As Integer, _
        flags As UInteger)
    End Sub

    Private Const SM_CXSCREEN As Integer = 0
    Private Const SM_CYSCREEN As Integer = 1
    Private Shared HWND_TOP As IntPtr = IntPtr.Zero
    Private Const SWP_SHOWWINDOW As Integer = 64
    ' 0x0040
    Public Shared ReadOnly Property ScreenX() As Integer
        Get
            Return GetSystemMetrics(SM_CXSCREEN)
        End Get
    End Property

    Public Shared ReadOnly Property ScreenY() As Integer
        Get
            Return GetSystemMetrics(SM_CYSCREEN)
        End Get
    End Property

    Public Shared Sub SetWinFullScreen(hwnd As IntPtr)
        SetWindowPos(hwnd, HWND_TOP, 0, 0, ScreenX, ScreenY, _
            SWP_SHOWWINDOW)
    End Sub
End Class

''' <summary>
''' Class used to preserve / restore state of the form
''' </summary>
Public Class FormState
    Private winState As FormWindowState
    Private brdStyle As FormBorderStyle
    Private topMost As Boolean
    Private bounds As Rectangle

    Private IsMaximized As Boolean = False

    Public Sub Maximize(targetForm As Form)
        If Not IsMaximized Then
            IsMaximized = True
            Save(targetForm)
            targetForm.WindowState = FormWindowState.Maximized
            targetForm.FormBorderStyle = FormBorderStyle.None
            targetForm.TopMost = False
            WinApi.SetWinFullScreen(targetForm.Handle)
        End If
    End Sub

    Public Sub Save(targetForm As Form)
        winState = targetForm.WindowState
        brdStyle = targetForm.FormBorderStyle
        topMost = targetForm.TopMost
        bounds = targetForm.Bounds
    End Sub

    Public Sub Restore(targetForm As Form)
        targetForm.WindowState = winState
        targetForm.FormBorderStyle = brdStyle
        targetForm.TopMost = topMost
        targetForm.Bounds = bounds
        IsMaximized = False
    End Sub
End Class

