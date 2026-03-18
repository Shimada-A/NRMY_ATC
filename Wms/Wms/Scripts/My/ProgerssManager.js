var ProgerssManager = {
    result: "",
    percent: 0,
    timer: null,
    timer2: null,

    show: function () {
        //表示設定
        ProgerssManager.setWaitingPanel();
        //進捗率の取得
        ProgerssManager.getProgress();
        //プログレスバー進捗
        ProgerssManager.procAccept();
    },

    setWaitingPanel: function () {
        var obj = document.getElementById("waitDialog");

        if (obj == null) {
            return;
        }

        // 背景用DIVタグを生成
        var d = document.createElement("div");

        d.id = "div_mask";
        d.style.position = "absolute";
        d.style.zIndex = 1000;
        d.style.top = "0px";
        d.style.left = "0px";
        d.style.width = "100%";
        d.style.height = "100%";
        d.style.textAlign = "center";
        d.style.verticalAlign = "middle";
        d.style.backgroundColor = "#000000";
        d.style.filter = "Alpha(opacity=40)";

        //背景用DIVタグを追加
        document.getElementsByTagName("form")[0].appendChild(d);

        //Wait画面の設定    
        obj.style.position = "absolute";
        obj.style.zIndex = 1001;
        obj.style.backgroundColor = "#ffffff";
        obj.style.top = ((d.clientHeight - 120) / 2 - 50) + "px";
        obj.style.left = ((d.clientWidth - 650) / 2) + "px";
        obj.style.display = "block";

        //ファンクションキーを無効化
        window.document.onkeydown = null;
    },

    getProgress: function () {
        WebApp.ServiceBridge.fncGetProgress($get("hid_wk_id").value
                                   , function (res) {
                                       if (res != null && res != "undefined") {
                                           ProgerssManager.afterGetProg(res);
                                       } else {
                                           ProgerssManager.setInnerText($get("span_msg"), "");
                                       }
                                   }
                                   , function (ext) {
                                       ProgerssManager.setInnerText($get("span_msg"), ext.get_message());
                                   }
        );

        ProgerssManager.timer = setTimeout("ProgerssManager.getProgress()", Number($get("hid_interval").value));
    },

    afterGetProg: function (res) {
        if (res[0] != null) {
            ProgerssManager.setInnerText($get("span_msg"), res[1]);

            if (res[2] != "1") {
                clearTimeout(ProgerssManager.timer);
                clearTimeout(ProgerssManager.timer2);

                if (res[2] != "9") {
                    //正常終了
                    $get("img_icon").src = "../Image/Icon/i_info.gif";
                    ProgerssManager.percent = 100;

                    ProgerssManager.setWidth(Number($get("out_prog").style.width.replace("px", "")));

                } else {
                    //異常終了
                    $get("img_icon").src = "../Image/Icon/i_error.gif";
                    $get("in_prog").className = "in_progress_err";

                    ProgerssManager.percent = 100;
                    ProgerssManager.setWidth(Number($get("out_prog").style.width.replace("px", "")));

                    ProgerssManager.setInnerText($get("in_prog"), "Error!!");
                }

                $get("btn_close").style.display = "inline";
                $get("btn_close").focus();

                ProgerssManager.result = res[2];

                return;

            } else {
                //処理中
                if (ProgerssManager.percent != res[0]) {
                    var outwidth = Number($get("out_prog").style.width.replace("px", ""));
                    var border = Number($get("out_prog").style.borderWidth.replace("px", ""));
                    var per = Number(ProgerssManager.percent);

                    ProgerssManager.setWidth(outwidth * (per / 100) - (border * 2));
                }

                ProgerssManager.percent = res[0];
            }
        }
    },

    procAccept: function () {
        var outwidth = Number($get("out_prog").style.width.replace("px", ""));
        var inwidth = Number($get("in_prog").style.width.replace("px", ""));
        var border = Number($get("out_prog").style.borderWidth.replace("px", ""));
        var per = Number(ProgerssManager.percent);

        if (outwidth * (per / 100) - (border * 2) > inwidth) {
            ProgerssManager.setWidth(inwidth + 1);
        }

        ProgerssManager.timer2 = setTimeout("ProgerssManager.procAccept()", 150);
    },

    setWidth: function (val) {
        var indiv = $get("in_prog");
        var outwidth = Number($get("out_prog").style.width.replace("px", ""));

        indiv.style.width = val + "px";
        ProgerssManager.setInnerText(indiv, Math.ceil(val / outwidth * 100) + "%");
    },

    setInnerText: function (elm, str) {
        if (typeof elm.textContent != "undefined") {
            elm.textContent = str;
        } else {
            elm.innerText = str;
        }
    },

    close: function (btnid) {
        var wait = $get("waitDialog");
        wait.parentNode.removeChild(wait);

        var mask = $get("div_mask");
        mask.parentNode.removeChild(mask);

        //ファンクションキーを有効化
        window.document.onkeydown = KeyEvent;

        //ボタンクリック
        if (ProgerssManager.result != "9") {
            //正常終了時
            if (btnid != null && btnid.length > 0) {
                document.getElementById(btnid).click();
            }
        }
    }
};
