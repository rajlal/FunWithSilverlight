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

Partial Public Class PageIsolatedStore
    Inherits UserControl

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

        ReadIsolatedStoreTreeView()
    End Sub '   LayoutRoot_Loaded

    Private Sub AddFile(sender As Object, e As RoutedEventArgs)

        Try
            Dim store As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()

            Dim counter As Integer = store.GetFileNames().Length + 1
            Dim rootFile As IsolatedStorageFileStream = store.CreateFile("File" + CStr(counter) + ".txt")

            rootFile.Close()
            ReadIsolatedStoreTreeView()
        Catch ex As IsolatedStorageException
            StatusBar.Text = "Unable to access store"
        End Try
    End Sub '   AddFile

    Private Sub ClearAll(sender As Object, e As RoutedEventArgs)

        Try
            Dim store As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()
            store.Remove()
            ReadIsolatedStoreTreeView()
        Catch ex As IsolatedStorageException
            StatusBar.Text = "Unable to access store"
        End Try
    End Sub '   ClearAll

    Private Sub GetQuota(sender As Object, e As RoutedEventArgs)

        Try
            Dim store As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()
            StatusQuota.Text = "Quota: " +
                                String.Format("{0:###,###}", store.AvailableFreeSpace / 1024) + " KB /" +
                                String.Format("{0:###,###}", store.Quota / 1024) + " KB"
        Catch ex As IsolatedStorageException
            StatusBar.Text = "Unable to access store"
        End Try
    End Sub '   GetQuota

    Private Sub IncreaseQuota(sender As Object, e As RoutedEventArgs)

        Try
            Dim store As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()
            Dim addQuota As Int64 = 1024000
            Dim availableQuota As Int64 = store.AvailableFreeSpace

            '  Increase quota
            If (store.IncreaseQuotaTo(store.Quota + addQuota)) Then
                StatusBar.Text = "Quota increased by 1 MB"
                GetQuota(Nothing, Nothing)
            Else
                StatusBar.Text = "Quota not increased "
            End If
        Catch ex As IsolatedStorageException
            StatusBar.Text = "Unable to access store"
        End Try
    End Sub '   IncreaseQuota

    Private Sub ResetQuota(sender As Object, e As RoutedEventArgs)

        StatusBar.Text = "AppData\\LocalLow\\Microsoft\\Silverlight\\is"

        Dim t As ToolTip = New ToolTip()

        t.Content = "To Reset Quota Delete \n(User\\AppData\\LocalLow\\Microsoft\\Silverlight\\is) \nfolder"
        ToolTipService.SetToolTip(StatusBar, t)
        HtmlPage.Window.Alert("To Reset Quota Delete \n(User\\AppData\\LocalLow\\Microsoft\\Silverlight\\is) \nfolder")
    End Sub '   ResetQuota

    Private Sub AddDirectory(sender As Object, e As RoutedEventArgs)

        Try
            Dim store As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()

            Dim counter As Integer = store.GetDirectoryNames().Length + 1

            store.CreateDirectory("Folder" + counter.ToString())
            ReadIsolatedStoreTreeView()
        Catch ex As IsolatedStorageException
            StatusBar.Text = "Unable to access store"
        Catch ex As Exception
            StatusBar.Text = "Error Not "
        End Try
    End Sub '   AddDirectory

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

                If (dir = "Folder1") Then
                    Dim searchpath As String = Path.Combine("Folder1", "*.*")

                    Dim filesInSubDirs As String() = store.GetFileNames(searchpath)

                    For Each fileName As String In filesInSubDirs
                        tv1.Items.Add(fileName)
                    Next    '   fileName
                End If

                tv1.IsExpanded = True
                treeIsolatedRoot.Items.Add(tv1)
            Next    '   dir

            For Each fileName As String In filesInTheRoot

                Dim tv1 As TreeViewItem = New TreeViewItem()
                tv1.Name = fileName
                tv1.Header = fileName
                treeIsolatedRoot.Items.Add(tv1)
            Next    '   fileName
        Catch ex As IsolatedStorageException
            StatusBar.Text = "Unable to access store"
        End Try
    End Sub '   ReadIsolatedStoreTreeView
    Private Sub AddFileFolder(sender As Object, e As RoutedEventArgs)

        Try
            Dim store As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()

            If (store.GetDirectoryNames().Length = 0) Then
                AddDirectory(Nothing, Nothing)
            End If

            Dim subFiles As String() = store.GetFileNames(Path.Combine("Folder1", "*"))

            Dim counter As Integer = subFiles.Length + 1
            Dim subFolderFile As IsolatedStorageFileStream = store.CreateFile(Path.Combine("Folder1", "Subfile" + CStr(counter) + ".txt"))

            subFolderFile.Close()
            ReadIsolatedStoreTreeView()
        Catch ex As IsolatedStorageException
            StatusBar.Text = "Unable to access store"
        Catch ex As Exception
            StatusBar.Text = "Error Not "
        End Try
    End Sub '   AddFileFolder
End Class   '   PageIsolatedStore
' End Namespace   '   LocalStorage
' ..\Project_05\ExtendBrowser\3-IsolatedStorage\Pages\2-PageIsolatedStore.xaml.cs
