'  <copyright file="TimeCode.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements the MediaPlayer class</summary>
'  <author>Microsoft Expression Encoder Team</author>
Imports System
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Text

' Namespace ExpressionMediaPlayer

    ''' <summary>
    ''' Represents a SMPTE 12M standard time code and provides conversion operations to various SMPTE time code formats and rates.
    ''' </summary>
    ''' <remarks>
    ''' Framerates supported by the TimeCode class include, 23.98 IVTC Film Sync, 24fps Film Sync, 25fps PAL, 29.97 drop frame,
    ''' 29.97 Non drop, and 30fps.
    ''' </remarks>
    public partial struct TimeCode : IComparable, IComparable<TimeCode>, IEquatable<TimeCode>
        #region Private Fields

        ''' <summary>
        ''' Regular expression string used for parsing out the timecode.
        ''' </summary>
        private const string smpteRegExString = "(?<Hours>\\d{2}):(?<Minutes>\\d{2}):(?<Seconds>\\d{2})(?::|;)(?<Frames>\\d{2})"

        ''' <summary>
        ''' Regular expression object used for validating timecode.
        ''' </summary>
        Private readonly As  Regex validateTimecode = New Regex(smpteRegExString, RegexOptions.CultureInvariant)

        ''' <summary>
        ''' The private Timespan used to track absolute time for this instance.
        ''' </summary>
        private readonly double absoluteTime

        ''' <summary>
        ''' The frame rate for this instance.
        ''' </summary>
        private SmpteFrameRate frameRate

        #End Region

        #region Constructors

        ''' <summary>
        '''  Initializes a new instance of the TimeCode struct to a specified number of hours, minutes, and seconds.
        ''' </summary>
        ''' <param name="hours">Number of hours.</param>
        ''' <param name="minutes">Number of minutes.</param>
        ''' <param name="seconds">Number of seconds.</param>
        ''' <param name="frames">Number of frames.</param>
        ''' <param name="rate">The SMPTE frame rate.</param>
        ''' <exception cref="System.FormatException">
        ''' The parameters specify a TimeCode value less than TimeCode.MinValue.
        ''' or greater than TimeCode.MaxValue, or the values of time code components are not valid for the SMPTE framerate.
        ''' </exception>
        ''' <code source="..\Documentation\SdkDocSamples\TimecodeSamples.cs" region="CreateTimeCode_2398FromIntegers" lang="CSharp" title="Create TimeCode from Integers"/>
        public TimeCode(int hours, int minutes, int seconds, int frames, SmpteFrameRate rate)
        {

            Dim timeCode As String  = String.Format(CultureInfo.InvariantCulture, "{0:D2}:{1:D2}:{2:D2}:{3:D2}", hours, minutes, seconds, frames)

            Me.frameRate = rate
            Me.absoluteTime = Smpte12mToAbsoluteTime(timeCode, Me.frameRate)
        }

        ''' <summary>
        ''' Initializes a new instance of the TimeCode struct using an Int32 in hex format containing the time code value compatible with the Windows Media Format SDK.
        ''' Time code is stored so that the hexadecimal value is read as if it were a decimal value. That is, the time code value 0x01133512 does not represent decimal 18035986, rather it specifies 1 hour, 13 minutes, 35 seconds, and 12 frames.
        ''' </summary>
        ''' <param name="windowsMediaTimeCode">The integer value of the timecode.</param>
        ''' <param name="rate">The SMPTE frame rate.</param>
        public TimeCode(int windowsMediaTimeCode, SmpteFrameRate rate)
        {
            '  Timecode is provided back formatted as hexadecimal bytes read in single bytes from left to right.
            byte() timeCodeBytes = BitConverter.GetBytes(windowsMediaTimeCode)

            Dim timeCode As String  = String.Format(CultureInfo.InvariantCulture,"{3:x2}:{2:x2}:{1:x2}:{0:x2}", timeCodeBytes(0), timeCodeBytes(1), timeCodeBytes(2), timeCodeBytes(3))

            Me.frameRate = rate
            Me.absoluteTime = Smpte12mToAbsoluteTime(timeCode, Me.frameRate)
        }

        ''' <summary>
        ''' Initializes a new instance of the TimeCode struct using a time code string that contains the framerate at the end of the string.
        ''' </summary>
        ''' <remarks>
        ''' Pass in a timecode in the format "timecode@framrate".
        ''' Supported rates include @23.98, @24, @25, @29.97, @30
        ''' </remarks>
        ''' <example>
        ''' "00:01:00:00@29.97" is equivalent to 29.97 non drop frame.
        ''' "00:01:00;00@29.97" is equivalent to 29.97 drop frame.
        ''' </example>
        ''' <param name="timeCodeAndRate">The SMPTE 12m time code string.</param>
        public TimeCode(string timeCodeAndRate)
        {
            string() timeAndRate = timeCodeAndRate.Split('@')

            Dim time As String  = string.Empty
            Dim rate As String  = string.Empty


            If (timeAndRate.Length = 1) Then
                time = timeAndRate(0)
                rate = "29.97"
            ElseIf (timeAndRate.Length = 2) Then
                time = timeAndRate(0)
                rate = timeAndRate(1)
            End If


            Me.frameRate = SmpteFrameRate.Smpte2997NonDrop


            If (rate = "29.97"  AndAlso  time.IndexOf(';') > -1) Then
                Me.frameRate = SmpteFrameRate.Smpte2997Drop
            "29.97"  AndAlso  time.IndexOf(';') = -1) Then
            ElseIf (rate = -1) Then
                Me.frameRate = SmpteFrameRate.Smpte2997NonDrop
            End If

            else if (rate = "25")
            {
                Me.frameRate = SmpteFrameRate.Smpte25
            }
            else if (rate = "23.98")
            {
                Me.frameRate = SmpteFrameRate.Smpte2398
            }
            else if (rate = "24")
            {
                Me.frameRate = SmpteFrameRate.Smpte24
            }
            else if (rate = "30")
            {
                Me.frameRate = SmpteFrameRate.Smpte30
            }

            Me.absoluteTime = Smpte12mToAbsoluteTime(time, Me.frameRate)
        }

        ''' <summary>
        ''' Initializes a new instance of the TimeCode struct using a time code string and a SMPTE framerate.
        ''' </summary>
        ''' <param name="timeCode">The SMPTE 12m time code string.</param>
        ''' <param name="rate">The SMPTE framerate used for this instance of TimeCode.</param>
        public TimeCode(string timeCode, SmpteFrameRate rate)
        {
            Me.frameRate = rate
            Me.absoluteTime = Smpte12mToAbsoluteTime(timeCode, Me.frameRate)
        }

        ''' <summary>
        ''' Initializes a new instance of the TimeCode struct using an absolute time value, and the SMPTE framerate.
        ''' </summary>
        ''' <param name="absoluteTime">The double that represents the absolute time value.</param>
        ''' <param name="rate">The SMPTE framerate that this instance should use.</param>
        public TimeCode(double absoluteTime, SmpteFrameRate rate)
        {
            Me.absoluteTime = absoluteTime
            Me.frameRate = rate
        }

        ''' <summary>
        ''' Initializes a new instance of the TimeCode struct a long value that represents a value of a 27 Mhz clock.
        ''' </summary>
        ''' <param name="ticks27Mhz">The long value in 27 Mhz clock ticks.</param>
        ''' <param name="rate">The SMPTE frame rate to use for this instance.</param>
        public TimeCode(long ticks27Mhz, SmpteFrameRate rate)
        {
            Me.absoluteTime = Ticks27MhzToAbsoluteTime(ticks27Mhz)
            Me.frameRate = rate
        }

        #End Region

        #region Public Static Properties

        ''' <summary>
        '''  Gets the number of ticks in 1 day.
        '''  This field is constant.
        ''' </summary>
        Public ReadOnly Property long() As
            Get
                return 864000000000
            End Get
        End Property

        ''' <summary>
        '''  Gets the number of absolute time ticks in 1 day.
        '''  This field is constant.
        ''' </summary>
        Public ReadOnly Property double() As
            Get
                return 86400
            End Get
        End Property

        ''' <summary>
        '''  Gets the number of ticks in 1 hour. This field is constant.
        ''' </summary>
        Public ReadOnly Property long() As
            Get
                return 36000000000
            End Get
        End Property

        ''' <summary>
        '''  Gets the number of absolute time ticks in 1 hour. This field is constant.
        ''' </summary>
        Public ReadOnly Property double() As
            Get
                return 3600
            End Get
        End Property

        ''' <summary>
        ''' Gets the number of ticks in 1 millisecond. This field is constant.
        ''' </summary>
        Public ReadOnly Property long() As
            Get
                return 10000
            End Get
        End Property

        ''' <summary>
        ''' Gets the number of ticks in 1 millisecond. This field is constant.
        ''' </summary>
        Public ReadOnly Property double() As
            Get
                return 0.0010000D
            End Get
        End Property

        ''' <summary>
        ''' Gets the number of ticks in 1 minute. This field is constant.
        ''' </summary>
        Public ReadOnly Property long() As
            Get
                return 600000000
            End Get
        End Property

        ''' <summary>
        ''' Gets the number of absolute time ticks in 1 minute. This field is constant.
        ''' </summary>
        Public ReadOnly Property double() As
            Get
                return 60
            End Get
        End Property

        ''' <summary>
        ''' Gets the number of ticks in 1 second.
        ''' </summary>
        Public ReadOnly Property long() As
            Get
                return 10000000
            End Get
        End Property

        ''' <summary>
        ''' Gets the number of ticks in 1 second.
        ''' </summary>
        Public ReadOnly Property double() As
            Get
                return 1.0000000D
            End Get
        End Property

        ''' <summary>
        '''  Gets the maximum TimeCode value. The Max value for Timecode. This field is read-only.
        ''' </summary>
        Public ReadOnly Property MaxValue() As Double
            Get


                Select Case (Me.frameRate)

                    case SmpteFrameRate.Smpte2398:
                        return 86486.3582916667
                    case SmpteFrameRate.Smpte24:
                        return 86399.9583333333
                    case SmpteFrameRate.Smpte25:
                        return 86399.9600000000
                    case SmpteFrameRate.Smpte2997Drop:
                        return 86399.8802333333
                    case SmpteFrameRate.Smpte2997NonDrop:
                        return 86486.3666333333
                    case SmpteFrameRate.Smpte30:
                        return 86399.9666666667
                    default:
                        return 86424
                End Select '    Me.frameRate
            End Get
        End Property


        ''' <summary>
        ''' Gets the minimum TimeCode value. This field is read-only.
        ''' </summary>
        Public ReadOnly Property double() As
            Get
                return 0
            End Get
        End Property

        #End Region

        #region Public Properties

        ''' <summary>
        ''' Gets the absolute time in seconds of the current TimeCode object.
        ''' </summary>
        ''' <returns>
        '''  A double that is the absolute time in seconds duration of the current TimeCode object.
        ''' </returns>
        Public ReadOnly Property Duration() As Double
            Get
                return Me.absoluteTime
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the current SMPTE framerate for this TimeCode instance.
        ''' </summary>
        public SmpteFrameRate FrameRate
        {
            get { return Me.frameRate; }
            set { Me.frameRate = value; }
        }

        ''' <summary>
        '''  Gets the number of whole hours represented by the current TimeCode
        '''  structure.
        ''' </summary>
        ''' <returns>
        '''  The hour component of the current TimeCode structure. The return value
        '''     ranges from 0 through 23.
        ''' </returns>
        Public ReadOnly Property HoursSegment() As Integer
            Get

                Dim timeCode As String  = AbsoluteTimeToSmpte12M(Me.absoluteTime, Me.frameRate)
                Dim hours As String  = timeCode.Substring(0, 2)

                return Int32.Parse(hours, CultureInfo.InvariantCulture)
            End Get
        End Property

        ''' <summary>
        ''' Gets the number of whole minutes represented by the current TimeCode structure.
        ''' </summary>
        ''' <returns>
        ''' The minute component of the current TimeCode structure. The return
        ''' value ranges from 0 through 59.
        ''' </returns>
        Public ReadOnly Property MinutesSegment() As Integer
            Get

                Dim timeCode As String  = AbsoluteTimeToSmpte12M(Me.absoluteTime, Me.frameRate)
                Dim minutes As String  = timeCode.Substring(3, 2)

                return Int32.Parse(minutes, CultureInfo.InvariantCulture)
            End Get
        End Property

        ''' <summary>
        ''' Gets the number of whole seconds represented by the current TimeCode structure.
        ''' </summary>
        ''' <returns>
        '''  The second component of the current TimeCode structure. The return
        '''    value ranges from 0 through 59.
        ''' </returns>
        Public ReadOnly Property SecondsSegment() As Integer
            Get

                Dim timeCode As String  = AbsoluteTimeToSmpte12M(Me.absoluteTime, Me.frameRate)
                Dim seconds As String  = timeCode.Substring(6, 2)

                return Int32.Parse(seconds, CultureInfo.InvariantCulture)
            End Get
        End Property

        ''' <summary>
        ''' Gets the number of whole frames represented by the current TimeCode
        '''     structure.
        ''' </summary>
        ''' <returns>
        ''' The frame component of the current TimeCode structure. The return
        '''     value depends on the framerate selected for this instance. All frame counts start at zero.
        ''' </returns>
        Public ReadOnly Property FramesSegment() As Integer
            Get

                Dim timeCode As String  = AbsoluteTimeToSmpte12M(Me.absoluteTime, Me.frameRate)
                Dim frames As String  = timeCode.Substring(9, 2)

                return Int32.Parse(frames, CultureInfo.InvariantCulture)
            End Get
        End Property

        ''' <summary>
        ''' Gets the value of the current TimeCode structure expressed in whole
        '''     and fractional hours.
        ''' </summary>
        ''' <returns>
        '''  The total number of hours represented by this instance.
        ''' </returns>
        Public ReadOnly Property TotalHours() As Double
            Get

                long framecount = AbsoluteTimeToFrames(Me.absoluteTime, Me.frameRate)
                return (framecount / 108000D) Mod 24
            End Get
        End Property

        ''' <summary>
        ''' Gets the value of the current TimeCode structure expressed in whole
        ''' and fractional minutes.
        ''' </summary>
        ''' <returns>
        '''  The total number of minutes represented by this instance.
        ''' </returns>
        Public ReadOnly Property TotalMinutes() As Double
            Get

                long framecount = AbsoluteTimeToFrames(Me.absoluteTime, Me.frameRate)

                Dim minutes As Double


                Select Case (Me.frameRate)

                    case SmpteFrameRate.Smpte2398:
                    case SmpteFrameRate.Smpte24:
                        minutes = framecount / 1400D

                    case SmpteFrameRate.Smpte25:
                        minutes = framecount / 1500D

                    case SmpteFrameRate.Smpte2997Drop:
                    case SmpteFrameRate.Smpte2997NonDrop:
                    case SmpteFrameRate.Smpte30:
                        minutes = framecount / 1800D

                    default:
                        minutes = framecount / 1800D
                End Select '    Me.frameRate


                return minutes
            End Get
        End Property

        ''' <summary>
        ''' Gets the value of the current TimeCode structure expressed in whole
        ''' and fractional seconds.
        ''' </summary>
        ''' <returns>
        ''' The total number of seconds represented by this instance.
        ''' </returns>
        Public ReadOnly Property TotalSeconds() As Double
            Get

                return Me.absoluteTime
            End Get
        End Property

        ''' <summary>
        ''' Gets the value of the current TimeCode structure expressed in frames.
        ''' </summary>
        ''' <returns>
        '''  The total number of frames represented by this instance.
        ''' </returns>
        public long TotalFrames
        {
            get
            {
                return AbsoluteTimeToFrames(Me.absoluteTime, Me.frameRate)
            }
        }

        #End Region

        #region Operator Overloads

        ''' <summary>
        ''' Subtracts a specified TimeCode from another specified TimeCode.
        ''' </summary>
        ''' <param name="time1">The first TimeCode.</param>
        ''' <param name="time2">The second TimeCode.</param>
        ''' <returns>A TimeCode whose value is the result of the value of time1 minus the value of time2.
        ''' </returns>
        ''' <exception cref="System.OverflowException">The return value is less than TimeCode.MinValue or greater than TimeCode.MaxValue.
        ''' </exception>
        Public ReadOnly Property TimeCode() As

        ''' <summary>
        ''' Adds two specified TimeCode instances.
        ''' </summary>
        ''' <param name="time1">The first TimeCode.</param>
        ''' <param name="time2">The second TimeCode.</param>
        ''' <returns>A TimeCode whose value is the sum of the values of time1 and time2.</returns>
        ''' <exception cref="System.OverflowException">
        ''' The resulting TimeCode is less than TimeCode.MinValue or greater than TimeCode.MaxValue.
        ''' </exception>
        Public ReadOnly Property TimeCode() As

        ''' <summary>
        '''  Indicates whether a specified TimeCode is less than another
        '''  specified TimeCode.
        ''' </summary>
        ''' <param name="time1">The first TimeCode.</param>
        ''' <param name="time2">The second TimeCode.</param>
        ''' <returns> True if the value of time1 is less than the value of time2; otherwise, false.</returns>
        Public ReadOnly Property bool() As

        ''' <summary>
        ''' Indicates whether a specified TimeCode is greater than another specified
        '''     TimeCode.
        ''' </summary>
        ''' <param name="time1">The first TimeCode.</param>
        ''' <param name="time2">The second TimeCode.</param>
        ''' <returns>true if the value of time1 is greater than the value of time2; otherwise, false.
        ''' </returns>
        Public ReadOnly Property bool() As

        ''' <summary>
        '''  Indicates whether two TimeCode instances are equal.
        ''' </summary>
        ''' <param name="time1">The first TimeCode.</param>
        ''' <param name="time2">The second TimeCode.</param>
        ''' <returns>true if the values of time1 and time2 are equal; otherwise, false.</returns>
        Public ReadOnly Property bool() As

        ''' <summary>
        ''' Indicates whether two TimeCode instances are not equal.
        ''' </summary>
        ''' <param name="time1">The first TimeCode.</param>
        ''' <param name="time2">The second TimeCode.</param>
        ''' <returns>true if the values of time1 and time2 are not equal; otherwise, false.</returns>
        Public ReadOnly Property bool() As

        ''' <summary>
        '''  Indicates whether a specified TimeCode is less than or equal to another
        '''  specified TimeCode.
        ''' </summary>
        ''' <param name="time1">The first TimeCode.</param>
        ''' <param name="time2">The second TimeCode.</param>
        ''' <returns>True if the value of time1 is less than or equal to the value of time2; otherwise, false.</returns>
        Public ReadOnly Property bool() As

        ''' <summary>
        ''' Indicates whether a specified TimeCode is greater than or equal to
        '''     another specified TimeCode.
        ''' </summary>
        ''' <param name="time1">The first TimeCode.</param>
        ''' <param name="time2">The second TimeCode.</param>
        ''' <returns>True if the value of time1 is greater than or equal to the value of time2; otherwise, false.</returns>
        Public ReadOnly Property bool() As

        ''' <summary>
        ''' Compares two TimeCode values and returns an integer that indicates their relationship.
        ''' </summary>
        ''' <param name="time1">The first TimeCode.</param>
        ''' <param name="time2">The second TimeCode.</param>
        ''' <returns>
        ''' Value Condition -1 time1 is less than time2, 0 time1 is equal to time2, 1 time1 is greater than time2.
        ''' </returns>
        Public ReadOnly Property int() As

        ''' <summary>
        '''  Returns a value indicating whether two specified instances of TimeCode
        '''  are equal.
        ''' </summary>
        ''' <param name="time1">The first TimeCode.</param>
        ''' <param name="time2">The second TimeCode.</param>
        ''' <returns>true if the values of time1 and time2 are equal; otherwise, false.</returns>
        Public ReadOnly Property bool() As

        #End Region

        #region Public Static Methods

        ''' <summary>
        ''' Returns a SMPTE 12M formatted time code string from a 27Mhz ticks value.
        ''' </summary>
        ''' <param name="ticks27Mhz">27Mhz ticks value.</param>
        ''' <param name="rate">The SMPTE time code framerated desired.</param>
        ''' <returns>A SMPTE 12M formatted time code string.</returns>
        Public ReadOnly Property string() As

        ''' <summary>
        '''  Returns a TimeCode that represents a specified number of hours, where
        '''  the specification is accurate to the nearest millisecond.
        ''' </summary>
        ''' <param name="hours">A number of hours accurate to the nearest millisecond.</param>
        ''' <param name="rate">The desired framerate for this instance.</param>
        ''' <returns> A TimeCode that represents value.</returns>
        ''' <exception cref="System.OverflowException">
        ''' value is less than TimeCode.MinValue or greater than TimeCode.MaxValue.
        ''' -or-value is System.Double.PositiveInfinity.-or-value is System.Double.NegativeInfinity.
        ''' </exception>
        ''' <exception cref="System.FormatException">
        ''' value is equal to System.Double.NaN.
        ''' </exception>
        Public ReadOnly Property TimeCode() As

        ''' <summary>
        '''   Returns a TimeCode that represents a specified number of minutes,
        '''   where the specification is accurate to the nearest millisecond.
        ''' </summary>
        ''' <param name="minutes">A number of minutes, accurate to the nearest millisecond.</param>
        ''' <param name="rate">The <see cref="SmpteFrameRate"/> to use for the calculation.</param>
        ''' <returns>A TimeCode that represents value.</returns>
        ''' <exception cref="System.OverflowException">
        ''' value is less than TimeCode.MinValue or greater than TimeCode.MaxValue.-or-value
        ''' is System.Double.PositiveInfinity.-or-value is System.Double.NegativeInfinity.
        ''' </exception>
        ''' <exception cref="System.ArgumentException">
        ''' value is equal to System.Double.NaN.
        ''' </exception>
        Public ReadOnly Property TimeCode() As

        ''' <summary>
        ''' Returns a TimeCode that represents a specified number of seconds,
        ''' where the specification is accurate to the nearest millisecond.
        ''' </summary>
        ''' <param name="seconds">A number of seconds, accurate to the nearest millisecond.</param>
        ''' ''' <param name="rate">The framerate of the Timecode.</param>
        ''' <returns>A TimeCode that represents value.</returns>
        ''' <exception cref="System.OverflowException">
        ''' value is less than TimeCode.MinValue or greater than TimeCode.MaxValue.-or-value
        '''  is System.Double.PositiveInfinity.-or-value is System.Double.NegativeInfinity.
        ''' </exception>
        ''' <exception cref="System.ArgumentException">
        ''' value is equal to System.Double.NaN.
        ''' </exception>
        Public ReadOnly Property TimeCode() As

        ''' <summary>
        ''' Returns a TimeCode that represents a specified number of frames.
        ''' </summary>
        ''' <param name="frames">A number of frames.</param>
        ''' <param name="rate">The framerate of the Timecode.</param>
        ''' <returns>A TimeCode that represents value.</returns>
        ''' <exception cref="System.OverflowException">
        '''  value is less than TimeCode.MinValue or greater than TimeCode.MaxValue.-or-value
        '''    is System.Double.PositiveInfinity.-or-value is System.Double.NegativeInfinity.
        ''' </exception>
        ''' <exception cref="System.ArgumentException">
        ''' value is equal to System.Double.NaN.
        ''' </exception>
        Public ReadOnly Property TimeCode() As

        ''' <summary>
        ''' Returns a TimeCode that represents a specified time, where the specification
        '''  is in units of ticks.
        ''' </summary>
        ''' <param name="ticks"> A number of ticks that represent a time.</param>
        ''' <param name="rate">The Smpte framerate.</param>
        ''' <returns>A TimeCode with a value of value.</returns>
        Public ReadOnly Property TimeCode() As

        ''' <summary>
        ''' Returns a TimeCode that represents a specified time, where the specification is
        ''' in units of 27 Mhz clock ticks.
        ''' </summary>
        ''' <param name="ticks27Mhz">A number of ticks in 27 Mhz clock format.</param>
        ''' <param name="rate">A Smpte framerate.</param>
        ''' <returns>A TimeCode.</returns>
        Public ReadOnly Property TimeCode() As

        ''' <summary>
        ''' Returns a TimeCode that represents a specified time, where the specification is
        ''' in units of absolute time.
        ''' </summary>
        ''' <param name="time">The absolute time in 100 nanosecond units.</param>
        ''' <param name="rate">The SMPTE framerate.</param>
        ''' <returns>A TimeCode.</returns>
        Public ReadOnly Property TimeCode() As

        ''' <summary>
        ''' Validates that the string provided is in the correct format for SMPTE 12M time code.
        ''' </summary>
        ''' <param name="timeCode">String that is the time code.</param>
        ''' <returns>True if this is a valid SMPTE 12M time code string.</returns>
        Public ReadOnly Property bool() As

        ''' <summary>
        ''' Validates that the hexadecimal formatted integer provided is in the correct format for SMPTE 12M time code
        ''' Time code is stored so that the hexadecimal value is read as if it were an integer value.
        ''' That is, the time code value 0x01133512 does not represent integer 18035986, rather it specifies 1 hour, 13 minutes, 35 seconds, and 12 frames.
        ''' </summary>
        ''' <param name="windowsMediaTimeCode">Integer that is the time code stored in hexadecimal format.</param>
        ''' <returns>True if this is a valid SMPTE 12M time code string.</returns>
        Public ReadOnly Property bool() As

        ''' <summary>
        ''' Returns the value of the provided time code string and framerate in 27Mhz ticks.
        ''' </summary>
        ''' <param name="timeCode">The SMPTE 12M formatted time code string.</param>
        ''' <param name="rate">The SMPTE framerate.</param>
        ''' <returns>A long that represents the value of the time code in 27Mhz ticks.</returns>
        Public ReadOnly Property long() As

        ''' <summary>
        ''' Parses a framerate value as double and converts it to a member of the SmpteFrameRate enumeration.
        ''' </summary>
        ''' <param name="rate">Double value of the framerate.</param>
        ''' <returns>A SmpteFrameRate enumeration value that matches the incoming rates.</returns>
        Public ReadOnly Property SmpteFrameRate() As

        ''' <summary>
        ''' Converts an absolute time and a frame rate to a formatted string.
        ''' </summary>
        ''' <param name="absoluteTime">Double precision floating point time in seconds.</param>
        ''' <param name="frameRate">SMPTE frame rate enum.</param>
        ''' <returns>A string that contains the correct format.</returns>
        Public ReadOnly Property string() As

        #End Region

        #region Public Methods

        ''' <summary>
        ''' Adds the specified TimeCode to this instance.
        ''' </summary>
        ''' <param name="ts">A TimeCode.</param>
        ''' <returns>A TimeCode that represents the value of this instance plus the value of ts.
        ''' </returns>
        ''' <exception cref="System.OverflowException">
        ''' The resulting TimeCode is less than TimeCode.MinValue or greater than TimeCode.MaxValue.
        ''' </exception>
        public TimeCode Add(TimeCode ts)
        {
            return this + ts
        }

        ''' <summary>
        '''  Compares this instance to a specified object and returns an indication of
        '''   their relative values.
        ''' </summary>
        ''' <param name="value">An object to compare, or null.</param>
        ''' <returns>
        '''  Value Condition -1 The value of this instance is less than the value of value.
        '''    0 The value of this instance is equal to the value of value. 1 The value
        '''    of this instance is greater than the value of value.-or- value is null.
        ''' </returns>
        ''' <exception cref="System.ArgumentException">
        '''  value is not a TimeCode.
        ''' </exception>
        public Function CompareTo(obj As Object) As Integer


            If ( Not (obj is TimeCode)) Then
                throw New ArgumentException(Resources.Smpte12MOutOfRange)
            End If


            TimeCode time1 = (TimeCode)obj


            If (this < time1) Then
                return -1
            End If



            If (this = time1) Then
                return 0
            End If


            return 1
        End Function  '   CompareTo


        ''' <summary>
        ''' Compares this instance to a specified TimeCode object and returns
        ''' an indication of their relative values.
        ''' </summary>
        ''' <param name="other"> A TimeCode object to compare to this instance.</param>
        ''' <returns>
        ''' A signed number indicating the relative values of this instance and value.Value
        ''' Description A negative integer This instance is less than value. Zero This
        ''' instance is equal to value. A positive integer This instance is greater than
        ''' value.
        ''' </returns>
        public Function CompareTo(other As TimeCode) As Integer


            If (this < other) Then
                return -1
            End If



            If (this = other) Then
                return 0
            End If


            return 1
        End Function  '   CompareTo


        ''' <summary>
        '''  Returns a value indicating whether this instance is equal to a specified
        '''  object.
        ''' </summary>
        ''' <param name="other">An object to compare with this instance.</param>
        ''' <returns>
        ''' True if value is a TimeCode object that represents the same time interval
        ''' as the current TimeCode structure; otherwise, false.
        ''' </returns>
        public override bool Equals(object obj)
        {

            If (this = (TimeCode)obj) Then
                return true
            End If


            return false
        }

        ''' <summary>
        ''' Returns a value indicating whether this instance is equal to a specified
        '''     TimeCode object.
        ''' </summary>
        ''' <param name="obj">An TimeCode object to compare with this instance.</param>
        ''' <returns>true if obj represents the same time interval as this instance; otherwise, false.
        ''' </returns>
        public Function Equals(other As TimeCode) As Boolean


            If (this = other) Then
                return true
            End If


            return false
        End Function  '   Equals


        ''' <summary>
        ''' Returns a hash code for this instance.
        ''' </summary>
        ''' <returns> A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return Me.GetHashCode()
        }

        ''' <summary>
        ''' Subtracts the specified TimeCode from this instance.
        ''' </summary>
        ''' <param name="ts">A TimeCode.</param>
        ''' <returns>A TimeCode whose value is the result of the value of this instance minus the value of ts.</returns>
        ''' <exception cref="OverflowException">The return value is less than TimeCode.MinValue or greater than TimeCode.MaxValue.</exception>
        public TimeCode Subtract(TimeCode ts)
        {
            return this - ts
        }

        ''' <summary>
        ''' Returns the SMPTE 12M string representation of the value of this instance.
        ''' </summary>
        ''' <returns>
        ''' A string that represents the value of this instance. The return value is
        '''     of the form: hh:mm:ss:ff for non-drop frame and hh:mm:ss;ff for drop frame code
        '''     with "hh" hours, ranging from 0 to 23, "mm" minutes
        '''     ranging from 0 to 59, "ss" seconds ranging from 0 to 59, and  "ff"  based on the
        '''     chosen framerate to be used by the time code instance.
        ''' </returns>
        public override string ToString()
        {
            return AbsoluteTimeToSmpte12M(Me.absoluteTime, Me.frameRate)
        }

        ''' <summary>
        ''' Outputs a string of the current time code in the requested framerate.
        ''' </summary>
        ''' <param name="rate">The SmpteFrameRate required for the string output.</param>
        ''' <returns>SMPTE 12M formatted time code string converted to the requested framerate.</returns>
        public Function ToString(rate As SmpteFrameRate) As String

            return AbsoluteTimeToSmpte12M(Me.absoluteTime, rate)
        End Function  '   ToString


        ''' <summary>
        ''' Returns the value of this instance in 27 Mhz ticks.
        ''' </summary>
        ''' <returns>A long value that is in 27 Mhz ticks.</returns>
        public long ToTicks27Mhz()
        {
            return AbsoluteTimeToTicks27Mhz(Me.absoluteTime)
        }

        ''' <summary>
        ''' Returns the value of this instance in MPEG 2 PCR time base (PcrTb) format.
        ''' </summary>
        ''' <returns>A long value that is in PcrTb.</returns>
        public long ToTicksPcrTb()
        {
            return AbsoluteTimeToTicksPcrTb(Me.absoluteTime)
        }

        #End Region

        #region Private Static Methdos

        ''' <summary>
        ''' Converts a SMPTE timecode to absolute time.
        ''' </summary>
        ''' <param name="timeCode">The timecode to convert from.</param>
        ''' <param name="rate">The <see cref="SmpteFrameRate"/> of the timecode.</param>
        ''' <returns>A <see cref="double"/> with the absolute time.</returns>
        Private ReadOnly Property double() As

        ''' <summary>
        ''' Parses a timecode string for the different parts of the timecode.
        ''' </summary>
        ''' <param name="timeCode">The source timecode to parse.</param>
        ''' <param name="hours">The Hours section from the timecode.</param>
        ''' <param name="minutes">The Minutes section from the timecode.</param>
        ''' <param name="seconds">The Seconds section from the timecode.</param>
        ''' <param name="frames">The frames section from the timecode.</param>
        Private ReadOnly Property Sub() As

        ''' <summary>
        ''' Generates a string representation of the timecode.
        ''' </summary>
        ''' <param name="hours">The Hours section from the timecode.</param>
        ''' <param name="minutes">The Minutes section from the timecode.</param>
        ''' <param name="seconds">The Seconds section from the timecode.</param>
        ''' <returns>The timecode in string format.</returns>
        Private ReadOnly Property string() As

        ''' <summary>
        ''' Converts to Absolute time from SMPTE 12M 23.98.
        ''' </summary>
        ''' <param name="timeCode">The timecode to parse.</param>
        ''' <returns>A <see cref="double"/> that contains the absolute duration.</returns>
        Private ReadOnly Property double() As

        ''' <summary>
        ''' Converts to Absolute time from SMPTE 12M 24.
        ''' </summary>
        ''' <param name="timeCode">The timecode to parse.</param>
        ''' <returns>A <see cref="double"/> that contains the absolute duration.</returns>
        Private ReadOnly Property double() As

        ''' <summary>
        ''' Converts to Absolute time from SMPTE 12M 25.
        ''' </summary>
        ''' <param name="timeCode">The timecode to parse.</param>
        ''' <returns>A <see cref="double"/> that contains the absolute duration.</returns>
        Private ReadOnly Property double() As

        ''' <summary>
        ''' Converts to Absolute time from SMPTE 12M 29.97 Drop frame.
        ''' </summary>
        ''' <param name="timeCode">The timecode to parse.</param>
        ''' <returns>A <see cref="double"/> that contains the absolute duration.</returns>
        Private ReadOnly Property double() As

        ''' <summary>
        ''' Converts to Absolute time from SMPTE 12M 29.97 Non Drop.
        ''' </summary>
        ''' <param name="timeCode">The timecode to parse.</param>
        ''' <returns>A <see cref="double"/> that contains the absolute duration.</returns>
        Private ReadOnly Property double() As

        ''' <summary>
        ''' Converts to Absolute time from SMPTE 12M 30.
        ''' </summary>
        ''' <param name="timeCode">The timecode to parse.</param>
        ''' <returns>A <see cref="double"/> that contains the absolute duration.</returns>
        Private ReadOnly Property double() As

        ''' <summary>
        ''' Converts from 27Mhz ticks to PCRTb.
        ''' </summary>
        ''' <param name="ticks27Mhz">The number of 27Mhz ticks to convert from.</param>
        ''' <returns>A <see cref="long"/> with the PCRTb.</returns>
        Private ReadOnly Property long() As

        ''' <summary>
        '''     Converts the provided absolute time to PCRTb.
        ''' </summary>
        ''' <param name="absoluteTime">Absolute time to be converted.</param>
        ''' <returns>The number of PCRTb ticks.</returns>
        Private ReadOnly Property long() As

        ''' <summary>
        '''     Converts the specified absolute time to 27 mhz ticks.
        ''' </summary>
        ''' <param name="absoluteTime">Absolute time to be converted.</param>
        ''' <returns>THe number of 27Mhz ticks.</returns>
        Private ReadOnly Property long() As

        ''' <summary>
        '''     Converts the specified absolute time to absolute time.
        ''' </summary>
        ''' <param name="ticksPcrTb">Ticks PCRTb to be converted.</param>
        ''' <returns>The absolute time.</returns>
        Private ReadOnly Property double() As

        ''' <summary>
        '''     Converts the specified absolute time to absolute time.
        ''' </summary>
        ''' <param name="ticks27Mhz">Ticks 27Mhz to be converted.</param>
        ''' <returns>The absolute time.</returns>
        Private ReadOnly Property double() As

        ''' <summary>
        ''' Converts to SMPTE 12M.
        ''' </summary>
        ''' <param name="absoluteTime">The absolute time to convert from.</param>
        ''' <param name="rate">The SMPTE frame rate.</param>
        ''' <returns>A string in SMPTE 12M format.</returns>
        Private ReadOnly Property string() As

        ''' <summary>
        ''' Returns the number of frames.
        ''' </summary>
        ''' <param name="absoluteTime">The absolute time to use for parsing from.</param>
        ''' <param name="rate">The SMPTE frame rate to use for the conversion.</param>
        ''' <returns>A <see cref="long"/> with the number of frames.</returns>
        Private ReadOnly Property long() As

        ''' <summary>
        ''' Returns the absolute time.
        ''' </summary>
        ''' <param name="frames">The number of frames.</param>
        ''' <param name="rate">The SMPTE frame rate to use for the conversion.</param>
        ''' <returns>The absolute time.</returns>
        Private ReadOnly Property double() As

        ''' <summary>
        ''' Returns the SMPTE 12M 23.98 timecode.
        ''' </summary>
        ''' <param name="absoluteTime">The absolute time to convert from.</param>
        ''' <returns>A string that contains the correct format.</returns>
        Private ReadOnly Property string() As

        ''' <summary>
        ''' Converts to SMPTE 12M 24fps.
        ''' </summary>
        ''' <param name="absoluteTime">The absolute time to convert from.</param>
        ''' <returns>A string that contains the correct format.</returns>
        Private ReadOnly Property string() As

        ''' <summary>
        ''' Converts to SMPTE 12M 25fps.
        ''' </summary>
        ''' <param name="absoluteTime">The absolute time to convert from.</param>
        ''' <returns>A string that contains the correct format.</returns>
        Private ReadOnly Property string() As

        ''' <summary>
        ''' Converts to SMPTE 12M 29.97fps Drop.
        ''' </summary>
        ''' <param name="absoluteTime">The absolute time to convert from.</param>
        ''' <returns>A string that contains the correct format.</returns>
        Private ReadOnly Property string() As

        ''' <summary>
        ''' Converts to SMPTE 12M 29.97fps Non Drop.
        ''' </summary>
        ''' <param name="absoluteTime">The absolute time to convert from.</param>
        ''' <returns>A string that contains the correct format.</returns>
        Private ReadOnly Property string() As

        ''' <summary>
        ''' Converts to SMPTE 12M 30fps.
        ''' </summary>
        ''' <param name="absoluteTime">The absolute time to convert from.</param>
        ''' <returns>A string that contains the correct format.</returns>
        Private ReadOnly Property string() As

        ''' <summary>
        ''' Converts absolute time to HMS.
        ''' </summary>
        ''' <param name="absoluteTime">The absolute time to convert from.</param>
        ''' <returns>A string that contains the correct format.</returns>
        Private ReadOnly Property string() As

        ''' <summary>
        ''' Converts to Ticks 27Mhz.
        ''' </summary>
        ''' <param name="timeCode">The timecode to convert from.</param>
        ''' <returns>The number of 27Mhz ticks.</returns>
        Private ReadOnly Property long() As

        ''' <summary>
        ''' Converts to Ticks 27Mhz.
        ''' </summary>
        ''' <param name="timeCode">The timecode to convert from.</param>
        ''' <returns>The number of 27Mhz ticks.</returns>
        Private ReadOnly Property long() As

        ''' <summary>
        ''' Converts to Ticks 27Mhz.
        ''' </summary>
        ''' <param name="timeCode">The timecode to convert from.</param>
        ''' <returns>The number of 27Mhz ticks.</returns>
        Private ReadOnly Property long() As

        ''' <summary>
        ''' Converts to Ticks 27Mhz.
        ''' </summary>
        ''' <param name="timeCode">The timecode to convert from.</param>
        ''' <returns>The number of 27Mhz ticks.</returns>
        Private ReadOnly Property long() As

        ''' <summary>
        ''' Converts to Ticks 27Mhz.
        ''' </summary>
        ''' <param name="timeCode">The timecode to convert from.</param>
        ''' <returns>The number of 27Mhz ticks.</returns>
        Private ReadOnly Property long() As

        ''' <summary>
        ''' Converts to Ticks 27Mhz.
        ''' </summary>
        ''' <param name="timeCode">The timecode to convert from.</param>
        ''' <returns>The number of 27Mhz ticks.</returns>
        Private ReadOnly Property long() As

        ''' <summary>
        ''' Converts to SMPTE 12M 29.27fps Non Drop.
        ''' </summary>
        ''' <param name="ticks27Mhz">The number of 27Mhz ticks to convert from.</param>
        ''' <returns>A string that contains the correct format.</returns>
        Private ReadOnly Property string() As

        ''' <summary>
        ''' Converts to SMPTE 12M 29.27fps Non Drop.
        ''' </summary>
        ''' <param name="ticks27Mhz">The number of 27Mhz ticks to convert from.</param>
        ''' <returns>A string that contains the correct format.</returns>
        Private ReadOnly Property string() As

        ''' <summary>
        ''' Converts to SMPTE 12M 23.98fps.
        ''' </summary>
        ''' <param name="ticks27Mhz">The number of 27Mhz ticks to convert from.</param>
        ''' <returns>A string that contains the correct format.</returns>
        Private ReadOnly Property string() As

        ''' <summary>
        ''' Converts to SMPTE 12M 24fps.
        ''' </summary>
        ''' <param name="ticks27Mhz">The number of 27Mhz ticks to convert from.</param>
        ''' <returns>A string that contains the correct format.</returns>
        Private ReadOnly Property string() As

        ''' <summary>
        ''' Converts to SMPTE 12M 25fps.
        ''' </summary>
        ''' <param name="ticks27Mhz">The number of 27Mhz ticks to convert from.</param>
        ''' <returns>A string that contains the correct format.</returns>
        Private ReadOnly Property string() As

        ''' <summary>
        ''' Converts to SMPTE 12M 30fps.
        ''' </summary>
        ''' <param name="ticks27Mhz">The number of 27Mhz ticks to convert from.</param>
        ''' <returns>A string that contains the correct format.</returns>
        Private ReadOnly Property string() As

        #region Unused Code

        /*

        ''' <summary>
        '''     Converts the specified absolute time to PCRtb
        ''' </summary>
        ''' <param name="ticksPcrTb">PCR-tb time to be converted</param>
        Private ReadOnly Property double() As
        */

        #End Region

        #End Region
    }

    ''' <summary>
    ''' extension to allow screen reader to read times out nicely.
    ''' </summary>
    Public Dim Class TimeSpanExtensions
    {
        Public ReadOnly Property string() As
    End Class   '   TimeSpanExtensions
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\TimeCode.cs
