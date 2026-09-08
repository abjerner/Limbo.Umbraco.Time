using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.OpeningHours;

/// <summary>
/// Represents the configuration for the opening hours editor.
/// </summary>
public class OpeningHoursConfiguration {

    /// <summary>
    /// Gets or sets whether the weekdays part of the opening hours property editor should be hidden.
    /// </summary>
    [ConfigurationField("hideWeekdays")]
    public bool HideWeekdays { get; set; }

    /// <summary>
    /// Gets or sets whether the holidays part of the opening hours property editor should be hidden.
    /// </summary>
    [ConfigurationField("hideHolidays")]
    public bool HideHolidays { get; set; }

    /// <summary>
    /// Gets or sets whether it should be possible to enter multiple time slots.
    /// </summary>
    [ConfigurationField("allowMultipleTimeSlots")]
    public bool AllowMultipleTimeSlots { get; set; }

    /// <summary>
    /// Gets or sets the maximum allowed number of timeslots per item.
    /// </summary>
    [ConfigurationField("maxTimeSlots")]
    public int MaxTimeSlots { get; set; }

    /// <summary>
    /// Gets or sets whether the property editor label should be hidden.
    /// </summary>
    [ConfigurationField("hideLabel")]
    public bool HideLabel { get; set; }

    /// <summary>
    /// Gets or sets the date format to be used when converting open and close times to a textual representation.
    /// </summary>
    [ConfigurationField("timeFormat")]
    public string? TimeFormat { get; set; }

    /// <summary>
    /// Gets or sets the date format to be used when converting open and close times to a textual representation.
    /// </summary>
    [ConfigurationField("timeFormatEnglish")]
    public string? TimeFormatEnglish { get; set; }

}
