using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.DayOfWeek;

/// <summary>
/// Configuration for <see cref="DayOfWeekPropertyEditor"/>.
/// </summary>
public class DayOfWeekConfiguration {

    /// <summary>
    /// Gets or sets whether the value is nullable.
    /// </summary>
    [ConfigurationField("nullable")]
    public bool IsNullable { get; set; }

}