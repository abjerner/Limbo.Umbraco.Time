using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Limbo.Umbraco.Time.Api;

#pragma warning disable CS1591

public class TimeSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions> {

    public void Configure(SwaggerGenOptions options) {

        options.SwaggerDoc(TimeApiConstants.Alias, new OpenApiInfo {
            Title = TimeApiConstants.Name,
            Version = TimeApiConstants.Version
        });

        options.OperationFilter<TimeSecurityFilter>();

    }

}