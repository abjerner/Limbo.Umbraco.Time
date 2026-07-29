using Microsoft.Extensions.DependencyInjection;
using Limbo.Umbraco.Time.Providers;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

#pragma warning disable CS1591

namespace Limbo.Umbraco.Time.Composers;

public class TimeComposer : IComposer {

    public void Compose(IUmbracoBuilder builder) {

        // Register the default time zone provider with the DI container
        builder.Services.AddSingleton<ITimeZoneProvider, DefaultTimeZoneProvider>();

        // NOTE: The Umbraco 13 "IManifestFilter" registration is gone. The backoffice extensions are now declared
        // client side, in "wwwroot/umbraco-package.json" (served from "/App_Plugins/Limbo.Umbraco.Time").

    }

}