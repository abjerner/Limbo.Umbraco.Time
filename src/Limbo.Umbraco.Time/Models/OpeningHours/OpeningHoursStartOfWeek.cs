using System;

namespace Limbo.Umbraco.Time.Models.OpeningHours {

    /// <summary>
    /// Enum class indicating the first day of the week.
    /// </summary>
    public enum OpeningHoursStartOfWeek {

        /// <summary>
        /// Indicates that the first day of the week should automatically be determined.
        /// </summary>
        Auto,

        /// <summary>
        /// Indicates that <see cref="DayOfWeek.Monday"/> should be used as the first day of the week.
        /// </summary>
        Monday,

        /// <summary>
        /// Indicates that <see cref="DayOfWeek.Sunday"/> should be used as the first day of the week.
        /// </summary>
        Sunday

    }

}