angular.module('counselApp.services', []).factory('MeasureService', function($resource) {
    return $resource('http://localhost:14358/api/v1/measures/:id', {id:'@_id'}, {
        update: {
            method: 'PUT'
        }
    });
}).service('popupService', function($window) {
    this.showConfirmPopup = function(message) {
        return $window.confirm(message);
    };
    
    this.showInformationPopup = function(message) {
        return $window.alert(message);
    };
});
