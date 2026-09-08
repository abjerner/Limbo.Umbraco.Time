using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.PropertyEditors.DateOnly;

/// <summary>
/// Configuration editor for <see cref="DateOnlyPropertyEditor"/>.
/// </summary>
public class DateOnlyConfigurationEditor : ConfigurationEditor<DateOnlyConfiguration> {

    public DateOnlyConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }

}