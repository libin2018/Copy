var exsshelper = Vue.directive('exss', {
    inserted: function (el, binding) {
        var o = $.extend({ opr: "=", type: 0, format: "" }, binding.value);
        var ext = o.key + o.opr + "[strval]";
        switch (o.format.toLowerCase()) {
            case "number":
                ext = o.key + o.opr + "[val]";
                break;
            case "date":
                ext = o.key + o.opr + "Convert.ToDateTime(\"[val]\")";
                break;
            case "like":
                ext = o.key + ".Contains([strval])";
                break;
            case "maxdate":
                ext = o.key + o.opr + "Convert.ToDateTime(\"[val] 23:59:59\")";
                break;
        }
        $(el).attr("exss", ext);
    },
    build_exss: function () {
        var exss_tmp = $("[exss]");
        var tmps = [];
        var vals = [];
        for (var i = 0; i < exss_tmp.length; i++) {
            var o = exss_tmp[i];
            var exss = $(o).attr("exss");
            var val = $(o).val();
            if (val && val != null) {
                if ($.isArray(val)) {
                    var ar = [];
                    $(val).each(function () {
                        if (this != "") {
                            ar.push(exss.replace("[val]", this));
                        }
                    })
                    if (ar.length > 0) {
                        tmps.push("(" + ar.join("||") + ")");
                    }
                }
                else if (val.trim() != "") {
                    if (exss.indexOf(["strval"]) > 0) {
                        var s = exss.replace("[strval]", "@" + vals.length);
                        vals.push(val);
                        tmps.push(s);
                    }
                    else {
                        var s = exss.replace("[val]", val);
                        tmps.push(s);
                    }                
                }
            }
        }
        return { Ques: tmps, Vals: vals };
    }
});