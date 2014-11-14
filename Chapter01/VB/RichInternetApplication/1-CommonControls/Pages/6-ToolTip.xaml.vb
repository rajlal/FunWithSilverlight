Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
' Namespace CommonControls

Partial Public Class PageToolTip
    Inherits UserControl

    Public Sub New()

        InitializeComponent()
        SetToolTip3()
    End Sub '   New

    Private Sub SetToolTip3()

        Dim tt As ToolTip = New ToolTip()
        tt.Template = CType(Resources("ToolTipTemplate"), ControlTemplate)
        tt.Content = New TextBlock() With {
            .FontFamily = New FontFamily("Arial"),
            .FontSize = 12,
            .Text = "Tooltip generated in the code.",
            .TextWrapping = TextWrapping.Wrap}
        ToolTipService.SetToolTip(ToolTip3, tt)
    End Sub '   SetToolTip3
End Class   '   PageToolTip
' End Namespace 
' ..\RichInternetApplication\1-CommonControls\Pages\6-ToolTip.xaml.cs
