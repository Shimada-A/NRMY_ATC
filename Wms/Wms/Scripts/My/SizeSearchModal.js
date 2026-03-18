/**
 * ブランド検索モーダル
 * _SizeSearchModal.cshtmlに必要
 */
var sizeSearchModal = (function () {
    var current_scrollY;
    var id = '#sizeSearchModalDialog';

    return {
        /**
         * モーダルダイアログの初期設定
         */
        defaultSetting: function () {
            var w = $(window).width();
            var h = $(window).height();
            $(id).dialog({
                autoOpen: false,
                width: w/2,
                height: h-50,
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
        },
        resize: function () {
            var timer = false;
            if (timer !== false) {
                clearTimeout(timer);
            }
            timer = setTimeout(function () { sizeSearchModal.adjust(); }, 200);
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
                $(this).prev().find(".ui-dialog-content").dialog("close");
            });
        },
        /**
         * 選択内容を仮保存する(確定ボタンが押されなかったら破棄する)
         */
        saveTemp: function () {
            var index = $("input[type='checkbox'][name$=IsCheck]").index(this);
            var id = $("input[type='hidden'][name$='Location.CenterId']").eq(index).val();

            if ($(this).prop("checked")) {
                //hiddenを作る
                $("<input>", {
                    type: "hidden",
                    name: "checkedSize",
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
            $("#selectedSize").empty();
            $("input[type='hidden'][name='checkedSize']").clone().appendTo("#selectedSize");
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
            $("#selectedSize input[type='hidden'][name='checkedSize']").clone().appendTo("#checked");

            //全てのチェックをOFF
            $("input[type='radio'][name$='IsCheck']").prop("checked", false);

            //選択項目でチェックをON
            //チェック済み(#checked)のhiddenを取得
            var checked = $("#checked input[type='hidden']");
            checked.each(function () {
                //各行hiddenから同じvalueを持つものを取得
                var target = $("input[type='hidden'][name$='Size.ItemSizeId']").filter("[value='" + $(this).val() + "']");
                //それの兄弟要素のチェックボックスのチェックをオンにする
                $(target).siblings("input[type='radio']").prop("checked", true);
            });

            $(id).dialog("open");
        },
        /**
         * フッターのチェックボックスカウンターの設定
         */
        setCheckBoxCounter: function () {
            var checkBoxes = $("input[name$='IsCheck']");
            //checkBoxCounter.init(checkBoxes, null);
            $("#btnSizeSearchFreeWord").on("click", this.SizeSearchChange);
        },
        /**
         * 再検索
         */
        SizeSearchChange: function () {
            $("#frmSizeSearch").submit();
        },
        /**
         * 初期化
         */
        init: function () {
            this.defaultSetting();
            this.adjust();
            this.setCheckBoxCounter();
            this.setCloseEvent();
            $(window).on('resize', this.resize);
            $('#sizeClose').on('click', this.close);
            $('#sizeSelect').on('click', this.close);
            $("#sizeSelect").on("click", this.saveChanges);
            $("input[type='checkbox'][name$=IsCheck]").on("change", this.saveTemp);
        }
    };
})();
