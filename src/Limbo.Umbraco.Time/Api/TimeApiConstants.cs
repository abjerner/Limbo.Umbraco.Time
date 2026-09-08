namespace Limbo.Umbraco.Time.Api;

public static class TimeApiConstants {

    public const string Route = "limbo/time";

    public const string Alias = "limbo-time-v1";

    public const string Name = "Limbo Time API v1";

    public const string GroupName = "Limbo Time";

    public const string Version = "1.0";

    public static class GroupNames {

        public const string TimeZones = "Time Zones";

    }

    public static class Routes {

        public const string TimeZones = $"{Route}/time-zones";

    }

}