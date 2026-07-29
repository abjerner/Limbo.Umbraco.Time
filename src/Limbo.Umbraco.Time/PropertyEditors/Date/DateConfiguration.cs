using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.Date;

// [CHANGE: upgrade to Umbraco 17] Related: DateTimeConfiguration.cs, TimeConfiguration.cs, UnixTimestampConfiguration.cs, OpeningHoursConfiguration.cs, src/index.ts
// In v14+ the [ConfigurationField] attribute only carries the storage key. The editing UI for each
// setting (label, description, propertyEditorUiAlias) is declared in the backoffice manifest
// (src/index.ts -> meta.settings.properties).

/// <summary>
/// Configuration for <see cref="DatePropertyEditor"/>.
/// </summary>
public class DateConfiguration {

    /// <summary>
    /// Gets or sets .NET value type returned by properties using this data type.
    /// </summary>
    [ConfigurationField("valueType")]
    public string? ValueType { get; set; }

    /// <summary>
    /// Gets or sets whether the value is nullable.
    /// </summary>
    [ConfigurationField("nullable")]
    public bool IsNullable { get; set; }

}
