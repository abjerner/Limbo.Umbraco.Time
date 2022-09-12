﻿angular.module("umbraco").controller("Limbo.Umbraco.Timestamp.Controller", function ($scope) {

    var vm = this;

    $scope.model.readonly = $scope.model.config.readonly === true;

    vm.config = {
        inline: false,
        enableTime: true,
        dateFormat: "Y-m-d H:i:ss",
        momentFormat: "YYYY-MM-DD HH:mm:ss",
        time_24hr: true
    };

    vm.date = null;
    vm.value = null;

    vm.datePickerChange = function (selectedDates, dateStr) {
        vm.value = dateStr;
        vm.rawValue = dateStr;
        vm.date = selectedDates.length > 0 ? selectedDates[0] : null;
        update();
    };

    vm.clearDate = function () {
        vm.value = null;
        vm.rawValue = null;
        vm.date = null;
        update();
    };

    vm.inputChanged = function () {

        if (vm.rawValue === "") {

            vm.clearDate();

        } else if (vm.rawValue) {

            let momentDate = moment(vm.rawValue, vm.config.momentFormat, true);

            if (!momentDate || !momentDate.isValid()) {
                momentDate = moment(new Date(vm.rawValue));
            }

            if (momentDate && momentDate.isValid()) {

                console.log(momentDate);

                vm.date = momentDate.toDate();
                vm.value = vm.rawValue = momentDate.format(vm.config.momentFormat);

                update();

            }

        }

    };

    function update() {

        if (!vm.value) {
            $scope.model.value = null;
            return;
        }

        if (!vm.date) {
            $scope.model.value = null;
            return;
        }

        // Copy the current date so we don't modify the value
        const d = new Date(vm.date.getTime());

        // Adjust for timezone offset
        d.setSeconds(d.getSeconds() + d.getTimezoneOffset() * 60);

        // Convert the Date to a string
        $scope.model.value = d.toIsoDateTimeString();

    }

    function init() {

        if ($scope.model.value === null) return;
        if ($scope.model.value === "") return;

        // Convert from string to Date
        vm.date = new Date($scope.model.value);

        // Adjust for timezone offset
        vm.date.setSeconds(vm.date.getSeconds() - new Date().getTimezoneOffset() * 60);

        // Update the value for the UI
        vm.value = vm.rawValue = moment(vm.date).format(vm.config.momentFormat);

    }

    init();

});
