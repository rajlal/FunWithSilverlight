﻿// <copyright file="SmpteFrameRate.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>Implements the MediaPlayer class</summary>
// <author>Microsoft Expression Encoder Team</author>
namespace ExpressionMediaPlayer
{
    /// <summary>
    /// SMPTE Frame Rates enumeration. Use this enumeration with the Timecode struct to set the framerate for the Timecode.
    /// </summary>
    /// <remarks> 
    /// Framerates supported by the Timecode class include, 23.98 IVTC Film Sync, 24fps Film Sync, 25fps PAL, 29.97 drop frame,
    /// 29.97 Non drop, and 30fps.
    /// </remarks>
    public enum SmpteFrameRate
    {
        /// <summary>
        /// unknown setting.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// SMPTE 23.98 frame rate. Also known as Film Sync.
        /// </summary>
        Smpte2398 = 1,
        
        /// <summary>
        /// SMPTE 24 fps frame rate.
        /// </summary>
        Smpte24 = 2,
        
        /// <summary>
        /// SMPTE 25 fps frame rate. Also known as PAL.
        /// </summary>
        Smpte25 = 3,
        
        /// <summary>
        /// SMPTE 29.97 fps Drop Frame timecode. Used in the NTSC television system.
        /// </summary>
        Smpte2997Drop = 4,
        
        /// <summary>
        /// SMPTE 29.97 fps Non Drop Fram timecode. Used in the NTSC television system.
        /// </summary>
        Smpte2997NonDrop = 5,
        
        /// <summary>
        /// SMPTE 30 fps frame rate.
        /// </summary>
        Smpte30 = 6,        
    }
}
