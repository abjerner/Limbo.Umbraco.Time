using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace Limbo.Umbraco.Time.PropertyEditors.OpeningHours {

    /// <summary>
    /// Represents a property editor for specifying opening hours.
    /// </summary>
    [DataEditor(EditorAlias, EditorType.PropertyValue, EditorName, EditorView, ValueType = ValueTypes.Json, Group = EditorGroup, Icon = EditorIcon)]
    public class OpeningHoursEditor : DataEditor {

        private readonly IIOHelper _ioHelper;
        private readonly IEditorConfigurationParser _editorConfigurationParser;

        #region Constants

        internal const string EditorAlias = "Limbo.Umbraco.Time.OpeningHours";

        internal const string EditorName = "Limbo Opening Hours";

        internal const string EditorGroup = "Limbo";

        internal const string EditorIcon = "icon-calendar color-limbo";

        internal const string EditorView = "/App_Plugins/Limbo.Umbraco.Time/Views/Editors/OpeningHours.html";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OpeningHoursEditor"/> class.
        /// </summary>

        public OpeningHoursEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(dataValueEditorFactory) {
            _ioHelper = ioHelper;
            _editorConfigurationParser = editorConfigurationParser;
        }

        #endregion

        #region Member methods

        /// <inheritdoc />
        protected override IConfigurationEditor CreateConfigurationEditor() => new OpeningHoursConfigurationEditor(_ioHelper, _editorConfigurationParser);

        #endregion

    }

}