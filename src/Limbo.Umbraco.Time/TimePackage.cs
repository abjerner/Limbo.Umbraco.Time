using System;
using System.Diagnostics;
using Umbraco.Cms.Core.Semver;

namespace Limbo.Umbraco.Time {

    /// <summary>
    /// Static class with various information and constants about the package.
    /// </summary>
    public static class TimePackage {

        /// <summary>
        /// Gets the alias of the package.
        /// </summary>
        public const string Alias = "Limbo.Umbraco.Time";

        /// <summary>
        /// Gets the friendly name of the package.
        /// </summary>
        public const string Name = "Limbo Time";

        /// <summary>
        /// Gets the version of the package.
        /// </summary>
        public static readonly Version Version = typeof(TimePackage).Assembly.GetName().Version;

        /// <summary>
        /// Gets the semantic version of the package.
        /// </summary>
        public static readonly SemVersion SemVersion = FileVersionInfo.GetVersionInfo(typeof(TimePackage).Assembly.Location).ProductVersion;

    }

}