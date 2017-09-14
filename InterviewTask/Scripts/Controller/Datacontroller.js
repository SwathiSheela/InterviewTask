(function () {
    'use strict';

    angular
        .module('mainApp')
        .controller('Datacontroller', Testcontroller);

    Testcontroller.$inject = ['$scope', 'datafactory'];

    function Testcontroller($scope, datafactory) {
        $scope.title = 'Datacontroller';

        activate();

        $scope.Msg = "Hello World"

        function activate() { }

        function preventMe() {
            $scope.showError = true;
            $scope.showOutput = false;
            $scope.errorMsg = "Please enter valid information"
            return false;
        }

        $scope.formatData = function () {
            var _name = $scope.Name;
            var _price = $scope.Price;
            if (_name === '' || _name === undefined || _price === '' || _price === undefined) {
                return preventMe();
            }

            if (_price.split(".").length > 2 || _price.split("-").length > 2) {
                return preventMe();
            }

            var model = {
                Name: _name,
                Price: _price
            }

            datafactory.getData(model).then(function (response) {
                $scope.showOutput = true;
                $scope.showError = false;
                $scope.formattedData = response.data.data
            }, function (error) {
                $scope.errorMsg = error;
            });

        }
    }
})();
