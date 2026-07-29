// [CHANGE: upgrade to Umbraco 17] Related: da.ts, de.ts, index.ts - ported from wwwroot/Lang/en-US.xml
// The v13 package shipped these as AngularJS XML language files. They are now localization manifests.
import type { UmbLocalizationDictionary } from '@umbraco-cms/backoffice/localization-api';

export default {
  limboOpeningHours: {
    weekdayTitle: 'Weekday opening hours',
    holidayTitle: 'Holiday opening hours',
    addHoliday: 'Add holiday',
    day: 'Day',
    date: 'Date',
    text: 'Text',
    opens: 'Opens',
    closes: 'Closes',
    monday: 'Monday',
    tuesday: 'Tuesday',
    wednesday: 'Wednesday',
    thursday: 'Thursday',
    friday: 'Friday',
    saturday: 'Saturday',
    sunday: 'Sunday',
    closed: 'Closed',
    addOpeningHours: 'Add opening hours',
    from: 'from',
    to: 'to',
    and: 'and',
    // New in v17: the holiday label placeholder was hardcoded in the old Holidays.html view.
    holidayLabelPlaceholder: "I.e. 'Christmas Day'",
    holidayDate: 'Date',
  },
} as UmbLocalizationDictionary;
