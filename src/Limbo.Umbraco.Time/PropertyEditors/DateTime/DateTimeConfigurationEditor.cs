using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace Limbo.Umbraco.Time.PropertyEditors.DateTime {

    /// <summary>
    /// Configuration editor for <see cref="DateTimeEditor"/>.
    /// </summary>
    public class DateTimeConfigurationEditor : ConfigurationEditor<DateTimeConfiguration> {

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeConfigurationEditor"/> class.
        /// </summary>
        /// <param name="ioHelper">An instance of <see cref="IIOHelper"/>.</param>
        /// <param name="editorConfigurationParser">An instance of <see cref="IEditorConfigurationParser"/>.</param>
        public DateTimeConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(ioHelper, editorConfigurationParser) { }

    }

}