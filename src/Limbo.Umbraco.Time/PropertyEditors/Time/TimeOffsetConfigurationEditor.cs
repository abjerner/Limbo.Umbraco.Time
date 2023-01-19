using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace Limbo.Umbraco.Time.PropertyEditors.TimeOffset {

    /// <summary>
    /// Represents the configuration editor for the time offset value editor.
    /// </summary>
    public class TimeOffsetConfigurationEditor : ConfigurationEditor<TimeOffsetConfiguration> {

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOffsetConfigurationEditor"/> class.
        /// </summary>
        /// <param name="ioHelper">An instance of <see cref="IIOHelper"/>.</param>
        /// <param name="editorConfigurationParser">An instance of <see cref="IEditorConfigurationParser"/>.</param>
        public TimeOffsetConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(ioHelper, editorConfigurationParser) { }

    }

}