using System;
using Skybrud.Essentials.Enums;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

namespace Limbo.Umbraco.Time.PropertyEditors.DayOfWeek;

public class DayOfWeekValueConverter : PropertyValueConverterBase {

    public override bool IsConverter(IPublishedPropertyType propertyType) {
        return propertyType.EditorAlias.InvariantEquals(DayOfWeekPropertyEditor.EditorAlias);
    }

    public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {

        DayOfWeekConfiguration config = GetConfig(propertyType);

        return config.IsNullable ? typeof(System.DayOfWeek?) : typeof(System.DayOfWeek);

    }

    public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) {

        DayOfWeekConfiguration config = GetConfig(propertyType);

        if (config.IsNullable) {
            return EnumUtils.TryParseEnum(inter as string, out System.DayOfWeek? dayOfWeek) ? dayOfWeek : null;
        } else {
            return EnumUtils.TryParseEnum(inter as string, out System.DayOfWeek dayOfWeek) ? dayOfWeek : System.DayOfWeek.Monday;
        }

    }

    private static DayOfWeekConfiguration GetConfig(IPublishedPropertyType propertyType) {
        return propertyType.DataType.ConfigurationObject as DayOfWeekConfiguration ?? new DayOfWeekConfiguration();
    }

}