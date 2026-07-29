using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.PropertyEditors.UnixTime;

// [CHANGE: upgrade to Umbraco 17] Related: DatePropertyEditor.cs, DateTimePropertyEditor.cs, TimePropertyEditor.cs, OpeningHoursPropertyEditor.cs, src/index.ts
// Name, icon, group and the editor view moved to the propertyEditorUi manifest in src/index.ts.

/// <summary>
/// Represents a unix time property editor (server-side schema).
/// </summary>
[DataEditor(EditorAlias, ValueType = EditorValueType)]
public class UnixTimestampPropertyEditor : DataEditor {

    private readonly IIOHelper _ioHelper;

    #region Constants

    public const string EditorAlias = "Limbo.Umbraco.UnixTimestamp";

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
