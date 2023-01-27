using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.Models.OpeningHours {

    public class OpeningHoursJsonConverter : JsonConverter {

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
            switch (value)  {
                case Dictionary<DayOfWeek, OpeningHoursDayItem> obj:
                    serializer.Serialize(writer, obj.ToDictionary(pair => (int) pair.Key + "", pair => pair.Value));
                    return;
                case DateTime time:
                    serializer.Serialize(writer, time.ToString("yyyy-MM-dd"));
                    return;
                case TimeSpan span:
                    serializer.Serialize(writer, span.ToString().Substring(0, 5));
                    return;
                default:
                    serializer.Serialize(writer, value);
                    break;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanRead {
            get { return false; }
        }

        public override bool CanConvert(Type type) {
            return false;
        }

    }

}