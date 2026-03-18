$.validator.addMethod("istime",
    function (value, element, params) {
        if (value == "") {
            return true;
        }
        value = value.replace(":", "");
        if (!this.optional(element)) {
            if (params == "hh24mm") {
                if (value.length != 4) {
                    return false;
                }
            }
            if (params == "hh24mmss") {
                if (value.length != 6) {
                    return false;
                }
            }
        }

        return value.match(/^([01][0-9]|2[0-3])([0-5][0-9]){1,2}$/);
    }
);
$.validator.unobtrusive.adapters.addSingleVal("istime", "otherproperty");