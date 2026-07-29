using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.PropertyEditors.Time;

// [CHANGE: upgrade to Umbraco 17] Related: DatePropertyEditor.cs, DateTimePropertyEditor.cs, UnixTimestampPropertyEditor.cs, OpeningHoursPropertyEditor.cs, src/index.ts
// Name, icon, group and the editor view moved to the propertyEditorUi manifest in src/index.ts.

/// <summary>
/// Represents a time offset property editor (server-side schema).
/// </summary>
[DataEditor(EditorAlias, ValueType = ValueTypes.String)]
public class TimePropertyEditor : DataEditor {

    private readonly IIOHelper _ioHelper;

    #region Constants

    public const string EditorAlias = "Limbo.Umbraco.Time";

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TimePropertyEditor"/> class.
    /// </summary>
    public TimePropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper) : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
    }

    #endregion

    #region Member methods

    /// <inheritdoc/>
    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new TimeConfigurationEditor(_ioHelper);
    }

    #endregion

}
