﻿#ExternalChecksum("C:\Users\Main OutDoor\Documents\Visual Studio 2010\Projects\S\Silverlight\VBProject_09\Media\1-RichMedia\Media\2-pageMediaState.xaml","{406ea660-64cf-4c82-b6f0-42d48172a799}","C30778FB65275DDBF4961001D711148D")
'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.261
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.Windows
Imports System.Windows.Automation
Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Interop
Imports System.Windows.Markup
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Imaging
Imports System.Windows.Resources
Imports System.Windows.Shapes
Imports System.Windows.Threading



<Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  _
Partial Public Class pageMediaState
    Inherits System.Windows.Controls.UserControl
    
    Friend WithEvents canvasMedia As System.Windows.Controls.Canvas
    
    Friend WithEvents Media As System.Windows.Controls.MediaElement
    
    Friend WithEvents btnPlaybig As System.Windows.Controls.Image
    
    Friend WithEvents borderMediastates As System.Windows.Controls.Border
    
    Friend WithEvents btnPlay As System.Windows.Controls.Image
    
    Friend WithEvents btnPause As System.Windows.Controls.Image
    
    Friend WithEvents btnStop As System.Windows.Controls.Image
    
    Friend WithEvents btnVolumeDown As System.Windows.Controls.Image
    
    Friend WithEvents txtVolume As System.Windows.Controls.TextBlock
    
    Friend WithEvents btnVolumeup As System.Windows.Controls.Image
    
    Friend WithEvents btnRewind As System.Windows.Controls.Image
    
    Friend WithEvents btnFastForward As System.Windows.Controls.Image
    
    Friend WithEvents btnFullscreen As System.Windows.Controls.Image
    
    Friend WithEvents borderStatus As System.Windows.Controls.Border
    
    Friend WithEvents StatusGrid As System.Windows.Controls.Grid
    
    Friend WithEvents StatusBar As System.Windows.Controls.TextBlock
    
    Friend WithEvents StatusPosition As System.Windows.Controls.TextBlock
    
    Private _contentLoaded As Boolean
    
    '''<summary>
    '''InitializeComponent
    '''</summary>
    <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Sub InitializeComponent()
        If _contentLoaded Then
            Return
        End If
        _contentLoaded = true
        System.Windows.Application.LoadComponent(Me, New System.Uri("/Media;component/2-pageMediaState.xaml", System.UriKind.Relative))
        Me.canvasMedia = CType(Me.FindName("canvasMedia"),System.Windows.Controls.Canvas)
        Me.Media = CType(Me.FindName("Media"),System.Windows.Controls.MediaElement)
        Me.btnPlaybig = CType(Me.FindName("btnPlaybig"),System.Windows.Controls.Image)
        Me.borderMediastates = CType(Me.FindName("borderMediastates"),System.Windows.Controls.Border)
        Me.btnPlay = CType(Me.FindName("btnPlay"),System.Windows.Controls.Image)
        Me.btnPause = CType(Me.FindName("btnPause"),System.Windows.Controls.Image)
        Me.btnStop = CType(Me.FindName("btnStop"),System.Windows.Controls.Image)
        Me.btnVolumeDown = CType(Me.FindName("btnVolumeDown"),System.Windows.Controls.Image)
        Me.txtVolume = CType(Me.FindName("txtVolume"),System.Windows.Controls.TextBlock)
        Me.btnVolumeup = CType(Me.FindName("btnVolumeup"),System.Windows.Controls.Image)
        Me.btnRewind = CType(Me.FindName("btnRewind"),System.Windows.Controls.Image)
        Me.btnFastForward = CType(Me.FindName("btnFastForward"),System.Windows.Controls.Image)
        Me.btnFullscreen = CType(Me.FindName("btnFullscreen"),System.Windows.Controls.Image)
        Me.borderStatus = CType(Me.FindName("borderStatus"),System.Windows.Controls.Border)
        Me.StatusGrid = CType(Me.FindName("StatusGrid"),System.Windows.Controls.Grid)
        Me.StatusBar = CType(Me.FindName("StatusBar"),System.Windows.Controls.TextBlock)
        Me.StatusPosition = CType(Me.FindName("StatusPosition"),System.Windows.Controls.TextBlock)
    End Sub
End Class

