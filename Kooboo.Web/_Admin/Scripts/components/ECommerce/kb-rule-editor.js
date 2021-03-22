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
        keyValues: [],
      };
    },
    created() {
      if (!this.rule.type) this.rule.type = this.defines.matchingType[0].name;
    },
    mounted() {
      for (const condition of this.rule.conditions) {
        this.propertyChanged(condition)
      }
    },
    computed: {},
    methods: {
      addCondition() {
        var condition = {
          id: Kooboo.Guid.NewGuid(),
          left: this.defines.conditionDefines[0].name,
          comparer: this.defines.conditionDefines[0].comparers[0].name,
          right: "",
        };

        this.rule.conditions.push(condition);
        this.propertyChanged(condition);
      },
      currentDefine(value) {
        var define = this.defines.conditionDefines.find((f) => f.name == value);
        return define;
      },
      propertyChanged(item) {
        var define = this.currentDefine(item.left);

        switch (define.valueType) {
          case "ProductTypeId":
            Kooboo.ProductType.keyValue().then((rsp) => {
              this.keyValues = rsp.model;
            });
            break;

          default:
            break;
        }

        item.value="";
      },
    },
  });
})();
