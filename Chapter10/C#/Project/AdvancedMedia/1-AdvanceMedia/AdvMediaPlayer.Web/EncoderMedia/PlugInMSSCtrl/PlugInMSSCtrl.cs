using System;

[assembly: CLSCompliant(true)]
namespace Microsoft.Expression.Encoder.PlugInMssCtrl
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Control interface for the Smooth Streaming Media Stream Source
    /// </summary>
    public interface IPlugInMssCore
    {
        /// <summary>
        /// The MediaElement used to render the content
        /// </summary>
        MediaElement MediaElement { get; set; }
        /// <summary>
        /// The internet address of the content manifest
        /// </summary>
        Uri ManifestUrl { get; set; }
        /// <summary>
        /// Start Smooth Streaming playerback
        /// </summary>
        void StartPlayback();
        /// <summary>
        /// Stop Smooth Streaming playback
        /// </summary>
        void StopPlayback();
    }

    /// <summary>
    /// Interface for supporting offline playback of Smooth Streaming content.
    /// </summary>
    public interface IPlugInMssOfflineSupport
    {
        /// <summary>
        /// Parse a Smooth Streaming Manifest from a stream containing XML
        /// </summary>
        /// <param name="manifestStream">stream containing the XML</param>
        void ParseManifestFromStream(Stream manifestStream, Uri manifestUrl);
        /// <summary>
        /// Get the offline bitrate reccommened by the heuristics based on the stream type and the  size of the player window
        /// </summary>
        /// <param name="streamType">what type of stream video or audio</param>
        /// <param name="playerSize">The dimensions of the playback window -- the heuristics will not reccommend a bitrate whose dimensions are larger.</param>
        /// <returns></returns>
        long RecommendBitrateInKbps(MediaStreamType streamType, Size playerSize);
        /// <summary>
        /// Return a collection of URLS for all the chunks at the specified bitrate
        /// </summary>
        /// <param name="streamType">The desired stream type</param>
        /// <param name="bitrateInKbps">The desired bitrate -- note this must match one of the bitrates returned by GetBitratesInKbps</param>
        /// <returns></returns>
        ReadOnlyCollection<Uri> GetChunkUris(MediaStreamType streamType, long bitrateInKbps);
        /// <summary>
        /// Set the bitrate that is always selected by the MSS when playing in offline mode
        /// </summary>
        /// <param name="streamType">The target stream type video or audio</param>
        /// <param name="offlineBitrateInKbps">The bitrate to play in offline mode</param>
        void SetOfflinePlaybackBitrateInKbps(MediaStreamType streamType, long offlineBitrateInKbps);
    }

    /// <summary>
    /// Interface for providing a statistics graph
    /// </summary>
    public interface IPlugInMssStatisticsGraph
    {
        /// <summary>
        /// UIElement to be inserted into the visual tree to display a graph of the playback statistics.
        /// </summary>
        UIElement StatisticsGraph { get; }
    }
}
