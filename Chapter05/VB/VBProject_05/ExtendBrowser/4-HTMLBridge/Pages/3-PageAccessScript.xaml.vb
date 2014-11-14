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

    Public Partial Class PageAccessScript
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub '   New


        private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

          '   HtmlDocument doc = HtmlPage.Document
        End Sub '   LayoutRoot_Loaded


        private Sub Button_Click(sender As Object, e As RoutedEventArgs)

            Dim strTime As String  = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString()

            Try
                HtmlPage.Window.Invoke("GetTime", strTime)

            Catch ex As Exception

                HtmlPage.Window.Alert("Error while calling JavaScript Method: GetTime() \nCheck if the function exist in the page")
            End Try
        End Sub '   Button_Click
    End Class   '   PageAccessScript
' End Namespace   '   HTMLBridge
' ..\Project_05\ExtendBrowser\4-HTMLBridge\Pages\3-PageAccessScript.xaml.cs
