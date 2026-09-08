using System;
using Limbo.Umbraco.Time.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

namespace Limbo.Umbraco.Time.PropertyEditors.TimeOnly;

/// <summary>
/// Value converter for <see cref="TimeOnlyPropertyEditor"/>.
/// </summary>
public class TimeOnlyValueConverter : PropertyValueConverterBase {

    /// <inheritdoc />
    public override bool IsConverter(IPublishedPropertyType propertyType) {
        return propertyType.EditorAlias.InvariantEquals(TimeOnlyPropertyEditor.EditorAlias);
    }

    /// <inheritdoc />
    public override object? ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object? source, bool preview) {
        return source;
    }

    /// <inheritdoc />
    public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) {

        // Get the configuration
        TimeOnlyConfiguration? config = propertyType.DataType.ConfigurationObject as TimeOnlyConfiguration;

        // Is the data type nullable?
        bool nullable = config?.IsNullable ?? false;

        return config?.ValueType switch {
            "TimeOnly" => ConvertToTimeOnly(inter, nullable),
            _ => ConvertToTimeOffset(inter, nullable, config)

        };

    }

    /// <inheritdoc />
    public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {

        // Get the configuration
        TimeOnlyConfiguration? config = propertyType.DataType.ConfigurationObject as TimeOnlyConfiguration;

        // Is the data type nullable?
        bool nullable = config?.IsNullable ?? false;

        return config?.ValueType switch {
            "TimeOnly" => nullable ? typeof(System.TimeOnly?) : typeof(System.TimeOnly),
            _ => typeof(TimeValue)
        };

    }

    private static System.TimeOnly? ConvertToTimeOnly(object? inter, bool nullable) {

        if (inter is string str && TimeSpan.TryParse(str, out TimeSpan time)) {
            return new System.TimeOnly(time.Ticks);
        }

        return nullable ? null : System.TimeOnly.MinValue;

    }

    private static TimeValue? ConvertToTimeOffset(object? inter, bool nullable, TimeOnlyConfiguration? config) {

        if (inter is string str && System.TimeOnly.TryParse(str, out System.TimeOnly time)) {
            return new TimeValue(time, config);
        }

        return nullable ? null : new TimeValue(System.TimeOnly.MinValue, config);

    }

}