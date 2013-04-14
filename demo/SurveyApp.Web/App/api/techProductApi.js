define(['durandal/http'],
function (http) {

    var root = 'api/TechProduct';

    return {
        ValidateSerialNumberUnique: function (serialNumber, techProductId) {
            return http.post(root + "/ValidateSerialNumberUnique", { SerialNumber: serialNumber, TechProductId: techProductId });
        }
    };
})