using System;
using Limbo.Umbraco.Time.Models;
using Newtonsoft.Json;

namespace Limbo.Umbraco.Time.Serialization.Newtonsoft;

internal class TimeValueNewtonsoftJsonConverter : JsonConverter<TimeValue> {

    public override void WriteJson(JsonWriter writer, TimeValue? value, JsonSerializer serializer) {

        if (value is null) {
            writer.WriteNull();
            return;
        }

        writer.WriteValue(value.ToInvariantString());

    }

    public override TimeValue? ReadJson(JsonReader reader, Type objectType, TimeValue? existingValue, bool hasExistingValue, JsonSerializer serializer) {
        throw new NotSupportedException();
    }

    public override bool CanRead => false;

}