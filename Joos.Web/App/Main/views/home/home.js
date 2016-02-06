(function() {
    var controllerId = 'app.views.home';
    angular.module('app').controller(controllerId, [
        '$scope', function($scope) {
            var vm = this;
            vm.userName = 'rishabh';
            vm.func = function () {
                abp.ajax({
                    url: '/api/apitest/vote',
                    method: 'GET'
                }).done(function (data) {
                    abp.notify.success(JSON.stringify(data));
                });
            }
        }
    ]);
})();