using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.PropertyEditors.Time;

/// <summary>
/// Represents a time offset property editor.
/// </summary>
[DataEditor(EditorAlias, EditorType.PropertyValue, "Limbo Time", EditorView, Group = "Limbo", Icon = "icon-time color-limbo", ValueType = ValueTypes.String)]
public class TimePropertyEditor : DataEditor {

    private readonly IIOHelper _ioHelper;
    private readonly IEditorConfigurationParser _editorConfigurationParser;

    #region Constants

    public const string EditorAlias = "Limbo.Umbraco.Time";

    public const string EditorView = "/App_Plugins/Limbo.Umbraco.Time/Views/Editors/TimePicker.html";

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TimePropertyEditor"/> class.
    /// </summary>
    public TimePropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
        _editorConfigurationParser = editorConfigurationParser;
    }

    #endregion

    #region Member methods

    /// <inheritdoc/>
    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new TimeConfigurationEditor(_ioHelper, _editorConfigurationParser);
    }

    #endregion

}