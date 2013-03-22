define(['knockoutPlugins/bindingHandlers/bingMap'],
function () {
    
    //have this 'class' simply to allow googleMap binding handler to be loaded
    return function (location) {
        $.extend(this, location);
    };
});