'  <copyright file="IAccessible.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements the download progress bar class</summary>
'  <author>Microsoft Expression Encoder Team</author>
' Namespace ExpressionMediaPlayer

    ''' <summary>
    ''' implement this if you want to provide screen reader info to your listbox items
    ''' </summary>
    public interface IAccessible
    {

        Dim AccessibilityText As String  { get; }

    }
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\IAccessible.cs
