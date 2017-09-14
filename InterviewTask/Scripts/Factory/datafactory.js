(function () {
    'use strict';

    angular
        .module('mainApp')
        .factory('datafactory', datafactory);

    datafactory.$inject = ['$http'];

    function datafactory($http) {
        var url = '/Home/GetPriceInWords';
        var service = {
            getData: getData
        };

        return service;

        function getData(data) {
            return $http.post(url, data)
        }
    }
})();