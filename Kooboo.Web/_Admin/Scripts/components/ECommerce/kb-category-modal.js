(function () {
  Kooboo.loadJS([
    "/_Admin/Scripts/kooboo/Guid.js",
    "/_Admin/Scripts/lib/jstree.min.js",
  ]);
  Kooboo.loadCSS(["/_Admin/Styles/jstree/style.min.css"]);

  Vue.component("kb-category-modal", {
    template: Kooboo.getTemplate(
      "/_Admin/Scripts/components/ECommerce/kb-category-modal.html"
    ),
    props: ["value", "selected"],
    data: function () {
      return {
        jsTree: null,
        currentNode: null,
      };
    },
    mounted: function () {
      this.currentNode = null;
    },
    computed: {
      show: {
        get: function () {
          return this.value;
        },
        set: function (value) {
          this.$emit("input", value);
        },
      },
    },
    methods: {
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
      modelToTreeData(model) {
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
      start: function () {
        this.show = false;
        this.$emit("start", this.currentNode);
      },
    },
    watch: {
      value: function (value) {
        if (!value) return;
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
              me.currentNode = data.selected[0];
            })
            .data("jstree");
        });
      },
    },
  });
})();
