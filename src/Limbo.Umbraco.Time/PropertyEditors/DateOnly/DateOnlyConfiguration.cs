using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.DateOnly;

/// <summary>
/// Configuration for <see cref="DateOnlyPropertyEditor"/>.
/// </summary>
public class DateOnlyConfiguration {

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