$.validator.addMethod("decimalchk",
    function (value, element, params) {
        if (value == false) {
            return true;
        }

        var intnum = params[0];
        var decnum = params[1];
        var rangeFrom = params[2];
        var rangeTo = params[3];
        value = value.replace(/\,/g, "");

        if (rangeFrom != "" && rangeTo != "") {
            if (parseFloat(value) > parseFloat(rangeTo) || parseFloat(value) < parseFloat(rangeFrom)) {
                return false;
            }
        }

        if (value.match(/[^0-9.-]/)) return false;

        if (intnum != "0" && decnum != "0") {
            var rege1 = "/^[-]?[0-9]\\d{0,";
            var rege2 = "}(.\\d{1,";
            var rege3 = "})?%?$/";
            var regeMatch = rege1 + intnum + rege2 + decnum + rege3;

            //return value.match(/^[-]?[0-9]\d{0,3}(.\d{1,7})?%?$/);
            return value.match(eval(regeMatch));
        }

        return true;
    }
);
$.validator.unobtrusive.adapters.add("decimalchk", ["intnum", "decnum", "rangefrom", "rangeto"], function (options) {
    options.rules["decimalchk"] = [options.params["intnum"], options.params["decnum"], options.params["rangefrom"], options.params["rangeto"]];
    if (options.message) {
        options.messages["decimalchk"] = options.message;
    }
})