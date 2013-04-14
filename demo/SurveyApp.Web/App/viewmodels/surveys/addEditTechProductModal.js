define(['./techProduct'],
function (techProductViewModel) {

    return function(techProduct) {
        var self = this;

        self.techProduct = techProduct || new techProductViewModel();

        //activate 
        self.validator = ko.validatedObservable(techProduct);
        
        self.dirtyFlag = new ko.DirtyFlag(self.techProduct, false, ko.mapping.toJSON);  //kolite plugin

        
        self.apply = ko.command({ //kolite plugin
            //function to execute if Apply button is clicked
            execute: function () {
                //close resolves a promise to whomever opened the modal
                //here we pass back the modified tech product
                this.modal.close(self.techProduct);
            },
            //button Apply button is disabled if canExecute is false
            canExecute: function (isExecuting) {
                //only allow apply to be active if the user has made changes and the form is valid
                return !isExecuting && self.dirtyFlag().isDirty() && self.validator().isValid();
            }
        });

        self.cancel = function () {
            //close resolves a promise to whomever opened the modal
            //here 'null' indicates no changes were made
            self.modal.close(null);
        };
    };
});