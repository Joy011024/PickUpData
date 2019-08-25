//调用前，请先引用 lang.js
function bindPostAjax(url, param, succFun, errorFun, complateFun) {
	var isFullUrl=(param!=null&&param.isFullUrl);
    var fullUrl =isFullUrl?url: "http://" + window.location.host + url;
    var lay = parent.layer ? parent.layer : layer;
    lay.load(1, { type: 3 });
    //layer.load(1, {type:3,text:'Handing。。。。。',title:'tip'}); 
    $.ajax({
        url: fullUrl,
        data: param,
        type: 'post',
        traditional:true,//支持传递数组
        success: function (response, statue) {
            var lay = parent.layer ? parent.layer : layer;
            lay.close(lay.index);
            if (succFun == undefined) {
                return;
            }
            succFun(response, statue);
        },
        error: function (response, statue) {
            var lay = parent.layer ? parent.layer : layer;
            if (errorFun == undefined) {
                if (lay == undefined) {
                    return;
                }
                lay.alert('Error', {
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
function CloseChrome() {//关闭浏览器
    var browserName = navigator.appName;
    console.warm("browser type:")
    console.log(browserName);
    if (browserName == "Netscape") {
        window.open('', '_self', '');
        window.close();
    } else {
        window.close();
    }
}
$(function () {
    $.fn.extend({
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
            return arr != null ? unescape(arr[2]) : undefined;
        },
        SetCookie: function (key, value) {
            var Days = 30;
            var exp = new Date();
            exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
            document.cookie = key + "=" + escape(value) + ";expires=" + exp.toGMTString();
        },
        WriteFile: function (value, type, name) {
            if ($.trim(type) == '')
            {
                type = "text/latex";
            }
            if ($.trim(name) == '') {
                name = (~new Date()) + '.log';
            }
            var blob;
            if (typeof window.Blob == "function") {
                blob = new Blob([value], { type: type });
            } else {
                var BlobBuilder = window.BlobBuilder || window.MozBlobBuilder || window.WebKitBlobBuilder || window.MSBlobBuilder;
                var bb = new BlobBuilder();
                bb.append(value);
                blob = bb.getBlob(type);
            }
            var URL = window.URL || window.webkitURL;
            var bloburl = URL.createObjectURL(blob);
            var anchor = document.createElement("a");
            if ('download' in anchor) {
                anchor.style.visibility = "hidden";
                anchor.href = bloburl;
                anchor.download = name;
                document.body.appendChild(anchor);
                var evt = document.createEvent("MouseEvents");
                evt.initEvent("click", true, true);
                anchor.dispatchEvent(evt);
                document.body.removeChild(anchor);
            } else if (navigator.msSaveBlob) {
                navigator.msSaveBlob(blob, name);
            } else {
                location.href = bloburl;
            }
        }
    });
});