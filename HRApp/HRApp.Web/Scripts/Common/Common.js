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