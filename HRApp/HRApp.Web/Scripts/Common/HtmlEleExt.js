$(function () {
    $('input').each(function (i,ele) {//增加带删除的标签
        inputAddClear(ele);
    });
});
function inputAddClear(ele) {
    //判断该元素是否增加了标签
    var silbing = $(ele).next('.clearTag');
    if (silbing.length == 0) {
        $(ele).after('<label class="clearTag">x</label>');
        $(ele).next('.clearTag').click(function () {
            var target = $(this).prev('input');//查找上一个兄弟节点
            target.val('');//清空数据
        })
    }
}
function GetXPathObject(rootEle, apiKeyAttr) {
    var obj = {};
    if (!rootEle) {
        return obj;
    }
    if (!apiKeyAttr) {
        apiKeyAttr = "apiKey";
    }
    var data = [];
    $.each($(rootEle).find('input,textarea,select'), function (i, ele) {
        var objEle = $(ele);
        //这里不能访问到具局部变量data
    });
    var paramEles = $(rootEle).find('input,textarea,select');
    for (var i = 0; i < paramEles.length; i++) {
        var objEle = $(paramEles[i]);
        if (objEle.attr(apiKeyAttr) == undefined)
        {//这不是目标元素
            continue;
        }
        obj[objEle.attr(apiKeyAttr)] = objEle.val();
    }
    return obj;
}
function ValidEleIsRequired(rootEle, isReuqiredTag,notAddWarmingEle) {
    var valid = false;
    if ($.trim(isReuqiredTag) == '') {
        isReuqiredTag = ".isReuqired";
    }
    $(rootEle).find(isReuqiredTag).each(function (i, ele) {
        //判断必填项是否存在非空项
        if ($.trim($(ele).val()) != '') {//过滤首尾空格项
            valid = valid && true;
            return true;
        }
        valid =valid&& false;
        if (notAddWarmingEle == true) {//不绘制渲染警告元素
            return true;
        }
        //判断是否增加了显示提示框
        var warmEle=$(ele).next('.validWarming');
        if (warmEle.length == 0) {//兄弟节点中没有增加警告元素
            $(ele).parent().append('<label class="validWarming">！</label>');
            warmEle = $(ele).next('.validWarming');
        }
        warmEle.show();
        return true;
    });
    return valid;
}
function AjaxWarming(warmEle, json) {

}