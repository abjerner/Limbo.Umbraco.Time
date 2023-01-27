using System;

namespace Limbo.Umbraco.Time.Models.OpeningHours {

    /// <summary>
    /// Class representing a time slot of a day at a specific date.
    /// </summary>
    public class OpeningHoursDayTimeSlot {

        #region Properties

        /// <summary>
        /// Gets the opening time of this item.
        /// </summary>
        public DateTime Opens { get; }

        /// <summary>
        /// Gets the closing time of this item.
        /// </summary>
        public DateTime Closes { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="date"/> and <paramref name="timeSlot"/>.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="timeSlot">The time slot.</param>
        public OpeningHoursDayTimeSlot(DateTime date, OpeningHoursTimeSlot timeSlot) {
            Opens = new DateTime(date.Year, date.Month, date.Day, timeSlot.Opens.Hours, timeSlot.Opens.Minutes, timeSlot.Opens.Seconds);
            Closes = new DateTime(date.Year, date.Month, date.Day, timeSlot.Closes.Hours, timeSlot.Closes.Minutes, timeSlot.Closes.Seconds);
        }

        #endregion

    }

}