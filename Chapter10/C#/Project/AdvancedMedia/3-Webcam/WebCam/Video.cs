using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCam
{
    public class Video
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int SampleSize { get; set; }
        public float FramesPerSecond { get; set; }
        public byte[] Data { get; set; }
    }

    public class VideoEventArgs : EventArgs
    {
        public VideoEventArgs(Video video)
        {
            this.Video = video;
        }

        public Video Video { get; private set; }
    }
}
