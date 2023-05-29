angular.module("umbraco").controller("Limbo.Umbraco.Time.OpeningHours.Controller", function ($scope) {

    function parseBoolean(str) {
        str = str + "";
        return str === "1" || str === "true";
    }

    const vm = this;

    // Ensure properties of the configuration object
    $scope.model.config.hideWeekdays = parseBoolean($scope.model.config.hideWeekdays);
    $scope.model.config.hideHolidays = parseBoolean($scope.model.config.hideHolidays);

    // Option to hide property label
    $scope.model.hideLabel = parseBoolean($scope.model.config.hideLabel);

    // Make sure we have a value/object
    if ($scope.model.value === "null") $scope.model.value = null;
    if (!$scope.model.value) {
        vm.value = { weekdays: {}, holidays: [] };
    } else {
        vm.value = {};
        vm.value.weekdays = Object.keys($scope.model.value.weekdays).length > 0 ? $scope.model.value.weekdays : {};
        vm.value.holidays = Array.isArray($scope.model.value.holidays) ? $scope.model.value.holidays : [];
    }

    // Watch for changes to the shadow model
    let flag = true;
    $scope.$watch("vm.value", function(value) {
        if (flag) {
            flag = false;
            return;
        }
        console.log("changed");
        $scope.model.value = {};
        if (value.weekdays && Object.keys(value.weekdays).length > 0) $scope.model.value.weekdays = value.weekdays;
        if (Array.isArray(value.holidays) && value.holidays.length > 0) $scope.model.value.holidays = value.holidays;
        if (Object.keys($scope.model.value).length === 0) $scope.model.value = "";
    }, true);

});