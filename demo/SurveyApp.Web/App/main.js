requirejs.config({
    paths: {
        text: 'durandal/amd/text', //for durandal
        async: 'requirePlugins/async' //required for JSONP google maps plugin to work properly
    }
});

define(['durandal/app', 'durandal/viewLocator', 'durandal/system', 'durandal/viewEngine', 'durandal/plugins/router'],
    function (app, viewLocator, system, viewEngine, router) {
    
    //>>excludeStart("build", true);
    system.debug(true);
    //>>excludeEnd("build");

    app.title = 'Survey App';
    app.start().then(function () {
        //Replace 'viewmodels' in the moduleId with 'views' to locate the view.
        //Look for partial views in a 'views' folder in the root.
        viewLocator.useConvention();
        
        //by default durandal expects html files, override this
        viewEngine.viewExtension = ".cshtml";

        configureRouting();

        app.adaptToDevice();
        
        //Show the app by setting the root view model for our application with a transition.
        app.setRoot('viewmodels/shell', 'entrance');
    });
        
    function configureRouting() {
        //sets default convention of all routes being based off of 'viewmodels'
        router.useConvention();                                             

        //creates a route for surveys that is visible in navigation
        //by convention it determines that the viewmodel location is 'viewmodels/surveys'
        //ex: visiting '#/surveys' results in 'viewmodels/surveys'
        router.mapNav('surveys');

        //creates a route for showing a particular survey
        //':id' extracts the value and it is passed as an argument into the 'activate' function of the viewmodel
        //mapRoute doesn't make the route visible in navigation
        //the viewmodel path doesn't follow the convention so it is explicitly specified
        router.mapRoute('surveys/:id', 'viewmodels/surveys/createEdit');    
    }
});