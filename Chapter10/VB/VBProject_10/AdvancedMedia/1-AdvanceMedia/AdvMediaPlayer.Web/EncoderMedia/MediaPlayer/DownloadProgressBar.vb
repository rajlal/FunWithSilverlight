'  <copyright file="DownloadProgressBar.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements the download progress bar class</summary>
'  <author>Microsoft Expression Encoder Team</author>
Imports System
Imports System.Windows
Imports System.Windows.Controls

' Namespace ExpressionMediaPlayer

    ''' <summary>
    ''' This class represents a progress bar for downloading a media item. Extends ProgressBar
    ''' by allowing start offset indicating DownloadProgressOffset.
    ''' </summary>
    <TemplatePart(Name := DownloadProgressBar.DeterminateRoot, Type := typeofCType()>, FrameworkElement)
    Public Class DownloadProgressBar
        Inherits ProgressBar
        ''' <summary>
        ''' String for the visual element of the progress bar.
        ''' </summary>
        private const string DeterminateRoot = "DeterminateRoot"
        ''' <summary>
        ''' Visual element representing progress bar indicator.
        ''' </summary>
        Private m_determinateRoot As FrameworkElement
        ''' <summary>
        ''' Dependancy property for Download Offset.
        ''' </summary>
        Public Property readonly() As
            Get

                return (Double)Me.GetValue(DownloadOffsetProperty)
            End Get

            Set

                Me.SetValue(DownloadOffsetProperty, value)
            End Set
        End Property
        ''' <summary>
        ''' Overridden OnApplyTemplate, sets the DeterminateRoot member, which is the visual element for the progress indicator.
        ''' </summary>
        public override Sub OnApplyTemplate()

 	        MyBase.OnApplyTemplate()
            m_determinateRoot = GetTemplateChild(DeterminateRoot) as FrameworkElement
        End Sub '   OnApplyTemplate
        ''' <summary>
        ''' Update the visual for the offset.
        ''' </summary>
        ''' <param name="offset">Amount of offset (gets bounded between Minimum and Maximum).</param>
        private Sub SetOffsetVisual(offset As Double)

            offset = Math.Max(Math.Min(offset, Maximum), Minimum)
            Double left = ((offset - Minimum) / (Maximum - Minimum)) * ActualWidth

            If (m_determinateRoot  IsNot Nothing) Then
                m_determinateRoot.Margin = New Thickness(left, m_determinateRoot.Margin.Top, m_determinateRoot.Margin.Right, m_determinateRoot.Margin.Bottom)
            End If
        End Sub '   SetOffsetVisual
        ''' <summary>
        ''' Called when "DownloadProgressOffset" is set.
        ''' </summary>
        ''' <param name="obj">DownloadProgressBar source.</param>
        ''' <param name="args">New value etc.</param>
        End Property


            Dim downloadProgressBar As DownloadProgressBar  = CType(obj,  DownloadProgressBar)


            If (downloadProgressBar  IsNot Nothing) Then

                Dim downloadProgressOffset As Double  = (Double)args.NewValue


                If (Double.IsNaN(downloadProgressOffset)  OrElse  downloadProgressOffset < 0.0  OrElse  downloadProgressOffset > 100.0) Then
                    throw New ArgumentException()
                End If

                downloadProgressBar.SetOffsetVisual((Double)args.NewValue)
            End If
        End Sub '   OnDownloadProgressOffsetPropertyChanged
    End Class   '   DownloadProgressBar
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\DownloadProgressBar.cs
