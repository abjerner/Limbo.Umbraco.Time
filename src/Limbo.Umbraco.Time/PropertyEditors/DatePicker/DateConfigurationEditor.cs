using System.Collections.Generic;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.PropertyEditors.DatePicker {

    /// <summary>
    /// Configuration editor for <see cref="DateEditor"/>.
    /// </summary>
    public class DateConfigurationEditor : ConfigurationEditor<DateConfiguration> {

        public DateConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(ioHelper, editorConfigurationParser) { }

        /// <inheritdoc />
        public override IDictionary<string, object> ToValueEditor(object? configuration) {
            var d = base.ToValueEditor(configuration);
            d["format"] = "YYYY-MM-DD";
            d["pickTime"] = false;
            return d;
        }

        public override IDictionary<string, object> DefaultConfiguration => new Dictionary<string, object> {
            {"nullable", true}
        };
    }

}