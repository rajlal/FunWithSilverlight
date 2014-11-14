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
Imports System.Windows.Resources

' Namespace LocalStorage

Partial Public Class PageIsolatedEmbeddedFiles
    Inherits UserControl

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

        ReadIsolatedStoreTreeView()
    End Sub '   LayoutRoot_Loaded

    Private Sub ClearAll(sender As Object, e As RoutedEventArgs)

        Try
            Dim store As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()
            store.Remove()
            treeIsolatedRoot.Items.Clear()

        Catch ex As IsolatedStorageException

            StatusBar.Text = "Error while accessing store"
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

    Private Sub TextBlock_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        Dim t As TextBlock = CType(sender, TextBlock)

        SaveSilverlightFileIsolatedStore(t.Text)
    End Sub '   TextBlock_MouseLeftButtonUp

    Private Sub SaveSilverlightFileIsolatedStore(fname As String)

        Try

            Dim store As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()

            '  Load an image resource file embedded in the application assembly.
            Dim uri As Uri = New Uri("LocalStorage;component/Files/" + fname, UriKind.Relative)

            Dim sri As StreamResourceInfo = Application.GetResourceStream(Uri)

            Dim fileStream As Stream = sri.Stream
            Dim fileLength As Integer = CType(fileStream.Length, Integer)

            If (fileLength > store.AvailableFreeSpace) Then
                HtmlPage.Window.Alert("Please increase the Quota  Not ")
            Else
                Dim cAData As Byte()
                ReDim cAData(0 To fileLength - 1)
                fileStream.Read(cAData, 0, fileLength)
                fileStream.Close()

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
End Class   '   PageIsolatedEmbeddedFiles
' End Namespace   '   LocalStorage
' ..\Project_05\ExtendBrowser\3-IsolatedStorage\Pages\4-PageIsolatedEmbeddedFiles.xaml.cs
