using System.Collections.Generic;
using System.Linq;
using Limbo.Umbraco.Time.Models.TimeZones;
using Limbo.Umbraco.Time.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Api.Management.Controllers;
using Umbraco.Cms.Api.Management.Routing;

#pragma warning disable 1591

namespace Limbo.Umbraco.Time.Controllers.Api.Management;

/// <summary>
/// Management API controller exposing the time zones of the <see cref="ITimeZoneProvider"/> to the backoffice.
/// </summary>
/// <remarks>
/// Replaces the Umbraco 13 <c>UmbracoAuthorizedApiController</c>, which was removed together with the AngularJS
/// backoffice. Endpoints are served from <c>/umbraco/management/api/v1/time/...</c> and are authorized by the
/// Management API defaults inherited from <see cref="ManagementApiControllerBase"/>.
/// </remarks>
[ApiExplorerSettings(GroupName = "Limbo Time")]
[VersionedApiBackOfficeRoute("time")]
public class TimeZoneController : ManagementApiControllerBase {

    private readonly ITimeZoneProvider _timeZoneProvider;

    #region Constructors

    public TimeZoneController(ITimeZoneProvider timeZoneProvider) {
        _timeZoneProvider = timeZoneProvider;
    }

    #endregion

    #region Public API methods

    [HttpGet("time-zones")]
    [ProducesResponseType<IEnumerable<TimeZoneModel>>(StatusCodes.Status200OK)]
    public IActionResult GetTimeZones() {
        IEnumerable<TimeZoneModel> timeZones = _timeZoneProvider
            .GetTimeZones()
            .Select(x => new TimeZoneModel(x.Id, x.Name));
        return Ok(timeZones);
    }

    #endregion

}
