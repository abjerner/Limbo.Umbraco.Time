using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.UnixTime;

// [CHANGE: upgrade to Umbraco 17] Related: DateConfiguration.cs, DateTimeConfiguration.cs, TimeConfiguration.cs, OpeningHoursConfiguration.cs, src/index.ts
// [ConfigurationField] now only carries the storage key - the field UIs live in src/index.ts.

/// <summary>
/// Configuration for <see cref="UnixTimestampPropertyEditor"/>.
/// </summary>
public class UnixTimestampConfiguration {

    /// <summary>
    /// Gets or sets the unique identifer of the time zone to be used.
    /// </summary>
    [ConfigurationField("timeZone")]
    public string? TimeZone { get; set; }

    /// <summary>
    /// Gets or sets whether the UNIX timestamp should be shown to the user.
    /// </summary>
    [ConfigurationField("showUnixTimestamp")]
    public bool ShowUnixTimestampp { get; set; }

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
