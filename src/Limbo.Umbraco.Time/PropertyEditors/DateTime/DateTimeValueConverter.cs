using System;
using Limbo.Umbraco.Time.Models.TimeZones;
using Limbo.Umbraco.Time.Providers;
using Skybrud.Essentials.Time;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

#pragma warning disable 1591

namespace Limbo.Umbraco.Time.PropertyEditors.DateTime;

// [CHANGE: upgrade to Umbraco 17] Related: DateValueConverter.cs, DateTimeValueConverter.cs, TimeValueConverter.cs, UnixTimestampValueConverter.cs, OpeningHoursValueConverter.cs
// IPublishedDataType.Configuration was renamed to ConfigurationObject in v14+; ConfigurationAs<T>()
// is the typed accessor. Everything else (value types, time zone handling, the DateTimeKind fix) is
// unchanged, so existing content keeps converting to the exact same values.

public class DateTimeValueConverter : PropertyValueConverterBase {

    private readonly ITimeZoneProvider _timeZoneProvider;

    #region Constructors

    public DateTimeValueConverter(ITimeZoneProvider timeZoneProvider) {
        _timeZoneProvider = timeZoneProvider;
    }

    #endregion

    #region Member methods

    public override bool IsConverter(IPublishedPropertyType propertyType) {
        return propertyType.EditorAlias.InvariantEquals(DateTimePropertyEditor.EditorAlias);
    }

    public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
        return PropertyCacheLevel.Element;
    }

    public override object? ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object? source, bool preview) {
        return source;
    }

    public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) {

        // Get the configuration
        DateTimeConfiguration? config = propertyType.DataType.ConfigurationAs<DateTimeConfiguration>();

        // Is the data type nullable?
        bool nullable = config?.IsNullable ?? true;

        return config?.ValueType switch {
            "DateTime" => ConvertToDateTime(inter, nullable),
            "DateTimeOffset" => ConvertToDateTimeOffset(inter, nullable, config),
            "EssentialsDate" => ConvertToEssentialsDate(inter, nullable, config),
            _ => ConvertToEssentialsTime(inter, nullable, config)
        };

    }

    public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {

        // Get the configuration
        DateTimeConfiguration? config = propertyType.DataType.ConfigurationAs<DateTimeConfiguration>();

        // Is the data type nullable?
        bool nullable = config?.IsNullable ?? true;

        // Return the selected value type
        return config?.ValueType switch {
            "EssentialsDate" => typeof(EssentialsDate),
            "DateTime" => nullable ? typeof(System.DateTime?) : typeof(System.DateTime),
            "DateTimeOffset" => nullable ? typeof(DateTimeOffset?) : typeof(DateTimeOffset),
            _ => typeof(EssentialsTime)
        };

    }

    private TimeZoneInfo GetTimeZoneInfo(DateTimeConfiguration? config) {
        if (string.IsNullOrWhiteSpace(config?.TimeZone)) return TimeZoneInfo.Local;
        return _timeZoneProvider.TryGetTimeZone(config.TimeZone, out ITimeZone? result) ? result.TimeZoneInfo : TimeZoneInfo.Local;
    }

    private static System.DateTime? ConvertToDateTime(object? inter, bool nullable) {

        if (inter is not System.DateTime date) return nullable ? null : System.DateTime.MinValue;

        if (date == System.DateTime.MinValue) return nullable ? null : System.DateTime.MinValue;

        // Fix the "Kind" if needed
        TimePackageUtils.FixDateTimeKind(ref date);

        return date.ToLocalTime();

    }

    private DateTimeOffset? ConvertToDateTimeOffset(object? inter, bool nullable, DateTimeConfiguration? config) {

        if (inter is not System.DateTime date) return nullable ? null : DateTimeOffset.MinValue;

        if (date == System.DateTime.MinValue) return nullable ? null : DateTimeOffset.MinValue;

        // Fix the "Kind" if needed
        TimePackageUtils.FixDateTimeKind(ref date);

        return ConvertToEssentialsTime(inter, nullable, config)!.DateTimeOffset;

    }

    private EssentialsDate? ConvertToEssentialsDate(object? inter, bool nullable, DateTimeConfiguration? config) {

        if (inter is not System.DateTime date) return nullable ? null : EssentialsDate.MinValue;

        if (date == System.DateTime.MinValue) return nullable ? null : EssentialsDate.MinValue;

        // Fix the "Kind" if needed
        TimePackageUtils.FixDateTimeKind(ref date);

        return new EssentialsDate(ConvertToEssentialsTime(inter, nullable, config)!);

    }

    private EssentialsTime? ConvertToEssentialsTime(object? inter, bool nullable, DateTimeConfiguration? config) {

        if (inter is not System.DateTime date) return nullable ? null : EssentialsTime.MinValue;

        if (date == System.DateTime.MinValue) return nullable ? null : EssentialsTime.MinValue;

        // Fix the "Kind" if needed
        TimePackageUtils.FixDateTimeKind(ref date);

        // Find the selected time zone
        TimeZoneInfo timeZone = GetTimeZoneInfo(config);

        // Convert to UNIX time from the specified seconds
        EssentialsTime timestamp = new(date, timeZone);

        return timestamp;

    }

    #endregion

}