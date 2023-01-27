angular.module("umbraco").controller("Limbo.Umbraco.TimeZone.Overlay.Controller", function ($scope) {

    const vm = this;

    vm.close = function () {
        $scope.model.close($scope.model);
    };

    vm.selectItem = function (item) {
        $scope.model.selectedItem = item;
        $scope.model.submit($scope.model);
    };

});