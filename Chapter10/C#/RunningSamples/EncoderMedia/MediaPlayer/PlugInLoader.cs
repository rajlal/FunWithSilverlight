namespace ExpressionMediaPlayer
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Net;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Resources;
    using System.Xml;

    internal class PlugInLoaderFailedException : Exception
    {
        internal PlugInLoaderFailedException() : base() { }
        internal PlugInLoaderFailedException(string message) : base(message) { }
        internal PlugInLoaderFailedException(string message, Exception exp) : base(message, exp) { }
    }

    internal class XAPReadCompletedEventArgs : AsyncCompletedEventArgs 
    {
        internal XAPReadCompletedEventArgs(Exception error)
            : base(error, false, null)
        {
        }

        internal XAPReadCompletedEventArgs(Stream result)
            : base(null, false, null)
        {
            this.Result = result;
        }

        internal XAPReadCompletedEventArgs(OpenReadCompletedEventArgs e)
            : base(e.Error, e.Cancelled, e.UserState)
        {
            if ((e.Error == null) && !e.Cancelled)
            {
                this.Result = e.Result;
            }
        }

        public Stream Result { get; private set;  }

    }

    internal class PlugInLoader
    {
        private Control uiThreadObject;
        private WebClient webClient;
        private string targetAssemblyName;
        private Assembly assemblyTarget;

        internal event EventHandler<XAPReadCompletedEventArgs> PlugInLoadCompleted;

        internal PlugInLoader()
        {
            Debug.WriteLine("PlugInLoader()");
        }

        internal PlugInLoader(Control uiThreadObject)
        {
            Debug.WriteLine("PlugInLoader(Control uiThreadObject)");
            this.uiThreadObject = uiThreadObject;
        }

        private void LoadAssembly(string source, StreamResourceInfo streamInfo)
        {
            Debug.WriteLine("PlugInLoader.LoadAssembly(" + source + ", " + streamInfo.ToString() + " )");

            AssemblyPart asmPart = new AssemblyPart();
            Assembly loadedAssembly = asmPart.Load(streamInfo.Stream);
            if (0 == string.Compare(source, this.targetAssemblyName, StringComparison.OrdinalIgnoreCase))
            {
                this.assemblyTarget = loadedAssembly;
            }
        }

        private void AppManifestLoader(Stream xap)
        {
            Debug.WriteLine("PlugInLoader.AppManifestLoader( xap length=" + xap.Length.ToString());

            var appManifestUri = new Uri("AppManifest.xaml", UriKind.Relative);
            var resourceStreamInfo = new StreamResourceInfo(xap, null);
            var resourceStream = Application.GetResourceStream(resourceStreamInfo, appManifestUri);
            Debug.Assert(resourceStream != null);
            StreamReader manifestStreamReader = new StreamReader(resourceStream.Stream);

            XmlReader appManifestReader = XmlReader.Create(manifestStreamReader);
            while (appManifestReader.Read())
            {
                if ((appManifestReader.NodeType == XmlNodeType.Element)
                && (0==string.Compare(appManifestReader.Name, "AssemblyPart", StringComparison.OrdinalIgnoreCase))
                && (appManifestReader.HasAttributes))
                {
                    string source = appManifestReader.GetAttribute("Source");
                    StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(xap, "application/binary"), new Uri(source, UriKind.Relative));
                    if (this.uiThreadObject != null)
                    {
                        this.uiThreadObject.Dispatcher.BeginInvoke(delegate
                        {
                            LoadAssembly(source, streamInfo);
                        });
                    }
                    else
                    {
                        LoadAssembly(source, streamInfo);
                    }
                }
            }
        }

        private void WebReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            var xe = new XAPReadCompletedEventArgs(e);
            XAPReadCompleted(sender, xe);
        }

        private void XAPReadCompleted(object sender, XAPReadCompletedEventArgs e)
        {
            Debug.WriteLine("PlugInLoader.XAPReadCompleted()");

            // Check for error conditions
            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    AppManifestLoader(e.Result);
                }

                if (this.PlugInLoadCompleted != null)
                {
                    this.PlugInLoadCompleted(sender, e);
                }
            }

            this.webClient = null;
        }

        internal void Load(Uri uriXAP, string targetAssemblyNameParameter)
        {
            Debug.WriteLine("PlugInLoader.Load(" + uriXAP.ToString() + ", " + targetAssemblyName + " )");

            this.assemblyTarget = null;
            this.targetAssemblyName = targetAssemblyNameParameter;

            if (MediaPlayer.IsOffline)
            {
                var xap = MediaPlayer.MakeOfflineIsoUri(uriXAP);
                if (xap.IsoFileExists)
                {
                    Debug.WriteLine("Attempting to load:" + uriXAP + " from:" + xap.StreamName);
                    var xapStream = xap.Stream;
                    var successArgs = new XAPReadCompletedEventArgs(xapStream);
                    XAPReadCompleted(null, successArgs);
                }
                else
                {
                    var errorArgs = new XAPReadCompletedEventArgs(new FileNotFoundException(uriXAP.ToString()));
                    XAPReadCompleted(null, errorArgs);
                }
            }
            else
            {
                this.webClient = new WebClient();
                this.webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(WebReadCompleted);
                this.webClient.OpenReadAsync(uriXAP);
            }
        }

/*
        internal void LocalLoad(Stream localXapStream, string targetAssemblyNameParameter)
        {
            Debug.WriteLine("PlugInLoader.LocalLoad(" + localXapStream.Length.ToString() + ", " + targetAssemblyName + " )");

            this.assemblyTarget = null;
            this.targetAssemblyName = targetAssemblyNameParameter;
            AppManifestLoader(localXapStream);
        }
 */

        internal bool Ready { get { return this.assemblyTarget != null; } }

        internal object CreateObject(string className)
        {
            Debug.WriteLine("PlugInLoader.CreateObject(" + className + " )");

            string message = string.Empty;
            try
            {
                if (this.Ready)
                {
                    return this.assemblyTarget.CreateInstance(className);
                }
            }
            catch (MissingMethodException mme)
            {
                message = mme.ToString();
                Debug.WriteLine("CreateObject failed:" + message);
            }
            throw new PlugInLoaderFailedException("CreateObject:" + message);
        }
    }
}