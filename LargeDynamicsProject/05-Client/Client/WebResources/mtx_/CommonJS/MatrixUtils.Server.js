if (typeof (MatrixUtils) == "undefined")
    MatrixUtils = {};

MatrixUtils.Server = (function () {

    var serverUrl = Xrm.Page.context.getClientUrl();
    var webApiPath = Xrm.Page.context.getClientUrl() + "/api/data/v8.1/";

    var CrmDataTypes = {
        Int: "int",
        String: "string",
        EntityRefernce: "EntityReference",
        Bool: "bool",
        DateTime: "DateTime",
        OptionSet: "OptionSetValue",
        Money: "Money"
    };

    var callAction = function (actionName, targetEntityName, targetId, dataArray, isAsync, successCallBack, errorCallback) {

        var Types = CrmDataTypes;
        var data = null;

        if (dataArray) {
            data = new Object();

            for (var i = 0; i < dataArray.length; i++) {
                var item = dataArray[i];
                switch (item.type) {
                    case Types.Int: {
                        data[item.key] = item.value.toString();
                        break;
                    }
                    case Types.String: {
                        data[item.key] = item.value;
                        break;
                    }
                    case Types.EntityRefernce: {
                        var guid = item.value.entityActivity == true ? "activityid" : item.value.entityType + "id";
                        var refObj = {};
                        refObj[guid] = item.value.id;
                        refObj["@odata.type"] = "Microsoft.Dynamics.CRM." + item.value.entityType;
                        refObj[item.value.parameterName.key] = item.value.parameterName.value;

                        data[item.key] = refObj;
                        break;
                    }
                    case Types.Bool: {
                        data[item.key] = item.value;
                        break;
                    }
                    case Types.DateTime: {
                        data[item.key] = item.value;
                        break;
                    }
                    case Types.OptionSet: {
                        data[item.key] = item.value.toString();
                        break;
                    }
                    case Types.Money: {

                        break;
                    }
                    default:

                }
            }
        }


        var reqEntityName = "";
        if (!MatrixUtils.JsExtantions.String.IsNullOrEmpty(targetEntityName) && !MatrixUtils.JsExtantions.String.IsNullOrEmpty(targetId)) {
            var reqEntityName = getEntityPluralName(targetEntityName) + "(" + MatrixUtils.JsExtantions.String.RemoveBraces(targetId) + ")/";
        }

        var reqActionName = "Microsoft.Dynamics.CRM." + actionName;
        reqUrl = webApiPath.toLowerCase() + reqEntityName.toLowerCase() + reqActionName;

        return executeHttpRequest("POST", reqUrl, isAsync, successCallBack, errorCallback, data);

    };

    var retrieve = function (entityName, recordId, isAsync, successCallBack, errorCallback) {

        if (!recordId) return null;

        recordId = MatrixUtils.JsExtantions.String.RemoveBraces(recordId);

        return generalRetrieve(entityName, recordId, null, null, null, null, isAsync, successCallBack, errorCallback);
    };

    var retrieveMultiple = function (entityName, select, filter, orderby, expand, isAsync, successCallBack, errorCallback) {

        return generalRetrieve(entityName, null, select, filter, orderby, expand, isAsync, successCallBack, errorCallback);
    };

    var generalRetrieve = function (entityName, recordId, select, filter, orderby, expand, isAsync, successCallBack, errorCallback) {

        var entityNameString = getEntityPluralName(entityName);
        entityNameString += !MatrixUtils.JsExtantions.String.IsNullOrEmpty(recordId) ? "(" + recordId + ")" : "";

        var query = "";

        query += !MatrixUtils.JsExtantions.String.IsNullOrEmpty(select) ? "$select=" + select : "";

        query += !MatrixUtils.JsExtantions.String.IsNullOrEmpty(expand)
            && !MatrixUtils.JsExtantions.String.IsNullOrEmpty(query)
            ? "&$expand=" + expand
            : !MatrixUtils.JsExtantions.String.IsNullOrEmpty(expand)
            ? "$expand=" + expand
            : "";

        query += !MatrixUtils.JsExtantions.String.IsNullOrEmpty(filter)
            && !MatrixUtils.JsExtantions.String.IsNullOrEmpty(query)
            ? "&$filter=" + filter
            : !MatrixUtils.JsExtantions.String.IsNullOrEmpty(filter)
            ? "$filter=" + filter
            : "";

        query += !MatrixUtils.JsExtantions.String.IsNullOrEmpty(orderby)
            && !MatrixUtils.JsExtantions.String.IsNullOrEmpty(query)
            ? "&$orderby=" + orderby
            : !MatrixUtils.JsExtantions.String.IsNullOrEmpty(orderby)
            ? "$orderby=" + orderby
            : "";

        query = !MatrixUtils.JsExtantions.String.IsNullOrEmpty(query)
            ? "?" + query
            : "";



        var reqUrl = webApiPath.toLowerCase() + entityNameString.toLowerCase() + query;

        return executeHttpRequest("GET", reqUrl, isAsync, successCallBack, errorCallback);
    };

    var fetch = function (entityName, fetchXmlStr) {

        var reqUrl = webApiPath + getEntityPluralName(entityName) + "?fetchXml=" + fetchXmlStr;

        return executeHttpRequest("GET", reqUrl);
    };

    var update = function (entityName, recordId, dataObj) {

        var reqUrl = webApiPath + getEntityPluralName(entityName) + "(" + MatrixUtils.JsExtantions.String.RemoveBraces(recordId) + ")";

        executeHttpRequest("PATCH", reqUrl, true, null, null, dataObj);
    };

    var executeHttpRequest = function (message, reqUrl, isAsync, successCallBack, errorCallback, dataObj) {

        var receivedData = null;
        var request = new XMLHttpRequest();
        var isAsync = isAsync ? isAsync : false;
        request.open(message, encodeURI(reqUrl), isAsync);
        request.setRequestHeader("Accept", "application/json");
        request.setRequestHeader("Content-Type", "application/json;charset=utf-8");
        request.setRequestHeader("OData-MaxVersion", "4.0");
        request.setRequestHeader("OData-Version", "4.0");
        request.setRequestHeader("Prefer", "odata.include-annotations=\"*\"");
        request.onreadystatechange = function () {
            if (request.readyState == 4) {
                if (request.status == 200) {

                    var responce = JSON.parse(request.responseText);
                    receivedData = responce.value ? responce.value : responce;
                    receivedData = parseResponceData(receivedData);
                    console.log("success\n" + request.responseText);

                    if (successCallBack) successCallBack(receivedData);
                }
                else if (request.status == 204) {

                    receivedData = true;
                    if (successCallBack) successCallBack(receivedData);
                }
                else {
                    receivedData = JSON.parse(request.responseText).error
                    console.log("error\n" + receivedData.message);

                    if (errorCallback) errorCallback(receivedData);
                }
            }
        }

        if ((message == "POST" || message == "PATCH") && dataObj) {

            request.send(JSON.stringify(dataObj));
        }
        else {
            request.send();
        }

        return receivedData;
    };

    var getEntityPluralName = function (entityName) {

        if (entityName.endsWith('s') || entityName.endsWith('sh') || entityName.endsWith('ch') || entityName.endsWith('x') || entityName.endsWith('z')) {

            return entityName + 'es';
        }

        if (entityName.endsWith('y')) {

            return entityName.substr(0, entityName.length - 1) + 'ies';
        }

        return entityName + 's';
    };

    var parseResponceData = function (responce) {

        var ret;

        if (responce.length) {
            ret = new Array();

            for (var i = 0; i < responce.length; i++) {
                ret.push(parseWebApiJsonObject(responce[i]));
            }
        }
        else {
            ret = parseWebApiJsonObject(responce);
        }

        return ret;

    };

    var parseWebApiJsonObject = function (dataObj) {

        var ret = new Object()
        var props = Object.keys(dataObj);

        for (var i = 0; i < props.length; i++) {

            var index = props[i].indexOf('@');

            if (index != -1) {

                var propNamePrefix = props[i].substring(0, index);
                var propNameSuffix = (props[i].substring(index, props[i].length));
                var suffixesArr = propNameSuffix.split('.');
                var fieldName = propNamePrefix.startsWith('_') && propNamePrefix.endsWith('value') ? propNamePrefix.replace('_', '').replace('_value', '') : propNamePrefix + "_" + suffixesArr[suffixesArr.length - 1];

                switch (propNameSuffix) {

                    case "@OData.Community.Display.V1.FormattedValue": {

                        ret[fieldName] = ret[fieldName] ? ret[fieldName] : {};
                        ret[fieldName].Name = dataObj[props[i]];

                        break;
                    }
                    case "@Microsoft.Dynamics.CRM.lookuplogicalname": {
                        ret[fieldName] = ret[fieldName] ? ret[fieldName] : {};
                        ret[fieldName].LogicalName = dataObj[props[i]];

                        break;
                    }
                    default: {
                        var propName = propNamePrefix + "_" + suffixesArr[suffixesArr.length - 1];
                        ret[propName] = dataObj[props[i]];
                        break;
                    }
                }
            }
            else if (props[i].startsWith('_')) {
                var objName = props[i].replace('_', '').replace('_value', '')
                ret[objName] = ret[objName] ? ret[objName] : {};
                ret[objName].Id = dataObj[props[i]];
            }
            else {

                if (typeof (dataObj[props[i]]) == "object" && dataObj[props[i]] && Object.keys(dataObj[props[i]]).length >= 1) { //related entity
                    ret[props[i]] = ret[props[i]] ? ret[props[i]] : {};
                    ret[props[i]].Expand = parseWebApiJsonObject(dataObj[props[i]]);
                }
                else {
                    ret[props[i]] = dataObj[props[i]];
                }
            }
        }

        return ret;
    };


    return {
        CrmDataTypes: CrmDataTypes,
        CallAction: callAction,
        Retrieve: retrieve,
        RetrieveMultiple: retrieveMultiple,
        Fetch: fetch,
        Update: update
    };

})(window.MatrixUtils.Server = window.MatrixUtils.Server || {});
