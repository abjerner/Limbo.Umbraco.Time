# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

`Limbo.Umbraco.Time` — an Umbraco package (NuGet: `Limbo.Umbraco.Time`) adding date/time property editors that return richer types than Umbraco's built-ins: `EssentialsDate`, `EssentialsTime` (from Skybrud.Essentials), `TimeOffset`, and `OpeningHoursModel`.

Two halves: a C# project (`src/Limbo.Umbraco.Time`, `net10.0`, Umbraco 17) and a TypeScript/Lit backoffice client living **inside** that same project folder (`src/`, `package.json`, `vite.config.ts`). No test project, no CI.

See `documentation/v17-upgrade.md` for what changed in the Umbraco 13 → 17 upgrade and why.

## Build

```
dotnet build src/Limbo.Umbraco.Time.sln          # runs npm ci + npm run build first
dotnet build ... -p:SkipClientBuild=true          # C# only, skips the npm steps
npm run build                                     # from src/Limbo.Umbraco.Time: tsc --noEmit + vite build
npm run watch
dotnet pack src/Limbo.Umbraco.Time/Limbo.Umbraco.Time.csproj -c Release
```

Vite emits into `wwwroot/` (gitignored); `wwwroot/umbraco-package.json` is committed and declares a single `bundle` extension pointing at `limbo-time.js`, whose `manifests` export registers everything.

To verify against a real instance, `test/TestSite` (gitignored) is an unattended-install Umbraco 17 site with a project reference and a boot-time verification composer.

## Branching

Branch per Umbraco major: `v1/main`, `v10/main`, `v13/main`, `v17/dev` (current). PRs for the v17 line go to `v17/dev`.

## Architecture

Five property editors, each a folder under `PropertyEditors/<Name>/` with three C# files plus a TypeScript element:

- `<Name>PropertyEditor.cs` — `DataEditor` subclass carrying only the **server-side schema**: `[DataEditor(EditorAlias, ValueType = ...)]`. Name, icon, group and the editor UI are declared client side.
- `<Name>Configuration.cs` — POCO with `[ConfigurationField("key")]`. The key is the only thing the attribute carries in v14+; label/description/field UI live in `src/index.ts` under `meta.settings.properties` and **must use the same alias**.
- `<Name>ConfigurationEditor.cs` — `ConfigurationEditor<T>(IIOHelper)`.
- `<Name>ValueConverter.cs` — reads config via `propertyType.DataType.ConfigurationAs<T>()`. The Date/DateTime/UnixTime converters branch on the configured `ValueType` string (`"DateOnly"`, `"DateTime"`, `"DateTimeOffset"`, `"EssentialsTime"`, …) crossed with `nullable`; `GetPropertyValueType` and `ConvertIntermediateToObject` must stay in sync across every branch.

Aliases are inconsistent by history — `Limbo.Umbraco.Time` (time picker), `Limbo.Umbraco.Date`, `Limbo.Umbraco.DateTime`, `Limbo.Umbraco.UnixTimestamp`, `Limbo.Umbraco.Time.OpeningHours`. **Never rename them**: they are persisted in customer databases, and the TypeScript `propertyEditorUi` alias must equal the C# editor alias so migrated data types resolve the new UI.

### DateTimeKind trap

`TimePackageUtils.FixDateTimeKind(ref DateTime)` promotes `Unspecified` → `Utc`. Umbraco stores node-level date properties in the `dateValue` column (read back as `Utc`), but values nested in a JSON blob (block list/grid) come back `Unspecified`, which .NET then treats as local time and mis-converts. **Every** code path in a value converter that receives a `DateTime` from `inter` must call this before doing time-zone math.

### Other pieces

- `Composers/TimeComposer.cs` — registers `ITimeZoneProvider` → `DefaultTimeZoneProvider` (singleton, caches the system time zone list). Property editors and value converters are auto-discovered.
- `Controllers/Api/Management/TimeZoneController.cs` — Management API controller (`ManagementApiControllerBase`, `[VersionedApiBackOfficeRoute("time")]`) serving `/umbraco/management/api/v1/time/time-zones` to the time zone config UI. Returns `TimeZoneModel` (System.Text.Json), not `ITimeZone`.
- `Models/OpeningHours/` — the only non-trivial model. `OpeningHoursModel.Create(JObject?, OpeningHoursConfiguration?)` parses the raw JSON; serialization goes through `OpeningHoursJsonConverter` (write-only; `DateTime` as `yyyy-MM-dd`, `TimeSpan` truncated to `HH:mm`, weekdays keyed by `(int)DayOfWeek`). This is **Newtonsoft** via Skybrud.Essentials' `Skybrud.Essentials.Json.Newtonsoft.*` helpers — not `System.Text.Json`. The stored JSON shape must stay compatible with `opening-hours.element.ts`.
- `src/*.element.ts` — one Lit element per editor, plus two config-only UIs (`value-type`, `time-zone`). `src/localization/{en,da}.ts` holds the `limboOpeningHours_*` keys.

## Conventions

`.editorconfig` is authoritative and non-default in places: 4-space indent, **CRLF**, no final newline, file-scoped namespaces, `System` usings first, nullable enabled. Public API members carry XML doc comments; files that intentionally skip them use a per-file `#pragma warning disable CS1591`.

Per the user's global rule, changes spanning more than two files carry an inline `// [CHANGE: <reason>] Related: <other files>` comment at each edit site — the upgrade commit follows this throughout.
