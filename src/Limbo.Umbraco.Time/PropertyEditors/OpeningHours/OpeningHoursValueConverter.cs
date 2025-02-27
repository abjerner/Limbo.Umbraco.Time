using System;
using Limbo.Umbraco.Time.Models.OpeningHours;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

namespace Limbo.Umbraco.Time.PropertyEditors.OpeningHours;

/// <summary>
/// Value converter for <see cref="OpeningHoursPropertyEditor"/>.
/// </summary>
public class OpeningHoursValueConverter : PropertyValueConverterBase {

    /// <inheritdoc />
    public override bool IsConverter(IPublishedPropertyType propertyType) {
        return propertyType.EditorAlias.InvariantEquals(OpeningHoursPropertyEditor.EditorAlias);
    }

    /// <inheritdoc />
    public override object? ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object? source, bool preview) {
        return source is string str && str.DetectIsJson() ? JsonUtils.ParseJsonObject(str) : null;
    }

    /// <inheritdoc />
    public override object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) {
        return OpeningHoursModel.Create(inter as JObject, propertyType.DataType.Configuration as OpeningHoursConfiguration);
    }

    /// <inheritdoc />
    public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
        return PropertyCacheLevel.Element;
    }

    /// <inheritdoc />
    public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {
        return typeof(OpeningHoursModel);
    }

}