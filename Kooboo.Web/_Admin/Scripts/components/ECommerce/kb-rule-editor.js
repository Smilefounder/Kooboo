(function () {
  Kooboo.loadJS([
    "/_Admin/Scripts/kooboo/Guid.js",
    "/_Admin/Scripts/components/kbForm.js",
  ]);

  Vue.component("kb-rule-editor", {
    template: Kooboo.getTemplate(
      "/_Admin/Scripts/components/ECommerce/kb-rule-editor.html"
    ),
    props: ["rule", "defines"],
    data() {
      return {
        id: Kooboo.Guid.NewGuid(),
        keyValues: [],
      };
    },
    created() {
      if (!this.rule.type) this.rule.type = this.defines[0].name;
    },
    mounted() {
      for (const condition of this.rule.conditions) {
        this.propertyChanged(condition);
      }
    },
    methods: {
      addCondition() {
        var condition = {
          id: Kooboo.Guid.NewGuid(),
          left: this.defines[0].name,
          comparer: this.defines[0].comparers[0],
          right: "",
        };

        this.rule.conditions.push(condition);
        this.propertyChanged(condition);
      },
      currentDefine(value) {
        var define = this.defines.find((f) => f.name == value);
        return define;
      },
      propertyChanged(item) {
        var define = this.currentDefine(item.left);

        if (!define.comparers.find((f) => f == item.comparer)) {
          item.comparer = define.comparers[0];
          item.value = "";
        }

        switch (define.valueType) {
          case "ProductTypeId":
            Kooboo.ProductType.keyValue().then((rsp) => {
              this.keyValues = rsp.model;
            });
            break;
          case "ProductId":
            Kooboo.Product.keyValue().then((rsp) => {
              this.keyValues = rsp.model;
            });
            break;
          case "CategoryId":
            Kooboo.Category.keyValue().then((rsp) => {
              this.keyValues = rsp.model;
            });
            break;

          default:
            break;
        }

        item.value = "";
      },
      propertyName(str) {
        var text = Kooboo.text.commerce.properties[str];
        return text ? text : str;
      },
      comparerDisplay(comparer) {
        var text = Kooboo.text.commerce.comparers[comparer];
        return text ? text : comparer;
      },
    },
  });
})();
