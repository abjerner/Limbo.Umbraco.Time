using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.Time {

    /// <summary>
    /// Represents the configuration for the time offset value editor.
    /// </summary>
    public class TimeConfiguration {

        /// <summary>
        /// Gets or sets .NET value type returned by properties using this data type.
        /// </summary>
        [ConfigurationField("valueType", "Value type", "/App_Plugins/Limbo.Umbraco.Time/Views/Editors/TimeValueType.html", Description = "Select the .NET value type returned by properties using this data type.")]
        public string? ValueType { get; set; }

        /// <summary>
        /// Gets or sets whether the value is nullable.
        /// </summary>
        [ConfigurationField("nullable", "Is nullable?", "boolean", Description = "Select whether the returned value is nullable.")]
        public bool IsNullable { get; set; } = true;

        /// <summary>
        /// Gets or sets the output format of the time offset value.
        /// </summary>
        [ConfigurationField("outputFormat", "Output format", "textstring", Description = "Specify the time format used when converting the property value to a string. If left blank, the format will default to <code>t</code>.<br /><br /><a href=\"https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings#time-formats\" target=\"_blank\" rel=\"noopener\" class=\"skybrud-timepicker-link\">Read more about <em>Standard date and time format strings</em></a>.<br /><br />The format is ignored if the value type is <code>TimeOnly</code>.")]
        public string? OutputFormat { get; set; }

    }

}