console.log("meh");

angular.module("umbraco.directives").directive("limboTimeDatePicker", function () {
    return {
        scope: {
            value: "="
        },
        restrict: "E",
        replace: true,
        templateUrl: "/App_Plugins/Limbo.Umbraco.Time/Views/Directives/DatePicker.html",
        link: function (scope) {

            // Since the date field is combined with Angular, the value of the field is a Date() instance, while we
            // wish to save the date using the ISO 8601 date format instead
            scope.datePickerChanged = function () {
                scope.value = scope.date && scope.date.toIsoDateString ? scope.date.toIsoDateString() : null;
            };

            // If "value" already have a value, we parse it into a new Date() instance
            scope.date = scope.value ? new Date(scope.value) : null;

        }
    };
});