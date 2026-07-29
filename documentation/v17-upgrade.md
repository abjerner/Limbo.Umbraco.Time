# Umbraco 13 → 17 upgrade recap

This document recaps the work done to upgrade **Limbo.Umbraco.Time** from Umbraco 13 to Umbraco 17.

## Why it's a rewrite, not a bump

Umbraco 14 removed the AngularJS backoffice. Everything client-side from the v13 package had to be
**rewritten** as TypeScript/Lit web components registered through a `umbraco-package.json` manifest:

- 13 AngularJS controllers/directives (`Scripts/Controllers/*.js`, `Scripts/Directives/*.js`)
- 10 HTML views (`Views/Editors/*.html`, `Views/Directives/*.html`)
- the LESS/CSS stylesheet and the Web Compiler `compilerconfig.json`
- four XML language files (`wwwroot/Lang/*.xml`)
- `TimeManifestFilter` (`IManifestFilter`)
- the `UmbracoAuthorizedApiController` serving the time zone list

The C# layer survives but changed shape. Stored content, property editor aliases and configuration
keys are unchanged, so this is a pure upgrade with **no data migration**.

## Decisions taken

| Topic | Decision |
|-------|----------|
| Migration strategy | Native v17 rewrite, **keep all five aliases** so existing data types and content keep working |
| Build tooling | Vite + TypeScript + Lit, client project inside the csproj, output to `wwwroot/` (same layout as `Limbo.Umbraco.TextBox`) |
| Config location | Strongly-typed config stays in C# (read by the value converters); the **editing UI** for settings moves to the manifest (`meta.settings.properties`) — required by v14+ |
| Config field UIs | Built-in `Umb.PropertyEditorUi.Toggle` / `Integer` / `TextBox`, plus two custom config-only UIs (value type + time zone) that store a plain string like the old views did |
| Backoffice API | `UmbracoAuthorizedApiController` → Management API controller (`ManagementApiControllerBase`) at `/umbraco/management/api/v1/time/time-zones` |
| Time zone picker | Old `editorService` overlay replaced by a `uui-select` populated from the Management API |
| Localization | The `limboOpeningHours_*` keys are ported from the old XML language files to TS localization manifests (en, da-dk, de-de, de-ch) |
| Dependencies | `Umbraco.Cms.Core` + `Umbraco.Cms.Api.Management` `[17.0.0,18.0.0)`; dropped `Umbraco.Cms.Web.BackOffice` (13.x is its last version) and `Umbraco.Cms.Web.Website` (unused); Skybrud.Essentials → 1.1.68 |
| Target framework | `net10.0` (Umbraco 17 requirement) |
| Version | `17.0.0` |

## C# changes

- **`Limbo.Umbraco.Time.csproj`** — `net10.0`; package references above; version `17.0.0`;
  project/documentation URLs to `/v17/`; MSBuild targets run `npm ci` + `npm run build` before the
  .NET build so `dotnet pack` produces a complete package (skip with `-p:SkipClientBuild=true`).
  Dropped the AngularJS asset `ItemGroup` and the LESS compile config.
- **Five `DataEditor` classes** (`Date`, `DateTime`, `Time`, `UnixTimestamp`, `OpeningHours`) —
  reduced to the v17 `[DataEditor(alias, ValueType = ...)]` shape. `EditorType`, `Name`, `Group`,
  `Icon` and the `EditorView` constant are gone; those now live in the manifest. The `ValueType` of
  each editor is **unchanged from v13** — it decides the storage column of the property value
  (`String` → `varcharValue`, `DateTime` → `dateValue`, `Json` → `textValue`), so changing it would
  strand every value written by the v13 version of the package. In particular `Limbo.Umbraco.Date`
  stays on `ValueTypes.String`.
- **Five `*Configuration` classes** — `[ConfigurationField("key")]` now only carries the storage key
  (v14+ change); labels, descriptions and field UIs moved to `src/index.ts`.
- **Five `*ConfigurationEditor` classes** — `ConfigurationEditor<T>` now takes only `IIOHelper`
  (`IEditorConfigurationParser` was removed). `DateConfigurationEditor.ToValueEditor` (which injected
  the AngularJS `format`/`pickTime` values) was dropped; the new date UI is date-only by design.
- **Five value converters** — `IPublishedDataType.Configuration` → `ConfigurationAs<T>()`. All value
  type mapping, time zone handling and the `FixDateTimeKind` block list fix are untouched.
- **`TimeController` → `Controllers/Api/Management/TimeZoneController`** — `ManagementApiControllerBase`
  with `[VersionedApiBackOfficeRoute("time")]`, returning the new `TimeZoneModel` (`System.Text.Json`
  annotated) instead of `ITimeZone`, which exposes a `TimeZoneInfo` that shouldn't be on the wire.
- **Removed** — `Manifests/TimeManifestFilter.cs`. `TimeComposer` now only registers `ITimeZoneProvider`.
- **`Models/OpeningHours`** — only fix needed was swapping `Skybrud.Essentials.Json.Extensions` for
  `Skybrud.Essentials.Json.Newtonsoft.Extensions` (obsolete in Skybrud.Essentials 1.1.68).

## Frontend (new)

Vite + TypeScript + Lit inside the csproj. Built bundles go to `wwwroot/` and are gitignored;
`wwwroot/umbraco-package.json` is committed.

- `wwwroot/umbraco-package.json` — a single `bundle` extension importing the built entry.
- `src/index.ts` — five `propertyEditorUi` manifests + two config-only UIs + two `localization`
  manifests. Settings UI under `meta.settings.properties`.
- `src/date.element.ts` — date-only picker; stores `"YYYY-MM-DD 00:00:00"`.
- `src/datetime.element.ts` — datetime-local picker; stores `"YYYY-MM-DD HH:mm:ss"`.
- `src/time.element.ts` — time picker; stores `"HH:mm"` (replaces the two hour/minute number inputs).
- `src/unix-timestamp.element.ts` — datetime-local picker storing UNIX seconds as a string, with the
  optional "Show UNIX timestamp" readout.
- `src/opening-hours.element.ts` — weekday and holiday tables with 5-minute time slot selects, add/
  remove slots, `maxTimeSlots` enforcement. Replaces the `limboTimeWeekdays`, `limboTimeHolidays` and
  `limboTimeDatePicker` directives.
- `src/value-type.element.ts` — config-only UI listing the allowed .NET value types per editor
  (passed through the manifest as `config: [{ alias: 'items', value: [...] }]`).
- `src/time-zone.element.ts` — config-only UI reading the Management API; requires the backoffice
  token from `UMB_AUTH_CONTEXT`.
- `src/localization/{en,da,de}.ts` — the `limboOpeningHours_*` keys, ported from the deleted
  `wwwroot/Lang/*.xml` files (de-DE and de-CH were identical, so both cultures share `de.ts`).

## Alias mapping (why existing data types keep working)

These are **code-based** editors. The v13 → v14 migration assigns a data type's `EditorUiAlias` to the
editor's existing alias and **retains** `EditorAlias`. The TypeScript `propertyEditorUi` aliases
therefore equal the C# editor aliases:

| Editor | Alias (C# schema = TS UI) |
|--------|---------------------------|
| Date | `Limbo.Umbraco.Date` |
| Date & Time | `Limbo.Umbraco.DateTime` |
| Time | `Limbo.Umbraco.Time` |
| UNIX Timestamp | `Limbo.Umbraco.UnixTimestamp` |
| Opening Hours | `Limbo.Umbraco.Time.OpeningHours` |

Configuration keys (`valueType`, `nullable`, `timeZone`, `readonly`, `showUnixTimestamp`,
`hideWeekdays`, `hideHolidays`, `allowMultipleTimeSlots`, `maxTimeSlots`, `timeFormat`,
`timeFormatEnglish`, `outputFormat`) are unchanged, so stored data type configuration is read as-is.

## Behavioural changes

- **Date & Time**: the v13 AngularJS controller shifted the picked value by the browser's UTC offset
  before saving. The new element saves the wall clock time as picked — the same thing Umbraco's own
  date picker does in v14+. The data type's time zone is still applied server side by
  `DateTimeValueConverter`, so read-side behaviour is unchanged.
- **`hideLabel`** (Opening Hours): no longer has any effect. Label visibility is controlled on the
  property type in v14+. The setting is kept for backwards compatible configuration but is no longer
  exposed in the data type UI.
- **Time zone picker**: a searchable overlay in v13, a `uui-select` in v17.
- **Time slot inputs**: `<select>` elements are used for the opening hours times (as in v13) rather
  than UUI comboboxes, to keep the 288-entry lists snappy.

## Known follow-ups (not part of this upgrade)

- The public models (`OpeningHoursModel` and friends, `TimeOffset`, `ITimeZone`) are annotated for
  **Newtonsoft.Json** only. Serializing them through `System.Text.Json` — e.g. the Delivery API —
  does not honour those annotations. This is unchanged from v13, so nothing regressed, but it is
  worth addressing separately.
- Localizations cover the same four cultures as v13 (en, da-dk, de-de, de-ch).

## Verification

Verified against a real Umbraco 17.5 instance (unattended install, SQLite, gitignored under `test/`):

- Clean boot, no DI errors — all five `DataEditor`s resolve from `PropertyEditorCollection`.
- All five data types created through `IDataTypeService`, and `ConfigurationObject` deserialized back
  into the right typed configuration class with the right values (e.g. `Limbo.Umbraco.DateTime` →
  `DateTimeConfiguration { valueType = EssentialsTime, timeZone = Europe/Copenhagen, nullable = true }`).
- A JSON value in the **v13 opening hours format** parses through `OpeningHoursModel` unchanged
  (Monday open, 1 time slot, 1 holiday).
- `GET /App_Plugins/Limbo.Umbraco.Time/umbraco-package.json` and every built JS chunk → **200**.
- The backoffice manifest endpoint lists the package and its bundle extension.
- `GET /umbraco/management/api/v1/time/time-zones` → **401** unauthenticated, **200** with a
  backoffice token, returning 420 time zones as `{ id, name }` — the shape the time zone element expects.
  The endpoint is present in the Management API OpenAPI document.
- `npm run build` runs `tsc --noEmit` (clean) followed by the Vite build; `dotnet build` is warning free.

Not verified: rendering of the Lit elements in a browser (no browser automation available in this
environment). The elements compile and type-check, but the editors should be clicked through in the
backoffice before release.
