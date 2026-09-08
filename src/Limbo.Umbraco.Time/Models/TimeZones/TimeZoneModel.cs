using System.Text.Json.Serialization;

namespace Limbo.Umbraco.Time.Models.TimeZones;

/// <summary>
/// Class representing the JSON model of a time zone as returned by the Management API.
/// </summary>
/// <remarks>
/// <see cref="ITimeZone"/> exposes a <see cref="System.TimeZoneInfo"/> instance, which shouldn't be part of the API
/// surface. The Management API serializes using <c>System.Text.Json</c>, so this model is annotated accordingly.
/// </remarks>
public class TimeZoneModel {

    /// <summary>
    /// Gets the unique identifier of the time zone.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; }

    /// <summary>
    /// Gets the friendly name of the time zone.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="id"/> and <paramref name="name"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the time zone.</param>
    /// <param name="name">The friendly name of the time zone.</param>
    public TimeZoneModel(string id, string name) {
        Id = id;
        Name = name;
    }

}
