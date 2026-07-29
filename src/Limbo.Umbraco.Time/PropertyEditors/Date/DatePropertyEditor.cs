using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.PropertyEditors.Date;

// [CHANGE: upgrade to Umbraco 17] Related: DateTimePropertyEditor.cs, TimePropertyEditor.cs, UnixTimestampPropertyEditor.cs, OpeningHoursPropertyEditor.cs, src/index.ts
// The v14+ [DataEditor] only describes the server-side schema. Name, icon, group and the editor
// view now live in the TypeScript propertyEditorUi manifest (src/index.ts).
//
// The value type MUST remain "ValueTypes.String" (as in v13): it decides the storage column of the
// property value ("String" => "varcharValue", "DateTime" => "dateValue"). Changing it would leave
// every value saved by the v13 version of the package stranded in a column Umbraco no longer reads.
// "DateValueConverter" inherits "DatePickerValueConverter", which parses the stored string into a
// "DateTime" in "ConvertSourceToIntermediate", so the read side is unaffected.

/// <summary>
/// Represents a date picker property editor (server-side schema).
/// </summary>
[DataEditor(EditorAlias, ValueType = ValueTypes.String)]
public class DatePropertyEditor : DataEditor {

    private readonly IIOHelper _ioHelper;

    #region Constants

    public const string EditorAlias = "Limbo.Umbraco.Date";

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DatePropertyEditor"/> class.
    /// </summary>
    public DatePropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper) : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
    }

    #endregion

    #region Member methods

    /// <inheritdoc />
    protected override IConfigurationEditor CreateConfigurationEditor() => new DateConfigurationEditor(_ioHelper);

    #endregion

}
