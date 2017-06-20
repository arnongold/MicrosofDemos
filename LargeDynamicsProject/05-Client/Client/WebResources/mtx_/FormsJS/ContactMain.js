

var ContactMainBL = (function () {

    var serverUrl = Xrm.Page.context.getClientUrl();

    var crmPageJsPath = serverUrl + "/WebResources/mtx_/CommonJS/MatrixUtils.CrmPage.js"

    var onLoad = function () {
        //alert("test");


        var formType = Xrm.Page.ui.getFormType();



        require([crmPageJsPath],
            function () {

                alert("FormType = " + MatrixUtils.CrmPage.FormType.Create);

            });
    };

    return {
        onLoad: onLoad
    }

})();


function executeGenericLoader() {
    ContactMainBL.onLoad();
}