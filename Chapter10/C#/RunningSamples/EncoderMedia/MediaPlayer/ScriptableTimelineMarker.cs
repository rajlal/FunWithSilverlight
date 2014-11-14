// <copyright file="ScriptableObservableCollection.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>Implements a Scriptable version of the ObservableCollection class</summary>
// <author>Microsoft Expression Encoder Team</author>
using System.Windows;
using System.Windows.Browser;
using System.Windows.Media;

namespace ExpressionMediaPlayer
{
    [ScriptableType]
    public class ScriptableTimelineMarkerRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// event args for when timeline marker reached
        /// </summary>
        /// <param name="marker"></param>
        public ScriptableTimelineMarkerRoutedEventArgs(ScriptableTimelineMarker marker)
        {
            Marker = marker;
        }

        public ScriptableTimelineMarker Marker { get; set; }
    }

    public delegate void ScriptableTimelineMarkerRoutedEventHandler(object sender, ScriptableTimelineMarkerRoutedEventArgs e);

    [ScriptableType]
    public class ScriptableTimelineMarker
    {
        /// <summary>
        /// text of marker
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// time of marker (seconds)
        /// </summary>
        public double Time { get; set; }
        /// <summary>
        /// type of marker
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// create new ScriptableTimelineMarker from TimelineMarker
        /// </summary>
        /// <param name="marker"></param>
        public ScriptableTimelineMarker(TimelineMarker marker)
        {
            Text = marker.Text;
            Time = marker.Time.TotalSeconds;
            Type = marker.Type;
        }
    }
}
