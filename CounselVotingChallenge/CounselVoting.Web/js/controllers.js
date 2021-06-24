 angular.module('counselApp.controllers',[]).controller('MeasureListController', function ($scope, $state, popupService, $window, MeasureService) {

    $scope.measures = MeasureService.query();

    $scope.deleteMeasure = function (measure) {
        if (popupService.showConfirmPopup('Do you really want to delete this?')) {
            measure.$delete(function () {
                $window.location.href = '';
            });
        }
    }

}).controller('MeasureViewController', function ($scope, $http, $state, $stateParams, popupService, MeasureService) {

    $scope.voteMeasure = function() {
        var data = {
            measureId: $stateParams.id,
            name: $scope.voteName,
            voteChoice: $scope.voteChoice
        };
        $http.post('http://localhost:14358/api/v1/measures/vote', JSON.stringify(data)).then(function(){
            //popupService.showInformationPopup('Vote computed successfully!');
            $scope.measure = $scope.measure = MeasureService.get({ id:$stateParams.id });
            $scope.resetVoteForm();
        }, function() {
            popupService.showInformationPopup('Error while trying to compute the vote. Try again.');
        });
    };

    $scope.resetVoteForm = function() {
        $scope.voteName = '';
        $scope.voteChoice = 'No';
    };

    $scope.measure = $scope.measure = MeasureService.get({ id:$stateParams.id });

    $scope.voteName = '';
    $scope.voteChoice = 'No';

}).controller('MeasureCreateController', function ($scope, $state, $stateParams, MeasureService) {

    $scope.measure = new MeasureService();

    $scope.addMeasure = function() {
        $scope.measure.$save(function(){
            $state.go('measures');
        });
    }

}).controller('MeasureEditController', function ($scope, $state, $stateParams, MeasureService) {

    $scope.updateMeasure = function() {
        $scope.measure.$update(function () {
            $state.go('measures');
        });
    };

    $scope.loadMeasure = function() {
        $scope.measure = MeasureService.get({ id:$stateParams.id });
    };

    $scope.loadMeasure();
});