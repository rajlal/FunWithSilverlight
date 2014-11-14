Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes
Imports System.Windows.Browser

' Namespace HTMLBridge

Partial Public Class PageAccessSilverlight
    Inherits UserControl

    Public Sub New()

        InitializeComponent()
        '  Create and register a scriptable object.
        HtmlPage.RegisterScriptableObject("ScriptableMember", New ScriptableMember())
        HtmlPage.RegisterScriptableObject("ScriptableClass", New ScriptableClass())
    End Sub '   New

    Private Sub LayoutRoot_Loaded(sender As Object, e As EventArgs)

    End Sub

    Public Class ScriptableMember
        <ScriptableMember()>
        Public Function IsPalindrome(s As String) As Boolean

            Dim length As Integer = s.Length

            Dim chrArray As Char() = s.ToCharArray()

            If (length = 0) Then
                Return False
            End If

            If (length = 1) Then
                Return True
            End If

            Dim nStart As Integer = 0
            Dim nEnd As Integer = length - 1

            While (nEnd > nStart)
                If (chrArray(nStart) = chrArray(nEnd)) Then
                    nStart += 1
                    nEnd -= 1
                Else
                    Return False
                End If
            End While   '
            Return True
        End Function  '   IsPalindrome
    End Class   '   ScriptableMember

    <ScriptableType()>
    Public Class ScriptableClass

        Public Function GetLength(s As String) As Integer

            Return s.Length
        End Function  '   GetLength
    End Class   '   ScriptableClass

    Private Sub Palindrome_Click(sender As Object, e As RoutedEventArgs)

        Dim localclass As ScriptableMember = New ScriptableMember()

        txtValue.Text = localclass.IsPalindrome(txtValue.Text).ToString().ToLower()
    End Sub '   Palindrome_Click

    Private Sub GetLength_Click(sender As Object, e As RoutedEventArgs)

        Dim localclass As ScriptableClass = New ScriptableClass()

        txtValue.Text = localclass.GetLength(txtValue.Text).ToString().ToLower()
    End Sub '   GetLength_Click
End Class   '   PageAccessSilverlight
' End Namespace   '   HTMLBridge
' ..\Project_05\ExtendBrowser\4-HTMLBridge\Pages\4-PageAccessSilverlight.xaml.cs
