﻿(function () {
    'use strict';

    var achievementsServices = angular.module('achievementsService', []);

    achievementsServices.factory('achievements', ['$http', '$q', function ($http, $q) {

        return {
            getAchievements: function () {
                var deferred = $q.defer();

                $http.get("https://localhost:44335/achievements/index")
                   .success(function (data, status, headers, config) {
                       console.log(data);
                       deferred.resolve(data);
                   }).error(function (data, status, headers, config) {
                       deferred.reject("Server error");
                   });

                return deferred.promise;
            }
        };

    }]);
})();