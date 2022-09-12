using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace Limbo.Umbraco.Time.PropertyEditors.Timestamp {

    /// <summary>
    /// Configuration editor for <see cref="TimestampEditor"/>.
    /// </summary>
    public class TimestampConfigurationEditor : ConfigurationEditor<TimestampConfiguration> {

        /// <summary>
        /// Initializes a new instance of the <see cref="TimestampConfigurationEditor"/> class.
        /// </summary>
        /// <param name="ioHelper">An instance of <see cref="IIOHelper"/>.</param>
        /// <param name="editorConfigurationParser">An instance of <see cref="IEditorConfigurationParser"/>.</param>
        public TimestampConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(ioHelper, editorConfigurationParser) { }

    }

}