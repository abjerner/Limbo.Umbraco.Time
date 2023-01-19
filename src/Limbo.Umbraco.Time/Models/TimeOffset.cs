using System;
using Limbo.Umbraco.Time.PropertyEditors.Time;
using Newtonsoft.Json;
using Skybrud.Essentials.Json.Converters;

namespace Limbo.Umbraco.Time.Models {

    /// <summary>
    /// Class representing a time offset.
    /// </summary>
    [JsonConverter(typeof(StringJsonConverter))]
    public class TimeOffset {

        private readonly TimeSpan _time;

        private readonly string _format;

        #region Properties

        /// <summary>
        /// Gets the hours of the time offset.
        /// </summary>
        public int Hours => _time.Hours;

        /// <summary>
        /// Gets the minutes of the time offset.
        /// </summary>
        public int Minutes => _time.Minutes;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize a new instance based on the specified <paramref name="time"/> and <paramref name="configuration"/>.
        /// </summary>
        /// <param name="time">A <see cref="TimeSpan"/> representing the time offset.</param>
        /// <param name="configuration">The configuration from the data type.</param>
        public TimeOffset(TimeSpan time, TimeConfiguration? configuration) {
            _time = time;
            _format = string.IsNullOrWhiteSpace(configuration?.OutputFormat) ? "t" : configuration.OutputFormat;
        }

        #endregion

        #region Member methods

        /// <inheritdoc />
        public override string ToString() {
            return DateTime.Today.Add(_time).ToString(_format);
        }

        #endregion

    }

}