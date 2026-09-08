using Limbo.Umbraco.Time.Api;
using Limbo.Umbraco.Time.Manifests;
using Limbo.Umbraco.Time.Providers;
using Microsoft.Extensions.DependencyInjection;
using Skybrud.Essentials.Umbraco.Composing;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Limbo.Umbraco.Time.Composers;

public class TimeComposer : IComposer {

    public void Compose(IUmbracoBuilder builder) {

        // Register the custom package manifest reader
        builder.AddPackageManifestReader<TimePackageManifestReader>();

        // Register the default time zone provider with the DI container
        builder.Services.AddSingleton<ITimeZoneProvider, DefaultTimeZoneProvider>();

        // Register the SwaggerGen options for the Time API
        builder.Services.ConfigureOptions<TimeSwaggerGenOptions>();

    }

}