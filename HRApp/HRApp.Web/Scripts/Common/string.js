String.prototype.format = function () {
    var args = arguments;
    return this.replace(/\{\d+\}/g, function (m) {
        var dt = args[m.substring(1, m.length - 1)];
        return (dt == null || dt == undefined) ? "" : dt;
    });
};
String.prototype.format.regex = new RegExp("{-?[0-9]+}", "g");

String.prototype.toDate = function () {
    return new Date(Date.parse(this.replace(/-/g, "/")));
};
String.prototype.replaceAll = function (s1, s2) {
    return this.replace(new RegExp(s1, "gm"), s2);
};
if (!String.prototype.startsWith) {
    String.prototype.startsWith = function (searchString, position) {
        position = position || 0;
        return this.lastIndexOf(searchString, position) === position;
    };
}
/*
Tools
*/
var ris_utils = (function () {
    this.htmlEncode = function (str) {
        if (str == null || str === undefined || str === "") return "";
        if (!/[&<> '"]/g.test(str)) return str;

        return str.replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/ /g, "&nbsp;")
            .replace(/\'/g, "&#39;")
            .replace(/\"/g, "&quot;")
        ;
    };

    this.htmlDecode = function (str) {
        if (str == null || str === undefined || str === "") return "";

        return str.replace(/&amp;/g, "&")
            .replace(/&lt;/g, "<")
            .replace(/&gt;/g, ">")
            .replace(/\u00a0/g, " ") //&nbsp;
            .replace(/&#39;/g, "\'")
            .replace(/&quot;/g, "\"")
        ;
    };

    function logPlugIn() {
        function myConsole() {
            var domNodeId = "#sys_log";
            var logDom = $(domNodeId);
            if (logDom.size() == 0) {
                logDom = $("<div id=\"" + domNodeId + "\"></div>");
                logDom.css({
                    "position": "absolute",
                    "right": "0",
                    "top": "0",
                    "width": "300px",
                    "height": "400px",
                    "border": "solid 1px #666"
                });
                logDom.appendTo(document.body);
            }
            return {
                log: function (msg) {
                    var htm = logDom.html();
                    if (htm == "") {
                        logDom.html(msg);
                    } else {
                        logDom.html(htm + "<br/>" + msg);
                    }
                },
                error: function (msg) {
                    logDom.html(logDom.html() + "<br/><span style=\"color:#00FF00;\">" + msg + "</span>");
                }
            };
        };

        return window.console || myConsole();
    };

    //export ligerGrid to excel
    this.exportLigerGrid = function (worklist, ignoreCols) {
        var rows = [];
        var headerCells = [];
        $(worklist.columns).each(function () {
            if ($.trim(this.columnname).length == 0 || (ignoreCols && $.trim(this.columnname).length > 0 && ignoreCols[this.columnname]))
                return true;

            if (!this.issystem || (this.issystem && this.isrownumber))
                headerCells.push("<th>{0}</th>".format($.trim(this.display)));
        });
        rows.push(headerCells.join("\r\n"));
        $(worklist.data.Rows).each(function () {
            var that = this;
            var cells = [];
            $(worklist.columns).each(function () {
                if ($.trim(this.columnname).length == 0 || (ignoreCols && $.trim(this.columnname).length > 0 && ignoreCols[this.columnname]))
                    return true;

                if (!this.issystem || (this.issystem && this.isrownumber)) {
                    cells.push("<td>{0}&nbsp;</td>".format(worklist._getCellHtml(that, this).replace(/<[^>]*>|<\/[^>]*>/gm, "")));
                }
            });
            rows.push("<tr>{0}</tr>".format(cells.join("\r\n")));
        });

        exportTableToExcel("<table>{0}</table>".format(rows.join("\r\n")));
    };

    this.exportTableToExcel = function (tableHtml) {
        var url = "/handlers/ExportHelper.ashx";
        if (tableHtml == null || tableHtml == undefined || tableHtml.length == 0) {
            alert("无导出内容。");
            return;
        }
        if (typeof (tableHtml) == "object") tableHtml = $(tableHtml).prop("outerHTML");
        tableHtml = tableHtml.replace(/</gm, "[");
        $.post(url, { exportType: "table", content: tableHtml }, function (key) {
            if (key) location.href = url + "?key=" + key;
        });
    };

    var defaultLogger = {
        log: function () { },
        error: function () { }
    };

    this.logger = /chrome/ig.test(navigator.userAgent) ? logPlugIn() : defaultLogger;

    this.getPropertyNames = function (obj, excludeFunc) {
        var props = [];
        if (typeof (obj) != "object") return props;
        for (var s in obj) {
            if (obj.hasOwnProperty(s)) {
                if (excludeFunc && typeof (obj[s]) == "function") {
                    //add nothing
                } else {
                    props.push(s);
                }
            }
        }
        return props;
    };

    this.property2Array = function (obj) {
        var rslt = [];
        for (var s in obj) {
            if (obj.hasOwnProperty(s)) rslt.push(obj[s]);
        }
        return rslt;
    };

    /*data:[{Text:"",Value:"",Keyword:"",xxx}] xxx is additional*/
    this.generateOptHtml = function (data, includeAdditional) {
        var keyWordKey = "Keyword";
        if (!data || !$.isArray(data) || data.length === 0) return "";
        var tmpl = "<option value=\"{0}\" keywords=\"{2}\">{1}</option>";
        var tmpl2 = "<option value=\"{0}\" keywords=\"{3}\" {1}>{2}</option>";
        var output = [];
        var attrs = [];

        if (includeAdditional) {
            var props = getPropertyNames(data[0]);
            $(props).each(function (i, item) {
                if (item !== "Text" && item !== "Value") {
                    attrs.push(item);
                }
            });
        }

        $(data).each(function (i, item) {
            if (!includeAdditional && attrs.length == 0) {
                output.push(tmpl.format(
                    item.Value,
                    htmlEncode(item.Text, item.hasOwnProperty(keyWordKey) ? item[keyWordKey] : "")
                ));
            } else {
                var attr2Add = [];
                $(attrs).each(function (i, attrName) {
                    attr2Add.push("data-{0}=\"{1}\"".format(attrName, htmlEncode(item[attrName])));
                });

                output.push(
                    tmpl2.format(
                        item.Value,
                        attr2Add.join(" "),
                        htmlEncode(item.Text), item.hasOwnProperty(keyWordKey) ? item[keyWordKey] : ""
                    )
                );
            }
        });
        return output.join("\r\n");
    };

    this.bindOptHtml = function (ctrl, data, incAddi, hasEmpty) {
        if ($.isArray(data)) {
            if (hasEmpty) {
                data.unshift({ Text: "", Value: "" });
            }
            $(ctrl).html(generateOptHtml(data, incAddi));
        }
    };

    this.enableLogger = function (bEnabled) {
        this.logger = bEnabled ? logPlugIn() : defaultLogger;
    };

    var lang = "";
    try { var lang = SERVERLANGUAGE; } catch (e) { lang = ""; }
    this.getStaffFullName = (lang === "zh-CN") ? function (staff) {
        return staff.LastName + staff.FirstName;
    } : function (staff) {
        return "{0} {1}{2}".format(staff.FirstName, staff.MiddleName && staff.MiddleName != "" ? staff.MiddleName + " " : "", staff.LastName);
    };

    //replace \r\n to newline in web
    this.convert2WebEnter = function (text) {
        if (text == null || text == undefined) return "";
        return /\<[a-z0-9/]+\>/ig.test(text) ? text : text.replace(/\n/ig, "<div/>");
    };
    /*
    jquery's clone and html func dosen't copy the value of field to html.
    */
    this.getOutHtmlWithFormValue = function (wrapper) {
        if (wrapper == null || wrapper == undefined) return "";
        var node = ris_utils.cloneNodeWithFormData($(wrapper));
        return node != null ? node.html() : "";
    };

    this.cloneNodeWithFormData = function (wrapper) {
        if (wrapper == null || wrapper == undefined) return null;
        var wrapper = $(wrapper);
        if (wrapper.size() > 0) {
            var cloned = $(wrapper).clone();

            $(":checkbox", cloned).each(function (i, item) {
                var ele = wrapper.find(":checkbox").eq(i);
                $(item).removeAttr("checked");
                if (ele.prop("checked")) {
                    $(item).attr("checked", "checked");
                }
            });

            var radioNames = {};
            $(":radio[name]", cloned).each(function () { radioNames[$(this).attr("name")] = true; });

            $(ris_utils.getPropertyNames(radioNames)).each(function (i, item) {
                var val = $(":radio[name='" + item + "'][checked='checked']", wrapper).val();
                $(":radio[name='" + item + "']", cloned).removeAttr("checked");
                $(":radio[name='" + item + "'][value=\'" + val + "\']", cloned).attr("checked", "checked");
            });

            $(":text", cloned).each(function (i, item) {
                var ele = $(":text", wrapper).eq(i);
                $(item).attr("value", $(ele).val());
            });

            $("select", cloned).each(function (i, item) {
                var ele = $("select", wrapper).eq(i);
                var val = ele.val();
                var opt = $(item).find("option[value='" + val + "']");
                if (opt.size() > 0) opt.attr("selected", "selected");
            });

            $("textarea", cloned).each(function (i, item) {
                var ele = $("textarea", wrapper).eq(i);
                var val = ele.val();
                $(item).text(val);
            });

            return cloned;
        }
    };

    /*{
        height://dialog height
        width://dialog width
    }*/
    this.getScaledSize = function (opt) {
        var winHeight = $(window).height();
        var dialogSpace = 40;
        var rate = opt.width / opt.height;
        var scale = { width: 1, height: winHeight - dialogSpace };
        scale.width = scale.height * rate;
        return (opt.height < scale.height) ? opt : scale;
    };

    this.autoResizeDialog = function (dialogOptons) {
        var scaled = getScaledSize(dialogOptons);
        dialogOptons.width = scaled.width;
        dialogOptons.height = scaled.height;
        return dialogOptons;
    };

    this.invokeFrameFunc = function (container, funcName, params) {
        if (!params) params = [];
        var iframeEle = $("iframe", container);
        if (iframeEle.size() === 0) return;
        var frameWin = iframeEle[0].contentWindow;
        if (frameWin == null || frameWin === undefined) {
            this.logger.log("frame.contentWindow is null or undefined.");
            return;
        }
        if (typeof frameWin[funcName] == 'function') {
            frameWin[funcName].apply(frameWin, params);
        } else {
            this.logger.log("func {0} is not contains in iframe.".format(funcName));
        }
    }

    this.bindAutoComplete = function (ele, data, attrs, filter) {
        if (!filter) {
            filter = function (item) { return !item.IsDelete; };
        }
        var prepareAttr = $.isArray(attrs) ? function (obj, item) {
            $(attrs).each(function (i, attr) {
                if (item.hasOwnProperty(attr)) {
                    obj[attr] = item[attr];
                }
            });
            return obj;
        } : function (obj, item) { return obj; };

        var items = $(data).map(function (i, item) {
            if (filter(item)) {
                return prepareAttr({ Id: item.Id, Name: item.Name }, item);
            }
        });

        $(ele).unautocomplete().autocomplete(items, {
            minChars: 0,
            max: 65536,
            formatItem: function (item) { return item.Name; },
            formatMatch: function (item) {
                return "{0} {1} {2}".format(item.Name,
                    (item.hasOwnProperty("MnemonicCode") ? item.MnemonicCode : ""),
                    (item.hasOwnProperty("Code") ? item.Code : "")
                );
            },
            matchContains: true
        }).result(function (e, selected) {
            var target = $(e.target);
            target.attr("data-id", selected.Id).attr("data-name", selected.Name).val(selected.Name);
        });
    }

    this.getAutoCompleteVal = function (ele) {
        var node = $(ele);
        return node.attr("data-name") === node.val() ? node.val() : "";
    };

    this.getSinglePropOfItems = function (items, prop) {
        var rslt = [];
        items = items || [];
        $(items).each(function (i, item) {
            if (item.hasOwnProperty(prop)) {
                rslt.push(item[prop]);
            }
        });
        return rslt;
    };

    this.getCurrentDept = function () {
        var curUser = storeManager.Configs.getUser();
        var deptId = null;
        if (curUser && curUser.StaffDTO && (deptId = curUser.StaffDTO.DepartmentId)) {
            return {
                DepartmentId: curUser.StaffDTO.DepartmentId,
                DepartmentName: curUser.StaffDTO.DepartmentName
            };
        }
        return null;
    };

    this.filterByCurrentDept = function (items) {
        var dept = this.getCurrentDept();
        var rslt = [];
        if (dept && items && $.isArray(items)) {
            $.each(items, function (i, item) {
                if (item.DepartmentId == dept.DepartmentId) {
                    rslt.push(item);
                }
            });
        }
        return rslt;
    }

    this.getTextFromHtml = function (html) {
        if (html == null || html == undefined) return "";
        return html.replace(/(&nbsp;|<([^>]+)>)/ig, "");
    }
    return this;
})();