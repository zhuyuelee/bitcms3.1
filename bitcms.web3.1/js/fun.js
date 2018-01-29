jQuery.getQueryString = function (name) { var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i"); var r = window.location.search.substr(1).match(reg); if (r != null) return unescape(r[2]); return null; }
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
var ajaxResult = function (result, href) {
    switch (result.error) {
        case 0:
            layer.msg(result.message || '提交成功', { icon: 1 }, function (index) {
                if (href)
                    location.href = href;
                layer.close(index);
            });

            break;
        case 9000:
        case 9001:
        case 9002:
            layer.alert(result.message, function () {
                location.href = "/login?url=" + encodeURIComponent(location.href);
            });
            break;
        default:
            layer.alert(result.message || "提交失败！", {
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
    if (typeof (UE) == "object") {
        UE.delEditor(textareaid);
        return UE.getEditor(textareaid, $.extend({
            //这里可以选择自己需要的工具按钮名称,此处仅选择如下七个
            //focus时自动清空初始化时的内容
            autoClearinitialContent: false,
            //默认的编辑区域高度
            toolbars: [[
               'fullscreen',
               'bold', 'italic', 'underline', 'strikethrough', '|', 'removeformat',
               'forecolor', 'backcolor', 'horizontal', '|',
               'paragraph', 'fontfamily', 'fontsize', '|',
               'simpleupload', '|'
            ]]
            //更多其他参数，请参考umeditor.config.js中的配置项
        }, options)); //重新实例化编辑器
    } else {
        return textarea(textareaid);
    }
};

//验证表单
var formValidate = function (form, options) {
    return $(form).validate(
            $.extend({
                ignore: ":hidden",
                errorElement: 'label',
                errorClass: 'help-block',
                focusInvalid: true,
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

+ function ($) {
    //点赞
    jQuery.fn.setAgree = function (options) {
        var box = $(this);
        var defaults = { detailid: "data-did", reviewid: "data-rid", agree: ".agree", against: ".against", lab: "em" };
        options = $.extend(defaults, options);

        var dids = "", rids = "";
        box.each(function (i) {
            var agreebox = $(this);
            if (agreebox.is("[" + options.detailid + "]")) {
                if (dids.length > 0)
                    dids += ",";
                dids += agreebox.attr(options.detailid);
            }
            if (agreebox.is("[" + options.reviewid + "]")) {
                if (rids.length > 0)
                    rids += ",";
                rids += agreebox.attr(options.reviewid);
            }
        });

        if (rids.length > 0 || dids.length > 0) {
            $.post("/cmsaction/getviewpointlist", { dids: dids, rids: rids }, function (result) {
                if (result.error == 0) {
                    if (result.data && result.data.length > 0) {
                        $(result.data).each(function (i, n) {
                            box.each(function (j) {
                                if (n.ReviewId > 0 && $(this).is("[" + options.reviewid + "=" + n.ReviewId + "]")) {
                                    $(this).addClass("agreed");
                                    return false;
                                }
                                if (n.DetailId > 0 && $(this).is("[" + options.detailid + "=" + n.DetailId + "]")) {
                                    $(this).addClass("agreed");
                                    return false;
                                }
                            });
                        });
                    }
                    $(options.agree + "," + options.against, box).on("click", function () {
                        var agree = $(this);
                        var agreebox = $(this).closest(box.selector);
                        
                        $.post("/cmsaction/setviewpoint", { detailid: agreebox.attr(options.detailid), reviewid: agreebox.attr(options.reviewid), agree: agree.is(options.agree) ? 1 : -1, against: agree.is(options.against) ? 1 : -1 }, function (viewpointresult) {
                            if (viewpointresult.error == 0) {
                                layer.msg('点赞成功', { icon: 1 }, function () {
                                    $(options.lab, agree).text(viewpointresult.data.num);
                                });
                                if (agreebox.is(".agreed")) {
                                    agreebox.removeClass("agreed");
                                } else {
                                    agreebox.addClass("agreed");
                                }
                            } else if (viewpointresult.error >= 9000 && viewpointresult.error <= 9002) {
                                layer.confirm('您还没有登录，请登录后再操作', { icon: 3, title: '登录提示' },
                                    function () {
                                        location.href = "/login?url=" + encodeURIComponent(location.href);
                                    });
                            } else {
                                ajaxResult(result);
                            }
                        });
                    });
                } else if (result.error >= 9000 && result.error <= 9002) {
                    $(options.agree + "," + options.against, box).click(function () {
                        layer.confirm('您还没有登录，请登录后再操作', { icon: 3, title: '登录提示' },
                            function () {
                                location.href = "/login?url=" + encodeURIComponent(location.href);
                            });
                    });
                };
            });
        }
    };
    //关注
    $.fn.setFollow = function (option) {
        var box = $(this);
        if (option.userid && option.userid > 0) {
            var defaults = {};
            option = $.extend(defaults, option);
            $.post("/cmsaction/checkuserfollow", { userid: option.userid, followuserid: option.followuserid }, function (result) {
                if (result.error == 0) {
                    if (result.data) {
                        box.text("已关注");
                        box.removeClass("follow");
                    } else {
                        box.on("click", function () {
                            $.post("/cmsaction/addfollow", option, function (turn) {
                                if (turn.error == 0) {
                                    box.off("click");
                                    box.text("已关注");
                                    box.removeClass("follow");
                                    layer.msg('关注成功', { icon: 1 });
                                } else {
                                    layer.alert("提交失败！", { icon: 5 });
                                }
                            });
                        });
                    }
                } else if (result.error == 9000) {
                    box.on("click", function () {
                        layer.alert('您还没有登录，请登录后再操作', function () {
                            location.href = "/login?url=" + encodeURIComponent(option.link);
                        });
                    });
                }
            });
        } else {
            box.on("click", function () {
                layer.alert('您还没有登录，请登录后再操作', function () {
                    location.href = "/login?url=" + encodeURIComponent(option.link);
                });
            });
        }
    }
}(window.jQuery);