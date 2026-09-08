using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.TimeOnly;

/// <summary>
/// Represents the configuration for the <see cref="TimeOnlyPropertyEditor"/>.
/// </summary>
public class TimeOnlyConfiguration {

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
    /// Gets or sets the time format of the time value.
    /// </summary>
    [ConfigurationField("timeFormat")]
    public string? TimeFormat { get; set; }

}