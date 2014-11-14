'  <copyright file="GlobalSuppressions.cs" company="Microsoft">
'      Copyright � Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>
'  This file is used by Code Analysis to maintain SuppressMessage
'  attributes that are applied to this project.
'  Project-level suppressions either have no target or are given
'  a specific target and scoped to a namespace, type, member, etc.
'
'  To add a suppression to this file, right-click the message in the
'  Error List, point to "Suppress Message(s)", and click
'  "In Project Suppression File".
'  You do not need to add suppressions to this file manually.
'  </summary>
'  <author>Microsoft Expression Encoder Team</author>

<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1014:MarkAssembliesWithClsCompliant")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1824:MarkAssembliesWithNeutralResourcesLanguage")>
"type", Target = "ExpressionMediaPlayer.InvalidPlaylistException")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Scope = "ExpressionMediaPlayer.InvalidPlaylistException")>
"type", Target = "ExpressionMediaPlayer.MediaPlayer")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Scope = "ExpressionMediaPlayer.MediaPlayer")>
"member", Target = "ExpressionMediaPlayer.MediaPlayer.#.ctor()")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "ExpressionMediaPlayer.MediaPlayer.#.ctor()")>
"member", Target = "ExpressionMediaPlayer.MediaPlayer.#HookHandlers()")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "ExpressionMediaPlayer.MediaPlayer.#HookHandlers()")>
"member", Target = "ExpressionMediaPlayer.MediaPlayer.#MediaPlayerKeyDown(System.Object,System.Windows.Input.KeyEventArgs)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Scope = "ExpressionMediaPlayer.MediaPlayer.#MediaPlayerKeyDown(System.Object,System.Windows.Input.KeyEventArgs)")>
"member", Target = "ExpressionMediaPlayer.MediaPlayer.#OnStartup(System.Object,System.Windows.StartupEventArgs)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Scope = "ExpressionMediaPlayer.MediaPlayer.#OnStartup(System.Object,System.Windows.StartupEventArgs)")>
"member", Target = "ExpressionMediaPlayer.MediaPlayer.#Stop()")>
"Stop", Scope = "ExpressionMediaPlayer.MediaPlayer.#Stop()")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "ExpressionMediaPlayer.MediaPlayer.#Stop()")>
"member", Target = "ExpressionMediaPlayer.MediaPlayer.#TextblockErrorMessage")>
"Textblock", Scope = "ExpressionMediaPlayer.MediaPlayer.#TextblockErrorMessage")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ExpressionMediaPlayer.MediaPlayer.#TextblockErrorMessage")>
"member", Target = "ExpressionMediaPlayer.MediaPlayer.#UnEscape(System.String)")>
"Un", Scope = "ExpressionMediaPlayer.MediaPlayer.#UnEscape(System.String)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ExpressionMediaPlayer.MediaPlayer.#UnEscape(System.String)")>
"member", Target = "ExpressionMediaPlayer.Playlist.#Items")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Scope = "ExpressionMediaPlayer.Playlist.#Items")>
"member", Target = "ExpressionMediaPlayer.Playlist.#Serialize(System.Xml.XmlWriter)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "ExpressionMediaPlayer.Playlist.#Serialize(System.Xml.XmlWriter)")>
"member", Target = "ExpressionMediaPlayer.PlaylistItem.#Chapters")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Scope = "ExpressionMediaPlayer.PlaylistItem.#Chapters")>
"member", Target = "ExpressionMediaPlayer.PlaylistItem.#CreateNewChapterItem()")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "ExpressionMediaPlayer.PlaylistItem.#CreateNewChapterItem()")>
"member", Target = "ExpressionMediaPlayer.PlaylistItem.#Serialize(System.Xml.XmlWriter)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "ExpressionMediaPlayer.PlaylistItem.#Serialize(System.Xml.XmlWriter)")>
"member", Target = "ExpressionMediaPlayer.PlaylistItem.#SmpteFrameRate")>
"Smpte", Scope = "ExpressionMediaPlayer.PlaylistItem.#SmpteFrameRate")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ExpressionMediaPlayer.PlaylistItem.#SmpteFrameRate")>
"member", Target = "ExpressionMediaPlayer.ScriptableTimelineMarker.#Type")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Scope = "ExpressionMediaPlayer.ScriptableTimelineMarker.#Type")>
"type", Target = "ExpressionMediaPlayer.ScriptableTimelineMarkerRoutedEventHandler")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances", Scope = "ExpressionMediaPlayer.ScriptableTimelineMarkerRoutedEventHandler")>
"type", Target = "ExpressionMediaPlayer.SmpteFrameRate")>
"Smpte", Scope = "ExpressionMediaPlayer.SmpteFrameRate")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ExpressionMediaPlayer.SmpteFrameRate")>
"member", Target = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte2398")>
"Smpte", Scope = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte2398")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte2398")>
"member", Target = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte24")>
"Smpte", Scope = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte24")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte24")>
"member", Target = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte25")>
"Smpte", Scope = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte25")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte25")>
"member", Target = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte2997Drop")>
"Smpte", Scope = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte2997Drop")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte2997Drop")>
"member", Target = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte2997NonDrop")>
"Smpte", Scope = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte2997NonDrop")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte2997NonDrop")>
"member", Target = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte30")>
"Smpte", Scope = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte30")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte30")>
"member", Target = "ExpressionMediaPlayer.TimeCode.#Smpte12MToTicks27Mhz(System.String,ExpressionMediaPlayer.SmpteFrameRate)")>
"Smpte", Scope = "ExpressionMediaPlayer.TimeCode.#Smpte12MToTicks27Mhz(System.String,ExpressionMediaPlayer.SmpteFrameRate)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ExpressionMediaPlayer.TimeCode.#Smpte12MToTicks27Mhz(System.String,ExpressionMediaPlayer.SmpteFrameRate)")>
"member", Target = "ExpressionMediaPlayer.TimeCode.#Ticks27MhzToSmpte12M(System.Int64,ExpressionMediaPlayer.SmpteFrameRate)")>
"Smpte", Scope = "ExpressionMediaPlayer.TimeCode.#Ticks27MhzToSmpte12M(System.Int64,ExpressionMediaPlayer.SmpteFrameRate)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ExpressionMediaPlayer.TimeCode.#Ticks27MhzToSmpte12M(System.Int64,ExpressionMediaPlayer.SmpteFrameRate)")>
"member", Target = "ExpressionMediaPlayer.TimeCode.#ToTicksPcrTb()")>
"Tb", Scope = "ExpressionMediaPlayer.TimeCode.#ToTicksPcrTb()")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ExpressionMediaPlayer.TimeCode.#ToTicksPcrTb()")>
"member", Target = "ExpressionMediaPlayer.TimeCode.#ToTicksPcrTb()")>
"Pcr", Scope = "ExpressionMediaPlayer.TimeCode.#ToTicksPcrTb()")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ExpressionMediaPlayer.TimeCode.#ToTicksPcrTb()")>
"member", Target = "ExpressionMediaPlayer.TimeCode.#ValidateSmpte12MTimeCode(System.Int32)")>
"Smpte", Scope = "ExpressionMediaPlayer.TimeCode.#ValidateSmpte12MTimeCode(System.Int32)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ExpressionMediaPlayer.TimeCode.#ValidateSmpte12MTimeCode(System.Int32)")>
"member", Target = "ExpressionMediaPlayer.TimeCode.#ValidateSmpte12MTimeCode(System.String)")>
"Smpte", Scope = "ExpressionMediaPlayer.TimeCode.#ValidateSmpte12MTimeCode(System.String)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ExpressionMediaPlayer.TimeCode.#ValidateSmpte12MTimeCode(System.String)")>
"member", Target = "ExpressionMediaPlayer.DownloadProgressBar.#OnDownloadProgressOffsetPropertyChanged(System.Windows.DependencyObject,System.Windows.DependencyPropertyChangedEventArgs)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Scope = "ExpressionMediaPlayer.DownloadProgressBar.#OnDownloadProgressOffsetPropertyChanged(System.Windows.DependencyObject,System.Windows.DependencyPropertyChangedEventArgs)")>
"member", Target = "ExpressionMediaPlayer.IsoUri.#.ctor(System.String)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads", Scope = "ExpressionMediaPlayer.IsoUri.#.ctor(System.String)")>
"member", Target = "ExpressionMediaPlayer.MediaPlayer.#CreateUri(System.String)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "ExpressionMediaPlayer.MediaPlayer.#CreateUri(System.String)")>
"member", Target = "ExpressionMediaPlayer.MediaPlayer.#CreateUri(System.String)")>
"0#", Scope = "ExpressionMediaPlayer.MediaPlayer.#CreateUri(System.String)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "ExpressionMediaPlayer.MediaPlayer.#CreateUri(System.String)")>
"member", Target = "ExpressionMediaPlayer.MediaPlayer.#ClickButton(System.Windows.Controls.Primitives.ButtonBase,System.Windows.RoutedEventHandler)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope = "ExpressionMediaPlayer.MediaPlayer.#ClickButton(System.Windows.Controls.Primitives.ButtonBase,System.Windows.RoutedEventHandler)")>
"member", Target = "ExpressionMediaPlayer.MediaPlayer.#MediaPlayerKeyDown(System.Object,System.Windows.Input.KeyEventArgs)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "ExpressionMediaPlayer.MediaPlayer.#MediaPlayerKeyDown(System.Object,System.Windows.Input.KeyEventArgs)")>
"member", Target = "ExpressionMediaPlayer.SensitiveSlider.#OnDragCompleted(System.Object,System.Windows.Controls.Primitives.DragCompletedEventArgs)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Scope = "ExpressionMediaPlayer.SensitiveSlider.#OnDragCompleted(System.Object,System.Windows.Controls.Primitives.DragCompletedEventArgs)")>
"member", Target = "ExpressionMediaPlayer.SensitiveSlider.#OnDragStarted(System.Object,System.Windows.Controls.Primitives.DragStartedEventArgs)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Scope = "ExpressionMediaPlayer.SensitiveSlider.#OnDragStarted(System.Object,System.Windows.Controls.Primitives.DragStartedEventArgs)")>
"member", Target = "ExpressionMediaPlayer.SensitiveSlider.#OnMouseClick(System.Object,System.Windows.Input.MouseButtonEventArgs)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Scope = "ExpressionMediaPlayer.SensitiveSlider.#OnMouseClick(System.Object,System.Windows.Input.MouseButtonEventArgs)")>
"namespace", Target = "MediaPlayerExtensions")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "MediaPlayerExtensions")>
"namespace", Target = "Microsoft.Expression.Encoder.PlugInLoader")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "Microsoft.Expression.Encoder.PlugInLoader")>
"member", Target = "ExpressionMediaPlayer.ChapterItem.#ExpressionMediaPlayer.IAccessible.AccessibilityText")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "ExpressionMediaPlayer.ChapterItem.#ExpressionMediaPlayer.IAccessible.AccessibilityText")>
"member", Target = "ExpressionMediaPlayer.OfflineDownloadProgressDialog.#CancelWasClicked")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "ExpressionMediaPlayer.OfflineDownloadProgressDialog.#CancelWasClicked")>
"member", Target = "ExpressionMediaPlayer.OfflineDownloadProgressDialog.#Click_CancelOfflineDownload(System.Object,System.Windows.RoutedEventArgs)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "ExpressionMediaPlayer.OfflineDownloadProgressDialog.#Click_CancelOfflineDownload(System.Object,System.Windows.RoutedEventArgs)")>
"member", Target = "ExpressionMediaPlayer.OfflineDownloadProgressDialog.#Click_CompleteInstallation(System.Object,System.Windows.RoutedEventArgs)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "ExpressionMediaPlayer.OfflineDownloadProgressDialog.#Click_CompleteInstallation(System.Object,System.Windows.RoutedEventArgs)")>
"member", Target = "ExpressionMediaPlayer.OfflineDownloadProgressDialog.#LayoutRoot")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "ExpressionMediaPlayer.OfflineDownloadProgressDialog.#LayoutRoot")>
"member", Target = "ExpressionMediaPlayer.PlaylistItem.#ExpressionMediaPlayer.IAccessible.AccessibilityText")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "ExpressionMediaPlayer.PlaylistItem.#ExpressionMediaPlayer.IAccessible.AccessibilityText")>
"member", Target = "ExpressionMediaPlayer.MediaPlayer.#ParseStartupParameters(System.Windows.StartupEventArgs)")>
<assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "ExpressionMediaPlayer.MediaPlayer.#ParseStartupParameters(System.Windows.StartupEventArgs)")>
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\GlobalSuppressions.cs
