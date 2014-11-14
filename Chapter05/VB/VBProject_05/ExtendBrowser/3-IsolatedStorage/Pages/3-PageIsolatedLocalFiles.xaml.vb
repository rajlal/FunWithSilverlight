Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Collections
Imports System.Text
Imports System.Windows.Browser
Imports System.Windows.Media.Imaging
Imports Microsoft.Windows.Controls

' Namespace LocalStorage

    Public Partial Class PageIsolatedLocalFiles
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub '   New

        private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

            ReadIsolatedStoreTreeView()
        End Sub '   LayoutRoot_Loaded

        private Sub OpenFileDialog()

            videoContainer.Stop()
            videoContainer.Visibility = Visibility.Collapsed
            textContainer.Visibility = Visibility.Collapsed
        imageContainer.Visibility = Visibility.Collapsed

        Dim dialog As OpenFileDialog = New OpenFileDialog()

        dialog.Multiselect = False
        dialog.Filter = "All files (*.*)|*.*|Text files (txt)|*.txt|XAML files (xaml)|*.xaml|Image files (jpg, png)|*.jpg;*.png|Windows Media Video (wmv)|*.wmv"
        dialog.FilterIndex = 1

        If (dialog.ShowDialog().Value) Then
            Dim fileLocal As FileInfo = dialog.File

            If (fileLocal.Name.Length > 20) Then
                StatusBar.Text = "Added: " + fileLocal.Name.Substring(0, 8) + "..." + fileLocal.Name.Substring(fileLocal.Name.Length - 8)
            Else
                StatusBar.Text = "Added: " + fileLocal.Name
            End If
            lblFilename.Text = "Preview"

            Dim t As ToolTip = New ToolTip()

            t.Content = fileLocal.Name
            ToolTipService.SetToolTip(lblFilename, t)
            Try
                Select Case (dialog.File.Extension.ToLower())

                    Case ".txt"
                        SaveToIsolatedStore(fileLocal, "text")
                        Return
                    Case ".xaml"
                        SaveToIsolatedStore(fileLocal, "text")
                        Return
                    Case ".jpg"
                        SaveToIsolatedStore(fileLocal, "image")
                        Return
                    Case ".jpeg"
                        SaveToIsolatedStore(fileLocal, "image")
                        Return
                    Case ".png"
                        SaveToIsolatedStore(fileLocal, "image")
                        Return
                    Case ".wmv"
                        SaveToIsolatedStore(fileLocal, "video")
                        Return
                    Case Else
                        SaveToIsolatedStore(fileLocal, "text")
                        Return
                End Select '    dialog.File.Extension.ToLower(
            Catch ex As Exception
                HtmlPage.Window.Alert("Error  Not ")
            End Try
        End If
    End Sub '   OpenFileDialog

    Private Sub SaveToIsolatedStore(f As FileInfo, type As String)

        updatePreview(f, type)

        Try

            Dim store As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()

            Dim ofileStream As Stream = f.OpenRead()
            Dim fileLength As Integer = CType(ofileStream.Length, Integer)

            If (fileLength > store.AvailableFreeSpace) Then
                HtmlPage.Window.Alert("Please increase the Quota  Not ")
            Else
                Dim cAData As Byte()
                ReDim cAData(0 To fileLength - 1)
                ofileStream.Read(cAData, 0, fileLength)
                ofileStream.Close()

                Dim mediaFile As IsolatedStorageFileStream = store.CreateFile(f.Name)

                mediaFile.Write(cAData, 0, fileLength)
                mediaFile.Close()
                ReadIsolatedStoreTreeView()
                mediaFile.Close()
            End If
        Catch ex As IsolatedStorageException
            StatusBar.Text = "Error while accessing store"
        End Try
    End Sub '   SaveToIsolatedStore
    Private Sub ClearAll(sender As Object, e As RoutedEventArgs)

        Try
            Dim store As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()
            store.Remove()
            treeIsolatedRoot.Items.Clear()

        Catch ex As IsolatedStorageException

            '  StatusBar.Text = "Error while accessing store"
        End Try
    End Sub '   ClearAll
    Private Sub GetQuota(sender As Object, e As RoutedEventArgs)

        Try
            Dim store As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()
            StatusQuota.Text = "Quota: " + String.Format("{0:###,###}", store.AvailableFreeSpace / 1024) + " KB /" + String.Format("{0:###,###}", store.Quota / 1024) + " KB"

        Catch ex As IsolatedStorageException

            StatusBar.Text = "Unable to access store"
        End Try
    End Sub '   GetQuota
    Private Sub IncreaseQuota(sender As Object, e As RoutedEventArgs)

        IncreaseQuotaBy(5)
    End Sub '   IncreaseQuota

    Private Sub IncreaseQuotaBy(sizeinMB As Integer)

        Try
            Dim store As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()
            Dim addQuota As Int64 = 1024000 * sizeinMB
            Dim availableQuota As Int64 = store.AvailableFreeSpace

            '  Increase quota

            If (store.IncreaseQuotaTo(store.Quota + addQuota)) Then
                StatusBar.Text = "Quota increased by " + CStr(sizeinMB) + " MB"
                GetQuota(Nothing, Nothing)
            Else
                StatusBar.Text = "Quota not increased "
            End If
        Catch ex As IsolatedStorageException
            StatusBar.Text = "Unable to access store"
        End Try
    End Sub '   IncreaseQuotaBy

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)

        OpenFileDialog()
    End Sub '   Button_Click

    Private Sub ReadIsolatedStoreTreeView()

        Try

            Dim store As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()

            treeIsolatedRoot.Items.Clear()
            '  Gather file information
            Dim directoriesInTheRoot As String() = store.GetDirectoryNames()
            Dim filesInTheRoot As String() = store.GetFileNames()

            For Each dir As String In directoriesInTheRoot

                Dim tv1 As TreeViewItem = New TreeViewItem()
                tv1.Name = dir
                tv1.Header = "(" + dir + ")"
                tv1.IsExpanded = True
                treeIsolatedRoot.Items.Add(tv1)
            Next    '   dir

            For Each fileName As String In filesInTheRoot

                Dim lfilename As String

                If (fileName.Length > 20) Then
                    lfilename = fileName.Substring(0, 8) + "..." + fileName.Substring(fileName.Length - 8)
                Else
                    lfilename = fileName
                End If

                Dim tv1 As TreeViewItem = New TreeViewItem()
                tv1.Name = fileName
                tv1.Header = lfilename
                tv1.Cursor = Cursors.Hand
                treeIsolatedRoot.Items.Add(tv1)
            Next    '   fileName

            StatusQuota.Text = "Quota: " + String.Format("{0:###,###}", store.AvailableFreeSpace / 1024) + " KB /" + String.Format("{0:###,###}", store.Quota / 1024) + " KB"
        Catch ex As IsolatedStorageException

            StatusBar.Text = "Unable to access store"
        End Try
    End Sub '   ReadIsolatedStoreTreeView

    Private Sub treeIsolated_SelectedItemChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Object))

        Dim tv1 As TreeViewItem = New TreeViewItem()
        tv1 = CType(treeIsolated.SelectedItem, TreeViewItem)

        Dim tv1Name As String = tv1.Name

        If (tv1Name.ToLower().EndsWith(".jpg") OrElse tv1Name.ToLower().EndsWith(".jpeg") OrElse tv1Name.ToLower().EndsWith(".png")) Then
            DisplayfromIsolatedStorage(tv1Name, "image")
        ElseIf (tv1Name.ToLower().EndsWith(".wmv")) Then
            DisplayfromIsolatedStorage(tv1Name, "video")
        Else
            DisplayfromIsolatedStorage(tv1Name, "text")
        End If
    End Sub '   treeIsolated_SelectedItemChanged

    Private Sub DisplayfromIsolatedStorage(fName As String, type As String)

        videoContainer.Stop()
        videoContainer.Visibility = Visibility.Collapsed
        imageContainer.Visibility = Visibility.Collapsed
        textContainer.Visibility = Visibility.Collapsed

        Dim localfName As String = ""

        If (fName.Length > 20) Then
            localfName = fName.Substring(0, 8) + "..." + fName.Substring(fName.Length - 8)
        Else
            localfName = fName
        End If

        Dim store As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()

        If (store.FileExists(fName)) Then
            If (type = "image") Then

                Dim isoStream As Stream = store.OpenFile(fName, FileMode.Open, FileAccess.Read)
                Dim bi As BitmapImage = New BitmapImage()

                bi.SetSource(isoStream)
                imageContainer.Source = bi
                imageContainer.Visibility = Visibility.Visible
            ElseIf (type = "video") Then

                Dim isoStream As Stream = store.OpenFile(fName, FileMode.Open, FileAccess.Read)

                videoContainer.SetSource(isoStream)
                videoContainer.Play()
                videoContainer.Visibility = Visibility.Visible
            Else

                Dim reader As StreamReader = New StreamReader(store.OpenFile(fName, FileMode.Open, FileAccess.Read))

                textContainer.Text = reader.ReadToEnd()
                textContainer.Visibility = Visibility.Visible
            End If

            StatusBar.Text = "Isolated Store:" + localfName
        Else
            StatusBar.Text = "File does not exists:" + localfName
        End If
    End Sub '   DisplayfromIsolatedStorage

    Private Sub updatePreview(f As FileInfo, type As String)

        videoContainer.Visibility = Visibility.Collapsed
        imageContainer.Visibility = Visibility.Collapsed
        textContainer.Visibility = Visibility.Collapsed

        If (type = "image") Then

            Dim oimage As BitmapImage = New BitmapImage()

            oimage.SetSource(f.OpenRead())
            imageContainer.Source = oimage
            imageContainer.Visibility = Visibility.Visible
        ElseIf (type = "video") Then
            videoContainer.SetSource(f.OpenRead())
            videoContainer.Play()
            videoContainer.Visibility = Visibility.Visible
        Else
            Dim txtS As StreamReader = f.OpenText()

            textContainer.Text = txtS.ReadToEnd()
            textContainer.Visibility = Visibility.Visible
        End If
    End Sub '   updatePreview

    Private Sub SaveSilverlightFileIsolatedStore(fname As String, type As String)

        Try

            Dim store As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()
            Dim sr As StreamReader = New StreamReader("T.txt")

            Dim ofileStream As FileStream = New FileStream("Files/Silverlight.png", FileMode.Open, FileAccess.Read)
            ' Stream ofileStream = f.OpenRead()
            Dim fileLength As Integer = CType(ofileStream.Length, Integer)

            If (fileLength > store.AvailableFreeSpace) Then
                HtmlPage.Window.Alert("Please increase the Quota  Not ")
            Else
                Dim cAData As Byte()
                ReDim cAData(0 To fileLength - 1)
                ofileStream.Read(cAData, 0, fileLength)
                ofileStream.Close()

                Dim mediaFile As IsolatedStorageFileStream = store.CreateFile(fname)

                mediaFile.Write(cAData, 0, fileLength)
                mediaFile.Close()
                ReadIsolatedStoreTreeView()
                mediaFile.Close()
            End If
        Catch ex As IsolatedStorageException
            StatusBar.Text = "Error while accessing store"
        End Try
    End Sub '   SaveSilverlightFileIsolatedStore
End Class   '   PageIsolatedLocalFiles


' End Namespace   '   LocalStorage
' ..\Project_05\ExtendBrowser\3-IsolatedStorage\Pages\3-PageIsolatedLocalFiles.xaml.cs
