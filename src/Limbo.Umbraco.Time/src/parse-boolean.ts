// [CHANGE: upgrade to Umbraco 17] Related: opening-hours.element.ts, datetime.element.ts, unix-timestamp.element.ts
// Data types created by the v13 version of the package stored the boolean settings as the strings
// "1" and "0" (that is what the AngularJS "boolean" configuration view saved, hence the "parseBoolean"
// helper in every one of the old controllers). Those values survive the v13 -> v14+ migration, and
// `Boolean("0")` is `true`, so a plain cast would silently invert them - e.g. hiding the weekdays of
// an existing Opening Hours data type, or making an existing Date & Time editor readonly.
export function parseBoolean(value: unknown): boolean {
  if (typeof value === 'boolean') return value;
  if (typeof value === 'number') return value !== 0;
  if (typeof value === 'string') {
    const normalized = value.trim().toLowerCase();
    return normalized === '1' || normalized === 'true';
  }
  return Boolean(value);
}
