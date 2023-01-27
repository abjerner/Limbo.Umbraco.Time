using System;
using Limbo.Umbraco.Time.PropertyEditors.OpeningHours;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Essentials.Time;

namespace Limbo.Umbraco.Time.Models.OpeningHours {

    /// <summary>
    /// Class representing an opening hours holiday item.
    /// </summary>
    public class OpeningHoursHolidayItem : OpeningHoursDayItem {

        #region Properties

        /// <summary>
        /// Gets the date specified for this holiday item. If a date hasn't been specified,
        /// <see cref="DateTime.MinValue"/> will be returned instead.
        /// </summary>
        [JsonProperty("date", Order = 3)]
        public EssentialsDate? Date { get; set; }

        /// <summary>
        /// Gets whether a date has been specified for this holiday item.
        /// </summary>
        [JsonIgnore]
        public bool HasDate {
            get { return Date != null; }
        }

        /// <summary>
        /// Gets whether the holiday item is valid. Currently this property is just an alias of <see cref="HasDate"/>.
        /// </summary>
        [JsonIgnore]
        public bool IsValid {
            get { return HasDate; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new holiday item based on the specified <paramref name="json"/> object.
        /// </summary>
        /// <param name="json">The JSON object representing the holiday item.</param>
        /// <param name="configuration">The opening hours configuration.</param>
        protected OpeningHoursHolidayItem(JObject json, OpeningHoursConfiguration? configuration) : base(json, default, configuration) {
            Date = json.GetString("date", EssentialsDate.Parse);
            DayOfWeek = Date.DayOfWeek;
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Initializes an empty instance.
        /// </summary>
        /// <returns>Returns an instance of <see cref="OpeningHoursHolidayItem"/>.</returns>
        public static OpeningHoursHolidayItem GetEmptyModel() {
            return new OpeningHoursHolidayItem(JObject.Parse("{label:null,date:null,items:[]}"), null);
        }

        /// <summary>
        /// Gets an instance of <see cref="OpeningHoursHolidayItem"/> from the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="json">The instance of <see cref="JObject"/> to parse.</param>
        public static OpeningHoursHolidayItem Parse(JObject json) {
            return json == null ? null : new OpeningHoursHolidayItem(json, null);
        }

        #endregion

    }

}