using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace Limbo.Umbraco.Time.PropertyEditors.DateTime {

    /// <summary>
    /// Represents a unix time property editor.
    /// </summary>
    [DataEditor(EditorAlias, EditorType.PropertyValue, "Limbo Date & Time", EditorView, Group = "Limbo", Icon = "icon-time color-limbo", ValueType = EditorValueType)]
    public class DateTimeEditor : DataEditor {

        private readonly IIOHelper _ioHelper;
        private readonly IEditorConfigurationParser _editorConfigurationParser;

        #region Constants

        internal const string EditorAlias = "Limbo.Umbraco.DateTime";

        internal const string EditorView = "/App_Plugins/Limbo.Umbraco.Time/Views/Editors/DateTime.html";

        internal const string EditorValueType = ValueTypes.DateTime;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeEditor"/> class.
        /// </summary>
        public DateTimeEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(dataValueEditorFactory) {
            _ioHelper = ioHelper;
            _editorConfigurationParser = editorConfigurationParser;
        }

        #endregion

        #region Member methods

        /// <inheritdoc />
        protected override IConfigurationEditor CreateConfigurationEditor() {
            return new DateTimeConfigurationEditor(_ioHelper, _editorConfigurationParser);
        }

        #endregion

    }

}