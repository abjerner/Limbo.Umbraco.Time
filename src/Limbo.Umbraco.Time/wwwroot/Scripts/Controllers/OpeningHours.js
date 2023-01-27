angular.module("umbraco").controller("Limbo.Umbraco.Time.OpeningHours.Controller", function ($scope) {

    function parseBoolean(str) {
        str = str + "";
        return str === "1" || str === "true";
    }

    // Ensure properties of the configuration object
    $scope.model.config.hideWeekdays = parseBoolean($scope.model.config.hideWeekdays);
    $scope.model.config.hideHolidays = parseBoolean($scope.model.config.hideHolidays);

    // Option to hide property label
    $scope.model.hideLabel = parseBoolean($scope.model.config.hideLabel);

    // Make sure we have a value/object
    if (!$scope.model.value) $scope.model.value = {};

});