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
        defines: {},
        selectedRows: [],
        showModal: false,
        editData: null,
        showSelectorModal: false,
        products: [],
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
          type: this.defines.types[0].name,
          rules: [],
          products: [],
        };

        this.showModal = true;
      },
      addRule() {
        this.editData.rules.push({
          id: Kooboo.Guid.NewGuid(),
          property: this.defines.properties[0].name,
          comparer: this.defines.comparers[0].name,
          value: "0",
        });
      },
      edit(id) {
        Kooboo.Category.get({ id: id }).then((rsp) => {
          if (!rsp.success) return;
          this.editData = rsp.model;
          this.showModal = true;
        });
      },
      productSelected(rows) {
        this.editData.products.push(...rows.map((m) => m.key));
      },
      save() {
        Kooboo.Category.post(this.editData).then((rsp) => {
          if (!rsp.success) return;
          this.getData();
          this.showModal = false;
        });
      },
      getData() {
        $.when(
          Kooboo.Category.getList(),
          Kooboo.Category.define(),
          Kooboo.Product.keyValue()
        ).then((categories, define, keyValue) => {
          this.list = categories[0].model;
          this.defines = define[0].model;
          this.products = keyValue[0].model;
        });
      },
      deletes() {
        Kooboo.Category.Delete(this.selectedRows.map((m) => m.id)).then(
          (rsp) => {
            if (!rsp.success) return;
            this.getData();
          }
        );
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
      selectedProducts() {
        var selectedProducts = this.editData ? this.editData.products : [];
        var result = [];

        for (const product of this.products) {
          if (selectedProducts.find((f) => f == product.key)) {
            result.push(product);
          }
        }

        return result;
      },
    },
  });
});
