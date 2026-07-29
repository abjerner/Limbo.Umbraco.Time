using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.PropertyEditors.DateTime;

// [CHANGE: upgrade to Umbraco 17] Related: DatePropertyEditor.cs, TimePropertyEditor.cs, UnixTimestampPropertyEditor.cs, OpeningHoursPropertyEditor.cs, src/index.ts
// Name, icon, group and the editor view moved to the propertyEditorUi manifest in src/index.ts.

/// <summary>
/// Represents a date and time property editor (server-side schema).
/// </summary>
[DataEditor(EditorAlias, ValueType = EditorValueType)]
public class DateTimePropertyEditor : DataEditor {

    private readonly IIOHelper _ioHelper;

    #region Constants

    public const string EditorAlias = "Limbo.Umbraco.DateTime";

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
