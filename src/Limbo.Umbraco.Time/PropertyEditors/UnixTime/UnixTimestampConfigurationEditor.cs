using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.UnixTime;

/// <summary>
/// Configuration editor for <see cref="UnixTimestampPropertyEditor"/>.
/// </summary>
public class UnixTimestampConfigurationEditor : ConfigurationEditor<UnixTimestampConfiguration> {

    /// <summary>
    /// Initializes a new instance of the <see cref="UnixTimestampConfigurationEditor"/> class.
    /// </summary>
    /// <param name="ioHelper">An instance of <see cref="IIOHelper"/>.</param>
    public UnixTimestampConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }

}