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

            if (inter is not System.DateTime dt) return null;

            // get the configuration
            DateTimeConfiguration? config = propertyType.DataType.Configuration as DateTimeConfiguration;

            // Find the selected time zone
            TimeZoneInfo timeZone = GetTimeZoneInfo(config);

            // Convert to UNIX time from the specified seconds
            EssentialsTime timestamp = new(dt, timeZone);

            // Return the a value of selected value type
            return config?.ValueType switch {
                "EssentialsDate" => new EssentialsDate(timestamp),
                "DateTime" => timestamp.DateTimeOffset.DateTime,
                "DateTimeOffset" => timestamp.DateTimeOffset,
                _ => timestamp
            };

        }

        public override object? ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) {
            return inter;
        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {


            // Get the configuration
            DateTimeConfiguration? config = propertyType.DataType.Configuration as DateTimeConfiguration;


            // Return the selected value type
            return config?.ValueType switch {
                "EssentialsDate" => typeof(EssentialsDate),
                "DateTime" => typeof(System.DateTime),
                "DateTimeOffset" => typeof(DateTimeOffset),
                _ => typeof(EssentialsTime)
            };

        }

        private TimeZoneInfo GetTimeZoneInfo(DateTimeConfiguration? config) {
            if (string.IsNullOrWhiteSpace(config?.TimeZone)) return TimeZoneInfo.Local;
            return _timeZoneProvider.TryGetTimeZone(config.TimeZone, out ITimeZone? result) ? result!.TimeZoneInfo : TimeZoneInfo.Local;
        }

        #endregion

    }

}