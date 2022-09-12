using System.Collections.Generic;
using Skybrud.Essentials.Strings.Extensions;
using Umbraco.Cms.Core.Manifest;

namespace Limbo.Umbraco.Time.Manifests {

    /// <inheritdoc />
    public class TimeManifestFilter : IManifestFilter {

        /// <inheritdoc />
        public void Filter(List<PackageManifest> manifests) {
            manifests.Add(new PackageManifest {
                PackageName = TimePackage.Alias.ToKebabCase(),
                BundleOptions = BundleOptions.Independent,
                Scripts = new[] {
                    $"/App_Plugins/{TimePackage.Alias}/Scripts/Controllers/TimePicker.js",
                    $"/App_Plugins/{TimePackage.Alias}/Scripts/Controllers/Timestamp.js",
                    $"/App_Plugins/{TimePackage.Alias}/Scripts/Controllers/TimeZone.js",
                    $"/App_Plugins/{TimePackage.Alias}/Scripts/Controllers/UnixTimestamp.js"
                },
                Stylesheets = new[] {
                    $"/App_Plugins/{TimePackage.Alias}/Styles/Default.css"
                }
            });
        }

    }

}