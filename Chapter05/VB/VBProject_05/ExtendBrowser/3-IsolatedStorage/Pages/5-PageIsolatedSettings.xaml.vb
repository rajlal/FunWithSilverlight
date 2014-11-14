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
Imports System.Windows.Media.Imaging
Imports System.Windows.Browser
Imports System.IO.IsolatedStorage

' Namespace LocalStorage

Partial Public Class PageIsolatedSettings
    Inherits UserControl

    Private appSettings As IsolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

        LoadSettings()
    End Sub '   LayoutRoot_Loaded

    Private Sub go_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        Dim h As HtmlWindow = HtmlPage.Window
        Dim i As Image = CType(sender, Image)
        Dim myUri As String = CType(appSettings(i.Name.Substring(5).ToLower()), String)

        h.Navigate(New Uri(myUri), "_blank")
    End Sub '   go_MouseLeftButtonUp

    Private Sub delete_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        Dim i As Image = CType(sender, Image)

        appSettings.Remove(i.Name.Substring(6).ToLower())
        LoadSettings()
    End Sub '   delete_MouseLeftButtonUp

    Private Sub LoadSettings()

        txtHome.Text = "Home"
        txtEmail.Text = "Email"
        txtSearch.Text = "Search"
        txtSilverlight.Text = "Silverlight"

        If (appSettings.Contains("name")) Then
            txtSettingsName.Text = CType(appSettings("name"), String)
            txtSettings.Text = CType(appSettings("name"), String)
        End If

        If (appSettings.Contains("home")) Then
            txtHome.Text = "" + CType(appSettings("home"), String)
        End If

        If (appSettings.Contains("email")) Then
            txtEmail.Text = "" + CType(appSettings("email"), String)
        End If

        If (appSettings.Contains("search")) Then
            txtSearch.Text = "" + CType(appSettings("search"), String)
        End If

        If (appSettings.Contains("silverlight")) Then
            txtSilverlight.Text = "" + CType(appSettings("silverlight"), String)
        End If

        cmbSettings.SelectedIndex = 0
    End Sub '   LoadSettings

    Private Sub ResetSettings()

        appSettings.Clear()
        appSettings.Add("name", "Scott Guthrie")
        appSettings.Add("home", "http://weblogs.asp.net/Scottgu/")
        appSettings.Add("email", "http://hotmail.com")
        appSettings.Add("search", "http://google.com")
        appSettings.Add("silverlight", "http://silverlightfun.com")
        txtSettingsName.Text = CType(appSettings("name"), String)
    End Sub '   ResetSettings

    Private Sub SaveSettings()

        Dim cbi As ComboBoxItem = CType(cmbSettings.SelectedItem, ComboBoxItem)

        appSettings(cbi.Content.ToString().ToLower()) = txtSettings.Text
        LoadSettings()

        txtStatus.Foreground = New SolidColorBrush(Colors.Green)
        txtStatus.Text = "Saved  Not "
    End Sub '   SaveSettings

    Private Sub cmbSettings_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

        Try
            txtStatus.Foreground = New SolidColorBrush(Colors.Black)
            txtStatus.Text = "Edit Settings"

            Dim cbi As ComboBoxItem = CType(cmbSettings.SelectedItem, ComboBoxItem)

            txtSettings.Text = CType(appSettings(cbi.Content.ToString().ToLower()), String)

        Catch ex As Exception

        End Try
    End Sub '   cmbSettings_SelectionChanged
    Private Sub Save(sender As Object, e As RoutedEventArgs)

        SaveSettings()
    End Sub '   Save

    Private Sub Reset(sender As Object, e As RoutedEventArgs)

        ResetSettings()
        LoadSettings()
    End Sub '   Reset

    Private Sub txtSettings_TextChanged(sender As Object, e As TextChangedEventArgs)

        txtStatus.Foreground = New SolidColorBrush(Colors.Black)
        txtStatus.Text = "Edit Settings"
    End Sub '   txtSettings_TextChanged
End Class   '   PageIsolatedSettings
' End Namespace   '   LocalStorage
' ..\Project_05\ExtendBrowser\3-IsolatedStorage\Pages\5-PageIsolatedSettings.xaml.cs
