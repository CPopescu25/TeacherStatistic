function demo_rename() {
    var ref = $('#jstree_demo').jstree(true),
     sel = ref.get_selected();
    if (!sel.length) { return false; }
    sel = sel[0];
    ref.editDomenii(sel);
};