'  <copyright file="ScriptableObservableCollection.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements a Scriptable version of the ObservableCollection class</summary>
'  <author>Microsoft Expression Encoder Team</author>
Imports System.Windows
Imports System.Windows.Browser
Imports System.Windows.Media

' Namespace ExpressionMediaPlayer

    <ScriptableType>
    Public Class ScriptableTimelineMarkerRoutedEventArgs
        Inherits RoutedEventArgs
        ''' <summary>
        ''' event args for when timeline marker reached
        ''' </summary>
        ''' <param name="marker"></param>
        Public Sub New(marker As ScriptableTimelineMarker)
            Marker = marker
        End Sub '   New


        Public Property Marker As ScriptableTimelineMarker
    End Class   '   ScriptableTimelineMarkerRoutedEventArgs


    public delegate Sub ScriptableTimelineMarkerRoutedEventHandler(sender As Object, e As ScriptableTimelineMarkerRoutedEventArgs)
    End Sub '   ScriptableTimelineMarkerRoutedEventHandler


    <ScriptableType>
    Public Class ScriptableTimelineMarker
        ''' <summary>
        ''' text of marker
        ''' </summary>
        Public Property Text As String
        ''' <summary>
        ''' time of marker (seconds)
        ''' </summary>
        Public Property Time As Double
        ''' <summary>
        ''' type of marker
        ''' </summary>
        Public Property Type As String
        ''' <summary>
        ''' create new ScriptableTimelineMarker from TimelineMarker
        ''' </summary>
        ''' <param name="marker"></param>
        Public Sub New(marker As TimelineMarker)
            Text = marker.Text
            Time = marker.Time.TotalSeconds

            Dim = As Type  marker.Type

        End Sub '   New
    End Class   '   ScriptableTimelineMarker
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\ScriptableTimelineMarker.cs
