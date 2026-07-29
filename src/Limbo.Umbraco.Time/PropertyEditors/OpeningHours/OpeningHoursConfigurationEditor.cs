using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.PropertyEditors.OpeningHours;

// [CHANGE: upgrade to Umbraco 17] Related: DateConfigurationEditor.cs, DateTimeConfigurationEditor.cs, TimeConfigurationEditor.cs, UnixTimestampConfigurationEditor.cs

public class OpeningHoursConfigurationEditor : ConfigurationEditor<OpeningHoursConfiguration> {

    public OpeningHoursConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }

}
