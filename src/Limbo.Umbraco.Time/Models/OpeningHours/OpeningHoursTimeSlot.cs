using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Limbo.Umbraco.Time.PropertyEditors.OpeningHours;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Essentials.Strings;

namespace Limbo.Umbraco.Time.Models.OpeningHours {

    /// <summary>
    /// Class representing a time slot of a day at an unspecified date.
    /// </summary>
    public class OpeningHoursTimeSlot : OpeningHoursJsonObject {

        private readonly OpeningHoursConfiguration? _configuration;

        #region Properties

        /// <summary>
        /// Gets the opening time of this item.
        /// </summary>
        [JsonIgnore]
        public TimeSpan Opens { get; }

        /// <summary>
        /// Gets a textual representation of <see cref="Opens"/>.
        /// </summary>
        [JsonProperty("opens")]
        public string OpensText => Format(Opens);

        /// <summary>
        /// Gets the closing time of this item.
        /// </summary>
        [JsonIgnore]
        public TimeSpan Closes { get; }

        /// <summary>
        /// Gets a textual representation of <see cref="Closes"/>.
        /// </summary>
        [JsonProperty("closes")]
        public string ClosesText => Format(Closes);

        #endregion

        #region Constructors

        private OpeningHoursTimeSlot(JObject json, OpeningHoursConfiguration? configuration) : base(json) {
            _configuration = configuration;
            Opens = json.GetString("opens", TimeSpan.Parse);
            Closes = json.GetString("closes", TimeSpan.Parse);
        }

        internal OpeningHoursTimeSlot(TimeSpan opens, TimeSpan closes, OpeningHoursConfiguration configuration) : base(null) {
            _configuration = configuration;
            Opens = opens;
            Closes = closes;
        }

        #endregion

        #region Member methods

        private string Format(TimeSpan time) {
            string format1 = StringUtils.FirstWithValue(_configuration?.TimeFormat, "HH\\:mm");
            string format2 = StringUtils.FirstWithValue(_configuration?.TimeFormatEnglish, "hh\\:mm tt");
            return DateTime.Today.Add(time).ToString(CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "en" ? format2 : format1);
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Gets an instance of <see cref="OpeningHoursTimeSlot"/> from the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="obj">The instance of <see cref="JObject"/> to parse.</param>
        /// <param name="configuration">The opening hours configuration.</param>
        [return: NotNullIfNotNull("obj")]
        public static OpeningHoursTimeSlot? Parse(JObject? obj, OpeningHoursConfiguration? configuration) {
            return obj == null ? null : new OpeningHoursTimeSlot(obj, configuration);
        }

        #endregion

    }

}