// [CHANGE: upgrade to Umbraco 17] Related: wwwroot/umbraco-package.json, PropertyEditors/**, Manifests/TimeManifestFilter.cs (deleted)
// Bundle entry point, replacing the v13 "TimeManifestFilter" (IManifestFilter) and its list of
// AngularJS controllers/views. The umbraco-package.json "bundle" extension imports this module and
// registers everything exported as `manifests`.
//
// The propertyEditorUi aliases intentionally match the C# Data Editor aliases: the v13->v14
// migration sets a Data Type's EditorUiAlias to the code-based editor's alias, so existing data
// types resolve these UIs automatically. Setting aliases (e.g. "valueType", "timeZone") likewise
// match the C# [ConfigurationField] keys, so stored configuration keeps working unchanged.
import type { ManifestPropertyEditorUi } from '@umbraco-cms/backoffice/property-editor';
import type { ManifestLocalization } from '@umbraco-cms/backoffice/localization';

const VALUE_TYPE_UI_ALIAS = 'Limbo.Umbraco.Time.ValueType';
const TIME_ZONE_UI_ALIAS = 'Limbo.Umbraco.Time.TimeZone';

/** Setting for picking the .NET type returned by the value converter. */
const valueTypeSetting = (items: Array<string>, description: string) => ({
  alias: 'valueType',
  label: 'Value type',
  description,
  propertyEditorUiAlias: VALUE_TYPE_UI_ALIAS,
  config: [{ alias: 'items', value: items }],
});

const timeZoneSetting = (description: string) => ({
  alias: 'timeZone',
  label: 'Time zone',
  description,
  propertyEditorUiAlias: TIME_ZONE_UI_ALIAS,
});

const nullableSetting = {
  alias: 'nullable',
  label: 'Nullable',
  description: 'Specify whether the property value type should be a nullable type.',
  propertyEditorUiAlias: 'Umb.PropertyEditorUi.Toggle',
};

const readonlySetting = {
  alias: 'readonly',
  label: 'Readonly',
  description: 'Specify whether the editor should be readonly.',
  propertyEditorUiAlias: 'Umb.PropertyEditorUi.Toggle',
};

const date: ManifestPropertyEditorUi = {
  type: 'propertyEditorUi',
  alias: 'Limbo.Umbraco.Date',
  name: 'Limbo Date Property Editor UI',
  element: () => import('./date.element.js'),
  meta: {
    label: 'Limbo Date',
    icon: 'icon-calendar',
    group: 'pickers',
    propertyEditorSchemaAlias: 'Limbo.Umbraco.Date',
    supportsReadOnly: true,
    keywords: ['date', 'calendar', 'limbo'],
    settings: {
      properties: [
        valueTypeSetting(
          ['EssentialsDate', 'EssentialsTime', 'DateTime', 'DateTimeOffset', 'DateOnly'],
          'Select the .NET value type returned by properties using this data type.',
        ),
        { ...nullableSetting, description: 'Select whether the returned value is nullable.' },
      ],
      defaultData: [{ alias: 'nullable', value: true }],
    },
  },
};

const dateTime: ManifestPropertyEditorUi = {
  type: 'propertyEditorUi',
  alias: 'Limbo.Umbraco.DateTime',
  name: 'Limbo Date & Time Property Editor UI',
  element: () => import('./datetime.element.js'),
  meta: {
    label: 'Limbo Date & Time',
    icon: 'icon-time',
    group: 'pickers',
    propertyEditorSchemaAlias: 'Limbo.Umbraco.DateTime',
    supportsReadOnly: true,
    keywords: ['date', 'time', 'limbo'],
    settings: {
      properties: [
        timeZoneSetting(
          'Select the time zone of the returned timestamp. This does not affect the value saved in Umbraco.',
        ),
        readonlySetting,
        nullableSetting,
        valueTypeSetting(
          ['EssentialsTime', 'EssentialsDate', 'DateTime', 'DateTimeOffset'],
          'Select the .NET value type returned by properties using this data type.',
        ),
      ],
      defaultData: [{ alias: 'nullable', value: true }],
    },
  },
};

const time: ManifestPropertyEditorUi = {
  type: 'propertyEditorUi',
  alias: 'Limbo.Umbraco.Time',
  name: 'Limbo Time Property Editor UI',
  element: () => import('./time.element.js'),
  meta: {
    label: 'Limbo Time',
    icon: 'icon-time',
    group: 'pickers',
    propertyEditorSchemaAlias: 'Limbo.Umbraco.Time',
    supportsReadOnly: true,
    keywords: ['time', 'clock', 'limbo'],
    settings: {
      properties: [
        valueTypeSetting(
          ['TimeOffset', 'TimeOnly'],
          'Select the .NET value type returned by properties using this data type.',
        ),
        { ...nullableSetting, description: 'Select whether the returned value is nullable.' },
        {
          alias: 'outputFormat',
          label: 'Output format',
          description:
            'Specify the time format used when converting the property value to a string. If left blank, the format will default to "t". See the .NET docs on "Standard date and time format strings". The format is ignored if the value type is TimeOnly.',
          propertyEditorUiAlias: 'Umb.PropertyEditorUi.TextBox',
        },
      ],
      defaultData: [{ alias: 'nullable', value: true }],
    },
  },
};

const unixTimestamp: ManifestPropertyEditorUi = {
  type: 'propertyEditorUi',
  alias: 'Limbo.Umbraco.UnixTimestamp',
  name: 'Limbo Unix Timestamp Property Editor UI',
  element: () => import('./unix-timestamp.element.js'),
  meta: {
    label: 'Limbo Unix Timestamp',
    icon: 'icon-time',
    group: 'pickers',
    propertyEditorSchemaAlias: 'Limbo.Umbraco.UnixTimestamp',
    supportsReadOnly: true,
    keywords: ['unix', 'timestamp', 'epoch', 'limbo'],
    settings: {
      properties: [
        timeZoneSetting(
          'Select the time zone of the returned EssentialsTime. This does not affect the value saved in Umbraco.',
        ),
        {
          alias: 'showUnixTimestamp',
          label: 'Show UNIX timestamp',
          description: 'Show the UNIX timestamp in the editor.',
          propertyEditorUiAlias: 'Umb.PropertyEditorUi.Toggle',
        },
        readonlySetting,
        nullableSetting,
        valueTypeSetting(
          ['EssentialsTime', 'EssentialsDate', 'DateTime', 'DateTimeOffset'],
          'Select the .NET value type returned by properties using this data type.',
        ),
      ],
      defaultData: [{ alias: 'nullable', value: true }],
    },
  },
};

const openingHours: ManifestPropertyEditorUi = {
  type: 'propertyEditorUi',
  alias: 'Limbo.Umbraco.Time.OpeningHours',
  name: 'Limbo Opening Hours Property Editor UI',
  element: () => import('./opening-hours.element.js'),
  meta: {
    label: 'Limbo Opening Hours',
    icon: 'icon-calendar',
    group: 'pickers',
    propertyEditorSchemaAlias: 'Limbo.Umbraco.Time.OpeningHours',
    supportsReadOnly: true,
    keywords: ['opening hours', 'weekdays', 'holidays', 'limbo'],
    settings: {
      properties: [
        {
          alias: 'hideWeekdays',
          label: 'Hide weekdays',
          description: 'If selected, the part of the UI for entering weekdays will not be shown.',
          propertyEditorUiAlias: 'Umb.PropertyEditorUi.Toggle',
        },
        {
          alias: 'hideHolidays',
          label: 'Hide holidays',
          description: 'If selected, the part of the UI for entering holidays will not be shown.',
          propertyEditorUiAlias: 'Umb.PropertyEditorUi.Toggle',
        },
        {
          alias: 'allowMultipleTimeSlots',
          label: 'Allow multiple time slots',
          description: 'Allows editors to specify multiple time slots for a given day.',
          propertyEditorUiAlias: 'Umb.PropertyEditorUi.Toggle',
        },
        {
          alias: 'maxTimeSlots',
          label: 'Max time slots',
          description: 'The maximum amount of time slots that can be added (0 = unlimited).',
          propertyEditorUiAlias: 'Umb.PropertyEditorUi.Integer',
        },
        {
          alias: 'timeFormat',
          label: 'Time format',
          description: 'Specify the time format to be used. Defaults to "HH\\:mm" if not specified.',
          propertyEditorUiAlias: 'Umb.PropertyEditorUi.TextBox',
        },
        {
          alias: 'timeFormatEnglish',
          label: 'Time format (English)',
          description: 'Specify an alternative time format to be used for English cultures. Default is "hh\\:mm tt".',
          propertyEditorUiAlias: 'Umb.PropertyEditorUi.TextBox',
        },
      ],
    },
  },
};

/** Configuration-only UIs (no propertyEditorSchemaAlias - they can only configure other editors). */
const valueTypeUi: ManifestPropertyEditorUi = {
  type: 'propertyEditorUi',
  alias: VALUE_TYPE_UI_ALIAS,
  name: 'Limbo Time Value Type Property Editor UI',
  element: () => import('./value-type.element.js'),
  meta: {
    label: 'Limbo Value Type',
    icon: 'icon-brackets',
    group: 'common',
  },
};

const timeZoneUi: ManifestPropertyEditorUi = {
  type: 'propertyEditorUi',
  alias: TIME_ZONE_UI_ALIAS,
  name: 'Limbo Time Zone Property Editor UI',
  element: () => import('./time-zone.element.js'),
  meta: {
    label: 'Limbo Time Zone',
    icon: 'icon-globe',
    group: 'common',
  },
};

const localizations: Array<ManifestLocalization> = [
  {
    type: 'localization',
    alias: 'Limbo.Umbraco.Time.Localization.En',
    name: 'English',
    meta: { culture: 'en' },
    js: () => import('./localization/en.js'),
  },
  {
    type: 'localization',
    alias: 'Limbo.Umbraco.Time.Localization.Da',
    name: 'Danish',
    meta: { culture: 'da-dk' },
    js: () => import('./localization/da.js'),
  },
  {
    type: 'localization',
    alias: 'Limbo.Umbraco.Time.Localization.DeDe',
    name: 'German (DE)',
    meta: { culture: 'de-de' },
    js: () => import('./localization/de.js'),
  },
  {
    type: 'localization',
    alias: 'Limbo.Umbraco.Time.Localization.DeCh',
    name: 'German (CH)',
    meta: { culture: 'de-ch' },
    js: () => import('./localization/de.js'),
  },
];

export const manifests = [
  date,
  dateTime,
  time,
  unixTimestamp,
  openingHours,
  valueTypeUi,
  timeZoneUi,
  ...localizations,
];
