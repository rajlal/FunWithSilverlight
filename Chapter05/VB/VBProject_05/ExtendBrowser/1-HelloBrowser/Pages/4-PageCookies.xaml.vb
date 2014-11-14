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

' Namespace HelloBrowser

Partial Public Class PageCookies
    Inherits UserControl

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)

        If (HtmlPage.BrowserInformation.CookiesEnabled) Then

            If (CType(chkSaveCookie.IsChecked, Boolean)) Then
                '  for secure connection 6th parameter ="secure"
                SaveCookie("name", txtName.Text, 7, "/", "", "")
                SaveCookie("email", txtEmail.Text, 7, "/", "", "")
                SaveCookie("web", txtWeb.Text, 7, "/", "", "")
                txtStatus.Text = "Information saved in Browser Cookies\nRefresh Page or Click Return to see the values"
            Else
                txtStatus.Text = "Information not saved"
            End If

            partLogin.Visibility = Visibility.Collapsed
            partStatus.Visibility = Visibility.Visible
        End If
    End Sub '   Button_Click

    Private Function ReadCookie(key As String) As String

        If (HtmlPage.BrowserInformation.CookiesEnabled) Then

            Dim arrayCookies As String() = HtmlPage.Document.Cookies.Split(";"c)

            For Each cookie As String In arrayCookies

                Dim cookieKeyValues As String() = cookie.Trim().Split("="c)

                If (cookieKeyValues.Length = 2) Then

                    If (cookieKeyValues(0).ToString() = key) Then
                        Return cookieKeyValues(1)
                    End If
                End If
            Next    '   cookie
        End If

        Return ""
    End Function  '   ReadCookie

    Private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

        txtName.Text = ReadCookie("name")
        txtEmail.Text = ReadCookie("email")
        txtWeb.Text = ReadCookie("web")
        partLogin.Visibility = Visibility.Visible
        partStatus.Visibility = Visibility.Collapsed
    End Sub '   LayoutRoot_Loaded

    Private Sub SaveCookie(key As String, value As String, expires As Integer, path As String, domain As String, secure As String)

        Dim myDoc As HtmlDocument = HtmlPage.Document

        Dim expireDate As DateTime = DateTime.Now + TimeSpan.FromDays(expires)
        Dim expiration As DateTime = DateTime.UtcNow + TimeSpan.FromDays(expires)
        Dim cookie As String = ""

        cookie = String.Format("{0}={1};expires={2};path={3};domain={4}", key, value, expiration.ToString("R"), path, domain)

        HtmlPage.Document.SetProperty("cookie", cookie)
    End Sub '   SaveCookie

    Private Sub Delete_Click(sender As Object, e As RoutedEventArgs)

        DeleteCookie("name", "/", "")
        DeleteCookie("email", "/", "")
        DeleteCookie("web", "/", "")
        txtStatus.Text = "Cookies Deleted  Not \nRefresh Page or Click Return to go back"
    End Sub '   Delete_Click

    Private Sub DeleteCookie(key As String, path As String, domain As String)

        Dim expiration As DateTime = DateTime.UtcNow - TimeSpan.FromDays(1)
        Dim cookie As String = String.Format("{0}=;expires={1};path={2};domain={3}", key, expiration.ToString("R"), path, domain)

        HtmlPage.Document.SetProperty("cookie", cookie)
    End Sub '   DeleteCookie

    Private Sub Return_Click(sender As Object, e As RoutedEventArgs)

        Dim documentUri As String = HtmlPage.Document.DocumentUri.OriginalString

        documentUri = documentUri.Split("?"c)(0)

        Dim h As HtmlWindow = HtmlPage.Window

        h.Navigate(New Uri(documentUri))
    End Sub '   Return_Click
End Class   '   PageCookies
' End Namespace   '   HelloBrowser
' ..\Project_05\ExtendBrowser\1-HelloBrowser\Pages\4-PageCookies.xaml.cs
