/**
 * 仕入先検索モーダル
 * _VendorSearchModal.cshtmlに必要
 */
var vendorSearchModal = (function () {
    var current_scrollY;
    var id = '#vendorSearchModalDialog';

    return {
        getSelectedVendor: function () {
            const selectedVendor = $('#vendorSearchResult input[type=radio]:checked').parents('tr');

            if (!selectedVendor.length) {
                return ['', ''];
            }

            const findText = (index) => selectedVendor.find('td:eq(' + index + ')').text().trim();
            const vendorId = findText(4);
            const vendorName = findText(5);

            return [vendorId, vendorName];
        },
        /**
         * モーダルダイアログの初期設定
         */
        defaultSetting: function () {
            var w = $(window).width();
            var h = $(window).height();
            $(id).dialog({
                autoOpen: false,
                width: w / 2,
                position: { my: 'center top+30', at: 'center top', of: window },
                height: h - 50,
                modal: true,
                dialogClass: 'modalct',
                close: function () {
                    $('.wrap').attr({ style: '' });
                    $('html, body').prop({ scrollTop: current_scrollY });
                }
            });
        },
        adjust: function () {
            var w = $(window).width();
            var h = $(window).height();
            $('.modalct').css('width', w / 2);
            $('.modalct .tb_scroll').css('height', h - 350);
            $(id).dialog("option", "position", { my: 'center top+30', at: 'center top', of: window });
        },
        resize: function () {
            var timer = false;
            if (timer !== false) {
                clearTimeout(timer);
            }
            timer = setTimeout(function () { vendorSearchModal.adjust(); }, 200);
        },
        /**
         * ウィンドウを閉じる(キャンセル)
         */
        close: function () {
            $('.wrap').attr({ style: '' });
            $('html, body').prop({ scrollTop: current_scrollY });
            $(id).dialog("close");
            document.activeElement.blur(); /* フォーカス解除 IE11 */
        },
        /**
         * ウィンドウの外をクリックしたときにウィンドウを閉じる
         */
        setCloseEvent: function () {
            //Attach cancel button.
            $(id).find("button.btn_close").on("click", function () {
                $(id).dialog("close");
            });

            //Attach outside of dialog area.
            $(document).on("click", ".ui-widget-overlay", function () {
                $(".ui-dialog-content").dialog("close");
            });
        },
        /**
         * 選択内容を仮保存する(確定ボタンが押されなかったら破棄する)
         */
        saveTemp: function () {
            var index = $("input[type='checkbox'][name$=IsCheck]").index(this);
            var id = $("input[type='hidden'][name$='Vendor.CenterId']").eq(index).val();

            if ($(this).prop("checked")) {
                //hiddenを作る
                $("<input>", {
                    type: "hidden",
                    name: "checkedVendors",
                    value: id
                }).appendTo("#checked");
            } else {
                $("#checked input[type='hidden']").filter("[value='" + id + "']").remove();
            }
        },
        /**
         * 選択内容を保持する(確定)
         */
        saveChanges: function () {
            $("#selectedVendor").empty();
            $("input[type='hidden'][name='checkedVendors']").clone().appendTo("#selectedVendor");
        },
        /**
         * モーダルダイアログを開く
         */
        open: function () {
            current_scrollY = $(window).scrollTop();
            $('.wrap').css({
                position: 'fixed',
                width: '100%',
                top: -1 * current_scrollY
            });

            //画面側の選択項目をコピー
            $("#checked").empty();
            $("#selectedVendor input[type='hidden'][name='checkedVendors']").clone().appendTo("#checked");

            //全てのチェックをOFF
            $("input[type='checkbox'][name$='IsCheck']").prop("checked", false);

            //選択項目でチェックをON
            //チェック済み(#checked)のhiddenを取得
            var checked = $("#checked input[type='hidden']");
            checked.each(function () {
                //各行hiddenから同じvalueを持つものを取得
                var target = $("input[type='hidden'][name$='Vendor.CenterId']").filter("[value='" + $(this).val() + "']");
                //それの兄弟要素のチェックボックスのチェックをオンにする
                $(target).siblings("input[type='checkbox']").prop("checked", true);
            });

            $(id).dialog("open");
        },
        /**
         * 初期化
         */
        init: function () {
            this.defaultSetting();
            this.adjust();
            this.setCloseEvent();
            $(window).on('resize', this.resize);
            $('#vendorClose').on('click', this.close);
            $('#vendorSelect').on('click', this.close);
            $("#vendorSelect").on("click", this.saveChanges);
            $("input[type='checkbox'][name$=IsCheck]").on("change", this.saveTemp);
        }
    };
})();
