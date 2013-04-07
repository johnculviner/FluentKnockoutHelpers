define(['utility/typeMetadataHelper'],
function (typeMetadataHelper) {
    return function (apiTechProduct /*undefined on add*/) {
        var self = this;

        //if not specifed we are doing an add, default to 'laptop'
        apiTechProduct = apiTechProduct || typeMetadataHelper.getTemplateInstance('laptop');

        //here NO custom mappings being performed here but we want a
        //javascript representation (on 'this') of a C# TechProduct
        ko.mapping.fromJS(apiTechProduct, {}, self);


        //#region Computer
        self.isDesktop = ko.computed(function () {
            return typeMetadataHelper.isType(self, 'Desktop');
        });
        
        self.isLaptop = ko.computed(function () {
            return typeMetadataHelper.isType(self, 'Laptop');
        });

        self.isComputer = ko.computed(function() {
            return self.isDesktop() || self.isLaptop();
        });
        
        self.computerBasicSpecs = ko.computed(function () {
            self.$type(); //force knockout to re-evaluate when $type changes
            if (self.Mhz && self.GigsOfRam && self.HasSsd)
                return self.Mhz() + " Mhz Processor w/ " + self.GigsOfRam() + " GB RAM and " + (self.HasSsd() ? "an SSD" : "no SSD (bummer)");
        });
        //#endregion

        //#region Digital camera
        self.isPointAndShoot = ko.computed(function () {
            return typeMetadataHelper.isType(self, 'PointAndShoot');
        });

        self.isSlr = ko.computed(function () {
            return typeMetadataHelper.isType(self, 'Slr');
        });

        self.isDigitalCamera = ko.computed(function () {
            return self.isPointAndShoot() || self.isSlr();
        });

        self.digitalCameraBasicSpecs = ko.computed(function () {
            self.$type(); //force knockout to re-evaluate when $type changes
            if (self.MegaPixels)
                return self.MegaPixels() + ' Megapixels';
        });
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
        });

        self.specSummary = ko.computed(function() {
            if(self.isComputer())
                return self.computerBasicSpecs();
            
            if (self.isDigitalCamera())
                return self.digitalCameraBasicSpecs();
        });
        //#endregion


        self.productType = ko.computed({
            read: function () {
                return typeMetadataHelper.getTypeName(self);
            },
            write: function (typeName) {
                typeMetadataHelper.createAndAssignType(typeName, self);
            }
        });
    };
});