/*********textarea操作***************/
var textarea = function (textareaid) {
    var area = $("#" + textareaid);
    area.setContent = function (val) {
        area.val(val);
    };
    area.setContent = function (val) {
        area.val(val);
    };
    area.getContent = function () {
       return area.val();
    };
    return area;
};