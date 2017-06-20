if (typeof (MatrixUtils) == "undefined")
    MatrixUtils = {};

MatrixUtils.JsExtantions = (function () {

    var string = function () {

        var isNullOrEmpty = function (string) {

            return !string || string == "" || string.trim().length == 0;
        };

        var removeBraces = function (string) {

            if (string && string.indexOf('{') != -1 && string.indexOf('}') != -1) {
                string = string.replace(/{|}/g, '');
            }

            return string;
        }

        var endsWith = function (string) {
            return this.substr(this.length - string.length) === string;
        }

        var startsWith = function (string) {
            return this.substr(0, string.length) === string;
        };

        var format = function (string) {
            var formatted = string;

            for (var i = 0 ; i < arguments.length; i++) {
                var regexp = new RegExp('\\{' + i + '\\}', 'gi');
                formatted = formatted.replace(regexp, arguments[i + 1]);
            }
            return formatted;
        };


        return {
            IsNullOrEmpty: isNullOrEmpty,
            RemoveBraces: removeBraces,
            EndsWith: endsWith,
            StartsWith: startsWith,
            Format: format
        };
    }();

    if (!String.prototype.endsWith) {
        String.prototype.endsWith = string.EndsWith;
    }

    if (!String.prototype.startsWith) {
        String.prototype.startsWith = string.StartsWith;
    }

    return {
        String: string
    };

})(window.MatrixUtils.JsExtantions = window.MatrixUtils.JsExtantions || {});








