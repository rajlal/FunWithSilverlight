' Namespace ExpressionMediaPlayer

Imports    using System
Imports Microsoft.VisualBasic
    using System.Diagnostics
    using System.Globalization
    using System.IO.IsolatedStorage
    using System.Windows
    using System.Windows.Browser
    using System.Windows.Controls
    using System.Windows.Controls.Primitives

    <TemplatePart(Name := OfflineDownloadProgressDialog.ButtonCancelOfflineDownload, Type := GetType(ButtonBase))>
    <TemplatePart(Name := OfflineDownloadProgressDialog.ButtonCompleteInstallation, Type := GetType(ButtonBase))>
    <TemplatePart(Name := OfflineDownloadProgressDialog.ProgressBarThisFile, Type := GetType(ProgressBar))>
    <TemplatePart(Name := OfflineDownloadProgressDialog.ProgressBarOfflining, Type := GetType(ProgressBar))>
    <TemplatePart(Name := OfflineDownloadProgressDialog.TextBlockXofY, Type := GetType(TextBlock))>
    <TemplatePart(Name := OfflineDownloadProgressDialog.TextBlockDownloading, Type := GetType(TextBlock))>
    <TemplatePart(Name := OfflineDownloadProgressDialog.TextBlockPromptToFinish, Type := GetType(TextBlock))>
    <TemplatePart(Name := OfflineDownloadProgressDialog.TextBlockErrorMessage, Type := GetType(TextBlock))>
    Public Partial Class OfflineDownloadProgressDialog
        Inherits Control
        Private parent As Popup
        Private grid As Grid
        Private player As MediaPlayer

        ''' <summary>
        ''' The percentage of the current file that has been downloaded
        ''' </summary>
        internal double DownloadPercent { get; set; }
        ''' <summary>
        ''' The total number of items to be downloaded
        ''' </summary>
        internal int TotalItemCount { get; private set; }
        ''' <summary>
        ''' The current item just downloaded
        ''' </summary>
        internal int CurrentItem { get; set; }
        ''' <summary>
        ''' The flag set when the user clicks the cancel button
        ''' </summary>
        internal bool CancelWasClicked { get; private set; }

        ''' <summary>
        ''' The flag set when the background thread encounters a error
        ''' </summary>
        internal bool ErrorOccured { get; private set; }

        ''' <summary>
        ''' The error message set when the background thread encounters a error
        ''' </summary>
        internal string ErrorMessage { get; private set; }

        Public Sub New(player As MediaPlayer)
            Me.player = player
            Me.DefaultStyleKey = GetType(OfflineDownloadProgressDialog)
        End Sub '   New


        ''' <summary>
        ''' String for the start button template element.
        ''' </summary>
        protected const string ButtonCancelOfflineDownload = "buttonCancelOfflineDownload"
        protected const string ButtonCompleteInstallation = "buttonCompleteInstallation"
        protected const string ProgressBarThisFile = "progressBarThisFile"
        protected const string ProgressBarOfflining = "progressBarOfflining"

        protected const string TextBlockXofY = "textBlockXofY"

        protected const string TextBlockDownloading = "textBlockDownloading"
        protected const string TextBlockPromptToFinish = "textBlockPromptToFinish"
        protected const string TextBlockErrorMessage = "textBlockErrorMessage"

        public override Sub OnApplyTemplate()

            MyBase.OnApplyTemplate()
            GetTemplateChildren()
            HookHandlers()
        End Sub '   OnApplyTemplate


        ''' <summary>
        ''' Gets the child elements of the template.
        ''' </summary>
        protected virtual Sub GetTemplateChildren()

            m_buttonCancelOfflineDownload = GetTemplateChild(ButtonCancelOfflineDownload) as ButtonBase
            m_buttonCompleteInstallation = GetTemplateChild(ButtonCompleteInstallation) as ButtonBase

            m_progressBarThisFile = GetTemplateChild(ProgressBarThisFile) as ProgressBar
            m_progressBarOfflining = GetTemplateChild(ProgressBarOfflining) as ProgressBar

            m_textBlockXofY = GetTemplateChild(TextBlockXofY) as TextBlock

            m_textBlockDownloading = GetTemplateChild(TextBlockDownloading) as TextBlock
            m_textBlockPromptToFinish = GetTemplateChild(TextBlockPromptToFinish) as TextBlock
            m_textBlockErrorMessage = GetTemplateChild(TextBlockErrorMessage) as TextBlock
        End Sub '   GetTemplateChildren


        ''' <summary>
        ''' Hooks our event handlers.
        ''' </summary>
        protected virtual Sub HookHandlers()


            If (m_buttonCancelOfflineDownload  IsNot Nothing) Then
                m_buttonCancelOfflineDownload.Click += Click_CancelOfflineDownload
            End If



            If (m_buttonCompleteInstallation  IsNot Nothing) Then
                m_buttonCompleteInstallation.Click += Click_CompleteInstallation
            End If
        End Sub '   HookHandlers

        ButtonBase m_buttonCancelOfflineDownload
        ButtonBase m_buttonCompleteInstallation

        ProgressBar m_progressBarThisFile
        ProgressBar m_progressBarOfflining

        Dim m_textBlockXofY As TextBlock

        Dim m_textBlockDownloading As TextBlock
        Dim m_textBlockPromptToFinish As TextBlock
        Dim m_textBlockErrorMessage As TextBlock

        ''' <summary>
        ''' Browser window resize event handler
        ''' </summary>
        private Sub OnPluginSizeChanged(sender As Object, e As EventArgs)

            UpdateSize()
        End Sub '   OnPluginSizeChanged


        ''' <summary>
        ''' Resizes the grid container -- effectively centering the dialog in the application window
        ''' </summary>
        private Sub UpdateSize()

            Me.grid.Width = Application.Current.Host.Content.ActualWidth
            Me.grid.Height = Application.Current.Host.Content.ActualHeight
        End Sub '   UpdateSize


        ''' <summary>
        ''' Respond to the user clicking the cancel button
        ''' </summary>
        private Sub Click_CancelOfflineDownload(sender As Object, e As RoutedEventArgs)

            Me.CancelWasClicked = true
            Me.m_buttonCancelOfflineDownload.IsEnabled = false
            '  Download was completed -- the user is cancelling the final install

            If (Me.m_buttonCompleteInstallation.IsEnabled) Then
                Close()
            End If

            Me.player.SetOfflineButtonEnabled(true)
        End Sub '   Click_CancelOfflineDownload


        ''' <summary>
        ''' Respond to the user clicking the finish installation button
        ''' </summary>
        private Sub Click_CompleteInstallation(sender As Object, e As RoutedEventArgs)

            Dim errorMessage As String  = string.Empty
            Dim installSuccess As Boolean  = false

            Try
                '  actually take the app offline.
                installSuccess = Application.Current.Install()
                Debug.WriteLine("Application.Current.Install() result=" + installSuccess.ToString())

                If (installSuccess) Then
                    Me.player.SetOfflineButtonVisibility(Visibility.Collapsed)
                    Close()
                End If


            Catch exp As InvalidOperationException

                Debug.WriteLine("Exception attempting to install offline" + exp.ToString())
                errorMessage = exp.ToString()

            Catch exp As Exception

                Debug.WriteLine("Exception attempting to install offline" + exp.ToString())
                errorMessage = exp.ToString()
            End Try


            If ( Not installSuccess) Then
                '  Failed for some reason -- let them try again ?
                Me.player.SetOfflineButtonEnabled(true)

                If ( Not string.IsNullOrEmpty(errorMessage)) Then
                    Me.ReportError(errorMessage)
                End If
            End If
        End Sub '   Click_CompleteInstallation


        ''' <summary>
        ''' Displays this popup window
        ''' </summary>
        internal Sub Show()


            If (Me.parent Is Nothing) Then
                Me.parent = New Popup()
                Me.grid = New Grid()
                grid.Children.Add(this)
                Me.parent.Child = grid
                Application.Current.Host.Content.Resized += OnPluginSizeChanged
                Me.parent.IsOpen = true

                If (Me.m_buttonCompleteInstallation  IsNot Nothing) Then
                    Me.m_buttonCompleteInstallation.IsEnabled = false
                End If

                UpdateSize()
            End If
        End Sub '   Show

        ''' <summary>
        ''' Shuts down this popup window
        ''' </summary>
        internal Sub Close()


            If (Me.CancelWasClicked) Then
                Try
                    '  Zap all those files just downloaded.
                    IsolatedStorageFile.GetUserStoreForApplication().Remove()

                Catch iso As IsolatedStorageException

                    Debug.WriteLine("IsolatedStorageException clearing out data during cancel" + iso.Message)
                    ReportError(iso.Message)
                End Try
            End If


            If (Me.parent  IsNot Nothing) Then
                Me.parent.IsOpen = false
                Application.Current.Host.Content.Resized -= OnPluginSizeChanged
                Me.grid.Children.Remove(this)
                Me.parent.Child = Nothing
                Me.parent = Nothing
            End If
        End Sub '   Close

        ''' <summary>
        ''' downloading playlist
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        internal Sub DownloadProgressChanged(sender As Object, e As PlaylistDownloadProgressEventArgs)

            Me.DownloadPercent = e.Progress * 100.0
            Me.Dispatcher.BeginInvoke(() => { UpdateUI(); })
        End Sub '   DownloadProgressChanged


        Dim sm_strXofYFormat As String  = ExpressionMediaPlayer.Resources.textOfflineProgressXOFYFormat

        ''' <summary>
        ''' Update the UI with the current progress status
        ''' </summary>
        '''
        internal Sub UpdateUI()

            Me.m_progressBarThisFile.Value = Me.DownloadPercent

            If (Me.CurrentItem > Me.TotalItemCount) Then
                Me.TotalItemCount = Me.CurrentItem
            End If


            UpdateXOfY()
        End Sub '   UpdateUI


        Private timeOfLastTotalCountUpdate As DateTime = DateTime.MinValue

        internal Sub IncrementTotalItemCount()

            Me.TotalItemCount += 1

            If ((DateTime.Now - timeOfLastTotalCountUpdate).TotalMilliseconds > 200) Then
                Me.Dispatcher.BeginInvoke(() => { UpdateXOfY(); })
                timeOfLastTotalCountUpdate = DateTime.Now
            End If
        End Sub '   IncrementTotalItemCount

        private Sub UpdateXOfY()


            If (m_progressBarOfflining  IsNot Nothing  AndAlso  Me.m_textBlockXofY  IsNot Nothing) Then
                Me.m_progressBarOfflining.Value = (100.0 * Me.CurrentItem) / Me.TotalItemCount

                Dim strXofY As String  = String.Format(CultureInfo.CurrentUICulture, sm_strXofYFormat, Me.CurrentItem.ToString(CultureInfo.CurrentUICulture), Me.TotalItemCount.ToString(CultureInfo.CurrentUICulture))

                Me.m_textBlockXofY.Text = strXofY
            End If
        End Sub '   UpdateXOfY

        ''' <summary>
        ''' Called downloader once all items have be downloaded.
        ''' </summary>
        internal Sub DownloadCompletedSuccessfully()

            Me.UpdateXOfY()
            Me.m_progressBarOfflining.Value = 100
            Me.m_textBlockDownloading.Visibility = Visibility.Collapsed
            Me.m_textBlockPromptToFinish.Visibility = Visibility.Visible
            Me.m_buttonCompleteInstallation.IsEnabled = true
        End Sub '   DownloadCompletedSuccessfully


        internal Sub InfoMessage(infoMessage As String)

            Me.ErrorMessage = infoMessage
            Me.Dispatcher.BeginInvoke(() =>
            {
                Me.m_textBlockErrorMessage.Visibility = Visibility.Visible

                If (string.IsNullOrEmpty(Me.m_textBlockErrorMessage.Text)) Then
                    Me.m_textBlockErrorMessage.Text = Me.ErrorMessage
                else
                    Me.m_textBlockErrorMessage.Text += ChrW(13) & ChrW(10)
 Handles Me.m_textBlockErrorMessage.Text
                End If

            })
        End Sub '   InfoMessage


        internal Sub ReportError(errorMessage As String)

            Me.ErrorOccured = true
            Me.ErrorMessage = errorMessage
            Me.Dispatcher.BeginInvoke(() => { DisplayErrorOnUIThread(); })
        End Sub '   ReportError


        internal Sub DisplayErrorOnUIThread()


            If (Me.ErrorOccured) Then
                Me.m_textBlockErrorMessage.Visibility = Visibility.Visible
                Me.m_textBlockErrorMessage.Text = Me.ErrorMessage
                Me.m_textBlockDownloading.Visibility = Visibility.Collapsed
                Me.m_textBlockPromptToFinish.Visibility = Visibility.Collapsed
                Me.m_buttonCompleteInstallation.IsEnabled = false
            End If
        End Sub '   DisplayErrorOnUIThread
    End Class   '   OfflineDownloadProgressDialog
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\OfflineDownloadProgressDialog.cs
