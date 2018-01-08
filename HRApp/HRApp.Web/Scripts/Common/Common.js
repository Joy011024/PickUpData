//调用前，请先引用 lang.js
function bindPostAjax(url, param, succFun, errorFun, complateFun) {
    var fullUrl = "http://" + window.location.host + url;
    $.ajax({
        url: fullUrl,
        data: param,
        type: 'post',
        traditional:true,//支持传递数组
        success: function (response, statue) {
            if (succFun == undefined) {
                return;
            }
            succFun(response, statue);
        },
        error: function (response, statue) {
            if (errorFun == undefined) {
                if (layer == undefined) {
                    return;
                }
                layer.alert('Error', {
                    title: lang.tip.error
                });
                return;
            }
            errorFun(response, statue);
        },
        complete: function (response, statue) {
            if (complateFun == undefined) {
                return;
            }
            complateFun(response, statue);
        }
    });
}
function bindGetAjax(url, succFun, errorFun, complateFun) {
    var fullUrl = "http://" + window.location.host + url;
    $.ajax({
        url: fullUrl,
        
        type: 'get',
        success: function (response, statue) {
            if (succFun == undefined) {
                return;
            }
            succFun(response, statue);
        },
        error: function (response, statue) {
            if (errorFun == undefined) {
                if (layer == undefined) {
                    return;
                }
                layer.alert('Error', {
                    title: lang.tip.error
                });
                return;
            }
            errorFun(response, statue);
        },
        complete: function (response, statue) {
            if (complateFun == undefined) {
                return;
            }
            complateFun(response, statue);
        }
    });
}
function GenerateGuid16String(succFun) {
    bindPostAjax('/SysManage/GenerateGuid16String',null, function (response, statue) {
        if (succFun)
            succFun(response, statue);
    });
}
function ClearCookie() {

}
$.fn.exttend({
    ClearAllCookie: function () {
        var keys = document.cookie.match(/[^ =;]+(?=\=)/g);
        if (keys == null || keys.length == 0) {
            return false;
        }
        $.each(keys, function (i, ele) {
            var exp = new Date();
            exp.setTime(exp.getTime() - 1);
            var cval = $.fn.GetCookie(ele);
            if (cval != null)
                document.cookie = ele + "=" + cval + ";expires=" + exp.toGMTString();
        });
        return true;
    },
    GetCookie: function (key) {
        var reg = new RegExp("(^| )" + key + "=([^;]*)(;|$)");
        var arr = document.cookie.match(reg);
        return arr!=null? unescape(arr[2]):undefined;
    },
    SetCookie: function (key, value) {
        var Days = 30;
        var exp = new Date();
        exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
        document.cookie = key + "=" + escape(value) + ";expires=" + exp.toGMTString();
    }
});