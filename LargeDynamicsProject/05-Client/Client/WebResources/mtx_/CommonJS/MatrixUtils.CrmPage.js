if (typeof (MatrixUtils) == "undefined")
    MatrixUtils = {};

MatrixUtils.CrmPage = (function () {

    var formType = {
        Create: 1,
        Update: 2,
        ReadOnly: 3,
        Disable: 4,
        QucikCreate: 5,
        BulkEdit: 6,
        ReadOptimized: 11
    };

    var serverUrl = Xrm.Page.context.getClientUrl();

    var validatorJsPath = serverUrl + "/WebResources/mtx_/CommonJS/MatrixUtils.Validators.js"

    var handleTelephoneAttribute = function (attributeName) {

        require([validatorJsPath],
               function () {
                   var PHONE_CONTROL_NOTIFICATION_MESSAGE = "עליך להזין מספר טלפון חוקי.";
                   var PHONE_CONTROL_NOTIFICATION_MESSAGE_UNIQUE_KEY = "3543543413645";
                   var phoneControl = Xrm.Page.getControl(attributeName);
                   var phoneValue = phoneControl.getAttribute().getValue();


                   if (phoneValue != null && !MatrixUtils.Validators.IsValidPhoneNumber(phoneValue)) {

                       phoneControl.setNotification(PHONE_CONTROL_NOTIFICATION_MESSAGE, PHONE_CONTROL_NOTIFICATION_MESSAGE_UNIQUE_KEY);
                   }
                   else {
                       if (phoneValue != null && phoneValue.indexOf("-") != -1) {
                           phoneControl.getAttribute().setValue(phoneValue.replace("-", ""));
                       }
                       phoneControl.clearNotification(PHONE_CONTROL_NOTIFICATION_MESSAGE_UNIQUE_KEY)
                   }
               });
    };



    var handleMobilePhoneAttribute = function (attributeName) {

        require([validatorJsPath],
              function () {
                  var MOBILEPHONE_CONTROL_NOTIFICATION_MESSAGE = "עליך להזין מספר טלפון נייד חוקי.";
                  var MOBILEPHONE_CONTROL_NOTIFICATION_MESSAGE_UNIQUE_KEY = "93959964999";

                  var phoneControl = Xrm.Page.getControl(attributeName);
                  var phoneValue = phoneControl.getAttribute().getValue();

                  if (phoneValue != null && !MatrixUtils.Validators.IsValidMobileNumber(phoneValue)) {

                      phoneControl.setNotification(MOBILEPHONE_CONTROL_NOTIFICATION_MESSAGE, MOBILEPHONE_CONTROL_NOTIFICATION_MESSAGE_UNIQUE_KEY);
                  }
                  else {
                      if (phoneValue != null && phoneValue.indexOf("-") != -1) {
                          phoneControl.getAttribute().setValue(phoneValue.replace("-", ""));
                      }
                      phoneControl.clearNotification(MOBILEPHONE_CONTROL_NOTIFICATION_MESSAGE_UNIQUE_KEY)
                  }
              });
    };

    var handleEmailAddressAttribute = function (attributeName) {

        require([validatorJsPath],
             function () {
                 var EMAIL_ADDRESS_CONTROL_NOTIFICATION_MESSAGE = "עליך להזין כתובת דואר אלקטרוני חוקית.";
                 var EMAIL_ADDRESS_CONTROL_NOTIFICATION_MESSAGE_UNIQUE_KEY = "565468";
                 var emailAdressControl = Xrm.Page.getControl(attributeName);
                 var emailAdressValue = emailAdressControl.getAttribute().getValue();

                 if (emailAdressValue != null && !MatrixUtils.Validators.isValidEmailAddress(emailAdressValue)) {
                     emailAdressControl.setNotification(EMAIL_ADDRESS_CONTROL_NOTIFICATION_MESSAGE, EMAIL_ADDRESS_CONTROL_NOTIFICATION_MESSAGE_UNIQUE_KEY);
                 }
                 else {
                     emailAdressControl.clearNotification(EMAIL_ADDRESS_CONTROL_NOTIFICATION_MESSAGE_UNIQUE_KEY)
                 }
             });

    };

    var handleGovIdAttribute = function (AttributeName) {

        require([validatorJsPath],
        function () {
            var GOVID_CONTROL_NOTIFICATION_MESSAGE = 'עליך להזין מספר תעודת זהות חוקי.';
            var GOVID_CONTROL_NOTIFICATION_MESSAGE_UNIQUE_KEY = "755449";
            var govIdControl = Xrm.Page.getControl(attributeName);
            var govIdValue = govIdControl.getAttribute().getValue();

            if (govIdValue != null && MatrixUtils.Validators.isValidGovId(govIdValue)) {

                govIdControl.setNotification(GOVID_CONTROL_NOTIFICATION_MESSAGE, GOVID_CONTROL_NOTIFICATION_MESSAGE_UNIQUE_KEY);
            }
            else {
                govIdControl.clearNotification(GOVID_CONTROL_NOTIFICATION_MESSAGE_UNIQUE_KEY);
            }
        });

    };

    var handleTelephoneAttributeChange = function (context) {
        handleTelephoneAttribute(context.getEventSource().getName());
    };

    var handleMobilePhoneAttributeChange = function (context) {
        handleMobilePhoneAttribute(context.getEventSource().getName());
    };

    var handleEmailAddressAttributeChange = function (context) {
        handleEmailAddressAttribute(context.getEventSource().getName());
    }

    var handleDateTimeAttributeForFutureDateChange = function (context) {
        var DATETIME_CONTROL_NOTIFICATION_MESSAGE = "";
        var DATETIME_CONTROL_NOTIFICATION_MESSAGE_UNIQUE_KEY = "75545645436489";
        var dateTimeControl = Xrm.Page.getControl(context.getEventSource().getName());
        var dateTimeAttribute = dateTimeControl.getAttribute();
        if (dateTimeAttribute.getValue() != null && new Date(dateTimeAttribute.getValue()) < new Date) {
            dateTimeControl.setNotification(DATETIME_CONTROL_NOTIFICATION_MESSAGE, DATETIME_CONTROL_NOTIFICATION_MESSAGE_UNIQUE_KEY);
        }
        else {
            dateTimeControl.clearNotification(DATETIME_CONTROL_NOTIFICATION_MESSAGE_UNIQUE_KEY);
        }
    };

    var handleGovIdAttributeChange = function (context) {
        handleGovIdAttribute(context.getEventSource().getName());
    };

    var setLookup = function (lookupName, id, text, logicalName) {

        if (lookupName != null && lookupName != "undefined"
            && id != null && id != "undefined"
            && text != null && text != "undefined"
            && logicalName != null && logicalName != "undefined") {

            if (id.indexOf('{') == -1)
                id = '{' + id;
            if (id.indexOf('}') == -1)
                id = id + '}';
            id = id.toUpperCase();

            var value = new Array();
            value[0] = new Object();
            value[0].id = id;
            value[0].name = text;
            value[0].entityType = logicalName;
            Xrm.Page.getAttribute(lookupName).setValue(value);
        }
    };

    var getAllDirtyFields = function () {
        var attr = Xrm.Page.data.entity.attributes.get();
        var listOfDirty = new Array();
        if (attr != null) {
            for (var i in attr) {
                if (attr[i].getIsDirty()) {
                    listOfDirty.push(attr[i].getName());
                }
            }
        }
        return listOfDirty;
    };

    return {
        FormType: formType,
        HandleTelephoneAttribute: handleTelephoneAttribute,
        HandleMobilePhoneAttribute: handleMobilePhoneAttribute,
        HandleEmailAddressAttribute: handleEmailAddressAttribute,
        HandleGovIdAttribute: handleGovIdAttribute,
        HandleTelephoneAttributeChange: handleTelephoneAttributeChange,
        HandleMobilePhoneAttributeChange: handleMobilePhoneAttributeChange,
        HandleEmailAddressAttributeChange: handleEmailAddressAttributeChange,
        HandleDateTimeAttributeForFutureDateChange: handleDateTimeAttributeForFutureDateChange,
        SetLookup: setLookup,
        GetAllDirtyFields: getAllDirtyFields,
    };

})(window.MatrixUtils.CrmPage = window.MatrixUtils.CrmPage || {});