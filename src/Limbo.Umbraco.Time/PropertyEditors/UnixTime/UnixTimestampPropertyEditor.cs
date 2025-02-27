using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.PropertyEditors.UnixTime;

/// <summary>
/// Represents a unix time property editor.
/// </summary>
[DataEditor(EditorAlias, EditorType.PropertyValue, "Limbo Unix Timestamp", EditorView, Group = "Limbo", Icon = "icon-time color-limbo", ValueType = EditorValueType)]
public class UnixTimestampPropertyEditor : DataEditor {

    private readonly IIOHelper _ioHelper;
    private readonly IEditorConfigurationParser _editorConfigurationParser;

    #region Constants

    public const string EditorAlias = "Limbo.Umbraco.UnixTimestamp";

    public const string EditorView = "/App_Plugins/Limbo.Umbraco.Time/Views/Editors/UnixTimestamp.html";

    /// <remarks>
    /// Value type must be "STRING" to support zero as a value
    /// </remarks>
    public const string EditorValueType = ValueTypes.String;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="UnixTimestampPropertyEditor"/> class.
    /// </summary>
    public UnixTimestampPropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
        _editorConfigurationParser = editorConfigurationParser;
    }

    #endregion

    #region Member methods

    /// <inheritdoc />
    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new UnixTimestampConfigurationEditor(_ioHelper, _editorConfigurationParser);
    }

    #endregion

}