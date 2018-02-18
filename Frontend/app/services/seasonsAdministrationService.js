(function () {
    'use strict';

    var seasonsAdministration = angular.module('seasonsAdministrationService', []);

    seasonsAdministration.factory('seasonsAdministration', ['$http', '$q', "$cookieStore", function ($http, $q, $cookieStore) {

        return {
            startNewSeason: function () {
                var deferred = $q.defer();

                var voidRequest = {
                    Email: $cookieStore.get("email"),
                    Password: $cookieStore.get("password")
                };

                $http.post("https://localhost:44335/SeasonsAdministration/StartNewSeason", voidRequest)
                .success(function (data, status, headers, config) {
                    deferred.resolve(data);
                }).error(function (data, status, headers, config) {
                    deferred.reject("Error loading data");
                });
                return deferred.promise;
            },
            getSeasons: function (email) {
                var deferred = $q.defer();
                
                var voidRequest = {
                    Email: $cookieStore.get("email"),
                    Password: $cookieStore.get("password")
                };

                $http.post("https://localhost:44335/SeasonsAdministration/GetSeasons", voidRequest)
                .success(function (data, status, headers, config) {
                    deferred.resolve(data);
                }).error(function (data, status, headers, config) {
                    deferred.reject("Error loading data");
                });
                return deferred.promise;
            }
        };

    }]);
})();