$(function () {
  new Vue({
    el: "#app",
    data: function () {
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
        list: null,
        defines: null,
        selectedRows: [],
        showModal: false,
        editData: null,
        showSelectorModal: false,
        products: [],
        validateModel: {
          name: { valid: true, msg: "" },
        },
        types: [],
      };
    },
    mounted() {
      this.getData();
    },
    methods: {
      add() {
        this.editData = {
          id: Kooboo.Guid.NewGuid(),
          name: "",
          type: "Manual",
          rule: {
            type: "All",
            conditions: [],
          },
          products: [],
          enable: true,
        };

        this.showModal = true;
      },
      edit(id) {
        Kooboo.Category.get({ id: id }).then((rsp) => {
          if (!rsp.success) return;
          this.editData = rsp.model;
          this.showModal = true;
        });
      },
      productSelected(rows) {
        for (const i of rows) {
          if (!this.editData.products.find((f) => f == i.key)) {
            this.editData.products.push(i.key);
          }
        }
      },
      save() {
        if (!this.valid()) return;

        Kooboo.Category.post(this.editData).then((rsp) => {
          if (!rsp.success) return;
          this.getData();
          this.showModal = false;
        });
      },
      getData() {
        $.when(
          Kooboo.Category.getList(),
          Kooboo.MatchRule.categoryDefines(),
          Kooboo.Product.keyValue(),
          Kooboo.ProductType.keyValue()
        ).then((categories, define, keyValue, productType) => {
          this.list = categories[0].model;
          this.defines = define[0].model;
          this.products = keyValue[0].model;
          this.types = productType[0].model;
        });
      },
      deletes() {
        var confirmStr = Kooboo.text.confirm.deleteItems;
        if (!confirm(confirmStr)) return;

        Kooboo.Category.Delete(this.selectedRows.map((m) => m.id)).then(
          (rsp) => {
            if (!rsp.success) return;
            this.getData();
          }
        );
      },
      getRules(rule) {
        return rule.conditions
          .map(
            (m) =>
              ` [${this.propertyName(m.left)} ${
                Kooboo.text.commerce.comparers[m.comparer]
              } ${this.getTypeName(m.right)}] `
          )
          .join(Kooboo.text.commerce.logical[rule.type]);
      },
      valid() {
        var valid = true;

        this.validateModel.name = Kooboo.validField(this.editData.name, [
          { required: Kooboo.text.validation.required },
          {
            min: 2,
            max: 30,
            message:
              Kooboo.text.validation.minLength +
              2 +
              ", " +
              Kooboo.text.validation.maxLength +
              30,
          },
        ]);

        if (!this.validateModel.name.valid) valid = false;
        return valid;
      },
      getTypeName(id) {
        var type = this.types.find((f) => f.key == id);
        return type ? type.value : id;
      },
      propertyName(str) {
        var text = Kooboo.text.commerce.properties[str];
        return text ? text : str;
      },
      getProductTitle(id) {
        var product = this.products.find((f) => f.key == id);
        return product ? product.value : id;
      },
    },
    computed: {
      selectorProducts() {
        var selectedProducts = this.editData ? this.editData.products : [];
        var result = [];

        for (const product of this.products) {
          if (!selectedProducts.find((f) => f == product.key)) {
            result.push(product);
          }
        }

        return result;
      },
    },
    watch: {
      editData: {
        handler() {
          for (const key in this.validateModel) {
            this.validateModel[key] = { valid: true, msg: "" };
          }
        },
        deep: true,
      },
    },
  });
});
