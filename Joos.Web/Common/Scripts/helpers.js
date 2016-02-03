var App = App || {};
(function () {

    var appLocalizationSource = abp.localization.getSource('Joos');
    App.localize = function () {
        return appLocalizationSource.apply(this, arguments);
    };

})(App);