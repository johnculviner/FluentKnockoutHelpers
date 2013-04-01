define(function() {

    return function(techProduct) {
        var self = this;

        self.techProduct = techProduct;

        self.cancel = function() {
            self.modal.close(null);
        };
    };
});