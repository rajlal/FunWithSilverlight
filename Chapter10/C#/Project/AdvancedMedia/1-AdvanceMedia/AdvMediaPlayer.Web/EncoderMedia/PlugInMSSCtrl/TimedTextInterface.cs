using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using Microsoft.Expression.Encoder.PlugInMssCtrl;

namespace TimedTextInterface
{
    public interface ITimedTextEvents
    {
        int Count { get; }
        TimeSpan TimeSpan { get; }
        string ErrorInfo { get; }
    }
    
    public interface ITimedTextModel
    {
        /// <summary>
        /// The media element object that for triggering events
        /// </summary>
        MediaElementShim MediaElement { get;  set; }

        /// <summary>
        /// The Panel on which to display the DFXP captions 
        /// </summary>
        Panel Destination { get; set; }

        /// <summary>
        /// Parse the supplied DFXP data and add event from it between the specified time ranges into the set of events
        /// </summary>
        /// <param name="dfxpData"></param>
        /// <param name="offset"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>string.Empty on success, some sort of error message otherwise</returns>
        ITimedTextEvents ParseData(TimeSpan timeStamp, Stream dfxpData);

        /// <summary>
        /// Remove all event data
        /// </summary>
        void ClearEventData();

        /// <summary>
        /// Remove all DFXP Markers
        /// </summary>
        void ClearMarkers();
       
        /// <summary>
        /// Attach markers for current event data -- not this method must be called on the UI thread
        /// </summary>
        void AttachEvents(ITimedTextEvents events);

        /// <summary>
        /// Clear the DFXP caption area (call this after a seek)
        /// </summary>
        void ClearCaptionArea();

        /// <summary>
        /// Redraw the DFXP caption area (call this after a window resize event)
        /// </summary>
        void RefreshCaptionArea();
    }
}
