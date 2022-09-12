angular.module("umbraco").controller("Limbo.Umbraco.Time.ValueType.Controller", function ($scope) {

    const vm = this;

    vm.valueTypes = [
        { alias: "EssentialsTime" },
        { alias: "EssentialsDate" },
        { alias: "DateTime" },
        { alias: "DateTimeOffset" }
    ];

    vm.select = function (valueType) {
        $scope.model.value = valueType.alias;
        vm.valueTypes.forEach(function (vt) {
            vt.selected = vt === valueType;
        });
    };

    function init() {
        let flag = false;
        vm.valueTypes.forEach(function (vt) {
            vt.selected = vt.alias === $scope.model.value;
            if (vt.selected) flag = true;
        });
        if (!flag) vm.valueTypes[0].selected = true;
    }

    init();

});
