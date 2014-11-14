' Namespace ExpressionMediaPlayer

Imports    using System
    using System.ComponentModel
    using System.Diagnostics
    using System.IO
    using System.IO.IsolatedStorage
    using System.Net
    using System.Reflection
    using System.Windows
    using System.Windows.Controls
    using System.Windows.Resources
    using System.Xml

    internal Class PlugInLoaderFailedException
        Inherits Exception
        internal PlugInLoaderFailedException() : base() { }
        internal PlugInLoaderFailedException(string message) : base(message) { }
        internal PlugInLoaderFailedException(string message, Exception exp) : base(message, exp) { }
    End Class   '   PlugInLoaderFailedException


    internal Class XAPReadCompletedEventArgs
        Inherits AsyncCompletedEventArgs
        internal XAPReadCompletedEventArgs(Exception error)
            : base(error, false, Nothing)
        }

        internal XAPReadCompletedEventArgs(Stream result)
            : base(Nothing, false, Nothing)
        {
            Me.Result = result
        }

        internal XAPReadCompletedEventArgs(OpenReadCompletedEventArgs e)
            : base(e.Error, e.Cancelled, e.UserState)
        {

            If ((e.Error Is Nothing)  AndAlso   Not e.Cancelled) Then
                Me.Result = e.Result
            End If

        }

        Public ReadOnly Property Result() As Stream

        internal PlugInLoader(Control uiThreadObject)
        {
            Debug.WriteLine("PlugInLoader(Control uiThreadObject)")
            Me.uiThreadObject = uiThreadObject
        }

        private Sub LoadAssembly(source As String, streamInfo As StreamResourceInfo)

            Debug.WriteLine("PlugInLoader.LoadAssembly(" + source + ", " + streamInfo.ToString() + " )")

            AssemblyPart asmPart = New AssemblyPart()
            Assembly loadedAssembly = asmPart.Load(streamInfo.Stream)

            If (0 = string.Compare(source, Me.targetAssemblyName, StringComparison.OrdinalIgnoreCase)) Then
                Me.assemblyTarget = loadedAssembly
            End If
        End Sub '   LoadAssembly

        private Sub AppManifestLoader(xap As Stream)

            Debug.WriteLine("PlugInLoader.AppManifestLoader( xap length=" + xap.Length.ToString())

            var appManifestUri = New Uri("AppManifest.xaml", UriKind.Relative)
            var resourceStreamInfo = New StreamResourceInfo(xap, Nothing)
            var resourceStream = Application.GetResourceStream(resourceStreamInfo, appManifestUri)
            Debug.Assert(resourceStream  IsNot Nothing)

            Dim manifestStreamReader As StreamReader  = New StreamReader(resourceStream.Stream)

            Dim appManifestReader As XmlReader  = XmlReader.Create(manifestStreamReader)


            While (appManifestReader.Read())


                If ((appManifestReader.NodeType = XmlNodeType.Element) Then
                 AndAlso  (0=string.Compare(appManifestReader.Name, "AssemblyPart", StringComparison.OrdinalIgnoreCase))
                 AndAlso  (appManifestReader.HasAttributes))
                {

                    Dim source As String  = appManifestReader.GetAttribute("Source")
 End If

                    StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(xap, "application/binary"), New Uri(source, UriKind.Relative))

                    If (Me.uiThreadObject  IsNot Nothing) Then
                        Me.uiThreadObject.Dispatcher.BeginInvoke(delegate
                        {
                            LoadAssembly(source, streamInfo)
                        })
                    else
                        LoadAssembly(source, streamInfo)
                    End If

                }
            End While   '
        End Sub '   AppManifestLoader

        private Sub WebReadCompleted(sender As Object, e As OpenReadCompletedEventArgs)

            var xe = New XAPReadCompletedEventArgs(e)
            XAPReadCompleted(sender, xe)
        End Sub '   WebReadCompleted


        private Sub XAPReadCompleted(sender As Object, e As XAPReadCompletedEventArgs)

            Debug.WriteLine("PlugInLoader.XAPReadCompleted()")

            '  Check for error conditions

            If ( Not e.Cancelled) Then

                If (e.Error Is Nothing) Then
                    AppManifestLoader(e.Result)
                End If



                If (Me.PlugInLoadCompleted  IsNot Nothing) Then
                    Me.PlugInLoadCompleted(sender, e)
                End If
            End If

            Me.webClient = Nothing
        End Sub '   XAPReadCompleted


        internal Sub Load(uriXAP As Uri, targetAssemblyNameParameter As String)

            Debug.WriteLine("PlugInLoader.Load(" + uriXAP.ToString() + ", " + targetAssemblyName + " )")

            Me.assemblyTarget = Nothing
            Me.targetAssemblyName = targetAssemblyNameParameter


            If (MediaPlayer.IsOffline) Then
                var xap = MediaPlayer.MakeOfflineIsoUri(uriXAP)
                if (xap.IsoFileExists)
                {
                    Debug.WriteLine("Attempting to load:" + uriXAP + " from:" + xap.StreamName)
                    var xapStream = xap.Stream
                    var successArgs = New XAPReadCompletedEventArgs(xapStream)
                    XAPReadCompleted(Nothing, successArgs)
                }
                else
                {
                    var errorArgs = New XAPReadCompletedEventArgs(new FileNotFoundException(uriXAP.ToString()))
                    XAPReadCompleted(Nothing, errorArgs)
                }
            else
                Me.webClient = New WebClient()
                AddHandler Me.webClient.OpenReadCompleted, AddressOf New OpenReadCompletedEventHandler(WebReadCompleted)
                Me.webClient.OpenReadAsync(uriXAP)
            End If
        End Sub '   Load

/*
        internal Sub LocalLoad(localXapStream As Stream, targetAssemblyNameParameter As String)

            Debug.WriteLine("PlugInLoader.LocalLoad(" + localXapStream.Length.ToString() + ", " + targetAssemblyName + " )")

            Me.assemblyTarget = Nothing
            Me.targetAssemblyName = targetAssemblyNameParameter
            AppManifestLoader(localXapStream)
        End Sub '   LocalLoad

 */

        internal bool Ready { get { return Me.assemblyTarget  IsNot Nothing; } }

        internal object CreateObject(string className)
        {
            Debug.WriteLine("PlugInLoader.CreateObject(" + className + " )")

            Dim message As String  = string.Empty

            Try

                If (Me.Ready) Then
                    return Me.assemblyTarget.CreateInstance(className)
                End If


            Catch mme As MissingMethodException

                message = mme.ToString()
                Debug.WriteLine("CreateObject failed:" + message)
            End Try

            throw New PlugInLoaderFailedException("CreateObject:" + message)
        }
    End Class   '   PlugInLoader
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\PlugInLoader.cs
