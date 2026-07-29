using System.Collections.Generic;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.PropertyEditors.Date;

// [CHANGE: upgrade to Umbraco 17] Related: DateTimeConfigurationEditor.cs, TimeConfigurationEditor.cs, UnixTimestampConfigurationEditor.cs, OpeningHoursConfigurationEditor.cs
// ConfigurationEditor<T> now takes only IIOHelper (IEditorConfigurationParser was removed). The old
// "ToValueEditor" override injected AngularJS specific values (format/pickTime) into the view; the
// new date UI is date-only by design instead.

/// <summary>
/// Configuration editor for <see cref="DatePropertyEditor"/>.
/// </summary>
public class DateConfigurationEditor : ConfigurationEditor<DateConfiguration> {

    public DateConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }

    public override IDictionary<string, object> DefaultConfiguration => new Dictionary<string, object> {
        {"nullable", true}
    };

}
