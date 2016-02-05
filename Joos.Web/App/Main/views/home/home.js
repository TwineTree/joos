(function() {
    var controllerId = 'app.views.home';
    angular.module('app').controller(controllerId, [
        '$scope', function($scope) {
            var vm = this;
            vm.userName = 'rishabh';
            vm.func = function () {
                abp.ajax({
                    url: '/api/questions/Add'
                }).done(function (data) {
                    abp.notify.success('juice');
                });
            }
        }
    ]);
})();