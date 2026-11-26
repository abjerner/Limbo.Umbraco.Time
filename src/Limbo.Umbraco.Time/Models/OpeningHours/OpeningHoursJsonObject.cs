using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.Models.OpeningHours;

public class OpeningHoursJsonObject {

    /// <summary>
    /// Gets the internal <see cref="T:Newtonsoft.Json.Linq.JObject" /> the object was created from, if any.
    /// </summary>
    [JsonIgnore]
    public JObject? JObject { get; }

    #region Constructors

    protected OpeningHoursJsonObject(JObject? json) {
        JObject = json;
    }

    #endregion

}