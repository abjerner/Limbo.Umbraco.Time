using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.OpeningHours {

    /// <summary>
    /// Represents the configuration for the opening hours editor.
    /// </summary>
    public class OpeningHoursConfiguration {

        /// <summary>
        /// Gets or sets whether the weekdays part of the opening hours property editor should be hidden.
        /// </summary>
        [ConfigurationField("hideWeekdays", "Hide weekdays", "boolean", Description = "If selected, the part of the UI for entering weekdays will not be shown.")]
        public bool HideWeekdays { get; set; }

        /// <summary>
        /// Gets or sets whether the holidays part of the opening hours property editor should be hidden.
        /// </summary>
        [ConfigurationField("hideHolidays", "Hide holidays", "boolean", Description = "If selected, the part of the UI for entering holidays will not be shown.")]
        public bool HideHolidays { get; set; }

        /// <summary>
        /// Gets or sets whether it should be possible to enter multiple time slots.
        /// </summary>
        [ConfigurationField("allowMultipleTimeSlots", "Allow multiple time slots", "boolean", Description = "Allows editors to specify multiple time slots for a given day.")]
        public bool AllowMultipleTimeSlots { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowed number of timeslots per item.
        /// </summary>
        [ConfigurationField("maxTimeSlots", "Max time slots", "number", Description = "The maximum amount of time slots that can be added (0 = unlimited).")]
        public int MaxTimeSlots { get; set; }

        /// <summary>
        /// Gets or sets whether the property editor label should be hidden.
        /// </summary>
        [ConfigurationField("hideLabel", "Hide label", "boolean", Description = "Set whether to hide the editor label and take up the full width of the editing area.")]
        public bool HideLabel { get; set; }

        /// <summary>
        /// Gets or sets the date format to be used when converting open and close times to a textual representation.
        /// </summary>
        [ConfigurationField("timeFormat", "Time format", "textstring", Description = "Specify the time format to be used. Defaults to <code>HH\\:mm</code> if not specified.")]
        public string? TimeFormat { get; set; }

        /// <summary>
        /// Gets or sets the date format to be used when converting open and close times to a textual representation.
        /// </summary>
        [ConfigurationField("timeFormatEnglish", "Time format (English)", "textstring", Description = "Specify an alternative time format to be used for English cultures. Default is <code>hh\\:mm tt</code>.")]
        public string? TimeFormatEnglish { get; set; }

    }

}