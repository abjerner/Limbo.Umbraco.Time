using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Limbo.Umbraco.Time.Models;

namespace Limbo.Umbraco.Time.Serialization.SystemTextJson;

/// <summary>
/// JSON converter for serializing instances of <see cref="TimeValue"/> as strings.
/// </summary>
internal class TimeValueSystemTextJsonConverter : JsonConverter<TimeValue> {

    /// <inheritdoc />
    public override TimeValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TimeValue value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToInvariantString());
    }

}