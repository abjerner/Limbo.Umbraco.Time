using Limbo.Umbraco.Time.Constants;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.PropertyEditors.OpeningHours;

/// <summary>
/// Represents a property editor for specifying opening hours.
/// </summary>
[DataEditor(EditorAlias, ValueType = EditorValueType)]
public class OpeningHoursPropertyEditor : DataEditor {

    private readonly IIOHelper _ioHelper;

    #region Constants

    public const string EditorAlias = PropertyEditorAliases.OpeningHours;

    public const string EditorUiAlias = TimePropertyEditorUiAliases.OpeningHours;

    public const string EditorValueType = ValueTypes.Json;

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
