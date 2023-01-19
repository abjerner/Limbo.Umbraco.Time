using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace Limbo.Umbraco.Time.PropertyEditors.Time {

    /// <summary>
    /// Represents the configuration editor for the time offset value editor.
    /// </summary>
    public class TimeConfigurationEditor : ConfigurationEditor<TimeConfiguration> {

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeConfigurationEditor"/> class.
        /// </summary>
        /// <param name="ioHelper">An instance of <see cref="IIOHelper"/>.</param>
        /// <param name="editorConfigurationParser">An instance of <see cref="IEditorConfigurationParser"/>.</param>
        public TimeConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(ioHelper, editorConfigurationParser) { }

    }

}