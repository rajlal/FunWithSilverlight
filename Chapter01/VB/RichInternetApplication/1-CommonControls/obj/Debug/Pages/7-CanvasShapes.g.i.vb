﻿#ExternalChecksum("D:\Writing\Silverlight 4\VB\Silverlight VB\VBProject_01\VBProject_01\VBProject_01\RichInternetApplication\1-CommonControls\Pages\7-CanvasShapes.xaml","{406ea660-64cf-4c82-b6f0-42d48172a799}","A06ADDBD1815157919AA5FECC089AEEE")
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
Partial Public Class PageCanvasShapes
    Inherits System.Windows.Controls.UserControl
    
    Friend WithEvents cnvContainer As System.Windows.Controls.Canvas
    
    Friend WithEvents Rectangle As System.Windows.Shapes.Rectangle
    
    Friend WithEvents Ellipse As System.Windows.Shapes.Ellipse
    
    Friend WithEvents lineHorizontal As System.Windows.Shapes.Line
    
    Friend WithEvents lineVertical As System.Windows.Shapes.Line
    
    Friend WithEvents Triangle As System.Windows.Shapes.Path
    
    Friend WithEvents RightIcon As System.Windows.Shapes.Path
    
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
        System.Windows.Application.LoadComponent(Me, New System.Uri("/CommonControls;component/Pages/7-CanvasShapes.xaml", System.UriKind.Relative))
        Me.cnvContainer = CType(Me.FindName("cnvContainer"),System.Windows.Controls.Canvas)
        Me.Rectangle = CType(Me.FindName("Rectangle"),System.Windows.Shapes.Rectangle)
        Me.Ellipse = CType(Me.FindName("Ellipse"),System.Windows.Shapes.Ellipse)
        Me.lineHorizontal = CType(Me.FindName("lineHorizontal"),System.Windows.Shapes.Line)
        Me.lineVertical = CType(Me.FindName("lineVertical"),System.Windows.Shapes.Line)
        Me.Triangle = CType(Me.FindName("Triangle"),System.Windows.Shapes.Path)
        Me.RightIcon = CType(Me.FindName("RightIcon"),System.Windows.Shapes.Path)
    End Sub
End Class

