/* Single DatePicker */
var single_flatpickr_opts = {
    "locale": "ja",
    dateFormat: "Y/m/d",
    allowInput: true,
};
$(".flatpickr_single").flatpickr(single_flatpickr_opts);

/* Range DatePicker */
var ok_flag = false;
var close_flag = false;
var init_open_flag = false;
var obj;
var selectDate;

$('.startDate,.endDate').on("focus", function () {
    setTimeout(function () {
        if ($('.multiMonth').hasClass('open')) {
            $(".wrap").addClass("calender_open");
        }
    }, 200);
    ok_flag = false;
});

$('.startDate,.endDate').keydown(function (e) {
    if (e.which == 9) {
        init_open_flag = false;
    }
});

$(document).on("click", ".calender_open", function () {
    ok_flag = true;
    obj.close();
});

var formatDate = function (date, format) {
    if (!format) format = 'YYYY-MM-DD hh:mm:ss.SSS';
    format = format.replace(/YYYY/g, date.getFullYear());
    format = format.replace(/MM/g, ('0' + (date.getMonth() + 1)).slice(-2));
    format = format.replace(/DD/g, ('0' + date.getDate()).slice(-2));
    format = format.replace(/hh/g, ('0' + date.getHours()).slice(-2));
    format = format.replace(/mm/g, ('0' + date.getMinutes()).slice(-2));
    format = format.replace(/ss/g, ('0' + date.getSeconds()).slice(-2));
    if (format.match(/S/g)) {
        var milliSeconds = ('00' + date.getMilliseconds()).slice(-3);
        var length = format.match(/S/g).length;
        for (var i = 0; i < length; i++) format = format.replace(/S/, milliSeconds.substring(i, i + 1));
    }
    return format;
};

var flatpickrClosed = function (instance) { };
var prevStartVal;
var prevEndVal;

var flatpickr_opts = {
    "locale": "ja",
    mode: "range",
    dateFormat: "Y/m/d",
    showMonths: 2,
    saveButton: true,
    rangeSeparator: ":",
    onClose: function (selectedDates, dateStr, instance) {
        var $startDate = $(instance.element).parent().parent().find('.startDate');
        var $endDate = $(instance.element).parent().parent().find('.endDate');

        if (ok_flag == false) {
            instance.open();
        } else {
            close_flag = true;
            init_open_flag = false;
            var startDate = '';
            var endDate = '';
            if (selectedDates[0]) {
                startDate = formatDate(selectedDates[0], 'YYYY/MM/DD');
            } else {
                if ($startDate.val()) {
                    startDate = $startDate.val();
                } else if ($endDate.val()) {
                    startDate = $endDate.val();
                }
            }
            if (selectedDates[1]) {
                endDate = formatDate(selectedDates[1], 'YYYY/MM/DD');
            } else {
                if ($startDate.val()) {
                    endDate = $startDate.val();
                } else if ($endDate.val()) {
                    endDate = $endDate.val();
                }
            }
            instance.setDate([startDate, endDate], true);
            $startDate.val(startDate);
            $endDate.val(endDate);
            $(".wrap").removeClass("calender_open");
            ok_flag = false;
            close_flag = false;
            if (prevStartVal != startDate || prevEndVal != endDate) {
                flatpickrClosed(instance);
            }
        }
    },
    onChange: function (dateObj, dateStr, instance) {
        var $startDate = $(instance.element).parent().parent().find('.startDate');
        var $endDate = $(instance.element).parent().parent().find('.endDate');

        if (close_flag == false) {
            if (dateObj[0]) {
                $startDate.val(formatDate(dateObj[0], 'YYYY/MM/DD'));
            }
            if (dateObj[1]) {
                $endDate.val(formatDate(dateObj[1], 'YYYY/MM/DD'));
            } else {
                $endDate.val('');
            }
        }
    },
    onOpen: function (dateObj, dateStr, instance) {
        var $startDate = $(instance.element).parent().parent().find('.startDate');
        var $endDate = $(instance.element).parent().parent().find('.endDate');

        if ((instance.input.className.indexOf('startDate') > 0 && $('.multiMonth').eq(1).hasClass('open')) || (instance.input.className.indexOf('endDate') > 0 && $('.multiMonth').eq(0).hasClass('open'))) {
            ok_flag = true;
            obj.close();
        }
        obj = instance;
        ok_flag = false;
        if (init_open_flag == false) {
            if ($startDate.val() && $endDate.val()) {
                instance.setDate([$startDate.val(), $endDate.val()], true);
            }
        }
        if (init_open_flag === false) {
            init_open_flag = true;
            prevStartVal = $startDate.val();
            prevEndVal = $endDate.val();
        }
    },
    onReady: function (dateObj, dateStr, instance) {
        var $startDate = $(instance.element).parent().parent().find('.startDate');
        var $endDate = $(instance.element).parent().parent().find('.endDate');

        $startDate.prop('readonly', false);
        $endDate.prop('readonly', false);
        var $cal = $(instance.calendarContainer);
        if ($cal.find('.flatpickr-clear').length < 1) {
            $cal.append('<div class="flatpickr-btn flatpickr-clear">Clear</div>');
            $cal.find('.flatpickr-clear').on('click', function () {
                instance.clear();
                $startDate.val('');
                $endDate.val('');
            });
        }
        if ($cal.find('.flatpickr-month1').length < 1) {
            $cal.append('<div class="flatpickr-btn flatpickr-month1">今月</div>');
            $cal.find('.flatpickr-month1').on('click', function () {
                var date = new Date();
                var startDay = new Date(date.getFullYear(), date.getMonth(), 1);
                startDay = instance.formatDate(startDay, "Y/m/d");
                var endDay = new Date(date.getFullYear(), date.getMonth() + 1, 0)
                endDay = instance.formatDate(endDay, "Y/m/d");
                instance.setDate([startDay, endDay], true);
            });
        }
        if ($cal.find('.flatpickr-week').length < 1) {
            $cal.append('<div class="flatpickr-btn flatpickr-week">今週</div>');
            $cal.find('.flatpickr-week').on('click', function () {
                var date = new Date();
                iDate = date.getDate() - date.getDay();
                var startDay = new Date(date.getFullYear(), date.getMonth(), iDate);
                startDay = instance.formatDate(startDay, "Y/m/d");
                iDate = date.getDate() + 6 - date.getDay();
                var endDay = new Date(date.getFullYear(), date.getMonth(), iDate);
                instance.setDate([startDay, endDay], true);
            });
        }
        if ($cal.find('.flatpickr-today').length < 1) {
            $cal.append('<div class="flatpickr-btn flatpickr-today">今日</div>');
            $cal.find('.flatpickr-today').on('click', function () {
                instance.setDate(['today', 'today'], true);
            });
        }
        if ($cal.find('.flatpickr-ok').length < 1) {
            $cal.append('<div class="flatpickr-btn flatpickr-ok">OK</div>');
            $cal.find('.flatpickr-ok').on('click', function () {
                ok_flag = true;
                instance.close();
            });
        }
    }
};

var flatpickr = $(".startDate,.endDate").flatpickr(flatpickr_opts);

function Remove(e) {
    $("#" + e.id).val("");
    $("#" + e.id).flatpickr(single_flatpickr_opts);
    $("#" + e.id).attr('readonly', true);
}
