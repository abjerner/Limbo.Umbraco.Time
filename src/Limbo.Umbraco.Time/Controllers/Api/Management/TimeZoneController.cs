using System.Collections.Generic;
using System.Linq;
using Asp.Versioning;
using Limbo.Umbraco.Time.Api;
using Limbo.Umbraco.Time.Models.TimeZones;
using Limbo.Umbraco.Time.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Api.Common.Attributes;
using Umbraco.Cms.Api.Management.Controllers;
using Umbraco.Cms.Api.Management.Routing;
using Umbraco.Cms.Web.Common.Authorization;

#pragma warning disable 1591

namespace Limbo.Umbraco.Time.Controllers.Api.Management;

/// <summary>
/// Management API controller exposing the time zones of the <see cref="ITimeZoneProvider"/> to the backoffice.
/// </summary>
[ApiController]
[MapToApi(TimeApiConstants.Alias)]
[Authorize(Policy = AuthorizationPolicies.SectionAccessContent)]
[ApiVersion(TimeApiConstants.Version)]
[ApiExplorerSettings(GroupName = TimeApiConstants.GroupNames.TimeZones)]
[VersionedApiBackOfficeRoute(TimeApiConstants.Routes.TimeZones)]
public class TimeZoneController : ManagementApiControllerBase {

    private readonly ITimeZoneProvider _timeZoneProvider;

    #region Constructors

    public TimeZoneController(ITimeZoneProvider timeZoneProvider) {
        _timeZoneProvider = timeZoneProvider;
    }

    #endregion

    #region Public API methods

    [HttpGet("")]
    [ProducesResponseType<IEnumerable<TimeZoneModel>>(StatusCodes.Status200OK)]
    public IActionResult GetTimeZones() {
        IEnumerable<TimeZoneModel> timeZones = _timeZoneProvider
            .GetTimeZones()
            .Select(x => new TimeZoneModel(x.Id, x.Name));
        return Ok(timeZones);
    }

    #endregion

}