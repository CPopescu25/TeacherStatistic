function CreateNode() {
    var ref = $('#jstree_demo').jstree(true),
   sel = "#";
    sel = ref.create_node('#', { "type": "default" });
    if (sel) {
        ref.creareDomenii(sel);
    }
}
function demo_create() {
    var ref = $('#jstree_demo').jstree(true),
     sel = ref.get_selected();
    if (!sel.length) { return false; }
    sel = sel[0];
    sel = ref.create_node(sel, { "type": "default" });
    if (sel) {
        ref.creareDomenii(sel);
    }
}