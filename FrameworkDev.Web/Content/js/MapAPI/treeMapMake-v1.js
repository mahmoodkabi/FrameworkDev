function _makeTree(options, parentName) {
    var children, e, id, o, pid, temp, _i, _len, _ref, res;
    id = options.id || "ArcServiceID";
    pid = options.parentid || "ArcServiceParentID";
    children = options.children || "children";
    temp = {};
    o = [];
    _ref = options.q;
    for (_i = 0, _len = _ref.length; _i < _len; _i++) {
        e = _ref[_i];
        e[children] = [];
        temp[e[id]] = e;
        if (temp[e[pid]] != null) {
            temp[e[pid]][children].push(e);
        } else {
            o.push(e);
        }
    }
    res = _renderTree(o, "FirstCall");


    document.getElementById(parentName).innerHTML = res;


    var imported = document.createElement('script');
    imported.src = webServerURL + '/Content/cdn/Scripts/MapAPI/treeFunc-v1.js';
    document.head.appendChild(imported);
};


function _renderTree(tree, call) {
    var e, html, _i, _len;
    if (call == "FirstCall")
        html = "<ul " + " class=" + "treeview" + " id=" + "treeviewService" + " >";
    else
        html = "<ul>"
    for (_i = 0, _len = tree.length; _i < _len; _i++) {
        e = tree[_i];
        html += " <li class='parent'><input type='checkbox' id=Service" + e.ArcServiceID + " title=" + e.ServiceName + " value=" + e.ServiceAddress + "/>" + "<font color=" + "white >" + "<label class=" + "custom-unchecked > " + "  " + e.ServiceName + "  " + "  </label> </font> </checkbox> ";
        html += " <input type=hidden id=Service" + e.ArcServiceID + " value=" + e.ServiceAddress + "/>"
        if (e.children != null) {
            html += _renderTree(e.children, "NotFirstCall");
        }
        html += "</li>";
    }
    html += "</ul>";
    return html;
};

