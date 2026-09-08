using Limbo.Umbraco.Time.Constants;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.PropertyEditors.UnixTime;

/// <summary>
/// Represents a unix time property editor.
/// </summary>
[DataEditor(EditorAlias, ValueType = EditorValueType)]
public class UnixTimestampPropertyEditor : DataEditor {

    private readonly IIOHelper _ioHelper;

    #region Constants

    public const string EditorAlias = PropertyEditorAliases.UnixTimestamp;

    public const string EditorUiAlias = TimePropertyEditorUiAliases.UnixTimestamp;

    /// <remarks>
    /// Value type must be "STRING" to support zero as a value
    /// </remarks>
    public const string EditorValueType = ValueTypes.String;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="UnixTimestampPropertyEditor"/> class.
    /// </summary>
    public UnixTimestampPropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper) : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
    }

    #endregion

    #region Member methods

    /// <inheritdoc />
    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new UnixTimestampConfigurationEditor(_ioHelper);
    }

    #endregion

}
