define(['durandal/plugins/router', 'durandal/app'], function (router, app) {
    return {
        router: router,
        activate: function () {
            return router.activate('surveys');
        }
    };
});