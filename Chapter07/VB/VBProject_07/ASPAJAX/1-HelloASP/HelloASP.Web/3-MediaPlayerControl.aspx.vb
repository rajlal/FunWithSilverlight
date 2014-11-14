Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace _1_HelloASP.Web

    Partial Public Class __MediaPlayerControl
        Inherits System.Web.UI.Page
        Protected Sub Page_Load(sender As Object, e As EventArgs)
        End Sub '   Page_Load

        Protected Sub Skin_Click(sender As Object, e As EventArgs)

            Dim l As LinkButton = CType(sender, LinkButton)
            Dim selectedSkin As String = l.Text

            If (l.Text = "Prof.") Then
                selectedSkin = "Professional"
            End If

            MediaPlayer1.MediaSkinSource = "~/MediaSkins/" + selectedSkin + ".xaml"
        End Sub '   Skin_Click

        Protected Sub HighQuality_Click(sender As Object, e As EventArgs)

            If (MediaPlayer1.MediaSource = "~/Videos/SilverLight_Intro.wmv") Then
                MediaPlayer1.MediaSource = "~/Videos/SilverLight_IntroHQ.mp4"
                HighQuality.Text = "Regular Quality"
            Else
                MediaPlayer1.MediaSource = "~/Videos/SilverLight_Intro.wmv"
                HighQuality.Text = "High Quality"
            End If
        End Sub '   HighQuality_Click

        Protected Sub DisableGPU_Click(sender As Object, e As EventArgs)


            If (MediaPlayer1.EnableGPUAcceleration) Then
                MediaPlayer1.EnableGPUAcceleration = False
                DisableGPU.Text = "Enable GPU Acc."
            Else
                MediaPlayer1.EnableGPUAcceleration = True
                DisableGPU.Text = "Disable GPU Acc."
            End If
        End Sub '   DisableGPU_Click

        Protected Sub CacheVisual_Click(sender As Object, e As EventArgs)

            If (Not MediaPlayer1.EnableCacheVisualization) Then
                MediaPlayer1.EnableCacheVisualization = True
                CacheVisual.Text = "Disable Cache Vis."
            Else
                MediaPlayer1.EnableCacheVisualization = False
                CacheVisual.Text = "Enable Cache Vis."
            End If
        End Sub '   CacheVisual_Click
    End Class   '   __MediaPlayerControl
End Namespace   '   _1_HelloASP.Web
' ..\Project_07\ASPAJAX\1-HelloASP\HelloASP.Web\3-MediaPlayerControl.aspx.cs
