// <copyright file="LocalizedStrings.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>Exposes localized resources in a public class so that they may be referenced in XAML</summary>
// <author>Microsoft Expression Encoder Team</author>
using System;

namespace ExpressionMediaPlayer
{
    /// <summary>
    /// This class Exposes localized resources in a public class so that they may be referenced in XAML
    /// </summary>
    public class LocalizedStrings
    {
        /// <summary>
        /// Initializes a new instance of the LocalizedStrings class.
        /// </summary>
        public LocalizedStrings() { }

        /// <summary>
        /// private reference to the localized resources
        /// </summary>
        private static Resources localizedStrings = new Resources();

        /// <summary>
        /// public property that provides XAML access to the localized strings
        /// </summary>
        public Resources Strings { get { return localizedStrings; } }
    }
}
