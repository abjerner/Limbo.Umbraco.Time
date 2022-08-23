using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace Limbo.Umbraco.Time.PropertyEditors.UnixTime {

    /// <summary>
    /// Configuration editor for <see cref="UnixTimestampEditor"/>.
    /// </summary>
    public class UnixTimestampConfigurationEditor : ConfigurationEditor<UnixTimestampConfiguration> {

        /// <summary>
        /// Initializes a new instance of the <see cref="UnixTimestampConfigurationEditor"/> class.
        /// </summary>
        /// <param name="ioHelper">An instance of <see cref="IIOHelper"/>.</param>
        /// <param name="editorConfigurationParser">An instance of <see cref="IEditorConfigurationParser"/>.</param>
        public UnixTimestampConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(ioHelper, editorConfigurationParser) { }

    }

}