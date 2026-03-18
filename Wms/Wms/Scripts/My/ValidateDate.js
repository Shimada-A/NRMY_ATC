var validateDate = (function () {
    return {
        checkDate: function (date1,time1, date2,time2,idHtml, idDbField, message) {
            if (date1 != "" && date2 != "" && time1 != "" && time2 != "" && date1 + time1 > date2 + time2 ) {
                validateDate.setValidationErrorMessage(date1, idHtml, idDbField, message)
                $("#" + idDbField).focus();
                return false;
            } else {
                validateDate.setValidationErrorMessage('T', idHtml, idDbField, message)
                return true;
            }
        },
        checkDateMust: function (date1, date2, idHtml1, idDbField1, idHtml2, idDbField2, message) {
            if (date1 == "" ) {
                validateDate.setErrorMessage(date1, idHtml1, idDbField1, message)
                $("#" + idDbField1).focus();
                return false;
            } else if(date2 == "") {
                validateDate.setErrorMessage(date1, idHtml2, idDbField2, message)
                $("#" + idDbField2).focus();
                return false;
            } else {
                validateDate.setErrorMessage('T', idHtml1, idDbField1, message)
                return true;
            }
        },
        checkDateRange: function (date1, date2, idHtml, idDbField, message) {
            if ((Math.abs((date1 - date2)) / (1000 * 60 * 60 * 24) + 1)>7) {
                validateDate.setErrorMessage(date1, idHtml, idDbField, message)
                $("#" + idDbField).focus();
                return false;
            } else {
                validateDate.setErrorMessage('T', idHtml, idDbField, message)
                return true;
            }
        },
        setValidationErrorMessage: function (clsMsgFlg, idHtml, idDbField, message) {
            var htmlSpan = null;

            if (clsMsgFlg == 'T') {
                //htmlSpan.addClass('field-validation-valid')
                //htmlSpan.html("")
            } else {
                $("span.field\\-validation\\-valid").each(function () {
                    if ($(this).attr('data-valmsg-for') == idDbField) {
                        htmlSpan = $(this);
                    }
                });
                if (!htmlSpan) {
                    //alert(htmlSpan.html())
                    $("span.field\\-validation\\-error").each(function () {
                        if ($(this).attr('data-valmsg-for') == idDbField) {
                            htmlSpan = $(this);
                        }
                    });
                }

                htmlSpan.removeClass('field-validation-error')
                htmlSpan.removeClass('field-validation-valid')
                $("#" + idHtml).removeClass("input-validation-error");

                htmlSpan.addClass('field-validation-error')
                $("#" + idHtml).addClass("input-validation-error");
                htmlSpan.html('<span id=' + idHtml + '"-error" class="">' + message + '</span>')
            }
        },
        setErrorMessage: function (clsMsgFlg, idHtml, idDbField, message) {
            var htmlSpan = null;

            if (clsMsgFlg == 'T') {
               // htmlSpan.addClass('field-validation-valid')
               // htmlSpan.html("")
            } else {
                $("span.field\\-validation\\-valid").each(function () {
                    if ($(this).attr('data-valmsg-for') == idDbField) {
                        htmlSpan = $(this);
                    }
                });
                if (!htmlSpan) {
                    //alert(htmlSpan.html())
                    $("span.field\\-validation\\-error").each(function () {
                        if ($(this).attr('data-valmsg-for') == idDbField) {
                            htmlSpan = $(this);
                        }
                    });
                }

                htmlSpan.removeClass('field-validation-error')
                htmlSpan.removeClass('field-validation-valid')
                $("#" + idHtml).removeClass("input-validation-error");

                htmlSpan.addClass('field-validation-error')
                $("#" + idHtml).addClass("input-validation-error");
                htmlSpan.html('<span id=' + idHtml + '"-error" class="">' + message + '</span>')
            }
        }
    }
})();