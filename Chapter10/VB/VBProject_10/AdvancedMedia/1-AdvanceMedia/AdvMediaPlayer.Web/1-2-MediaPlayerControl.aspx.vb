Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

' Namespace AdvMediaPlayer.Web

Partial Public Class _2_MediaPlayerControl
    Inherits System.Web.UI.Page

    'Shared m_rLinkButtonFormer As LinkButton = Nothing

    Protected Sub Page_Load(sender As Object, e As EventArgs)

    End Sub '   Page_Load

    Protected Sub Skin_Click(sender As Object, e As EventArgs)

        Dim rLinkButtonNew As LinkButton = CType(sender, LinkButton)

        Dim selectedSkin As String = rLinkButtonNew.Text

        'Dim rControl As System.Web.UI.WebControls.WebControl

        If (rLinkButtonNew.Text = "Prof.") Then
            selectedSkin = "Professional"
        End If

        'If (m_rLinkButtonFormer IsNot Nothing) Then
        '    With m_rLinkButtonFormer
        '        .Font.Bold = False
        '        .Font.Italic = False
        '    End With
        'End If

        Basic.Font.Bold = False
        Basic.Font.Italic = False
        Classic.Font.Bold = False
        Classic.Font.Italic = False
        Console.Font.Bold = False
        Console.Font.Italic = False
        Expression.Font.Bold = False
        Expression.Font.Italic = False
        Futuristic.Font.Bold = False
        Futuristic.Font.Italic = False
        Professional.Font.Bold = False
        Professional.Font.Italic = False
        Simple.Font.Bold = False
        Simple.Font.Italic = False
        AudioGray.Font.Bold = False
        AudioGray.Font.Italic = False

        'm_rLinkButtonFormer = rLinkButtonNew

        With rLinkButtonNew
            .Font.Bold = True
            .Font.Italic = True
        End With

        'With SkinTable
        '    For Each oRow As HtmlControls.HtmlTableRow In .Rows
        '        For Each oCell As HtmlControls.HtmlTableCell In oRow.Cells
        '            For Each rControl In oCell.Controls    ' cast error here
        '                If (rControl Is GetType(LinkButton)) Then
        '                    rLinkButton = CType(rControl, LinkButton)

        '                    With rLinkButton
        '                        .BackColor = Drawing.Color.Yellow
        '                        .Font.Bold = .Equals(l)
        '                        .Font.Italic = .Equals(l)
        '                    End With
        '                End If
        '            Next
        '        Next
        '    Next
        'End With

        MediaPlayer1.MediaSkinSource = "~/MediaSkins/" + selectedSkin + ".xaml"
    End Sub '   Skin_Click
End Class   '   _2_MediaPlayerControl
' End Namespace   '   AdvMediaPlayer.Web
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\2-MediaPlayerControl.aspx.cs
