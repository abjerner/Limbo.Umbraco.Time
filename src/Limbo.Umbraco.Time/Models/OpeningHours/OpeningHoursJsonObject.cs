using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.Models.OpeningHours {

    public class OpeningHoursJsonObject : JsonObjectBase {

        #region Constructors

        protected OpeningHoursJsonObject(JObject? json) : base(json) { }

        #endregion

    }

}