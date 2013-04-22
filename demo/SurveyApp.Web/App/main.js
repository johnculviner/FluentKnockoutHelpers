requirejs.config({
    paths: {
        text: 'durandal/amd/text', //for durandal
        async: 'requirePlugins/async' //required for JSONP google maps plugin to work properly
    }
});

define(['durandal/app', 'durandal/viewLocator', 'durandal/system', 'durandal/viewEngine', 'durandal/plugins/router', 'typeMetadata', 'utility/typeMetadataHelper'],
    function (app, viewLocator, system, viewEngine, router, typeMetadata, typeMetadataHelper) {
    
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
        
        //setup FluentKnockoutHelper's typeMetadataHelper for:
        //•auto wireup of knockout.validation validations based on C# types and DataAnnotations
        //•handling JavaScript 'instanciation' of class hierarchies
        //typeMetadata is a require module defined in Index.cshtml as a result of a C# TypeMetadataHelper.EmitTypeMetadataArray() call
        typeMetadataHelper.configure(typeMetadata);
        
        //configure knockout validations to have allow for nice errored field formatting
        ko.validation.init({ decorateElement: true });
    });
        
    function configureRouting() {
        //sets default convention of all routes being based off of 'viewmodels'
        router.useConvention();                                             

        //creates a route for surveys that is visible in navigation
        //by convention it determines that the viewmodel location is 'viewmodels/surveys'
        //ex: visiting '#/surveys' results in 'viewmodels/surveys'
        router.mapNav('surveys');
        
        router.mapNav('foodgroups', 'viewmodels/foodgroups', 'Food Groups');

        //creates a route for showing a particular survey
        //':id' extracts the value and it is passed as an argument into the 'activate' function of the viewmodel
        //mapRoute doesn't make the route visible in navigation
        //the viewmodel path doesn't follow the convention so it is explicitly specified
        router.mapRoute('surveys/:id', 'viewmodels/surveys/addEditSurveyPage');    
    }
});