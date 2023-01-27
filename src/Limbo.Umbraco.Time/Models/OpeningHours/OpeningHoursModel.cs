using System;
using System.Collections.Generic;
using System.Linq;
using Limbo.Umbraco.Time.PropertyEditors.OpeningHours;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Converters.Enums;
using Skybrud.Essentials.Json.Extensions;

namespace Limbo.Umbraco.Time.Models.OpeningHours {

    /// <summary>
    /// Class representing the model of the <strong>Opening Hours</strong> property editor.
    /// </summary>
    public class OpeningHoursModel : OpeningHoursJsonObject {

        private readonly Dictionary<DayOfWeek, OpeningHoursDayItem> _weekdays;
        private readonly Dictionary<string, OpeningHoursHolidayItem> _holidays;

        #region Properties

        /// <summary>
        /// Gets a reference to the opening hour configuration, or <c>null</c> if not available.
        /// </summary>
        protected internal OpeningHoursConfiguration? Configuration { get; }

        /// <summary>
        /// Gets the day representing the first day of the week.
        /// </summary>
        [JsonProperty("startOfWeek")]
        [JsonConverter(typeof(EnumCamelCaseConverter))]
        public DayOfWeek StartOfWeek => DayOfWeek.Monday;

        /// <summary>
        /// Gets a dictionary of the standard weekdays as specified in the property editor.
        /// </summary>
        [JsonProperty("weekdays")]
        public IReadOnlyList<OpeningHoursDayItem> Weekdays {

            get {

                OpeningHoursDayItem[] days = new OpeningHoursDayItem[7];

                for (int i = 0; i < 7; i++) {

                    int index = (i + (int) StartOfWeek) % 7;

                    days[i] = _weekdays[(DayOfWeek) index];

                }

                return days;


            }

        }

        /// <summary>
        /// Gets a list the holidays as specified in the property editor.
        /// </summary>
        [JsonProperty("holidays")]
        public IReadOnlyList<OpeningHoursHolidayItem> Holidays { get; }

        /// <summary>
        /// Gets whether the entity (store, company or similar) is currently open.
        /// </summary>
        [JsonIgnore]
        public bool IsCurrentlyOpen {
            get { return GetDay(DateTime.Now).IsCurrentlyOpen; }
        }

        /// <summary>
        /// Gets whether the entity (store, company or similar) is currently closed.
        /// </summary>
        [JsonIgnore]
        public bool IsCurrentlyClosed {
            get { return GetDay(DateTime.Now).IsCurrentlyClosed; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance with default options.
        /// </summary>
        public OpeningHoursModel() : base(null) {

            _weekdays = new Dictionary<DayOfWeek, OpeningHoursDayItem>();
            for (int i = 0; i < 7; i++) {
                DayOfWeek dayOfWeek = (DayOfWeek) i;
                _weekdays.Add(dayOfWeek, OpeningHoursDayItem.GetEmptyModel(dayOfWeek));
            }

            Holidays = Array.Empty<OpeningHoursHolidayItem>();
            _holidays = new Dictionary<string, OpeningHoursHolidayItem>();

        }

        /// <summary>
        /// Initializes a new instance from the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="json">The instance of <see cref="JObject"/> representing the model.</param>
        /// <param name="configuration">The opening hours configuration.</param>
        private OpeningHoursModel(JObject json, OpeningHoursConfiguration configuration) : base(json) {

            Configuration = configuration;

            // Parse the weekdays
            _weekdays = new Dictionary<DayOfWeek, OpeningHoursDayItem>();

            for (int i = 1; i <= 7; i++) {

                // Calculate the day of week
                DayOfWeek dayOfWeek = (DayOfWeek) (i%7);

                // Calculate the JSON object key
                string key = "weekdays." + (int) dayOfWeek;

                // Parse and add the day to the dictionary
                _weekdays.Add(dayOfWeek, json.GetObject(key, x => OpeningHoursDayItem.Parse(x, dayOfWeek, configuration)));

            }

            // Parse holidays
            Holidays = json.GetArrayItems("holidays", OpeningHoursHolidayItem.Parse);

            // Create a dictionary with the holidays - for O(1) lookups
            _holidays = Holidays
                .Where(x => x.IsValid)
                .DistinctBy(x => x.Date.ToString("yyyyMMdd"))
                .ToDictionary(x => x.Date.ToString("yyyyMMdd"));

        }

        #endregion

        #region Member methods

        /// <summary>
        /// Gets whether the day at the specified <code>date</code> is a holiday. Notice that this check will only work
        /// if <strong>Require Holiday Dates</strong> has been checked in the pre-value editor.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>Returns <code>true</code> if the day at the specified <code>date</code> is a holiday, otherwise
        /// <code>false</code>.</returns>
        public bool IsHoliday(DateTime date) {
            return _holidays.ContainsKey(date.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// Gets whether the day at the specified <paramref name="date"/> is a holiday. Notice that this check will
        /// only work if <strong>Require Holiday Dates</strong> has been checked in the pre-value editor.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>Returns <code>true</code> if the day at the specified <code>date</code> is a holiday, otherwise
        /// <code>false</code>.</returns>
        public bool IsHoliday(DateTimeOffset date) {
            return _holidays.ContainsKey(date.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// Gets a reference to the holiday at the specified <code>date</code>, or <code>null</code> if the day doesn't
        /// represent a holiday.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>Returns an instance of <see cref="OpeningHoursHolidayItem"/> representing the holiday, or
        /// <code>null</code> if the day doesn't represent a holiday.</returns>
        public OpeningHoursHolidayItem? GetHoliday(DateTime date) {
            return _holidays.TryGetValue(date.ToString("yyyyMMdd"), out OpeningHoursHolidayItem? holiday) ? holiday : null;
        }

        /// <summary>
        /// Gets a reference to the holiday at the specified <code>date</code>, or <code>null</code> if the day doesn't
        /// represent a holiday.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="holiday">A reference to the holiday.</param>
        /// <returns>Returns <code>true</code> if the day represents a holiday, otherwise <code>false</code>.</returns>
        public bool TryGetHoliday(DateTime date, out OpeningHoursHolidayItem? holiday) {
            return _holidays.TryGetValue(date.ToString("yyyyMMdd"), out holiday);
        }

        /// <summary>
        /// Gets a reference to the holiday at the specified <paramref name="date"/>, or <code>null</code> if the day
        /// doesn't represent a holiday.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>An instance of <see cref="OpeningHoursHolidayItem"/> representing the holiday, or
        /// <code>null</code> if the day doesn't represent a holiday.</returns>
        public OpeningHoursHolidayItem? GetHoliday(DateTimeOffset date) {
            return _holidays.TryGetValue(date.ToString("yyyyMMdd"), out OpeningHoursHolidayItem? holiday) ? holiday : null;
        }

        /// <summary>
        /// Gets a reference to the holiday at the specified <paramref name="date"/>, or <code>null</code> if the day
        /// doesn't represent a holiday.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="holiday">A reference to the holiday.</param>
        /// <returns><code>true</code> if the day represents a holiday, otherwise <code>false</code>.</returns>
        public bool TryGetHoliday(DateTimeOffset date, out OpeningHoursHolidayItem? holiday) {
            return _holidays.TryGetValue(date.ToString("yyyyMMdd"), out holiday);
        }

        /// <summary>
        /// Gets an instance of <see cref="OpeningHoursDay"/> representing the day at the specified <code>date</code>. If <strong>Require Holiday Dates</strong> has
        /// been checked in the pre-value editor, holidays will be taken into account as well.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>Returns an instance of <see cref="OpeningHoursDay"/> representing the day at the specified <code>date</code>.</returns>
        public OpeningHoursDay GetDay(DateTime date) {

            // Get information about the weekday (and whether it is a holiday)
            OpeningHoursDayItem woh = _weekdays[date.DayOfWeek];
            OpeningHoursHolidayItem? hoh = GetHoliday(date);

            // Initialize the instance for the day
            return new OpeningHoursDay(date, woh, hoh);

        }

        /// <summary>
        /// Gets an instance of <see cref="OpeningHoursDayOffset"/> representing the day at the specified
        /// <paramref name="date"/>. If <strong>Require Holiday Dates</strong> has been checked in the pre-value
        /// editor, holidays will be taken into account as well.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>An instance of <see cref="OpeningHoursDayOffset"/> representing the day at the specified
        /// <paramref name="date"/>.</returns>
        public OpeningHoursDayOffset GetDay(DateTimeOffset date) {

            // Get information about the weekday (and whether it is a holiday)
            OpeningHoursDayItem woh = _weekdays[date.DayOfWeek];
            OpeningHoursHolidayItem? hoh = GetHoliday(date);

            // Initialize the instance for the day
            return new OpeningHoursDayOffset(date, woh, hoh);

        }

        /// <summary>
        /// Gets an array of the next <code>count</code> upcoming days. If <strong>Require Holiday Dates</strong> has
        /// been checked in the pre-value editor, holidays will be incorporated into the result.
        /// </summary>
        /// <param name="count">The amount of days to be returned (including the current day).</param>
        /// <returns>Returns an array of <see cref="OpeningHoursDay"/> representing the opening hours of the upcoming days.</returns>
        public OpeningHoursDay[] GetUpcomingDays(int count) {

            // Array containing the days
            OpeningHoursDay[] upcomingDays = new OpeningHoursDay[count];

            // Iterate through the days one by one
            for (int i = 0; i < count; i++) {
                upcomingDays[i] = GetDay(DateTime.Today.AddDays(i));
            }

            return upcomingDays;

        }

        /// <summary>
        /// Gets an array of the next <paramref name="count"/> upcoming days. If <strong>Require Holiday Dates</strong>
        /// has been checked in the pre-value editor, holidays will be incorporated into the result.
        /// </summary>
        /// <param name="count">The amount of days to be returned (including the current day).</param>
        /// <param name="timeZone">The <see cref="TimeZoneInfo"/> that should be used.</param>
        /// <returns>An array of <see cref="OpeningHoursDay"/> representing the opening hours of the upcoming days.</returns>
        public OpeningHoursDayOffset[] GetUpcomingDays(int count, TimeZoneInfo timeZone) {

            // Array containing the days
            OpeningHoursDayOffset[] upcomingDays = new OpeningHoursDayOffset[count];

            // Iterate through the days one by one
            for (int i = 0; i < 14; i++) {

                // Get the current timestamp (according to the specified time zone)
                DateTimeOffset timeZoneNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZone);

                // Get the timestamp for the day
                DateTimeOffset dt = timeZoneNow.AddDays(i);

                OpeningHoursDayItem weekday = _weekdays[dt.DayOfWeek];
                OpeningHoursHolidayItem? holiday = GetHoliday(dt);

                upcomingDays[i] = new OpeningHoursDayOffset(dt, weekday, holiday);

            }

            // Return the array
            return upcomingDays;

        }

        #endregion

        #region Static methods

        /// <summary>
        /// Gets an instance of <see cref="OpeningHoursModel"/> from the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="json">The instance of <see cref="JObject"/> to parse.</param>
        public static OpeningHoursModel Parse(JObject json) {
            return json == null ? null : new OpeningHoursModel(json, null);
        }

        /// <summary>
        /// Gets an instance of <see cref="OpeningHoursModel"/> from the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="json">The instance of <see cref="JObject"/> to parse.</param>
        /// <param name="configuration">The opening hours configuration.</param>
        public static OpeningHoursModel Parse(JObject json, OpeningHoursConfiguration? configuration) {
            return json == null ? null : new OpeningHoursModel(json, configuration);
        }


        #endregion

    }

}