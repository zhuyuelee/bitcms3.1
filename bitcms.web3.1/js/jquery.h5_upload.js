+function ($) {
    $.fn.h5_upload = function (options) {
        var that = $(this);
        that.option = $.extend({
            uploadUrl: "/tools/upload/", //上传的地址
            folder: "images",
            preview: null,
            imgclass: null,
            watermark: 1,
            cut: 1,
            maxwidth: undefined,
            maxheight: undefined,
            change: null,
            success: null,
            error: null,
        }, options);

        var $size = 0;
        if (that.option.preview && typeof (that.option.preview) == "string" && $(that.option.preview).is("img")) {
            that.removeAttr("multiple");
        }
        that.change(function () {
            var $exts = ["image/png", "image/jpg", "image/gif", "image/jpeg"];
            $(this.files).each(function (i, uploadfile) {
                var index = getRandom(8);
                var $file = {
                    filetype: uploadfile.type,
                    filename: uploadfile.name
                };
                if (jQuery.inArray($file.filetype, $exts) < 0) {
                    if (typeof (that.option.error) == "function") {
                        that.option.error({ error: 1, message: "图片格式错误" });
                    }
                    else {
                        throw new Error("图片格式错误");
                    }
                    return false;
                }
                if (typeof (that.option.change) == "function") {
                    if (that.option.change(uploadfile) == false) {
                        return true;//继续下一个
                    }
                }
                var reader = new FileReader();
                reader.onload = function (e) {
                    that.compress(this.result, $file, index);
                };
                reader.readAsDataURL(uploadfile);
            });
            $(this).val("");
        });

        that.compress = function (res, file, index) {
            var img = new Image();
            img.onload = function () {
                var uploaddata = "";
                if (file.filetype != "image/gif") {//gif不压缩
                    var newSize = that.compressSize(img.width, img.height);
                    var cvs = document.createElement('canvas'),
                        ctx = cvs.getContext('2d');
                    cvs.width = newSize.width;
                    cvs.height = newSize.height;

                    ctx.clearRect(0, 0, cvs.width, cvs.height);
                    ctx.drawImage(img, newSize.x, newSize.y, img.width - 2 * newSize.x, img.height - 2 * newSize.y, 0, 0, newSize.width, newSize.height);
                    uploaddata = cvs.toDataURL(file.filetype, 0.92);
                }
                else {
                    uploaddata = img.src;
                }
                if (that.option.preview) {
                    if (typeof (that.option.preview) == "function") {
                        that.option.preview(uploaddata, index);
                    } else {
                        if ($(that.option.preview).is("img")) {
                            $(that.option.preview).attr("src", uploaddata);
                        } else {
                            var imgage = "<img src='" + uploaddata + "'";
                            if (that.option.imgclass) {
                                imgage += " class='" + that.option.imgclass + "'";
                            }
                            imgage += " data-index='" + index + "' />";
                            $(that.option.preview).append(imgage);
                        }
                    }
                }
                that.upload(uploaddata, file, index);
            };
            img.src = res;
        };
        that.compressSize = function (width, height) {
            var $newWidth, $newHeight;
            var $x = 0; var $y = 0;
            //需要压缩
            if (that.option.maxheight > 0 && that.option.maxwidth > 0) {
                if (height > that.option.maxheight || width > that.option.maxwidth) {
                    if (that.option.cut == 1) {//裁切
                        var aspectRatio = width / height;
                        var newAspectRatio = that.option.maxwidth / that.option.maxheight;

                        $newWidth = that.option.maxwidth;
                        $newHeight = that.option.maxheight;
                        if (aspectRatio > newAspectRatio) {//以高为基准
                            $x = parseInt(Math.floor((width - height * newAspectRatio) / 2));

                        } else if (aspectRatio < newAspectRatio) {//以宽为基准
                            $y = parseInt(Math.floor((height - width / newAspectRatio) / 2));
                        }
                    } else {
                        $newWidth = width;
                        $newHeight = height;
                        if (width > that.option.maxwidth) {
                            $newHeight = parseInt(height * that.option.maxwidth / width);
                            $newWidth = that.option.maxwidth;

                            if ($newHeight > that.option.maxheight) {
                                $newWidth = parseInt(width * that.option.maxheight / height);
                                $newHeight = that.option.maxheight;
                            }

                        } else if (height > that.option.maxheight) {
                            $newWidth = parseInt(width * that.option.maxheight / height);
                            $newHeight = that.option.maxheight;

                            if ($newWidth > that.option.maxwidth) {
                                $newHeight = parseInt(height * that.option.maxwidth / width);
                                $newWidth = that.option.maxwidth;
                            }
                        }
                    }
                }
                else {
                    $newWidth = width;
                    $newHeight = height;
                }
            } else {//限制一边
                $newWidth = width;
                $newHeight = height;

                if (that.option.maxheight > 0 && height > that.option.maxheight) {
                    $newWidth = parseInt(width * that.option.maxheight / height);
                    $newHeight = that.option.maxheight;
                } else if (that.option.maxwidth > 0 && width > that.option.maxwidth) {
                    $newHeight = parseInt(height * that.option.maxwidth / width);
                    $newWidth = that.option.maxwidth;
                }
            }
            return { width: $newWidth, height: $newHeight, x: $x, y: $y };
        };
        that.upload = function (image_data, file, index) {
            data = image_data.split(',')[1];
            data = window.atob(data);
            var ia = new Uint8Array(data.length);
            for (var i = 0; i < data.length; i++) {
                ia[i] = data.charCodeAt(i);
            };
            var blob = new Blob([ia], {
                type: file.filetype
            });
            var formData = new FormData();
            formData.append('image_data', blob, file.filename);
            formData.append('folder', that.option.folder);
            formData.append('watermark', that.option.watermark);

            $.ajax({
                url: that.option.uploadUrl,
                type: 'POST',
                contentType: false,
                processData: false,
                data: formData,
                dataType: 'json',
                success: function (res) {
                    if (that.option.preview && typeof (that.option.preview) == "string") {
                        if ($(that.option.preview).is("img")) {
                            $(that.option.preview).attr("src", res.data);
                        } else {
                            $("img[data-index=" + index + "]", that.option.preview).attr("src", res.data);
                        }
                    }
                    if (typeof (that.option.success) == "function") {
                        that.option.success(res, file, index);
                    }
                }, error: function (res) {
                    if (typeof (that.option.error) == "function") {
                        that.option.error(res, index);
                    }
                }
            });
        }
    };
}(window.jQuery);