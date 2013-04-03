define(['./techProduct'],
function (techProductDefinition) {

    return function(techProduct) {
        var self = this;

        self.techProduct = techProduct || new techProductDefinition();

        self.ok = function () {
            //close resolves a promise to whoever opened the modal, if they are listening
            //in the case of add someone is listening
            self.modal.close(self.techProduct);
        };
    };
});