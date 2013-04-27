define(['durandal/plugins/router'], function (router) {
    return {
        router: router,
        activate: function () {
            return router.activate('surveys');
        }
    };
});