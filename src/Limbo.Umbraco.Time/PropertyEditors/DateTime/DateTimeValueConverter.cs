using System;
using Limbo.Umbraco.Time.Models.TimeZones;
using Limbo.Umbraco.Time.Providers;
using Skybrud.Essentials.Time;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

#pragma warning disable 1591

namespace Limbo.Umbraco.Time.PropertyEditors.DateTime {

    public class DateTimeValueConverter : PropertyValueConverterBase {

        private readonly ITimeZoneProvider _timeZoneProvider;

        #region Constructors

        public DateTimeValueConverter(ITimeZoneProvider timeZoneProvider) {
            _timeZoneProvider = timeZoneProvider;
        }

        #endregion

        #region Member methods

        public override bool IsConverter(IPublishedPropertyType propertyType) {
            return propertyType.EditorAlias.InvariantEquals(DateTimeEditor.EditorAlias);
        }

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
            return PropertyCacheLevel.Element;
        }

        public override object? ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object? source, bool preview) {
            return source;
        }

        public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) {

            // Get the configuration
            DateTimeConfiguration? config = propertyType.DataType.Configuration as DateTimeConfiguration;

            // Is the data type nullable?
            bool nullable = config?.IsNullable ?? true;

            return config?.ValueType switch {
                "DateTime" => ConvertToDateTime(inter, nullable),
                "DateTimeOffset" => ConvertToDateTimeOffset(inter, nullable, config),
                "EssentialsDate" => ConvertToEssentialsDate(inter, nullable, config),
                _ => ConvertToEssentialsTime(inter, nullable, config)
            };

        }

        public override object? ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) {
            return inter;
        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {

            // Get the configuration
            DateTimeConfiguration? config = propertyType.DataType.Configuration as DateTimeConfiguration;

            // Is the data type nullable?
            bool nullable = config?.IsNullable ?? false;

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

        private static object? ConvertToDateTime(object? inter, bool nullable) {

            if (inter is not System.DateTime date) return nullable ? null : System.DateTime.MinValue;

            if (date == System.DateTime.MinValue) return nullable ? null : System.DateTime.MinValue;

            return date.ToLocalTime();

        }

        private object? ConvertToDateTimeOffset(object? inter, bool nullable, DateTimeConfiguration? config) {

            if (inter is not System.DateTime date) return nullable ? null : DateTimeOffset.MinValue;

            if (date == System.DateTime.MinValue) return nullable ? null : DateTimeOffset.MinValue;

            return ConvertToEssentialsTime(inter, nullable, config)!.DateTimeOffset;

        }

        private object? ConvertToEssentialsDate(object? inter, bool nullable, DateTimeConfiguration? config) {

            if (inter is not System.DateTime date) return nullable ? null : EssentialsDate.MinValue;

            if (date == System.DateTime.MinValue) return nullable ? null : EssentialsDate.MinValue;

            return new EssentialsDate(ConvertToEssentialsTime(inter, nullable, config)!);

        }

        private EssentialsTime? ConvertToEssentialsTime(object? inter, bool nullable, DateTimeConfiguration? config) {

            if (inter is not System.DateTime date) return nullable ? null : EssentialsTime.MinValue;

            if (date == System.DateTime.MinValue) return nullable ? null : EssentialsTime.MinValue;

            // Find the selected time zone
            TimeZoneInfo timeZone = GetTimeZoneInfo(config);

            // Convert to UNIX time from the specified seconds
            EssentialsTime timestamp = new(date, timeZone);

            return timestamp;

        }

        #endregion

    }

}