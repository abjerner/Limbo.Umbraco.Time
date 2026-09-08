import { umbHttpClient } from "@umbraco-cms/backoffice/http-client";

/**
 * Adds the specified number of days to a date. A negative value subtracts days
 * from the date instead.
 *
 * @param {Date} date - The date to which the days should be added.
 * @param {number} days - The number of days to add. May be negative to subtract days.
 * @returns {Date} A new Date instance representing the resulting date.
 */
function addDays(date, days) {
    const result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
}

/**
 * Returns the date of Easter Sunday for the specified year according to the
 * Gregorian (Western) calendar.
 *
 * The Julian calendar used by some Eastern Orthodox churches may observe Easter
 * on a different date and is not supported.
 *
 * @param {number} year - The year for which to calculate Easter Sunday.
 * @returns {Date} The date of Easter Sunday for the specified year.
 * @see https://github.com/skybrud/Skybrud.Essentials/blob/v1.1.69/src/Skybrud.Essentials/Time/CalendarUtils.cs#L87
 */
function getEasterSunday(year) {

    const a = year % 19;
    const b = Math.floor(year / 100.00);

    const c = year % 100;
    const d = Math.floor(b / 4.00);

    const e = b % 4;
    const f = Math.floor((b + 8) / 25.00);
    const g = Math.floor((b - f + 1) / 3.00);

    const h = (19 * a + b - d - g + 15) % 30;
    const i = Math.floor(c / 4.00);

    const k = c % 4;
    const l = (32 + 2 * e + 2 * i - h - k) % 7;

    const m = Math.floor((a + 11 * h + 22 * l) / 451.00);

    const n = Math.floor((h + l - 7 * m + 114) / 31.00);
    const p = (h + l - 7 * m + 114) % 31 + 1;

    return new Date(year, n - 1, p);

}

/**
 * Returns the date of Maundy Thursday ("Skærtorsdag" in Danish) for the specified
 * year according to the Gregorian (Western) calendar.
 *
 * The Julian calendar used by some Eastern Orthodox churches may observe Easter
 * on a different date and is not supported.
 *
 * @param {number} year - The year for which to calculate Maundy Thursday.
 * @returns {Date} The date of Maundy Thursday for the specified year.
 */
function getMaundyThursday(year) {
    return addDays(getEasterSunday(year), -3);
}

/**
 * Returns the date of Good Friday ("Langfredag" in Danish) for the specified year
 * according to the Gregorian (Western) calendar.
 *
 * The Julian calendar used by some Eastern Orthodox churches may observe Easter
 * on a different date and is not supported.
 *
 * @param {number} year - The year for which to calculate Good Friday.
 * @returns {Date} The date of Good Friday for the specified year.
 */
function getGoodFriday(year) {
    return addDays(getEasterSunday(year), -2);
}

/**
 * Returns the date of Ascension Day ("Kristi himmelfartsdag" in Danish) for the
 * specified year according to the Gregorian (Western) calendar.
 *
 * @param {number} year - The year for which to calculate Ascension Day.
 * @returns {Date} The date of Ascension Day for the specified year.
 */
function getAscensionDay(year) {
    return addDays(getEasterSunday(year), 39);
}

/**
 * Returns the date of Whit Sunday ("Pinsedag" in Danish) for the specified year
 * according to the Gregorian (Western) calendar.
 *
 * @param {number} year - The year for which to calculate Whit Sunday.
 * @returns {Date} The date of Whit Sunday for the specified year.
 */
function getWhitSunday(year) {
    return addDays(getEasterSunday(year), 49);
}

/**
 * Returns the date of Whit Monday ("2. pinsedag" in Danish) for the specified
 * year according to the Gregorian (Western) calendar.
 *
 * @param {number} year - The year for which to calculate Whit Monday.
 * @returns {Date} The date of Whit Monday for the specified year.
 */
function getWhitMonday(year) {
    return addDays(getEasterSunday(year), 50);
}







/**
 * Returns a new Date instance based on the specified year, month, and day.
 *
 * @param {number} year - The full year, for example, 2026.
 * @param {number} month - The one-based month, where 1 is January and 12 is December.
 * @param {number} day - The day of the month.
 * @returns {Date} A new Date instance representing the specified date.
 */
function getDate(year, month, day) {
    return new Date(year, month - 1, day);
}

/**
 * Gets the date of the first occurrence of the specified weekday in a given month.
 *
 * @param {number} year - The full year, for example, 2026.
 * @param {number} month - The zero-based month, where 0 is January and 11 is December.
 * @param {number|string} weekday - The weekday to find. Can be a number from 0 (Sunday)
 * to 6 (Saturday), or a lowercase weekday name such as "monday".
 * @returns {Date} The date of the first occurrence of the specified weekday in the month.
 */
function getFirstWeekdayOfMonth(year, month, weekday) {
    if (typeof weekday === "string") {
        switch (weekday) {
            case "sunday": weekday = 0; break;
            case "monday": weekday = 1; break;
            case "tuesday": weekday = 2; break;
            case "wednesday": weekday = 3; break;
            case "thursday": weekday = 4; break;
            case "friday": weekday = 5; break;
            case "saturday": weekday = 6; break;
        }
    }
    const firstDay = new Date(year, month, 1);
    const day = firstDay.getDay();
    const offset = (weekday - day + 7) % 7;
    return new Date(year, month, 1 + offset);
}

const general = [
    { alias: "palmSunday", callback: (year) => addDays(getEasterSunday(year), -7) },
    { alias: "easterSunday", callback: (year) => getEasterSunday(year) },
    { alias: "easterMonday", callback: (year) => addDays(getEasterSunday(year), +1) },
    { alias: "maundyThursday", callback: (year) => getMaundyThursday(year) },
    { alias: "goodFriday", callback: (year) => getGoodFriday(year) },
    { alias: "ascensionDay", callback: (year) => getAscensionDay(year) },
    { alias: "whitSunday", callback: (year) => getWhitSunday(year) },
    { alias: "whitMonday", callback: (year) => getWhitMonday(year) },
    { alias: "christmasEve", callback: (year) => getDate(year, 12, 24) },
    { alias: "christmasDay", callback: (year) => getDate(year, 12, 25) },
    { alias: "boxingDay", callback: (year) => getDate(year, 12, 26) },
    { alias: "newYearsEve", callback: (year) => getDate(year, 12, 31) },
    { alias: "newYearsDay", callback: (year) => getDate(year, 1, 1) }
];

const canada = [
    { alias: "ca_canadaDay", callback: (year) => getDate(year, 7, 1) },
    { alias: "ca_labourDay", callback: (year) => getFirstWeekdayOfMonth(year, 9, 1) }, // Labour
        /// Day is a public holiday celebrated on the first Monday in September
    { alias: "ca_thanksgivingDay", callback: (year) => addDays(getFirstWeekdayOfMonth(year, 10, 1), 7) } // Thanksgiving Day occurs on the second Monday in October
];

const denmark = [
    { alias: "dk_generalPrayerDay", callback: (year) => addDays(getGoodFriday(year), 7 * 4) },
    { alias: "dk_constitutionDay", callback: (year) => getDate(year, 6, 5) }
];

const unitedStates = [
    { alias: "us_independenceDay", callback: (year) => getDate(year, 7, 4) },
    { alias: "us_patriotDay", callback: (year) => getDate(year, 9, 11) },
    { alias: "us_laborDay", callback: (year) => getFirstWeekdayOfMonth(year, 9, 1) },
    { alias: "us_memorialDay", callback: (year) => getFirstWeekdayOfMonth(year, 5, 1) },
    { alias: "us_veteransDay", callback: (year) => getDate(year, 11, 11) },
    { alias: "us_thanksgivingDay", callback: (year) => addDays(getFirstWeekdayOfMonth(year, 11, "thursday"), 7 * 4) }
];

const categories = [
    {
        alias: "general",
        days: general
    },
    {
        alias: "canada",
        days: canada
    },
    {
        alias: "denmark",
        days: denmark
    },
    {
        alias: "unitedStates",
        days: unitedStates
    }
];

/**
 * Returns the next date for the specified day relative to the current date.
 *
 * @param {Object} day - The day for which to determine the next date.
 * @param {Function} day.callback - A callback that returns the date for a specified year.
 * @param {Date} now - The date from which to determine the next occurrence.
 * @returns {Date} The next occurrence of the specified day.
 */
function getNextDate(day, now) {
    var date = day.callback(now.getFullYear());
    return date < now ? day.callback(now.getFullYear() + 1) : date;
}

export class CalendarService {

    /**
     * Returns the available calendar categories and their days, including the
     * next occurrence of each day relative to the specified date.
     *
     * @param {Date} [now=new Date()] - The date from which to determine the next occurrences.
     * @returns {Array<Object>} The available calendar categories and their days.
     */
    static getCategories(now) {
        if (!now) now = new Date();
        return categories.map(function(c) {
            return {
                alias: c.alias,
                days: c.days.map(function(d) {
                    return {
                        alias: d.alias,
                        nextDate: getNextDate(d, now)
                    };
                })
            };
        });
    }

}

export class TimeService {

    /**
     * Returns a list of the available time zones.
     *
     * @returns {Promise<Object>} The response containing the available time zones.
     */
    static async getTimeZones() {
        return await umbHttpClient.get({
            url: "/umbraco/management/api/v1/limbo/time/time-zones",
            security: [{ type: "http", scheme: "bearer" }],
        });
    }

}

export default TimeService;