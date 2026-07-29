using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.Time;

// [CHANGE: upgrade to Umbraco 17] Related: DateConfigurationEditor.cs, DateTimeConfigurationEditor.cs, UnixTimestampConfigurationEditor.cs, OpeningHoursConfigurationEditor.cs

/// <summary>
/// Represents the configuration editor for the time offset value editor.
/// </summary>
public class TimeConfigurationEditor : ConfigurationEditor<TimeConfiguration> {

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeConfigurationEditor"/> class.
    /// </summary>
    /// <param name="ioHelper">An instance of <see cref="IIOHelper"/>.</param>
    public TimeConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }

}
