using Limbo.Umbraco.Time.Constants;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.DateOnly;

[DataEditor(EditorAlias, ValueType = EditorValueType)]
public class DateOnlyPropertyEditor : DataEditor {

    private readonly IIOHelper _ioHelper;

    #region Constants

    public const string EditorAlias = PropertyEditorAliases.DateOnly;

    public const string EditorUiAlias = TimePropertyEditorUiAliases.DateOnly;

    public const string EditorValueType = ValueTypes.String;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DateOnlyPropertyEditor"/> class.
    /// </summary>
    public DateOnlyPropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper) : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
    }

    #endregion

    #region Member methods

    /// <inheritdoc />
    protected override IConfigurationEditor CreateConfigurationEditor() => new DateOnlyConfigurationEditor(_ioHelper);

    #endregion

}