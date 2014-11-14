Imports System
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes
Imports System.ComponentModel

' Namespace ExpressionMediaPlayer

    ''' <summary>
    ''' This class allows for properties of type long to be exposed by a Silverlight control.
    ''' </summary>
    Public Class LongConverter
        Inherits TypeConverter
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {

            If (sourceType = typeofCType(), String) Then
                return true
            else
                return MyBase.CanConvertFrom(context, sourceType)
            End If
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {

            If (value Is Nothing) Then
                return 0
            ElseIf (value is string) Then
                long temp = System.Int64.Parse(value as string)
                return temp
            End If

            else
            {
                return MyBase.ConvertFrom(context, culture, value)
            }
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            '  Not incorporated in Silverlight 3
            return MyBase.CanConvertTo(context, destinationType)
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            '  Not incorporated in Silverlight 3
            return MyBase.ConvertTo(context, culture, value, destinationType)
        }
    End Class   '   LongConverter
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\LongConverter.cs
