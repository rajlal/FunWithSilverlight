﻿#ExternalChecksum("C:\Users\Main OutDoor\Documents\Visual Studio 2010\Projects\S\Silverlight\VBProject_05\ExtendBrowser\1-HelloBrowser\Pages\4-PageCookies.xaml","{406ea660-64cf-4c82-b6f0-42d48172a799}","73D73F13789C120F5EE9686508FC5E76")
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
Partial Public Class PageCookies
    Inherits System.Windows.Controls.UserControl
    
    Friend WithEvents LayoutRoot As System.Windows.Controls.Grid
    
    Friend WithEvents LoginBox As System.Windows.Controls.Border
    
    Friend WithEvents StackLayout As System.Windows.Controls.StackPanel
    
    Friend WithEvents partLogin As System.Windows.Controls.Border
    
    Friend WithEvents Login As System.Windows.Controls.StackPanel
    
    Friend WithEvents txtName As System.Windows.Controls.TextBox
    
    Friend WithEvents txtEmail As System.Windows.Controls.TextBox
    
    Friend WithEvents txtWeb As System.Windows.Controls.TextBox
    
    Friend WithEvents chkSaveCookie As System.Windows.Controls.CheckBox
    
    Friend WithEvents partStatus As System.Windows.Controls.Border
    
    Friend WithEvents txtStatus As System.Windows.Controls.TextBlock
    
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
        System.Windows.Application.LoadComponent(Me, New System.Uri("/HelloBrowser;component/Pages/4-PageCookies.xaml", System.UriKind.Relative))
        Me.LayoutRoot = CType(Me.FindName("LayoutRoot"),System.Windows.Controls.Grid)
        Me.LoginBox = CType(Me.FindName("LoginBox"),System.Windows.Controls.Border)
        Me.StackLayout = CType(Me.FindName("StackLayout"),System.Windows.Controls.StackPanel)
        Me.partLogin = CType(Me.FindName("partLogin"),System.Windows.Controls.Border)
        Me.Login = CType(Me.FindName("Login"),System.Windows.Controls.StackPanel)
        Me.txtName = CType(Me.FindName("txtName"),System.Windows.Controls.TextBox)
        Me.txtEmail = CType(Me.FindName("txtEmail"),System.Windows.Controls.TextBox)
        Me.txtWeb = CType(Me.FindName("txtWeb"),System.Windows.Controls.TextBox)
        Me.chkSaveCookie = CType(Me.FindName("chkSaveCookie"),System.Windows.Controls.CheckBox)
        Me.partStatus = CType(Me.FindName("partStatus"),System.Windows.Controls.Border)
        Me.txtStatus = CType(Me.FindName("txtStatus"),System.Windows.Controls.TextBlock)
    End Sub
End Class

