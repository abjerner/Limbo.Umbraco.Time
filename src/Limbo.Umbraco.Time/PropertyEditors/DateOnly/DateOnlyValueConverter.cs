using System;
using Skybrud.Essentials.Time;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Extensions;

namespace Limbo.Umbraco.Time.PropertyEditors.DateOnly;

/// <summary>
/// Value converter for <see cref="DateOnlyPropertyEditor"/>.
/// </summary>
public class DateOnlyValueConverter : DatePickerValueConverter {

    /// <inheritdoc />
    public override bool IsConverter(IPublishedPropertyType propertyType) {
        return propertyType.EditorAlias.InvariantEquals(DateOnlyPropertyEditor.EditorAlias);
    }

    /// <inheritdoc />
    public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {

        // Get the configuration
        DateOnlyConfiguration? config = propertyType.DataType.ConfigurationAs<DateOnlyConfiguration>();

        // Is the data type nullable?
        bool nullable = config?.IsNullable ?? false;

        return config?.ValueType switch {
            "DateOnly" => nullable ? typeof(System.DateOnly?) : typeof(System.DateOnly),
            "DateTime" => nullable ? typeof(System.DateTime?) : typeof(System.DateTime),
            "DateTimeOffset" => nullable ? typeof(DateTimeOffset?) : typeof(DateTimeOffset),
            "EssentialsTime" => typeof(EssentialsTime),
            _ => typeof(EssentialsDate)
        };

    }

    /// <inheritdoc />
    public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) {

        // Get the configuration
        DateOnlyConfiguration? config = propertyType.DataType.ConfigurationAs<DateOnlyConfiguration>();

        // Is the data type nullable?
        bool nullable = config?.IsNullable ?? false;

        return config?.ValueType switch {
            "DateOnly" => ConvertToDateOnly(inter, nullable),
            "DateTime" => ConvertToSystemDateTime(inter, nullable),
            "DateTimeOffset" => ConvertToSystemDateTimeOffset(inter, nullable),
            "EssentialsTime" => ConvertToEssentialsTime(inter, nullable),
            _ => ConvertToEssentialsDate(inter, nullable)
        };

    }

    private static System.DateOnly? ConvertToDateOnly(object? inter, bool nullable) {

        if (inter is not System.DateTime date) return nullable ? null : System.DateOnly.MinValue;
        if (date == System.DateTime.MinValue && nullable) return null;

        TimePackageUtils.FixDateTimeKind(ref date);

        return new System.DateOnly(date.Year, date.Month, date.Day);

    }

    private static System.DateTime? ConvertToSystemDateTime(object? inter, bool nullable) {

        if (inter is not System.DateTime date) return nullable ? null : System.DateTime.MinValue;
        if (date == System.DateTime.MinValue) return nullable ? null : System.DateTime.MinValue;

        TimePackageUtils.FixDateTimeKind(ref date);

        return new System.DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Local);

    }

    private static DateTimeOffset? ConvertToSystemDateTimeOffset(object? inter, bool nullable) {

        if (inter is not System.DateTime date) return nullable ? null : DateTimeOffset.MinValue;
        if (date == System.DateTime.MinValue) return nullable ? null : DateTimeOffset.MinValue;

        TimePackageUtils.FixDateTimeKind(ref date);

        TimeSpan offset = TimeZoneInfo.Local.GetUtcOffset(date);

        return new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, offset);

    }

    private static EssentialsDate? ConvertToEssentialsDate(object? inter, bool nullable) {

        if (inter is not System.DateTime date) return nullable ? null : EssentialsDate.MinValue;
        if (date == System.DateTime.MinValue && nullable) return null;

        TimePackageUtils.FixDateTimeKind(ref date);

        return new EssentialsDate(date);

    }

    private static EssentialsTime? ConvertToEssentialsTime(object? inter, bool nullable) {

        if (inter is not System.DateTime date) return nullable ? null : EssentialsTime.MinValue;
        if (date == System.DateTime.MinValue) return nullable ? null : EssentialsTime.MinValue;

        TimePackageUtils.FixDateTimeKind(ref date);

        return new EssentialsTime(date, TimeZoneInfo.Local);

    }

}