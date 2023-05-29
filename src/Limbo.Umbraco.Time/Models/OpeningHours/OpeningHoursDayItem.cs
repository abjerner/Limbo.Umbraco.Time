using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Limbo.Umbraco.Time.PropertyEditors.OpeningHours;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Converters.Enums;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Essentials.Strings.Extensions;

namespace Limbo.Umbraco.Time.Models.OpeningHours {

    /// <summary>
    /// Class representing a weekday.
    /// </summary>
    public class OpeningHoursDayItem : OpeningHoursJsonObject {

        private string? _label;

        #region Properties

        /// <summary>
        /// Gets the day of the week - eg. <see cref="System.DayOfWeek.Monday"/>.
        /// </summary>
        [JsonProperty("day", Order = 1)]
        [JsonConverter(typeof(EnumCamelCaseConverter))]
        public DayOfWeek DayOfWeek { get; protected set; }

        /// <summary>
        /// Gets the label of the day.
        /// </summary>
        [JsonProperty("label", Order = 2)]
        public string Label {
            get => _label ?? LocalDayName;
            set => _label = value;
        }

        /// <summary>
        /// Gets the English name of the day.
        /// </summary>
        [JsonIgnore]
        public string EnglishDayName => CultureInfo.InvariantCulture.DateTimeFormat.GetDayName(DayOfWeek).FirstCharToUpper();

        /// <summary>
        /// Gets the local name of the day according to <see cref="CultureInfo.CurrentCulture"/>.
        /// </summary>
        [JsonProperty("localDayName")]
        public string LocalDayName => CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DayOfWeek).FirstCharToUpper();

        /// <summary>
        /// Gets an array of the time slots of the day.
        /// </summary>
        [JsonProperty("items", Order = 4)]
        public IReadOnlyList<OpeningHoursTimeSlot> Items { get; }

        /// <summary>
        /// Gets where the entity has at least one open time slot throughout the day.
        /// </summary>
        [JsonIgnore]
        public bool IsOpen {
            get { return Items is { Count: > 0 }; }
        }

        /// <summary>
        /// Gets whether the entity is closed throughout the entire day.
        /// </summary>
        [JsonIgnore]
        public bool IsClosed {
            get { return !IsOpen; }
        }

        /// <summary>
        /// Gets whether the entity is during multiple periods throughout the day.
        /// </summary>
        [JsonIgnore]
        public bool HasMultiple {
            get { return Items is { Count: > 1 }; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="json"/> object and <paramref name="dayOfWeek"/>.
        /// </summary>
        /// <param name="json">The JSON object representing the item.</param>
        /// <param name="dayOfWeek">The day of the week.</param>
        /// <param name="configuration">The opening hours configuration.</param>
        protected OpeningHoursDayItem(JObject json, DayOfWeek dayOfWeek, OpeningHoursConfiguration? configuration) : base(json) {
            DayOfWeek = dayOfWeek;
            Label = json.GetString("label")!;
            Items = json.GetArrayItems("items", x => OpeningHoursTimeSlot.Parse(x, configuration));
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Initializes an empty instance for the specified <see cref="DayOfWeek"/>.
        /// </summary>
        /// <param name="dayOfWeek">The day of the week.</param>
        /// <returns>Returns an instance of <see cref="OpeningHoursDayItem"/>.</returns>
        public static OpeningHoursDayItem GetEmptyModel(DayOfWeek dayOfWeek) {
            return new OpeningHoursDayItem(JObject.Parse("{\"items\":[]}"), dayOfWeek, null);
        }

        /// <summary>
        /// Gets an instance of <see cref="OpeningHoursTimeSlot"/> from the specified <paramref name="json"/> object.
        /// </summary>
        /// <param name="json">The instance of <see cref="JObject"/> to parse.</param>
        /// <param name="dayOfWeek">The day of the week.</param>
        /// <param name="configuration">The opening hours configuration.</param>
        [return: NotNullIfNotNull("json")]
        public static OpeningHoursDayItem? Parse(JObject? json, DayOfWeek dayOfWeek, OpeningHoursConfiguration configuration) {
            return json == null ? null : new OpeningHoursDayItem(json, dayOfWeek, configuration);
        }

        #endregion

    }

}