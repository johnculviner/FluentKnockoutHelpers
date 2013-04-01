define(['utility/typeHelper'],
function (typeHelper) {
    return function (apiTechProduct) {
        var self = this;

        //here NO custom mappings being performed here but we want a
        //javascript representation (on 'this') of the various derivations of a C# TechProduct
        ko.mapping.fromJS(apiTechProduct, {}, self);


        //#region Computer
        self.isDesktop = ko.computed(function () {
            return typeHelper.isType(self, 'Desktop');
        });
        
        self.isLaptop = ko.computed(function () {
            return typeHelper.isType(self, 'Laptop');
        });

        self.isComputer = ko.computed(function() {
            return self.isDesktop() || self.isLaptop();
        });
        
        self.computerBasicSpecs = ko.computed(function () {
            return self.Mhz() + " Mhz Processor w/ " + self.GigsOfRam() + " GB RAM and " + (self.HasSsd() ? "an SSD" : "no SSD (bummer)");
        }, this, { deferEvaluation: true });
        //#endregion

        //#region Digital camera
        self.isPointAndShoot = ko.computed(function () {
            return typeHelper.isType(self, 'PointAndShoot');
        });

        self.isSlr = ko.computed(function () {
            return typeHelper.isType(self, 'Slr');
        });

        self.isDigitalCamera = ko.computed(function () {
            return self.isPointAndShoot() || self.isSlr();
        });

        self.digitalCameraBasicSpecs = ko.computed(function() {
            return self.MegaPixels() + ' Megapixels';
        }, this, { deferEvaluation: true });
        //#endregion

        //#region Summary Display Information
        self.productTypeDisplay = ko.computed(function () {
            if (self.isDesktop())
                return "Desktop";
            
            if (self.isLaptop())
                return "Laptop";
            
            if (self.isSlr())
                return "SLR";
            
            if (self.isPointAndShoot())
                return "Point & Shoot";
            
            throw "unrecognized tech product type";
        });

        self.specSummary = ko.computed(function() {
            if(self.isComputer())
                return self.computerBasicSpecs();
            
            if (self.isDigitalCamera())
                return self.digitalCameraBasicSpecs();

            throw "unrecognized tech product type";
        });
        //#endregion


        self.productType = ko.computed({
            read: function () {
                return typeHelper.getTypeName(self);
            },
            write: function (typeName) {
                typeHelper.createAndAssignType(typeName, self);
            }
        });
    };
});