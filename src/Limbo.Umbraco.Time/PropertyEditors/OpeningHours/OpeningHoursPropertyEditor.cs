using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.PropertyEditors.OpeningHours;

// [CHANGE: upgrade to Umbraco 17] Related: DatePropertyEditor.cs, DateTimePropertyEditor.cs, TimePropertyEditor.cs, UnixTimestampPropertyEditor.cs, src/index.ts
// Name, icon, group and the editor view moved to the propertyEditorUi manifest in src/index.ts, so
// the EditorName/EditorGroup/EditorIcon/EditorView constants are gone.

/// <summary>
/// Represents a property editor for specifying opening hours (server-side schema).
/// </summary>
[DataEditor(EditorAlias, ValueType = ValueTypes.Json)]
public class OpeningHoursPropertyEditor : DataEditor {

    private readonly IIOHelper _ioHelper;

    #region Constants

    public const string EditorAlias = "Limbo.Umbraco.Time.OpeningHours";

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="OpeningHoursPropertyEditor"/> class.
    /// </summary>
    public OpeningHoursPropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper) : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
    }

    #endregion

    #region Member methods

    /// <inheritdoc />
    protected override IConfigurationEditor CreateConfigurationEditor() => new OpeningHoursConfigurationEditor(_ioHelper);

    #endregion

}
