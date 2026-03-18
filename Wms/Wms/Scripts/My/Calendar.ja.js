(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports) :
        typeof define === 'function' && define.amd ? define(['exports'], factory) :
            (global = global || self, factory(global.ja = {}));
}(this, function (exports) {
    'use strict';

    var fp = typeof window !== "undefined" && window.flatpickr !== undefined
        ? window.flatpickr
        : {
            l10ns: {}
        };
    var Japanese = {
        weekdays: {
            shorthand: ["SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT"],
            longhand: ["日曜日", "月曜日", "火曜日", "水曜日", "木曜日", "金曜日", "土曜日"],
        },
        months: {
            shorthand: [
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9",
                "10",
                "11",
                "12",
            ],
            longhand: [
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9",
                "10",
                "11",
                "12",
            ],
        }
    };
    fp.l10ns.ja = Japanese;
    var ja = fp.l10ns;

    exports.Japanese = Japanese;
    exports.default = ja;

    Object.defineProperty(exports, '__esModule', { value: true });

}));