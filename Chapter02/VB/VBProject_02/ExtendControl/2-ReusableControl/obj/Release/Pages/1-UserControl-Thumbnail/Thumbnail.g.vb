﻿#ExternalChecksum("C:\Users\Main OutDoor\Documents\Visual Studio 2010\Projects\E\ExtendControlVB\ExtendControlVB\2-ReusableControl\Pages\1-UserControl-Thumbnail\Thumbnail.xaml","{406ea660-64cf-4c82-b6f0-42d48172a799}","B51E82E9911F4AD2DDA394D18576FF5A")
'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.17379
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
Partial Public Class Thumbnail
    Inherits System.Windows.Controls.UserControl
    
    Friend WithEvents Root As System.Windows.Controls.Canvas
    
    Friend WithEvents ThumbShadow As System.Windows.Shapes.Rectangle
    
    Friend WithEvents ThumbBorder As System.Windows.Controls.Border
    
    Friend WithEvents ThumbGrid As System.Windows.Controls.Grid
    
    Friend WithEvents ThumbImage As System.Windows.Controls.Image
    
    Friend WithEvents ThumbnailText As System.Windows.Controls.TextBlock
    
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
        System.Windows.Application.LoadComponent(Me, New System.Uri("/ReusableControl;component/Pages/1-UserControl-Thumbnail/Thumbnail.xaml", System.UriKind.Relative))
        Me.Root = CType(Me.FindName("Root"),System.Windows.Controls.Canvas)
        Me.ThumbShadow = CType(Me.FindName("ThumbShadow"),System.Windows.Shapes.Rectangle)
        Me.ThumbBorder = CType(Me.FindName("ThumbBorder"),System.Windows.Controls.Border)
        Me.ThumbGrid = CType(Me.FindName("ThumbGrid"),System.Windows.Controls.Grid)
        Me.ThumbImage = CType(Me.FindName("ThumbImage"),System.Windows.Controls.Image)
        Me.ThumbnailText = CType(Me.FindName("ThumbnailText"),System.Windows.Controls.TextBlock)
    End Sub
End Class

