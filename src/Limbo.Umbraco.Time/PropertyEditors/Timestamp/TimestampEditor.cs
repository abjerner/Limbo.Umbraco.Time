using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace Limbo.Umbraco.Time.PropertyEditors.Timestamp {

    /// <summary>
    /// Represents a unix time property editor.
    /// </summary>
    [DataEditor(EditorAlias, EditorType.PropertyValue, "Limbo Timestamp", EditorView, Group = "Limbo", Icon = "icon-time color-limbo", ValueType = EditorValueType)]
    public class TimestampEditor : DataEditor {

        private readonly IIOHelper _ioHelper;
        private readonly IEditorConfigurationParser _editorConfigurationParser;

        #region Constants

        internal const string EditorAlias = "Limbo.Umbraco.Timestamp";

        internal const string EditorView = "/App_Plugins/Limbo.Umbraco.Time/Views/Editors/Timestamp.html";

        internal const string EditorValueType = ValueTypes.DateTime;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimestampEditor"/> class.
        /// </summary>
        public TimestampEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(dataValueEditorFactory) {
            _ioHelper = ioHelper;
            _editorConfigurationParser = editorConfigurationParser;
        }

        #endregion

        #region Member methods

        /// <inheritdoc />
        protected override IConfigurationEditor CreateConfigurationEditor() {
            return new TimestampConfigurationEditor(_ioHelper, _editorConfigurationParser);
        }

        #endregion

    }

}