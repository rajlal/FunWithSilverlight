﻿#ExternalChecksum("C:\Users\Main OutDoor\Documents\Visual Studio 2010\Projects\S\Silverlight\VBProject_05\ExtendBrowser\1-HelloBrowser\Pages\3c-PageHistorySubCategory.xaml","{406ea660-64cf-4c82-b6f0-42d48172a799}","5AC9682264D7526207EA7AA0A613D9BD")
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
Partial Public Class PageHistorySubCategory
    Inherits System.Windows.Controls.UserControl
    
    Friend WithEvents BorderDynamicItem As System.Windows.Controls.Border
    
    Friend WithEvents DynamicItemContainer As System.Windows.Controls.Canvas
    
    Friend WithEvents txtCategory As System.Windows.Controls.HyperlinkButton
    
    Friend WithEvents txtSubCategory As System.Windows.Controls.TextBlock
    
    Friend WithEvents StatusGrid As System.Windows.Controls.Grid
    
    Friend WithEvents StatusBar As System.Windows.Controls.TextBlock
    
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
        System.Windows.Application.LoadComponent(Me, New System.Uri("/HelloBrowser;component/Pages/3c-PageHistorySubCategory.xaml", System.UriKind.Relative))
        Me.BorderDynamicItem = CType(Me.FindName("BorderDynamicItem"),System.Windows.Controls.Border)
        Me.DynamicItemContainer = CType(Me.FindName("DynamicItemContainer"),System.Windows.Controls.Canvas)
        Me.txtCategory = CType(Me.FindName("txtCategory"),System.Windows.Controls.HyperlinkButton)
        Me.txtSubCategory = CType(Me.FindName("txtSubCategory"),System.Windows.Controls.TextBlock)
        Me.StatusGrid = CType(Me.FindName("StatusGrid"),System.Windows.Controls.Grid)
        Me.StatusBar = CType(Me.FindName("StatusBar"),System.Windows.Controls.TextBlock)
    End Sub
End Class

