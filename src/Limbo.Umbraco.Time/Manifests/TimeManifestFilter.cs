using System.Collections.Generic;
using System.Reflection;
using Umbraco.Cms.Core.Manifest;

namespace Limbo.Umbraco.Time.Manifests;

/// <inheritdoc />
public class TimeManifestFilter : IManifestFilter {

    /// <inheritdoc />
    public void Filter(List<PackageManifest> manifests) {

        // Initialize a new manifest filter for this package
        PackageManifest manifest = new() {
            AllowPackageTelemetry = true,
            PackageName = TimePackage.Name,
            Version = TimePackage.InformationalVersion,
            BundleOptions = BundleOptions.Independent,
            Scripts = new[] {
                $"/App_Plugins/{TimePackage.Alias}/Scripts/Controllers/TimePicker.js",
                $"/App_Plugins/{TimePackage.Alias}/Scripts/Controllers/DateTime.js",
                $"/App_Plugins/{TimePackage.Alias}/Scripts/Controllers/TimeZone.js",
                $"/App_Plugins/{TimePackage.Alias}/Scripts/Controllers/TimeZoneOverlay.js",
                $"/App_Plugins/{TimePackage.Alias}/Scripts/Controllers/UnixTimestamp.js",
                $"/App_Plugins/{TimePackage.Alias}/Scripts/Controllers/DateTimeValueType.js",
                $"/App_Plugins/{TimePackage.Alias}/Scripts/Controllers/DateValueType.js",
                $"/App_Plugins/{TimePackage.Alias}/Scripts/Controllers/TimeValueType.js",
                $"/App_Plugins/{TimePackage.Alias}/Scripts/Controllers/OpeningHours.js",
                $"/App_Plugins/{TimePackage.Alias}/Scripts/Directives/DatePicker.js",
                $"/App_Plugins/{TimePackage.Alias}/Scripts/Directives/Holidays.js",
                $"/App_Plugins/{TimePackage.Alias}/Scripts/Directives/Weekdays.js"
            },
            Stylesheets = new[] {
                $"/App_Plugins/{TimePackage.Alias}/Styles/Default.css"
            }
        };

        // The "PackageId" property isn't available prior to Umbraco 12, and since the package is build against
        // Umbraco 10, we need to use reflection for setting the property value for Umbraco 12+. Ideally this
        // shouldn't fail, but we might at least add a try/catch to be sure
        try {
            PropertyInfo? property = manifest.GetType().GetProperty("PackageId");
            property?.SetValue(manifest, TimePackage.Alias);
        } catch {
            // We don't really care about the exception
        }

        // Append the manifest
        manifests.Add(manifest);

    }

}