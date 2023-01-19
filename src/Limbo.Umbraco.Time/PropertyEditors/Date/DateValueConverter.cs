using System;
using Skybrud.Essentials.Time;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Extensions;

namespace Limbo.Umbraco.Time.PropertyEditors.DatePicker {

    /// <summary>
    /// Value converter for <see cref="DateEditor"/>.
    /// </summary>
    public class DateValueConverter : DatePickerValueConverter {

        /// <inheritdoc />
        public override bool IsConverter(IPublishedPropertyType propertyType) {
            return propertyType.EditorAlias.InvariantEquals(DateEditor.EditorAlias);
        }

        /// <inheritdoc />
        public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {

            // Get the configuration
            DateConfiguration? config = propertyType.DataType.Configuration as DateConfiguration;

            // Is the data type nullable?
            bool nullable = config?.IsNullable ?? false;

            return config?.ValueType switch {
                "DateOnly" => nullable ? typeof(DateOnly?) : typeof(DateOnly),
                "DateTime" => nullable ? typeof(DateTime?) : typeof(DateTime),
                "DateTimeOffset" => nullable ? typeof(DateTimeOffset?) : typeof(DateTimeOffset),
                "EssentialsTime" => typeof(EssentialsTime),
                _ => typeof(EssentialsDate)
            };

        }

        /// <inheritdoc />
        public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) {

            // Get the configuration
            DateConfiguration? config = propertyType.DataType.Configuration as DateConfiguration;

            // Is the data type nullable?
            bool nullable = config?.IsNullable ?? false;

            return config?.ValueType switch {
                "DateOnly" => ConvertToDateOnly(inter, nullable),
                "DateTime" => ConvertToDateTime(inter, nullable),
                "DateTimeOffset" => ConvertToDateTimeOffset(inter, nullable),
                "EssentialsTime" => ConvertToEssentialsTime(inter, nullable),
                _ => ConvertToEssentialsDate(inter, nullable)
            };

        }

        private static object? ConvertToDateOnly(object? inter, bool nullable) {

            if (inter is not DateTime date) return nullable ? null : DateOnly.MinValue;

            if (date == DateTime.MinValue && nullable) return null;

            return new DateOnly(date.Year, date.Month, date.Day);

        }

        private static object? ConvertToDateTime(object? inter, bool nullable) {

            if (inter is not DateTime date) return nullable ? null : DateTime.MinValue;

            if (date == DateTime.MinValue) return nullable ? null : DateTime.MinValue;

            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Local);

        }

        private static object? ConvertToDateTimeOffset(object? inter, bool nullable) {

            if (inter is not DateTime date) return nullable ? null : DateTimeOffset.MinValue;

            if (date == DateTime.MinValue) return nullable ? null : DateTimeOffset.MinValue;

            TimeSpan offset = TimeZoneInfo.Local.GetUtcOffset(date);

            return new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, offset);

        }

        private static object? ConvertToEssentialsDate(object? inter, bool nullable) {

            if (inter is not DateTime date) return nullable ? null : EssentialsDate.MinValue;

            if (date == DateTime.MinValue && nullable) return null;

            return new EssentialsDate(date);

        }

        private static object? ConvertToEssentialsTime(object? inter, bool nullable) {

            if (inter is not DateTime date) return nullable ? null : EssentialsTime.MinValue;

            if (date == DateTime.MinValue) return nullable ? null : EssentialsTime.MinValue;

            // return new EssentialsTime(date, TimeZoneInfo.Local); // <-- time zone is not set due to bug in Skybrud.Essentials
            return new EssentialsTime(date).ToTimeZone(TimeZoneInfo.Local);

        }

    }

}