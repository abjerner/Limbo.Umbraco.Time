// [CHANGE: upgrade to Umbraco 17] Related: en.ts, da.ts, index.ts - ported from wwwroot/Lang/de-DE.xml
// de-DE and de-CH were identical in the v13 language files, so both cultures share this dictionary.
import type { UmbLocalizationDictionary } from '@umbraco-cms/backoffice/localization-api';

export default {
  limboOpeningHours: {
    weekdayTitle: 'Öffnungszeiten Wochentage',
    holidayTitle: 'Öffnungszeiten Feiertage',
    addHoliday: 'Feiertag hinzufügen',
    day: 'Tag',
    date: 'Datum',
    text: 'Text',
    opens: 'Öffnet',
    closes: 'Schliesst',
    monday: 'Montag',
    tuesday: 'Dienstag',
    wednesday: 'Mittwoch',
    thursday: 'Donnerstag',
    friday: 'Freitag',
    saturday: 'Samstag',
    sunday: 'Sonntag',
    closed: 'Geschlossen',
    addOpeningHours: 'Öffnungszeit hinzufügen',
    from: 'von',
    to: 'bis',
    and: 'und',
    holidayLabelPlaceholder: 'Z.B. "Weihnachtstag"',
    holidayDate: 'Datum',
  },
} as UmbLocalizationDictionary;
