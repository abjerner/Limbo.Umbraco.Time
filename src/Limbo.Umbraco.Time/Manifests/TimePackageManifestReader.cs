using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Limbo.Umbraco.Time.Constants;
using Limbo.Umbraco.Time.Models;
using Limbo.Umbraco.Time.PropertyEditors.DateOnly;
using Limbo.Umbraco.Time.PropertyEditors.DateTime;
using Limbo.Umbraco.Time.PropertyEditors.DayOfWeek;
using Limbo.Umbraco.Time.PropertyEditors.OpeningHours;
using Limbo.Umbraco.Time.PropertyEditors.TimeOnly;
using Limbo.Umbraco.Time.PropertyEditors.UnixTime;
using Skybrud.Essentials.Time;
using Skybrud.Essentials.Umbraco.Constants;
using Skybrud.Essentials.Umbraco.Manifests.Extensions;
using Skybrud.Essentials.Umbraco.Manifests.Extensions.PropertyEditors;
using Umbraco.Cms.Core.Manifest;
using Umbraco.Cms.Infrastructure.Manifest;

namespace Limbo.Umbraco.Time.Manifests;

public class TimePackageManifestReader : IPackageManifestReader {

    public const string Alias = TimePackage.Alias;

    public const string Name = TimePackage.Name;

    public async Task<IEnumerable<PackageManifest>> ReadPackageManifestsAsync() {

        List<object> extensions = [];

        extensions.AddRange(GetDayOfWeekExtensions());
        extensions.AddRange(GetDateOnlyExtensions());
        extensions.AddRange(GetDateTimeExtensions());
        extensions.AddRange(GetTimeOnlyExtensions());
        extensions.AddRange(GetUnixTimestampExtensions());
        extensions.AddRange(GetOpeningHoursExtensions());

        extensions.Add(new PropertyEditorUiExtension {
            Alias = TimePropertyEditorUiAliases.ValueType,
            Name = $"{Name}: Value Type Property Editor UI",
            Element = $"/App_Plugins/{Alias}/Elements/ValueList.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo Value Type",
                Icon = "icon-list",
                Group = TimePackage.Name
            }
        });

        extensions.Add(new PropertyEditorUiExtension {
            Alias = TimePropertyEditorUiAliases.TimeZone,
            Name = $"{Name}: Time Zone Property Editor UI",
            Element = $"/App_Plugins/{Alias}/Elements/TimeZone.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo Time Zone",
                Icon = "icon-list",
                Group = TimePackage.Name
            }
        });

        extensions.Add(new PropertyEditorUiExtension {
            Alias = TimePropertyEditorUiAliases.AllowedDays,
            Name = $"{Name}: Allowed Days Property Editor UI",
            Element = $"/App_Plugins/{Alias}/Elements/AllowedDays.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo Allowed Days",
                Icon = "icon-list",
                Group = TimePackage.Name
            }
        });

        extensions.Add(new {
            type = "localization",
            alias = $"{Alias}.Localization.EnUs",
            name = $"{Name}: English (en-US)",
            js = $"/App_Plugins/{Alias}/Localization/en-US.js",
            meta = new {
                culture = "en"
            }
        });

        extensions.Add(new {
            type = "localization",
            alias = $"{Alias}.Localization.DaDk",
            name = $"{Name}: Danish (da-DK)",
            js = $"/App_Plugins/{Alias}/Localization/da-DK.js",
            meta = new {
                culture = "da"
            }
        });

        List<PackageManifest> temp = [
            new() {
                Id = TimePackage.Alias,
                Name = TimePackage.Name,
                AllowTelemetry = true,
                Version = TimePackage.InformationalVersion,
                Extensions = [..extensions],
                Importmap = new PackageManifestImportmap {
                    Imports = new Dictionary<string, string> {
                        {"@limbo/time/elements/button-list", $"/App_Plugins/{Alias}/Elements/ButtonList.js"},
                        {"@limbo/time/utils", $"/App_Plugins/{Alias}/Utils.js"},
                        {"@limbo/time/service", $"/App_Plugins/{Alias}/TimeService.js"},
                        {"@limbo/time/services", $"/App_Plugins/{Alias}/TimeService.js"}
                    }
                }
            }
        ];

        return await Task.FromResult(temp);

    }

    private static PropertyEditorSettingsProperty CreateNullableProperty() {
        return new PropertyEditorSettingsProperty {
            Alias = "nullable",
            Label = "Nullable?",
            Description = "Allow nullable/empty values?",
            PropertyEditorUiAlias = UmbracoPropertyEditorUiAliases.Toggle
        };
    }

    private static PropertyEditorSettingsProperty CreateValueTypeProperty(string[] items) {
        return new PropertyEditorSettingsProperty {
            Alias = "valueType",
            Label = "Value type",
            Description = "Select the .NET value type returned by properties using this data type.",
            PropertyEditorUiAlias = UmbracoPropertyEditorUiAliases.RadioButtonList,
            Config = [
                new PropertyEditorConfigProperty { Alias = "items", Value = items }
            ]
        };
    }

    private static IEnumerable<IExtension> GetDateOnlyExtensions() {

        yield return new PropertyEditorSchemaExtension {
            Alias = DateOnlyPropertyEditor.EditorAlias,
            Name = $"{Name}: Date Only Property Editor Schema",
            Meta = new PropertyEditorSchemaMeta {
                DefaultPropertyEditorUiAlias = DateOnlyPropertyEditor.EditorUiAlias,
                Settings = new PropertyEditorSettings {
                    Properties = [
                        CreateValueTypeProperty(["EssentialsDate", "EssentialsTime", "DateTime", "DateTimeOffset", "DateOnly"]),
                        CreateNullableProperty()
                    ],
                    DefaultData = [
                        new PropertyEditorSettingsDefaultData {
                            Alias = "valueType",
                            Value = nameof(EssentialsDate)
                        }
                    ]
                }
            }
        };

        yield return new PropertyEditorUiExtension {
            Alias = DateOnlyPropertyEditor.EditorUiAlias,
            Name = $"{Name}: Date Only Property Editor UI",
            Element = $"/App_Plugins/{Alias}/Elements/DateOnly.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo Date Only",
                Icon = "icon-calendar-alt",
                Group = TimePackage.Name,
                Keywords = ["date", "calendar", "birthday", "deadline", "day", "anniversary", "expiry", "start", "end", "release"],
                PropertyEditorSchemaAlias = DateOnlyPropertyEditor.EditorAlias
            }
        };

    }

    private static IEnumerable<IExtension> GetDateTimeExtensions() {

        yield return new PropertyEditorSchemaExtension {
            Alias = DateTimePropertyEditor.EditorAlias,
            Name = $"{Name}: Date Time Property Editor Schema",
            Meta = new PropertyEditorSchemaMeta {
                DefaultPropertyEditorUiAlias = DateTimePropertyEditor.EditorUiAlias,
                Settings = new PropertyEditorSettings {
                    Properties = [
                        new PropertyEditorSettingsProperty {
                            Alias = "timeZone",
                            Label = "Time Zone",
                            Description = "Select the time zone of the returned timestamp. This does not affect the value saved in Umbraco.",
                            PropertyEditorUiAlias = TimePropertyEditorUiAliases.TimeZone
                        },
                        CreateValueTypeProperty(["EssentialsTime", "EssentialsDate", "DateTime", "DateTimeOffset", "DateOnly"]),
                        CreateNullableProperty()
                    ],
                    DefaultData = [
                        new PropertyEditorSettingsDefaultData {
                            Alias = "valueType",
                            Value = nameof(EssentialsTime)
                        }
                    ]
                }
            }
        };

        yield return new PropertyEditorUiExtension {
            Alias = DateTimePropertyEditor.EditorUiAlias,
            Name = $"{Name}: Date Time Property Editor UI",
            Element = $"/App_Plugins/{Alias}/Elements/DateTime.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo Date & Time",
                Icon = "icon-time",
                Group = TimePackage.Name,
                Keywords = ["date", "time", "limbo"],
                PropertyEditorSchemaAlias = DateTimePropertyEditor.EditorAlias
            }
        };

    }

    private static IEnumerable<IExtension> GetDayOfWeekExtensions() {

        yield return new PropertyEditorSchemaExtension {
            Alias = DayOfWeekPropertyEditor.EditorAlias,
            Name = $"{Name}: Day of Week Property Editor Schema",
            Meta = new PropertyEditorSchemaMeta {
                DefaultPropertyEditorUiAlias = DayOfWeekPropertyEditor.EditorUiAlias,
                Settings = new PropertyEditorSettings {
                    Properties = [
                        CreateNullableProperty()
                    ]
                }
            }
        };

        yield return new PropertyEditorUiExtension {
            Alias = DayOfWeekPropertyEditor.EditorUiAlias,
            Name = $"{Name}: Day of Week Property Editor UI",
            Element = $"/App_Plugins/{Alias}/Elements/DayOfWeek.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo Day of Week",
                Icon = "icon-calendar",
                Group = TimePackage.Name,
                PropertyEditorSchemaAlias = DayOfWeekPropertyEditor.EditorAlias
            }
        };

    }

    private static IEnumerable<IExtension> GetTimeOnlyExtensions() {

        yield return new PropertyEditorSchemaExtension {
            Alias = TimeOnlyPropertyEditor.EditorAlias,
            Name = $"{Name}: Time Only Property Editor Schema",
            Meta = new PropertyEditorSchemaMeta {
                DefaultPropertyEditorUiAlias = TimeOnlyPropertyEditor.EditorUiAlias,
                Settings = new PropertyEditorSettings {
                    Properties = [
                        CreateValueTypeProperty([nameof(TimeValue), nameof(TimeOnly)]),
                        CreateNullableProperty(),
                        new PropertyEditorSettingsProperty {
                            // TODO: Umbraco adds this to the UI, not the schema. Should we do the same? We also use the format for controlling how the value is formatted/serialized
                            Alias = "timeFormat",
                            Label = "#dateTimePicker_config_timeFormat",
                            PropertyEditorUiAlias = UmbracoPropertyEditorUiAliases.RadioButtonList,
                            Config = [
                                new PropertyEditorConfigProperty {
                                    Alias = "items",
                                    Value = new object[] {
                                        new { Name = "HH:mm", Value = "HH:mm" },
                                        new { Name = "HH:mm:ss", Value = "HH:mm:ss" },
                                    }
                                }
                            ]
                        }
                    ],
                    DefaultData = [
                        new PropertyEditorSettingsDefaultData { Alias = "valueType", Value = nameof(TimeValue) },
                        new PropertyEditorSettingsDefaultData { Alias = "timeFormat", Value = "HH:mm" }
                    ]
                }
            }
        };

        yield return new PropertyEditorUiExtension {
            Alias = TimeOnlyPropertyEditor.EditorUiAlias,
            Name = $"{Name}: Time Only Property Editor UI",
            Element = $"/App_Plugins/{Alias}/Elements/TimeOnly.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo Time Only",
                Icon = "icon-time",
                Group = TimePackage.Name,
                Keywords = ["time", "clock", "hour", "schedule", "duration", "opening", "closing", "start", "end", "minute"],
                PropertyEditorSchemaAlias = TimeOnlyPropertyEditor.EditorAlias
            }
        };

    }

    private static IEnumerable<IExtension> GetUnixTimestampExtensions() {

        yield return new PropertyEditorSchemaExtension {
            Alias = UnixTimestampPropertyEditor.EditorAlias,
            Name = $"{Name}: Unix Timestamp Property Editor Schema",
            Meta = new PropertyEditorSchemaMeta {
                DefaultPropertyEditorUiAlias = UnixTimestampPropertyEditor.EditorUiAlias,
                Settings = new PropertyEditorSettings {
                    Properties = [
                        new PropertyEditorSettingsProperty {
                            Alias = "timeZone",
                            Label = "Time Zone",
                            Description = "Select the time zone of the returned timestamp. This does not affect the value saved in Umbraco.",
                            PropertyEditorUiAlias = TimePropertyEditorUiAliases.TimeZone
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "showUnixTimestamp",
                            Label = "Show UNIX Timestamp",
                            Description = "Show the UNIX timestamp in the editor.",
                            PropertyEditorUiAlias = UmbracoPropertyEditorUiAliases.Toggle
                        },
                        CreateValueTypeProperty(["EssentialsTime", "EssentialsDate", "DateTime", "DateTimeOffset", "DateOnly"]),
                        CreateNullableProperty()
                    ],
                    DefaultData = [
                        new PropertyEditorSettingsDefaultData {
                            Alias = "valueType",
                            Value = nameof(EssentialsTime)
                        }
                    ]
                }
            }
        };

        yield return new PropertyEditorUiExtension {
            Alias = UnixTimestampPropertyEditor.EditorUiAlias,
            Name = $"{Name}: Unix Timestamp Property Editor UI",
            Element = $"/App_Plugins/{Alias}/Elements/UnixTimestamp.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo Unix Timestamp",
                Icon = "icon-time",
                Group = TimePackage.Name,
                Keywords = ["date", "time", "limbo", "unix", "timestamp"],
                PropertyEditorSchemaAlias = UnixTimestampPropertyEditor.EditorAlias
            }
        };

    }

    private static IEnumerable<IExtension> GetOpeningHoursExtensions() {

        yield return new PropertyEditorSchemaExtension {
            Alias = OpeningHoursPropertyEditor.EditorAlias,
            Name = $"{Name}: Opening Hours Property Editor Schema",
            Meta = new PropertyEditorSchemaMeta {
                DefaultPropertyEditorUiAlias = OpeningHoursPropertyEditor.EditorUiAlias,
                Settings = new PropertyEditorSettings {
                    Properties = [
                        new PropertyEditorSettingsProperty {
                            Alias = "hideWeekdays",
                            Label = "Hide Weekdays?",
                            Description = "If selected, the part of the UI for entering weekdays will not be shown.",
                            PropertyEditorUiAlias = UmbracoPropertyEditorUiAliases.Toggle
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "hideHolidays",
                            Label = "Hide holidays?",
                            Description = "If selected, the part of the UI for entering holidays will not be shown.",
                            PropertyEditorUiAlias = UmbracoPropertyEditorUiAliases.Toggle
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "allowMultipleTimeSlots",
                            Label = "Allow multiple time slots",
                            Description = "Allows editors to specify multiple time slots for a given day.",
                            PropertyEditorUiAlias = UmbracoPropertyEditorUiAliases.Toggle
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "maxTimeSlots",
                            Label = "Max time slots",
                            Description = "Specifies the maximum number of time slots allowed for a given day (0 = unlimited).",
                            PropertyEditorUiAlias = UmbracoPropertyEditorUiAliases.Integer
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "hideLabel",
                            Label = "Hide label?",
                            Description = "Set whether to hide the editor label and take up the full width of the editing area.",
                            PropertyEditorUiAlias = UmbracoPropertyEditorUiAliases.Toggle
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "timeFormat",
                            Label = "#dateTimePicker_config_timeFormat",
                            PropertyEditorUiAlias = UmbracoPropertyEditorUiAliases.RadioButtonList,
                            Config = [
                                new PropertyEditorConfigProperty {
                                    Alias = "items",
                                    Value = new object[] {
                                        new { Name = "HH:mm", Value = "HH:mm" },
                                        new { Name = "HH:mm:ss", Value = "HH:mm:ss" },
                                    }
                                }
                            ]
                        },
                        //new PropertyEditorSettingsProperty {
                        //    Alias = "allowedDays",
                        //    Label = "Allowed Days",
                        //    Description = "Set which days are allowed for selection.",
                        //    PropertyEditorUiAlias = TimePropertyEditorUiAliases.AllowedDays
                        //}
                    ],
                    DefaultData = [
                        new PropertyEditorSettingsDefaultData { Alias = "hideWeekdays", Value = false },
                        new PropertyEditorSettingsDefaultData { Alias = "hideHolidays", Value = false },
                        new PropertyEditorSettingsDefaultData { Alias = "allowMultipleTimeSlots", Value = false },
                        new PropertyEditorSettingsDefaultData { Alias = "maxTimeSlots", Value = 0 },
                        new PropertyEditorSettingsDefaultData { Alias = "hideLabel", Value = false },
                        new PropertyEditorSettingsDefaultData { Alias = "timeFormat", Value = "HH:mm" },
                    ]
                }
            }
        };

        yield return new PropertyEditorUiExtension {
            Alias = OpeningHoursPropertyEditor.EditorUiAlias,
            Name = $"{Name}: Opening Hours Property Editor UI",
            Element = $"/App_Plugins/{Alias}/Elements/OpeningHours.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo Opening Hours",
                Icon = "icon-time",
                Group = TimePackage.Name,
                Keywords = ["time", "clock", "hour", "schedule", "duration", "opening", "closing", "start", "end", "minute"],
                PropertyEditorSchemaAlias = OpeningHoursPropertyEditor.EditorAlias
            }
        };

    }

}