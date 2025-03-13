using System;

namespace Limbo.Umbraco.Time;

internal class TimePackageUtils {

    /// <remarks>
    /// For values of properties added directly to an actual Umbraco node, Umbraco will save the value in the
    /// <c>dateValue</c> column in the database, and when later read from the database, the kind will be <c>Utc</c>.
    ///
    /// However, for property values that are saved as part of a larger JSON blob (e.g. if inside a block list), the
    /// value is saved as a string in a format that doesn't explicitly specify a time zone. As such, the kind will
    /// instead be <c>Unspecified</c>, so when we try to convert the timestamp to a specific time zone, .NET will
    /// assume the saved timestamp is local time, and not UTC, causing an incorrect result.
    /// </remarks>
    internal static void FixDateTimeKind(ref DateTime value) {
        if (value.Kind == DateTimeKind.Unspecified) value = new DateTime(value.Ticks, DateTimeKind.Utc);
    }

}