//设置提交按钮
var setSubmit = function (btn, msg) {
    if (typeof (btn) == "String") {
        btn = $(btn);
    }
    if (!msg) {
        msg = "正在提交...";
    }
    if (btn.is("[type=submit]")) {
        btn.attr("type", "button");
        btn.attr("data-text", btn.text());
        btn.text(msg);
    } else {
        btn.attr("type", "submit");
        btn.text(btn.attr("data-text"));
    }
};

var layerloadsuccess = function (layero) {
    $(".layui-layer-btn0", layero).focus();
};

//ajax返回结果处理
var ajaxResult = function (result, refresh) {
    refresh = refresh || true;
    switch (result.error) {
        case 0:
            layer.msg('提交成功', { icon: 1 }, function (index) {
                if (refresh)
                    //refreshContent();//刷新加载
                    location.reload();
                layer.close(index);
            });

            break;
        case 9000:
        case 9001:
        case 9002:
            layer.alert(result.message, {
                success: function (layero, index) {
                    layerloadsuccess(layero);
                }
            }, function () {
                location.href = "/admin/login/";
            });
            break;
        default:
            layer.alert("提交失败！", {
                icon: 5, success: function (layero, index) {
                    layerloadsuccess(layero);
                }
            });
            break;

    }
};

//出自http://www.cnblogs.com/ahjesus 尊重作者辛苦劳动成果,转载请注明出处,谢谢!
var jsonDateFormat = function (jsonDate, format) {//json日期格式转换为正常格式
    try {
        var date = new Date(parseInt(jsonDate.replace("/Date(", "").replace(")/", ""), 10));
        var o = {
            "M+": date.getMonth() + 1, //month 
            "d+": date.getDate(), //day 
            "h+": date.getHours(), //hour 
            "m+": date.getMinutes(), //minute 
            "s+": date.getSeconds(), //second 
            "q+": Math.floor((date.getMonth() + 3) / 3), //quarter 
            "S": date.getMilliseconds() //millisecond 
        }

        if (/(y+)/.test(format)) {
            format = format.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
        }

        for (var k in o) {
            if (new RegExp("(" + k + ")").test(format)) {
                format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
            }
        }
        return format;

    } catch (ex) {
        return "";
    }
};

//获取随机数
var getRandom = function (n) {
    var chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    var res = "";
    for (var i = 0; i < n ; i++) {

        var index = Math.ceil(Math.random() * (chars.length - 1));
        res += chars[index];
    }
    return res;
}


//编辑器
var editor = function (textareaid, options) {
    UE.delEditor(textareaid);
    return UE.getEditor(textareaid, $.extend({
        //这里可以选择自己需要的工具按钮名称,此处仅选择如下七个
        //focus时自动清空初始化时的内容
        autoClearinitialContent: true,
        //默认的编辑区域高度
        toolbars: [[
           'fullscreen', 'source', '|', 'undo', 'redo', '|',
           'bold', 'italic', 'underline', 'strikethrough', '|', 'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify', '|', 'removeformat', 'formatmatch', 'pasteplain', '|',
           'forecolor', 'backcolor', 'horizontal', '|',
           'paragraph', 'fontfamily', 'fontsize', '|',
           'link', 'unlink', 'anchor', '|',
           'simpleupload', 'insertimage', 'emotion', 'insertvideo', 'attachment', 'scrawl', 'map', '|'

        ]]
        //更多其他参数，请参考umeditor.config.js中的配置项
    }, options)); //重新实例化编辑器
};

//验证表单
var formValidate = function (form, options) {
    $(form).validate(
            $.extend({
                ignore: ":hidden",
                errorElement: 'label', //default input error message container
                errorClass: 'help-block', // default input error message class
                focusInvalid: true, // do not focus the last invalid input
                highlight: function (element) { // hightlight error inputs
                    $(element)
                        .closest('.form-group').addClass('has-error'); // set error class to the control group
                },

                success: function (label) {
                    label.closest('.form-group').removeClass('has-error').addClass('has-success');
                    label.remove();

                },
                errorPlacement: function (error, element) {
                    if (element.parent().is(".input-group"))
                        error.insertAfter(element.parent());
                    else
                        error.insertAfter(element);
                }
            }, options));
};



//初始化表格
var initBootstrapTable = function (table, options) {

    var bootstraptab = typeof (table) == "string" ? $(table) : table;
    bootstraptab.bootstrapTable($.extend({
        dataType: "json",
        search: true,
        singleSelect: true,
        pagination: true, //分页
        queryParamsType: "pagesize",
        queryParams: function (params) {
            var total = bootstraptab.attr("data-totalRows") || 0;
            if (bootstraptab.bootstrapTable("getOptions").search) {
                var key = bootstraptab.bootstrapTable("getOptions").searchText;
                var dataKey = bootstraptab.attr("data-key");
                if (key != dataKey) {
                    bootstraptab.attr("data-key", key);
                    total = 0;
                }
            }
            if (!total) {
                total = 0;
            }
            var parms = {
                pagesize: params.pageSize,
                pagenumber: params.pageNumber,
                totalcount: total
            };
            if (key) {
                parms.key = key;
            }
            return parms;
        },
        pageNumber: 1,
        pageSize: 10,
        trimOnSearch: false,
        sidePagination: "server", //服务端请求
        clickToSelect: true,
        locale: "zh-CN", //表格汉化
        onLoadSuccess: function (data) {
            bootstraptab.attr("data-totalRows", bootstraptab.bootstrapTable("getOptions").totalRows);
        }

    }, options));

    return bootstraptab;
};