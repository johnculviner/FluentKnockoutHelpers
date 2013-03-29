define(['utility/typeParser'],
function (typeParser) {
    return function (apiTechProduct) {
        var self = this;

        //here NO custom mappings being performed here but we want a
        //javascript representation of C# TechProduct to do TypeParsing
        ko.mapping.fromJS(apiTechProduct, {}, self);


        //#region Computer parsing
        self.isDesktop = ko.computed(function () {
            return typeParser.isType(self, 'Desktop');
        });
        
        self.isLaptop = ko.computed(function () {
            return typeParser.isType(self, 'Laptop');
        });

        self.isComputer = ko.computed(function() {
            return self.isDesktop() || self.isLaptop();
        });
        
        self.computerBasicSpecs = ko.computed(function () {
            return self.Mhz() + " Mhz Processor w/ " + self.GigsOfRam() + " GB RAM and " + (self.HasSsd() ? "an SSD" : "no SSD (bummer)");
        }, this, { deferEvaluation: true });
        //#endregion


        //#region Digital camera parsing
        self.isPointAndShoot = ko.computed(function () {
            return typeParser.isType(self, 'PointAndShoot');
        });

        self.isSlr = ko.computed(function () {
            return typeParser.isType(self, 'Slr');
        });

        self.isDigitalCamera = ko.computed(function () {
            return self.isPointAndShoot() || self.isSlr();
        });

        self.digitalCameraBasicSpecs = ko.computed(function() {
            return self.MegaPixels() + " Megapixels";
        }, this, { deferEvaluation: true });
        //#endregion


        self.productType = ko.computed(function() {
            return typeParser.typeName(self);
        });

        self.basicSpecs = ko.computed(function() {
            
            if(self.isComputer())
                return self.computerBasicSpecs();
            
            if (self.isDigitalCamera())
                return self.digitalCameraBasicSpecs();

            throw "unrecognized tech product type";
        });
    };
});