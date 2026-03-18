$.validator.addMethod('stringrequired',
    function (value, element, param) {

        if (value === '')
            return false;
        return true;
    });

$.validator.unobtrusive.adapters.addBool('stringrequired');