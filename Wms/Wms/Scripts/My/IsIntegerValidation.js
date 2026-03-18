$.validator.addMethod("isinteger",
    function (value, element, param) {
        if (value == false) {
            return true;
        }
        return value.match(/^\d+$/);
    }
);
$.validator.unobtrusive.adapters.addBool("isinteger");