/// <reference path="../CommonJS/Moi.Enums.js" />

var RecruitedFunctionMainBL = (function () {

    var DUPLICATE_ROLE_NOTIFICATION_MESSAGE = "{0} בעל ת.ז {1} כבר משויך לתפקיד זה. לא ניתן לשייך שני מגויסים בעלי תפקיד זהה באותה רשות משתתפת.";
    var DUPLICATE_ROLE_NOTIFICATION_MESSAGE_BALLOTBOX = "{0} בעל ת.ז {1} כבר משויך לתפקיד זה. לא ניתן לשייך שני מגויסים בעלי תפקיד זהה באותה קלפי";
    var DUPLICATE_ROLE_NOTIFICATION_MESSAGE_UNIQUE_KEY = "dr01";

    var onLoad = function () {
        initOnChange();
    };

    var initOnChange = function () {
        //xrm add onchange
        Xrm.Page.getAttribute("mtx_participantmunicipalcouncilid").addOnChange(participantMnicipalCouncilIdOnChange);
        Xrm.Page.getAttribute("mtx_electionfunctiontypecode").addOnChange(electionFunctionTypeCodeOnChange);
        Xrm.Page.getAttribute("mtx_ballotboxid").addOnChange(ballotBoxIdOnChange);
    }

    var ballotBoxIdOnChange = function () {
        duplicateRoleValidation()
    };

    var participantMnicipalCouncilIdOnChange = function () {
        duplicateRoleValidation()
    };

    var electionFunctionTypeCodeOnChange = function () {
        duplicateRoleValidation()
    }

    var duplicateRoleValidation = function () {

        var participantMunicipalCouncilIdControl = Xrm.Page.getControl("mtx_participantmunicipalcouncilid");
        var participantMunicipalCouncilIdValue = participantMunicipalCouncilIdControl.getAttribute().getValue();
        var electionFunctionTypeCodeControl = Xrm.Page.getControl("mtx_electionfunctiontypecode");
        var electionFunctionTypeCodeValue = electionFunctionTypeCodeControl.getAttribute().getValue();
        var ballotBoxIdControl = Xrm.Page.getControl("mtx_ballotboxid");
        var ballotBoxIdValue = ballotBoxIdControl.getAttribute().getValue();
        var notificationMessage = "";
        var res = "";
        var filter = "";
        // clear notifications
        Xrm.Page.ui.clearFormNotification(DUPLICATE_ROLE_NOTIFICATION_MESSAGE_UNIQUE_KEY);
        electionFunctionTypeCodeControl.clearNotification(DUPLICATE_ROLE_NOTIFICATION_MESSAGE_UNIQUE_KEY);

        if (electionFunctionTypeCodeValue != null) {

            var id = MatrixUtils.JsExtantions.String.RemoveBraces(Xrm.Page.data.entity.getId());
            if (participantMunicipalCouncilIdValue != null && 
                    (electionFunctionTypeCodeValue == ElectionFunctionTypeCode.ElectionManager ||
                    electionFunctionTypeCodeValue == ElectionFunctionTypeCode.ElectionAssistant)) 
            {
                participantMunicipalCouncilIdValue = MatrixUtils.JsExtantions.String.RemoveBraces(participantMunicipalCouncilIdValue[0].id);
                filter = "_mtx_participantmunicipalcouncilid_value eq " + participantMunicipalCouncilIdValue + " and mtx_electionfunctiontypecode eq " + electionFunctionTypeCodeValue;
                filter += MatrixUtils.JsExtantions.String.IsNullOrEmpty(id) ? "" : " and mtx_recruitedfunctionid ne " + id;

            } else if (ballotBoxIdValue != null && (electionFunctionTypeCodeValue == ElectionFunctionTypeCode.BallotBoxManager ||
                electionFunctionTypeCodeValue == ElectionFunctionTypeCode.BallotBoxAssistant)) {
                ballotBoxIdValue = MatrixUtils.JsExtantions.String.RemoveBraces(ballotBoxIdValue[0].id);

                filter = "_mtx_ballotboxid_value eq " + ballotBoxIdValue + " and mtx_electionfunctiontypecode eq " + electionFunctionTypeCodeValue;
                filter += MatrixUtils.JsExtantions.String.IsNullOrEmpty(id) ? "" : " and mtx_recruitedfunctionid ne " + id;
            }

            if(filter != ""){ 

                res = MatrixUtils.Server.RetrieveMultiple("mtx_recruitedfunction",
                   null,
                   filter,
                   null,
                   "mtx_ContactId($select=fullname,governmentid)");

                if (res[0] && res.length > 0) {
                    // Set Notifications
                    var name = res[0].mtx_ContactId.Expand.fullname;
                    var govenmentId = res[0].mtx_ContactId.Expand.governmentid;

                    if (electionFunctionTypeCodeValue == 1 ||
                    electionFunctionTypeCodeValue == 2) {
                        notificationMessage = MatrixUtils.JsExtantions.String.Format(DUPLICATE_ROLE_NOTIFICATION_MESSAGE, name, govenmentId);
                    } else {
                        notificationMessage = MatrixUtils.JsExtantions.String.Format(DUPLICATE_ROLE_NOTIFICATION_MESSAGE_BALLOTBOX, name, govenmentId);
                    }

                    Xrm.Page.ui.setFormNotification(notificationMessage, "EROR", DUPLICATE_ROLE_NOTIFICATION_MESSAGE_UNIQUE_KEY);
                    electionFunctionTypeCodeControl.setNotification(notificationMessage, DUPLICATE_ROLE_NOTIFICATION_MESSAGE_UNIQUE_KEY);
                }
            }
        }
    }

    return {
        onLoad: onLoad
    }
})();

function executeGenericLoader() {
    RecruitedFunctionMainBL.onLoad();
}