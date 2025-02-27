## Date Picker

The date picker uses Umbraco's own date picker view, but ensures that the view is configured for selecting a date without any time. The property editor should be used in situations where only the date matters, and as such it returns an instance of [EssentialsDate](https://packages.skybrud.dk/skybrud.essentials/reference/time/essentialsdate/) instead of the normal `DateTime`.

The `EssentialsDate` class comes from our [**Limbo.Essentials** package](https://packages.skybrud.dk/skybrud.essentials/reference/time/essentialsdate/), which provides extended functionality for working with dates compared to `DateTime`.

Calling the `ToString` method on said class will result in a string representation of the date formatted using the [**ISO 8601** date format](https://en.wikipedia.org/wiki/ISO_8601), which is `yyyy-MM-dd` - eg. `2020-08-07`. The same format is used when serializing the value using [Json.NET](https://www.newtonsoft.com/json).

As of this time, the date picker currently doesn't have any prevalues (data type configuration).