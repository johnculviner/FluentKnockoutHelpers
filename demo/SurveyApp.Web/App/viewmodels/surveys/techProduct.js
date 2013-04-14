define(['utility/typeMetadataHelper', 'api/techProductApi'],
function (typeMetadataHelper, api) {
    return function (apiTechProduct /*undefined on add since we don't know what type the user will pick*/) {
        var self = this;

        if (!apiTechProduct)
            self.$type = ko.observable(); //type will be set once it is choosen
        else {
            
            //here NO custom mappings being performed here but we want a
            //javascript representation (on 'this') of a C# TechProduct
            ko.mapping.fromJS(apiTechProduct, {}, self);
            wireAddlValidationRules();
        }


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
        
        var computerBasicSpecs = ko.computed(function () {
            self.$type(); //tell knockout to re-evaluate when $type changes
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

        var digitalCameraBasicSpecs = ko.computed(function () {
            self.$type(); //tell knockout to re-evaluate when $type changes
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
                return computerBasicSpecs();
            
            if (self.isDigitalCamera())
                return digitalCameraBasicSpecs();
        });
        //#endregion

        //this is more complicated than normal because we have to support NO techProduct being defined at start because the user picks it
        //the self.SerialNumber *doesn't* exist until after a type is picked by the user and created on the instance below
        function wireAddlValidationRules() {
            self.SerialNumber.extend(
                {
                    asyncValidation: function () {
                            //peek should be used below to prevent knockout from creating a computed dependency chain on the below parameters
                            //ex: without the peek knockout's dependency tracking would think that any time 'SerialNumber' or 'Id' changed
                            //validation needs to run. It is the case here for SerialNumber but not Id.
                            return api.ValidateSerialNumberUnique(self.SerialNumber.peek(), self.TechProductId.peek());   
                        }
                });
        }

        //#region Events
        self.productType = ko.computed({
            read: function () {
                //what type of product is this?
                return typeMetadataHelper.getTypeName(self);
            },
            write: function (typeName) {
                //UI requesting to change the type. find the type in typeMetadata, assign it to "this" and wire up validation internally
                typeMetadataHelper.getInstanceAndAssign(typeName, self, { validation: wireAddlValidationRules });
            }
        });
        //#endregion
        
        self.isProductTypeSet = ko.computed(function () {
            return self.productType() !== null;
        });
    };
});