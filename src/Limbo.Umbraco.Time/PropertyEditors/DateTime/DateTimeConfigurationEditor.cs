using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.DateTime;

/// <summary>
/// Configuration editor for <see cref="DateTimePropertyEditor"/>.
/// </summary>
public class DateTimeConfigurationEditor : ConfigurationEditor<DateTimeConfiguration> {

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeConfigurationEditor"/> class.
    /// </summary>
    /// <param name="ioHelper">An instance of <see cref="IIOHelper"/>.</param>
    public DateTimeConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }

}