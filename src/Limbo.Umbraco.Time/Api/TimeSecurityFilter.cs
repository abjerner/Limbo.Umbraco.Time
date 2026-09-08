using Umbraco.Cms.Api.Management.OpenApi;

namespace Limbo.Umbraco.Time.Api;

internal class TimeSecurityFilter : BackOfficeSecurityRequirementsOperationFilterBase {

    protected override string ApiName => TimeApiConstants.Name;

}