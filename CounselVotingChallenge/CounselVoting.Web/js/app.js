angular.module('counselApp', ['ui.router','ngResource','counselApp.controllers','counselApp.services']);

angular.module('counselApp').config(function($stateProvider, $httpProvider) {
    $stateProvider.state('measures', {
        url: '/measures',
        templateUrl: 'partials/measures.html',
        controller: 'MeasureListController'
    }).state('viewMeasure', {
       url: '/measures/:id/view',
       templateUrl: 'partials/measure-view.html',
       controller: 'MeasureViewController'
    }).state('newMeasure', {
        url: '/measures/new',
        templateUrl: 'partials/measure-add.html',
        controller: 'MeasureCreateController'
    }).state('editMeasure', {
        url: '/measures/:id/edit',
        templateUrl: 'partials/measure-edit.html',
        controller: 'MeasureEditController'
    });
}).run(function($state) {
   $state.go('measures');
});
