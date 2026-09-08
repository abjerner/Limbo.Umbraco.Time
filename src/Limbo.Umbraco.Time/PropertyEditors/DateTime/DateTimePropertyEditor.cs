using Limbo.Umbraco.Time.Constants;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.PropertyEditors.DateTime;

/// <summary>
/// Represents a date and time property editor.
/// </summary>
[DataEditor(EditorAlias, ValueType = EditorValueType)]
public class DateTimePropertyEditor : DataEditor {

    private readonly IIOHelper _ioHelper;

    #region Constants

    public const string EditorAlias = PropertyEditorAliases.DateTime;

    public const string EditorUiAlias = TimePropertyEditorUiAliases.DateTime;

    public const string EditorValueType = ValueTypes.DateTime;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimePropertyEditor"/> class.
    /// </summary>
    public DateTimePropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper) : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
    }

    #endregion

    #region Member methods

    /// <inheritdoc />
    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new DateTimeConfigurationEditor(_ioHelper);
    }

    #endregion

}