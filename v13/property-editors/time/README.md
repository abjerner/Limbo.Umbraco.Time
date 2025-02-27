# Time Picker

The time picker uses a custom view for letting editors pick a time without any relation to a specific date. The picker returns an instance of `TimeOffset` representing the selected time, and the `Hours` and `Minutes` properties can be used to read the selected selected time.

Calling the `ToString` method will result in a string representation of the date formatted as `HH:mm`. The `ToString` method is also used when serializing the value using [Json.NET](https://www.newtonsoft.com/json).

The output format of the `ToString` method can be controlled via the *Output format* prevalue option in the data type configuration. The format is that of the [TimeSpan](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-timespan-format-strings) class in .NET. Notice that the format is different from that of the `DateTime` class, so a format like `hh\:mm` will result is something like `15:45` (which for `DateTime` would have been `HH:mm`).

![image](https://user-images.githubusercontent.com/3634580/89651798-c0ea6d80-d8c4-11ea-9061-2098428d8ee9.png)