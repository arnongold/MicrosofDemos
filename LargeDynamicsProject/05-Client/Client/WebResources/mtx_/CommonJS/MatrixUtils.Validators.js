if (typeof (MatrixUtils) == "undefined")
    MatrixUtils = {};

MatrixUtils.Validators = (function () {

    var isValidMobileNumber = function (phoneNumber) {

        var reg = /^0(5[0123456789]){1}(\-)?[^0\D]{1}\d{6}$/;
        if (phoneNumber != null && !reg.test(phoneNumber)) {
            return false;
        }
        return true;
    };

    var isValidPhoneNumber = function (phoneNumber) {
        var reg = /^0(5[0123456789]|7[12346789]|[23489]){1}(\-)?[^0\D]{1}\d{6}$/;
        if (phoneNumber != null && !reg.test(phoneNumber)) {
            return false;
        }
        return true;
    };

    var isValidEmailAddress = function (emailAddress) {
        var reg = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (emailAddress != null && !reg.test(emailAddress)) {
            return false;
        }
        return true;
    };

    var isValidGovId = function (govId) {

        if (!MatrixUtils.JsExtantions.String.IsNullOrEmpty(govId) || (govId.length > 9) || (govId.length < 2)) {
            return false;
        }

        if (govId.length < 9) {
            while (govId.length < 9) {
                govId = '0' + govId;
            }
        }

        var mone = 0;
        var incNum;

        for (var i = 0; i < 9; i++) {
            incNum = Number(govId.charAt(i));
            incNum *= (i % 2) + 1;
            if (incNum > 9)
                incNum -= 9;
            mone += incNum;
        }

        if (mone % 10 == 0) {
            return true;
        }
        else {
            return false;
        }
    };

    return {
        IsValidMobileNumber: isValidMobileNumber,
        IsValidPhoneNumber: isValidPhoneNumber,
        IsValidEmailAddress: isValidEmailAddress,
        IsValidGovId: isValidGovId,
    };

})(window.MatrixUtils.Validators = window.MatrixUtils.Validators || {});