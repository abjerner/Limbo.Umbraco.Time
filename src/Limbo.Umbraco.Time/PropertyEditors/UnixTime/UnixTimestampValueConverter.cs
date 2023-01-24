using System;
using Skybrud.Essentials.Time;
using Limbo.Umbraco.Time.Models.TimeZones;
using Limbo.Umbraco.Time.Providers;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;
using Skybrud.Essentials.Strings;

#pragma warning disable 1591

namespace Limbo.Umbraco.Time.PropertyEditors.UnixTime {

    public class UnixTimestampValueConverter : PropertyValueConverterBase {

        private readonly ITimeZoneProvider _timeZoneProvider;

        #region Constructors

        public UnixTimestampValueConverter(ITimeZoneProvider timeZoneProvider) {
            _timeZoneProvider = timeZoneProvider;
        }

        #endregion

        #region Member methods

        public override bool IsConverter(IPublishedPropertyType propertyType) {
            return propertyType.EditorAlias.InvariantEquals(UnixTimestampEditor.EditorAlias);
        }

        public override object? ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object? source, bool preview) {
            return source;
        }

        public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) {

            // Get the configuration
            UnixTimestampConfiguration? config = propertyType.DataType.Configuration as UnixTimestampConfiguration;

            // Is the data type nullable?
            bool nullable = config?.IsNullable ?? true;

            // Parse the saved seconds
            int seconds = StringUtils.ParseInt32(inter as string);

            // Find the selected time zone
            TimeZoneInfo timeZone = GetTimeZoneInfo(config);

            // Convert to UNIX time from the specified seconds
            EssentialsTime? timestamp = seconds == 0 ? null : EssentialsTime.FromUnixTimeSeconds(seconds).ToTimeZone(timeZone);

            // Convert to the configured type
            return config?.ValueType switch {
                "DateTime" => ConvertToDateTime(timestamp, nullable),
                "DateTimeOffset" => ConvertToDateTimeOffset(timestamp, nullable),
                "EssentialsDate" => ConvertToEssentialsDate(timestamp, nullable),
                _ => ConvertToEssentialsTime(timestamp, nullable)
            };

        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {

            // Get the configuration
            UnixTimestampConfiguration? config = propertyType.DataType.Configuration as UnixTimestampConfiguration;

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

        private TimeZoneInfo GetTimeZoneInfo(UnixTimestampConfiguration? config) {
            if (string.IsNullOrWhiteSpace(config?.TimeZone)) return TimeZoneInfo.Local;
            return _timeZoneProvider.TryGetTimeZone(config.TimeZone, out ITimeZone? result) ? result.TimeZoneInfo : TimeZoneInfo.Local;
        }

        private static object? ConvertToDateTime(EssentialsTime? timestamp, bool nullable) {

            if (timestamp is null) return nullable ? null : System.DateTime.MinValue;

            System.DateTime dt = timestamp.DateTimeOffset.DateTime;

            if (timestamp.TimeZone?.Id == TimeZoneInfo.Local.Id) {
                return new System.DateTime(dt.Ticks, DateTimeKind.Local);
            }

            if (timestamp.TimeZone?.Id == TimeZoneInfo.Utc.Id) {
                return new System.DateTime(dt.Ticks, DateTimeKind.Utc);
            }

            return dt;

        }

        private static object? ConvertToDateTimeOffset(EssentialsTime? timestamp, bool nullable) {
            if (timestamp is null) return nullable ? null : DateTimeOffset.MinValue;
            return timestamp.DateTimeOffset;
        }

        private static object? ConvertToEssentialsDate(EssentialsTime? timestamp, bool nullable) {
            if (timestamp is null) return nullable ? null : EssentialsDate.MinValue;
            return new EssentialsDate(timestamp);
        }

        private static object? ConvertToEssentialsTime(EssentialsTime? timestamp, bool nullable) {
            if (timestamp is null) return nullable ? null : EssentialsTime.MinValue;
            return timestamp;
        }

        #endregion

    }

}