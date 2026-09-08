using Limbo.Umbraco.Time.Constants;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Time.PropertyEditors.DayOfWeek;

[DataEditor(EditorAlias, ValueType = EditorValueType)]
public class DayOfWeekPropertyEditor : DataEditor {

    private readonly IIOHelper _ioHelper;

    public const string EditorAlias = PropertyEditorAliases.DayOfWeek;

    public const string EditorUiAlias = TimePropertyEditorUiAliases.DayOfWeek;

    public const string EditorValueType = ValueTypes.String;

    public DayOfWeekPropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper) : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
    }

    protected override IConfigurationEditor CreateConfigurationEditor() => new DayOfWeekConfigurationEditor(_ioHelper);

}