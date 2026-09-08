using System;
using System.Globalization;
using Limbo.Umbraco.Time.PropertyEditors.TimeOnly;
using Limbo.Umbraco.Time.Serialization.Newtonsoft;

namespace Limbo.Umbraco.Time.Models;

/// <summary>
/// Class representing a time value.
/// </summary>
[Newtonsoft.Json.JsonConverter(typeof(TimeValueNewtonsoftJsonConverter))]
[System.Text.Json.Serialization.JsonConverter(typeof(TimeValueNewtonsoftJsonConverter))]
public class TimeValue : IFormattable {

    private readonly string? _format;

    #region Properties

    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public TimeOnly Source { get; }

    /// <summary>
    /// Gets the hour component of the time value.
    /// </summary>
    public int Hour => Source.Hour;

    /// <summary>
    /// Gets the minute component of the time value.
    /// </summary>
    public int Minute => Source.Minute;

    /// <summary>
    /// Gets the second component of the time value.
    /// </summary>
    public int Second => Source.Second;

    #endregion

    #region Constructors

    /// <summary>
    /// Initialize a new instance based on the specified <paramref name="source"/> and <paramref name="configuration"/>.
    /// </summary>
    /// <param name="source">A <see cref="TimeOnly"/> representing the time value.</param>
    /// <param name="configuration">The configuration from the data type.</param>
    public TimeValue(TimeOnly source, TimeOnlyConfiguration? configuration) {
        Source = source;
        _format = configuration?.TimeFormat;
    }

    #endregion

    #region Member methods

    /// <summary>
    /// Returns a culture-specific string representation of the time value.
    /// </summary>
    /// <returns>
    /// A string representation of the time value, formatted according to the configured time format and the current culture.
    /// </returns>
    public override string ToString() {
        return Source.ToString(_format == "HH:mm:ss" ? "T" : "t");
    }

    /// <summary>
    /// Returns a string representation of the time value using the specified format
    /// and culture-specific formatting information.
    /// </summary>
    /// <param name="format">
    /// A standard or custom time format string, or <see langword="null"/> to use the default format.
    /// </param>
    /// <param name="formatProvider">
    /// An object that supplies culture-specific formatting information, or <see langword="null"/>
    /// to use the current culture.
    /// </param>
    /// <returns>The string representation of the time value.</returns>
    public string ToString(string? format, IFormatProvider? formatProvider) {
        return Source.ToString(format, formatProvider);
    }

    /// <summary>
    /// Returns a string representation of the time value using the configured time format
    /// and the specified culture-specific formatting information.
    /// </summary>
    /// <param name="formatProvider">
    /// An object that supplies culture-specific formatting information, or <see langword="null"/>
    /// to use the current culture.
    /// </param>
    /// <returns>The string representation of the time value.</returns>
    public string ToString(IFormatProvider? formatProvider) {
        return Source.ToString(_format == "HH:mm:ss" ? "T" : "t", formatProvider);
    }

    /// <summary>
    /// Returns a culture-invariant string representation of the time value.
    /// </summary>
    /// <returns>
    /// A culture-invariant string representation of the time value, formatted according
    /// to the configured time format.
    /// </returns>
    public string ToInvariantString() {
        return Source.ToString(_format == "HH:mm:ss" ? "HH:mm:ss" : "HH:mm", CultureInfo.InvariantCulture);
    }

    #endregion

}