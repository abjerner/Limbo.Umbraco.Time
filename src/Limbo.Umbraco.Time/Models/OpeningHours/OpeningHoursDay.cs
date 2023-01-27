using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Skybrud.Essentials.Strings.Extensions;
using Skybrud.Essentials.Time.Extensions;

namespace Limbo.Umbraco.Time.Models.OpeningHours {

    /// <summary>
    /// Class representing the opening hours of a day at a specific date.
    /// </summary>
    public class OpeningHoursDay {

        #region Properties

        /// <summary>
        /// Gets a reference to the weekday this day is based on.
        /// </summary>
        public OpeningHoursDayItem Weekday { get;  }

        /// <summary>
        /// Gets a reference to the holiday this day is based on, or <c>null</c> if not present.
        /// </summary>
        public OpeningHoursHolidayItem? Holiday { get; }

        /// <summary>
        /// Gets where the entity is open at least once during the day.
        /// </summary>
        public bool IsOpen { get; }

        /// <summary>
        /// Gets whether the entity is closed during the entire the day.
        /// </summary>
        public bool IsClosed {
            get { return !IsOpen; }
        }

        /// <summary>
        /// Gets the date of the day.
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// Gets the day of the week.
        /// </summary>
        public DayOfWeek DayOfWeek {
            get { return Date.DayOfWeek; }
        }

        /// <summary>
        /// Gets whether the day is today.
        /// </summary>
        public bool IsToday {
            get { return HelloTimeUtils.IsToday(Date); }
        }

        /// <summary>
        /// Gets whether the day is tomorrow.
        /// </summary>
        public bool IsTomorrow {
            get { return HelloTimeUtils.IsTomorrow(Date); }
        }

        /// <summary>
        /// Gets whether current time is today and within the opening hours.
        /// </summary>
        public bool IsCurrentlyOpen {
            get { return IsToday && Items.Any(x => x.Opens <= DateTime.Now && DateTime.Now <= x.Closes); }
        }

        /// <summary>
        /// Gets whether entity is currently closed (AKA different day or not within the opening hours of this day).
        /// </summary>
        public bool IsCurrentlyClosed {
            get { return !IsCurrentlyOpen; }
        }

        /// <summary>
        /// Gets the name of the weekday according to <see cref="CultureInfo.CurrentCulture"/>.
        /// </summary>
        public string WeekDayName {
            get { return Date.ToString("dddd", CultureInfo.CurrentCulture); }
        }

        /// <summary>
        /// Gets the name of the weekday according to <see cref="CultureInfo.CurrentCulture"/>. The first character of
        /// the name will always be uppercase.
        /// </summary>
        public string WeekDayNameFirstCharToUpper {
            get { return WeekDayName.FirstCharToUpper(); }
        }

        /// <summary>
        /// Gets the label of this day. Use <see cref="HasLabel"/> to check whether the day has a label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets whether this day has a label (AKA the <see cref="Label"/> has been specified).
        /// </summary>
        public bool HasLabel {
            get { return !string.IsNullOrEmpty(Label); }
        }

        /// <summary>
        /// Gets an array of the time slot of the day.
        /// </summary>
        public IReadOnlyList<OpeningHoursDayTimeSlot> Items { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="date"/>, <paramref name="weekday"/> item and <paramref name="holiday"/> item.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="weekday">The weekday item.</param>
        /// <param name="holiday">The holiday item.</param>
        public OpeningHoursDay(DateTime date, OpeningHoursDayItem weekday, OpeningHoursHolidayItem? holiday) {
            Date = date;
            Weekday = weekday;
            Holiday = holiday;
            IsOpen = holiday?.IsOpen ?? weekday.IsOpen;
            Label = holiday == null ? weekday.Label : holiday.Label;
            Items = (holiday == null ? weekday.Items : holiday.Items).Select(x => new OpeningHoursDayTimeSlot(date, x)).ToArray();
        }

        #endregion

    }

}