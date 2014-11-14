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

Partial Public Class PageAccessDOM
    Inherits UserControl

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub GetId()

        CollapseAll()
        CanvasId.Visibility = Visibility.Visible

        Dim elementSL As HtmlElement = HtmlPage.Document.GetElementById("ContentText")

        If (elementSL IsNot Nothing) Then
            txtID.Text = elementSL.GetProperty("OuterHtml").ToString()
        Else
            txtID.Text = "Element with id=ContentText not found"
        End If
    End Sub '   GetId

    Private Function GetNodeChildren(elemColl As ScriptObjectCollection, returnStr As System.Text.StringBuilder, depth As Integer) As String

        Dim str As System.Text.StringBuilder = New System.Text.StringBuilder()


        For Each elem As HtmlElement In elemColl

            Dim elemName As String

            elemName = elem.GetAttribute("id")

            If (elemName Is Nothing OrElse elemName.Length = 0) Then
                elemName = "<No id>"
            Else
                elemName = "<" + elemName + ">"
            End If

            str.Append(" "c, depth * 4)
            str.Append(elemName + ": " + elem.TagName)
            returnStr.AppendLine(str.ToString())

            Try
                GetNodeChildren(elem.Children, returnStr, depth + 1)
            Catch ex As Exception

            End Try

            str.Remove(0, str.Length)
        Next    '   HtmlElement

        Return (returnStr.ToString())
    End Function  '   GetNodeChildren

    Private Sub GetTreeCollection()


        If (HtmlPage.Document IsNot Nothing) Then
            Dim elemColl As ScriptObjectCollection = Nothing
            Dim doc As HtmlDocument = HtmlPage.Document

            If (doc IsNot Nothing) Then
                elemColl = doc.GetElementsByTagName("HTML")

                Dim str As String = GetNodeChildren(elemColl, New System.Text.StringBuilder(), 0)

                txtHTML.Text = str
            End If
        End If
    End Sub '   GetTreeCollection

    Private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

        GetId()
    End Sub '   LayoutRoot_Loaded

    Private Sub showID(sender As Object, e As MouseButtonEventArgs)

        GetId()
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   showID

    Private Sub showTree(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        CanvasTree.Visibility = Visibility.Visible
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        GetTreeCollection()
    End Sub '   showTree

    Private Sub showTag(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        CanvasTag.Visibility = Visibility.Visible

        Dim DivTags As ScriptObjectCollection = HtmlPage.Document.GetElementsByTagName("DIV")

        txtTag.Text += "  " + CStr(DivTags.Count) + " DIV tags found in the HTML page" + Environment.NewLine + Environment.NewLine

        Dim i As Integer = 1


        For Each oDiv As HtmlElement In DivTags

            Dim elemID As String = oDiv.GetAttribute("id")

            txtTag.Text += "   " + CStr(i) + "." + " Id=" + elemID + Environment.NewLine
            i += 1
        Next    '   HtmlElement

        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   showTag

    Private Sub CollapseAll()

        CanvasId.Visibility = Visibility.Collapsed
        CanvasTag.Visibility = Visibility.Collapsed
        CanvasTree.Visibility = Visibility.Collapsed
        txtID.Text = ""
        txtTag.Text = ""
        txtHTML.Text = ""
    End Sub '   CollapseAll
End Class   '   PageAccessDOM
' End Namespace   '   HTMLBridge
' ..\Project_05\ExtendBrowser\4-HTMLBridge\Pages\2-PageAccessDOM.xaml.cs
