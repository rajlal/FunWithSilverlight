namespace ExpressionMediaPlayer
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO.IsolatedStorage;
    using System.Windows;
    using System.Windows.Browser;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    [TemplatePart(Name = OfflineDownloadProgressDialog.ButtonCancelOfflineDownload, Type = typeof(ButtonBase))]
    [TemplatePart(Name = OfflineDownloadProgressDialog.ButtonCompleteInstallation, Type = typeof(ButtonBase))]
    [TemplatePart(Name = OfflineDownloadProgressDialog.ProgressBarThisFile, Type = typeof(ProgressBar))]
    [TemplatePart(Name = OfflineDownloadProgressDialog.ProgressBarOfflining, Type = typeof(ProgressBar))]
    [TemplatePart(Name = OfflineDownloadProgressDialog.TextBlockXofY, Type = typeof(TextBlock))]
    [TemplatePart(Name = OfflineDownloadProgressDialog.TextBlockDownloading, Type = typeof(TextBlock))]
    [TemplatePart(Name = OfflineDownloadProgressDialog.TextBlockPromptToFinish, Type = typeof(TextBlock))]
    [TemplatePart(Name = OfflineDownloadProgressDialog.TextBlockErrorMessage, Type = typeof(TextBlock))]
    public partial class OfflineDownloadProgressDialog : Control
    {
        private Popup parent;
        private Grid grid;
        private MediaPlayer player;

        /// <summary>
        /// The percentage of the current file that has been downloaded
        /// </summary>
        internal double DownloadPercent { get; set; }
        /// <summary>
        /// The total number of items to be downloaded
        /// </summary>
        internal int TotalItemCount { get; private set; }
        /// <summary>
        /// The current item just downloaded
        /// </summary>
        internal int CurrentItem { get; set; }
        /// <summary>
        /// The flag set when the user clicks the cancel button
        /// </summary>
        internal bool CancelWasClicked { get; private set; }

        /// <summary>
        /// The flag set when the background thread encounters a error
        /// </summary>
        internal bool ErrorOccured { get; private set; }

        /// <summary>
        /// The error message set when the background thread encounters a error
        /// </summary>
        internal string ErrorMessage { get; private set; }

        public OfflineDownloadProgressDialog(MediaPlayer player)
        {
            this.player = player;
            this.DefaultStyleKey = typeof(OfflineDownloadProgressDialog);
        }

        /// <summary>
        /// String for the start button template element.
        /// </summary>
        protected const string ButtonCancelOfflineDownload = "buttonCancelOfflineDownload";
        protected const string ButtonCompleteInstallation = "buttonCompleteInstallation";
        protected const string ProgressBarThisFile = "progressBarThisFile";
        protected const string ProgressBarOfflining = "progressBarOfflining";

        protected const string TextBlockXofY = "textBlockXofY";

        protected const string TextBlockDownloading = "textBlockDownloading";
        protected const string TextBlockPromptToFinish = "textBlockPromptToFinish";
        protected const string TextBlockErrorMessage = "textBlockErrorMessage";

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            GetTemplateChildren();
            HookHandlers();
        }

        /// <summary>
        /// Gets the child elements of the template.
        /// </summary>
        protected virtual void GetTemplateChildren()
        {
            m_buttonCancelOfflineDownload = GetTemplateChild(ButtonCancelOfflineDownload) as ButtonBase;
            m_buttonCompleteInstallation = GetTemplateChild(ButtonCompleteInstallation) as ButtonBase;

            m_progressBarThisFile = GetTemplateChild(ProgressBarThisFile) as ProgressBar;
            m_progressBarOfflining = GetTemplateChild(ProgressBarOfflining) as ProgressBar;

            m_textBlockXofY = GetTemplateChild(TextBlockXofY) as TextBlock;

            m_textBlockDownloading = GetTemplateChild(TextBlockDownloading) as TextBlock;
            m_textBlockPromptToFinish = GetTemplateChild(TextBlockPromptToFinish) as TextBlock;
            m_textBlockErrorMessage = GetTemplateChild(TextBlockErrorMessage) as TextBlock;
        }

        /// <summary>
        /// Hooks our event handlers.
        /// </summary>
        protected virtual void HookHandlers()
        {
            if (m_buttonCancelOfflineDownload != null)
            {
                m_buttonCancelOfflineDownload.Click += Click_CancelOfflineDownload;
            }

            if (m_buttonCompleteInstallation != null)
            {
                m_buttonCompleteInstallation.Click += Click_CompleteInstallation;
            }
        }

        ButtonBase m_buttonCancelOfflineDownload;
        ButtonBase m_buttonCompleteInstallation;

        ProgressBar m_progressBarThisFile;
        ProgressBar m_progressBarOfflining;

        TextBlock m_textBlockXofY;

        TextBlock m_textBlockDownloading;
        TextBlock m_textBlockPromptToFinish;
        TextBlock m_textBlockErrorMessage;

        /// <summary>
        /// Browser window resize event handler
        /// </summary>
        private void OnPluginSizeChanged(object sender, EventArgs e)
        {
            UpdateSize();
        }

        /// <summary>
        /// Resizes the grid container -- effectively centering the dialog in the application window
        /// </summary>
        private void UpdateSize()
        {
            this.grid.Width = Application.Current.Host.Content.ActualWidth;
            this.grid.Height = Application.Current.Host.Content.ActualHeight;
        }

        /// <summary>
        /// Respond to the user clicking the cancel button
        /// </summary> 
        private void Click_CancelOfflineDownload(object sender, RoutedEventArgs e)
        {
            this.CancelWasClicked = true;
            this.m_buttonCancelOfflineDownload.IsEnabled = false;
            if (this.m_buttonCompleteInstallation.IsEnabled) // Download was completed -- the user is cancelling the final install 
            {
                Close();
            }
            this.player.SetOfflineButtonEnabled(true);
        }

        /// <summary>
        /// Respond to the user clicking the finish installation button
        /// </summary> 
        private void Click_CompleteInstallation(object sender, RoutedEventArgs e)
        {
            string errorMessage = string.Empty;
            bool installSuccess = false;
            try
            {
                // actually take the app offline.
                installSuccess = Application.Current.Install();
                Debug.WriteLine("Application.Current.Install() result=" + installSuccess.ToString());
                if (installSuccess)
                {
                    this.player.SetOfflineButtonVisibility(Visibility.Collapsed);
                    Close();
                }
            }
            catch (InvalidOperationException exp)
            {
                Debug.WriteLine("Exception attempting to install offline" + exp.ToString());
                errorMessage = exp.ToString();
            }
            catch (Exception exp)
            {
                Debug.WriteLine("Exception attempting to install offline" + exp.ToString());
                errorMessage = exp.ToString();
            }       
            if (!installSuccess)
            {
                // Failed for some reason -- let them try again ?
                this.player.SetOfflineButtonEnabled(true);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    this.ReportError(errorMessage);
                }
            }
        }

        /// <summary>
        /// Displays this popup window
        /// </summary>
        internal void Show()
        {
            if (this.parent == null)
            {
                this.parent = new Popup();
                this.grid = new Grid();
                grid.Children.Add(this);
                this.parent.Child = grid;
                Application.Current.Host.Content.Resized += OnPluginSizeChanged;
                this.parent.IsOpen = true;
                if (this.m_buttonCompleteInstallation != null)
                {
                    this.m_buttonCompleteInstallation.IsEnabled = false;
                }
                UpdateSize();
            }
        }

        /// <summary>
        /// Shuts down this popup window
        /// </summary>
        internal void Close()
        {
            if (this.CancelWasClicked)
            {
                try
                {
                    IsolatedStorageFile.GetUserStoreForApplication().Remove(); // Zap all those files just downloaded.
                }
                catch (IsolatedStorageException iso)
                {
                    Debug.WriteLine("IsolatedStorageException clearing out data during cancel" + iso.Message);
                    ReportError(iso.Message);
                }
            }

            if (this.parent != null)
            {
                this.parent.IsOpen = false;
                Application.Current.Host.Content.Resized -= OnPluginSizeChanged;
                this.grid.Children.Remove(this);
                this.parent.Child = null;
                this.parent = null;
            }
        }

        /// <summary>
        /// downloading playlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void DownloadProgressChanged(object sender, PlaylistDownloadProgressEventArgs e)
        {
            this.DownloadPercent = e.Progress * 100.0;
            this.Dispatcher.BeginInvoke(() => { UpdateUI(); });
        }

        static string sm_strXofYFormat = ExpressionMediaPlayer.Resources.textOfflineProgressXOFYFormat;

        /// <summary>
        /// Update the UI with the current progress status
        /// </summary>
        /// 
        internal void UpdateUI()
        {
            this.m_progressBarThisFile.Value = this.DownloadPercent;
            if (this.CurrentItem > this.TotalItemCount)
            {
                this.TotalItemCount = this.CurrentItem;
            }

            UpdateXOfY();
        }

        private DateTime timeOfLastTotalCountUpdate = DateTime.MinValue;

        internal void IncrementTotalItemCount()
        {
            this.TotalItemCount++;
            if ((DateTime.Now - timeOfLastTotalCountUpdate).TotalMilliseconds > 200)
            {
                this.Dispatcher.BeginInvoke(() => { UpdateXOfY(); });
                timeOfLastTotalCountUpdate = DateTime.Now;
            }
        }

        private void UpdateXOfY()
        {
            if (m_progressBarOfflining != null && this.m_textBlockXofY != null)
            {
                this.m_progressBarOfflining.Value = (100.0 * this.CurrentItem) / this.TotalItemCount;


                string strXofY = String.Format(CultureInfo.CurrentUICulture, sm_strXofYFormat, this.CurrentItem.ToString(CultureInfo.CurrentUICulture), this.TotalItemCount.ToString(CultureInfo.CurrentUICulture));

                this.m_textBlockXofY.Text = strXofY;
            }
        }

        /// <summary>
        /// Called downloader once all items have be downloaded.
        /// </summary> 
        internal void DownloadCompletedSuccessfully()
        {
            this.UpdateXOfY();
            this.m_progressBarOfflining.Value = 100;
            this.m_textBlockDownloading.Visibility = Visibility.Collapsed;
            this.m_textBlockPromptToFinish.Visibility = Visibility.Visible;
            this.m_buttonCompleteInstallation.IsEnabled = true;
        }

        internal void InfoMessage(string infoMessage)
        {
            this.ErrorMessage = infoMessage;
            this.Dispatcher.BeginInvoke(() =>
            {
                this.m_textBlockErrorMessage.Visibility = Visibility.Visible;
                if (string.IsNullOrEmpty(this.m_textBlockErrorMessage.Text))
                {
                    this.m_textBlockErrorMessage.Text = this.ErrorMessage;
                }
                else
                {
                    this.m_textBlockErrorMessage.Text += "\r\n";
                    this.m_textBlockErrorMessage.Text += this.ErrorMessage;
                }
            });
        }

        internal void ReportError(string errorMessage)
        {
            this.ErrorOccured = true;
            this.ErrorMessage = errorMessage;
            this.Dispatcher.BeginInvoke(() => { DisplayErrorOnUIThread(); });
        }

        internal void DisplayErrorOnUIThread()
        {
            if (this.ErrorOccured)
            {
                this.m_textBlockErrorMessage.Visibility = Visibility.Visible;
                this.m_textBlockErrorMessage.Text = this.ErrorMessage;
                this.m_textBlockDownloading.Visibility = Visibility.Collapsed;
                this.m_textBlockPromptToFinish.Visibility = Visibility.Collapsed;
                this.m_buttonCompleteInstallation.IsEnabled = false;
            }
        }
    }
}
                