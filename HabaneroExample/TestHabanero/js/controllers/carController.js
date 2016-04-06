(function () {
    'use strict';
    var module = angular.module('testHabanero.controllers', []);

    module.controller('carController', [
        '$scope', 'carService', function carController($scope, carService) {
            var self = this;
            self.cars = [];

            var getAllCars = function() {
                return $http.get('/Car/Index');
            }

            var init = function() {
                $q.all([getAllCars()]).then(function (response) {
                    var returnedCars = response.data;
                    if (returnedCars !== null) {
                        self.cars = returnedCars;
                    }
                });
            }
            init();
            return self;
        }
    ]);
})();