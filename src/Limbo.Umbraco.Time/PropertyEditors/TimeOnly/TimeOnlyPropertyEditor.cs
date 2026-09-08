using Limbo.Umbraco.Time.Constants;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.TimeOnly;

/// <summary>
/// Represents a time only property editor.
/// </summary>
[DataEditor(EditorAlias, ValueType = EditorValueType)]
public class TimeOnlyPropertyEditor : DataEditor {

    private readonly IIOHelper _ioHelper;

    #region Constants

    public const string EditorAlias = PropertyEditorAliases.TimeOnly;

    public const string EditorUiAlias = TimePropertyEditorUiAliases.TimeOnly;

    public const string EditorValueType = ValueTypes.String;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeOnlyPropertyEditor"/> class.
    /// </summary>
    public TimeOnlyPropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper) : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
    }

    #endregion

    #region Member methods

    /// <inheritdoc/>
    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new TimeOnlyConfigurationEditor(_ioHelper);
    }

    #endregion

}