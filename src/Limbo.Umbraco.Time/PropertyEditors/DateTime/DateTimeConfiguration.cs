using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.DateTime;

// [CHANGE: upgrade to Umbraco 17] Related: DateConfiguration.cs, TimeConfiguration.cs, UnixTimestampConfiguration.cs, OpeningHoursConfiguration.cs, src/index.ts
// [ConfigurationField] now only carries the storage key - labels, descriptions and the field UIs
// (including the custom time zone picker) are declared in src/index.ts.

/// <summary>
/// Configuration for <see cref="DateTimePropertyEditor"/>.
/// </summary>
public class DateTimeConfiguration {

    /// <summary>
    /// Gets or sets the unique identifer of the time zone to be used.
    /// </summary>
    [ConfigurationField("timeZone")]
    public string? TimeZone { get; set; }

    /// <summary>
    /// Gets or sets whether the field is readonly.
    /// </summary>
    [ConfigurationField("readonly")]
    public bool IsReadOnly { get; set; }

    /// <summary>
    /// Gets or sets whether the property value type should be a nullable type.
    /// </summary>
    [ConfigurationField("nullable")]
    public bool IsNullable { get; set; } = true;

    /// <summary>
    /// Gets or sets .NET value type returned by properties using this data type.
    /// </summary>
    [ConfigurationField("valueType")]
    public string? ValueType { get; set; }

}
