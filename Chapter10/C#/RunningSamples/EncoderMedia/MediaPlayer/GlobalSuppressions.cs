// <copyright file="GlobalSuppressions.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project. 
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc. 
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File". 
// You do not need to add suppressions to this file manually. 
// </summary>
// <author>Microsoft Expression Encoder Team</author>

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1014:MarkAssembliesWithClsCompliant")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1824:MarkAssembliesWithNeutralResourcesLanguage")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Scope = "type", Target = "ExpressionMediaPlayer.InvalidPlaylistException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Scope = "type", Target = "ExpressionMediaPlayer.MediaPlayer")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "ExpressionMediaPlayer.MediaPlayer.#.ctor()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member", Target = "ExpressionMediaPlayer.MediaPlayer.#HookHandlers()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Scope = "member", Target = "ExpressionMediaPlayer.MediaPlayer.#MediaPlayerKeyDown(System.Object,System.Windows.Input.KeyEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Scope = "member", Target = "ExpressionMediaPlayer.MediaPlayer.#OnStartup(System.Object,System.Windows.StartupEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Stop", Scope = "member", Target = "ExpressionMediaPlayer.MediaPlayer.#Stop()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Textblock", Scope = "member", Target = "ExpressionMediaPlayer.MediaPlayer.#TextblockErrorMessage")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Scope = "member", Target = "ExpressionMediaPlayer.MediaPlayer.#UnEscape(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Scope = "member", Target = "ExpressionMediaPlayer.Playlist.#Items")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "ExpressionMediaPlayer.Playlist.#Serialize(System.Xml.XmlWriter)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Scope = "member", Target = "ExpressionMediaPlayer.PlaylistItem.#Chapters")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "ExpressionMediaPlayer.PlaylistItem.#CreateNewChapterItem()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "ExpressionMediaPlayer.PlaylistItem.#Serialize(System.Xml.XmlWriter)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Smpte", Scope = "member", Target = "ExpressionMediaPlayer.PlaylistItem.#SmpteFrameRate")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Scope = "member", Target = "ExpressionMediaPlayer.ScriptableTimelineMarker.#Type")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances", Scope = "type", Target = "ExpressionMediaPlayer.ScriptableTimelineMarkerRoutedEventHandler")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Smpte", Scope = "type", Target = "ExpressionMediaPlayer.SmpteFrameRate")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Smpte", Scope = "member", Target = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte2398")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Smpte", Scope = "member", Target = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte24")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Smpte", Scope = "member", Target = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte25")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Smpte", Scope = "member", Target = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte2997Drop")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Smpte", Scope = "member", Target = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte2997NonDrop")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Smpte", Scope = "member", Target = "ExpressionMediaPlayer.SmpteFrameRate.#Smpte30")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Smpte", Scope = "member", Target = "ExpressionMediaPlayer.TimeCode.#Smpte12MToTicks27Mhz(System.String,ExpressionMediaPlayer.SmpteFrameRate)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Smpte", Scope = "member", Target = "ExpressionMediaPlayer.TimeCode.#Ticks27MhzToSmpte12M(System.Int64,ExpressionMediaPlayer.SmpteFrameRate)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Tb", Scope = "member", Target = "ExpressionMediaPlayer.TimeCode.#ToTicksPcrTb()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pcr", Scope = "member", Target = "ExpressionMediaPlayer.TimeCode.#ToTicksPcrTb()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Smpte", Scope = "member", Target = "ExpressionMediaPlayer.TimeCode.#ValidateSmpte12MTimeCode(System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Smpte", Scope = "member", Target = "ExpressionMediaPlayer.TimeCode.#ValidateSmpte12MTimeCode(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Scope = "member", Target = "ExpressionMediaPlayer.DownloadProgressBar.#OnDownloadProgressOffsetPropertyChanged(System.Windows.DependencyObject,System.Windows.DependencyPropertyChangedEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads", Scope = "member", Target = "ExpressionMediaPlayer.IsoUri.#.ctor(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "ExpressionMediaPlayer.MediaPlayer.#CreateUri(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Scope = "member", Target = "ExpressionMediaPlayer.MediaPlayer.#CreateUri(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope = "member", Target = "ExpressionMediaPlayer.MediaPlayer.#ClickButton(System.Windows.Controls.Primitives.ButtonBase,System.Windows.RoutedEventHandler)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member", Target = "ExpressionMediaPlayer.MediaPlayer.#MediaPlayerKeyDown(System.Object,System.Windows.Input.KeyEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Scope = "member", Target = "ExpressionMediaPlayer.SensitiveSlider.#OnDragCompleted(System.Object,System.Windows.Controls.Primitives.DragCompletedEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Scope = "member", Target = "ExpressionMediaPlayer.SensitiveSlider.#OnDragStarted(System.Object,System.Windows.Controls.Primitives.DragStartedEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Scope = "member", Target = "ExpressionMediaPlayer.SensitiveSlider.#OnMouseClick(System.Object,System.Windows.Input.MouseButtonEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "MediaPlayerExtensions")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Microsoft.Expression.Encoder.PlugInLoader")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "ExpressionMediaPlayer.ChapterItem.#ExpressionMediaPlayer.IAccessible.AccessibilityText")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "ExpressionMediaPlayer.OfflineDownloadProgressDialog.#CancelWasClicked")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "ExpressionMediaPlayer.OfflineDownloadProgressDialog.#Click_CancelOfflineDownload(System.Object,System.Windows.RoutedEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "ExpressionMediaPlayer.OfflineDownloadProgressDialog.#Click_CompleteInstallation(System.Object,System.Windows.RoutedEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "ExpressionMediaPlayer.OfflineDownloadProgressDialog.#LayoutRoot")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "ExpressionMediaPlayer.PlaylistItem.#ExpressionMediaPlayer.IAccessible.AccessibilityText")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "ExpressionMediaPlayer.MediaPlayer.#ParseStartupParameters(System.Windows.StartupEventArgs)")]
