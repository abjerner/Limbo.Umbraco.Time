using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.Time;

// [CHANGE: upgrade to Umbraco 17] Related: DateConfiguration.cs, DateTimeConfiguration.cs, UnixTimestampConfiguration.cs, OpeningHoursConfiguration.cs, src/index.ts
// [ConfigurationField] now only carries the storage key. The description for "outputFormat" (which
// contained HTML and a link to the .NET format string docs) now lives in src/index.ts.

/// <summary>
/// Represents the configuration for the time offset value editor.
/// </summary>
public class TimeConfiguration {

    /// <summary>
    /// Gets or sets .NET value type returned by properties using this data type.
    /// </summary>
    [ConfigurationField("valueType")]
    public string? ValueType { get; set; }

    /// <summary>
    /// Gets or sets whether the value is nullable.
    /// </summary>
    [ConfigurationField("nullable")]
    public bool IsNullable { get; set; } = true;

    /// <summary>
    /// Gets or sets the output format of the time offset value.
    /// </summary>
    [ConfigurationField("outputFormat")]
    public string? OutputFormat { get; set; }

}
