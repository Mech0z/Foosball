(function () {
    'use strict';

    var leaderboardServices = angular.module('leaderboardService', []);

    leaderboardServices.factory('leaderboard', ['$http', '$q', function ($http, $q) {

        return {
            getLeaderboard: function () {
                var deferred = $q.defer();

                $http.get("https://localhost:44335/leaderboard/index")
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
