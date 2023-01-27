angular.module("umbraco").controller("Limbo.Umbraco.TimeZone.Controller", function ($scope, $http, editorService) {

    const vm = this;

    vm.loading = true;
    vm.timeZone = null;
    vm.timeZones = null;

    vm.change = function () {
        $scope.model.value = vm.timeZone.id;
    };

    $http.get("/umbraco/backoffice/Limbo/Time/GetTimeZones").then(function (res) {

        vm.loading = false;
        vm.timeZones = res.data;

        if ($scope.model.value) {
            vm.timeZones.forEach(function (tz) {
                tz.icon = "icon-time";
                tz.description = tz.id;

                if (tz.name.indexOf("Server time zone:") === 0) {
                    tz.name = tz.name.substr(18);
                    tz.description = "Local Server Time";
                }

                if (tz.id === $scope.model.value) {
                    vm.timeZone = tz;
                }
            });
        }

        if (!vm.timeZone) {
            vm.timeZone = vm.timeZones.find(x => x.id === "local");
            $scope.model.value = "local";
        }

    });

    vm.openPicker = function () {

        editorService.open({
            title: "Select time zone",
            size: "medium",
            view: "/App_plugins/Limbo.Umbraco.Time/Views/Editors/TimeZoneOverlay.html",
            availableItems: vm.timeZones,
            close: function () {
                editorService.close();
            },
            submit: function (model) {
                vm.timeZone = model.selectedItem;
                vm.change();
                editorService.close();
            }
        });

    };

});