using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.Date {

    /// <summary>
    /// Configuration for <see cref="DateEditor"/>.
    /// </summary>
    public class DateConfiguration {

        /// <summary>
        /// Gets or sets .NET value type returned by properties using this data type.
        /// </summary>
        [ConfigurationField("valueType", "Value type", "/App_Plugins/Limbo.Umbraco.Time/Views/Editors/DateValueType.html", Description = "Select the .NET value type returned by properties using this data type.")]
        public string? ValueType { get; set; }

        /// <summary>
        /// Gets or sets whether the value is nullable.
        /// </summary>
        [ConfigurationField("nullable", "Is nullable?", "boolean", Description = "Select whether the value is nullable.")]
        public bool IsNullable { get; set; }

    }

}