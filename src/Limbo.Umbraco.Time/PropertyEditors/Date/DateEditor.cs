using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace Limbo.Umbraco.Time.PropertyEditors.Date {

    /// <summary>
    /// Represents a date picker property editor.
    /// </summary>
    [DataEditor(EditorAlias, EditorType.PropertyValue, "Limbo Date", EditorView, ValueType = ValueTypes.String, Group = "Limbo", Icon = "icon-calendar color-limbo")]
    public class DateEditor : DateTimePropertyEditor {

        private readonly IIOHelper _ioHelper;
        private readonly IEditorConfigurationParser _editorConfigurationParser;

        #region Constants

        internal const string EditorAlias = "Limbo.Umbraco.Date";

        internal const string EditorView = "datepicker";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DateEditor"/> class.
        /// </summary>

        public DateEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(dataValueEditorFactory, ioHelper) {
            _ioHelper = ioHelper;
            _editorConfigurationParser = editorConfigurationParser;
        }

        #endregion

        #region Member methods

        /// <inheritdoc />
        protected override IConfigurationEditor CreateConfigurationEditor() => new DateConfigurationEditor(_ioHelper, _editorConfigurationParser);

        #endregion

    }

}