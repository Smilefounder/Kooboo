$(function () {
  new Vue({
    el: "#app",
    data: function () {
      self = this;
      return {
        breads: [
          {
            name: "SITES",
          },
          {
            name: "DASHBOARD",
          },
          {
            name: Kooboo.text.common.ProductCategories,
          },
        ],
        model: [],
        currentNode: null,
        jsTree: null,
      };
    },
    mounted: function () {
      var me = this;
      Kooboo.ProductCategory.getList().then(function (rsp) {
        if (!rsp.success) return;
        me.model = rsp.model;
        me.jsTree = $("#categories")
          .jstree({
            plugins: ["dnd", "types"],
            types: {
              default: {
                icon: "fa fa-tag icon-color-dark",
              },
            },
            core: {
              strings: {
                "Loading ...": Kooboo.text.common.loading + " ...",
              },
              theme: { name: "proton", responsive: true },
              check_callback: true,
              multiple: false,
              data: function (node, callback) {
                callback.call(node, me.treeData);
              },
            },
          })
          .on("select_node.jstree", function (e, data) {
            self.currentNode = data.selected[0];
          })
          .on("create_node.jstree", function (e, data) {
            me.jsTree.deselect_all();
            me.jsTree.select_node(data.node);
          })
          .data("jstree");
      });
    },
    computed: {
      treeData: function () {
        var me = this;
        function getChildren(parent) {
          var children = me.model.filter(function (f) {
            return f.parent == parent;
          });

          return children.map(function (m) {
            return {
              id: m.id,
              text: m.name,
              state: { opened: true },
              children: getChildren(m.id),
            };
          });
        }

        return getChildren(null);
      },
    },
    methods: {
      getNode: function () {
        return {
          id: Kooboo.Guid.NewGuid(),
          text: Kooboo.text.common.name,
          children: [],
        };
      },
      getSaveData: function () {
        var result = [];

        function addItem(data, parent) {
          data.forEach(function (m) {
            result.push({
              id: m.id,
              name: m.text,
              parent: parent,
            });

            if (m.children) addItem(m.children, m.id);
          });
        }

        addItem(this.jsTree.get_json(), null);
        return result;
      },
      onAddNewCategory: function () {
        var newNode = this.jsTree.create_node(null, this.getNode());
        this.jsTree.edit(newNode);
      },
      onAddNewSubCategory: function () {
        var rel = this.jsTree.get_selected();
        var subNode = this.jsTree.create_node(rel[0], this.getNode());
        this.jsTree.edit(subNode);
      },
      removeCategory: function () {
        var rel = this.jsTree.get_selected();
        this.jsTree.delete_node(rel);
        this.currentNode = null;
      },
      deleteById: function (id) {
        Kooboo.ProductCategory.Delete({
          id: id,
        }).then(function (res) {
          if (res.success) {
            jsTree.delete_node(self.currentNode.node);
          }
        });
      },
      onEdit: function () {
        var rel = this.jsTree.get_selected();
        this.jsTree.edit(rel[0]);
      },
      saveCategory: function () {
        var data = this.getSaveData();
        Kooboo.ProductCategory.post(data).then(
          function (res) {
            if (res.success) {
              window.info.show(Kooboo.text.info.save.success, true);
            }
          }
        );
      },
    },
  });
});
