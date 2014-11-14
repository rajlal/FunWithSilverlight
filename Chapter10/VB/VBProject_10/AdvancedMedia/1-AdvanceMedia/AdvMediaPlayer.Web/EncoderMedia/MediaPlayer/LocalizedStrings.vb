'  <copyright file="LocalizedStrings.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Exposes localized resources in a public class so that they may be referenced in XAML</summary>
'  <author>Microsoft Expression Encoder Team</author>
Imports System

' Namespace ExpressionMediaPlayer

    ''' <summary>
    ''' This class Exposes localized resources in a public class so that they may be referenced in XAML
    ''' </summary>
    Public Class LocalizedStrings
        ''' <summary>
        ''' Initializes a new instance of the LocalizedStrings class.
        ''' </summary>
        Public Sub New()
        End Sub '   New


        ''' <summary>
        ''' private reference to the localized resources
        ''' </summary>
        Private Resources As  localizedStrings = New Resources()

        ''' <summary>
        ''' public property that provides XAML access to the localized strings
        ''' </summary>
        public Resources Strings { get { return localizedStrings; } }
    End Class   '   LocalizedStrings
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\LocalizedStrings.cs
