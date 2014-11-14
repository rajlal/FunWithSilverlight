// <copyright file="IAccessible.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>Implements the download progress bar class</summary>
// <author>Microsoft Expression Encoder Team</author>
namespace ExpressionMediaPlayer
{
    /// <summary>
    /// implement this if you want to provide screen reader info to your listbox items
    /// </summary>
    public interface IAccessible
    {
        string AccessibilityText { get; }        
    }
}
