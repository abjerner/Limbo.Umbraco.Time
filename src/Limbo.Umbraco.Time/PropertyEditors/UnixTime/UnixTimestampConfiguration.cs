using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.UnixTime {

    /// <summary>
    /// Configuration for <see cref="UnixTimestampEditor"/>.
    /// </summary>
    public class UnixTimestampConfiguration {

        /// <summary>
        /// Gets or sets the unique identifer of the time zone to be used.
        /// </summary>
        [ConfigurationField("timeZone", "Time zone", "/App_Plugins/Limbo.Umbraco.Time/Views/Editors/TimeZone.html", Description = "Select the time zone of the returned <strong>EssentialsTime</strong>. This does not affect the value saved in Umbraco.")]
        public string? TimeZone { get; set; }

        /// <summary>
        /// Gets or sets whether the UNIX timestamp should be shown to the user.
        /// </summary>
        [ConfigurationField("showUnixTimestamp", "Show UNIX timestamp", "boolean", Description = "Show the UNIX timestamp in the editor.")]
        public bool ShowUnixTimestampp { get; set; }

        /// <summary>
        /// Gets or sets whether the field is readonly.
        /// </summary>
        [ConfigurationField("readonly", "Readonly", "boolean", Description = "Specify whether the editor should be readonly.")]
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets whether the property value type should be a nullable type.
        /// </summary>
        [ConfigurationField("nullable", "Nullable", "boolean", Description = "Specify whether the property value type should be a nullable type.")]
        public bool IsNullable { get; set; } = true;

        /// <summary>
        /// Gets or sets .NET value type returned by properties using this data type.
        /// </summary>
        [ConfigurationField("valueType", "Value type", "/App_Plugins/Limbo.Umbraco.Time/Views/Editors/DateTimeValueType.html", Description = "Select the .NET value type returned by properties using this data type.")]
        public string? ValueType { get; set; }

    }

}