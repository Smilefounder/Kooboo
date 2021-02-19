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
        currentNode: null,
        jsTree: null,
        showEditModal: false,
        editData: null,
      };
    },
    mounted: function () {
      var me = this;
      Kooboo.ProductCategory.getList().then(function (rsp) {
        if (!rsp.success) return;
        me.jsTree = $("#categories")
          .jstree({
            plugins: ["types"],
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
                callback.call(node, me.getTreeData(rsp.model));
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
    methods: {
      getSelectedNode: function () {
        var rel = this.jsTree.get_selected();
        return this.jsTree.get_node(rel[0]);
      },
      getParentNames: function (node, includeSelf) {
        var me = this;
        var names = [];
        if (includeSelf) names.push(node.text);

        node.parents.forEach(function (f) {
          names.push(me.jsTree.get_node(f).text);
        });

        return names.reverse();
      },
      onAddNewCategory: function () {
        this.editData = {
          parent: null,
          isEdit: false,
          id: Kooboo.Guid.NewGuid(),
          text: "",
          attributes: [],
          specifications: [],
        };
        this.showEditModal = true;
      },
      onAddNewSubCategory: function () {
        var node = this.getSelectedNode();
        var parentNames = this.getParentNames(node, true);
        this.editData = {
          parent: node.id,
          parentName: parentNames.join(" / "),
          isEdit: false,
          id: Kooboo.Guid.NewGuid(),
          text: "",
          attributes: [],
          specifications: [],
        };

        this.showEditModal = true;
      },
      removeCategory: function () {
        var me = this;
        var node = me.getSelectedNode();

        if (node.children.length > 0) {
          if (!confirm(Kooboo.text.confirm.deleteItems)) return;
        }

        Kooboo.ProductCategory.Delete([node.id]).then(function (res) {
          if (res.success) {
            me.jsTree.delete_node(node);
          }
        });

        var rel = this.jsTree.get_selected();
        this.jsTree.delete_node(rel);
        this.currentNode = null;
      },
      onEdit: function () {
        var node = this.getSelectedNode();

        var model = {
          parent: null,
          parentName: parentNames,
          isEdit: true,
          id: node.id,
          text: node.text,
          attributes: node.data.attributes,
          specifications: node.data.specifications,
        };

        if (node.parent != "#") {
          model.parent = node.parent;
          var parentNames = this.getParentNames(node);
          model.parentName = parentNames.join(" / ");
        }

        this.editData = JSON.parse(JSON.stringify(model));
        this.showEditModal = true;
      },
      saveCategory: function () {
        var me = this;

        var attributes = me.standardizationId(me.editData.attributes);
        var specifications = me.standardizationId(me.editData.specifications);

        var model = {
          parent: me.editData.parent,
          id: me.editData.id,
          name: me.editData.text,
          attributes: JSON.stringify(attributes),
          specifications: JSON.stringify(specifications),
        };

        Kooboo.ProductCategory.post(model).then(function (res) {
          if (res.success) {
            window.info.show(Kooboo.text.info.save.success, true);

            if (me.editData.isEdit) {
              var node = me.getSelectedNode();
              me.jsTree.rename_node(node, me.editData.text);
              node.data = {
                attributes: me.editData.attributes,
                specifications: me.editData.specifications,
              };
            } else {
              me.jsTree.create_node(
                me.editData.parent,
                me.modelToTreeData(model)
              );
            }

            me.showEditModal = false;
          }
        });
      },
      getTreeData: function (model) {
        var me = this;
        function getChildren(parent) {
          var children = model.filter(function (f) {
            return f.parent == parent;
          });

          return children.map(function (m) {
            var result = me.modelToTreeData(m);
            result.children = getChildren(m.id);
            return result;
          });
        }

        return getChildren(null);
      },
      addItem: function (items) {
        items.push({
          id: Kooboo.Guid.NewGuid(),
          name: "",
          type: 0,
          editingItem: "",
          options: [],
        });
      },
      removeItem: function (items, item) {
        var index = items.indexOf(item);
        items.splice(index, 1);
      },
      addOption: function (item) {
        if (!item.editingItem) return;

        item.options.push({
          id: Kooboo.Guid.NewGuid(),
          value: item.editingItem,
        });

        item.editingItem = "";
      },
      modelToTreeData: function (model) {
        return {
          id: model.id,
          text: model.name,
          state: { opened: true },
          children: [],
          data: {
            attributes: model.attributes ? JSON.parse(model.attributes) : [],
            specifications: model.specifications
              ? JSON.parse(model.specifications)
              : [],
          },
        };
      },
      standardizationId: function (items) {
        var me = this;
        var coped = JSON.parse(JSON.stringify(items));

        coped.forEach(function (f) {
          f.id = Kooboo.Guid.computeGuid(me.editData.id + f.name);
          if (f.options) {
            f.options.forEach(function (o) {
              o.id = Kooboo.Guid.computeGuid(f.id + o.value);
            });
          }
        });

        return coped;
      },
    },
  });
});
