﻿#ExternalChecksum("C:\Users\Main OutDoor\Documents\Visual Studio 2010\Projects\S\Silverlight\VBProject_05\ExtendBrowser\4-HTMLBridge\Pages\1-PageAccessHTML.xaml","{406ea660-64cf-4c82-b6f0-42d48172a799}","9697C0A1F778CA6BAE3816445A1C60FD")
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
Partial Public Class PageAccessHTML
    Inherits System.Windows.Controls.UserControl
    
    Friend WithEvents LayoutRoot As System.Windows.Controls.Grid
    
    Friend WithEvents txtTitle As System.Windows.Controls.TextBlock
    
    Friend WithEvents txtHTMLDocument As System.Windows.Controls.TextBlock
    
    Friend WithEvents txtHTMLBody As System.Windows.Controls.TextBlock
    
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
        System.Windows.Application.LoadComponent(Me, New System.Uri("/HTMLBridge;component/Pages/1-PageAccessHTML.xaml", System.UriKind.Relative))
        Me.LayoutRoot = CType(Me.FindName("LayoutRoot"),System.Windows.Controls.Grid)
        Me.txtTitle = CType(Me.FindName("txtTitle"),System.Windows.Controls.TextBlock)
        Me.txtHTMLDocument = CType(Me.FindName("txtHTMLDocument"),System.Windows.Controls.TextBlock)
        Me.txtHTMLBody = CType(Me.FindName("txtHTMLBody"),System.Windows.Controls.TextBlock)
    End Sub
End Class

