using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.TimeOnly;

/// <summary>
/// Represents the configuration editor for the time offset value editor.
/// </summary>
public class TimeOnlyConfigurationEditor : ConfigurationEditor<TimeOnlyConfiguration> {

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeOnlyConfigurationEditor"/> class.
    /// </summary>
    /// <param name="ioHelper">An instance of <see cref="IIOHelper"/>.</param>
    public TimeOnlyConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }

}