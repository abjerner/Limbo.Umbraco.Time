## Opening Hours

The package features a custom property editor for specifying opening hours. Opening hours may be specified for the normal workday, also with support for multiple open time slots during the day. The property editor also allows adding special days that may be closed or having different opening hours.

The value of a property using this property editor will be an instance of [OpeningHoursModel](https://github.com/abjerner/Limbo.Umbraco.Time/blob/v13/main/src/Limbo.Umbraco.Time/Models/OpeningHours/OpeningHoursModel.cs), which offers various ways to format the opening hours - as well as doing additional calculations based on the entered opening hours, such as whether a store is currently open.

![image](https://github.com/user-attachments/assets/8f2d30a2-948b-4267-b760-2321c237b74e)