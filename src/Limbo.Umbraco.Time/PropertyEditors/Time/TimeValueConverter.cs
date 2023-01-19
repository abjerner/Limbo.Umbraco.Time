using System;
using Limbo.Umbraco.Time.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

namespace Limbo.Umbraco.Time.PropertyEditors.Time {

    /// <summary>
    /// Value converter for <see cref="TimeEditor"/>.
    /// </summary>
    public class TimeValueConverter : PropertyValueConverterBase {

        /// <inheritdoc />
        public override bool IsConverter(IPublishedPropertyType propertyType) {
            return propertyType.EditorAlias.InvariantEquals(TimeEditor.EditorAlias);
        }

        /// <inheritdoc />
        public override object? ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object? source, bool preview) {
            return source;
        }

        /// <inheritdoc />
        public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) {

            // Get the configuration
            TimeConfiguration? config = propertyType.DataType.Configuration as TimeConfiguration;

            // Is the data type nullable?
            bool nullable = config?.IsNullable ?? false;

            return config?.ValueType switch {
                "TimeOnly" => ConvertToTimeOnly(inter, nullable),
                _ => ConvertToTimeOffset(inter, nullable, config)

            };

        }

        /// <inheritdoc />
        public override object? ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) {
            return inter;
        }

        /// <inheritdoc />
        public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {

            // Get the configuration
            TimeConfiguration? config = propertyType.DataType.Configuration as TimeConfiguration;

            // Is the data type nullable?
            bool nullable = config?.IsNullable ?? false;

            return config?.ValueType switch {
                "TimeOnly" => nullable ? typeof(TimeOnly?) : typeof(TimeOnly),
                _ => typeof(TimeOffset)
            };

        }

        private static object? ConvertToTimeOnly(object? inter, bool nullable) {

            if (inter is string str && TimeSpan.TryParse(str, out TimeSpan time)) {
                return new TimeOnly(time.Ticks);
            }

            return nullable ? null : TimeOnly.MinValue;

        }

        private static object? ConvertToTimeOffset(object? inter, bool nullable, TimeConfiguration? config) {

            if (inter is string str && TimeSpan.TryParse(str, out TimeSpan time)) {
                return new TimeOffset(time, config);
            }

            return nullable ? null : new TimeOffset(TimeSpan.Zero, config);

        }

    }

}