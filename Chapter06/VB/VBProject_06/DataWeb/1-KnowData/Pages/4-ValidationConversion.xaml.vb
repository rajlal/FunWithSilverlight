Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes
Imports System.Windows.Media.Imaging
Imports System.Windows.Data

' Namespace KnowData

Partial Public Class ValidationConversion
    Inherits UserControl

    Dim myData As HeterogeneousData = New HeterogeneousData()

    Public Sub New()
        InitializeComponent()
    End Sub '   New

    Private Sub MyTextBox_BindingValidationError(sender As Object, e As ValidationErrorEventArgs)

        Dim tb As TextBox = CType(sender, TextBox)

        If (e.Action = ValidationErrorEventAction.Added) Then
            If (tb.Name = "txtName") Then
                errName.Visibility = Visibility.Visible
            ElseIf (tb.Name = "txtEmail") Then
                errEmail.Visibility = Visibility.Visible
            ElseIf (tb.Name = "txtZipcode") Then
                errZipcode.Visibility = Visibility.Visible
            End If
        ElseIf (e.Action = ValidationErrorEventAction.Removed) Then
            If (tb.Name = "txtName") Then
                errName.Visibility = Visibility.Collapsed
            ElseIf (tb.Name = "txtEmail") Then
                errEmail.Visibility = Visibility.Collapsed
            ElseIf (tb.Name = "txtZipcode") Then
                errZipcode.Visibility = Visibility.Collapsed
            End If
        End If
    End Sub '   MyTextBox_BindingValidationError

    Private Sub Reset_Click(sender As Object, e As RoutedEventArgs)

        txtName.Text = "Developer"
        txtEmail.Text = "Dev@silverlightfun.com"
        txtZipcode.Text = "92126"
    End Sub '   Reset_Click

    Private Sub Select_Renamed(sender As Object, e As MouseButtonEventArgs)

        Dim t As TextBlock = CType(sender, TextBlock)

        Dim Selected As String = t.Text

        StatusBar.Text = ToolTipService.GetToolTip(t).ToString()

        If (Selected = "Validation") Then
            CanvasValidation.Visibility = Visibility.Visible
            CanvasConversion.Visibility = Visibility.Collapsed
        ElseIf (Selected = "Conversion") Then
            CanvasValidation.Visibility = Visibility.Collapsed
            CanvasConversion.Visibility = Visibility.Visible
            stackDataConverted.DataContext = myData
        End If
    End Sub '   Select_Renamed

    Private Sub layoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

        CanvasValidation.Visibility = Visibility.Visible
        CanvasConversion.Visibility = Visibility.Collapsed
        stackDataOriginal.DataContext = myData
    End Sub '   layoutRoot_Loaded
End Class   '   ValidationConversion

Public Class Account

    Private _name As String = "Developer"
    Private _email As String = "Dev@silverlightfun.com"
    Private _zipcode As String = ""

    Private Property Name() As String
        Get
            Return _name
        End Get
        Set(value As String)
            If (value.Length < 1) Then
                Throw New Exception("Name is Required")
            End If
            _name = value
        End Set
    End Property

    Public Property Email() As String
        Get
            Return _email
        End Get
        Set(value As String)
            If (Not (value.Contains("@") AndAlso value.Contains("."))) Then
                Throw New Exception("Invalid Email")
            End If
            _email = value
        End Set
    End Property

    Public Property Zipcode() As String
        Get
            Return _zipcode
        End Get
        Set(value As String)
            If (Convert.ToInt32(value) > 0) Then
                If ((Integer.Parse(value) < 10000 OrElse Integer.Parse(value) > 99999)) Then
                    Throw New Exception("5 Digit zipcode required")
                End If

                _zipcode = value
            End If
        End Set
    End Property
End Class   '   Account

Public Class HeterogeneousData

    Private _dataDateTime As DateTime = DateTime.Now
    Private _dataPhone As String = "8581112345"
    Private _dataRating As Double = 4.7867

    Public Property DataDateTime As DateTime

        Get
            Return _dataDateTime
        End Get
        Set(value As DateTime)
            _dataDateTime = value
        End Set
    End Property

    Public Property DataPhone() As String
        Get
            Return _dataPhone
        End Get
        Set(value As String)
            _dataPhone = value
        End Set
    End Property

    Public Property DataRating() As Double
        Get
            Return _dataRating
        End Get
        Set(value As Double)
            _dataRating = value
        End Set
    End Property
End Class   '   HeterogeneousData

Public Class PhoneToFormatedPhoneConverter
    Implements IValueConverter
    '  Define the Convert method to change a DateTime object to a month string.
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert

        '  value is the data from the source object.
        Dim inputPhone As String = CType(value, String)
        Dim outputPhone As String = "(" + inputPhone.Substring(0, 3) + ")" + inputPhone.Substring(3, 3) + "-" + inputPhone.Substring(6)
        '  Return the value to pass to the target.
        Return outputPhone
    End Function  '   Convert

    '  ConvertBack is not implemented for a OneWay binding.
    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack

        Throw New NotImplementedException()
    End Function  '   ConvertBack
End Class   '   PhoneToFormatedPhoneConverter

Public Class DateTimeToDateConverter
    Implements IValueConverter
    '  Define the Convert method to change a DateTime object to a month string.
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert

        '  value is the data from the source object.
        Dim inputdate As DateTime = CType(value, DateTime)

        Dim outputDate As String = inputdate.ToLongDateString()

        '  Return the value to pass to the target.
        Return outputDate
    End Function  '   Convert

    '  ConvertBack is not implemented for a OneWay binding.
    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack

        Throw New NotImplementedException()
    End Function  '   ConvertBack
End Class   '   DateTimeToDateConverter

Public Class RatingFormatConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert

        If (parameter IsNot Nothing) Then

            Dim formatterString As String = parameter.ToString()

            If (Not String.IsNullOrEmpty(formatterString)) Then
                Return String.Format(culture, formatterString, value)
            End If
        End If

        Return value.ToString()
    End Function  '   Convert

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack

        Return value
    End Function  '   ConvertBack
End Class   '   RatingFormatConverter
' End Namespace   '   KnowData
' ..\Project_06\DataWeb\1-KnowData\Pages\4-ValidationConversion.xaml.cs
