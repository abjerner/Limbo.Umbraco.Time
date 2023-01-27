using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.PropertyEditors.OpeningHours {

    public class OpeningHoursConfigurationEditor : ConfigurationEditor<OpeningHoursConfiguration> {

        public OpeningHoursConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(ioHelper, editorConfigurationParser) { }

    }

}