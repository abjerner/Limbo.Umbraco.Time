using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.PropertyEditors.DayOfWeek;


/// <summary>
/// Configuration editor for <see cref="DayOfWeekPropertyEditor"/>.
/// </summary>
public class DayOfWeekConfigurationEditor : ConfigurationEditor<DayOfWeekConfiguration> {

    public DayOfWeekConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }

}