'  <copyright file="SmoothSlider.cs" company="Microsoft">
Imports System 
Imports System.Windows 
Imports System.Windows.Input 
Imports System.Windows.Media.Animation 

' Namespace ExpressionMediaPlayer
    
public class SmoothSlider 
    Inherits SensitiveSlider

        ''' <summary>
        ''' animation storyboard for smooth slider
        ''' </summary>
        private m_Animation As Storyboard 
        private m_DoubleAnimation As DoubleAnimation 
        ''' <summary>
        ''' smooth animating slider
        ''' </summary>
        public Sub New()

            m_Animation = new Storyboard()
            m_DoubleAnimation = new DoubleAnimation()
            m_DoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3))
            Storyboard.SetTarget(m_DoubleAnimation, Me)
            Storyboard.SetTargetProperty(m_DoubleAnimation, new PropertyPath("(SmoothSlider.Value)"))
            m_Animation.Children.Add(m_DoubleAnimation)
            CircleEase circleEasing = new CircleEase()
            circleEasing.EasingMode = EasingMode.EaseInOut
            m_DoubleAnimation.EasingFunction = circleEasing
        End Sub
        ''' <summary>
        ''' value of slider
        ''' </summary>
        public Property Value() As Double
            get

                return MyBase.Value
            end Get
            Set

                m_DoubleAnimation.From = MyBase.Value
                m_DoubleAnimation.To = value
                m_Animation.Begin()
            End Set
        End Property
        ''' <summary>
        ''' overriden to prevent updating the mediaelement position while the slider is animating.
        ''' </summary>
        public overrides Property IsDragging() As Boolean
            Get

                Dim bReturn As Boolean = True

                If (base.IsDragging = False) Then
                    bReturn = m_Animation.GetCurrentState() = ClockState.Active
                End If

                Return bReturn
            End Get
        End Property
        ''' <summary>
        ''' update value on mouse click
        ''' </summary>
        protected overrides Sub OnMouseClick(sender As Object, args As MouseButtonEventArgs)

            Value = CalcValue(args)
        End Sub '   OnMouseClick
    End Class   '   SmoothSlider
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\ExpressionPlayer\SmoothSlider.cs
